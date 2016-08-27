using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Exceptions;
using NoFuture.Util.Math;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public class Balance : TransactionHistory, IBalance
    {
        #region fields
        private readonly Func<decimal, bool> _debitOp = x => x < 0;
        private readonly Func<decimal, bool> _creditOp = x => x > 0;
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

        public Pecuniam GetFees(DateTime dt)
        {
            if (!Transactions.Any())
                return Pecuniam.Zero;

            if (Transactions.All(x => x.AtTime > dt))
                return Pecuniam.Zero;

            var bal = Pecuniam.Zero;
            foreach (var t in Transactions)
            {
                if (t.AtTime > dt)
                    break;
                bal = bal + t.Fee;
            }
            return bal;
        }

        public Pecuniam GetCurrent(DateTime dt, float rate)
        {
            if (!Transactions.Any())
                return Pecuniam.Zero;

            if (Transactions.All(x => x.AtTime > dt))
                return Pecuniam.Zero;

            var prev = Transactions[0];
            var rest = Transactions.Skip(1);

            var bal = prev.Cash.Amount;
            foreach (var t in rest)
            {
                if (t.AtTime > dt)
                    break;
                var days = (t.AtTime - prev.AtTime).TotalDays;
                bal = bal.PerDiemInterest(rate, days);
                bal = bal + t.Cash.Amount;
                prev = t;
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
                var betwixtTs = GetTransactionsBetween(prevVdt, currVdt);
                betwixtTs.Sort(Comparer);

                //add in this date-ranges transactions to the running balance
                bal = betwixtTs.Aggregate(bal, (current, bts) => current + bts.Cash);

                //get the balance plus iterest for the number of days this rate was in effect
                bal = new Pecuniam(bal.Amount.PerDiemInterest(variableRate[vdt], daysAtRate));

                //save where we ended this time for next iteration
                prevVdt = currVdt;
            }

            return bal;
        }

        public List<ITransaction> GetTransactionsBetween(DateTime from, DateTime to, bool includeThoseOnToDate = false)
        {
            if (includeThoseOnToDate)
            {
                return Transactions.Where(
                    x => DateTime.Compare(x.AtTime, from) >= 0 && DateTime.Compare(x.AtTime, to) <= 0)
                    .ToList();
            }
            return Transactions.Where(
                x => DateTime.Compare(x.AtTime, from) >= 0 && DateTime.Compare(x.AtTime, to) < 0)
                .ToList();
        }

        protected internal Pecuniam GetRangeSum(Tuple<DateTime, DateTime> between, Func<decimal, bool> op)
        {
            var ts = Transactions;
            if (ts.Count <= 0)
                return Pecuniam.Zero;

            if (between == null)
                throw new ArgumentNullException(nameof(between));

            if (between.Item1.Equals(between.Item2))
                throw new ItsDeadJim("The calculation requires a date range.");

            var olderDate = DateTime.Compare(between.Item1, between.Item2) < 0 ? between.Item1 : between.Item2;
            var newerDate = DateTime.Compare(between.Item2, between.Item1) > 0 ? between.Item2 : between.Item1;

            var paymentsInRange =
                ts.Where(
                    x =>
                        DateTime.Compare(x.AtTime, olderDate) >= 0 && DateTime.Compare(x.AtTime, newerDate) <= 0 &&
                        op(x.Cash.Amount))
                    .ToList();

            if (paymentsInRange.Count == 0)
                return Pecuniam.Zero;

            var sumPayments = paymentsInRange.Select(x => x.Cash.Amount).Sum();
            return new Pecuniam(sumPayments);
        }

        public override string ToString()
        {
            return string.Join("\n", Transactions);
        }

        #endregion
    }
}
