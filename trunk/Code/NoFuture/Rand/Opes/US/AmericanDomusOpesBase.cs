﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Gov.US;

namespace NoFuture.Rand.Opes.US
{
    /// <inheritdoc cref="WealthBase" />
    /// <summary>
    /// Base type for US household wealth
    /// </summary>
    [Serializable]
    public abstract class AmericanDomusOpesBase : WealthBase
    {
        private static IVoca[] _incomeItemNames;
        private static IVoca[] _deductionItemNames;
        private static IVoca[] _expenseItemNames;
        private static IVoca[] _assetItemNames;
        private static IVoca[] _employmentItemNames;
        internal static XmlDocument OpesXml;
        internal const string US_DOMUS_OPES = "US_DomusOpes.xml";

        /// <summary>
        /// The general concept or kind on which the idea of wealth is divided.
        /// </summary>
        public enum DomusOpesDivisions
        {
            Employment,
            Income,
            Deduction,
            Expense,
            Assets
        }

        /// <summary>
        /// The group names of the Assets Division
        /// </summary>
        internal static class AssetGroupNames
        {
            internal const string REAL_PROPERTY = "Real Property";
            internal const string PERSONAL_PROPERTY = "Personal Property";
            internal const string SECURITIES = "Securities";
            internal const string INSTITUTIONAL = "Institutional";
        }

        /// <summary>
        /// The group names of the Expense Division
        /// </summary>
        internal static class ExpenseGroupNames
        {
            internal const string HOME = "Home";
            internal const string UTILITIES = "Utilities";
            internal const string TRANSPORTATION = "Transportation";
            internal const string INSURANCE = "Insurance Premiums";
            internal const string PERSONAL = "Personal";
            internal const string CHILDREN = "Children";
            internal const string DEBT = "Debts";
            internal const string HEALTH = "Health";
        }

        /// <summary>
        /// The group name of the Income Division
        /// </summary>
        internal static class IncomeGroupNames
        {
            internal const string JUDGMENTS = "Judgments";
            internal const string SUBITO = "Subito";
            internal const string EMPLOYMENT = "Employment";
            internal const string PUBLIC_BENEFITS = "Public Benefits";
            internal const string REAL_PROPERTY = AssetGroupNames.REAL_PROPERTY;
            internal const string SECURITIES = AssetGroupNames.SECURITIES;
            internal const string INSTITUTIONAL = AssetGroupNames.INSTITUTIONAL;
            internal const string DEBT = "Debts";
            internal const string HEALTH = ExpenseGroupNames.HEALTH;
        }

        /// <summary>
        /// The group name of the Deductions Division
        /// </summary>
        internal static class DeductionGroupNames
        {
            internal const string INSURANCE = "Insurance";
            internal const string GOVERNMENT = "Government";
            internal const string JUDGMENTS = IncomeGroupNames.JUDGMENTS;
            internal const string EMPLOYMENT = IncomeGroupNames.EMPLOYMENT;
        }

        /// <summary>
        /// The group name of the Employment Division
        /// </summary>
        internal static class EmploymentGroupNames
        {
            internal const string PAY = "Pay";
        }

        /// <summary>
        /// Determines which kind of wealth concept is 
        /// at play here (e.g. expense, assets, income, etc.).
        /// </summary>
        protected abstract DomusOpesDivisions Division { get; }

        protected internal override string DivisionName => Division.ToString();

        protected internal static XmlDocument UsDomusOpesData => OpesXml ?? (OpesXml =
                                                                     XmlDocXrefIdentifier.GetEmbeddedXmlDoc(
                                                                         US_DOMUS_OPES,
                                                                         Assembly.GetExecutingAssembly()));

        /// <summary>
        /// Gets the <see cref="IVoca"/> Income items
        /// from US Domus Opes data file
        /// </summary>
        /// <returns></returns>
        public static IVoca[] GetIncomeItemNames()
        {
            var xpath = $"//{DomusOpesDivisions.Income.ToString().ToLower()}//item";
            return _incomeItemNames = _incomeItemNames ?? GetDomusOpesItemNames(xpath);
        }

