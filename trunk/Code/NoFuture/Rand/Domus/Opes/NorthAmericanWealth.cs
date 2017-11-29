using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Cc;
using NoFuture.Rand.Data.Sp.Enums;

namespace NoFuture.Rand.Domus.Opes
{
    /// <summary>
    /// For generating the financial state and history of a <see cref="NorthAmerican"/>
    /// at random.
    /// </summary>
    [Serializable]
    public class NorthAmericanWealth : WealthBase
    {
        #region constants
        public const double DF_STD_DEV_PERCENT = 0.1285D;
        public const double DF_DAILY_SPEND_PERCENT = 0.1D;

        private const string UTIL_ELEC = "Electric";
        private const string UTIL_GAS = "Gas";
        private const string TELCO = "Telco";
        private const string UTIL_WATER = "Water";
        private const string HEALTH_CARE = "Healthcare";
        private const string INSURANCE = "Insurance";
        #endregion

        #region fields
        public IList<DepositAccount> SavingAccounts { get; } = new List<DepositAccount>();
        public IList<DepositAccount> CheckingAccounts { get; } = new List<DepositAccount>();

        public IList<IReceivable> HomeDebt { get; } = new List<IReceivable>();
        public IList<IReceivable> VehicleDebt { get; } = new List<IReceivable>();
        public IList<IReceivable> CreditCardDebt { get; } = new List<IReceivable>();

        private Pecuniam _residencePmt = Pecuniam.Zero;
        private Pecuniam _carPmt = Pecuniam.Zero;
        private Pecuniam _homeEquity = Pecuniam.Zero;
        private Pecuniam _vehicleEquity = Pecuniam.Zero;
        private readonly List<Tuple<string, int, Pecuniam>> _monthlyBills = new List<Tuple<string, int, Pecuniam>>
            {
                new Tuple<string, int, Pecuniam>(UTIL_ELEC, Etx.IntNumber(1, 28), null),
                new Tuple<string, int, Pecuniam>(UTIL_GAS, Etx.IntNumber(1, 28), null),
                new Tuple<string, int, Pecuniam>(UTIL_WATER, Etx.IntNumber(1, 28), null),
                new Tuple<string, int, Pecuniam>(TELCO, Etx.IntNumber(1, 28), null),
                new Tuple<string, int, Pecuniam>(HEALTH_CARE, Etx.IntNumber(1,28), null),
            };
        #endregion

        #region ctor
        /// <summary>
        /// Create a new Opes for the given <see cref="american"/>
        /// </summary>
        /// <param name="american"></param>
        /// <param name="isRenting">
        /// Optional, force the generated instance as renting (instead of mortgage).
        /// </param>
        public NorthAmericanWealth(NorthAmerican american, bool isRenting = false): base(american, isRenting)
        {
            CreditScore = new PersonalCreditScore(american);
        }
        #endregion

        #region properties
        public Pecuniam ResidencePayment => _residencePmt;
        public Pecuniam CarPayment => _carPmt;
        public Pecuniam Paycheck { get; }
        #endregion

        #region methods

        /// <summary>
        /// Generates the random accounts and history for the given Opes
        /// </summary>
        /// <param name="possiableVehicles"></param>
        /// <param name="stdDevAsPercent"></param>
        public void CreateRandomAmericanOpes(int possiableVehicles = 1, double stdDevAsPercent = DF_STD_DEV_PERCENT)
        {
            //determine residence pmt
            _residencePmt += IsRenting
                ? AddRent(stdDevAsPercent)
                : AddMortgage(stdDevAsPercent) ?? Pecuniam.Zero;

            //determin car payment(s)
            for(var i=0; i<possiableVehicles; i++)
                _carPmt += AddVehicleLoan(stdDevAsPercent) ?? Pecuniam.Zero;

            //add CC's
            AddTotalCcDebt(stdDevAsPercent);
            
            //add bank accounts
            AddBankingAccounts(stdDevAsPercent);
        }

