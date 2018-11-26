using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core.Enums;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Core
{
    /// <summary>
    /// Excercises control over the generation of random portions whose sum equals 1.
    /// </summary>
    /// <remarks>
    /// See <see cref="GetNames2Portions"/> for more details
    /// </remarks>
    [Serializable]
    public class RandPortions
    {
        internal const int DF_ROUND_DECIMAL_PLACES = 5;
        private double _derivativeSlope = -1.0;
        private readonly Predicate<string> _defaultDice = (d) => Etx.RandomRollBelowOrAt(3, Etx.Dice.Four);

        /// <summary>
        /// A discrete way to describe exponential slope.
        /// </summary>
        public enum DiminishingRate
        {
            /// <summary>
            /// The first item will receive about 33% of the total.
            /// </summary>
            Normal,

            /// <summary>
            /// The first item will receive about 15%-10% of the total
            /// </summary>
            VerySlow,

            /// <summary>
            /// The first item will receive about 25%-15% of the total.
            /// </summary>
            Slow,

            /// <summary>
            /// The first item will receive about 60%-50% of the total
            /// </summary>
            Fast,

            /// <summary>
            /// The first item will receive 80%-70% of the total
            /// </summary>
            VeryFast
        }

        /// <summary>
        /// Optional, this is only used when the option&apos;s given directly
        /// is not empty - its expected to exceed the total of the option&apos;s given directly.
        /// </summary>
        public virtual double? SumTotal { get; set; }

        /// <summary>
        /// Direct access to the underlying collection is protected.
        /// </summary>
        protected List<Tuple<VocaBase, double>> GivenDirectly { get; } = new List<Tuple<VocaBase, double>>();

        /// <summary>
        /// Mapping of possible-zero-out name to its predicate
        /// </summary>
        protected Dictionary<string, Predicate<string>> Pzos2Prob { get; } = new Dictionary<string, Predicate<string>>();

        /// <summary>
        /// Add a name to the list of what may-or-may-not be 
        /// assigned a 0.0 portion within <see cref="GetNames2Portions"/> at the default probability.
        /// </summary>
        public virtual void AddPossibleZeroOuts(params string[] pzos)
        {
            foreach(var pzo in pzos)
            {
                if(Pzos2Prob.ContainsKey(pzo))
                    continue;
                Pzos2Prob.Add(pzo, _defaultDice);
            }
        }

        /// <summary>
        /// Add a name to the list of what may-or-may-not be 
        /// assigned a 0.0 portion within <see cref="GetNames2Portions"/> 
        /// at the <see cref="prob"/> probability.
        /// </summary>
        public virtual void AddPossibleZeroOuts(Predicate<string> prob, params string[] pzos)
        {
            prob = prob ?? _defaultDice;
            foreach (var pzo in pzos)
            {
                if (Pzos2Prob.ContainsKey(pzo))
                    Pzos2Prob[pzo] = prob;
                else
                    Pzos2Prob.Add(pzo, prob);
            }
        }

        /// <summary>
        /// Assert if anything has been added to the possible-zero-out items
        /// </summary>
        public bool AnyPossibleZeroOuts => Pzos2Prob.Any();

        /// <summary>
        /// The current count of possible zero out items.
        /// </summary>
        public int PossibleZeroOutCount => Pzos2Prob.Count;

        /// <summary>
        /// Determines how fast the portions drop off after the first one.
        /// </summary>
        public virtual DiminishingRate Rate
        {
            get
            {
                switch (_derivativeSlope)
                {
                    case -0.1:
                        return DiminishingRate.VeryFast;
                    case -0.3:
                        return DiminishingRate.Fast;
                    case -10.0:
                        return DiminishingRate.VerySlow;
                    case -3.0:
                        return DiminishingRate.Slow;
                    default:
                        return DiminishingRate.Normal;
                }
            }
            set
            {
                var r = value;
                switch (r)
                {
                    case DiminishingRate.VeryFast:
                        _derivativeSlope = -0.1;
                        break;
                    case DiminishingRate.Fast:
                        _derivativeSlope = -0.3;
                        break;
                    case DiminishingRate.VerySlow:
                        _derivativeSlope = -10.0;
                        break;
                    case DiminishingRate.Slow:
                        _derivativeSlope = -3.0;
                        break;
                    default:
                        _derivativeSlope = -1.0;
                        break;
                }
            }
        }

        /// <summary>
        /// Add a directly-assigned item with no randomness
        /// </summary>
        /// <param name="name">Some direct name of the item</param>
        /// <param name="groupName">A name used for grouping items</param>
        /// <param name="amount">Optional, the numerical value assoc. to this named item, default is zero.</param>
        public virtual void AddGivenDirectly(string name, string groupName, double? amount = null)
        {
            GivenDirectly.Add(new Tuple<VocaBase, double>(new VocaBase(name, groupName), amount ?? 0D));
        }

        /// <summary>
        /// Add item which will be given zero portion.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="groupName"></param>
        public virtual void AddZeroPortion(string name, string groupName)
        {
            GivenDirectly.Add(new Tuple<VocaBase, double>(new VocaBase(name, groupName), 0D));
        }

        /// <summary>
        /// Same as its overload, just without a group name
        /// </summary>
        public virtual void AddGivenDirectly(string name, double? amount = null)
        {
            GivenDirectly.Add(new Tuple<VocaBase, double>(new VocaBase(name), amount ?? 0D));
        }

        /// <summary>
        /// Helper method to add a range of directly-assigned items
        /// </summary>
        /// <param name="name2Values"></param>
        public virtual void AddGivenDirectlyRange(IEnumerable<Tuple<string, string, double>> name2Values)
        {
            if (name2Values == null || !name2Values.Any())
                return;

            foreach (var n2v in name2Values)
            {
                AddGivenDirectly(n2v.Item1, n2v.Item2, n2v.Item3);
            }
        }

        /// <summary>
        /// Gets the current count of given-directly items
        /// </summary>
        public int GivenDirectlyCount => GivenDirectly.Count;

        /// <summary>
        /// Asserts if any given-directly items have been added.
        /// </summary>
        /// <returns></returns>
        public bool AnyGivenDirectly()
        {
            return GivenDirectly.Any();
        }

        /// <summary>
        /// Helper method to assert if any items have been added to option&apos;s given directly
        /// by name and group
        /// </summary>
        /// <param name="name"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool AnyGivenDirectlyOfNameAndGroup(string name, string groupName)
        {
            const StringComparison OPT = StringComparison.OrdinalIgnoreCase;
            return GivenDirectly.Any(g =>
                string.Equals(g.Item1.Name, name, OPT) && string.Equals(g.Item1.GetName(KindsOfNames.Group), groupName, OPT));
        }

        /// <summary>
        /// Helper method to assert if any items have been added to option&apos;s given directly
        /// with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool AnyGivenDirectlyOfName(string name)
        {
            const StringComparison OPT = StringComparison.OrdinalIgnoreCase;
            return GivenDirectly.Any(g => string.Equals(g.Item1.Name, name, OPT));
        }

        /// <summary>
        /// Helper method to assert if any option&apos;s given directly have group name <see cref="groupName"/>
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool AnyGivenDirectlyOfGroupName(string groupName)
        {
            return GivenDirectly.Any(x => x.Item1.AnyOfKindAndValue(KindsOfNames.Group, groupName));
        }

        /// <summary>
        /// The capital method to get random portions for a list of names.
        /// </summary>
        /// <param name="itemOrGroupNames">
        /// These are the names with which a random portion is supposed to be generated.
        /// </param>
        /// <returns>
        /// A set of item names to some percent where the sum of all the name&apos;s portion is 1 (i.e. 100%).
        /// </returns>
        /// <remarks>
        /// <![CDATA[
        /// FAQ
        /// Q: What happens if no names are given?
        /// A: An ArgumentNullException is thrown.
        /// 
        /// Q: What happens if you just invoke it with no options whatsoever (meaning, just instantiate it and call this)?
        /// A: Then every item-name gets a truly random value - the sum of which equals 1.
        /// 
        /// Q: How does SumTotal work with GivenDirectly?
        /// A: The only time it matters is when SumTotal exceeds the GivenDirectly's cumulative total; furthermore, SumTotal has no use
        ///    when the GivenDirectly is empty since we are getting random portions and not random values.
        ///    
        /// Q: So what happens when there are GivenDirectly values and no SumTotal?
        /// A: Then its just doing the math and nothing is random.
        /// 
        /// Q: What happens if SumTotal is less-than GivenDirectly's cumulative total.
        /// A: Then SumTotal is just reassigned to the cumulative total and it again just doing the math - nothing random.
        /// 
        /// Q: What about when SumTotal exceeds cumulative total?
        /// A: Then the excess amount is what is used to generate random portions for the other item-names not present 
        ///    in GivenDirectly.
        /// 
        /// Q: What if there are no items in GivenDirectly?
        /// A: It just resorts back to all item-names having a random portion.
        /// 
        /// Q: What happens when the item-names don't match the names present in GivenDirectly?
        /// A: The output is always tied to the item-names - any GivenDirectly not found in the item-names is ignored.
        /// 
        /// Q: Can a GivenDirectly entry be assigned an value of zero?
        /// A: Yes, and that is their main purpose to selectively remove randomness for certian item-names - recall 
        ///    that the function wants to assign some portion to every item-name, no matter how small.
        /// 
        /// Q: What happens if I force every item to be zero using GivenDirectly?
        /// A: An exception is thrown - the function cannot satisfy portions whose sum is equal to both zero and one.
        /// 
        /// Q: How do the PossibleZeroOuts play with explict values on GivenDirectly?
        /// A: The PossiableZeroOuts are only considered when they are not present in the GivenDirectly.
        /// 
        /// Q: What if the SumTotal exceeds the GivenDirectly's sum but all the other item-names are present 
        ///    in the PossiablyZeroOut's and, it just so happens, that they all get selected to be zero-ed out?
        /// A: It leaves one to receive the excess - in effect forcing the dice role to be false for at least 
        ///    one of the PossiablyZeroOuts in this case no matter the odds.
        /// ]]>
        /// </remarks>
        public virtual List<Tuple<string, double>> GetNames2Portions(string[] itemOrGroupNames)
        {
            const StringComparison STR_OPT = StringComparison.OrdinalIgnoreCase;

            //make this required
            if (itemOrGroupNames == null || !itemOrGroupNames.Any())
                throw new ArgumentNullException(nameof(itemOrGroupNames));

            var givenDirectlyItems = GivenDirectly ?? new List<Tuple<VocaBase, double>>();

            //immediately reduce this to only the items present in 'itemNames'
            givenDirectlyItems = givenDirectlyItems.Where(gd =>
                itemOrGroupNames.Any(n => string.Equals(gd.Item1.Name, n, STR_OPT))).ToList();

            //get the direct assign's total
            var givenDirectTotal = givenDirectlyItems.Where(x => x?.Item2 != null)
                .Select(x => Math.Round(Math.Abs(x.Item2), DF_ROUND_DECIMAL_PLACES)).Sum();

            //get total given by the caller if any
            var sumTotalR = SumTotal.GetValueOrDefault(0);

            //get a random rate for all item names
            var randPortions = Etx.RandomDiminishingPortions(itemOrGroupNames.Length, _derivativeSlope);

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
            var possibleZeroOuts = Pzos2Prob.Keys.Distinct().ToList();
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
                    var diceRoll = Pzos2Prob.ContainsKey(pzo) ? Pzos2Prob[pzo] : _defaultDice;

                    //these predicates are filters
                    var isAlreadyPresent = actualZeroOuts.Any(z => z.Item1 == pzo);
                    var isInGivenDirectly = givenDirectlyItems.Any(x =>
                        string.Equals(x.Item1.Name, pzo, STR_OPT));

                    if (diceRoll(pzo) && !isAlreadyPresent && !isInGivenDirectly)
                        actualZeroOuts.Add(new Tuple<string, double>(pzo, 0.0D));
                }
            }

            //apply any GivenDirectly's of zero like PossiableZeroOuts
            foreach (var dr in givenDirectlyItems.Where(o => o.Item2 == 0))
            {
                if (actualZeroOuts.All(z => z.Item1 != dr.Item1.Name))
                    actualZeroOuts.Add(new Tuple<string, double>(dr.Item1.Name, 0.0D));
            }

            //zero out all the select names
            if (actualZeroOuts.Any())
            {
                //make one last check that we aren't zero'ing out everything
                if (actualZeroOuts.Count == itemOrGroupNames.Length)
                    throw new WatDaFookIzDis("A sum total of 1 cannot be perserved when " +
                                             "all items have been directly assigned to 0.");

                randDict = ReassignRates(randDict, actualZeroOuts, _derivativeSlope);
            }

            //there is nothing left to do so leave
            if (givenDirectTotal == 0.0D)
            {
                return randDict.Select(kv => new Tuple<string, double>(kv.Key, kv.Value)).ToList();
            }

            //we will need a denominator if the caller didn't give one use the sum what they did give
            var total = sumTotalR;

            //if caller gives a sumtotal in which all the GivenDirectly won't fit then up the total to make it fit
            if (total < givenDirectTotal)
                total = givenDirectTotal;

            //get a dict of group names to all 0.0D
            var calcDict = new Dictionary<string, double>();

            //get the sum of each given directly item
            foreach (var d in givenDirectlyItems)
            {
                var dName = d.Item1.Name;
                if (string.IsNullOrWhiteSpace(dName)
                    || d.Item2 == 0)
                    continue;
                if (calcDict.ContainsKey(dName))
                    calcDict[dName] += Math.Abs(d.Item2);
                else
                    calcDict.Add(dName, Math.Abs(d.Item2));
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
                    if (calcMap.Any(t => string.Equals(t.Item1, k, STR_OPT)))
                        continue;
                    calcMap.Add(new Tuple<string, double>(k, 0.0D));
                }
                return calcMap;
            }

            //convert it to a dictionary 
            return ReassignRates(randDict, calcMap).Select(kv => new Tuple<string, double>(kv.Key, kv.Value)).ToList();
        }

        private static bool IsCloseEnoughToOne(double testValue)
        {
            var calcMapSumRemainder = 1 - Math.Abs(testValue);
            return calcMapSumRemainder <= 0.00001 && calcMapSumRemainder >= -0.00001;
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
                DF_ROUND_DECIMAL_PLACES - 1));

            if (reassignHasEveryKey && reassignIsSumOfOne)
                return reassignNames2Rates.ToDictionary(t => t.Item1, t => t.Item2);

            var relocateRates = new List<double>();
            var idxNames = new List<string>();

            var n2r = new Dictionary<string, double>();

            bool IsMatch(Tuple<string, double> tuple, string s) =>
                tuple.Item1 != null && string.Equals(tuple.Item1, s, StringComparison.OrdinalIgnoreCase);

            //make temp copies of all the differences in reassignment
            foreach (var k in names2Rates.Keys)
            {
                var currentRate = Math.Round(names2Rates[k], DF_ROUND_DECIMAL_PLACES);
                if (reassignNames2Rates.Any(x => IsMatch(x, k)))
                {
                    foreach (var reassign in reassignNames2Rates.Where(x => IsMatch(x, k)))
                    {
                        var diffInRate = Math.Round(currentRate - reassign.Item2, DF_ROUND_DECIMAL_PLACES);
                        relocateRates.Add(diffInRate);
                        if (n2r.ContainsKey(k))
                            n2r[k] = Math.Round(reassign.Item2, DF_ROUND_DECIMAL_PLACES);
                        else
                            n2r.Add(k, Math.Round(reassign.Item2, DF_ROUND_DECIMAL_PLACES));
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
                    decrement = Math.Round(n2r[randKey], DF_ROUND_DECIMAL_PLACES);
                }
                n2r[randKey] -= decrement;
                totalNegative = Math.Round(totalNegative - decrement, DF_ROUND_DECIMAL_PLACES);
            }

            n2r = n2r.ToDictionary(kv => kv.Key, kv => Math.Round(kv.Value, DF_ROUND_DECIMAL_PLACES - 1));
            return n2r;
        }
    }
}
