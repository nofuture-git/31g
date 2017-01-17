using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Timeline
{
    [Serializable]
    public class Rule : IRuleEntry
    {
        #region fields
        private double? _countIncrement;
        private int _width;
        private readonly string _id;
        #endregion

        #region ctors
        public Rule()
        {
            _width = Config.Numerics.RuleWidth;
            _id = "Rule";
        }
        #endregion

        #region properties
        public virtual int RuleLineSpacing { get; set; }
        public virtual double? CountIncrement {
            get
            {
                _countIncrement = Math.Floor((double)(EndValue - StartValue) / 10);
                return _countIncrement;
            }
            set { _countIncrement = value; }
        }
        public virtual int Length => GetIndexRule().Count;
        #endregion

        #region IRuleEntry methods
        public virtual double StartValue { get; set; }
        public virtual double EndValue { get; set; }
        public virtual int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        public virtual string Id => _id;

        #endregion

        #region Object overrides
        public override string ToString()
        {
            //determine if required vals are assigned
            if (StartValue == EndValue)
                throw new ArgumentException("The start at value must be greater-than or less-than the end at value.");

            //default these if not assigned
            if (RuleLineSpacing == 0)
                RuleLineSpacing = Config.Numerics.DefaultRuleLnSp;
            double increment = CountIncrement.Value;

            //construct a the ruler as a printable string 
            var strBuilder = new StringBuilder();
            double loopCounter = StartValue;

            Func<double, double, double, bool> loopOp = (a, z, i) => a > z ? i >= z : i <= z;

            while (loopOp(StartValue, EndValue, loopCounter))
            {
                strBuilder.AppendLine(string.Format("{0,-6}", loopCounter));
                for (var i = 0; i < RuleLineSpacing - 1; i++)
                {
                    strBuilder.AppendLine(string.Format("{0,-6}", " "));
                }
                loopCounter += increment;
            }

            return strBuilder.ToStringUnixNewline();
        }
        #endregion

        #region methods
        public virtual Tuple<int, int> CalcEntryIndex(IRuleEntry entry)
        {
            var ruleIndex = GetIndexRule();
            var item1 = -1;
            var item2 = -1;

            Func<double, double, double> rtOp = (d, i) => Math.Abs(d - i);
            var startDif = rtOp(ruleIndex[0], entry.StartValue);
            var endDif = rtOp(ruleIndex[0], entry.EndValue);

            for (var i = 1; i < ruleIndex.Count; i++)
            {
                if (item1 < 0)
                {
                    //there is a shorter distance between this rule index than the previous
                    if (startDif > rtOp(ruleIndex[i], entry.StartValue))
                        startDif = rtOp(ruleIndex[i], entry.StartValue);
                    else
                        item1 = i - 1;
                }

                if (item2 < 0)
                {
                    if (endDif > rtOp(ruleIndex[i], entry.EndValue))
                        endDif = rtOp(ruleIndex[i], entry.EndValue);
                    else
                        item2 = i - 1;
                }

                if (item1 > 0 && item2 > 0)
                    break;
            }
            return new Tuple<int, int>(item1, item2);
        }//end CalcEntryIndex
        public virtual List<double> GetIndexRule()
        {
            //determine edge
            var min = StartValue > EndValue ? EndValue : StartValue;

            var myRuleLines = ToString().Split(Config.GraphChars.UNIX_NL_CHAR);
            var perLineIncrement = CountIncrement.Value / RuleLineSpacing;

            var indexRule = new List<double>();
            var iVal = (double)min;
            for (var i = 0; i < myRuleLines.Length; i++)
            {
                iVal = !string.IsNullOrWhiteSpace(myRuleLines[i])
                    ? Convert.ToDouble(myRuleLines[i].Trim())
                    : iVal + perLineIncrement;

                indexRule.Add(iVal);
            }

            return indexRule;
        }//end GetIndexRule
        public virtual TextCanvas ToTextCanvas()
        {
            var tc = this.ToTextCanvas(this);
            foreach (var ti in tc.Items)
            {
                ti.Text.Add(Config.GraphChars.Bar);
            }
            return tc;
        }//end ToTextCanvas


        protected internal virtual string PrintYearsRange(IEnumerable<Tuple<double?, double?>> years)
        {
            return TimelineExtensions.PrintYearsRange(years, StartValue, EndValue);
        }

        protected internal virtual IEnumerable<Tuple<double?, double?>> ParseYearsRange(string printyears)
        {
            return TimelineExtensions.ParseYearsRange(printyears, StartValue, EndValue);
        }
        #endregion
    }
}