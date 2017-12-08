using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Domus.Pneuma;
using NoFuture.Rand.Gov.Fed;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Domus.Opes
{
    public abstract class WealthBase
    {
        #region constants
        public const double DF_STD_DEV_PERCENT = 0.1285D;

        #endregion

        #region fields
        protected internal IComparer<ITempore> Comparer { get; } = new TemporeComparer();
        private readonly NorthAmerican _amer;
        protected readonly bool IsRenting;
        private readonly NorthAmericanFactors _factors;
        private static IMereo[] _incomeItemNames;
        private static IMereo[] _deductionItemNames;
        private static IMereo[] _expenseItemNames;
        private static IMereo[] _assetItemNames;
        #endregion

        #region inner types

        /// <summary>
        /// The four major divisions of wealth
        /// </summary>
        public enum DomusOpesDivisions
        {
            Income,
            Deduction,
            Expense,
            Assets
        }

        internal static class AssetGroupNames
        {
            internal const string REAL_PROPERTY = "Real Property";
            internal const string PERSONAL_PROPERTY = "Personal Property";
            internal const string SECURITIES = "Securities";
            internal const string INSTITUTIONAL = "Institutional";
        }

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

        internal static class DeductionGroupNames
        {
            internal const string INSURANCE = "Insurance";
            internal const string GOVERNMENT = "Government";
            internal const string JUDGMENTS = IncomeGroupNames.JUDGMENTS;
            internal const string EMPLOYMENT = IncomeGroupNames.EMPLOYMENT;
        }

        #endregion

        #region ctors
        protected WealthBase(NorthAmerican american, bool isRenting = false)
        {
            if (american == null)
            {
                CreditScore = new PersonalCreditScore(null);
                _factors = new NorthAmericanFactors(null);
                IsRenting = isRenting;
                return;
            }
            _amer = american;
            var usCityArea = _amer?.Address?.HomeCityArea as UsCityStateZip;

            CreditScore = new PersonalCreditScore(american);

            //determine if renting or own
            IsRenting = isRenting || GetIsLeaseResidence(usCityArea);
            _factors = new NorthAmericanFactors(_amer);
        }
        #endregion

        #region inner types

        protected internal class RatesDictionaryArgs
        {
            internal double? SumOfRates { get; set; }
            internal double? DerivativeSlope { get; set; }
            internal Dictionary<string, double> DirectAssignNames2Rates { get; set; }
        }
        #endregion 

        public CreditScore CreditScore { get; protected set; }

        /// <summary>
        /// Exposes the calculated factors using the <see cref="NorthAmerican"/> passed into 
        /// the ctor.
        /// </summary>
        public NorthAmericanFactors Factors => _factors;

        protected internal virtual NorthAmerican Person => _amer;

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
            var ca = _amer?.Address?.HomeCityArea as UsCityStateZip;
            return (ca?.AverageEarnings ?? ca?.State?.GetStateData()?.AverageEarnings) ?? NAmerUtil.Equations.NatlAverageEarnings;
        }

        protected internal virtual Pondus[] GetCurrent(List<Pondus> items)
        {
            if (items == null)
                return null;
            var o = items.Where(x => x.Terminus == null).ToList();
            o.Sort(Comparer);
            return o.ToArray();
        }

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
        /// from <see cref="Data.TreeData.UsDomusOpes"/>
        /// </summary>
        /// <returns></returns>
        public static IMereo[] GetIncomeItemNames()
        {
            var xpath = $"//{DomusOpesDivisions.Income.ToString().ToLower()}//mereo";
            return _incomeItemNames = _incomeItemNames ?? GetDomusOpesItemNames(xpath);
        }

        /// <summary>
        /// Gets the <see cref="IMereo"/> Deduction items
        /// from <see cref="Data.TreeData.UsDomusOpes"/>
        /// </summary>
        /// <returns></returns>
        public static IMereo[] GetDeductionItemNames()
        {
            var xpath = $"//{DomusOpesDivisions.Deduction.ToString().ToLower()}//mereo";
            return _deductionItemNames = _deductionItemNames ?? GetDomusOpesItemNames(xpath);
        }

        /// <summary>
        /// Gets the <see cref="IMereo"/> Expense items 
        /// (i.e. household budget) from <see cref="Data.TreeData.UsDomusOpes"/>
        /// </summary>
        /// <returns></returns>
        public static IMereo[] GetExpenseItemNames()
        {
            var xpath = $"//{DomusOpesDivisions.Expense.ToString().ToLower()}//mereo";
            return _expenseItemNames = _expenseItemNames ?? GetDomusOpesItemNames(xpath);
        }

        /// <summary>
        /// Gets the <see cref="IMereo"/> Expense items 
        /// (i.e. real and private property) 
        /// from <see cref="Data.TreeData.UsDomusOpes"/>
        /// </summary>
        /// <returns></returns>
        public static IMereo[] GetAssetItemNames()
        {
            var xpath = $"//{DomusOpesDivisions.Assets.ToString().ToLower()}//mereo";
            return _assetItemNames = _assetItemNames ?? GetDomusOpesItemNames(xpath);
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

        internal static IMereo[] GetDomusOpesItemNames(string xPath)
        {
            if (String.IsNullOrWhiteSpace(xPath))
                return null;

            var xml = TreeData.UsDomusOpes;
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
        /// Determine if the given <see cref="NorthAmerican"/> is renting or has a mortgage
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
            var isYoung = _amer.GetAgeAt(null) < 32;
            var roll = 65;
            if (livesInDenseUrbanArea)
                roll -= 23;
            //is scaled where 29 year-old loses 3 while 21 year-old loses 11
            if (isYoung)
                roll -= 32 - _amer.GetAgeAt(null);
            return Etx.TryBelowOrAt(roll, Etx.Dice.OneHundred);
        }

        protected Pecuniam CalcValue(Pecuniam pecuniam, double d)
        {
            return Math.Round(pecuniam.ToDouble() * d, 2).ToPecuniam();
        }

        /// <summary>
        /// Helper method to get a bunch of random rates mapped to some names
        /// </summary>
        /// <param name="names"></param>
        /// <param name="sumOfRates">
        /// Optional, expected sum of all generated rates to approach 1 (a.k.a. 100 percent)
        /// </param>
        /// <param name="randRateFunc">
        /// Optional, allows calling assembly to control how a random rate is generated.
        /// Default is to generated anywhere between 0.01 and <see cref="sumOfRates"/>
        /// </param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetNames2RandomRates(IEnumerable<IMereo> names,
            double sumOfRates = 0.999999D, Func<double> randRateFunc = null)
        {
            //get just an array of rates
            var rates = new double[names.Count()];
            var d = new Dictionary<string, double>();

            var rMax = sumOfRates;
            double DfRandRateFunc() => Etx.RationalNumber(0.01, rMax);

            randRateFunc = randRateFunc ?? DfRandRateFunc;

            var l = rates.Sum();
            sumOfRates = Math.Abs(sumOfRates);
            while (l < sumOfRates)
            {
                //pick a random index 
                var idx = Etx.IntNumber(0, rates.Length - 1);

                //get random amount
                var randRate = randRateFunc();
                if (randRate + rates.Sum() > sumOfRates)
                {
                    randRate = sumOfRates - rates.Sum();
                }
                rates[idx] += randRate;

                l = rates.Sum();
            }

            //assign the values over to the dictionary
            var c = 0;
            foreach (var otName in names)
            {
                d.Add(otName.Name, Math.Round(rates[c], 6));
                c += 1;
            }

            return d;
        }

        /// <summary>
        /// Helper method to get a bunch of diminishing random rates mapped to some names.
        /// This differs from the counterpart <see cref="GetNames2RandomRates(System.Collections.Generic.IEnumerable{NoFuture.Rand.Data.Sp.IMereo},double,System.Func{double})"/> because 
        /// every item in <see cref="names"/> will get &apos;something&apos; - no matter how small.
        /// </summary>
        /// <param name="names"></param>
        /// <param name="sumOfRates"></param>
        /// <param name="derivativeSlope">
        /// Is passed into the <see cref="Etx.DiminishingPortions"/> - see its annotation.
        /// </param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetNames2DiminishingRates(IEnumerable<IMereo> names,
            double sumOfRates, double derivativeSlope = -1.0)
        {
            var nms = names.ToList();

            var diminishing = Etx.DiminishingPortions(nms.Count, derivativeSlope);
            var p2r = new Dictionary<string, double>();
            for (var i = 0; i < diminishing.Length; i++)
            {
                p2r.Add(nms[i].Name, sumOfRates * diminishing[i]);
            }

            return p2r;
        }


        /// <summary>
        /// Takes the rates away from <see cref="zeroOutNames"/> and adds the rate to 
        /// one of the other entries in <see cref="names2Rates"/> whose name is not in <see cref="zeroOutNames"/>.
        /// </summary>
        /// <param name="names2Rates"></param>
        /// <param name="zeroOutNames"></param>
        /// <param name="derivativeSlope">
        /// Is passed into the <see cref="Etx.DiminishingPortions"/> - see its annotation.  Which entries
        /// receive the re-located rates is based on a diminishing probability.
        /// </param>
        protected internal Dictionary<string, double> ZeroOutRates(Dictionary<string, double> names2Rates,
            string[] zeroOutNames, double derivativeSlope = -1.0D)
        {
            if (names2Rates == null || !names2Rates.Any() || zeroOutNames == null || !zeroOutNames.Any())
                return names2Rates;

            var reassignNames2Rates = zeroOutNames.Select(n => new Tuple<string, double>(n, 0D)).ToList();
            return ReassignRates(names2Rates, reassignNames2Rates, derivativeSlope);
        }

        /// <summary>
        /// Reassigns the rates in <see cref="names2Rates"/> to the Item2 of the matched tuple in <see cref="reassignNames2Rates"/>
        /// preserving the sum total of <see cref="names2Rates"/> values.
        /// </summary>
        /// <param name="names2Rates"></param>
        /// <param name="reassignNames2Rates"></param>
        /// <param name="derivativeSlope"></param>
        /// <returns></returns>
        protected internal Dictionary<string, double> ReassignRates(Dictionary<string, double> names2Rates,
            Dictionary<string, double> reassignNames2Rates, double derivativeSlope = -1.0D)
        {
            if (reassignNames2Rates == null)
                return names2Rates;
            var listOfTuples = reassignNames2Rates.Select(kv => new Tuple<string, double>(kv.Key, kv.Value)).ToList();
            return ReassignRates(names2Rates, listOfTuples, derivativeSlope);
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
        protected internal Dictionary<string, double> ReassignRates(Dictionary<string, double> names2Rates,
            List<Tuple<string, double>> reassignNames2Rates, double derivativeSlope = -1.0D)
        {
            if (names2Rates == null || !names2Rates.Any() || reassignNames2Rates == null || !reassignNames2Rates.Any())
                return names2Rates;

            var originalSum = names2Rates.Select(kv => kv.Value).Sum();

            var relocateRates = new List<double>();
            var idxNames = new List<string>();

            var n2r = new Dictionary<string, double>();

            bool IsMatch(Tuple<string, double> tuple, string s) =>
                tuple.Item1 != null && String.Equals(tuple.Item1, s, StringComparison.OrdinalIgnoreCase);

            //make temp copies of all the zero'ed out values
            foreach (var k in names2Rates.Keys)
            {
                if (reassignNames2Rates.Any(x => IsMatch(x,k)))
                {
                    foreach (var reassign in reassignNames2Rates.Where(x => IsMatch(x, k)))
                    {
                        var currentRate = names2Rates[k];
                        var diffInRate = currentRate - reassign.Item2;

                        relocateRates.Add(diffInRate);
                        if (n2r.ContainsKey(k))
                            n2r[k] = reassign.Item2;
                        else
                            n2r.Add(k, reassign.Item2);
                    }
                }
                else
                {
                    idxNames.Add(k);
                    n2r.Add(k, names2Rates[k]);
                }
            }

            if (!relocateRates.Any() || !idxNames.Any())
                return names2Rates;

            var diminishing = Etx.DiminishingPortions(idxNames.Count, derivativeSlope);
            var idxName2DiminishingRate = new Dictionary<string, double>();
            for (var i = 0; i < idxNames.Count; i++)
            {
                idxName2DiminishingRate.Add(idxNames[i], diminishing[i]);
            }

            foreach (var relocate in relocateRates)
            {
                var randKey = Etx.DiscreteRange(idxName2DiminishingRate);
                n2r[randKey] += relocate;
            }

            //blow out if caller passed in values that changes the sum-of-rates
            var newSum = n2r.Select(kv => kv.Value).Sum();
            var diffInSum = Math.Abs(newSum - originalSum);
            if(diffInSum >= 0.001 || diffInSum <= -0.001)
                throw new RahRowRagee("Could not preserve the sum-of-rates, " +
                                      $"the original sum was {originalSum} while the new sum is {newSum}");

            return n2r;
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

        /// <summary>
        /// Gets a rate, at random, using the <see cref="NAmerUtil.Equations.ClassicHook"/>
        /// </summary>
        /// <param name="age">
        /// Optional, will use the Person&apos;s Age or the mean age of Americans.
        /// </param>
        /// <returns></returns>
        protected internal virtual double GetRandomRateFromClassicHook(double? age = null)
        {
            //we want age to have an effect on the randomness
            var hookEquation = NAmerUtil.Equations.ClassicHook;
            age = age ?? Person?.Age;

            var ageAtDt = age == null || age <= 0 
                ? NAmerUtil.AVG_AGE_AMERICAN 
                : age.Value;

            //some asymetric percentage based on age
            var yVal = hookEquation.SolveForY(ageAtDt);

            //get something randome near this value
            var randRate = Etx.RandomValueInNormalDist(yVal, 0.01921);

            //its income so it shouldn't be negative by definition
            return randRate <= 0D ? 0D : randRate;
        }

        /// <summary>
        /// Gets January 1st date from negative <see cref="back"/> years from this year&apos; January 1st
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
        /// When the Sum of <see cref="directAssignments"/> exceeds <see cref="sumOfRates"/> then <see cref="sumOfRates"/>
        /// is reassigned to this greater sum.
        /// </summary>
        /// <param name="sumOfRates"></param>
        /// <param name="directAssignments"></param>
        protected internal void ReconcileDiffsInSums(ref double sumOfRates, Dictionary<string, double> directAssignments)
        {
            if (directAssignments == null || !directAssignments.Any())
                return;
            var sumOfDirectAssigns = directAssignments.Select(kv => kv.Value).Sum();
            if (sumOfDirectAssigns > sumOfRates)
                sumOfRates = sumOfDirectAssigns;
        }

        /// <summary>
        /// Breaks the whole span of time for this instance into yearly blocks.
        /// </summary>
        /// <returns></returns>
        protected internal List<Tuple<DateTime, DateTime?>> GetIncomeYearsInDates(DateTime startDate)
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
        /// Helper method to get items by <see cref="groupName"/> items by names 
        /// to a portion of <see cref="sumOfRates"/>
        /// </summary>
        /// <param name="division"></param>
        /// <param name="groupName"></param>
        /// <param name="sumOfRates"></param>
        /// <param name="probability">
        /// Allows calling assembly to control the probability of an expense being zero-ed out.
        /// The default is a 75% chance.
        /// </param>
        /// <param name="derivativeSlope">
        /// Is passed into the <see cref="Etx.DiminishingPortions"/> - see its annotation.
        /// </param>
        /// <param name="possiablyZeroNames"></param>
        /// <returns></returns>
        protected internal virtual Dictionary<string, double> GetNames2RandomRates(DomusOpesDivisions division,
            string groupName, double sumOfRates, Func<int, Etx.Dice, bool> probability,
            double derivativeSlope = -1.0, params string[] possiablyZeroNames)
        {
            List<IMereo> names = null;

            switch (division)
            {
                case DomusOpesDivisions.Income:
                    names = GetIncomeItemNames().Where(e =>
                            String.Equals(e.GetName(KindsOfNames.Group), groupName,
                                StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
                case DomusOpesDivisions.Assets:
                    names = GetAssetItemNames().Where(e =>
                            String.Equals(e.GetName(KindsOfNames.Group), groupName,
                                StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
                case DomusOpesDivisions.Deduction:
                    names = GetDeductionItemNames().Where(e =>
                            String.Equals(e.GetName(KindsOfNames.Group), groupName,
                                StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
                case DomusOpesDivisions.Expense:
                    names = GetExpenseItemNames().Where(e =>
                            String.Equals(e.GetName(KindsOfNames.Group), groupName,
                                StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
            }

            probability = probability ?? Etx.TryBelowOrAt;

            var p2r = GetNames2DiminishingRates(names, sumOfRates, derivativeSlope);

            var randZeros = new List<string>();

            foreach (var item in possiablyZeroNames)
            {
                if (probability(1, Etx.Dice.Four))
                    continue;
                randZeros.Add(item);
            }

            return ZeroOutRates(p2r, randZeros.ToArray());
        }

        /// <summary>
        /// Converts explicit amount into a rate of <see cref="amt"/>
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="explicitAmounts"></param>
        /// <param name="grp"></param>
        /// <returns></returns>
        protected internal static Dictionary<string, double> ConvertToExplicitRates(Pecuniam amt,
            Dictionary<IVoca, Pecuniam> explicitAmounts, string grp)
        {
            const StringComparison OPT = StringComparison.OrdinalIgnoreCase;
            var directAssignRates = new Dictionary<string, double>();
            if (!explicitAmounts.Any(x => String.Equals(x.Key.GetName(KindsOfNames.Group), grp, OPT)))
                return directAssignRates;

            foreach (var n in explicitAmounts.Where(x =>
                String.Equals(x.Key.GetName(KindsOfNames.Group), grp, OPT)))
            {
                var directAssignRate = Math.Round(n.Value.ToDouble() / amt.ToDouble(), 8);
                directAssignRates.Add(n.Key.GetName(KindsOfNames.Legal), directAssignRate);
            }
            return directAssignRates;
        }
    }
}
