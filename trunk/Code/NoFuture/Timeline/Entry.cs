using System;
using System.Linq;
using System.Text;
using NoFuture.Util.Core;

namespace NoFuture.Timeline
{
    [Serializable]
    public class Entry : IDrawing, IRuleEntry 
    {
        #region fields
        protected internal int _height;
        protected internal int _width;
        protected internal double _endValue;
        protected internal bool _endValueSetExplicitly;
        private readonly string _id;
        #endregion

        #region properties
        public virtual PrintLocation Location { get; set; }
        public virtual Rule Ruler { get; set; }
        public virtual string Text { get; set; }
        #endregion

        #region ctors
        public Entry()
        {
            _width = Config.Numerics.DefaultWidth;
            _id = Guid.NewGuid().ToString();
        }
        #endregion

        #region Object overrides
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Text))
                return string.Empty;
            if (Width == 0)
                _width = Config.Numerics.DefaultWidth;
            if (_height == 0)
                _height = CalcHeight();

            Func<int, string> sfOpL = i => "{0,-" + i + "}";
            Func<int, string> sfOpR = i => "{0," + i + "}";

            var strBuilder = new StringBuilder();
            switch (Location)
            {
                case PrintLocation.Center:
                    strBuilder.AppendLine(Etc.PrintInCenter(Width, Text));
                    break;
                case PrintLocation.Left:
                    strBuilder.AppendLine(string.Format(sfOpL(Width), Text));
                    break;
                case PrintLocation.Right:
                    strBuilder.AppendLine(string.Format(sfOpR(Width), Text.Trim()));
                    break;
            }

            if (_height <= 1)
                return strBuilder.ToStringUnixNewline();

            for (var i = 0; i < _height - 1; i++)
            {
                switch (Location)
                {
                    case PrintLocation.Center:
                        strBuilder.AppendLine(Etc.PrintInCenter(Width, Config.GraphChars.LifeLine.ToString()));
                        break;
                    case PrintLocation.Left:
                        strBuilder.AppendLine(string.Format(sfOpL(Width), Config.GraphChars.LifeLine));
                        break;
                    case PrintLocation.Right:
                        strBuilder.AppendLine(string.Format(sfOpR(Width), Config.GraphChars.LifeLine));
                        break;
                }
            }
            switch (Location)
            {
                case PrintLocation.Center:
                    strBuilder.AppendLine(Etc.PrintInCenter(Width, new string(Config.GraphChars.Rail,3)));
                    break;
                case PrintLocation.Left:
                    strBuilder.AppendLine(string.Format(sfOpL(Width), new string(Config.GraphChars.Rail, 3)));
                    break;
                case PrintLocation.Right:
                    strBuilder.AppendLine(string.Format(sfOpR(Width), new string(Config.GraphChars.Rail, 3)));
                    break;
            }
            return strBuilder.ToStringUnixNewline();
        }
        #endregion

        #region IRuleEntry methods
        public virtual double StartValue { get; set; }
        public virtual double EndValue 
        {
            get
            {
                if (!_endValueSetExplicitly)
                {
                    _endValue = StartValue;
                }
                return _endValue;
            }
            set
            {
                _endValueSetExplicitly = true;
                _endValue = value;
            }
        }
        public virtual int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        public virtual string Id => _id;

        #endregion

        #region hidden
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        protected internal int CalcHeight()
        {
            if (StartValue == EndValue)
                return 1;
            if (Ruler == null)
                return 1;
            var idx = Ruler.CalcEntryIndex(this);

            var y = 0;
            var x = Ruler.Length;

            if (idx.Item1 >= 0)
                x = idx.Item1;
            if (idx.Item2 > 0)
                y = idx.Item2;

            return y - x;
        }//end CalcHeight


        protected internal string PrintStartEndValue()
        {
            //if no range given print empty
            if (StartValue == 0 && EndValue == 0)
                return string.Empty;

            //convert to nullables
            var entrySv = StartValue == 0 ? null : new double?(StartValue);
            var entryEv = EndValue == 0 ? null : new double?(EndValue);

            //short-circuit out with when just StartValue
            if (entrySv.HasValue && entryEv.HasValue && entrySv == entryEv)
                return ((int) Math.Round(entrySv.Value)).ToString();

            var dfRuleValues = GetRulerStartEndValues();

            return TimelineExtensions.PrintYearsRange(new[] {new Tuple<double?, double?>(entrySv, entryEv) }, dfRuleValues.Item1,
                dfRuleValues.Item2);
        }

        protected internal void AssignStartEndValueByString(string printYears)
        {
            var dfRuleValues = GetRulerStartEndValues();
            var parsedValues =
                TimelineExtensions.ParseYearsRange(printYears, dfRuleValues.Item1, dfRuleValues.Item2).ToList();
            if (parsedValues.Any())
            {
                StartValue = parsedValues[0].Item1.GetValueOrDefault(0D);
                EndValue = parsedValues[0].Item2.GetValueOrDefault(0D);
            }
        }
        private Tuple<double, double> GetRulerStartEndValues()
        {
            var goBackOne = StartValue > EndValue ? 1 : -1;
            var goFowardOne = goBackOne * -1;

            var ruleSv = Ruler?.StartValue ?? StartValue + goBackOne;
            var ruleEv = Ruler?.EndValue ?? EndValue + goFowardOne;
            return new Tuple<double, double>(ruleSv, ruleEv);
        }
        #endregion
    }
}