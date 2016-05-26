using System;
using System.Collections.Generic;
using NoFuture.Exceptions;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Domus.Pneuma;
using NoFuture.Rand.Gov;
using NoFuture.Shared;

namespace NoFuture.Rand.Domus
{
    [Serializable]
    public abstract class Person : IPerson
    {
        #region constants
        internal const int PREG_DAYS = 280;
        internal const int MS_DAYS = 28;

        #endregion

        #region fields
        protected readonly List<Tuple<KindsOfPersonalNames, string>> _otherNames = new List<Tuple<KindsOfPersonalNames, string>>();
        protected readonly List<Uri> _netUris = new List<Uri>();
        protected readonly List<IPerson> _children = new List<IPerson>();
        protected Personality _personality = new Personality();
        protected BirthCert _birthCert;
        #endregion

        #region properties
        public virtual BirthCert BirthCert { get{return _birthCert;} }
        public virtual DateTime? DeathDate { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual Gender MyGender { get; set; }
        public virtual List<Uri> NetUri { get { return _netUris; } }
        public abstract MaritialStatus GetMaritalStatus(DateTime? dt);
        public abstract Spouse GetSpouse(DateTime? dt);
        public abstract IPerson GetFather();
        public abstract IPerson GetMother();
        public virtual List<IPerson> Children { get { return _children; } }
        public Personality Personality { get { return _personality; } }
        public virtual IEducation Education { get; set; }
        public List<Tuple<KindsOfPersonalNames, string>> OtherNames { get { return _otherNames; } }
        #endregion

        protected Person(DateTime dob)
        {
            _birthCert = new BirthCert(this) { DateOfBirth = dob };
        }

        #region methods
        public int GetAge(DateTime? atTime)
        {
            var dt = DateTime.Now;
            if (atTime != null)
                dt = atTime.Value;
            ThrowOnBirthDateNull(this);

            if (DeathDate != null && DateTime.Compare(DeathDate.Value, dt) < 0)
                throw new ItsDeadJim("Its Dead Jim.");

            return CalcAge(BirthCert.DateOfBirth, dt);
        }

        public Gender GetMyOppositeGender()
        {
            if(MyGender == Gender.Unknown)
                return Gender.Unknown;

            return MyGender == Gender.Female ? Gender.Male : Gender.Female;
        }

        /// <summary>
        /// Returns a new <see cref="NorthAmerican"/> with all values selected at random.
        /// </summary>
        /// <returns></returns>
        public static NorthAmerican American()
        {
            return new NorthAmerican(NAmerUtil.GetWorkingAdultBirthDate(), Etx.CoinToss ? Gender.Female : Gender.Male,
                true);
        }

        /// <summary>
        /// Returns a new <see cref="NorthAmerican"/> with all values select at random.
        /// The City, Providence and Postal Code are limited to the major Canadian cities.
        /// </summary>
        /// <returns></returns>
        public static NorthAmerican Canadian()
        {
            var canadian = American();
            var cpp = CityArea.Canadian();
            canadian.HomeCityArea = cpp;

            return canadian;
        }

        public static int CalcAge(DateTime dob, DateTime atTime)
        {
            return Convert.ToInt32(Math.Round((atTime - dob).TotalDays / Constants.DBL_TROPICAL_YEAR));
        }

        internal static void ThrowOnBirthDateNull(IPerson p)
        {
            if (p == null)
                throw new ArgumentNullException("p");
            if (p.BirthCert == null)
                throw
                    new RahRowRagee(
                        String.Format("The random person named {0}, {1} does not have a Date Of Birth assigned.",
                            p.LastName, p.FirstName));
        }

        #endregion
    }
}
