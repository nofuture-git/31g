using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
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

        public double DaysPerYear
        {
            get => _balance.DaysPerYear;
            set => _balance.DaysPerYear = value;
        }

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

        public virtual IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x, txtCase);
            var itemData = new Dictionary<string, object>();

            if(Inception != DateTime.MinValue)
                itemData.Add(textFormat(nameof(Inception)), Inception.ToString("s"));

            if(Terminus != null && Terminus != DateTime.MinValue)
                itemData.Add(textFormat(nameof(Terminus)), Terminus.Value.ToString("s"));

            if(FormOfCredit != null)
                itemData.Add(textFormat(nameof(FormOfCredit)), FormOfCredit.Value.ToString());

            var interval = DueFrequency.ToInterval();
            if(interval != null)
                itemData.Add(textFormat("Interval"), interval);

            if(Closure != null)
                itemData.Add(textFormat(nameof(ClosedCondition)), Closure);

            return itemData;
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
