using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="TransactionId"/>
    /// <inheritdoc cref="ITransaction"/>
    /// <summary>
    /// Single immutable money transaction
    /// </summary>
    [Serializable]
    public class Transaction : TransactionId, ITransaction
    {
        #region ctor
        internal Transaction(DateTime atTime, Pecuniam amt, Guid ledgerId, IVoca description = null):base(atTime, ledgerId)
        {
            Cash = amt ?? Pecuniam.Zero;
            Description = description;
        }

        internal Transaction(DateTime atTime, Pecuniam amt, Guid ledgerId, Guid fromLedgerId,
            ITransactionId history, IVoca description = null) : base(atTime, ledgerId, fromLedgerId, history)
        {
            Cash = amt ?? Pecuniam.Zero;
            Description = description;
        }

        #endregion

        #region properties
        public Pecuniam Cash { get; }
        public IVoca Description { get; }
        #endregion

        #region methods

        public ITransaction GetInverse()
        {
            return new Transaction(AtTime, (Cash.Amount *-1M).ToPecuniam(), LedgerId, FromLedgerId, Trace, Description);
        }

        public ITransaction Clone()
        {
            return new Transaction(AtTime, Cash, LedgerId, FromLedgerId, Trace, Description);
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
