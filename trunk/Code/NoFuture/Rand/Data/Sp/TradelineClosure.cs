using System;
using NoFuture.Rand.Data.Sp.Enums;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public struct TradelineClosure
    {
        public DateTime ClosedDate;
        public ClosedCondition Condition;
    }
}