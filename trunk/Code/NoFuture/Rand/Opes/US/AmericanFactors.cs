using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US;

namespace NoFuture.Rand.Opes.US
{
    /// <summary>
    /// Applies each of the <see cref="AmericanFactorTables"/> to the given <see cref="AmericanFactorOptions"/>
    /// which attempt to take into account education level, race, region, age, gender and 
    /// marital status.
    /// </summary>
    [Serializable]
    public class AmericanFactors
    {
        internal const string US_PERSONAL_DEBT = "US_PersonalDebt.xml";
        internal const string US_PERSONAL_WEALTH = "US_PersonalWealth.xml";

        internal static XmlDocument DebtXml;
        internal static XmlDocument WealthXml;

        /// <summary>
        /// Home Debt scalar factor above or below the national average(s)
        /// </summary>
        public double HomeDebtFactor { get; }

        /// <summary>
        /// Vehicle Debt scalar factor above or below the national average(s)
        /// </summary>
        public double VehicleDebtFactor { get; }

        /// <summary>
        /// Other unsecured debts scalar factor above or below the national average(s)
        /// </summary>
        public double OtherDebtFactor { get; }

        /// <summary>
        /// Credit Card Debt scalar factor above or below the national average(s)
        /// </summary>
        public double CreditCardDebtFactor { get; }

        /// <summary>
        /// Net Worth scalar factor above or below the national average(s)
        /// </summary>
        public double NetWorthFactor { get; }

        /// <summary>
        /// Home Equity scalar factor above or below the national average(s)
        /// </summary>
        public double HomeEquityFactor { get; }

        /// <summary>
        /// Vehicle Equity scalar factor above or below the national average(s)
        /// </summary>
        public double VehicleEquityFactor { get; }

        /// <summary>
        /// Checking account balance scalar factor above or below the national average(s)
        /// </summary>
        public double CheckingAcctFactor { get; }

        /// <summary>
        /// Savings account balance scalar factor above or below the national average(s)
        /// </summary>
        public double SavingsAcctFactor { get; }

