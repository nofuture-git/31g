﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Domus.Pneuma;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Domus.Opes
{
    public abstract class WealthBase
    {
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

        #region ctors
        protected WealthBase(NorthAmerican american, bool isRenting = false)
        {
            if (american == null)
            {
                _factors = new NorthAmericanFactors(null);
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

        public CreditScore CreditScore { get; protected set; }

        /// <summary>
        /// Exposes the calculated factors using the <see cref="NorthAmerican"/> passed into 
        /// the ctor.
        /// </summary>
        public NorthAmericanFactors Factors => _factors;

        protected internal virtual NorthAmerican Person => _amer;

        #region methods
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
        /// Optional, allows caller to specify how <see cref="NorthAmericanFactors.NetWorthFactor"/>
        /// is applied to the calculated base yearly income amount.  
        /// The default is a simple product of the two (i.e. basePay * netWorthFactor).
        /// </param>
        /// <param name="stdDevInUsd">
        /// Optional, a randomizes the calculated value around a mean.
        /// </param>
        /// <returns></returns>
        public Pecuniam GetYearlyIncome(DateTime? dt = null, Pecuniam min = null,
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
            return _incomeItemNames = _incomeItemNames ??  GetDomusOpesItemNames("//income//mereo");
        }

        /// <summary>
        /// Gets the <see cref="IMereo"/> Deduction items
        /// from <see cref="Data.TreeData.UsDomusOpes"/>
        /// </summary>
        /// <returns></returns>
        public static IMereo[] GetDeductionItemNames()
        {
            return _deductionItemNames = _deductionItemNames ?? GetDomusOpesItemNames("//deduction//mereo");
        }

        /// <summary>
        /// Gets the <see cref="IMereo"/> Expense items 
        /// (i.e. household budget) from <see cref="Data.TreeData.UsDomusOpes"/>
        /// </summary>
        /// <returns></returns>
        public static IMereo[] GetExpenseItemNames()
        {
            return _expenseItemNames = _expenseItemNames ?? GetDomusOpesItemNames("//expense//mereo");
        }

        /// <summary>
        /// Gets the <see cref="IMereo"/> Expense items 
        /// (i.e. real and private property) 
        /// from <see cref="Data.TreeData.UsDomusOpes"/>
        /// </summary>
        /// <returns></returns>
        public static IMereo[] GetAssetItemNames()
        {
            return _assetItemNames = _assetItemNames ?? GetDomusOpesItemNames("//assets//mereo");
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
                    if (string.IsNullOrWhiteSpace(eg))
                        continue;
                    egs.Add(eg);
                }
            }

            mereo = new Mereo(itemName);
            if (!string.IsNullOrWhiteSpace(abbrev))
                mereo.UpsertName(KindsOfNames.Abbrev, abbrev);
            if (!string.IsNullOrWhiteSpace(groupName))
                mereo.UpsertName(KindsOfNames.Group, groupName);
            if (egs.Any())
            {
                foreach (var eg in egs)
                    mereo.ExempliGratia.Add(eg);
            }

            return !string.IsNullOrWhiteSpace(itemName);
        }

        internal static IMereo[] GetDomusOpesItemNames(string xPath)
        {
            if (string.IsNullOrWhiteSpace(xPath))
                return null;

            var xml = Data.TreeData.UsDomusOpes;
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

        protected Pecuniam CalcValue(Pecuniam pecuniam, double d)
        {
            return Math.Round(pecuniam.ToDouble() * d, 2).ToPecuniam();
        }

        protected internal virtual Dictionary<string, double> GetRandomRates(IEnumerable<IMereo> names,
            double sumOfRates = 0.999999D, Func<double> randRateFunc = null)
        {
            //get just an array of rates
            var rates = new double[names.Count()];

            var rMax = sumOfRates;
            double DfRandRateFunc() => Etx.RationalNumber(0.01, rMax);

            randRateFunc = randRateFunc ?? DfRandRateFunc;

            var l = rates.Sum();
            sumOfRates = sumOfRates < 0D ? 0D : sumOfRates;
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
            var d = new Dictionary<string, double>();
            var c = 0;
            foreach (var otName in names)
            {
                d.Add(otName.Name, Math.Round(rates[c], 6));
                c += 1;
            }

            return d;
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
        #endregion

        /// <summary>
        /// Gets a rate, at random, using the <see cref="NAmerUtil.Equations.ClassicHook"/>
        /// </summary>
        /// <param name="age">
        /// Optional, will use the Person&apos;s Age or the mean age of the United States.
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
    }
}
