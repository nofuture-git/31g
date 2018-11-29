using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Geo.US;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US;
using NoFuture.Rand.Pneuma;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Enums;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Opes.US
{
    /// <inheritdoc cref="RandPortions" />
    /// <inheritdoc cref="ITempore" />
    /// <summary>
    /// An object to exercise control over the randomness of 
    /// the various types of Domus.Opes
    /// </summary>
    [Serializable]
    public class AmericanDomusOpesOptions : RandPortions, ITempore
    {
        private CityArea _cityArea;

        public AmericanDomusOpesOptions(AmericanFactorOptions factorOptions = null)
        {
            FactorOptions = factorOptions ?? AmericanFactorOptions.RandomFactorOptions();
        }

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
        public CityArea HomeLocation
        {
            get => _cityArea;
            set
            {
                _cityArea = value;
                if (!(_cityArea is UsCityStateZip usCityArea))
                    return;
                FactorOptions.Region =
                    UsStateData.GetStateData(usCityArea.StateName)?.Region ?? AmericanRegion.Midwest;
            }
        }

        /// <summary>
        /// The options related to the american factor multipliers
        /// </summary>
        public AmericanFactorOptions FactorOptions { get; }

        /// <summary>
        /// The name of the person to whom this wealth belongs
        /// </summary>
        public IVoca PersonsName { get; set; }

        public DateTime Inception { get; set; }

        /// <summary>
        /// Helper method to avoid checking for <see cref="Inception"/> as min-date-time
        /// </summary>
        public DateTime InceptionOrToday => Inception == DateTime.MinValue ? DateTime.Today : Inception;

        public DateTime? Terminus { get; set; }

        /// <summary>
        /// The interval is passed to the created items
        /// </summary>
        public Interval? Interval => DueFrequency.ToInterval();

        public TimeSpan? DueFrequency { get; set; }

        #endregion

        #region methods

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
        /// Creates a new instance on the heap with the exact same property values as this instance.
        /// </summary>
        /// <returns></returns>
        public AmericanDomusOpesOptions GetClone()
        {
            var o = new AmericanDomusOpesOptions(FactorOptions.GetClone());

            var pi = GetType().GetProperties(NfSettings.DefaultFlags).Where(p => p.CanWrite).ToList();
            foreach (var p in pi)
            {
                var gVal = p.GetValue(this);
                p.SetValue(o, gVal);
            }

            foreach (var pzo in Pzos2Prob.Keys)
            {
                var d = Pzos2Prob[pzo];
                if (o.Pzos2Prob.ContainsKey(pzo))
                    o.Pzos2Prob[pzo] = d;
                else
                {
                    o.Pzos2Prob.Add(pzo, d);
                }
            }

            foreach(var me in GivenDirectly)
                o.AddGivenDirectly(me.Item1.Name, me.Item1.GetName(KindsOfNames.Group), me.Item2);

            foreach(var ca in ChildrenDobs)
                o.ChildrenDobs.Add(ca);

            o.Rate = Rate;
            o.SumTotal = SumTotal;
            return o;
        }

        /// <summary>
        /// Creates a wealth options object at random
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static AmericanDomusOpesOptions RandomOpesOptions(string firstName = null, string lastName = null, Gender? gender = null, DateTime? birthDate = null)
        {
            var name = new VocaBase();
            lastName = lastName ?? Etx.RandomWord();
            firstName = firstName ?? Etx.RandomWord();
            name.AddName(KindsOfNames.First, firstName);
            name.AddName(KindsOfNames.Surname, lastName);
            var factorOptions = AmericanFactorOptions.RandomFactorOptions(gender, birthDate);
            var location = CityArea.RandomAmericanCity();
            var isRenting = GetIsLeaseResidence(location?.Msa?.MsaType, factorOptions.GetAge());
            var hasCar = GetAtLeastOneVehicle(location?.Msa?.MsaType);
            var opt = new AmericanDomusOpesOptions(factorOptions)
            {
                Inception = DateTime.Today.Add(Constants.TropicalYear.Negate()),
                HomeLocation = location,
                IsRenting = isRenting,
                Personality = Pneuma.Personality.RandomPersonality(),
                NumberOfCreditCards = Etx.RandomInteger(0, 3),
                NumberOfVehicles = hasCar ? 1 : 0,
                PersonsName = name,
                DueFrequency = Constants.TropicalYear,
                IsVehiclePaidOff = false
            };
            return opt;
        }

        internal static bool GetAtLeastOneVehicle(UrbanCentric? msaType)
        {
            if (msaType == null)
                return false;
            var percentNoCarWholeNumber = (int) Math.Round(AmericanData.PERCENT_WITH_NO_CAR * 100);

            var livesInDenseUrbanArea = msaType == (UrbanCentric.City | UrbanCentric.Large);

            //just made this up
            if (livesInDenseUrbanArea)
                percentNoCarWholeNumber += 21;

            return Etx.RandomRollBelowOrAt(percentNoCarWholeNumber, Etx.Dice.OneHundred);

        }

        /// <summary>
        /// Weights the probability that a person will lease when they are young, living in a dense urban area or both.
        /// </summary>
        /// <param name="msaType"></param>
        /// <param name="age"></param>
        /// <returns></returns>
        internal static bool GetIsLeaseResidence(UrbanCentric? msaType, int age)
        {
            if (msaType == null)
                return false;
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
        /// <param name="stdDevInUsd">
        /// Optional, a randomizes the calculated value around a mean.
        /// </param>
        /// <returns></returns>
        public virtual Pecuniam GetRandomYearlyIncome(DateTime? dt = null, Pecuniam min = null,
            double stdDevInUsd = 2000)
        {
            if (min == null)
                min = Pecuniam.Zero;

            //get linear eq for earning 
            var eq = GetAvgEarningPerYear();
            if (eq == null)
                return Pecuniam.Zero;
            var dtt = dt.GetValueOrDefault(InceptionOrToday);
            var baseValue = Math.Round(eq.SolveForY(dtt.ToDouble()), 2);
            if (baseValue <= 0)
                return Pecuniam.Zero;

            var netWorth = new AmericanFactors(FactorOptions).NetWorthFactor;

            var factorValue = baseValue * netWorth;

            baseValue = Math.Round(factorValue, 2);

            stdDevInUsd = Math.Abs(stdDevInUsd);

            var randValue = Math.Round(
                Etx.RandomValueInNormalDist(Math.Round(baseValue, 0), stdDevInUsd), 2);

            //honor the promise to never let the value go below the 'min' if caller gave one.
            if (min > Pecuniam.Zero && randValue < 0)
                randValue = 0;
            return new Pecuniam((decimal)randValue) + min;
        }

        /// <summary>
        /// Get the linear eq of the city if its found otherwise defaults to the state, and failing that to the national
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// compiled data from BEA
        /// </remarks>
        protected internal virtual IEquation GetAvgEarningPerYear()
        {
            var ca = HomeLocation as UsCityStateZip;
            if (ca == null)
                return AmericanEquations.NatlAverageEarnings;
            ca.GetXmlData();
            return (ca.AverageEarnings ?? UsStateData.GetStateData(ca?.StateName)?.AverageEarnings) ??
                   AmericanEquations.NatlAverageEarnings;
        }

        #endregion
    }
}
