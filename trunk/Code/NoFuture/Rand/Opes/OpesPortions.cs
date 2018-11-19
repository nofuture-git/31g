using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Sp;

namespace NoFuture.Rand.Opes
{
    /// <summary>
    /// Excercises control over the generation of random portions whose sum equals 1.
    /// </summary>
    [Serializable]
    public class OpesPortions
    {
        private double _derivativeSlope;

        /// <summary>
        /// Optional, this is only used when the option&apos;s given directly
        /// is not empty - its expected to exceed the total of the option&apos;s given directly.
        /// </summary>
        public Pecuniam SumTotal { get; set; }

        protected internal List<Tuple<VocaBase, Pecuniam>> GivenDirectly { get; } = new List<Tuple<VocaBase, Pecuniam>>();

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

        public void AddGivenDirectly(string name, string groupName, Pecuniam amount)
        {
            GivenDirectly.Add(new Tuple<VocaBase, Pecuniam>(new VocaBase(name, groupName),amount));
        }

        public void AddGivenDirectly(string name, Pecuniam amount)
        {
            GivenDirectly.Add(new Tuple<VocaBase, Pecuniam>(new VocaBase(name),amount));
        }

        public void AddGivenDirectlyZero(string name, string groupName)
        {
            if(string.IsNullOrWhiteSpace(groupName))
                AddGivenDirectly(name, Pecuniam.Zero);
            else
                AddGivenDirectly(name, groupName, Pecuniam.Zero);
        }

        public void AddGivenDirectlyRange(IEnumerable<Tuple<string, string, Pecuniam>> name2Values)
        {
            if (name2Values == null || !name2Values.Any())
                return;

            foreach (var n2v in name2Values)
            {
                AddGivenDirectly(n2v.Item1, n2v.Item2, n2v.Item3);
            }
        }

        public bool AnyGivenDirectly()
        {
            return GivenDirectly.Any();
        }

        public int GivenDirectlyCount => GivenDirectly.Count;


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
    }
}
