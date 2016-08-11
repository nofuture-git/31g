using System;

namespace NoFuture.Rand.Domus
{
    public class Spouse : IRelation
    {
        #region fields
        private readonly IPerson _est;
        private readonly IPerson _me;
        private readonly DateTime _marriedOn;
        private readonly DateTime? _separatedOn;
        private readonly int _ordinal;
        #endregion

        #region ctor
        public Spouse(IPerson me, IPerson so, DateTime marriageDt, DateTime? divorceDt, int ordinal)
        {
            _me = me;
            _est = so;
            _marriedOn = marriageDt;
            _separatedOn = divorceDt;
            _ordinal = ordinal;
        }
        #endregion

        #region properties
        public IPerson Est => _est;
        public DateTime MarriedOn => _marriedOn;
        public DateTime? SeparatedOn => _separatedOn;
        public int Ordinal => _ordinal;
        #endregion

        #region methods
        /// <summary>
        /// Detemines if this is the spouse of the <see cref="IPerson"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var sd = obj as Spouse;
            if (sd == null)
                return false;

            var so = DateTime.Compare(SeparatedOn.GetValueOrDefault(MarriedOn.Date).Date,
                sd.SeparatedOn.GetValueOrDefault(sd.MarriedOn.Date)) == 0;

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

        public override string ToString()
        {
            return string.Join(" ", Est.FirstName, Est.LastName);
        }
        #endregion
    }
}