        /// <summary>
        /// Creates random checking and savings accounts with a history which is intertwined with 
        /// other history.
        /// </summary>
        /// <param name="stdDevAsPercent"></param>
        protected internal void AddBankingAccounts(double stdDevAsPercent = DF_STD_DEV_PERCENT)
        {
            if (Paycheck == null)
            {
                //TODO - need to remove idea of 'Paycheck' and replace with IReditus
                return;
            }

            //1 year history
            var baseDate = DateTime.Today.AddYears(-1);

            //set up checking
            var checking = CheckingAccount.GetRandomCheckingAcct(Person,baseDate);
            var randChecking = NorthAmericanFactors.GetRandomFactorValue(FactorTables.CheckingAccount, Factors.CheckingAcctFactor, stdDevAsPercent);
            checking.Push(baseDate.AddDays(-1), randChecking.ToPecuniam(), Mereo.GetMereoById(null, "Init Checking"), Pecuniam.Zero);

            //set up a savings
            var randSavings = NorthAmericanFactors.GetRandomFactorValue(FactorTables.SavingsAccount, Factors.SavingsAcctFactor, stdDevAsPercent);
            var savings = SavingsAccount.GetRandomSavingAcct(Person, baseDate);
            savings.Push(baseDate.AddDays(-1), randSavings.ToPecuniam(), Mereo.GetMereoById(null, "Init Savings"), Pecuniam.Zero);
            var friCounter = 0;
            var historyTs = DateTime.Today - baseDate;

            //have insurance payments show up as same amount each month
            _monthlyBills.Add(new Tuple<string, int, Pecuniam>(INSURANCE, Etx.IntNumber(1, 28),
                GetRandomMonthlyBillAmount()));

            for (var i = 0; i < historyTs.TotalDays; i++)
            {
                var loopDtSt = baseDate.AddDays(i).Date;

                //add pay every other friday
                if (loopDtSt.DayOfWeek == DayOfWeek.Friday)
                {
                    if (friCounter%2 == 0)
                    {
                        checking.Push(loopDtSt.AddSeconds(1), Paycheck, Mereo.GetMereoById(null, "Pay"), Pecuniam.Zero);
                    }
                    
                    //replenish savings
                    if (savings.Value < randSavings.ToPecuniam())
                        DepositAccount.TransferFundsInBankAccounts(checking, savings, randSavings.ToPecuniam() - savings.Value,
                            loopDtSt.AddSeconds(15));
                    friCounter += 1;
                }

                //b-day money
                if (DateTime.Compare(loopDtSt,
                        new DateTime(loopDtSt.Year, Person.BirthCert.DateOfBirth.Month, Person.BirthCert.DateOfBirth.Day)) ==
                    0)
                {
                    checking.Push(loopDtSt.AddHours(15), Pecuniam.GetRandPecuniam(10,100,10), Mereo.GetMereoById(null, "b-day money"), Pecuniam.Zero);
                }

                //add house pmts
                checking.AddDebitTransactionsByDate(loopDtSt, HomeDebt);

                //add car pmts
                checking.AddDebitTransactionsByDate(loopDtSt, VehicleDebt);

                //add cc pmts
                checking.AddDebitTransactionsByDate(loopDtSt, CreditCardDebt);

                //add montly pmts
                PayMonthlyBills(loopDtSt, checking);

                //create some checking account transactions 
                var currentBalance = checking.GetValueAt(loopDtSt);

                //if broke, move funds from savings and no spending
                if (checking.GetStatus(loopDtSt) != SpStatus.Current || currentBalance <= new Pecuniam(10))
                {
                    DepositAccount.TransferFundsInBankAccounts(savings, checking,
                        randChecking.ToPecuniam() - checking.Value,
                        loopDtSt.AddHours(19));
                    continue;
                }

                var myPecuniam = currentBalance > Paycheck ? currentBalance : Paycheck;
                var dailyMax = (double) myPecuniam.Amount*DF_DAILY_SPEND_PERCENT;

                CreateSingleDaysPurchases(Person.Personality, checking, loopDtSt, dailyMax);
            }
            SavingAccounts.Add(savings);
            CheckingAccounts.Add(checking);
        }