        /// <summary>
        /// Gets the <see cref="IVoca"/> Deduction items
        /// (e.g. Fed Tax, Child Support, FICA, etc.) from US Domus Opes data file
        /// </summary>
        /// <returns></returns>
        public static IVoca[] GetDeductionItemNames()
        {
            var xpath = $"//{DomusOpesDivisions.Deduction.ToString().ToLower()}//item";
            return _deductionItemNames = _deductionItemNames ?? GetDomusOpesItemNames(xpath);
        }

        /// <summary>
        /// Gets the <see cref="IVoca"/> Expense items 
        /// (i.e. household budget) from US Domus Opes data file
        /// </summary>
        /// <returns></returns>
        public static IVoca[] GetExpenseItemNames()
        {
            var xpath = $"//{DomusOpesDivisions.Expense.ToString().ToLower()}//item";
            return _expenseItemNames = _expenseItemNames ?? GetDomusOpesItemNames(xpath);
        }

        /// <summary>
        /// Gets the <see cref="IVoca"/> Expense items 
        /// (i.e. real and private property) 
        /// from US Domus Opes data file
        /// </summary>
        /// <returns></returns>
        public static IVoca[] GetAssetItemNames()
        {
            var xpath = $"//{DomusOpesDivisions.Assets.ToString().ToLower()}//item";
            return _assetItemNames = _assetItemNames ?? GetDomusOpesItemNames(xpath);
        }

        /// <summary>
        /// Gets the <see cref="IVoca"/> Employment items 
        /// (e.g. wage, salary, tips, etc.) 
        /// from US Domus Opes data file
        /// </summary>
        /// <returns></returns>
        public static IVoca[] GetEmploymentItemNames()
        {
            var xpath = $"//{DomusOpesDivisions.Employment.ToString().ToLower()}//item";
            return _employmentItemNames = _employmentItemNames ?? GetDomusOpesItemNames(xpath);
        }

        /// <summary>
        /// Get the Domus Opes group names for the given division
        /// </summary>
        /// <param name="division"></param>
        /// <returns></returns>
        public override List<string> GetGroupNames(string division = null)
        {
            division = division ?? DivisionName;
            var grpNames = new List<string>();
            if (!Enum.TryParse(division, out DomusOpesDivisions div))
                return grpNames;
            switch (div)
            {
                case DomusOpesDivisions.Assets:
                    grpNames.AddRange(GetAssetItemNames().Select(x => x.GetName(KindsOfNames.Group)));
                    break;
                case DomusOpesDivisions.Deduction:
                    grpNames.AddRange(GetDeductionItemNames().Select(x => x.GetName(KindsOfNames.Group)));
                    break;
                case DomusOpesDivisions.Expense:
                    grpNames.AddRange(GetExpenseItemNames().Select(x => x.GetName(KindsOfNames.Group)));
                    break;
                case DomusOpesDivisions.Income:
                    grpNames.AddRange(GetIncomeItemNames().Select(x => x.GetName(KindsOfNames.Group)));
                    break;
                case DomusOpesDivisions.Employment:
                    grpNames.AddRange(GetEmploymentItemNames().Select(x => x.GetName(KindsOfNames.Group)));
                    break;
            }
            return grpNames.Distinct().ToList();
        }

        /// <summary>
        /// Get the Domus Opes item names as a composite <see cref="IVoca"/>
        /// which contains both the item name and its group name.
        /// </summary>
        /// <param name="division"></param>
        /// <returns></returns>
        public override IVoca[] GetItemNames(string division = null)
        {
            division = division ?? DivisionName;
            var grpNames = new List<IVoca>();
            if (!Enum.TryParse(division, out DomusOpesDivisions div))
                return grpNames.ToArray();
            switch (div)
            {
                case DomusOpesDivisions.Assets:
                    grpNames.AddRange(GetAssetItemNames());
                    break;
                case DomusOpesDivisions.Deduction:
                    grpNames.AddRange(GetDeductionItemNames());
                    break;
                case DomusOpesDivisions.Expense:
                    grpNames.AddRange(GetExpenseItemNames());
                    break;
                case DomusOpesDivisions.Income:
                    grpNames.AddRange(GetIncomeItemNames());
                    break;
            }
            return grpNames.Distinct().ToArray();
        }

