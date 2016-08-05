using System;
using System.Collections.Generic;
using System.Xml;
using NoFuture.Rand.Com;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Types;
using NoFuture.Shared;
using NoFuture.Util.Math;

namespace NoFuture.Rand.Domus
{
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

        #region fields
        private readonly NorthAmerican _amer;
        private readonly OccidentalEdu _edu;
        private readonly NorthAmericanRace _race;
        private readonly AmericanRegion _region;
        private readonly UsCityStateZip _usCityArea;

        private readonly double _homeDebtFactor;
        private readonly double _vehicleDebtFactor;
        private readonly double _ccDebtFactor;
        private readonly double _netWorthFactor;
        private readonly double _homeEquityFactor;

        private readonly double _checkingAcctFactor;
        private readonly double _savingAcctFactor;
        #endregion

        #region ctor
        /// <summary>
        /// Creates random transaction history.
        /// </summary>
        /// <param name="american"></param>
        /// <remarks>
        /// http://www.nmhc.org/Content.aspx?id=4708
        /// </remarks>
        public NorthAmericanWealth(NorthAmerican american)
        {
            _amer = american;
            _usCityArea = _amer.Address.HomeCityArea as UsCityStateZip;
            if (_usCityArea == null)
                return;
            CreditScore = new PersonalCreditScore(american);

            //simple split for renter-to-own when over 27
            IsRenting = _amer.GetAgeAt(null) <= 27 || Etx.TryBelowOrAt(65, Etx.Dice.OneHundred);

            _edu = _amer.Education?.EduLevel ?? (OccidentalEdu.HighSchool | OccidentalEdu.Grad);
            _race = _amer.Race;
            _region = _usCityArea.State?.GetStateData()?.Region ?? AmericanRegion.Midwest;

            _homeDebtFactor = GetFactor(FactorTables.HomeDebt, _edu, _race, _region, _amer.Age, _amer.MyGender,
                _amer.MaritialStatus);
            _vehicleDebtFactor = GetFactor(FactorTables.VehicleDebt, _edu, _race, _region, _amer.Age, _amer.MyGender,
                _amer.MaritialStatus);
            _ccDebtFactor = GetFactor(FactorTables.CreditCardDebt, _edu, _race, _region, _amer.Age, _amer.MyGender,
                _amer.MaritialStatus);
            _savingAcctFactor = GetFactor(FactorTables.SavingsAccount, _edu, _race, _region, _amer.Age, _amer.MyGender,
                _amer.MaritialStatus);
            _checkingAcctFactor = GetFactor(FactorTables.CheckingAccount, _edu, _race, _region, _amer.Age, _amer.MyGender,
                _amer.MaritialStatus);
            _netWorthFactor = GetFactor(FactorTables.NetWorth, _edu, _race, _region, _amer.Age, _amer.MyGender,
                _amer.MaritialStatus);
            _homeEquityFactor = GetFactor(FactorTables.HomeEquity, _edu, _race, _region, _amer.Age, _amer.MyGender,
                _amer.MaritialStatus);

        }
        #endregion

        #region properties
        public CreditScore CreditScore { get; }
        public bool IsRenting { get; }
        #endregion

        #region methods

