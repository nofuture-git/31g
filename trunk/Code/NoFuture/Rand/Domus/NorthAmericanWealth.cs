using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Exceptions;
using NoFuture.Rand.Com;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Types;
using NoFuture.Shared;
using NoFuture.Util.Math;

namespace NoFuture.Rand.Domus
{
    /// <summary>
    /// For generating the financial state and history of a <see cref="NorthAmerican"/>
    /// at random.
    /// </summary>
    public class NorthAmericanWealth : Opes
    {
        #region innerTypes
        public enum FactorTables
        {
            HomeDebt,
            VehicleDebt,
            CreditCardDebt,
            CheckingAccount,
            SavingsAccount,
            NetWorth,
            VehicleEquity,
            HomeEquity
        }
        #endregion

        #region constants
        public const double DF_STD_DEV_PERCENT = 0.1285D;
        public const double DF_DAILY_SPEND_PERCENT = 0.05D;
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
        #endregion

        #region ctor

        /// <summary>
        /// Creates random transaction history.
        /// </summary>
        /// <param name="american"></param>
        /// <param name="isRenting">Optional, force the generated instance as renting (instead of mortgage)</param>
        public NorthAmericanWealth(NorthAmerican american, bool isRenting = false)
        {
            if(american == null)
                throw new ArgumentNullException(nameof(american));
            _amer = american;
            var usCityArea = _amer.Address.HomeCityArea as UsCityStateZip;
            if (usCityArea == null)
                return;

            CreditScore = new PersonalCreditScore(american);

            //determine if renting or own
            _isRenting = isRenting || GetIsLeaseResidence(usCityArea);

            var edu = _amer.Education?.EduLevel ?? (OccidentalEdu.HighSchool | OccidentalEdu.Grad);
            var race = _amer.Race;
            var region = usCityArea.State?.GetStateData()?.Region ?? AmericanRegion.Midwest;

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
                pecuniam => Math.Round((double)GetYearlyIncome(pecuniam).Amount / 12, 2);
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
        /// Creates a full Opes state and history at random.
        /// </summary>
        /// <param name="stdDevAsPercent"></param>
        public void CreateRandomAmericanOpes(double stdDevAsPercent = DF_STD_DEV_PERCENT)
        {
            //determine residence pmt
            _residencePmt += _isRenting
                ? AddRent(stdDevAsPercent)
                : AddMortgage(stdDevAsPercent) ?? Pecuniam.Zero;

            //determin car payment
            _carPmt += AddVehicleLoan(stdDevAsPercent) ?? Pecuniam.Zero;

            //add CC's
            AddTotalCcDebt(stdDevAsPercent);
            
            //add bank accounts
            AddBankingAccounts(stdDevAsPercent);
        }

        protected internal override FinancialData GetFinancialState(DateTime? dt = null)
        {
            var endDt = dt.GetValueOrDefault(DateTime.Now).Date.AddDays(1).AddMilliseconds(-1);
            var startDt = endDt.Date.AddYears(-1);
            var allPaymentsOut =
                CheckingAccounts.Select(x => x.Balance.GetDebitSum(new Tuple<DateTime, DateTime>(startDt, endDt)));
            var allPaymentsIn =
                CheckingAccounts.Select(x => x.Balance.GetCreditSum(new Tuple<DateTime, DateTime>(startDt, endDt)));
            var totalIncome = Pecuniam.Zero;
            var totalExpense = Pecuniam.Zero;
            foreach (var po in allPaymentsOut)
                totalExpense += po;
            foreach(var pi in allPaymentsIn)
                totalIncome += pi;

            var netConIncome = new NetConIncome
            {
                Revenue = totalIncome.Round,
                NetIncome = totalIncome.Abs.Round - totalExpense.Abs.Round
            };
            var netConAssets = new NetConAssets
            {
                TotalAssets = GetTotalCurrentWealth(endDt).Abs.Round,
                TotalLiabilities = GetTotalCurrentDebt(endDt).Abs.Round
            };

            return new FinancialData {Assets = netConAssets, Income = netConIncome};
        }

        protected internal override Pecuniam GetTotalCurrentWealth(DateTime? dt = null)
        {
            var tlt = Pecuniam.Zero;
            tlt += _homeEquity.Abs;
            tlt += _vehicleEquity.Abs;
            foreach (var da in CheckingAccounts)
                tlt += da.Value.Abs;
            foreach (var da in SavingAccounts)
                tlt += da.Value.Abs;

            return tlt;
        }

        protected internal bool GetIsLeaseResidence(UsCityStateZip usCityArea)
        {
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
        /// other <see cref="Opes"/> history.
        /// </summary>
        /// <param name="paycheck"></param>
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
            checking.PutCashIn(baseDate.AddDays(-1), randChecking.ToPecuniam(), "Init Checking");

            //set up a savings
            var randSavings = GetRandomFactorValue(FactorTables.SavingsAccount, _savingAcctFactor, stdDevAsPercent);
            var savings = SavingsAccount.GetRandomSavingAcct(_amer, baseDate);
            savings.PutCashIn(baseDate.AddDays(-1), randSavings.ToPecuniam(), "Init Savings");
            var totalDays = (DateTime.Today - baseDate).TotalDays;
            var friCounter = 0;
            for (var i = 0; i < totalDays; i++)
            {
                var loopDtSt = baseDate.AddDays(i).Date;

                //add pay every other friday
                if (loopDtSt.DayOfWeek == DayOfWeek.Friday)
                {
                    if (friCounter%2 == 0)
                    {
                        checking.PutCashIn(loopDtSt.AddSeconds(1), Paycheck.Abs, "Pay");
                    }
                    
                    //replenish savings
                    if (savings.Value < randSavings.ToPecuniam())
                        TransferFundsInBankAccounts(checking, savings, randSavings.ToPecuniam() - savings.Value,
                            loopDtSt.AddSeconds(15));
                    friCounter += 1;
                }

                //b-day money
                if (DateTime.Compare(loopDtSt,
                        new DateTime(loopDtSt.Year, _amer.BirthCert.DateOfBirth.Month, _amer.BirthCert.DateOfBirth.Day)) ==
                    0)
                {
                    checking.PutCashIn(loopDtSt.AddHours(15), Pecuniam.GetRandPecuniam(10,100,10), "b-day money");
                }

                //add house pmts
                checking.AddDebitTransactionsByDate(loopDtSt, HomeDebt);

                //add car pmts
                checking.AddDebitTransactionsByDate(loopDtSt, VehicleDebt);

                //add cc pmts
                checking.AddDebitTransactionsByDate(loopDtSt, CreditCardDebt);

                //add utility pmts
                PayUtilityBills(loopDtSt, checking);

                //if broke, move funds from savings and no spending
                if (checking.GetStatus(loopDtSt) != AccountStatus.Current)
                {
                    TransferFundsInBankAccounts(savings, checking, randChecking.ToPecuniam() - checking.Value,
                        loopDtSt.AddHours(19));
                    continue;
                }

                //create some checking account transactions 
                CreateSingleDaysPurchases(checking, loopDtSt);
            }
            SavingAccounts.Add(savings);
            CheckingAccounts.Add(checking);
        }

        protected internal void PayUtilityBills(DateTime loopDtSt, CheckingAccount checking)
        {
            var utils = new List<Tuple<string, int>>
            {
                new Tuple<string, int>("Electric", Etx.IntNumber(1, 28)),
                new Tuple<string, int>("Gas", Etx.IntNumber(1, 28)),
                new Tuple<string, int>("Water", Etx.IntNumber(1, 28)),
                new Tuple<string, int>("Telco", Etx.IntNumber(1, 28))
            };

            var utilsDfMin = (int)Math.Round((double)Paycheck.Amount * DF_DAILY_SPEND_PERCENT);
            var utilsDfMax = (int)Math.Round((double)Paycheck.Amount * DF_DAILY_SPEND_PERCENT*2);
            var utilsDfMid = (int)Math.Round(((double)utilsDfMax - utilsDfMin) / 2);

            //add utility pmts
            foreach (var t in utils)
            {
                if (loopDtSt.Day != t.Item2)
                    continue;

                var randAmt = Pecuniam.GetRandPecuniam(utilsDfMin, utilsDfMax);

                //raise these by season-of-year
                if (t.Item1 == "Electric" && new[] { 6, 7, 8 }.Contains(loopDtSt.Month)
                    || t.Item1 == "Gas" && new[] { 12, 1, 2 }.Contains(loopDtSt.Month))
                {
                    randAmt += Pecuniam.GetRandPecuniam(utilsDfMin, utilsDfMid);
                }
                checking.PutCashIn(loopDtSt.AddHours(12), randAmt, t.Item1);
            }
        }

        protected internal void TransferFundsInBankAccounts(DepositAccount fromAccount, DepositAccount toAccount, Pecuniam amt, DateTime dt)
        {
            if (fromAccount == null || toAccount == null || amt == null || amt == Pecuniam.Zero)
                return;
            if (fromAccount.GetStatus(dt) != AccountStatus.Current && toAccount.GetStatus(dt) != AccountStatus.Current)
                return;

            if (fromAccount.OpenDate < dt || toAccount.OpenDate < dt)
                return;
            amt = amt.Abs;

            while (fromAccount.Value < amt)
            {
                amt = amt/2.ToPecuniam();
                if (amt.Amount < 0.01M)
                    break;
            }
            fromAccount.TakeCashOut(dt, amt, GetPaymentNote(fromAccount.AccountNumber));
            toAccount.PutCashIn(dt.AddMilliseconds(100), amt, GetPaymentNote(toAccount.AccountNumber));
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
        /// it to the <see cref="Opes.HomeDebt"/> collection.
        /// </summary>
        /// <param name="stdDevAsPercent"></param>
        /// <remarks>
        /// src [http://www.nmhc.org/Content.aspx?id=4708]
        /// </remarks>
        protected internal Pecuniam AddRent(double stdDevAsPercent = DF_STD_DEV_PERCENT)
        {
            if (!_isRenting)
                return Pecuniam.Zero;
            //create a rent object
            var avgRent = (double)Rent.GetAvgAmericanRentByYear(null).Amount;

            var randRent =
                new Pecuniam(
                    (decimal) GetRandomFactorValue(FactorTables.HomeDebt, _homeDebtFactor, stdDevAsPercent, avgRent));
            var term = 12;
            var randDate = Etx.Date(0, DateTime.Today.AddDays(-2));
            var rent = new Rent(randDate, term, randRent, Etx.CoinToss ? new Pecuniam(250) : new Pecuniam(500))
            {
                Id = _amer.Address,
                Description = $"{term}-Month Lease"
            };
            _amer.Address.IsLeased = true;
            //create payment history until current
            var firstPmt = rent.GetMinPayment(randDate);
            rent.PayRent(randDate.AddDays(1), firstPmt);

            var rentDueDate = randDate.Month == 12
                ? new DateTime(randDate.Year + 1, 1, 1)
                : new DateTime(randDate.Year, randDate.Month + 1, 1);

            while (rentDueDate < DateTime.Today)
            {
                var paidRentOn = rentDueDate;
                //move the date rent was paid to some late-date when person acts irresponsible
                if (_amer.Personality.GetRandomActsIrresponsible())
                    paidRentOn = paidRentOn.AddDays(Etx.IntNumber(5, 15));

                rent.PayRent(paidRentOn, randRent, GetPaymentNote(rent.Id));
                rentDueDate = rentDueDate.AddMonths(1);
            }
            
            HomeDebt.Add(rent);
            return randRent;
        }

        /// <summary>
        /// Creates random <see cref="FixedRateLoan"/> instance with a history and adds
        /// it to the <see cref="Opes.HomeDebt"/> collection.
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
            Pecuniam minPmt;
            var loan = GetRandomLoanWithHistory(_amer.Address, spCost, totalCost, (float) randRate, 30, out minPmt);
            loan.Description = "30-Year Mortgage";
            loan.TradeLine.FormOfCredit = FormOfCredit.Mortgage;
            HomeDebt.Add(loan);
            return minPmt;
        }

        /// <summary>
        /// Creates random <see cref="CreditCardAccount"/> instances with a history and adds
        /// it to the <see cref="Opes.CreditCardDebt"/> collection.
        /// </summary>
        /// <param name="stdDevAsPercent"></param>
        /// <param name="maxCcDebt"></param>
        protected internal void AddSingleCcDebt(double stdDevAsPercent = DF_STD_DEV_PERCENT)
        {
            //create random cc
            var ccAcct = CreditCardAccount.GetRandomCcAcct(_amer, CreditScore);

            var pmtNote = GetPaymentNote(ccAcct.Id);

            //determine timespan for generated history
            var historyTs = DateTime.Now - ccAcct.Cc.CardHolderSince;

            //create history
            for (var i = 1; i < historyTs.Days; i++)
            {
                var loopDt = ccAcct.Cc.CardHolderSince.AddDays(i);
                if (ccAcct.GetStatus(loopDt) == AccountStatus.Closed)
                    break;

                CreateSingleDaysPurchases(ccAcct, loopDt);

                if (i%30 != 0 || ccAcct.GetCurrentBalance(loopDt).Amount < 0.0M)
                    continue;

                var minDue = ccAcct.GetMinPayment(loopDt);
                if(minDue < new Pecuniam(10.0M))
                    minDue = new Pecuniam(10.0M);
                if (_amer.Personality.GetRandomActsIrresponsible())
                {
                    var fowardDays = Etx.IntNumber(1, 45);
                    var paidDate = loopDt.AddDays(fowardDays);
                    ccAcct.PutCashIn(paidDate, minDue, pmtNote);
                    i += fowardDays;
                }
                else
                {
                    var additionalPaid = Pecuniam.GetRandPecuniam(20, 200, 10);
                    ccAcct.PutCashIn(loopDt, minDue + additionalPaid, pmtNote);
                }
            }

            CreditCardDebt.Add(ccAcct);
        }

        /// <summary>
        /// Creates a random <see cref="FixedRateLoan"/> instance with a history and adds it to the 
        /// <see cref="Opes.VehicleDebt"/> collection.
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

            var vin = Gov.Nhtsa.Vin.GetRandomVin(randCarDebt <= 2000.0D);

            //this would never happen
            while (_amer.MyGender == Gender.Male && vin.Description == "Toyota Yaris")
                vin = Gov.Nhtsa.Vin.GetRandomVin(randCarDebt <= 2000.0D);

            //calc rand amount of equity
            var randCarEquity = GetRandomFactorValue(FactorTables.VehicleEquity, _vehicleEquityFactor, stdDevAsPercent);

            _vehicleEquity += randCarEquity.ToPecuniam();

            //get rand interest rate weighted by score
            var randRate = CreditScore.GetRandomInterestRate(null, Gov.Fed.RiskFreeInterestRate.DF_VALUE + 3.2) * 0.01;
            var spCost = new Pecuniam((decimal)randCarDebt);
            var totalCost = new Pecuniam((decimal)(randCarDebt + randCarEquity));

            Pecuniam minPmt;
            var loan = GetRandomLoanWithHistory(vin, spCost, totalCost, (float)randRate, 5, out minPmt);
            loan.Description = string.Join(" ", vin.Value, vin.Description, vin.GetModelYearYyyy());
            loan.TradeLine.FormOfCredit = FormOfCredit.Installment;
            VehicleDebt.Add(loan);
            return minPmt;
        }