        /// <summary>
        /// Assigns the scalar factor values based on the given <see cref="options"/>
        /// </summary>
        /// <param name="options"></param>
        public AmericanFactors(AmericanFactorOptions options)
        {
            if (options == null)
            {
                HomeDebtFactor = 1;
                VehicleDebtFactor = 1;
                CreditCardDebtFactor = 1;
                SavingsAcctFactor = 1;
                CheckingAcctFactor = 1;
                NetWorthFactor = 1;
                HomeEquityFactor = 1;
                VehicleEquityFactor = 1;
                OtherDebtFactor = 1;
                return;
            }

            var edu = options.EducationLevel;
            var race = options.Race;
            var region = options.Region;
            var age = options.GetAge();
            var gender = options.Gender;
            var maritalStatus = options.MaritialStatus;

            HomeDebtFactor = GetFactor(AmericanFactorTables.HomeDebt, edu, race, region, age, gender,
                maritalStatus);
            VehicleDebtFactor = GetFactor(AmericanFactorTables.VehicleDebt, edu, race, region, age, gender,
                maritalStatus);
            CreditCardDebtFactor = GetFactor(AmericanFactorTables.CreditCardDebt, edu, race, region, age, gender,
                maritalStatus);
            SavingsAcctFactor = GetFactor(AmericanFactorTables.SavingsAccount, edu, race, region, age, gender,
                maritalStatus);
            CheckingAcctFactor = GetFactor(AmericanFactorTables.CheckingAccount, edu, race, region, age, gender,
                maritalStatus);
            NetWorthFactor = GetFactor(AmericanFactorTables.NetWorth, edu, race, region, age, gender,
                maritalStatus);
            HomeEquityFactor = GetFactor(AmericanFactorTables.HomeEquity, edu, race, region, age, gender,
                maritalStatus);
            VehicleEquityFactor = GetFactor(AmericanFactorTables.VehicleEquity, edu, race, region, age, gender,
                maritalStatus);
            OtherDebtFactor = GetFactor(AmericanFactorTables.OtherDebt, edu, race, region, age, gender,
                maritalStatus);
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
        public static double GetRandomFactorValue(AmericanFactorTables factor, double factorMultiplier,
            double stdDevAsPercent, double? assignedBase = null)
        {
            var baseValue = assignedBase.GetValueOrDefault(GetFactorBaseValue(factor));
            baseValue = Math.Round(baseValue * factorMultiplier, 2);
            var randValue = Math.Round(
                Etx.RandomValueInNormalDist(Math.Round(baseValue, 0), Math.Round(baseValue * stdDevAsPercent, 0)), 2);
            return randValue;
        }

        /// <summary>
        /// Gets the mean of the factors based on the criteria.
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
        /// src https://www2.census.gov/programs-surveys/demo/tables/wealth/2013/wealth-asset-ownership/wealth-tables-2013.xlsx
        ///     https://www2.census.gov/programs-surveys/demo/tables/wealth/2011/wealth-asset-ownership/debt-tables-2011.xlsx
        /// </remarks>
        public static double GetFactor(AmericanFactorTables tbl, OccidentalEdu edu, NorthAmericanRace race,
            AmericanRegion region, int age, Gender gender, MaritialStatus maritialStatus)
        {
            DebtXml = DebtXml ?? XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_PERSONAL_DEBT,
                           Assembly.GetExecutingAssembly());
            WealthXml = WealthXml ??
                         XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_PERSONAL_WEALTH, Assembly.GetExecutingAssembly());
            var xmlDoc = tbl == AmericanFactorTables.CreditCardDebt || tbl == AmericanFactorTables.HomeDebt ||
                         tbl == AmericanFactorTables.VehicleDebt
                ? DebtXml
                : WealthXml;
            var tblName = Enum.GetName(typeof(AmericanFactorTables), tbl);
            var eduName = GetXmlEduName(edu);
            var raceName = Enum.GetName(typeof(NorthAmericanRace), race);
            var regionName = Enum.GetName(typeof(AmericanRegion), region);
            var genderName = Enum.GetName(typeof(Gender), gender);

            var tblXPath = $"//table[@name='{tblName}']";
            var factorCount = 0D;
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
                if (double.TryParse(factorVal, out var dblOut))
                {
                    sum += dblOut;
                    factorCount += 1;
                }
            }

            var ageNode = maritialStatus == MaritialStatus.Married || maritialStatus == MaritialStatus.Remarried
                ? xmlDoc.SelectSingleNode($"{tblXPath}/factor[@name='Age']/factor[@name='Married']")
                : xmlDoc.SelectSingleNode($"{tblXPath}/factor[@name='Age']/factor[@name='{genderName}']");
            factorCount = factorCount <= 0 ? 1 : factorCount;
            if (ageNode == null)
                return Math.Round(sum/factorCount,5);
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
                {
                    sum += dblOut;
                    factorCount += 1;
                }
            }
            return Math.Round(sum/factorCount,5);
        }

        /// <summary>
        /// Gets the base dollar value of the given factor
        /// </summary>
        /// <param name="tbl"></param>
        /// <returns></returns>
        public static double GetFactorBaseValue(AmericanFactorTables tbl)
        {
            DebtXml = DebtXml ?? XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_PERSONAL_DEBT,
                           Assembly.GetExecutingAssembly());
            WealthXml = WealthXml ??
                         XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_PERSONAL_WEALTH, Assembly.GetExecutingAssembly());


            var xmlDoc = tbl == AmericanFactorTables.CreditCardDebt || tbl == AmericanFactorTables.HomeDebt ||
                         tbl == AmericanFactorTables.VehicleDebt || tbl == AmericanFactorTables.OtherDebt
                ? DebtXml
                : WealthXml;
            var tblName = Enum.GetName(typeof(AmericanFactorTables), tbl);
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
    }
}
