using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NoFuture.Timeline
{
    /// <summary>
    /// This is for for creating fast sequence diagrams where you are not really 
    /// concerned with the values on the ruler - just the ordering of the entries.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// var myFPlate = new FastPlate("Band Practice", "Left side", "Middle side", "Right side");
    /// myFPlate.Blk("Left side").Txt("big meaty claws").Blk("Right side");
    /// myFPlate.Blk("Right side").Txt("d'er not just for attractin' mates").Blk("Left side");
    /// myFPlate.Blk("Left side").Txt("yeah - bring it on").Blk("Right side").Blk(null);
    /// myFPlate.Blk("Middle side").Txt("no people,").Blk("Left side").Blk(null);
    /// myFPlate.Blk("Middle side").Txt("lets bring it off.").Blk("Right side");
    /// System.Console.WriteLine(myFPlate.ToString());
    /// ]]>
    /// </example>
    [Serializable]
    public class FastPlate : Plate
    {
        #region fields
        private Block _liveBlock;
        private int _lineCounter = 2;
        private readonly Dictionary<double, string> _blockIdxName = new Dictionary<double, string>();
        #endregion

        #region ctor
        public FastPlate(string plateTitle, int? dfWidth, params string[] blockNames):this(plateTitle, dfWidth, null, blockNames)
        {

        }

        public FastPlate(string plateTitle, int? dfWidth, Rule rule)
        {
            if (dfWidth != null)
                Config.Numerics.DefaultWidth = dfWidth.Value;
            var ruler = rule ?? new Rule { StartValue = 1, EndValue = 16, RuleLineSpacing = 3 };

            Name = string.IsNullOrWhiteSpace(plateTitle) ? "Sequence Diagram" : plateTitle;
            Ruler = ruler;
        }

        public FastPlate(string plateTitle, int? dfWidth, Rule rule, params string[] blockNames) : this(plateTitle,dfWidth,rule)
        {

            if (blockNames == null || blockNames.Length <= 0)
                throw new ArgumentNullException(nameof(blockNames));
            var c = 0;
            foreach (var blkNm in blockNames)
            {
                var blk = new Block { Ruler = Ruler, Title = blkNm };
                Blocks.Add(blk);
                _blockIdxName.Add(c, blkNm);
                c += 1;
            }
        }

        public FastPlate(string plateTitle, int? dfWidth, Rule rule, Hashtable blocks) : this(plateTitle, dfWidth, rule)
        {
            if (blocks == null)
                return;

            var c = 0.0D;
            foreach (var i in blocks.Keys)
            {
                var blkNm = i?.ToString();
                if (blkNm == null)
                    continue;
                var blk = new Block { Ruler = Ruler, Title = blkNm };
                c += 1.0D;
                c = Math.Floor(c);
                Blocks.Add(blk);
                _blockIdxName.Add(c, blkNm);

                var innerBlks = blocks[i] as IEnumerable;
                if (innerBlks == null)
                    continue;
                foreach (var j in innerBlks)
                {
                    var iBlkNm = j?.ToString();
                    if (iBlkNm == null)
                        continue;
                    var iBlk = new Block
                    {
                        Ruler = Ruler,
                        Title = iBlkNm,
                        StartValue = Ruler.StartValue + Ruler.RuleLineSpacing
                    };
                    blk.AddInnerBlock(iBlk);
                    c += 0.1D;
                    c = (double)Math.Round((decimal)c, 1);
                    _blockIdxName.Add(c, iBlkNm);
                }
            }
            _lineCounter = Ruler.RuleLineSpacing + 1;
        }

        public FastPlate(string plateTitle, int? dfWidth, Rule rule, string jsonBlockInnerBlocks) : this(plateTitle, dfWidth, rule, JsonConvert.DeserializeObject<Hashtable>(jsonBlockInnerBlocks))
        {

        }
        #endregion

        #region methods
        /// <summary>
        /// This identifies the current block. When the current block
        /// is being replaced an arrow will be written from the previous
        /// block to this block.
        /// </summary>
        /// <param name="blockName">Pass in null clear the current block.</param>
        /// <returns></returns>
        public virtual FastPlate Blk(string blockName)
        {
            if (string.IsNullOrWhiteSpace(blockName) ||
                GetBlockByName(blockName) == null)
            {
                _liveBlock = null;
                return this;
            }
            if (_liveBlock == null)
            {
                _liveBlock = GetBlockByName(blockName);
                return this;
            }

            var searchBlk = GetBlockByName(blockName);

            if (_liveBlock!= null && !searchBlk.Equals(_liveBlock))
            {
                var arrow = new Arrow(_liveBlock, searchBlk)
                {
                    StartValue = _lineCounter
                };
                var isInnerArrow = _liveBlock.IsInnerBlock
                                   && searchBlk.IsInnerBlock
                                   && _liveBlock.MyOuterBlock.Equals(searchBlk.MyOuterBlock);

                if (isInnerArrow)
                    _liveBlock.MyOuterBlock.AddArrow(arrow);
                else
                    AddArrow(arrow);

                _lineCounter += 1;
            }

            _liveBlock = searchBlk;
            return this;
        }

        public virtual FastPlate Blk(double idx)
        {
            if (_blockIdxName.ContainsKey(idx))
            {
                return Blk(_blockIdxName[idx]);
            }
            _liveBlock = null;
            return this;
        }

        public virtual FastPlate this[double index] => Blk(index);

        /// <summary>
        /// Prints a single line on to current block.
        /// </summary>
        /// <param name="entryText">
        /// This is broken into many lines whenever the text is too large.</param>
        /// <param name="pl">Lets the caller determine the <see cref="PrintLocation"/> of this line</param>
        /// <returns></returns>
        public virtual FastPlate Txt(string entryText, PrintLocation pl = PrintLocation.Center)
        {
            if (_liveBlock == null)
                return this;
            entryText = entryText ?? string.Empty;

            var entryLine = new List<string>();

            foreach (var word in entryText.Split(' '))
            {
                entryLine.Add(word);
                if (string.Join(" ", entryLine).Length + 4 <= _liveBlock.Width)
                    continue;
                _liveBlock.AddEntry(_lineCounter, string.Join(" ", entryLine),pl);
                _lineCounter += 1;
                entryLine.Clear();
            }

            _liveBlock.AddEntry(_lineCounter, string.Join(" ", entryLine), pl);
            _lineCounter += 1;

            AdjustRuler();

            return this;
        }

        public Dictionary<double, string> GetIndex2Name()
        {
            return _blockIdxName;
        }

        protected internal void AdjustRuler()
        {
            if (_lineCounter + 2 <= Ruler.EndValue) return;

            var nRule = new Rule
            {
                StartValue = Ruler.StartValue,
                EndValue = _lineCounter + 2,
                RuleLineSpacing = Ruler.RuleLineSpacing
            };
            Ruler = nRule;
            foreach (var blk in Blocks)
            {
                blk.Ruler = nRule;
            }            
        }
        #endregion

    }
}