        /// <summary>
        /// Produces a <see cref="SecuredFixedRateLoan"/> with history.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="remainingCost"></param>
        /// <param name="totalCost"></param>
        /// <param name="rate"></param>
        /// <param name="termInYears"></param>
        /// <param name="minPmt"></param>
        /// <returns></returns>
        protected internal SecuredFixedRateLoan GetRandomLoanWithHistory(Identifier property, Pecuniam remainingCost,
            Pecuniam totalCost, float rate, int termInYears, out Pecuniam minPmt)
        {
            //if no or nonsense values given, change to some default
            if (totalCost == null || totalCost < Pecuniam.Zero)
                totalCost = new Pecuniam(2000);
            if(remainingCost == null || remainingCost < Pecuniam.Zero)
                remainingCost = Pecuniam.Zero;

            rate = Math.Abs(rate);

            //calc the monthly payment
            var fv = remainingCost.Amount.PerDiemInterest(rate, Constants.TropicalYear.TotalDays* termInYears);
            minPmt = new Pecuniam(Math.Round(fv/(termInYears * 12), 2));
            var minPmtRate = fv == 0 ? CreditCardAccount.DF_MIN_PMT_RATE : (float) Math.Round(minPmt.Amount/fv, 6);

            //given this value and rate - calc the timespan needed to have aquired this amount of equity
            var firstOfYear = new DateTime(DateTime.Today.Year, 1, 1);
            var loan = new SecuredFixedRateLoan(property, firstOfYear, minPmtRate, totalCost)
            {
                Rate = rate
            };

            var dtIncrement = firstOfYear.AddMonths(1);
            while (loan.GetCurrentBalance(dtIncrement) > remainingCost)
            {
                if (dtIncrement > DateTime.Now)
                    break;
                loan.PutCashIn(dtIncrement, minPmt);
                dtIncrement = dtIncrement.AddMonths(1);
            }

            //repeat process from calc'ed past date to create a history
            var calcPurchaseDt = DateTime.Today.AddDays(-1*(dtIncrement - firstOfYear).Days);
            loan = new SecuredFixedRateLoan(property, calcPurchaseDt, minPmtRate, totalCost)
            {
                Rate = rate
            };

            var pmtNote = GetPaymentNote(property);

            dtIncrement = calcPurchaseDt.AddMonths(1);
            while (loan.GetCurrentBalance(dtIncrement) > remainingCost)
            {
                var paidOnDate = dtIncrement;
                if (_amer.Personality.GetRandomActsIrresponsible())
                    paidOnDate = paidOnDate.AddDays(Etx.IntNumber(5, 15));

                //is this the payoff
                var isPayoff = loan.GetCurrentBalance(dtIncrement) <= minPmt;
                if (isPayoff)
                    minPmt = loan.GetCurrentBalance(dtIncrement);

                loan.PutCashIn(paidOnDate, minPmt, pmtNote);
                if (isPayoff)
                    break;

                dtIncrement = dtIncrement.AddMonths(1);
            }

            //assign boilerplate props
            loan.Lender = Bank.GetRandomBank(_amer?.Address?.HomeCityArea);
            return loan;
        }

