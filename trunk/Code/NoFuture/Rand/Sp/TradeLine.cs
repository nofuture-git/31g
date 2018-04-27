using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [Serializable]
    public class TradeLine : ITradeLine
    {
        #region constants
        public static TimeSpan DefaultDueFrequency = new TimeSpan(30,0,0,0);
        #endregion

        #region fields
        private readonly Guid _uniqueId = Guid.NewGuid();
        private readonly Balance _balance = new Balance();
        private DateTime _openDate;
        private DateTime? _closeDate;
        private TimeSpan? _dueFrequency;
        #endregion

        public TradeLine(){ }

        public TradeLine(DateTime openDate)
        {
            _openDate = openDate;
        }

        #region properties
        public FormOfCredit? FormOfCredit { get; set; }
        public IBalance Balance => _balance;

        public TimeSpan? DueFrequency
        {
            get => _dueFrequency;
            set => _dueFrequency = value;
        }

        public DateTime Inception
        {
            get => _openDate;
            set => _openDate = value;
        }

        public DateTime? Terminus
        {
            get => _closeDate;
            set => _closeDate = value;
        }

        public ClosedCondition? Closure { get; set; }

        #endregion

        #region methods

        public virtual bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = Inception <= dt;
            var beforeOrOnToDt = Terminus == null || Terminus.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }

        public override string ToString()
        {
            return _uniqueId.ToString();
        }

        public override bool Equals(object obj)
        {
            var tl = obj as TradeLine;
            if (tl == null)
                return false;
            return tl._uniqueId == _uniqueId;
        }

        public override int GetHashCode()
        {
            return _uniqueId.GetHashCode();
        }

        /// <summary>
        /// Applies a negative valued transaction against the balance.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="amt"></param>
        /// <param name="note"></param>
        /// <param name="fee"></param>
        public virtual void AddNegativeValue(DateTime dt, Pecuniam amt, IVoca note = null, Pecuniam fee = null)
        {
            if (amt == Pecuniam.Zero)
                return;
            fee = fee == null ? Pecuniam.Zero : fee.GetNeg();
            Balance.AddTransaction(dt, amt.GetNeg(), note, fee);
        }

        /// <summary>
        /// Applies a positive valued transaction against the balance
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="val"></param>
        /// <param name="note"></param>
        /// <param name="fee"></param>
        /// <returns></returns>
        public virtual bool AddPositiveValue(DateTime dt, Pecuniam val, IVoca note = null, Pecuniam fee = null)
        {
            if (val == Pecuniam.Zero)
                return false;
            fee = fee == null ? Pecuniam.Zero : fee.GetAbs();
            Balance.AddTransaction(dt, val.GetAbs(), note, fee);
            return true;
        }
        #endregion
    }
}
