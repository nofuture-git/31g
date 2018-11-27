using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Shared.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="IBalance" />
    /// <inheritdoc cref="Ledger" />
    /// <summary>
    /// </summary>
    [Serializable]
    public class Balance : Ledger, IBalance
    {
        #region fields
        private readonly Predicate<Pecuniam> _debitOp = x => (x ?? Pecuniam.Zero).Amount < 0;
        private readonly Predicate<Pecuniam> _creditOp = x => (x ?? Pecuniam.Zero).Amount > 0;
        private readonly Predicate<Pecuniam> _allOp = x => true;

        private readonly Func<ITransaction, DateTime, bool> _inclusiveTimes = (x, toDt) =>
            DateTime.Compare(x.AtTime, toDt) <= 0;
        private readonly Func<ITransaction, DateTime, bool> _exclusiveTimes = (x, toDt) =>
            DateTime.Compare(x.AtTime, toDt) < 0;
        #endregion

        #region methods
        public Pecuniam GetDebitSum(Tuple<DateTime, DateTime> between)
        {
            return GetRangeSum(between, _debitOp);
        }

        public Pecuniam GetCreditSum(Tuple<DateTime, DateTime> between)
        {
            return GetRangeSum(between, _creditOp);
        }

        public Pecuniam GetCurrent(DateTime dt, float rate)
        {
            if (!Transactions.Any())
                return Pecuniam.Zero;

            if (Transactions.All(x => x.AtTime > dt))
                return Pecuniam.Zero;

            var prev = FirstTransaction;
            var rest = Transactions.Skip(1);

            var bal = prev.Cash.Amount;
            foreach (var t in rest)
            {
                var tAtTime = t.AtTime;

                //does dt fall between this transaction and the last
                if (t.AtTime > dt)
                {
                    tAtTime = dt;
                }
                //the interest from prev to next
                var days = (tAtTime - prev.AtTime).TotalDays;
                bal = bal.PerDiemInterest(rate, days, DaysPerYear);

                //the current transaction is after the dt so we just want the interest
                if (tAtTime == dt)
                {
                    break;
                }
                bal = bal + t.Cash.Amount;
                prev = t;
            }

            //is there a gap in time between last recorded transaction and query dt
            if (LastTransaction.AtTime < dt)
            {
                //calc the interest for that number of days
                var days = (dt - LastTransaction.AtTime).TotalDays;
                bal = bal.PerDiemInterest(rate, days, DaysPerYear);
            }

            return new Pecuniam(bal);
        }

        public Pecuniam GetCurrent(DateTime dt, Dictionary<DateTime, float> variableRate)
        {
            if (variableRate == null || variableRate.Keys.Count <= 0)
                throw new ArgumentNullException(nameof(variableRate));

            //get very first recorded transaction
            var oldestTransaction = Transactions.FirstOrDefault();
            if (oldestTransaction == null)
                return Pecuniam.Zero;

            var bal = Pecuniam.Zero;

            //set first transaction as the lower bounds of the current time-frame
            var prevVdt = oldestTransaction.AtTime;

            foreach (var vdt in variableRate.Keys)
            {
                //the dictionary is the date the rate ended, not began
                if (DateTime.Compare(prevVdt, dt) > 0)
                    continue;

                var currVdt = vdt;

                //this rate was only applicable to this many days
                var daysAtRate = (currVdt - prevVdt).TotalDays;

                //this iteration's rate-ending-date is actually beyond our query date
                if (DateTime.Compare(currVdt, dt) > 0)
                {
                    daysAtRate = (dt - prevVdt).TotalDays;
                    currVdt = dt;
                }

                //get those transactions which occured between
                var betwixtTs = GetTransactions(prevVdt, currVdt);
                betwixtTs.Sort(Comparer);

                //add in this date-ranges transactions to the running balance
                bal = betwixtTs.Aggregate(bal, (current, bts) => current + bts.Cash);

                //get the balance plus iterest for the number of days this rate was in effect
                bal = new Pecuniam(bal.Amount.PerDiemInterest(variableRate[vdt], daysAtRate, DaysPerYear));

                //save where we ended this time for next iteration
                prevVdt = currVdt;
            }

            return bal;
        }

        public List<ITransaction> GetTransactions(DateTime? from = null, DateTime? to = null, bool includeThoseOnToDate = false)
        {
            var fromDt = @from ?? FirstTransaction.AtTime;
            var toDt = @to ?? LastTransaction.AtTime;

            return GetRange(new Tuple<DateTime, DateTime>(fromDt, toDt), _allOp, includeThoseOnToDate);
        }

        public List<ITransaction> GetDebits(DateTime? from = null, DateTime? to = null,
            bool includeThoseOnToDate = true)
        {
            var fromDt = @from ?? FirstTransaction.AtTime;
            var toDt = @to ?? LastTransaction.AtTime;

            return GetRange(new Tuple<DateTime, DateTime>(fromDt, toDt), _debitOp, includeThoseOnToDate);
        }

        public List<ITransaction> GetCredits(DateTime? from = null, DateTime? to = null,
            bool includeThoseOnToDate = true)
        {
            var fromDt = @from ?? FirstTransaction.AtTime;
            var toDt = @to ?? LastTransaction.AtTime;

            return GetRange(new Tuple<DateTime, DateTime>(fromDt, toDt), _creditOp, includeThoseOnToDate);
        }

        public IBalance GetInverse()
        {
            var b = new Balance();
            foreach (var t in Transactions)
            {
                b.AddTransaction(t.GetInverse());
            }

            return b;
        }

        public void PostBalance(IBalance source, DateTime? atTime = null)
        {
            if (source == null || source.TransactionCount <= 0)
                return;

            if (source.Id == Id || ReferenceEquals(source, this))
                return;
            var dt = atTime.GetValueOrDefault(DateTime.UtcNow);
            var credits = source.GetCredits();
            var debits = source.GetDebits();

            foreach (var credit in credits)
            {
                var traceEntry = new TraceTransactionId(credit.UniqueId, source.Id, dt) { Trace = credit.Trace };
                AddPositiveValue(credit.AtTime, credit.Cash, credit.Description, traceEntry);
                source.AddNegativeValue(DateTime.UtcNow, credit.Cash, new VocaBase($"{nameof(PostBalance)} from {source.Id} to {Id}"));
            }

            foreach (var debit in debits)
            {
                var traceEntry = new TraceTransactionId(debit.UniqueId, source.Id, dt) {Trace = debit.Trace};
                AddNegativeValue(debit.AtTime, debit.Cash, debit.Description, traceEntry);
                source.AddPositiveValue(DateTime.UtcNow, debit.Cash, new VocaBase($"{nameof(PostBalance)} from {source.Id} to {Id}"));
            }
        }

        public virtual Guid AddPositiveValue(ITransactionable source, Pecuniam amount, DateTime? atTime = null, IVoca description = null)
        {
            return Transfer(source as IBalance, amount, true, atTime, description);
        }

        public virtual Guid AddNegativeValue(ITransactionable source, Pecuniam amount, DateTime? atTime = null, IVoca description = null)
        {
            return Transfer(source as IBalance, amount, false, atTime, description);
        }

        protected internal Guid Transfer(IBalance source, Pecuniam amount, bool asCredit, DateTime? atTime = null,
            IVoca description = null)
        {
            if(source == null || source.IsEmpty)
                return Guid.Empty;

            amount = amount ?? Pecuniam.Zero;
            amount = amount.GetAbs();
            if (amount == Pecuniam.Zero)
                return Guid.Empty;

            var dt = atTime.GetValueOrDefault(DateTime.UtcNow);

            if(asCredit && amount > source.GetCurrent(dt, 0f))
                throw new WatDaFookIzDis($"The total balance for id {source.Id} is less-than the amount of {amount}.");

            if(!asCredit && amount > GetCurrent(dt, 0f))
                throw new WatDaFookIzDis($"The total balance for id {Id} is less-than the amount of {amount}.");
            var srcMethod = asCredit ? nameof(AddPositiveValue) : nameof(AddNegativeValue);
            description = description ?? new VocaBase($"{srcMethod} from {source.Id} to {Id}");

            var fromSrcId = asCredit
                ? source.AddNegativeValue(dt, amount, description)
                : AddNegativeValue(dt, amount, description);
            var traceEntry = new TraceTransactionId(fromSrcId, source.Id, dt);
            return asCredit
                ? AddPositiveValue(dt, amount, description, traceEntry)
                : source.AddPositiveValue(dt, amount, description, traceEntry);
        }

        protected internal Pecuniam GetRangeSum(Tuple<DateTime, DateTime> between, Predicate<Pecuniam> op)
        {
            var paymentsInRange = GetRange(between, op);

            if (paymentsInRange.Count == 0)
                return Pecuniam.Zero;

            var sumPayments = paymentsInRange.Select(x => x.Cash.Amount).Sum();
            return new Pecuniam(sumPayments);
        }

        protected internal List<ITransaction> GetRange(Tuple<DateTime, DateTime> between, Predicate<Pecuniam> op, bool includeThoseOnToDate = true)
        {
            var ts = Transactions;
            if (ts.Count <= 0)
                return new List<ITransaction>();

            var fromDt = between?.Item1 ?? FirstTransaction.AtTime;
            var toDt = between?.Item2 ?? LastTransaction.AtTime;

            if (fromDt.Equals(toDt))
                return new List<ITransaction>();

            var olderDate = DateTime.Compare(fromDt, toDt) < 0 ? fromDt : toDt;
            var newerDate = DateTime.Compare(toDt, fromDt) > 0 ? toDt : fromDt;

            op = op ?? _allOp;

            var uptoOp = includeThoseOnToDate ? _inclusiveTimes : _exclusiveTimes;

            return Transactions.Where(
                    x => op(x.Cash) && DateTime.Compare(x.AtTime, olderDate) >= 0 && uptoOp(x, newerDate))
                .ToList();
        }

        public override string ToString()
        {
            return string.Join("\n", Transactions);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var ledger = obj as Balance;
            if (ledger == null)
                return base.Equals(obj);
            return ledger.Id == Id;
        }

        #endregion
    }
}
