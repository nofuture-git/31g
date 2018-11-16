using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Sp.Enums;
using NoFuture.Shared.Core;

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
        #endregion

        public TradeLine(){ }

        public TradeLine(DateTime openDate)
        {
            Inception = openDate;
        }

        #region properties
        public FormOfCredit? FormOfCredit { get; set; }
        public IBalance Balance => _balance;
        public TimeSpan? DueFrequency { get; set; }

        public DateTime Inception { get; set; }

        public DateTime? Terminus { get; set; }

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
        public virtual void AddNegativeValue(DateTime dt, Pecuniam amt, IVoca note = null)
        {
            if (amt == Pecuniam.Zero)
                return;
            Balance.AddTransaction(dt, amt.GetNeg(), note);
        }

        /// <summary>
        /// Applies a positive valued transaction against the balance
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="val"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public virtual bool AddPositiveValue(DateTime dt, Pecuniam val, IVoca note = null)
        {
            if (val == Pecuniam.Zero)
                return false;
            Balance.AddTransaction(dt, val.GetAbs(), note);
            return true;
        }

        public virtual Pecuniam AveragePerDueFrequency()
        {
            var ts = DueFrequency ?? Constants.TropicalYear;

            var now = DateTime.Now;
            var start = Inception == DateTime.MinValue ? now.Add(Constants.TropicalYear.Negate()) : Inception;
            var end = Terminus == null || Terminus == DateTime.MinValue ? now : Terminus.Value;

            if(start > end)
                return Pecuniam.Zero;
            //how many whole-blocks of ts can we get between start and end
            var wholeTimeBlocks = Convert.ToInt32(Math.Floor((end - start).TotalDays / ts.TotalDays));
            if(wholeTimeBlocks <= 0)
                return Pecuniam.Zero;
            var avgPerBlock = new List<double>();
            var begin = start;
            for (var i = 0; i < wholeTimeBlocks; i++)
            {
                var transactions = Balance.GetTransactionsBetween(begin, begin.Add(ts), true);
                var avgAtBlockI = Util.Core.Math.Extensions.Mean(transactions.Select(t => t.Cash.ToDouble()));
                avgPerBlock.Add(avgAtBlockI);
            }

            var totalAvg = Util.Core.Math.Extensions.Mean(avgPerBlock);
            return new Pecuniam(Convert.ToDecimal(totalAvg));
        }
        #endregion
    }
}
