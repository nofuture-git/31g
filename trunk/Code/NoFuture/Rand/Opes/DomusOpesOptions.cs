﻿using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Geo.US;
using NoFuture.Rand.Gov.US;
using NoFuture.Rand.Opes.US;
using NoFuture.Rand.Pneuma;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Enums;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Opes
{
    /// <inheritdoc cref="RandPortions" />
    /// <inheritdoc cref="ITempore" />
    /// <summary>
    /// An object to exercise control over the randomness of 
    /// the various types of Domus.Opes
    /// </summary>
    [Serializable]
    public class DomusOpesOptions : RandPortions, ITempore
    {
        private CityArea _cityArea;

        #region properties

        public DomusOpesOptions(AmericanFactorOptions factorOptions = null)
        {
            FactorOptions = factorOptions ?? AmericanFactorOptions.RandomFactorOptions();
        }

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
        public DomusOpesOptions GetClone()
        {
            var o = new DomusOpesOptions(FactorOptions.GetClone());

            var pi = GetType().GetProperties(NfSettings.DefaultFlags).Where(p => p.CanWrite).ToList();
            foreach (var p in pi)
            {
                var gVal = p.GetValue(this);
                p.SetValue(o, gVal);
            }

            foreach (var zo in PossibleZeroOuts)
                o.PossibleZeroOuts.Add(zo);

            foreach(var me in GivenDirectlyRefactor)
                o.AddGivenDirectly(me.Item1.Name, me.Item1.GetName(KindsOfNames.Group), me.Item2);

            foreach(var ca in ChildrenDobs)
                o.ChildrenDobs.Add(ca);

            o.DerivativeSlope = DerivativeSlope;
            o.SumTotal = SumTotal;
            o.DiceRoll = DiceRoll;
            return o;
        }

        /// <summary>
        /// Creates a wealth options object at random
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static DomusOpesOptions RandomOpesOptions(string firstName = null, string lastName = null)
        {
            var name = new VocaBase();
            lastName = lastName ?? Etx.RandomWord();
            firstName = firstName ?? Etx.RandomWord();
            name.AddName(KindsOfNames.First, firstName);
            name.AddName(KindsOfNames.Surname, lastName);

            var opt = new DomusOpesOptions
            {
                Inception = DateTime.Today.Add(Constants.TropicalYear.Negate()),
                HomeLocation = CityArea.RandomAmericanCity(),
                IsRenting = Etx.RandomCoinToss(),
                Personality = Rand.Pneuma.Personality.RandomPersonality(),
                NumberOfCreditCards = Etx.RandomInteger(0, 3),
                NumberOfVehicles = 1,
                PersonsName = name,
                DueFrequency = Constants.TropicalYear,
                IsVehiclePaidOff = false
            };
            return opt;
        }

        #endregion
    }
}