        /// <summary>
        /// Calc a yearly income salary or pay at random.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="factorCalc">
        /// Optional, allows caller to specify how to factor the raw results of 
        /// <see cref="GetAvgEarningPerYear"/> for the <see cref="dt"/>.
        /// </param>
        /// <param name="stdDevInUsd"></param>
        /// <param name="dt">
        /// Optional, date used for solving the <see cref="GetAvgEarningPerYear"/> eq.
        /// </param>
        /// <returns></returns>
        protected internal Pecuniam GetYearlyIncome(Pecuniam min, Func<double, double, double> factorCalc = null,
            double stdDevInUsd = 2000, DateTime? dt = null)
        {
            if(min == null)
                min = Pecuniam.Zero;
            //get linear eq for earning 
            var eq = GetAvgEarningPerYear();
            if (eq == null)
                return Pecuniam.Zero;
            var baseValue = Math.Round(eq.SolveForY(dt.GetValueOrDefault(DateTime.Today).ToDouble()), 2);
            if (baseValue <= 0)
                return Pecuniam.Zero;

            Func<double, double, double> dfFunc = (d, d1) =>
            {
                var factorVal = (d/5)*d1;
                while (Math.Abs(factorVal) > d)
                    factorVal = factorVal/2;
                return factorVal;
            };

            factorCalc = factorCalc ?? dfFunc;

            var factorValue = factorCalc(baseValue, _netWorthFactor);

            baseValue = Math.Round(baseValue + factorValue, 2);

            var randValue = Math.Round(
                Etx.RandomValueInNormalDist(Math.Round(baseValue, 0), stdDevInUsd), 2);
            return new Pecuniam((decimal) randValue) + min;
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
        protected internal double GetRandomFactorValue(FactorTables factor, double factorMultiplier,
            double stdDevAsPercent, double? assignedBase = null)
        {
            var baseValue = assignedBase.GetValueOrDefault(GetFactorBaseValue(factor));
            baseValue = Math.Round(baseValue + baseValue*factorMultiplier, 2);
            var randValue = Math.Round(
                Etx.RandomValueInNormalDist(Math.Round(baseValue, 0), Math.Round(baseValue*stdDevAsPercent, 0)), 2);
            return randValue;
        }

        /// <summary>
        /// Creates purchase transactions on <see cref="t"/> at random for the given <see cref="ccDate"/>.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ccDate"></param>
        /// <param name="daysMax"></param>
        /// <param name="randMaxFactor">
        /// The multiplier used for the rand dollar's max, raising this value 
        /// will raise every transactions possiable max by a factor of this.
        /// </param>
        protected internal void CreateSingleDaysPurchases(ITransactionable t, DateTime ccDate, int randMaxFactor = 10)
        {
            var daysMax = (double) Paycheck.Amount*DF_DAILY_SPEND_PERCENT;
            //build charges history
            var keepSpending = true;
            var spentSum = new Pecuniam(0);

            while (keepSpending)//want possiable multiple transactions per day
            {
                //if we reached target then exit 
                if (spentSum >= new Pecuniam((decimal)daysMax))
                    return;

                var isXmasSeason = ccDate.Month >= 11 && ccDate.Day >= 20;
                var isWeekend = ccDate.DayOfWeek == DayOfWeek.Friday ||
                                ccDate.DayOfWeek == DayOfWeek.Saturday ||
                                ccDate.DayOfWeek == DayOfWeek.Sunday;
                var actingIrresp = _amer.Personality.GetRandomActsIrresponsible();
                var isbigTicketItem = Etx.TryAboveOrAt(96, Etx.Dice.OneHundred);
                var isSomeEvenAmt = Etx.TryBelowOrAt(3, Etx.Dice.Ten);

                //keep times during normal waking hours
                var randCcDate =
                        ccDate.Date.AddHours(Etx.IntNumber(6, isWeekend ? 23 : 19))
                            .AddMinutes(Etx.IntNumber(0, 59))
                            .AddSeconds(Etx.IntNumber(0, 59))
                            .AddMilliseconds(Etx.IntNumber(0,999));

                //make purchase based various factors
                var v = 2;
                v = isXmasSeason ? v + 1 : v;
                v = isWeekend ? v + 2 : v;
                v = actingIrresp ? v + 3 : v;
                randMaxFactor = isbigTicketItem ? randMaxFactor * 10 : randMaxFactor;

                if (Etx.TryBelowOrAt(v, Etx.Dice.Ten))
                {
                    //create some random purchase amount
                    var chargeAmt = Pecuniam.GetRandPecuniam(5, v*randMaxFactor, isSomeEvenAmt ? 10 : 0);

                    //check if account is maxed-out\empty
                    if (!t.TakeCashOut(randCcDate, chargeAmt))
                        return;

                    spentSum += chargeAmt;
                }
                //determine if more transactions for this day
                keepSpending = Etx.CoinToss;
            }
        }

        /// <summary>
        /// Get the linear eq of the city if its found otherwise
        /// defaults to the state.
        /// </summary>
        /// <returns></returns>
        protected internal LinearEquation GetAvgEarningPerYear()
        {
            var ca = _amer.Address?.HomeCityArea as UsCityStateZip;
            return ca?.AverageEarnings ?? ca?.State?.GetStateData()?.AverageEarnings;
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
        /// <returns></returns>
        internal static double GetFactor(FactorTables tbl, OccidentalEdu edu, NorthAmericanRace race,
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
                var xmlElem = xmlDoc.SelectSingleNode($"{tblXPath}/factor[@name='{factor}']/add[@name='{hash[factor]}']") as XmlElement;
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
                int min, max;
                if (!int.TryParse(minAge, out min) || !int.TryParse(maxAge, out max))
                    continue;
                var isInRange = age >= min && age <= max;
                if (!isInRange || string.IsNullOrWhiteSpace(ageElem.Attributes["value"]?.Value))
                    continue;
                var factorVal = ageElem.Attributes["value"].Value;
                double dblOut;
                if (double.TryParse(factorVal, out dblOut))
                    sum += dblOut;
            }
            return sum;
        }

        /// <summary>
        /// Gets the base dollar value of the given factor
        /// </summary>
        /// <param name="tbl"></param>
        /// <returns></returns>
        internal static double GetFactorBaseValue(FactorTables tbl)
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
            double dblOut;
            if (!double.TryParse(tblNode.Attributes["value"].Value, out dblOut))
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
                (short)edu < (short)(OccidentalEdu.Post | OccidentalEdu.Grad))
                eduName = "Bachelor";
            if ((short)edu >= (short)(OccidentalEdu.Post | OccidentalEdu.Grad))
                eduName = "PostGrad";
            return eduName;
        }

        internal static string GetPaymentNote(Identifier property, string prefix = null)
        {
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
        #endregion
    }

}
