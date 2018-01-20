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
    /// <inheritdoc cref="OpesPortions" />
    /// <inheritdoc cref="ITempore" />
    /// <summary>
    /// A control object to exercise control over the randomness of 
    /// the various types of Domus.Opes
    /// </summary>
    [Serializable]
    public class OpesOptions : OpesPortions, ITempore
    {
        #region properties

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
        /// Used to determine if a car payment is applicable.
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
        public CityArea HomeLocation { get; set; }

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

        public DateTime Inception { get; set; }
        public DateTime? Terminus { get; set; }

        /// <summary>
        /// The interval is passed to the created items
        /// </summary>
        public Interval Interval { get; set; }

        #endregion

        #region methods
        /// <summary>
        /// The current age of the person to whom this wealth belongs.
        /// </summary>
        public int GetCurrentAge()
        {
            if (BirthDate == DateTime.MinValue)
                return (int) Math.Round(AmericanData.AVG_AGE_AMERICAN);
            return Etc.CalcAge(BirthDate);
        }

        /// <summary>
        /// Gets the major region of this instances <see cref="HomeLocation"/>
        /// </summary>
        public AmericanRegion GetUsCardinalRegion()
        {
            var usCityArea = HomeLocation as UsCityStateZip;
            return UsStateData.GetStateData(usCityArea?.State?.ToString())?.Region ?? AmericanRegion.Midwest;
        }

        /// <summary>
        /// Gets the current age of all children based on 
        /// the assigned Birth Dates present in <see cref="ChildrenDobs"/>
        /// </summary>
        public List<int> GetChildrenAges()
        {
            var ages = new List<int>();
            foreach (var dob in ChildrenDobs)
                ages.Add(Etc.CalcAge(dob, DateTime.Today));
            return ages;
        }

        public bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = Inception <= dt;
            var beforeOrOnToDt = Terminus == null || Terminus.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }

        /// <summary>
        /// Helper method to assert if any items have been added to <see cref="OpesPortions.GivenDirectly"/>
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
        /// Helper method to assert if any items have been added to <see cref="OpesPortions.GivenDirectly"/>
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

            o.DerivativeSlope = DerivativeSlope;
            o.SumTotal = SumTotal == null ? null : new Pecuniam(SumTotal.Amount);
            o.DiceRoll = DiceRoll;

            return o;
        }

        /// <summary>
        /// Creates a wealth options object at random
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static OpesOptions RandomOpesOptions(string firstName = null, string lastName = null)
        {
            var name = new VocaBase();
            lastName = lastName ?? Etx.RandomWord();
            firstName = firstName ?? Etx.RandomWord();
            name.UpsertName(KindsOfNames.First, firstName);
            name.UpsertName(KindsOfNames.Surname, lastName);

            var opt = new OpesOptions
            {
                BirthDate = Etx.RandomAdultBirthDate(),
                Gender = Etx.RandomCoinToss() ? Gender.Female : Gender.Male,
                Inception = Etx.RandomDate(-1, null, true),
                HomeLocation = CityArea.RandomAmericanCity(),
                IsRenting = Etx.RandomCoinToss(),
                Personality = Pneuma.Personality.RandomPersonality(),
                Race = Etx.RandomPickOne(AmericanRacePercents.NorthAmericanRaceAvgs),
                EducationLevel = OccidentalEdu.HighSchool | OccidentalEdu.Grad,
                MaritialStatus = Etx.RandomCoinToss() ? Gov.MaritialStatus.Married : Gov.MaritialStatus.Single,
                NumberOfCreditCards = Etx.RandomInteger(0, 3),
                NumberOfVehicles = 1,
                PersonsName = name,
                Interval = Interval.Annually,
                IsVehiclePaidOff = false
            };
            return opt;
        }

        #endregion
    }
}
