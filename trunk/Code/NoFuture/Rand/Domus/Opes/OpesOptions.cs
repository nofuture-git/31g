using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Domus.Opes
{
    /// <summary>
    /// A control object to exercise control over the randomness
    /// </summary>
    public class OpesOptions
    {
        private double _derivativeSlope;

        public bool IsPayingChildSupport { get; set; }
        public bool IsReceivingChildSupport { get; set; }

        public bool IsPayingSpousalSupport { get; set; }
        public bool IsReceivingSpousalSupport { get; set; }

        public bool IsVehiclePaidOff { get; set; }
        public bool IsRenting { get; set; }
        public int NumberOfVehicles { get; set; }
        public int NumberOfCreditCards { get; set; }
        public int TotalNumberOfHouseholdMembers { get; set; }
        public List<int> ChildrenAges { get; set; } = new List<int>();

        public bool HasCreditCards => NumberOfCreditCards > 0;
        public bool HasVehicles => NumberOfVehicles > 0;
        public bool HasChildren => ChildrenAges != null && ChildrenAges.Any();

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// The interval is passed to the created items
        /// </summary>
        public Interval Interval { get; set; }

        /// <summary>
        /// Optional, settings this removes the randomness of the overall
        /// sum of all items in a group.  Each items value maybe random 
        /// but those random values will all add up to this value if its
        /// assigned.
        /// </summary>
        public Pecuniam SumTotal { get; set; }

        /// <summary>
        /// The means to assign an items value directly; thereby, removing
        /// all the randomness of its value.
        /// </summary>
        public List<IMereo> GivenDirectly { get; } = new List<IMereo>();

        /// <summary>
        /// By default, every item will get &apos;something&apos; - add item
        /// names to this to have them assigned to zero at random.
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
        public Func<int, Etx.Dice, bool> DiceRoll { get; set; } = Etx.TryBelowOrAt;

        /// <summary>
        /// Creates a new instance on the heap with the exact same property values as this instance.
        /// </summary>
        /// <returns></returns>
        public OpesOptions GetClone()
        {
            var o = new OpesOptions();

            var pi = GetType().GetProperties(NfConfig.DefaultFlags).Where(p => p.CanWrite).ToList();
            foreach (var p in pi)
            {
                var gVal = p.GetValue(this);
                p.SetValue(o, gVal);
            }

            foreach (var zo in PossibleZeroOuts)
                o.PossibleZeroOuts.Add(zo);

            foreach (var me in GivenDirectly)
                o.GivenDirectly.Add(new Mereo(me));

            return o;
        }
    }
}
