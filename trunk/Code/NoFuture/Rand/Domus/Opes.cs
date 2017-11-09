using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Domus.Pneuma;
using NoFuture.Util;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Domus
{
    [Serializable]
    public abstract class Opes
    {
        public IList<DepositAccount> SavingAccounts { get; } = new List<DepositAccount>();
        public IList<DepositAccount> CheckingAccounts { get; } = new List<DepositAccount>();

        public IList<IReceivable> HomeDebt { get; } = new List<IReceivable>();
        public IList<IReceivable> VehicleDebt { get; } = new List<IReceivable>();
        public IList<IReceivable> CreditCardDebt { get; } = new List<IReceivable>();

        public FinancialData FinancialData => GetFinancialState();

        protected internal abstract FinancialData GetFinancialState(DateTime? dt = null);

        protected internal Pecuniam GetTotalCurrentCcDebt(DateTime? dt = null)
        {
            var dk = new Pecuniam(0.0M);
            foreach (var cc in CreditCardDebt.Cast<CreditCardAccount>())
            {
                dk += cc.GetValueAt(dt.GetValueOrDefault(DateTime.Now)).Neg;
            }
            return dk;
        }

        protected internal Pecuniam GetTotalCurrentDebt(DateTime? dt = null)
        {
            var tlt = GetTotalCurrentCcDebt(dt).Neg;
            foreach (var hd in HomeDebt)
                tlt += hd.GetValueAt(dt.GetValueOrDefault(DateTime.Now)).Neg;
            foreach (var vd in VehicleDebt)
                tlt += vd.GetValueAt(dt.GetValueOrDefault(DateTime.Now)).Neg;
            return tlt;
        }

        protected internal abstract Pecuniam GetTotalCurrentWealth(DateTime? dt = null);
        protected internal abstract LinearEquation GetAvgEarningPerYear();

        /// <summary>
        /// Calc a yearly income salary or pay at random.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="netWorthFactor">A multiplier to linear eq's result</param>
        /// <param name="factorCalc">
        /// Optional, allows caller to specify how to factor the raw results of 
        /// <see cref="GetAvgEarningPerYear"/> for the <see cref="dt"/>.
        /// </param>
        /// <param name="stdDevInUsd"></param>
        /// <param name="dt">
        /// Optional, date used for solving the <see cref="GetAvgEarningPerYear"/> eq.
        /// </param>
        /// <returns></returns>
        public Pecuniam GetYearlyIncome(Pecuniam min, double netWorthFactor, Func<double, double> factorCalc = null,
            double stdDevInUsd = 2000, DateTime? dt = null)
        {
            if (min == null)
                min = Pecuniam.Zero;
            //get linear eq for earning 
            var eq = GetAvgEarningPerYear();
            if (eq == null)
                return Pecuniam.Zero;
            var baseValue = Math.Round(eq.SolveForY(dt.GetValueOrDefault(DateTime.Today).ToDouble()), 2);
            if (baseValue <= 0)
                return Pecuniam.Zero;

            Func<double, double> dfFunc = (d) =>
            {
                var factorVal = (d / 5) * netWorthFactor;
                while (Math.Abs(factorVal) > d)
                    factorVal = factorVal / 2;
                return factorVal;
            };

            factorCalc = factorCalc ?? dfFunc;

            var factorValue = factorCalc(baseValue);

            baseValue = Math.Round(baseValue + factorValue, 2);

            var randValue = Math.Round(
                Etx.RandomValueInNormalDist(Math.Round(baseValue, 0), stdDevInUsd), 2);
            return new Pecuniam((decimal)randValue) + min;
        }

        /// <summary>
        /// Creates purchase transactions on <see cref="t"/> at random for the given <see cref="dt"/>.
        /// </summary>
        /// <param name="spender"></param>
        /// <param name="t"></param>
        /// <param name="dt"></param>
        /// <param name="daysMax"></param>
        /// <param name="randMaxFactor">
        /// The multiplier used for the rand dollar's max, raising this value 
        /// will raise every transactions possiable max by a factor of this.
        /// </param>
        public static void CreateSingleDaysPurchases(Personality spender, ITransactionable t, DateTime? dt,
            double daysMax, int randMaxFactor = 7)
        {
            if (t == null)
                throw new ArgumentNullException(nameof(t));
            if (daysMax <= 0)
                return;
            var ccDate = dt ?? DateTime.Today;

            //build charges history
            var keepSpending = true;
            var spentSum = new Pecuniam(0);

            while (keepSpending) //want possiable multiple transactions per day
            {
                //if we reached target then exit 
                if (spentSum >= new Pecuniam((decimal)daysMax))
                    return;

                var isXmasSeason = ccDate.Month >= 11 && ccDate.Day >= 20;
                var isWeekend = ccDate.DayOfWeek == DayOfWeek.Friday ||
                                ccDate.DayOfWeek == DayOfWeek.Saturday ||
                                ccDate.DayOfWeek == DayOfWeek.Sunday;
                var actingIrresp = spender?.GetRandomActsIrresponsible() ?? false;
                var isbigTicketItem = Etx.TryAboveOrAt(96, Etx.Dice.OneHundred);
                var isSomeEvenAmt = Etx.TryBelowOrAt(3, Etx.Dice.Ten);

                //keep times during normal waking hours
                var randCcDate =
                    ccDate.Date.AddHours(Etx.IntNumber(6, isWeekend ? 23 : 19))
                        .AddMinutes(Etx.IntNumber(0, 59))
                        .AddSeconds(Etx.IntNumber(0, 59))
                        .AddMilliseconds(Etx.IntNumber(0, 999));

                //make purchase based various factors
                var v = 2;
                v = isXmasSeason ? v + 1 : v;
                v = isWeekend ? v + 2 : v;
                v = actingIrresp ? v + 3 : v;
                randMaxFactor = isbigTicketItem ? randMaxFactor * 10 : randMaxFactor;

                if (Etx.TryBelowOrAt(v, Etx.Dice.Ten))
                {
                    //create some random purchase amount
                    var chargeAmt = Pecuniam.GetRandPecuniam(5, v * randMaxFactor, isSomeEvenAmt ? 10 : 0);

                    //check if account is maxed-out\empty
                    if (!t.Pop(randCcDate, chargeAmt))
                        return;

                    spentSum += chargeAmt;
                }
                //determine if more transactions for this day
                keepSpending = Etx.CoinToss;
            }
        }

        internal static string GetPaymentNote(Identifier property, string prefix = null)
        {
            if(property == null)
                return prefix;

            var residenceLoan = property as ResidentAddress;

            if (residenceLoan != null)
            {
                return residenceLoan.IsLeased
                    ? string.Join(" ", prefix, "Rent Payment")
                    : string.Join(" ", prefix, "Mortgage Payment");
            }
            var carloan = property as Gov.Nhtsa.Vin;
            if (carloan != null)
                return string.Join(" ", prefix, "Vehicle Payment");
            var ccNum = property as CreditCardNumber;
            if (ccNum != null)
                return string.Join(" ", prefix, "Cc Payment");

            var acctNum = property as AccountId;
            if (acctNum != null)
                return string.Join(" ", prefix, "Bank Account Transfer");

            return prefix;
        }
    }
}
