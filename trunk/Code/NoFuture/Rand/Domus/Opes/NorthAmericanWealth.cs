using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Cc;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Domus.Opes
{
    /// <summary>
    /// For generating the financial state and history of a <see cref="NorthAmerican"/>
    /// at random.
    /// </summary>
    [Serializable]
    public class NorthAmericanWealth : WealthBase
    {
        #region innerTypes

        #endregion

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
        private readonly NorthAmerican _amer;
        private readonly bool _isRenting;

        private readonly double _homeDebtFactor;
        private readonly double _vehicleDebtFactor;
        private readonly double _ccDebtFactor;
        private readonly double _netWorthFactor;
        private readonly double _homeEquityFactor;
        private readonly double _vehicleEquityFactor;

        private readonly double _checkingAcctFactor;
        private readonly double _savingAcctFactor;

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
        public NorthAmericanWealth(NorthAmerican american, bool isRenting = false)
        {
            _amer = american ?? throw new ArgumentNullException(nameof(american));
            var usCityArea = _amer?.Address?.HomeCityArea as UsCityStateZip;

            CreditScore = new PersonalCreditScore(american);

            //determine if renting or own
            _isRenting = isRenting || GetIsLeaseResidence(usCityArea);

            var edu = _amer.Education?.EduFlag ?? (OccidentalEdu.HighSchool | OccidentalEdu.Grad);
            var race = _amer.Race;
            var region = usCityArea?.State?.GetStateData()?.Region ?? AmericanRegion.Midwest;

            _homeDebtFactor = GetFactor(FactorTables.HomeDebt, edu, race, region, _amer.Age, _amer.MyGender,
                _amer.MaritialStatus);
            _vehicleDebtFactor = GetFactor(FactorTables.VehicleDebt, edu, race, region, _amer.Age, _amer.MyGender,
                _amer.MaritialStatus);
            _ccDebtFactor = GetFactor(FactorTables.CreditCardDebt, edu, race, region, _amer.Age, _amer.MyGender,
                _amer.MaritialStatus);
            _savingAcctFactor = GetFactor(FactorTables.SavingsAccount, edu, race, region, _amer.Age, _amer.MyGender,
                _amer.MaritialStatus);
            _checkingAcctFactor = GetFactor(FactorTables.CheckingAccount, edu, race, region, _amer.Age, _amer.MyGender,
                _amer.MaritialStatus);
            _netWorthFactor = GetFactor(FactorTables.NetWorth, edu, race, region, _amer.Age, _amer.MyGender,
                _amer.MaritialStatus);
            _homeEquityFactor = GetFactor(FactorTables.HomeEquity, edu, race, region, _amer.Age, _amer.MyGender,
                _amer.MaritialStatus);
            _vehicleEquityFactor = GetFactor(FactorTables.VehicleEquity, edu, race, region, _amer.Age, _amer.MyGender,
                _amer.MaritialStatus);

            var payBase = new Pecuniam(2000);
            Func<Pecuniam, double> calcMonthlyPay =
                pecuniam => Math.Round((double)GetYearlyIncome(pecuniam, _netWorthFactor).Amount / 12, 2);
            Paycheck = Math.Round(calcMonthlyPay(payBase) / 2, 2).ToPecuniam();
        }
        #endregion

        #region properties
        public CreditScore CreditScore { get; }
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
            _residencePmt += _isRenting
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
        /// Produces the common <see cref="FinancialData"/> for the given date
        /// </summary>
        /// <param name="dt">Optional, null will be for all dates</param>
        /// <returns></returns>
        protected internal override FinancialData GetFinancialState(DateTime? dt = null)
        {
            var endDt = dt.GetValueOrDefault(DateTime.Now).Date.AddDays(1).AddMilliseconds(-1);
            var startDt = CheckingAccounts.Select(x => x.Inception).Min();

            //get all money paid out
            var allPaymentsOut =
                CheckingAccounts.Select(x => x.Balance.GetDebitSum(new Tuple<DateTime, DateTime>(startDt, endDt)))
                    .ToList();
            allPaymentsOut.AddRange(
                SavingAccounts.Select(x => x.Balance.GetDebitSum(new Tuple<DateTime, DateTime>(startDt, endDt))));

            //get all money paid in
            var allPaymentsIn =
                CheckingAccounts.Select(x => x.Balance.GetCreditSum(new Tuple<DateTime, DateTime>(startDt, endDt)))
                    .ToList();
            allPaymentsIn.AddRange(
                SavingAccounts.Select(x => x.Balance.GetCreditSum(new Tuple<DateTime, DateTime>(startDt, endDt))));

            //sum of income and expense
            var totalIncome = new Pecuniam(allPaymentsIn.Sum(x => x.Amount));
            var totalExpense = new Pecuniam(allPaymentsOut.Sum(x => x.Amount));

            var netConIncome = new IncomeSummary
            {
                Revenue = totalIncome.Round,
                NetIncome = totalIncome.Abs.Round - totalExpense.Abs.Round
            };
            var netConAssets = new AssetsSummary
            {
                TotalAssets = GetTotalCurrentWealth(endDt).Abs.Round,
                TotalLiabilities = GetTotalCurrentDebt(endDt).Abs.Round
            };

            return new FinancialData {AssetsSummary = netConAssets, IncomeSummary = netConIncome};
        }

        /// <summary>
        /// Gets the total value of all assests
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected internal override Pecuniam GetTotalCurrentWealth(DateTime? dt = null)
        {
            var tlt = Pecuniam.Zero;
            tlt += _homeEquity.Abs;
            tlt += _vehicleEquity.Abs;
            foreach (var da in CheckingAccounts)
                tlt += da.CurrentValue.Abs;
            foreach (var da in SavingAccounts)
                tlt += da.CurrentValue.Abs;

            return tlt;
        }

        /// <summary>
        /// Determine if the given <see cref="NorthAmerican"/> is renting or has a mortgage
        /// </summary>
        /// <param name="usCityArea"></param>
        /// <returns></returns>
        protected internal bool GetIsLeaseResidence(UsCityStateZip usCityArea)
        {
            if (usCityArea == null)
                return true;

            var cannotGetFinanced = CreditScore.GetRandomInterestRate(null, Gov.Fed.RiskFreeInterestRate.DF_VALUE) > 8.5;
            if (cannotGetFinanced)
                return true;

            var livesInDenseUrbanArea = usCityArea.Msa?.MsaType == (UrbanCentric.City | UrbanCentric.Large);
            var isYoung = _amer.GetAgeAt(null) < 32;
            var roll = 65;
            if (livesInDenseUrbanArea)
                roll -= 23;
            //is scaled where 29 year-old loses 3 while 21 year-old loses 11
            if (isYoung)
                roll -= 32 - _amer.GetAgeAt(null);
            return Etx.TryBelowOrAt(roll, Etx.Dice.OneHundred);
        }

        /// <summary>
        /// Creates random checking and savings accounts with a history which is intertwined with 
        /// other <see cref="WealthBase"/> history.
        /// </summary>
        /// <param name="stdDevAsPercent"></param>
        protected internal void AddBankingAccounts(double stdDevAsPercent = DF_STD_DEV_PERCENT)
        {
            if(Paycheck == null)
                throw new ArgumentNullException(nameof(Paycheck));

            //1 year history
            var baseDate = DateTime.Today.AddYears(-1);

            //set up checking
            var checking = CheckingAccount.GetRandomCheckingAcct(_amer,baseDate);
            var randChecking = GetRandomFactorValue(FactorTables.CheckingAccount, _checkingAcctFactor, stdDevAsPercent);
            checking.Push(baseDate.AddDays(-1), randChecking.ToPecuniam(), Mereo.GetMereoById(null, "Init Checking"), Pecuniam.Zero);

            //set up a savings
            var randSavings = GetRandomFactorValue(FactorTables.SavingsAccount, _savingAcctFactor, stdDevAsPercent);
            var savings = SavingsAccount.GetRandomSavingAcct(_amer, baseDate);
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
                    if (savings.CurrentValue < randSavings.ToPecuniam())
                        DepositAccount.TransferFundsInBankAccounts(checking, savings, randSavings.ToPecuniam() - savings.CurrentValue,
                            loopDtSt.AddSeconds(15));
                    friCounter += 1;
                }

                //b-day money
                if (DateTime.Compare(loopDtSt,
                        new DateTime(loopDtSt.Year, _amer.BirthCert.DateOfBirth.Month, _amer.BirthCert.DateOfBirth.Day)) ==
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
                        randChecking.ToPecuniam() - checking.CurrentValue,
                        loopDtSt.AddHours(19));
                    continue;
                }

                var myPecuniam = currentBalance > Paycheck ? currentBalance : Paycheck;
                var dailyMax = (double) myPecuniam.Amount*DF_DAILY_SPEND_PERCENT;

                CreateSingleDaysPurchases(_amer.Personality, checking, loopDtSt, dailyMax);
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
            if (!(_amer.Personality.Conscientiousness.Value.Zscore < 0.5D))
                return;

            //get random total cc debt
            var randCcValue = GetRandomFactorValue(FactorTables.CreditCardDebt, _ccDebtFactor, stdDevAsPercent);

            //get number of cc's to divide total cc debt into
            var numOfCc = 1;
            var roll = 100;
            if (_amer.Age >= 30)
            {
                numOfCc += 1;
                roll = 13;
            }
            if (_amer.Age >= 47 && _amer.Age < 66)
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
        /// it to the <see cref="WealthBase.HomeDebt"/> collection.
        /// </summary>
        /// <param name="stdDevAsPercent"></param>
        /// <remarks>
        /// src [http://www.nmhc.org/Content.aspx?id=4708]
        /// </remarks>
        protected internal Pecuniam AddRent(double stdDevAsPercent = DF_STD_DEV_PERCENT)
        {
            if (!_isRenting)
                return Pecuniam.Zero;

            var homeDebtFactor = _homeDebtFactor;

            var rent = Rent.GetRandomRentWithHistory(_amer.Address, homeDebtFactor, _amer.Personality, stdDevAsPercent);
            var randRent = rent.MonthlyPmt;

            HomeDebt.Add(rent);
            return randRent;
        }

        /// <summary>
        /// Creates random <see cref="FixedRateLoan"/> instance with a history and adds
        /// it to the <see cref="WealthBase.HomeDebt"/> collection.
        /// </summary>
        /// <param name="stdDevAsPercent"></param>
        protected internal Pecuniam AddMortgage(double stdDevAsPercent = DF_STD_DEV_PERCENT)
        {
            //calc a rand amount of what is still owed
            var randHouseDebt = GetRandomFactorValue(FactorTables.HomeDebt, _homeDebtFactor, stdDevAsPercent);

            //calc rand amount of equity accrued
            var randHouseEquity = GetRandomFactorValue(FactorTables.HomeEquity, _homeEquityFactor, stdDevAsPercent);
            _homeEquity += randHouseEquity.ToPecuniam();

            //get rand interest rate weighted by score
            var randRate = CreditScore.GetRandomInterestRate(null, Gov.Fed.RiskFreeInterestRate.DF_VALUE) * 0.01;
            var spCost = new Pecuniam((decimal)randHouseDebt);
            var totalCost = new Pecuniam((decimal)(randHouseDebt + randHouseEquity));

            //create a loan on current residence
            var loan = SecuredFixedRateLoan.GetRandomLoanWithHistory(_amer.Address, spCost, totalCost, (float) randRate,
                30, out var minPmt, _amer.Personality);
            loan.Description = Mereo.GetMereoById(null, "30-Year Mortgage");
            loan.TradeLine.FormOfCredit = FormOfCredit.Mortgage;
            HomeDebt.Add(loan);
            return minPmt;
        }

        /// <summary>
        /// Creates random <see cref="CreditCardAccount"/> instances with a history and adds
        /// it to the <see cref="WealthBase.CreditCardDebt"/> collection.
        /// </summary>
        /// <param name="stdDevAsPercent"></param>
        protected internal void AddSingleCcDebt(double stdDevAsPercent = DF_STD_DEV_PERCENT)
        {
            //create random cc
            var ccAcct = CreditCardAccount.GetRandomCcAcct(_amer, CreditScore);

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
                    if (_amer.Personality.GetRandomActsIrresponsible())
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

                var scaler = remainingCredit > Paycheck ? Paycheck : remainingCredit;

                var daysMax = (double)scaler.Amount*DF_DAILY_SPEND_PERCENT/2;

                CreateSingleDaysPurchases(_amer.Personality, ccAcct, loopDt,daysMax);
            }

            CreditCardDebt.Add(ccAcct);
        }

        /// <summary>
        /// Creates a random <see cref="FixedRateLoan"/> instance with a history and adds it to the 
        /// <see cref="WealthBase.VehicleDebt"/> collection.
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
            var randCarDebt = GetRandomFactorValue(FactorTables.VehicleDebt, _vehicleDebtFactor, stdDevAsPercent);

            //allow for car to be paid off
            if (Etx.TryBelowOrAt(23, Etx.Dice.OneHundred))
                randCarDebt = 0.0D;

            //calc rand amount of equity
            var randCarEquity = GetRandomFactorValue(FactorTables.VehicleEquity, _vehicleEquityFactor, stdDevAsPercent);

            _vehicleEquity += randCarEquity.ToPecuniam();

            //get rand interest rate weighted by score
            var randRate = CreditScore.GetRandomInterestRate(null, Gov.Fed.RiskFreeInterestRate.DF_VALUE + 3.2) * 0.01;
            var spCost = new Pecuniam((decimal)randCarDebt);
            var totalCost = new Pecuniam((decimal)(randCarDebt + randCarEquity));

            var loan = SecuredFixedRateLoan.GetRandomLoanWithHistory(new Gov.Nhtsa.Vin(), spCost, totalCost,
                (float) randRate, 5, out var minPmt, _amer.Personality);

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

            var utilsDfMin = (int) Math.Round((double) Paycheck.Amount*dailyPercent);
            var utilsDfMax = (int) Math.Round((double) Paycheck.Amount*dailyPercent*2);
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

        /// <summary>
        /// Get a random value for the given <see cref="factor"/> table.
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="factorMultiplier"></param>
        /// <param name="stdDevAsPercent"></param>
        /// <param name="assignedBase">
        /// Optional value to directly assign a factor table's base value, defaults to 
        /// the value from <see cref="GetFactorBaseValue"/>
        /// </param>
        /// <returns></returns>
        public static double GetRandomFactorValue(FactorTables factor, double factorMultiplier,
            double stdDevAsPercent, double? assignedBase = null)
        {
            var baseValue = assignedBase.GetValueOrDefault(GetFactorBaseValue(factor));
            baseValue = Math.Round(baseValue + baseValue*factorMultiplier, 2);
            var randValue = Math.Round(
                Etx.RandomValueInNormalDist(Math.Round(baseValue, 0), Math.Round(baseValue*stdDevAsPercent, 0)), 2);
            return randValue;
        }

        /// <summary>
        /// Get the linear eq of the city if its found otherwise
        /// defaults to the state.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// compiled data from BEA
        /// </remarks>
        protected internal override LinearEquation GetAvgEarningPerYear()
        {
            var ca = _amer.Address?.HomeCityArea as UsCityStateZip;
            return (ca?.AverageEarnings ?? ca?.State?.GetStateData()?.AverageEarnings) ?? NAmerUtil.Equations.NatlAverageEarnings;
        }

        /// <summary>
        /// Gets the sum of the factors based on the criteria.
        /// </summary>
        /// <param name="tbl"></param>
        /// <param name="edu"></param>
        /// <param name="race"></param>
        /// <param name="region"></param>
        /// <param name="age"></param>
        /// <param name="gender"></param>
        /// <param name="maritialStatus"></param>
        /// <returns>
        /// A scalar factor above or below the national average(s) based on the given criteria
        /// </returns>
        /// <remarks>
        /// src http://www.census.gov/people/wealth/files/Debt_Tables_2011.xlsx
        ///     http://www.census.gov/people/wealth/files/Wealth_Tables_2011.xlsx
        /// </remarks>
        public static double GetFactor(FactorTables tbl, OccidentalEdu edu, NorthAmericanRace race,
            AmericanRegion region, int age, Gender gender, MaritialStatus maritialStatus)
        {
            var xmlDoc = tbl == FactorTables.CreditCardDebt || tbl == FactorTables.HomeDebt ||
                         tbl == FactorTables.VehicleDebt
                ? TreeData.UsPersonalDebt
                : TreeData.UsPersonalWealth;
            var tblName = Enum.GetName(typeof(FactorTables), tbl);
            var eduName = GetXmlEduName(edu);
            var raceName = Enum.GetName(typeof(NorthAmericanRace), race);
            var regionName = Enum.GetName(typeof(AmericanRegion), region);
            var genderName = Enum.GetName(typeof(Gender), gender);

            var tblXPath = $"//table[@name='{tblName}']";

            var sum = 0.0D;
            var hash = new Dictionary<string, string>
            {
                {"Edu", eduName},
                {"Race", raceName},
                {"Region", regionName},
            };

            foreach (var factor in hash.Keys)
            {
                var xmlElem =
                    xmlDoc.SelectSingleNode($"{tblXPath}/factor[@name='{factor}']/add[@name='{hash[factor]}']") as
                        XmlElement;
                var factorVal = xmlElem?.Attributes["value"]?.Value;
                if (string.IsNullOrWhiteSpace(factorVal))
                    continue;
                double dblOut;
                if (double.TryParse(factorVal, out dblOut))
                    sum += dblOut;
            }

            var ageNode = maritialStatus == MaritialStatus.Remarried || maritialStatus == MaritialStatus.Remarried
                ? xmlDoc.SelectSingleNode($"{tblXPath}/factor[@name='Age']/factor[@name='Married']")
                : xmlDoc.SelectSingleNode($"{tblXPath}/factor[@name='Age']/factor[@name='{genderName}']");
            if (ageNode == null)
                return sum;
            foreach (var anode in ageNode.ChildNodes)
            {
                var ageElem = anode as XmlElement;
                if (ageElem == null)
                    continue;
                var minAge = ageElem.Attributes["min"]?.Value;
                var maxAge = ageElem.Attributes["max"]?.Value;
                if (string.IsNullOrWhiteSpace(minAge) || string.IsNullOrWhiteSpace(maxAge))
                    continue;
                if (!int.TryParse(minAge, out var min) || !int.TryParse(maxAge, out var max))
                    continue;
                var isInRange = age >= min && age <= max;
                if (!isInRange || string.IsNullOrWhiteSpace(ageElem.Attributes["value"]?.Value))
                    continue;
                var factorVal = ageElem.Attributes["value"].Value;
                if (double.TryParse(factorVal, out var dblOut))
                    sum += dblOut;
            }
            return sum;
        }

        /// <summary>
        /// Gets the base dollar value of the given factor
        /// </summary>
        /// <param name="tbl"></param>
        /// <returns></returns>
        public static double GetFactorBaseValue(FactorTables tbl)
        {
            var xmlDoc = tbl == FactorTables.CreditCardDebt || tbl == FactorTables.HomeDebt ||
                        tbl == FactorTables.VehicleDebt
               ? TreeData.UsPersonalDebt
               : TreeData.UsPersonalWealth;
            var tblName = Enum.GetName(typeof(FactorTables), tbl);
            var tblXPath = $"//table[@name='{tblName}']";
            var tblNode = xmlDoc.SelectSingleNode(tblXPath) as XmlElement;
            if (string.IsNullOrWhiteSpace(tblNode?.Attributes?["value"]?.Value))
                return 0.0D;
            if (!double.TryParse(tblNode.Attributes["value"].Value, out var dblOut))
                return 0.0D;
            return dblOut;
        }

        internal static string GetXmlEduName(OccidentalEdu edu)
        {
            var eduName = "High School";
            if ((short)edu < (short)(OccidentalEdu.HighSchool | OccidentalEdu.Some))
                eduName = "DropOut";
            if ((short)edu > (short)(OccidentalEdu.HighSchool | OccidentalEdu.Grad) &&
                (short)edu < (short)(OccidentalEdu.Assoc | OccidentalEdu.Grad))
                eduName = "Some College, No Degree";
            if ((short)edu >= (short)(OccidentalEdu.Assoc | OccidentalEdu.Grad) &&
                (short)edu < (short)(OccidentalEdu.Bachelor | OccidentalEdu.Grad))
                eduName = "Associate";
            if ((short)edu >= (short)(OccidentalEdu.Bachelor | OccidentalEdu.Grad) &&
                (short)edu < (short)(OccidentalEdu.Master | OccidentalEdu.Grad))
                eduName = "Bachelor";
            if ((short)edu >= (short)(OccidentalEdu.Master | OccidentalEdu.Grad))
                eduName = "PostGrad";
            return eduName;
        }
        #endregion
    }
}
