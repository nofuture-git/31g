using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Geo.US;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US;
using NoFuture.Rand.Opes.US;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Enums;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Opes
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

        private static IMereo[] _incomeItemNames;
        private static IMereo[] _deductionItemNames;
        private static IMereo[] _expenseItemNames;
        private static IMereo[] _assetItemNames;
        private static IMereo[] _employmentItemNames;
        internal static XmlDocument OpesXml;
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

        #region properties

        /// <summary>
        /// Determines which kind of wealth concept is 
        /// at play here (e.g. expense, assets, income, etc.).
        /// </summary>
        protected abstract DomusOpesDivisions Division { get; }

        /// <summary>
        /// The items which belong to this <see cref="Division"/>
        /// </summary>
        protected internal abstract List<Pondus> MyItems { get; }

        protected internal static XmlDocument UsDomusOpesData => OpesXml ?? (OpesXml =
                                                                     XmlDocXrefIdentifier.GetEmbeddedXmlDoc(
                                                                         US_DOMUS_OPES,
                                                                         Assembly.GetExecutingAssembly()));

        #endregion

        #region methods
        /// <summary>
        /// Adds the <see cref="item"/> to <see cref="MyItems"/>
        /// </summary>
        /// <param name="item"></param>
        public abstract void AddItem(Pondus item);

        public virtual void AddItem(string name, string groupName, Pecuniam expectedValue, Interval interval = Interval.Annually)
        {
            var p = new Pondus(new VocaBase(name, groupName));
            p.Expectation.Value = expectedValue;
            p.Expectation.Interval = interval;
            AddItem(p);
        }

        public virtual void AddItem(string name, double expectedValue, Interval interval = Interval.Annually,
            CurrencyAbbrev c = CurrencyAbbrev.USD)
        {
            var p = new Pondus(new VocaBase(name, Division.ToString()));
            p.Expectation.Value = new Pecuniam(Convert.ToDecimal(expectedValue), c);
            p.Expectation.Interval = interval;
            AddItem(p);
        }

        /// <summary>
        /// Maps the <see cref="Division"/> groups names to a function which produces that group&apos;s item names and rate.
        /// </summary>
        /// <returns></returns>
        protected abstract Dictionary<string, Func<OpesOptions, Dictionary<string, double>>> GetItems2Functions();

        /// <summary>
        /// Gets all the items of the given Opes type as random values based on the 
        /// options.
        /// </summary>
        /// <param name="options"></param>
        protected internal abstract void RandomizeAllItems(OpesOptions options);

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
        /// <param name="options">
        /// </param>
        /// <param name="stdDevInUsd">
        /// Optional, a randomizes the calculated value around a mean.
        /// </param>
        /// <returns></returns>
        public Pecuniam GetRandomYearlyIncome(DateTime? dt = null, OpesOptions options = null, Pecuniam min = null,
            double stdDevInUsd = 2000)
        {
            if (min == null)
                min = Pecuniam.Zero;

            //get linear eq for earning 
            var eq = GetAvgEarningPerYear(options);
            if (eq == null)
                return Pecuniam.Zero;
            var baseValue = Math.Round(eq.SolveForY(dt.GetValueOrDefault(DateTime.Today).ToDouble()), 2);
            if (baseValue <= 0)
                return Pecuniam.Zero;

            var netWorth = new AmericanFactors(options?.FactorOptions).NetWorthFactor;

            var factorValue = baseValue * netWorth;

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
        protected internal virtual LinearEquation GetAvgEarningPerYear(OpesOptions options)
        {
            var ca = options?.HomeLocation as UsCityStateZip;
            if (ca == null)
                return AmericanEquations.NatlAverageEarnings;
            ca.GetXmlData();
            return (ca.AverageEarnings ?? UsStateData.GetStateData(ca?.StateName)?.AverageEarnings) ??
                   AmericanEquations.NatlAverageEarnings;
        }

        /// <summary>
        /// Helper method to get only those on-going items from within <see cref="items"/>
        /// (i.e. items whose end date is null and whose expected value is not zero).
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        protected internal virtual Pondus[] GetCurrent(List<Pondus> items)
        {
            if (items == null)
                return null;
            var o = items.Where(x => x.Terminus == null && x.Expectation.Value != Pecuniam.Zero).ToList();
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
        /// from US Domus Opes data file
        /// </summary>
        /// <returns></returns>
        public static IMereo[] GetIncomeItemNames()
        {
            var xpath = $"//{DomusOpesDivisions.Income.ToString().ToLower()}//item";
            return _incomeItemNames = _incomeItemNames ?? GetDomusOpesItemNames(xpath);
        }

        /// <summary>
        /// Gets the <see cref="IMereo"/> Deduction items
        /// (e.g. Fed Tax, Child Support, FICA, etc.) from US Domus Opes data file
        /// </summary>
        /// <returns></returns>
        public static IMereo[] GetDeductionItemNames()
        {
            var xpath = $"//{DomusOpesDivisions.Deduction.ToString().ToLower()}//item";
            return _deductionItemNames = _deductionItemNames ?? GetDomusOpesItemNames(xpath);
        }

        /// <summary>
        /// Gets the <see cref="IMereo"/> Expense items 
        /// (i.e. household budget) from US Domus Opes data file
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
        /// from US Domus Opes data file
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
        /// from US Domus Opes data file
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
            options = options ?? OpesOptions.RandomOpesOptions();

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
        public virtual List<Tuple<string, double>> GetItemNames2Portions(string groupName, OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();

            //get all the item names we are targeting
            var itemNames = GetItemNames(Division).Where(x =>
                String.Equals(x.GetName(KindsOfNames.Group), groupName, StringComparison.OrdinalIgnoreCase)).ToArray();

            return GetNames2Portions(options, itemNames.Select(k => k.Name).ToArray());
        }

        /// <summary>
        /// The capital method to get random portions for a list of names.
        /// </summary>
        /// <param name="options">
        /// The options which control the randomness of the generated rates for each of the <see cref="itemOrGroupNames"/>
        /// </param>
        /// <param name="itemOrGroupNames">
        /// These are the names with which a random portion is supposed to be generated.
        /// </param>
        /// <returns>
        /// A set of item names to some percent where the sum of all the name&apos;s portion is 1 (i.e. 100%).
        /// </returns>
        /// <remarks>
        /// FAQ
        /// Q: What happens if no names are given?
        /// A: An ArgumentNullException is thrown.
        /// 
        /// Q: What happens if you just invoke it with no options whatsoever?
        /// A: Then every item-name gets a truly random value - the sum of which equals 1.
        /// 
        /// Q: How does SumTotal work with GivenDirectly?
        /// A: By relation of the SumTotal to the actual total of all the GivenDirectly&apos;s ExpectedValue.
        ///    The only time it matters is when SumTotal exceeds the actual total; furthermore, SumTotal has no use
        ///    when the GivenDirectly is empty since we are getting random portions and not random values.
        ///    
        /// Q: So what happens when there are GivenDirectly values and no SumTotal?
        /// A: Then its just doing the math and nothing is random.
        /// 
        /// Q: What happens if SumTotal is less-than GivenDirectly's actual total.
        /// A: Then SumTotal is just reassigned to the actual total and it again just doing the math - nothing random.
        /// 
        /// Q: What about when SumTotal exceeds actual total?
        /// A: Then the excess amount is what is used to generate random portions for the other item-names not present 
        ///    in GivenDirectly.
        /// 
        /// Q: What if there are no items in GivenDirectly?
        /// A: It just resorts back to all item-names having a random portion.
        /// 
        /// Q: What happens when the item-names don&apos;t match the names present in GivenDirectly?
        /// A: The output is always tied to the item-names - any GivenDirectly not found in the item-names is ignored.
        /// 
        /// Q: Can GivenDirectly be assigned an ExpectedValue of zero?
        /// A: Yes, and that is their main purpose to selectively remove randomness for certian item-names - recall 
        ///    that the function wants to assign some portion to every item-name, no matter how small.
        /// 
        /// Q: What happens if I force every item to be zero using GivenDirectly?
        /// A: An exception is thrown - the function cannot satisfy portions whose sum is equal to both zero and one.
        /// 
        /// Q: How do the PossiableZero outs play with explict values on GivenDirectly?
        /// A: The PossiableZeroOuts are only considered when they are not present in the GivenDirectly.
        /// 
        /// Q: What if the SumTotal exceeds the GivenDirectly's sum but all the other item-names are present 
        ///    in the PossiablyZeroOut&apos;s and it just so happens that they all, in fact do, get selected to be zero-ed out?
        /// A: It leaves one to receive the excess - in effect forcing the dice role to be false for at least 
        ///    one of the PossiablyZeroOuts in this case no matter the odds.
        /// </remarks>
        public static List<Tuple<string, double>> GetNames2Portions(OpesPortions options, string[] itemOrGroupNames)
        {
            const StringComparison STR_OPT = StringComparison.OrdinalIgnoreCase;

            //make this required
            if (itemOrGroupNames == null || !itemOrGroupNames.Any())
                throw new ArgumentNullException(nameof(itemOrGroupNames));

            options = options ?? new OpesPortions();

            var givenDirectlyItems = options.GivenDirectly ?? new List<IMereo>();

            //immediately reduce this to only the items present in 'itemNames'
            givenDirectlyItems = givenDirectlyItems.Where(gd =>
                itemOrGroupNames.Any(n => String.Equals(gd.Name, n, STR_OPT))).ToList();

            //get the direct assign's total
            var givenDirectTotal = givenDirectlyItems
                .Select(x => Math.Round(x.Value?.Abs.ToDouble() ?? 0.0D, DF_ROUND_DECIMAL_PLACES)).Sum();

            //get total given by the caller if any
            var sumTotal = (options.SumTotal ?? Pecuniam.Zero).ToDouble();

            //get a random rate for all item names
            var randPortions = Etx.RandomDiminishingPortions(itemOrGroupNames.Length, options.DerivativeSlope);

            //put this random rate together with each item name
            var randMap = itemOrGroupNames
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
                .Where(p => itemOrGroupNames.Any(i => String.Equals(p, i, STR_OPT))).ToList();

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
            foreach (var dr in givenDirectlyItems.Where(o => o.Value == Pecuniam.Zero))
            {
                if (actualZeroOuts.All(z => z.Item1 != dr.Name))
                    actualZeroOuts.Add(new Tuple<string, double>(dr.Name, 0.0D));
            }

            //zero out all the select names
            if (actualZeroOuts.Any())
            {
                //make one last check that we aren't zero'ing out everything
                if (actualZeroOuts.Count == itemOrGroupNames.Length)
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
                    || d.Value == null
                    || d.Value == Pecuniam.Zero)
                    continue;
                if (calcDict.ContainsKey(dName))
                    calcDict[dName] += d.Value.Abs.ToDouble();
                else
                    calcDict.Add(dName, d.Value.Abs.ToDouble());
            }

            var calcMap = new List<Tuple<string, double>>();

            //get the item sum as a ratio of the total
            foreach (var k in itemOrGroupNames)
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
                foreach (var k in itemOrGroupNames)
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
        /// from the US Domus Opes data file
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

            OpesXml = OpesXml ?? XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_DOMUS_OPES,
                           Assembly.GetExecutingAssembly());
            var nodes = OpesXml.SelectNodes(xPath);
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
        /// Weights the probability that a person will lease when they are young, living in a dense urban area or both.
        /// </summary>
        /// <param name="msaType"></param>
        /// <param name="age"></param>
        /// <returns></returns>
        protected internal bool GetIsLeaseResidence(UrbanCentric msaType, int age)
        {

            var livesInDenseUrbanArea = msaType == (UrbanCentric.City | UrbanCentric.Large);
            var isYoung = age < 32;
            var roll = 65;
            if (livesInDenseUrbanArea)
                roll -= 23;
            //is scaled where 29 year-old loses 3 while 21 year-old loses 11
            if (isYoung)
                roll -= 32 - age;
            return Etx.RandomRollBelowOrAt(roll, Etx.Dice.OneHundred);
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
        /// Is passed into the <see cref="Etx.RandomDiminishingPortions"/> - see its annotation.  Which entries
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
            var diminishing = Etx.RandomDiminishingPortions(idxNames.Count, derivativeSlope);
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
                var randKey = Etx.RandomPickOne(idxName2DiminishingRate);
                n2r[randKey] += relocate;
            }

            var totalNegative = Math.Round(Math.Abs(negativeRelocatRates.Sum()), DF_ROUND_DECIMAL_PLACES);

            while (totalNegative > 0.0D)
            {
                var randKey = Etx.RandomPickOne(idxName2DiminishingRate);
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
            age = age ?? (int)Math.Round(AmericanData.AVG_AGE_AMERICAN);

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
        /// The reusable\common parts of <see cref="RandomizeAllItems"/> 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual Pondus[] GetItemsForRange(OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();

            var itemsout = new List<Pondus>();

            var grp2Rates = GetGroupNames2Portions(options);

            foreach (var grp in grp2Rates)
            {
                itemsout.AddRange(GetItemsForRange(grp, options));
            }
            return itemsout.ToArray();
        }

        /// <summary>
        /// The reusable\common parts of <see cref="RandomizeAllItems"/> 
        /// </summary>
        /// <param name="grp2Rate"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected internal virtual Pondus[] GetItemsForRange(Tuple<string, double> grp2Rate, OpesOptions options)
        {
            options = options ?? OpesOptions.RandomOpesOptions();

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
                if (p.Expectation.Value == null || p.Expectation.Value == Pecuniam.Zero)
                    p.Expectation.Value = CalcValue(options.SumTotal, grpRates[item] * grpRate);
                p.Expectation.Interval = options.Interval;
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
            options = options ?? OpesOptions.RandomOpesOptions();
            var p = new Pondus(itemName, options.Interval)
            {
                Inception = options.Inception,
                Terminus = options.Terminus,
            };
            p.Expectation.UpsertName(KindsOfNames.Group, grpName);
            return p;
        }

        #endregion
    }
}
