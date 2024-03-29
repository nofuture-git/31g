using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Gov;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Domus
{
    [Serializable]
    public class Spouse : DiachronIdentifier, IRelation
    {
        #region fields
        private readonly IPerson _est;
        private readonly IPerson _me;
        private DateTime _marriedOn;
        private DateTime? _separatedOn;
        private readonly int _ordinal;
        private readonly int _totalYears;
        #endregion

        #region ctor
        public Spouse(IPerson me, IPerson so, DateTime marriageDt, DateTime? divorceDt, int ordinal)
        {
            _me = me;
            _est = so;
            _marriedOn = marriageDt;
            _separatedOn = divorceDt;
            _ordinal = ordinal;
            var edt = Est?.DeathCert?.DateOfDeath ?? _separatedOn.GetValueOrDefault(DateTime.Today);
            var rng = (edt - _marriedOn).Days;
            _totalYears = (int) Math.Round(rng/Constants.DBL_TROPICAL_YEAR);
        }
        #endregion

        #region properties
        /// <summary>
        /// Added for single-depth JSON serialization
        /// </summary>
        public string EstName => ToString();
        public IPerson Est => _est;
        public int Ordinal => _ordinal;
        public int TotalYears => _totalYears;

        /// <summary>
        /// Convenience method which is more straight-forward than <see cref="Inception"/>
        /// </summary>
        public DateTime MarriedOn => _marriedOn;

        /// <summary>
        /// Attempting to set to a null value is ignored.
        /// </summary>
        public override DateTime Inception
        {
            get => _marriedOn;
            set => _marriedOn = value;
        }

        public override DateTime? Terminus
        {
            get => _separatedOn;
            set => _separatedOn = value;
        }

        #endregion

        #region methods
        /// <summary>
        /// Determines if this is the spouse of the <see cref="IPerson"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var sd = obj as Spouse;
            if (sd == null)
                return false;

            var so = DateTime.Compare(Terminus.GetValueOrDefault(MarriedOn.Date).Date,
                sd.Terminus.GetValueOrDefault(sd.MarriedOn.Date)) == 0;

            return so && sd.Est == _me;
        }

        public override int GetHashCode()
        {
            var me = _me?.GetHashCode() ?? 0;
            var sh = _est?.GetHashCode() ?? 0;
            var mo = _marriedOn.GetHashCode();
            var so = _separatedOn?.GetHashCode() ?? 0;
            var o = _ordinal.GetHashCode();

            return me + sh + mo + so + o;
        }

        public override string Abbrev
        {
            get
            {
                if (Est == null)
                    return "Spouse";
                return Est.Gender == Gender.Female ? "Wife" : "Husband";
            }
        }

        public override string ToString()
        {
            var soTypeName = Abbrev;
            string soEnRng;
            if (TotalYears == 0)
                soEnRng = "Newlywed";
            else if (TotalYears < 0)
                soEnRng = "Engaged";
            else
                soEnRng = $"of {TotalYears} years";
            
            var lnData = $"({Ordinal.ToOrdinal()} {soTypeName} {soEnRng})";
            return Est == null ? lnData : string.Join(" ", Est.FirstName, Est.LastName, Est.Age, lnData);
        }
        #endregion
    }
}