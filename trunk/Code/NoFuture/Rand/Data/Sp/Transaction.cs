using System;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Single immutable money transaction
    /// </summary>
    [Serializable]
    public class Transaction : ITransaction
    {
        #region ctor
        public Transaction(DateTime atTime, Pecuniam amt, string description = null)
        {
            AtTime = atTime;
            Cash = amt;
            Description = description;
        }
        public Transaction(DateTime atTime, Pecuniam amt, Pecuniam fee, string description = null)
        {
            AtTime = atTime;
            Cash = amt;
            Description = description;
            Fee = fee;
        }
        #endregion

        #region properties
        public Guid UniqueId { get; } = Guid.NewGuid();
        public DateTime AtTime { get; }
        public Pecuniam Cash { get; }
        public Pecuniam Fee { get; }
        public string Description { get; }
        #endregion

        #region overrides
        public override bool Equals(object obj)
        {
            if (Equals(obj, null))
                return false;
            var t = obj as Transaction;
            if (t == null)
                return false;
            return UniqueId.Equals(t.UniqueId);
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