        /// <summary>
        /// Generates some monthly bill history for the <see cref="checking"/>.  
        /// The dates and amounts routine and random.
        /// </summary>
        /// <param name="loopDtSt"></param>
        /// <param name="checking"></param>
        protected internal void PayMonthlyBills(DateTime loopDtSt, CheckingAccount checking)
        {
            //add utility pmts
            if (_monthlyBills == null)
                return;
            foreach (var t in _monthlyBills)
            {
                if (loopDtSt.Day != t?.Item2)
                    continue;

                var billAmt = t.Item3 ?? GetRandomMonthlyBillAmount(t.Item1, loopDtSt.Month);
                checking.Pop(loopDtSt.AddHours(12), billAmt, Mereo.GetMereoById(null, t.Item1), Pecuniam.Zero);
            }
        }

        /// <summary>
        /// Number of Cards held Src [http://www.creditcards.com/credit-card-news/ownership-statistics-charts-1276.php]
        /// </summary>
        /// <param name="stdDevAsPercent"></param>
        protected internal void AddTotalCcDebt(double stdDevAsPercent = DF_STD_DEV_PERCENT)
        {
            //~ 25% americans have no CC
            if (!(Person.Personality.Conscientiousness.Value.Zscore < 0.5D))
                return;

            //get random total cc debt
            var randCcValue = NorthAmericanFactors.GetRandomFactorValue(FactorTables.CreditCardDebt, Factors.CreditCardDebtFactor, stdDevAsPercent);

            //get number of cc's to divide total cc debt into
            var numOfCc = 1;
            var roll = 100;
            if (Person.Age >= 30)
            {
                numOfCc += 1;
                roll = 13;
            }
            if (Person.Age >= 47 && Person.Age < 66)
                roll = 66;

            if (Etx.TryBelowOrAt(roll, Etx.Dice.OneHundred))
                numOfCc += 1;

            //get number of cc's as random portions of total cc debt
            var ccRatio = Etx.RandomPortions(numOfCc);

            //create cc and history for targeted count of cc
            foreach (var cc in ccRatio)
            {
                //don't waste clock cycles for very small total amounts
                if (cc*randCcValue < 1.0D)
                    break;
                //limit to total cc debt
                if (GetTotalCurrentCcDebt() > randCcValue.ToPecuniam())
                    break;

                AddSingleCcDebt(stdDevAsPercent);
            }
        }

        /// <summary>
        /// Creates random <see cref="Rent"/> instance with a history and adds
        /// it to the <see cref="HomeDebt"/> collection.
        /// </summary>
        /// <param name="stdDevAsPercent"></param>
        /// <remarks>
        /// src [http://www.nmhc.org/Content.aspx?id=4708]
        /// </remarks>
        protected internal Pecuniam AddRent(double stdDevAsPercent = DF_STD_DEV_PERCENT)
        {
            if (!IsRenting)
                return Pecuniam.Zero;

            var homeDebtFactor = Factors.HomeDebtFactor;

            var rent = Rent.GetRandomRentWithHistory(Person.Address, homeDebtFactor, Person.Personality, stdDevAsPercent);
            var randRent = rent.MonthlyPmt;

            HomeDebt.Add(rent);
            return randRent;
        }

