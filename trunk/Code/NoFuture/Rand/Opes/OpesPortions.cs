using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
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

        /// <summary>
        /// The means to assign an items value directly; thereby, removing
        /// all the randomness of its value - the resulting portion will be equal
        /// to the <see cref="IMereo.Value"/> over <see cref="SumTotal"/>
        /// </summary>
        public List<IMereo> GivenDirectly { get; } = new List<IMereo>();

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
            GivenDirectly.Add(new Mereo(name, groupName) {Value = amount});
        }

        public void AddGivenDirectly(string name, Pecuniam amount)
        {
            GivenDirectly.Add(new Mereo(name) { Value = amount });
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
    }
}
