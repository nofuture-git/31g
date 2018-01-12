using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Gov.TheStates;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Gov
{
    [Serializable]
    public abstract class UsState
    {
        #region constants
        public const int AGE_OF_ADULT = 18;
        public const int MIN_AGE_FOR_DL = 16;
        #endregion

        #region fields
        protected readonly string _stateAbbrv;
        protected DriversLicense[] dlFormats;
        #endregion

        #region ctor
        protected UsState(string stateAbbrv)
        {
            _stateAbbrv = stateAbbrv;
        }
        #endregion

        #region properties

        public virtual int AgeOfMajority { get; set; } = AGE_OF_ADULT;

        /// <summary>
        /// The two letter postal code abbreviation
        /// </summary>
        public string StateAbbrv => _stateAbbrv;

        /// <summary>
        /// This is always resolved on the first entry found in the <see cref="dlFormats"/>.
        /// </summary>
        public virtual string RandomDriversLicense
        {
            get
            {
                if (dlFormats == null || dlFormats.Length <= 0)
                    return String.Empty;
                return dlFormats[0].GetRandom();
            }
        }
        /// <summary>
        /// Derived from [https://insurancelink.custhelp.com/app/answers/detail/a_id/1631/~/license-formats-for-individual-states]
        /// </summary>
        public virtual DriversLicense[] DriversLicenseFormats => dlFormats;

        #endregion

        #region methods

        /// <summary>
        /// Returns a date being between <see cref="min"/> years ago today back to <see cref="max"/> years ago today.
        /// </summary>
        /// <remarks>
        /// The age is limited to min,max of 18,67 - generate with family to get other age sets
        /// </remarks>
        public static DateTime GetWorkingAdultBirthDate(int min = 21, int max = 67)
        {
            if (min < AGE_OF_ADULT)
                min = AGE_OF_ADULT;
            if (max > 67)
                max = 67;
            return DateTime.Now.AddYears(-1 * Etx.MyRand.Next(min, max)).AddDays(Etx.IntNumber(1, 360));
        }


        /// <summary>
        /// Gets a date of death based on the <see cref="Equations.LifeExpectancy"/>
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        public static DateTime GetDeathDate(DateTime dob, string gender)
        {
            var normDist = AmericanEquations.LifeExpectancy(gender);
            var ageAtDeath = Etx.RandomValueInNormalDist(normDist.Mean, normDist.StdDev);
            var years = (int)Math.Floor(ageAtDeath);
            var days = (int)Math.Round((ageAtDeath - years) * Constants.DBL_TROPICAL_YEAR);

            var deathDate =
                dob.AddYears(years)
                    .AddDays(days)
                    .AddHours(Etx.IntNumber(0, 12))
                    .AddMinutes(Etx.IntNumber(0, 59))
                    .AddSeconds(Etx.IntNumber(0, 59));
            return deathDate;
        }

        public override string ToString()
        {
            return String.Join(" ", Etc.DistillToWholeWords(GetType().Name));
        }

        /// <summary>
        /// Asserts that the given value matches at least one of this 
        /// State's DL format.
        /// </summary>
        /// <param name="dlnumber"></param>
        /// <returns></returns>
        public virtual bool ValidDriversLicense(string dlnumber)
        {
            if (dlFormats == null || dlFormats.Length <= 0)
                return false;
            return dlFormats.Any(dlf => dlf.Validate(dlnumber));
        }

        public override bool Equals(object obj)
        {
            var st = obj as UsState;
            if (st == null)
                return false;
            return String.Equals(st.StateAbbrv, StateAbbrv, StringComparison.OrdinalIgnoreCase) ||
                   String.Equals(st.GetType().Name, GetType().Name, StringComparison.Ordinal);
        }
        public override int GetHashCode()
        {
            return GetType().Name.GetHashCode();
        }
        #endregion

        #region static factories
        internal static readonly List<UsState> _theStates = new List<UsState>();//singleton

        public static UsState[] TheStates
        {
            get
            {
                if (_theStates.Count <= 0)
                    InitAllUsStates();
                return _theStates.ToArray();
            }
        }

        /// <summary>
        /// Gets the <see cref="UsState"/> type based on the <see cref="stateAbbrv"/> (e.g. California is 'CA')
        /// </summary>
        /// <param name="stateAbbrv"></param>
        public static UsState GetStateByPostalCode(string stateAbbrv)
        {
            if (_theStates.Count <= 0)
                InitAllUsStates();
            return _theStates.FirstOrDefault(s => s.StateAbbrv == stateAbbrv);
        }

        /// <summary>
        /// Gets the <see cref="UsState"/> type based on the <see cref="fullStateName"/> (case-insensitive).
        /// </summary>
        /// <param name="fullStateName"></param>
        /// <returns></returns>
        public static UsState GetStateByName(string fullStateName)
        {
            if (_theStates.Count <= 0)
                InitAllUsStates();

            return
                _theStates.FirstOrDefault(
                    x => String.Equals(x.GetType().Name, fullStateName, StringComparison.OrdinalIgnoreCase)) ??
                _theStates.FirstOrDefault(
                    x => String.Equals(x.ToString(), fullStateName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Helper method to be used in the ctor of each state when assigning thier respective <see cref="DriversLicense"/>.
        /// </summary>
        /// <param name="lenth"></param>
        /// <param name="startAt"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static RcharNumeric[] Numerics(int lenth,int startAt = 0)
        {
            var someNumerics = new List<RcharNumeric>();
            for (var i = startAt; i < lenth + startAt; i++)
            {
                someNumerics.Add(new RcharNumeric(i));
            }
            return someNumerics.ToArray();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static void InitAllUsStates()
        {
            _theStates.Add(new Alabama());
            _theStates.Add(new Alaska());
            _theStates.Add(new Arizona());
            _theStates.Add(new Arkansas());
            _theStates.Add(new California());
            _theStates.Add(new Colorado());
            _theStates.Add(new Connecticut());
            _theStates.Add(new Delaware());
            _theStates.Add(new Florida());
            _theStates.Add(new Georgia());
            _theStates.Add(new Hawaii());
            _theStates.Add(new Idaho());
            _theStates.Add(new Illinois());
            _theStates.Add(new Indiana());
            _theStates.Add(new Iowa());
            _theStates.Add(new Kansas());
            _theStates.Add(new Kentucky());
            _theStates.Add(new Louisiana());
            _theStates.Add(new Maine());
            _theStates.Add(new Maryland());
            _theStates.Add(new Massachusetts());
            _theStates.Add(new Michigan());
            _theStates.Add(new Minnesota());
            _theStates.Add(new Mississippi());
            _theStates.Add(new Missouri());
            _theStates.Add(new Montana());
            _theStates.Add(new Nebraska());
            _theStates.Add(new Nevada());
            _theStates.Add(new NewHampshire());
            _theStates.Add(new NewJersey());
            _theStates.Add(new NewMexico());
            _theStates.Add(new NewYork());
            _theStates.Add(new NorthCarolina());
            _theStates.Add(new NorthDakota());
            _theStates.Add(new Ohio());
            _theStates.Add(new Oklahoma());
            _theStates.Add(new Oregon());
            _theStates.Add(new Pennsylvania());
            _theStates.Add(new RhodeIsland());
            _theStates.Add(new SouthCarolina());
            _theStates.Add(new SouthDakota());
            _theStates.Add(new Tennessee());
            _theStates.Add(new Texas());
            _theStates.Add(new Utah());
            _theStates.Add(new Vermont());
            _theStates.Add(new Virginia());
            _theStates.Add(new Washington());
            _theStates.Add(new DistrictOfColumbia());
            _theStates.Add(new WestVirginia());
            _theStates.Add(new Wisconsin());
            _theStates.Add(new Wyoming());
        }
        
        #endregion
    }
}