        /// <summary>
        /// Creates random <see cref="FixedRateLoan"/> instance with a history and adds
        /// it to the <see cref="HomeDebt"/> collection.
        /// </summary>
        /// <param name="stdDevAsPercent"></param>
        protected internal Pecuniam AddMortgage(double stdDevAsPercent = DF_STD_DEV_PERCENT)
        {
            //calc a rand amount of what is still owed
            var randHouseDebt = NorthAmericanFactors.GetRandomFactorValue(FactorTables.HomeDebt, Factors.HomeDebtFactor, stdDevAsPercent);

            //calc rand amount of equity accrued
            var randHouseEquity = NorthAmericanFactors.GetRandomFactorValue(FactorTables.HomeEquity, Factors.HomeEquityFactor, stdDevAsPercent);
            _homeEquity += randHouseEquity.ToPecuniam();

            //get rand interest rate weighted by score
            var randRate = CreditScore.GetRandomInterestRate(null, Gov.Fed.RiskFreeInterestRate.DF_VALUE) * 0.01;
            var spCost = new Pecuniam((decimal)randHouseDebt);
            var totalCost = new Pecuniam((decimal)(randHouseDebt + randHouseEquity));

            //create a loan on current residence
            var loan = SecuredFixedRateLoan.GetRandomLoanWithHistory(Person.Address, spCost, totalCost, (float) randRate,
                30, out var minPmt, Person.Personality);
            loan.Description = Mereo.GetMereoById(null, "30-Year Mortgage");
            loan.TradeLine.FormOfCredit = FormOfCredit.Mortgage;
            HomeDebt.Add(loan);
            return minPmt;
        }

        /// <summary>
        /// Creates random <see cref="CreditCardAccount"/> instances with a history and adds
        /// it to the <see cref="CreditCardDebt"/> collection.
        /// </summary>
        /// <param name="stdDevAsPercent"></param>
        protected internal void AddSingleCcDebt(double stdDevAsPercent = DF_STD_DEV_PERCENT)
        {
            //create random cc
            var ccAcct = CreditCardAccount.GetRandomCcAcct(Person, CreditScore);

            var pmtNote = Mereo.GetMereoById(ccAcct.Id);

            //1 year history
            var baseDate = DateTime.Today.AddYears(-1);

            //determine timespan for generated history
            var historyTs = DateTime.Today - baseDate;

            var ccAcctDueDoM = Etx.DiscreteRange(new[] {1, 15, 28});

            //create history
            for (var i = 1; i < historyTs.Days; i++)
            {
                var loopDt = baseDate.AddDays(i).Date;
                if (ccAcct.GetStatus(loopDt) == SpStatus.Closed)
                    break;

                //payments
                if (loopDt.Day == ccAcctDueDoM)
                {
                    //this is a negative amount
                    var minDue = ccAcct.GetMinPayment(loopDt);
                    if (minDue > new Pecuniam(10.0M).Neg)
                        minDue = new Pecuniam(10.0M).Neg;
                    if (Person.Personality.GetRandomActsIrresponsible())
                    {
                        var fowardDays = Etx.IntNumber(1, 45);
                        var paidDate = loopDt.AddDays(fowardDays);
                        ccAcct.Push(paidDate, minDue, pmtNote, Pecuniam.Zero);
                    }
                    else
                    {
                        var additionalPaid = Pecuniam.GetRandPecuniam(20, 200, 10);
                        ccAcct.Push(loopDt, minDue + additionalPaid, pmtNote, Pecuniam.Zero);
                    }
                }

                //purchases
                var remainingCredit = ccAcct.Max - ccAcct.GetValueAt(loopDt);

                if (ccAcct.GetStatus(loopDt) != SpStatus.Current || remainingCredit <= new Pecuniam(10))
                    continue;

                //TODO - need to replace 'Paycheck' with IReditus
                var p = Paycheck ?? Pecuniam.Zero;
                var scaler = remainingCredit > p ? p : remainingCredit;

                var daysMax = (double)scaler.Amount*DF_DAILY_SPEND_PERCENT/2;

                CreateSingleDaysPurchases(Person.Personality, ccAcct, loopDt,daysMax);
            }

            CreditCardDebt.Add(ccAcct);
        }

