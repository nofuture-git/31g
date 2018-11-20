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

        public virtual Guid AddNegativeValue(DateTime dt, Pecuniam amt, IVoca note = null, ITransactionId trace = null)
        {
            return Balance.AddNegativeValue(dt, amt, note, trace);
        }

        public virtual Guid AddPositiveValue(DateTime dt, Pecuniam val, IVoca note = null, ITransactionId trace = null)
        {
            return Balance.AddPositiveValue(dt, val, note, trace);
        }

        public virtual Pecuniam AveragePerDueFrequency(TimeSpan? duration = null)
        {
            var ts = duration ?? DueFrequency ?? Constants.TropicalYear;
            //how many whole-blocks of ts can we get between start and end
            var wholeTimeBlocks = GetWholeTimeBlocks(duration);
            if(wholeTimeBlocks <= 0)
                return Pecuniam.Zero;
            var avgPerBlock = new List<double>();
            var begin = Inception == DateTime.MinValue ? DateTime.Today.Add(Constants.TropicalYear.Negate()) : Inception;
            for (var i = 0; i < wholeTimeBlocks; i++)
            {
                var transactions = Balance.GetTransactions(begin, begin.Add(ts), true);
                var avgAtBlockI = Util.Core.Math.Extensions.Mean(transactions.Select(t => t.Cash.ToDouble()));
                avgPerBlock.Add(avgAtBlockI);
            }

            var totalAvg = Util.Core.Math.Extensions.Mean(avgPerBlock);
            return new Pecuniam(Convert.ToDecimal(totalAvg));
        }

        protected internal virtual int GetWholeTimeBlocks(TimeSpan? duration = null)
        {
            var ts = duration ?? DueFrequency ?? Constants.TropicalYear;

            var now = DateTime.UtcNow;
            var start = Inception == DateTime.MinValue ? now.Add(Constants.TropicalYear.Negate()) : Inception;
            var end = Terminus == null || Terminus == DateTime.MinValue ? now : Terminus.Value;

            if (start > end)
                return 0;
            //how many whole-blocks of ts can we get between start and end
            var wholeTimeBlocks = Convert.ToInt32(Math.Floor((end - start).TotalDays / ts.TotalDays));
            return wholeTimeBlocks < 0 ? 0 : wholeTimeBlocks;
        }

        /// <summary>
        /// Produces a random <see cref="TradeLine"/> with random transactions which average about to <see cref="averageAmount"/>
        /// </summary>
        /// <param name="averageAmount">
        /// The random history will have many values whose average is more or less this value.
        /// </param>
        /// <param name="dueFrequency">
        /// Optional, if null then assigned to 30-days.
        /// Assigned to the resulting instance&apos;s property of the same name
        /// </param>
        /// <param name="inception">
        /// Optional, if null then random value some day in the past year.
        /// Assigned to the resulting instance&apos; property of the same name.
        /// </param>
        /// <param name="terminus">Passed directly to resulting instance</param>
        /// <param name="randomActsIrresponsible">A function pointer to a kind of personality</param>
        /// <returns></returns>
        [RandomFactory]
        public static ITradeLine RandomTradeLineWithVariedHistory(Pecuniam averageAmount = null, TimeSpan? dueFrequency = null,
            DateTime? inception = null, DateTime? terminus = null, Func<bool> randomActsIrresponsible = null)
        {
            averageAmount = averageAmount ?? Pecuniam.RandomPecuniam(30, 200);
            var start = inception ?? DateTime.Today.AddDays(Etx.RandomInteger(45, 360));
            var tl = new TradeLine(start)
            {
                Terminus = terminus,
                DueFrequency = dueFrequency
            };
            var wholeTimeBlocks = tl.GetWholeTimeBlocks();
            if (wholeTimeBlocks <= 0)
                return tl;

            tl.GetRandomHistory(averageAmount, false, randomActsIrresponsible);
            return tl;
        }

        /// <summary>
        /// Produces a random <see cref="TradeLine"/> with a history 
        /// </summary>
        /// <param name="amount">
        /// Every entry in the history will have this exact value on a regular <see cref="dueFrequency"/>
        /// </param>
        /// <param name="dueFrequency">
        /// Optional, if null then assigned to 30-days.
        /// Assigned to the resulting instance&apos;s property of the same name
        /// </param>
        /// <param name="inception">
        /// Optional, if null then random value some day in the past year.
        /// Assigned to the resulting instance&apos; property of the same name.
        /// </param>
        /// <param name="terminus">Passed directly to resulting instance</param>
        /// <returns></returns>
        [RandomFactory]
        public static ITradeLine RandomTradeLineWithSteadyHistory(Pecuniam amount = null, TimeSpan? dueFrequency = null,
            DateTime? inception = null, DateTime? terminus = null)
        {
            amount = amount ?? Pecuniam.RandomPecuniam(30, 200);
            var start = inception ?? DateTime.Today.AddDays(Etx.RandomInteger(45, 360));
            var tl = new TradeLine(start)
            {
                Terminus = terminus,
                DueFrequency = dueFrequency
            };
            tl.GetRandomHistory(amount);

            return tl;
        }

        /// <summary>
        /// A instance version of the random factories - this allows for child-type&apos;s random factories to reuse this code.
        /// </summary>
        protected internal virtual void GetRandomHistory(Pecuniam amount = null, bool steadyPayments = true, Func<bool> randomActsIrresponsible = null)
        {
            amount = amount ?? Pecuniam.RandomPecuniam(30, 200);
            var wholeTimeBlocks = GetWholeTimeBlocks();
            wholeTimeBlocks = wholeTimeBlocks == 0 ? 1 : wholeTimeBlocks;
            var tss = DueFrequency == null || DueFrequency == TimeSpan.MinValue ? PecuniamExtensions.GetTropicalMonth() : DueFrequency.Value;
            var start = Inception;
            var isNegative = amount.Amount < 0;
            if (steadyPayments)
            {
                for (var i = 0; i < wholeTimeBlocks; i++)
                {
                    if (isNegative)
                        Balance.AddNegativeValue(start, amount);
                    else
                        Balance.AddPositiveValue(start, amount);
                    start = start.Add(tss);
                }
                return;
            }

            var score = Etx.RandomDouble();
            randomActsIrresponsible =
                randomActsIrresponsible ?? (() => Etx.RandomValueInNormalDist(score, 0.33334D) > 0);

            var randEntries = Etx.RandomValuesFromAverage(amount.ToDouble(), wholeTimeBlocks).ToList();
            var someDaysFromBlockStart = Etx.RandomInteger(0, tss.Days - 1);
            for (var i = 0; i < randEntries.Count; i++)
            {
                var onDay = randomActsIrresponsible() ? Etx.RandomInteger(1, 5) : 0;
                onDay = onDay + someDaysFromBlockStart;
                if (isNegative)
                    Balance.AddNegativeValue(start.AddDays(onDay), randEntries[i].ToPecuniam());
                else
                    Balance.AddPositiveValue(start.AddDays(onDay), randEntries[i].ToPecuniam());
                start = start.Add(tss);
            }
        }
        #endregion
    }
}