        /// <summary>
        /// Creates random <see cref="Rent"/> instance with a history and adds
        /// it to the <see cref="Opes.HomeDebt"/> collection.
        /// </summary>
        /// <param name="stdDevAsPercent"></param>
        protected internal void GetRandomRent(double stdDevAsPercent = 0.1725D)
        {
            if (!IsRenting)
                return;
            //create a rent object
            var avgRent = (double)Rent.GetAvgAmericanRentByYear(null).Amount;

            var randRent = GetRandomFactorValue(FactorTables.HomeDebt, _homeDebtFactor, stdDevAsPercent, avgRent);
            var isYearTerm = Etx.CoinToss;
            var rendTerm = isYearTerm ? 12 : 6;
            var randDate = Etx.Date(0, DateTime.Today.AddDays(-2), isYearTerm ? 360 : 178);
            var rent = new Rent(randDate, rendTerm, new Pecuniam((decimal)randRent),
                Etx.CoinToss ? new Pecuniam(250) : new Pecuniam(500));

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

                rent.PayRent(paidRentOn, new Pecuniam((decimal)randRent));
                rentDueDate = rentDueDate.AddMonths(1);
            }
            rent.Description = $"{rendTerm}-Month Lease";
            HomeDebt.Add(rent);
        }

        /// <summary>
        /// Creates random <see cref="FixedRateLoan"/> instance with a history and adds
        /// it to the <see cref="Opes.HomeDebt"/> collection.
        /// </summary>
        /// <param name="stdDevAsPercent"></param>
        protected internal void GetRandomHomeLoan(double stdDevAsPercent = 0.1285D)
        {
            //calc a rand amount of what is still owed on house
            var randHouseDebt = GetRandomFactorValue(FactorTables.HomeDebt, _homeDebtFactor, stdDevAsPercent);
            //calc rand amount of equity accrued on house
            var randHouseEquity = GetRandomFactorValue(FactorTables.HomeEquity, _homeEquityFactor, stdDevAsPercent);

            //get rand interest rate weighted by score
            var randRate = CreditScore.GetRandomInterestRate(null, Gov.Fed.RiskFreeInterestRate.DF_VALUE) * 0.01;

            //calc total cost of house 
            var totalHouseCost = randHouseDebt + randHouseEquity;

            var spCost = new Pecuniam((decimal)randHouseDebt);

            //calc the monthly payment
            var fv = spCost.Amount.PerDiemInterest(randRate, Constants.TropicalYear.TotalDays * 30);
            var minPmt = new Pecuniam(Math.Round(fv / (30 * 12), 2));
            var minPmtRate = (float)Math.Round(minPmt.Amount / fv, 6);

            //given this value and rate - calc the timespan needed to have aquired this amount of equity
            var firstOfYear = new DateTime(DateTime.Today.Year, 1, 1);
            var loan = new FixedRateLoan(firstOfYear, minPmtRate, new Pecuniam((decimal)totalHouseCost))
            {
                Rate = (float)randRate
            };

            var dtIncrement = firstOfYear.AddMonths(1);
            while (loan.GetCurrentBalance(dtIncrement) > spCost)
            {
                loan.MakeAPayemnt(dtIncrement, minPmt);
                dtIncrement = dtIncrement.AddMonths(1);
            }

            //repeat process from calc'ed past date to create a history
            var housePurchaseDate = DateTime.Today.AddDays(-1 * (dtIncrement - firstOfYear).Days);
            loan = new FixedRateLoan(housePurchaseDate, minPmtRate, new Pecuniam((decimal)totalHouseCost))
            {
                Rate = (float)randRate
            };
            dtIncrement = housePurchaseDate.AddMonths(1);
            while (loan.GetCurrentBalance(dtIncrement) > spCost)
            {
                var paidOnDate = dtIncrement;
                if (_amer.Personality.GetRandomActsIrresponsible())
                    paidOnDate = paidOnDate.AddDays(Etx.IntNumber(5, 15));
                loan.MakeAPayemnt(paidOnDate, minPmt);
                dtIncrement = dtIncrement.AddMonths(1);
            }
            loan.Description = "30-Year Mortgage";
            loan.TradeLine.FormOfCredit = FormOfCredit.Mortgage;
            loan.TradeLine.DueFrequency = new TimeSpan(30, 0, 0, 0);
            loan.Lender = Bank.GetRandomBank(_amer?.Address?.HomeCityArea);
            HomeDebt.Add(loan);
        }

        /// <summary>
        /// Creates random <see cref="CreditCard"/> instances with a history and adds
        /// it to the <see cref="Opes.CreditCardDebt"/> collection.
        /// </summary>
        /// <param name="stdDevAsPercent"></param>
        /// <param name="maxCcDebt"></param>
        protected internal bool GetRandomCcDebt(double stdDevAsPercent = 0.1285D, double maxCcDebt = 15000D)
        {
            var randCcValue = GetRandomFactorValue(FactorTables.CreditCardDebt, _ccDebtFactor, stdDevAsPercent);
            System.Diagnostics.Debug.WriteLine($"Target is {randCcValue}");

            //have we already exceeded abs max
            if ((double)GetTotalCurrentCcDebt().Amount > maxCcDebt)
                return false;

            //create random cc
            var cc = CreditCard.GetRandomCc(_amer, CreditScore);

            //determine timespan for generated history
            var historyTs = DateTime.Now - cc.CardHolderSince;

            //create history
            for (var i = 1; i < historyTs.Days; i++)
            {
                var loopDt = cc.CardHolderSince.AddDays(i);
                if (cc.GetStatus(loopDt) == AccountStatus.Closed)
                    break;

                CreateSingleDaysCcCharges(cc, loopDt, randCcValue);

                if (i%30 != 0 || cc.GetCurrentBalance(loopDt).Amount < 0.0M)
                    continue;

                var minDue = cc.GetMinPayment(loopDt);
                if(minDue < new Pecuniam(10.0M))
                    minDue = new Pecuniam(10.0M);
                if (_amer.Personality.GetRandomActsIrresponsible())
                {
                    var fowardDays = Etx.IntNumber(1, 45);
                    var paidDate = loopDt.AddDays(fowardDays);
                    cc.MakeAPayemnt(paidDate, minDue);
                    i += fowardDays;
                }
                else
                {
                    var additionalPaid = Pecuniam.GetRandPecuniam(20, 200, 10);
                    cc.MakeAPayemnt(loopDt, minDue + additionalPaid);
                }
            }

            CreditCardDebt.Add(cc);
            return (double) GetTotalCurrentCcDebt().Amount < maxCcDebt;
        }

        protected internal void GetRandomBankAccounts()
        {
            var savings = Savings.GetRandomSavingAcct(_usCityArea);
            var checking = Checking.GetRandomCheckingAcct(_usCityArea);

            throw new NotImplementedException();
        }

        protected internal void GetRandomVehicles()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calc a yearly income salary or pay.
        /// </summary>
        /// <param name="factorCalc">
        /// Optional, allows caller to specify how to factor the raw results of 
        /// <see cref="GetAvgEarningPerYear"/> for the current date.
        /// </param>
        /// <param name="stdDevInUsd"></param>
        /// <returns></returns>
        protected internal Pecuniam GetPaycheck(Func<double,double, double> factorCalc = null,  double stdDevInUsd = 2000)
        {
            var eq = GetAvgEarningPerYear();
            if (eq == null)
                return null;
            var baseValue = Math.Round(eq.SolveForY(DateTime.Today.ToDouble()), 2);
            if (baseValue <= 0)
                return null;

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
            return new Pecuniam((decimal)randValue);
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
        /// Creates purchase transactions on <see cref="cc"/> at random for the given <see cref="loopDt"/>.
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="loopDt"></param>
        /// <param name="randCcValue"></param>
        protected internal void CreateSingleDaysCcCharges(CreditCard cc, DateTime loopDt, double randCcValue)
        {
            //build charges history
            var keepSpending = !cc.IsMaxedOut(loopDt);
            while (keepSpending)//want possiable multiple transactions per day
            {
                //if we reached target then exit 
                if (cc.GetCurrentBalance(loopDt) >= new Pecuniam((decimal)randCcValue))
                {
                    return;
                }

                //make purchase based on day-of-week and card holder personality
                var v = 2;
                if (loopDt.DayOfWeek == DayOfWeek.Friday || loopDt.DayOfWeek == DayOfWeek.Saturday ||
                    loopDt.DayOfWeek == DayOfWeek.Sunday)
                    v = 4;
                if (_amer.Personality.GetRandomActsIrresponsible())
                    v = 7;
                if (Etx.TryBelowOrAt(v, Etx.Dice.Ten))
                {
                    //create some random purchase amount
                    var chargeAmt = Pecuniam.GetRandPecuniam(2, v * 10);

                    //allow rare case for some big ticket item
                    if (Etx.TryAboveOrAt(94, Etx.Dice.OneHundred))
                        chargeAmt = Pecuniam.GetRandPecuniam(100, 1000);

                    //check if cardholder is maxed-out
                    if (!cc.MakeAPurchase(loopDt, chargeAmt))
                    {
                        return;
                    }
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
        #endregion
    }

}
