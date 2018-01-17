using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus.Pneuma;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Geo.US;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Domus.Opes
{
    /// <inheritdoc />
    /// <summary>
    /// A control object to exercise control over the randomness of 
    /// the various types of Domus.Opes
    /// </summary>
    [Serializable]
    public class OpesOptions : ITempore
    {
        private double _derivativeSlope;

        /// <summary>
        /// A judgement related expense - this is too complicated 
        /// to be determined randomly
        /// </summary>
        public bool IsPayingChildSupport { get; set; }

        /// <summary>
        /// A judgement related expense - this is too complicated 
        /// to be determined randomly
        /// </summary>
        public bool IsPayingSpousalSupport { get; set; }

        /// <summary>
        /// Used to determine if a car payment is applicable - depends on 
        /// <see cref="NumberOfVehicles"/> being greater than zero
        /// </summary>
        public bool IsVehiclePaidOff { get; set; }

        /// <summary>
        /// Used to determine rent versus mortgage of the person to whom this wealth belongs
        /// </summary>
        public bool IsRenting { get; set; }

        /// <summary>
        /// The total number of vehicles of the person to whom this wealth belongs
        /// </summary>
        public int NumberOfVehicles { get; set; }

        /// <summary>
        /// The total number of credit cards to whom this wealth belongs
        /// </summary>
        public int NumberOfCreditCards { get; set; }

        /// <summary>
        /// The total number of household members in which this person is also member
        /// </summary>
        public int TotalNumberOfHouseholdMembers { get; set; }

        /// <summary>
        /// This person&apos;s children&apos;s birth dates to whom this wealth belongs
        /// </summary>
        public List<DateTime> ChildrenDobs { get; } = new List<DateTime>();

        /// <summary>
        /// The personality of the person to whom this wealth belongs
        /// </summary>
        public IPersonality Personality { get; set; }

        /// <summary>
        /// The home city-state of the person to whom this wealth belongs
        /// </summary>
        public CityArea GeoLocation { get; set; }

        /// <summary>
        /// The birth date of the person to whom this wealth belongs
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// The gender of the person to whom this wealth belongs
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// The education level of the person to whom this wealth belongs
        /// </summary>
        public OccidentalEdu? EducationLevel { get; set; }

        /// <summary>
        /// The race of the person to whom this wealth belongs
        /// </summary>
        public NorthAmericanRace? Race { get; set; }

        /// <summary>
        /// The marital status of the person to whom this wealth belongs
        /// </summary>
        public MaritialStatus? MaritialStatus { get; set; }

        /// <summary>
        /// The name of the person to whom this wealth belongs
        /// </summary>
        public IVoca PersonsName { get; set; }

        /// <summary>
        /// The current age of the person to whom this wealth belongs.
        /// </summary>
        public int CurrentAge
        {
            get
            {
                if (BirthDate == DateTime.MinValue)
                    return (int)Math.Round(AmericanData.AVG_AGE_AMERICAN);
                return Etc.CalcAge(BirthDate);
            }
        }

        /// <summary>
        /// Gets the major region of this instances <see cref="GeoLocation"/>
        /// </summary>
        public AmericanRegion UsCardinalRegion
        {
            get
            {
                var usCityArea = GeoLocation as UsCityStateZip;
                return UsStateData.GetStateData(usCityArea?.State?.ToString())?.Region ?? AmericanRegion.Midwest;
            }
        }

        /// <summary>
        /// Gets the current age of all children based on 
        /// the assigned Birth Dates present in <see cref="ChildrenDobs"/>
        /// </summary>
        public List<int> ChildrenAges
        {
            get
            {
                var ages = new List<int>();
                foreach(var dob in ChildrenDobs)
                    ages.Add(Etc.CalcAge(dob, DateTime.Today));
                return ages;
            }
        }

        /// <summary>
        /// Asserts that <see cref="NumberOfCreditCards"/> is greater than zero
        /// </summary>
        public bool HasCreditCards => NumberOfCreditCards > 0;

        /// <summary>
        /// Asserts that <see cref="NumberOfVehicles"/> is greater than zero
        /// </summary>
        public bool HasVehicles => NumberOfVehicles > 0;

        /// <summary>
        /// Asserts this instance of options has children based on 
        /// the assigned birth dates 
        /// </summary>
        public bool HasChildren => ChildrenAges != null && ChildrenAges.Any();

        public DateTime Inception { get; set; }
        public DateTime? Terminus { get; set; }
        public bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = Inception <= dt;
            var beforeOrOnToDt = Terminus == null || Terminus.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }

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
        public Func<int, Etx.Dice, bool> DiceRoll { get; set; } = Etx.RandomRollBelowOrAt;

        /// <summary>
        /// Helper method to assert if any items have been added to <see cref="GivenDirectly"/>
        /// by name and group
        /// </summary>
        /// <param name="name"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool AnyGivenDirectlyOfNameAndGroup(string name, string groupName)
        {
            const StringComparison OPT = StringComparison.OrdinalIgnoreCase;
            return GivenDirectly.Any(g =>
                string.Equals(g.Name, name, OPT) && string.Equals(g.GetName(KindsOfNames.Group), groupName, OPT));
        }

        /// <summary>
        /// Helper method to assert if any items have been added to <see cref="GivenDirectly"/>
        /// with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool AnyGivenDirectlyOfName(string name)
        {
            const StringComparison OPT = StringComparison.OrdinalIgnoreCase;
            return GivenDirectly.Any(g => string.Equals(g.Name, name, OPT));
        }

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

            foreach(var ca in ChildrenDobs)
                o.ChildrenDobs.Add(ca);

            return o;
        }
    }
}