        /// <summary>
        /// Creates a random <see cref="FixedRateLoan"/> instance with a history and adds it to the 
        /// <see cref="VehicleDebt"/> collection.
        /// </summary>
        /// <param name="stdDevAsPercent"></param>
        /// <returns></returns>
        /// <remarks>
        /// Returns zero 9% of the time.
        /// [https://people.hofstra.edu/geotrans/eng/ch6en/conc6en/USAownershipcars.html]
        /// </remarks>
        protected internal Pecuniam AddVehicleLoan(double stdDevAsPercent = DF_STD_DEV_PERCENT)
        {
            //roll for no-car
            if (Etx.TryBelowOrAt(9, Etx.Dice.OneHundred))
                return Pecuniam.Zero;

            //calc a rand amount of what is still owed
            var randCarDebt = NorthAmericanFactors.GetRandomFactorValue(FactorTables.VehicleDebt, Factors.VehicleDebtFactor, stdDevAsPercent);

            //allow for car to be paid off
            if (Etx.TryBelowOrAt(23, Etx.Dice.OneHundred))
                randCarDebt = 0.0D;

            //calc rand amount of equity
            var randCarEquity = NorthAmericanFactors.GetRandomFactorValue(FactorTables.VehicleEquity, Factors.VehicleEquityFactor, stdDevAsPercent);

            _vehicleEquity += randCarEquity.ToPecuniam();

            //get rand interest rate weighted by score
            var randRate = CreditScore.GetRandomInterestRate(null, Gov.Fed.RiskFreeInterestRate.DF_VALUE + 3.2) * 0.01;
            var spCost = new Pecuniam((decimal)randCarDebt);
            var totalCost = new Pecuniam((decimal)(randCarDebt + randCarEquity));

            var loan = SecuredFixedRateLoan.GetRandomLoanWithHistory(new Gov.Nhtsa.Vin(), spCost, totalCost,
                (float) randRate, 5, out var minPmt, Person.Personality);

            //insure that the gen'ed history doesn't start before the year of make
            var maxYear = loan.TradeLine.OpennedDate.Year;
            var vin = Gov.Nhtsa.Vin.GetRandomVin(randCarDebt <= 2000.0D, maxYear);
            loan.PropertyId = vin;
            loan.Description = Mereo.GetMereoById(null, string.Join(" ", vin.Value, vin.Description, vin.GetModelYearYyyy()));
            loan.TradeLine.FormOfCredit = FormOfCredit.Installment;
            VehicleDebt.Add(loan);
            return minPmt;
        }

        /// <summary>
        /// Factory method to get a monthly bill amount at random
        /// </summary>
        /// <param name="name">The name of the bill (e.g. Electric, Gas, etc)</param>
        /// <param name="month">The month (1-12)</param>
        /// <param name="dailyPercent">
        /// The base percentage of the <see cref="Paycheck"/> around which 
        /// the random amounts are created.
        /// </param>
        /// <returns></returns>
        protected internal Pecuniam GetRandomMonthlyBillAmount(string name = null, int? month = null,
            double dailyPercent = DF_DAILY_SPEND_PERCENT)
        {
            var m = month.GetValueOrDefault(0);
            const StringComparison OPT = StringComparison.OrdinalIgnoreCase;
            var isUtilityBill = new[] {UTIL_ELEC, UTIL_GAS, UTIL_WATER}.Any(x => string.Equals(x, name, OPT));

            //reduce these 
            if (isUtilityBill)
            {
                dailyPercent = Math.Round(dailyPercent/4, 4);
            }
            if (string.Equals(name, TELCO, OPT))
            {
                dailyPercent = Math.Round(dailyPercent/3, 4);
            }

            var utilsDfMin = (int) Math.Round(Paycheck.ToDouble()*dailyPercent);
            var utilsDfMax = (int) Math.Round(Paycheck.ToDouble() * dailyPercent*2);
            var utilsDfMid = (int) Math.Round(((double) utilsDfMax - utilsDfMin)/2);

            var randBill = Pecuniam.GetRandPecuniam(utilsDfMin, utilsDfMax);

            //raise these by season-of-year
            if (string.Equals(name, UTIL_ELEC, OPT) && new[] {6, 7, 8}.Contains(m)
                || string.Equals(UTIL_GAS, name, OPT) && new[] {12, 1, 2}.Contains(m))
            {
                randBill += Pecuniam.GetRandPecuniam(utilsDfMin, utilsDfMid);
            }
            return randBill;
        }

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
        #endregion
    }
}
