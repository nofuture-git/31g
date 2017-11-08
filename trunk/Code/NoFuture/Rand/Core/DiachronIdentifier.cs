using System;

namespace NoFuture.Rand.Core
{
    /// <summary>
    /// A base identifier which is tied to some span of time
    /// </summary>
    [Serializable]
    public abstract class DiachronIdentifier : Identifier
    {
        public virtual DateTime? FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }

        public bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = FromDate == null || FromDate <= dt;
            var beforeOrOnToDt = ToDate == null || ToDate.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }
    }
}