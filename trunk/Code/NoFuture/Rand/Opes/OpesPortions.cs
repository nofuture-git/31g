using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Opes
{
    /// <summary>
    /// Excercises control over the generation of random portions whose sum equals 1.
    /// </summary>
    [Serializable]
    public class OpesPortions
    {
        public const double DF_STD_DEV_PERCENT = 0.0885D;
        protected internal const int DF_ROUND_DECIMAL_PLACES = 5;
        private double _derivativeSlope;

        /// <summary>
        /// Optional, this is only used when the option&apos;s given directly
        /// is not empty - its expected to exceed the total of the option&apos;s given directly.
        /// </summary>
        public double? SumTotal { get; set; }

        protected internal List<Tuple<VocaBase, double>> GivenDirectlyRefactor { get; } = new List<Tuple<VocaBase, double>>();

        /// <summary>
        /// By default, every item will get &apos;some portion&apos;, no matter 
        /// how small - add item names to this to have them assigned to zero at random.
        /// </summary>
        public List<string> PossibleZeroOuts { get; } = new List<string>();

        /// <summary>
        /// Controls the diminishing rates, the closer to zero the faster 
        /// the rates diminish (e.g. -0.2 will have probably over 75 % in the first 
        /// item with the second item having almost all the rest of it and everything
        /// else just getting a tiny sprinkle - values greater than -1.0 tend to flatten
        /// it out).
        /// </summary>
        public double DerivativeSlope
        {
            get
            {
                if (_derivativeSlope <= 0.0001 && _derivativeSlope >= -0.0001)
                    _derivativeSlope = -1.0D;

                return _derivativeSlope;
            }
            set => _derivativeSlope = value;
        }

        /// <summary>
        /// Related to the names in <see cref="PossibleZeroOuts"/> - turns 
        /// possible into actual.
        /// </summary>
        public Func<int, Etx.Dice, bool> DiceRoll { get; set; } = Etx.RandomRollBelowOrAt;

        /// <summary>
        /// Add a directly-assigned item with no randomness
        /// </summary>
        public void AddGivenDirectly(string name, string groupName, double? amount = null)
        {
            GivenDirectlyRefactor.Add(new Tuple<VocaBase, double>(new VocaBase(name, groupName), amount ?? 0D));
        }

        /// <summary>
        /// Add a directly-assigned item with no randomness
        /// </summary>
        public void AddGivenDirectly(string name, double? amount = null)
        {
            GivenDirectlyRefactor.Add(new Tuple<VocaBase, double>(new VocaBase(name), amount ?? 0D));
        }

        /// <summary>
        /// Helper method to add a range of directly-assigned items
        /// </summary>
        /// <param name="name2Values"></param>
        protected internal void AddGivenDirectlyRange(IEnumerable<Tuple<string, string, double>> name2Values)
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
        public int GivenDirectlyCount => GivenDirectlyRefactor.Count;

        /// <summary>
        /// Asserts if any given-directly items have been added.
        /// </summary>
        /// <returns></returns>
        public bool AnyGivenDirectly()
        {
            return GivenDirectlyRefactor.Any();
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
            return GivenDirectlyRefactor.Any(g =>
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
            return GivenDirectlyRefactor.Any(g => string.Equals(g.Item1.Name, name, OPT));
        }

        /// <summary>
        /// Helper method to assert if any option&apos;s given directly have group name <see cref="groupName"/>
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool AnyGivenDirectlyOfGroupName(string groupName)
        {
            return GivenDirectlyRefactor.Any(x => x.Item1.AnyOfKindAndValue(KindsOfNames.Group, groupName));
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
        public List<Tuple<string, double>> GetNames2Portions(string[] itemOrGroupNames)
        {
            const StringComparison STR_OPT = StringComparison.OrdinalIgnoreCase;

            //make this required
            if (itemOrGroupNames == null || !itemOrGroupNames.Any())
                throw new ArgumentNullException(nameof(itemOrGroupNames));

            var givenDirectlyItems = GivenDirectlyRefactor ?? new List<Tuple<VocaBase, double>>();

            //immediately reduce this to only the items present in 'itemNames'
            givenDirectlyItems = givenDirectlyItems.Where(gd =>
                itemOrGroupNames.Any(n => string.Equals(gd.Item1.Name, n, STR_OPT))).ToList();

            //get the direct assign's total
            var givenDirectTotal = givenDirectlyItems.Where(x => x?.Item2 != null)
                .Select(x => Math.Round(Math.Abs(x.Item2), DF_ROUND_DECIMAL_PLACES)).Sum();

            //get total given by the caller if any
            var sumTotalR = SumTotal.GetValueOrDefault(0);

            //get a random rate for all item names
            var randPortions = Etx.RandomDiminishingPortions(itemOrGroupNames.Length, DerivativeSlope);

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
            var possibleZeroOuts = PossibleZeroOuts.Distinct().ToList();
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
                    var diceRoll = DiceRoll(3, Etx.Dice.Four);

                    //these predicates are filters
                    var isAlreadyPresent = actualZeroOuts.Any(z => z.Item1 == pzo);
                    var isInGivenDirectly = givenDirectlyItems.Any(x =>
                        String.Equals(x.Item1.Name, pzo, STR_OPT));

                    if (diceRoll && !isAlreadyPresent && !isInGivenDirectly)
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

                randDict = ReassignRates(randDict, actualZeroOuts, DerivativeSlope);
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
