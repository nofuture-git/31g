using System;
using System.Collections.Generic;
using System.Xml;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Types;
using NoFuture.Util;
using NoFuture.Util.Math;

namespace NoFuture.Rand.Domus
{
    public class Opes
    {
        public IList<Savings> SavingAccounts { get; } = new List<Savings>();
        public IList<Checking> CheckingAccounts { get; } = new List<Checking>();

        public IList<IReceivable> HomeDebt { get; } = new List<IReceivable>();
        public IList<IReceivable> VehicleDebt { get; } = new List<IReceivable>();
        public IList<IReceivable> CreditCardDebt { get; } = new List<IReceivable>();
    }

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
        /// Rent prob is simple dice roll for any <see cref="american"/> which is over 27.
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

        protected internal void GetRandomRent()
        {
            if (!IsRenting)
                return;
            //create a rent object
            var avgRent = (double) Rent.GetAvgAmericanRentByYear(null).Amount;
            avgRent = avgRent + avgRent*_homeDebtFactor;

            var randRent = Math.Round(Etx.RandomValueInNormalDist(avgRent, (avgRent*0.1725)), 2);
            var isYearTerm = Etx.CoinToss;
            var rendTerm = isYearTerm ? 12 : 6;
            var randDate = Etx.Date(0, DateTime.Today.AddDays(-2), isYearTerm ? 360 : 178);
            var rent = new Rent(randDate, rendTerm, new Pecuniam((decimal) randRent),
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

            HomeDebt.Add(rent);
        }

        protected internal void GetRandomHomeLoan()
        {
            const double STD_DEV = 0.105D;
            var baseHomeDebt = GetFactorBaseValue(FactorTables.HomeDebt);

            System.Diagnostics.Debug.WriteLine($"Base house Debt {baseHomeDebt}");

            baseHomeDebt += baseHomeDebt*_homeDebtFactor;
            var randHouseDebt =
                Math.Round(
                    Etx.RandomValueInNormalDist(Math.Round(baseHomeDebt, 0), Math.Round(baseHomeDebt*STD_DEV, 0)), 2);

            System.Diagnostics.Debug.WriteLine($"Rand house Debt {randHouseDebt}");

            var baseHomeEquity = GetFactorBaseValue(FactorTables.HomeEquity);
            baseHomeEquity += baseHomeEquity*_homeEquityFactor;
            var randHouseEquity =
                Math.Round(
                    Etx.RandomValueInNormalDist(Math.Round(baseHomeEquity, 0), Math.Round(baseHomeEquity*STD_DEV, 0)), 2);

            System.Diagnostics.Debug.WriteLine($"Rand house Equity {randHouseEquity}");

            var randRate = this.CreditScore.GetRandomInterestRate(null);

            var totalHouseCost = randHouseDebt + randHouseEquity;

            System.Diagnostics.Debug.WriteLine($"Rand house Value {totalHouseCost}");

            var spCost = new Pecuniam((decimal)randHouseDebt);
            
            //given this value and rate - calc the timespan needed to have aquired this amount of equity
            var firstOfYear = new DateTime(DateTime.Today.Year, 1, 1);
            var loan = new FixedRateLoan(firstOfYear, (float) randRate, new Pecuniam((decimal)totalHouseCost));

            var minPmt = loan.GetMinPayment(firstOfYear.AddMonths(1));

            var dtIncrement = firstOfYear.AddMonths(1);
            while (loan.GetCurrentBalance(dtIncrement) > spCost)
            {
                loan.MakeAPayemnt(dtIncrement, minPmt);
                dtIncrement = dtIncrement.AddMonths(1);
            }

            //repeat process from calc'ed past date
            var housePurchaseDate = DateTime.Today.AddDays((dtIncrement - firstOfYear).Days);
            loan = new FixedRateLoan(housePurchaseDate, (float)randRate, new Pecuniam((decimal)totalHouseCost));

            dtIncrement = housePurchaseDate.AddMonths(1);
            while (loan.GetCurrentBalance(dtIncrement) > spCost)
            {
                var paidOnDate = dtIncrement;
                if (_amer.Personality.GetRandomActsIrresponsible())
                    paidOnDate = paidOnDate.AddDays(Etx.IntNumber(5, 15));
                loan.MakeAPayemnt(paidOnDate, minPmt);
                dtIncrement = dtIncrement.AddMonths(1);
            }

            HomeDebt.Add(loan);
        }

        protected internal void GetRandomCcDebt()
        {
            throw new NotImplementedException();
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
                if(double.TryParse(factorVal, out dblOut))
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
