using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc />
    /// <summary>
    /// Single immutable money transaction
    /// </summary>
    [Serializable]
    public class Transaction : ITransaction
    {
        #region ctor
        internal Transaction(DateTime atTime, Pecuniam amt, Guid ledgerId, IVoca description = null)
        {
            UniqueId = Guid.NewGuid();
            AtTime = atTime;
            Cash = amt ?? Pecuniam.Zero;
            Description = description;
            LedgerId = ledgerId;
        }

        internal Transaction(DateTime atTime, Pecuniam amt, Guid ledgerId, Guid fromLedgerId,
            ITransactionHistory history, IVoca description = null) : this(atTime, amt, ledgerId, description)
        {
            History = history;
            FromLedgerId = fromLedgerId;

        }

        #endregion

        #region properties
        public Guid FromLedgerId { get; }
        public Guid LedgerId { get; }
        public Guid UniqueId { get; } 
        public DateTime AtTime { get; }
        public Pecuniam Cash { get; }
        public IVoca Description { get; }
        public ITransactionHistory History { get; }
        #endregion

        #region methods

        public ITransaction GetInverse()
        {
            return new Transaction(AtTime, (Cash.Amount *-1M).ToPecuniam(), LedgerId, FromLedgerId, History, Description);
        }

        public ITransaction Clone()
        {
            return new Transaction(AtTime, Cash, LedgerId, FromLedgerId, History, Description);
        }

        public override bool Equals(object obj)
        {
            var t = obj as Transaction;
            return t != null && UniqueId.Equals(t.UniqueId);
        }

        public override int GetHashCode()
        {
            return UniqueId.GetHashCode();
        }

        public override string ToString()
        {
            return string.Join("\t", UniqueId, $"{AtTime:yyyy-MM-dd HH:mm:ss.ffff}", $"{Cash.Amount:0.00}", Description);
        }

        #endregion
    }
}
