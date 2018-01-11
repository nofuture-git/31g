﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Reflection;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Cc;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus.Pneuma;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.Fed;
using NoFuture.Rand.Gov.Nhtsa;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Domus.Opes
{
    /// <summary>
    /// A base type on which Income, Expense, Assets, etc. is built.
    /// </summary>
    [Serializable]
    public abstract class WealthBase
    {
        #region constants
        public const double DF_STD_DEV_PERCENT = 0.0885D;
        protected internal const int DF_ROUND_DECIMAL_PLACES = 5;
        internal const string US_DOMUS_OPES = "US_DomusOpes.xml";
        #endregion

        #region fields
        protected internal IComparer<ITempore> Comparer { get; } = new TemporeComparer();
        private readonly AmericanFactors _factors;
        private static IMereo[] _incomeItemNames;
        private static IMereo[] _deductionItemNames;
        private static IMereo[] _expenseItemNames;
        private static IMereo[] _assetItemNames;
        private static IMereo[] _employmentItemNames;

        #endregion

        #region inner types

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

        #endregion

        #region ctors
        protected WealthBase(OpesOptions options)
        {
            MyOptions = options ?? new OpesOptions();
            var usCityArea = MyOptions.GeoLocation as UsCityStateZip;

            CreditScore = new PersonalCreditScore()
            {
                GetAgeAt = x => Etc.CalcAge(MyOptions.BirthDate, x),
                OpennessZscore = MyOptions.Personality?.Openness?.Value?.Zscore ?? 0D,
                ConscientiousnessZscore = MyOptions.Personality?.Conscientiousness?.Value?.Zscore ?? 0D
            };
            //TODO this should be decided by calling assemlby
            MyOptions.IsRenting = GetIsLeaseResidence(usCityArea);
            _factors = new AmericanFactors(MyOptions);

        }
        #endregion

        #region properties

        /// <summary>
        /// An instance level which acts as a default 
        /// for null ref&apos; passed into any given method.
        /// </summary>
        protected internal OpesOptions MyOptions { get; set; }

        /// <summary>
        /// The credit score of <see cref="MyOptions"/>
        /// </summary>
        public CreditScore CreditScore { get; protected set; }

        /// <summary>
        /// Exposes the calculated factors using the <see cref="MyOptions"/> passed into 
        /// the ctor.
        /// </summary>
        public AmericanFactors Factors => _factors;

        /// <summary>
        /// Determines which kind of wealth concept is 
        /// at play here (e.g. expense, assets, income, etc.).
        /// </summary>
        protected abstract DomusOpesDivisions Division { get; }

        /// <summary>
        /// The items which belong to this <see cref="Division"/>
        /// </summary>
        protected internal abstract List<Pondus> MyItems { get; }

        #endregion

        #region methods
        /// <summary>
        /// Adds the <see cref="item"/> to <see cref="MyItems"/>
        /// </summary>
        /// <param name="item"></param>
        protected internal abstract void AddItem(Pondus item);

        /// <summary>
        /// Maps the <see cref="Division"/> groups names to a function which produces that group&apos;s item names and rate.
        /// </summary>
        /// <returns></returns>
        protected abstract Dictionary<string, Func<OpesOptions, Dictionary<string, double>>> GetItems2Functions();

        /// <summary>
        /// The general &quot;Do It&quot; function. Typically this would be invoked 
        /// right after the ctor.
        /// </summary>
        /// <param name="options"></param>
        protected internal abstract void ResolveItems(OpesOptions options = null);

        /// <summary>
        /// Calculate a yearly income at random.
        /// </summary>
        /// <param name="dt">
        /// Optional, date used for solving the <see cref="GetAvgEarningPerYear"/> equation, 
        /// the default is the current system time.
        /// </param>
        /// <param name="min">
        /// Optional, absolute minimum value where results should always be this value or higher.
        /// </param>
        /// <param name="factorCalc">
        /// Optional, allows calling assembly to control the base value around which 
        /// the random value is generated
        /// </param>
        /// <param name="stdDevInUsd">
        /// Optional, a randomizes the calculated value around a mean.
        /// </param>
        /// <returns></returns>
        public Pecuniam GetRandomYearlyIncome(DateTime? dt = null, Pecuniam min = null,
            Func<double, double> factorCalc = null,
            double stdDevInUsd = 2000)
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

            factorCalc = factorCalc ?? (d => d * _factors.NetWorthFactor);

            var factorValue = factorCalc(baseValue);

            baseValue = Math.Round(factorValue, 2);

            stdDevInUsd = Math.Abs(stdDevInUsd);

            var randValue = Math.Round(
                Etx.RandomValueInNormalDist(Math.Round(baseValue, 0), stdDevInUsd), 2);

            //honor the promise to never let the value go below the 'min' if caller gave one.
            if (min > Pecuniam.Zero && randValue < 0)
                randValue = 0;
            return new Pecuniam((decimal) randValue) + min;
        }

        /// <summary>
        /// Get the linear eq of the city if its found otherwise defaults to the state, and failing that to the national
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// compiled data from BEA
        /// </remarks>
        protected internal virtual LinearEquation GetAvgEarningPerYear()
        {
            var ca = MyOptions.GeoLocation as UsCityStateZip;
            return (ca?.AverageEarnings ?? UsStateData.GetStateData(ca?.State?.ToString())?.AverageEarnings) ??
                   AmericanEquations.NatlAverageEarnings;
        }

        /// <summary>
        /// Helper method to get only those on-going items from within <see cref="items"/>
        /// (i.e. items whose end date is null)
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        protected internal virtual Pondus[] GetCurrent(List<Pondus> items)
        {
            if (items == null)
                return null;
            var o = items.Where(x => x.Terminus == null).ToList();
            o.Sort(Comparer);
            return o.ToArray();
        }

        /// <summary>
        /// Helper method to reduce <see cref="items"/> down to only 
        /// those in range of <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        protected internal virtual Pondus[] GetAt(DateTime? dt, List<Pondus> items)
        {
            if (items == null)
                return null;
            var atDateItems = dt == null
                ? items.ToList()
                : items.Where(x => x.IsInRange(dt.Value)).ToList();
            atDateItems.Sort(Comparer);
            return atDateItems.ToArray();
        }

        /// <summary>
        /// Gets the <see cref="IMereo"/> Income items
        /// from <see cref="TreeData.UsDomusOpes"/>
        /// </summary>
        /// <returns></returns>
        public static IMereo[] GetIncomeItemNames()
        {
            var xpath = $"//{DomusOpesDivisions.Income.ToString().ToLower()}//item";
            return _incomeItemNames = _incomeItemNames ?? GetDomusOpesItemNames(xpath);
        }

        /// <summary>
        /// Gets the <see cref="IMereo"/> Deduction items
        /// (e.g. Fed Tax, Child Support, FICA, etc.) from <see cref="TreeData.UsDomusOpes"/>
        /// </summary>
        /// <returns></returns>
        public static IMereo[] GetDeductionItemNames()
        {
            var xpath = $"//{DomusOpesDivisions.Deduction.ToString().ToLower()}//item";
            return _deductionItemNames = _deductionItemNames ?? GetDomusOpesItemNames(xpath);
        }

        /// <summary>
        /// Gets the <see cref="IMereo"/> Expense items 
        /// (i.e. household budget) from <see cref="TreeData.UsDomusOpes"/>
        /// </summary>
        /// <returns></returns>
        public static IMereo[] GetExpenseItemNames()
        {
            var xpath = $"//{DomusOpesDivisions.Expense.ToString().ToLower()}//item";
            return _expenseItemNames = _expenseItemNames ?? GetDomusOpesItemNames(xpath);
        }

        /// <summary>
        /// Gets the <see cref="IMereo"/> Expense items 
        /// (i.e. real and private property) 
        /// from <see cref="TreeData.UsDomusOpes"/>
        /// </summary>
        /// <returns></returns>
        public static IMereo[] GetAssetItemNames()
        {
            var xpath = $"//{DomusOpesDivisions.Assets.ToString().ToLower()}//item";
            return _assetItemNames = _assetItemNames ?? GetDomusOpesItemNames(xpath);
        }

        /// <summary>
        /// Gets the <see cref="IMereo"/> Employment items 
        /// (e.g. wage, salary, tips, etc.) 
        /// from <see cref="TreeData.UsDomusOpes"/>
        /// </summary>
        /// <returns></returns>
        public static IMereo[] GetEmploymentItemNames()
        {
            var xpath = $"//{DomusOpesDivisions.Employment.ToString().ToLower()}//item";
            return _employmentItemNames = _employmentItemNames ?? GetDomusOpesItemNames(xpath);
        }

        /// <summary>
        /// Get the Domus Opes group names for the given division
        /// </summary>
        /// <param name="division"></param>
        /// <returns></returns>
        public static List<string> GetGroupNames(DomusOpesDivisions division)
        {
            var grpNames = new List<string>();
            switch (division)
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
        /// Get the Domus Opes item names as a composite <see cref="IMereo"/>
        /// which contains both the item name and its group name.
        /// </summary>
        /// <param name="division"></param>
        /// <returns></returns>
        public static IMereo[] GetItemNames(DomusOpesDivisions division)
        {
            var grpNames = new List<IMereo>();
            switch (division)
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
        /// Factory method to get a list of group-name to group-rate using 
        /// the given <see cref="options"/>
        /// </summary>
        /// <param name="options"></param>
        /// <returns>
        /// A set of item names to some percent where the sum of all the names is 1 (i.e. 100%).
        /// </returns>
        public virtual List<Tuple<string, double>> GetGroupNames2Portions(OpesOptions options)
        {
            options = options ?? new OpesOptions();

            var grpNames = GetGroupNames(Division);

            return GetNames2Portions(options, grpNames.ToArray());
        }

        /// <summary>
        /// Factory method to get a list of item-name to item-rate using
        /// the given <see cref="options"/>
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="options"></param>
        /// <returns>
        /// A set of item names to some percent where the sum of all the names is 1 (i.e. 100%).
        /// </returns>
        public virtual List<Tuple<string, double>> GetItemNames2Portions(string groupName,OpesOptions options)
        {
            options = options ?? new OpesOptions();

            //get all the item names we are targeting
            var itemNames = GetItemNames(Division).Where(x =>
                String.Equals(x.GetName(KindsOfNames.Group), groupName, StringComparison.OrdinalIgnoreCase)).ToArray();

            return GetNames2Portions(options, itemNames.Select(k => k.Name).ToArray());
        }

        /// <summary>
        /// General form of its counterparts <see cref="GetGroupNames2Portions"/> and <see cref="GetItemNames2Portions"/>
        /// </summary>
        /// <param name="options"></param>
        /// <param name="itemNames"></param>
        /// <returns>
        /// A set of item names to some percent where the sum of all the names is 1 (i.e. 100%).
        /// </returns>
        public static List<Tuple<string, double>> GetNames2Portions(OpesOptions options, string[] itemNames)
        {
            const StringComparison STR_OPT = StringComparison.OrdinalIgnoreCase;

            //make this required
            if (itemNames == null || !itemNames.Any())
                throw new ArgumentNullException(nameof(itemNames));

            options = options ?? new OpesOptions();

            var givenDirectlyItems = options.GivenDirectly ?? new List<IMereo>();

            //immediately reduce this to only the items present in 'itemNames'
            givenDirectlyItems = givenDirectlyItems.Where(gd =>
                itemNames.Any(n => String.Equals(gd.Name, n, STR_OPT))).ToList();

            //get the direct assign's total
            var givenDirectTotal = givenDirectlyItems
                .Select(x => Math.Round(x.ExpectedValue?.Abs.ToDouble() ?? 0.0D, DF_ROUND_DECIMAL_PLACES)).Sum();

            //get total given by the caller if any
            var sumTotal = (options.SumTotal ?? Pecuniam.Zero).ToDouble();

            //get a random rate for all item names
            var randPortions = Etx.DiminishingPortions(itemNames.Length, options.DerivativeSlope);

            //put this random rate together with each item name
            var randMap = itemNames
                .Zip(randPortions, (n, v) => new Tuple<string, double>(n, v)).ToList();

            //convert it to a dictionary
            var randDict = new Dictionary<string, double>();
            foreach (var t in randMap)
            {
                randDict.Add(t.Item1, t.Item2);
            }

            //filter zero out's down, likewise, to only what's actually in itemNames
            var possibleZeroOuts = options.PossibleZeroOuts.Distinct().ToList();
            possibleZeroOuts = possibleZeroOuts
                .Where(p => itemNames.Any(i => String.Equals(p, i, STR_OPT))).ToList();

            //zero outs will get applied just like any other ReassignRates
            var actualZeroOuts = new List<Tuple<string, double>>();

            //select actual zero outs from the possiable zero outs
            if (possibleZeroOuts.Any())
            {
                foreach (var pzo in possibleZeroOuts)
                {
                    //this is the only random part in actual zero-outs
                    var diceRoll = options.DiceRoll(3, Etx.Dice.Four);

                    //these predicates are filters
                    var isAlreadyPresent = actualZeroOuts.Any(z => z.Item1 == pzo);
                    var isInGivenDirectly = givenDirectlyItems.Any(x =>
                        String.Equals(x.Name, pzo, STR_OPT));
                    
                    if (diceRoll && !isAlreadyPresent && !isInGivenDirectly)
                        actualZeroOuts.Add(new Tuple<string, double>(pzo, 0.0D));
                }
            }

            //apply any GivenDirectly's of zero like PossiableZeroOuts
            foreach (var dr in givenDirectlyItems.Where(o => o.ExpectedValue == Pecuniam.Zero))
            {
                if (actualZeroOuts.All(z => z.Item1 != dr.Name))
                    actualZeroOuts.Add(new Tuple<string, double>(dr.Name, 0.0D));
            }

            //zero out all the select names
            if (actualZeroOuts.Any())
            {
                //make one last check that we aren't zero'ing out everything
                if (actualZeroOuts.Count == itemNames.Length)
                    throw new RahRowRagee("A sum total of 1 cannot be perserved when " +
                                          "all items have been directly assigned to 0.");

                randDict = ReassignRates(randDict, actualZeroOuts, options.DerivativeSlope);
            }

            //there is nothing left to do so leave
            if (givenDirectTotal == 0.0D)
            {
                return randDict.Select(kv => new Tuple<string, double>(kv.Key, kv.Value)).ToList();
            }

            //we will need a denominator if the caller didn't give one use the sum what they did give
            var total = sumTotal;

            //if caller gives a sumtotal in which all the GivenDirectly won't fit then up the total to make it fit
            if (total < givenDirectTotal)
                total = givenDirectTotal;

            //get a dict of group names to all 0.0D
            var calcDict = new Dictionary<string, double>();

            //get the sum of each given directly item
            foreach (var d in givenDirectlyItems)
            {
                var dName = d.Name;
                if (String.IsNullOrWhiteSpace(dName)
                    || d.ExpectedValue == null
                    || d.ExpectedValue == Pecuniam.Zero)
                    continue;
                if (calcDict.ContainsKey(dName))
                    calcDict[dName] += d.ExpectedValue.Abs.ToDouble();
                else
                    calcDict.Add(dName, d.ExpectedValue.Abs.ToDouble());
            }

            var calcMap = new List<Tuple<string, double>>();

            //get the item sum as a ratio of the total
            foreach (var k in itemNames)
            {
                //we only want to reassign when value was explicity given in options
                if (!calcDict.ContainsKey(k))
                {
                    continue;
                }

                var rate = Math.Round(calcDict[k] / total, DF_ROUND_DECIMAL_PLACES);
                calcMap.Add(new Tuple<string, double>(k, rate));
            }

            //if the calc map doesn't leave any room for reassignment then we are done
            if (IsCloseEnoughToOne(Math.Round(calcMap.Select(t => t.Item2).Sum(), DF_ROUND_DECIMAL_PLACES)))
            {
                //still need to add in the 0.0 items since calcMap has only the directly assigned values
                foreach (var k in itemNames)
                {
                    if (calcMap.Any(t => String.Equals(t.Item1, k, STR_OPT)))
                        continue;
                    calcMap.Add(new Tuple<string, double>(k, 0.0D));
                }
                return calcMap;
            }

            //convert it to a dictionary 
            return ReassignRates(randDict, calcMap).Select(kv => new Tuple<string, double>(kv.Key, kv.Value)).ToList();
        }

        /// <summary>
        /// Tries to parse a single &apos;mereo&apos; item
        /// from the <see cref="Data.TreeData.UsDomusOpes"/> xml
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="mereo"></param>
        /// <returns></returns>
        public static bool TryParseUsDomusOpesXml(XmlNode xmlNode, out IMereo mereo)
        {
            mereo = null;

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

            mereo = new Mereo(itemName);
            if (!String.IsNullOrWhiteSpace(abbrev))
                mereo.UpsertName(KindsOfNames.Abbrev, abbrev);
            if (!String.IsNullOrWhiteSpace(groupName))
                mereo.UpsertName(KindsOfNames.Group, groupName);
            if (egs.Any())
            {
                foreach (var eg in egs)
                    mereo.ExempliGratia.Add(eg);
            }

            return !String.IsNullOrWhiteSpace(itemName);
        }

        private static bool IsCloseEnoughToOne(double testValue)
        {
            var calcMapSumRemainder = 1 - Math.Abs(testValue);
            return calcMapSumRemainder <= 0.00001 && calcMapSumRemainder >= -0.00001;
        }

        /// <summary>
        /// Gets all Group-Item names at the given <see cref="xPath"/>
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        internal static IMereo[] GetDomusOpesItemNames(string xPath)
        {
            if (String.IsNullOrWhiteSpace(xPath))
                return null;

            var xml = Xml2XmlIdentifier.GetEmbeddedXmlDoc(US_DOMUS_OPES, Assembly.GetExecutingAssembly());
            var nodes = xml.SelectNodes(xPath);
            if (nodes == null || nodes.Count <= 0)
                return null;
            var names = new List<IMereo>();
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
        /// Determine if the given <see cref="American"/> is renting or has a mortgage
        /// </summary>
        /// <param name="usCityArea"></param>
        /// <returns></returns>
        protected internal bool GetIsLeaseResidence(UsCityStateZip usCityArea)
        {
            if (usCityArea == null)
                return true;

            var cannotGetFinanced = CreditScore.GetRandomInterestRate(null, RiskFreeInterestRate.DF_VALUE) > 8.5;
            if (cannotGetFinanced)
                return true;

            var livesInDenseUrbanArea = usCityArea.Msa?.MsaType == (UrbanCentric.City | UrbanCentric.Large);
            var isYoung = Etc.CalcAge(MyOptions.BirthDate) < 32;
            var roll = 65;
            if (livesInDenseUrbanArea)
                roll -= 23;
            //is scaled where 29 year-old loses 3 while 21 year-old loses 11
            if (isYoung)
                roll -= 32 - Etc.CalcAge(MyOptions.BirthDate);
            return Etx.TryBelowOrAt(roll, Etx.Dice.OneHundred);
        }

        protected Pecuniam CalcValue(Pecuniam pecuniam, double d)
        {
            pecuniam = pecuniam ?? Pecuniam.Zero;
            return Math.Round(pecuniam.ToDouble() * d, 2).ToPecuniam();
        }

        /// <summary>
        /// Reassigns the rates in <see cref="names2Rates"/> to the Item2 of the matched tuple in <see cref="reassignNames2Rates"/>
        /// preserving the sum total of <see cref="names2Rates"/> values.
        /// </summary>
        /// <param name="names2Rates"></param>
        /// <param name="reassignNames2Rates"></param>
        /// <param name="derivativeSlope">
        /// Is passed into the <see cref="Etx.DiminishingPortions"/> - see its annotation.  Which entries
        /// receive the re-located rates is based on a diminishing probability.
        /// </param>
        /// <returns></returns>
        public static Dictionary<string, double> ReassignRates(Dictionary<string, double> names2Rates,
            List<Tuple<string, double>> reassignNames2Rates, double derivativeSlope = -1.0D)
        {
            if (names2Rates == null || !names2Rates.Any() || reassignNames2Rates == null || !reassignNames2Rates.Any())
                return names2Rates;

            //first check that the reassign isn't already perfectly ordered and sum to 1
            var reassignHasEveryKey = names2Rates.All(kv =>
                reassignNames2Rates.Any(rkv => String.Equals(rkv.Item1, kv.Key, StringComparison.OrdinalIgnoreCase)));
            var reassignIsSumOfOne = IsCloseEnoughToOne(Math.Round(reassignNames2Rates.Select(kv => kv.Item2).Sum(),
                DF_ROUND_DECIMAL_PLACES-1));

            if (reassignHasEveryKey && reassignIsSumOfOne)
                return reassignNames2Rates.ToDictionary(t => t.Item1, t => t.Item2);

            var relocateRates = new List<double>();
            var idxNames = new List<string>();

            var n2r = new Dictionary<string, double>();

            bool IsMatch(Tuple<string, double> tuple, string s) =>
                tuple.Item1 != null && String.Equals(tuple.Item1, s, StringComparison.OrdinalIgnoreCase);

            //make temp copies of all the differences in reassignment
            foreach (var k in names2Rates.Keys)
            {
                var currentRate = Math.Round(names2Rates[k], DF_ROUND_DECIMAL_PLACES);
                if (reassignNames2Rates.Any(x => IsMatch(x,k)))
                {
                    foreach (var reassign in reassignNames2Rates.Where(x => IsMatch(x, k)))
                    {
                        var diffInRate = Math.Round(currentRate - reassign.Item2, DF_ROUND_DECIMAL_PLACES);
                        relocateRates.Add(diffInRate);
                        if (n2r.ContainsKey(k))
                            n2r[k] = Math.Round(reassign.Item2, DF_ROUND_DECIMAL_PLACES);
                        else
                            n2r.Add(k, Math.Round(reassign.Item2,DF_ROUND_DECIMAL_PLACES));
                    }
                }
                else
                {
                    idxNames.Add(k);
                    n2r.Add(k, currentRate);
                }
            }

            if (!relocateRates.Any() || !idxNames.Any())
                return names2Rates;

            //we don't wont to distribute the reassigned amount evenly
            var diminishing = Etx.DiminishingPortions(idxNames.Count, derivativeSlope);
            var idxName2DiminishingRate = new Dictionary<string, double>();
            for (var i = 0; i < idxNames.Count; i++)
            {
                idxName2DiminishingRate.Add(idxNames[i], diminishing[i]);
            }

            //split the relocateRates between positive and negative
            var negativeRelocatRates = relocateRates.Where(x => x < 0.0D);
            var positiveRelocateRates = relocateRates.Where(x => x > 0.0D);

            foreach (var relocate in positiveRelocateRates)
            {
                //pick one of the items not given a direct assignment with bias
                var randKey = Etx.DiscreteRange(idxName2DiminishingRate);
                n2r[randKey] += relocate;
            }

            var totalNegative = Math.Round(Math.Abs(negativeRelocatRates.Sum()), DF_ROUND_DECIMAL_PLACES);

            while (totalNegative > 0.0D)
            {
                var randKey = Etx.DiscreteRange(idxName2DiminishingRate);
                var decrement = Math.Round(totalNegative * idxName2DiminishingRate[randKey], DF_ROUND_DECIMAL_PLACES);
                if (Math.Abs(decrement) == 0.0D)
                    break;
                if (n2r[randKey] - decrement < 0.0D)
                {
                    //this should push the value to 0
                    decrement = Math.Round(n2r[randKey],DF_ROUND_DECIMAL_PLACES);
                }
                n2r[randKey] -= decrement;
                totalNegative = Math.Round(totalNegative - decrement, DF_ROUND_DECIMAL_PLACES);
            }

            n2r = n2r.ToDictionary(kv => kv.Key, kv => Math.Round(kv.Value, DF_ROUND_DECIMAL_PLACES-1));
            return n2r;
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
            age = age ?? MyOptions.CurrentAge;

            var ageAtDt = age <= 0 
                ? AmericanData.AVG_AGE_AMERICAN 
                : age.Value;

            //some asymetric percentage based on age
            var yVal = hookEquation.SolveForY(ageAtDt);

            //get something randome near this value
            var randRate = Etx.RandomValueInNormalDist(yVal, 0.01921);

            //its income so it shouldn't be negative by definition
            return randRate <= 0D ? 0D : randRate;
        }

        /// <summary>
        /// Gets January 1st date from negative <see cref="back"/> years from this year&apos;s January 1st
        /// </summary>
        /// <returns></returns>
        protected internal DateTime GetYearNeg(int back)
        {
            //current year is year 0
            var year0 = DateTime.Today.Year;

            var startYear0 = new DateTime(year0, 1, 1);
            return startYear0.AddYears(-1 * Math.Abs(back));
        }

        /// <summary>
        /// Breaks the whole span of time for this instance into yearly blocks.
        /// </summary>
        /// <returns></returns>
        protected internal List<Tuple<DateTime, DateTime?>> GetYearsInDates(DateTime startDate)
        {
            var stDt = startDate;
            if(stDt.Day!=1 || stDt.Month != 1)
                stDt = new DateTime(stDt.Year, 1, 1);

            var datesOut = new List<Tuple<DateTime, DateTime?>>();

            if (stDt.Year == DateTime.Today.Year)
            {
                datesOut.Add(new Tuple<DateTime, DateTime?>(stDt, null));
                return datesOut;
            }

            var endDt = new DateTime(stDt.Year, 12, 31);

            for (var i = 0; i <= DateTime.Today.Year - stDt.Year; i++)
            {
                if (stDt.Year + i >= DateTime.Today.Year)
                {
                    datesOut.Add(new Tuple<DateTime, DateTime?>(stDt.AddYears(i), null));
                    continue;
                }

                datesOut.Add(new Tuple<DateTime, DateTime?>(stDt.AddYears(i), endDt.AddYears(i)));
            }

            return datesOut;
        }

        /// <summary>
        /// The reusable\common parts of <see cref="ResolveItems"/> 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual Pondus[] GetItemsForRange(OpesOptions options = null)
        {
            options = options ?? MyOptions;

            var itemsout = new List<Pondus>();

            var grp2Rates = GetGroupNames2Portions(options);

            foreach (var grp in grp2Rates)
            {
                itemsout.AddRange(GetItemsForRange(grp, options));
            }
            return itemsout.ToArray();
        }

        /// <summary>
        /// The reusable\common parts of <see cref="ResolveItems"/> 
        /// </summary>
        /// <param name="grp2Rate"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual Pondus[] GetItemsForRange(Tuple<string, double> grp2Rate, OpesOptions options = null)
        {
            options = options ?? MyOptions;

            var itemsout = new List<Pondus>();

            var name2Op = GetItems2Functions();

            var grpName = grp2Rate.Item1;
            var grpRate = grp2Rate.Item2;

            var useFxMapping = name2Op.ContainsKey(grpName);

            var grpRates = useFxMapping
                ? name2Op[grpName](options)
                : GetItemNames2Portions(grpName, options)
                    .ToDictionary(k => k.Item1, k => k.Item2);

            foreach (var item in grpRates.Keys)
            {
                var p = GetPondusForItemAndGroup(item, grpName, options);
                if (p.My.ExpectedValue == null || p.My.ExpectedValue == Pecuniam.Zero)
                    p.My.ExpectedValue = CalcValue(options.SumTotal, grpRates[item] * grpRate);
                p.My.Interval = options.Interval;
                itemsout.Add(p);
            }

            return itemsout.ToArray();
        }

        /// <summary>
        /// While <see cref="GetItemsForRange(OpesOptions)"/> deals with all the 
        /// items of this <see cref="Division"/> this is concerned with one-item-at-a-time.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="grpName"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual Pondus GetPondusForItemAndGroup(string itemName, string grpName, OpesOptions options)
        {
            options = options ?? new OpesOptions();
            var p = new Pondus(itemName, options.Interval)
            {
                Inception = options.Inception,
                Terminus = options.Terminus,
            };
            p.My.UpsertName(KindsOfNames.Group, grpName);
            return p;
        }


        /// <summary>
        /// Creates a new random Checking Account
        /// </summary>
        /// <param name="p"></param>
        /// <param name="dt">Date account was openned, default to now.</param>
        /// <param name="debitPin">
        /// Optional, when present and random instance of <see cref="CheckingAccount.DebitCard"/> is created with 
        /// this as its PIN.
        /// </param>
        /// <returns></returns>
        public static CheckingAccount GetRandomCheckingAcct(IVoca p, DateTime? dt = null, string debitPin = null)
        {
            var dtd = dt.GetValueOrDefault(DateTime.Now);
            var accountId = new AccountId(Etx.GetRandomRChars(true));
            return CheckingAccount.IsPossiablePin(debitPin)
                ? new CheckingAccount(accountId, dtd,
                    new Tuple<ICreditCard, string>(GetRandomCreditCard(p), debitPin))
                : new CheckingAccount(accountId, dtd);
        }

        public static SavingsAccount GetRandomSavingAcct(IVoca p, DateTime? dt = null)
        {
            var dtd = dt.GetValueOrDefault(DateTime.Now);
            var accountId = new AccountId(Etx.GetRandomRChars(true));
            return new SavingsAccount(accountId, dtd);
        }

        /// <summary>
        /// Returs a new, randomly gen'ed, concrete instance of <see cref="ICreditCard"/>
        /// </summary>
        /// <param name="cardholder"></param>
        /// <param name="opennedDate"></param>
        /// <returns></returns>
        public static ICreditCard GetRandomCreditCard(IVoca cardholder, DateTime? opennedDate = null)
        {
            var fk = Etx.IntNumber(0, 3);
            var dt = opennedDate ?? Etx.Date(-3, null);

            switch (fk)
            {
                case 0:
                    return new MasterCardCc(cardholder, dt, dt.AddYears(3));
                case 2:
                    return new AmexCc(cardholder, dt, dt.AddYears(3));
                case 3:
                    return new DiscoverCc(cardholder, dt, dt.AddYears(3));
                default:
                    return new VisaCc(cardholder, dt, dt.AddYears(3));
            }
        }


        /// <summary>
        /// Factory method to generate a <see cref="Rent"/> instance at random with a payment history.
        /// </summary>
        /// <param name="property">The property identifier on which is being leased</param>
        /// <param name="homeDebtFactor">The home debt factor based on the renter's age, gender, edu, etc.</param>
        /// <param name="renterPersonality">Optional, used when creating a history of payments.</param>
        /// <param name="stdDevAsPercent">Optional, the stdDev around the mean.</param>
        /// <returns></returns>
        public static Rent GetRandomRentWithHistory(Identifier property, double homeDebtFactor, IPersonality renterPersonality = null,
            double stdDevAsPercent = WealthBase.DF_STD_DEV_PERCENT)
        {
            //create a rent object
            renterPersonality = renterPersonality ?? new Personality();

            var rent = GetRandomRent(property, homeDebtFactor, stdDevAsPercent);
            var randDate = rent.SigningDate;
            var randRent = rent.MonthlyPmt;
            //create payment history until current
            var firstPmt = rent.GetMinPayment(randDate);
            rent.PayRent(randDate.AddDays(1), firstPmt, new Mereo(property.ToString()));

            var rentDueDate = randDate.Month == 12
                ? new DateTime(randDate.Year + 1, 1, 1)
                : new DateTime(randDate.Year, randDate.Month + 1, 1);

            while (rentDueDate < DateTime.Today)
            {
                var paidRentOn = rentDueDate;
                //move the date rent was paid to some late-date when person acts irresponsible
                if (renterPersonality.GetRandomActsIrresponsible())
                    paidRentOn = paidRentOn.AddDays(Etx.IntNumber(5, 15));

                rent.PayRent(paidRentOn, randRent, new Mereo(rent.Id.ToString()));
                rentDueDate = rentDueDate.AddMonths(1);
            }
            return rent;
        }

        /// <summary>
        /// Factory method to generate a <see cref="Rent"/> instance at random.
        /// </summary>
        /// <param name="property">The property identifier on which is being leased</param>
        /// <param name="homeDebtFactor">The home debt factor based on the renter's age, gender, edu, etc.</param>
        /// <param name="stdDevAsPercent">Optional, the stdDev around the mean.</param>
        /// <param name="totalYearlyRent">
        /// Optional, allows the calling assembly to specify this, default 
        /// is calculated from <see cref="Rent.GetAvgAmericanRentByYear"/>
        /// </param>
        /// <returns></returns>
        public static Rent GetRandomRent(Identifier property, double homeDebtFactor,
            double stdDevAsPercent = WealthBase.DF_STD_DEV_PERCENT, double? totalYearlyRent = null)
        {
            var avgRent = totalYearlyRent ?? (double)Rent.GetAvgAmericanRentByYear(null).Amount;
            var randRent = new Pecuniam(
                (decimal)
                AmericanFactors.GetRandomFactorValue(FactorTables.HomeDebt, homeDebtFactor,
                    stdDevAsPercent, avgRent));
            var randTerm = Etx.DiscreteRange(new[] { 24, 18, 12, 6 });
            var randDate = Etx.Date(0, DateTime.Today.AddDays(-2), true);
            var randDepositAmt = (int)Math.Round((randRent.Amount - randRent.Amount % 250) / 2);
            var randDeposit = new Pecuniam(randDepositAmt);

            var rent = new Rent(property, randDate, randTerm, randRent, randDeposit);
            return rent;
        }

        /// <summary>
        /// Same as its counterpart <see cref="GetRandomLoan"/> only it also produces a history of transactions.
        /// </summary>
        /// <param name="property">The property on which the loan is secured.</param>
        /// <param name="remainingCost">The balance which currently remains.</param>
        /// <param name="totalCost">
        /// The original value of the loan, the difference between 
        /// this and the <see cref="remainingCost"/> determines how much history is needed.
        /// </param>
        /// <param name="rate">The interest rate</param>
        /// <param name="termInYears"></param>
        /// <param name="minPmt"></param>
        /// <param name="borrowerPersonality">Optional, used when creating a history of payments.</param>
        /// <returns></returns>
        public static SecuredFixedRateLoan GetRandomLoanWithHistory(Identifier property, Pecuniam remainingCost,
            Pecuniam totalCost, float rate, int termInYears, out Pecuniam minPmt, IPersonality borrowerPersonality = null)
        {

            var loan = GetRandomLoan(property, remainingCost, totalCost, rate, termInYears, out minPmt);

            var pmtNote = new Mereo(property.ToString());
            //makes the fake history more colorful
            borrowerPersonality = borrowerPersonality ?? new Personality();

            var dtIncrement = loan.Inception.AddMonths(1);
            while (loan.GetValueAt(dtIncrement) > remainingCost)
            {
                if (dtIncrement >= DateTime.Now)
                    break;
                var paidOnDate = dtIncrement;
                if (borrowerPersonality.GetRandomActsIrresponsible())
                    paidOnDate = paidOnDate.AddDays(Etx.IntNumber(5, 15));

                //is this the payoff
                var isPayoff = loan.GetValueAt(paidOnDate) <= minPmt;
                if (isPayoff)
                {
                    minPmt = loan.GetValueAt(paidOnDate);
                }

                loan.Push(paidOnDate, minPmt, pmtNote, Pecuniam.Zero);
                if (isPayoff)
                {
                    loan.Terminus = paidOnDate;
                    loan.Closure = ClosedCondition.ClosedWithZeroBalance;
                    break;
                }
                dtIncrement = dtIncrement.AddMonths(1);
            }

            //insure that the gen'ed history doesn't start before the year of make
            if (property is Vin)
            {
                var maxYear = loan.Inception.Year;
                loan.PropertyId = Vin.GetRandomVin(remainingCost.ToDouble() <= 2000.0D, maxYear);
            }

            return loan;
        }

        /// <summary>
        /// Produces a random <see cref="SecuredFixedRateLoan"/>.
        /// </summary>
        /// <param name="property">The property on which the loan is secured.</param>
        /// <param name="remainingCost">The balance which currently remains.</param>
        /// <param name="totalCost">
        /// The original value of the loan, the difference between 
        /// this and the <see cref="remainingCost"/> determines how far in the past the loan would
        /// have been openned.
        /// </param>
        /// <param name="rate">The interest rate</param>
        /// <param name="termInYears"></param>
        /// <param name="minPmt"></param>
        /// <returns></returns>
        public static SecuredFixedRateLoan GetRandomLoan(Identifier property, Pecuniam remainingCost,
            Pecuniam totalCost, float rate, int termInYears, out Pecuniam minPmt)
        {
            var isMortgage = property is PostalAddress;

            //if no or nonsense values given, change to some default
            if (totalCost == null || totalCost < Pecuniam.Zero)
                totalCost = new Pecuniam(2000);
            if (remainingCost == null || remainingCost < Pecuniam.Zero)
                remainingCost = Pecuniam.Zero;

            //remaining must always be less than the total 
            if (remainingCost > totalCost)
                totalCost = remainingCost + Pecuniam.GetRandPecuniam(1000, 3000);

            //interest rate must be a positive number
            rate = Math.Abs(rate);

            //handle if caller passes in rate like 5.5 meaning they wanted 0.055
            if (rate > 1)
                rate = Convert.ToSingle(Math.Round(rate / 100, 4));

            //calc the monthly payment
            var fv = totalCost.Amount.PerDiemInterest(rate, Constants.TropicalYear.TotalDays * termInYears);
            minPmt = new Pecuniam(Math.Round(fv / (termInYears * 12), 2));
            var minPmtRate = fv == 0 ? CreditCardAccount.DF_MIN_PMT_RATE : (float)Math.Round(minPmt.Amount / fv, 6);

            //given this value and rate - calc the timespan needed to have aquired this amount of equity
            var firstOfYear = new DateTime(DateTime.Today.Year, 1, 1);
            var loan = new SecuredFixedRateLoan(property, firstOfYear, minPmtRate, totalCost)
            {
                Rate = rate
            };

            var dtIncrement = firstOfYear.AddMonths(1);
            while (loan.GetValueAt(dtIncrement) > remainingCost)
            {
                if (dtIncrement > DateTime.Now.AddYears(termInYears))
                    break;
                loan.Push(dtIncrement, minPmt);
                dtIncrement = dtIncrement.AddMonths(1);
            }

            //repeat process from calc'ed past date to create a history
            var calcPurchaseDt = DateTime.Today.AddDays(-1 * (dtIncrement - firstOfYear).Days);
            loan = isMortgage
                ? new Mortgage(property, calcPurchaseDt, rate, totalCost)
                : new SecuredFixedRateLoan(property, calcPurchaseDt, minPmtRate, totalCost)
                {
                    Rate = rate
                };

            loan.FormOfCredit = property is PostalAddress
                ? FormOfCredit.Mortgage
                : FormOfCredit.Installment;

            return loan;
        }


        /// <summary>
        /// Randomly gen's one of the concrete types of <see cref="CreditCardAccount"/>.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ccScore">
        /// Optional, if given then will generate an interest-rate and cc-max 
        /// in accordance with the score.
        /// </param>
        /// <param name="baseInterestRate">
        /// This is the lowest possiable interest rate for the random generators
        /// </param>
        /// <param name="minPmtPercent">
        /// The value used to calc a minimum monthly payment
        /// </param>
        /// <returns></returns>
        public static CreditCardAccount GetRandomCcAcct(OpesOptions p, CreditScore ccScore,
            float baseInterestRate = 10.1F + RiskFreeInterestRate.DF_VALUE,
            float minPmtPercent = CreditCardAccount.DF_MIN_PMT_RATE)
        {
            if (ccScore == null)
            {

                ccScore = new PersonalCreditScore()
                {
                    GetAgeAt = x => Etc.CalcAge(p.BirthDate, x),
                    OpennessZscore = p.Personality?.Openness?.Value?.Zscore ?? 0D,
                    ConscientiousnessZscore = p.Personality?.Conscientiousness?.Value?.Zscore ?? 0D
                };
            }

            var cc = GetRandomCreditCard(p.PersonsName);
            var max = ccScore.GetRandomMax(cc.CardHolderSince);
            var randRate = ccScore.GetRandomInterestRate(cc.CardHolderSince, baseInterestRate) * 0.01;
            var ccAcct = new CreditCardAccount(cc, minPmtPercent, max) { Rate = (float)randRate };
            return ccAcct;
        }

        #endregion
    }
}
