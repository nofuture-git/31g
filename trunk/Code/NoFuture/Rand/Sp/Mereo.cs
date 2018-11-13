using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Sp.Enums;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="IMereo"/>
    /// <inheritdoc cref="VocaBase"/>
    /// <summary>
    /// Base implementation a name of any kind of money entry
    /// </summary>
    [Serializable]
    public class Mereo : VocaBase, IMereo
    {
        #region fields
        private Pecuniam _expectedValue = Pecuniam.Zero;
        private readonly List<string> _exempliGratia = new List<string>();
        private TimeSpan? _freq;

        #endregion

        #region ctors
        public Mereo()
        {
        }

        public Mereo(string name):base(name)
        {
        }

        public Mereo(string name, string groupName):base(name, groupName)
        {
        }

        public Mereo(IVoca names)
        {
            CopyFrom(names);
        }

        public Mereo(IMereo mereo) : this((IVoca)mereo)
        {
            Value = mereo.Value;
            DueFrequency = mereo.DueFrequency;
            Classification = mereo.Classification;
            foreach(var eg in mereo.GetExempliGratia())
                GetExempliGratia().Add(eg);
        }
        #endregion

        #region properties
        public Interval Interval => DueFrequency.ToInterval() ?? Interval.Annually;
        public TimeSpan? DueFrequency
        {
            get => _freq;
            set
            {
                var nextFreq = value;
                //the current value is denominated in some timespan, so adjust to match
                Value = GetValueInTimespanDenominator(nextFreq);
                _freq = nextFreq;
            }
        }

        public Classification? Classification { get; set; }

        public string Abbrev => Name;
        public string Src { get; set; }

        public Pecuniam Value
        {
            get => _expectedValue ?? (_expectedValue = Pecuniam.Zero);
            set => _expectedValue = value;
        }

        #endregion

        public Pecuniam GetValueInTimespanDenominator(TimeSpan? nextFreq)
        {
            if (_freq == null || nextFreq == null || _freq == nextFreq || Value == Pecuniam.Zero)
                return Value;
            return GetValueInTimespanDenominator(nextFreq.Value.TotalDays);
        }

        public Pecuniam GetValueInTimespanDenominator(double totalDays)
        {
            if (_freq == null || Math.Abs(totalDays) < 0.0000001 || Value == Pecuniam.Zero)
                return Value;
            var vPerDay = Convert.ToDouble(Value.Amount) / _freq.Value.TotalDays;
            return new Pecuniam(Convert.ToDecimal(totalDays * vPerDay));
        }

        public List<string> GetExempliGratia()
        {
            return _exempliGratia;
        }

        public override string ToString()
        {
            return new Tuple<string, string, Pecuniam, Interval>(Name, GetName(KindsOfNames.Group), Value,
                Interval).ToString();
        }

        public override IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => TransformText(x, txtCase);
            var itemData = new Dictionary<string, object>();

            foreach (var i in base.ToData(txtCase))
            {
                itemData.Add(i.Key, i.Value);
            }
            if(Interval != Interval.OnceOnly)
                itemData.Add(textFormat(nameof(Interval)), Interval.ToString());
            if(Classification != null)
                itemData.Add(textFormat(nameof(Classification)), Classification.ToString());
            if(!string.IsNullOrWhiteSpace(Src))
                itemData.Add(textFormat("Source"), Src);
            if(Value != Pecuniam.Zero)
                itemData.Add(textFormat("ExpectValue"), Value.ToString());

            return itemData;
        }

    }
}