        /// <summary>
        /// Tries to parse a single item
        /// from the US Domus Opes data file
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="voca"></param>
        /// <returns></returns>
        internal static bool TryParseUsDomusOpesXml(XmlNode xmlNode, out IVoca voca)
        {
            voca = null;

            if (xmlNode == null)
                return false;

            if (!(xmlNode is XmlElement xmlElem))
                return false;

            var egs = new List<string>();

            var groupName = xmlElem.ParentNode is XmlElement groupElem && groupElem.HasAttributes
                ? groupElem.GetAttribute("name")
                : "";
            var itemName = xmlElem.GetAttribute("name");
            var abbrev = xmlElem.GetAttribute("abbrev");
            if (xmlElem.HasChildNodes)
            {
                foreach (var cn in xmlElem.ChildNodes)
                {
                    if (!(cn is XmlElement childElem))
                        continue;
                    if (childElem.LocalName != "eg" || !childElem.HasAttributes)
                        continue;
                    var eg = childElem.GetAttribute("name");
                    if (String.IsNullOrWhiteSpace(eg))
                        continue;
                    egs.Add(eg);
                }
            }

            voca = new VocaBase(itemName);
            if (!string.IsNullOrWhiteSpace(abbrev))
                voca.AddName(KindsOfNames.Abbrev, abbrev);
            if (!string.IsNullOrWhiteSpace(groupName))
                voca.AddName(KindsOfNames.Group, groupName);

            return !string.IsNullOrWhiteSpace(itemName);
        }

        internal static string[] GetAllowZeroNames(DomusOpesDivisions division, string groupName)
        {
            if(string.IsNullOrWhiteSpace(groupName))
                return new string[]{};
            var xPath = $"//{division.ToString().ToLower()}/group[@name='{groupName}']/item";
            OpesXml = OpesXml ?? XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_DOMUS_OPES,
                          Assembly.GetExecutingAssembly());
            var nodes = OpesXml.SelectNodes(xPath);
            if (nodes == null || nodes.Count <= 0)
                return new string[] { };
            var allowZeroNames = new List<string>();
            foreach (var o in nodes)
            {
                if (!(o is XmlElement node))
                    continue;
                var name = node.GetAttribute("name");
                if (string.IsNullOrWhiteSpace(name))
                    continue;
                var allowZeroAttrValue = node.GetAttribute("allow-zero");
                if (string.IsNullOrWhiteSpace(allowZeroAttrValue))
                    continue;
                if (!bool.TryParse(allowZeroAttrValue, out var allowZero))
                    continue;
                if (!allowZero)
                    continue;
                allowZeroNames.Add(name);
            }

            return allowZeroNames.ToArray();
        }

        /// <summary>
        /// Gets all Group-Item names at the given <see cref="xPath"/>
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        internal static IVoca[] GetDomusOpesItemNames(string xPath)
        {
            if (String.IsNullOrWhiteSpace(xPath))
                return null;

            OpesXml = OpesXml ?? XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_DOMUS_OPES,
                          Assembly.GetExecutingAssembly());
            var nodes = OpesXml.SelectNodes(xPath);
            if (nodes == null || nodes.Count <= 0)
                return null;
            var names = new List<IVoca>();
            foreach (var o in nodes)
            {
                if (!(o is XmlNode node))
                    continue;
                if (TryParseUsDomusOpesXml(node, out var nameOut))
                    names.Add(nameOut);
            }
            return names.ToArray();
        }


        /// <summary>
        /// Gets a rate, at random, using the <see cref="AmericanEquations.ClassicHook"/>
        /// </summary>
        /// <param name="age">
        /// Optional, will use the Person&apos;s Age or the mean age of Americans.
        /// </param>
        /// <returns></returns>
        protected internal virtual double GetRandomRateFromClassicHook(double? age = null)
        {
            //we want age to have an effect on the randomness
            var hookEquation = AmericanEquations.ClassicHook;
            age = age ?? (int)Math.Round(AmericanData.AVG_AGE_AMERICAN);

            var ageAtDt = age <= 0 
                ? AmericanData.AVG_AGE_AMERICAN 
                : age.Value;

            //some asymmetric percentage based on age
            var yVal = hookEquation.SolveForY(ageAtDt);

            //get something random near this value
            var randRate = Etx.RandomValueInNormalDist(yVal, 0.01921);

            //its income so it shouldn't be negative by definition
            return randRate <= 0D ? 0D : randRate;
        }
    }
}
