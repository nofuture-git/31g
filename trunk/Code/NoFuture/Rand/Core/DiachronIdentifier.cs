﻿using System;

namespace NoFuture.Rand.Core
{
    /// <summary>
    /// A base identifier which is tied to some span of time
    /// </summary>
    [Serializable]
    public abstract class DiachronIdentifier : Identifier, ITempore
    {
        private Tuple<DateTime?, DateTime?> _dateRange;

        protected DiachronIdentifier()
        {
            _dateRange = new Tuple<DateTime?, DateTime?>(null, null);
        }
        protected DiachronIdentifier(DateTime? startDate, DateTime? endDate)
        {
            _dateRange = new Tuple<DateTime?, DateTime?>(startDate, endDate);
        }

        public virtual DateTime? FromDate
        {
            get => _dateRange.Item1;
            set => _dateRange = new Tuple<DateTime?, DateTime?>(value, _dateRange.Item2);
        }

        public virtual DateTime? ToDate
        {
            get => _dateRange.Item2;
            set => _dateRange = new Tuple<DateTime?, DateTime?>(_dateRange.Item1, value);
        }

        public bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = FromDate == null || FromDate <= dt;
            var beforeOrOnToDt = ToDate == null || ToDate.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }

        public override bool Equals(Identifier obj)
        {
            var dId = obj as DiachronIdentifier;
            if (dId == null)
                return base.Equals(obj);

            var vEq = Value == dId.Value;
            var sDt = dId.FromDate == FromDate;
            var eDt = dId.ToDate == ToDate;
            return vEq && sDt && eDt;
        }

        public override bool Equals(object obj)
        {
            var dId = obj as DiachronIdentifier;
            return dId != null && Equals(dId) || base.Equals(obj);
        }

        public override int GetHashCode()
        {

            return base.GetHashCode() +
                   (_dateRange?.GetHashCode() ?? new Tuple<DateTime?, DateTime?>(null, null).GetHashCode());
        }
    }
}