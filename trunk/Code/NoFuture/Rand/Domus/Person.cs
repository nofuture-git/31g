using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Exceptions;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Domus.Pneuma;
using NoFuture.Rand.Gov;
using NoFuture.Shared;
using NoFuture.Util;

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
        protected internal readonly List<Tuple<KindsOfNames, string>> _otherNames =
            new List<Tuple<KindsOfNames, string>>();
        protected internal IPerson _mother;
        protected internal IPerson _father;
        protected internal readonly List<Uri> _netUris = new List<Uri>();
        protected internal readonly List<Child> _children = new List<Child>();
        protected internal Personality _personality = new Personality();
        protected internal BirthCert _birthCert;
        protected internal readonly List<HomeAddress> _addresses = new List<HomeAddress>();
        protected internal Gender _myGender;
        #endregion

        #region properties
        public virtual Gender MyGender
        {
            get { return _myGender; }
            set { _myGender = value; }
        }
        public virtual BirthCert BirthCert => _birthCert;
        public virtual DateTime? DeathDate { get; set; }
        public virtual string FirstName
        {
            get { return _otherNames.First(x => x.Item1 == KindsOfNames.Firstname).Item2; }
            set { UpsertName(KindsOfNames.Firstname, value); }
        }
        public virtual string LastName
        {
            get { return _otherNames.First(x => x.Item1 == KindsOfNames.Surname).Item2; }
            set { UpsertName(KindsOfNames.Surname, value); }
        }
        public virtual List<Uri> NetUri => _netUris;
        public Personality Personality => _personality;
        public virtual IEducation Education => GetEducationAt(null);
        public List<Tuple<KindsOfNames, string>> OtherNames => _otherNames;
        public HomeAddress Address => GetAddressAt(null);
        public Spouse Spouse => GetSpouseAt(null);
        public int Age => GetAgeAt(null);
        public MaritialStatus MaritialStatus => GetMaritalStatusAt(null);
        public List<Child> Children => _children;
        #endregion

        #region ctors
        protected Person(DateTime dob)
        {
            _birthCert = new BirthCert(this) { DateOfBirth = dob };
        }
        #endregion

        #region methods
        public abstract MaritialStatus GetMaritalStatusAt(DateTime? dt);

        public abstract Spouse GetSpouseAt(DateTime? dt);

        public virtual List<Child> GetChildrenAt(DateTime? dt)
        {
            var ddt = dt.GetValueOrDefault(DateTime.Now);

            return
                _children.Where(
                    x => x.Est.BirthCert != null && ddt.ComparedTo(x.Est.BirthCert.DateOfBirth) == ChronoCompare.After).ToList();
        }

        public virtual IPerson GetMother() { return _mother; }

        public virtual IPerson GetFather() { return _father; }

        public abstract IEducation GetEducationAt(DateTime? dt);

        /// <summary>
        /// Resolves the <see cref="HomeAddress"/> which was current 
        /// at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt">Null for the current time right now.</param>
        /// <returns></returns>
        public virtual HomeAddress GetAddressAt(DateTime? dt)
        {
            //TODO enhance to have previous address
            return dt == null
                ? _addresses.First()
                : (_addresses.FirstOrDefault(x => x.FromDate <= dt.Value) ?? _addresses.First());
        }

        /// <summary>
        /// Helper method to get the age at time <see cref="atTime"/>
        /// </summary>
        /// <param name="atTime">Null for the current time right now.</param>
        /// <returns></returns>
        public int GetAgeAt(DateTime? atTime)
        {
            var dt = DateTime.Now;
            if (atTime != null)
                dt = atTime.Value;
            ThrowOnBirthDateNull(this);

            if (DeathDate != null && DateTime.Compare(DeathDate.Value, dt) < 0)
                throw new ItsDeadJim("The person has deceased.");

            return CalcAge(BirthCert.DateOfBirth, dt);
        }

        /// <summary>
        /// Helper method to get the opposite gender.
        /// </summary>
        /// <returns><see cref="Gender.Unknown"/> returns the same.</returns>
        public Gender GetMyOppositeGender()
        {
            if(MyGender == Gender.Unknown)
                return Gender.Unknown;

            return MyGender == Gender.Female ? Gender.Male : Gender.Female;
        }

        /// <summary>
        /// Helper method to overwrite or add the given <see cref="k"/>
        /// to this instance.
        /// </summary>
        /// <param name="k"></param>
        /// <param name="name"></param>
        protected internal void UpsertName(KindsOfNames k, string name)
        {
            var cname = _otherNames.FirstOrDefault(x => x.Item1 == k);

            if (cname != null)
            {
                _otherNames.Remove(cname);
            }
            _otherNames.Add(new Tuple<KindsOfNames, string>(k, name));
        }

        /// <summary>
        /// Handles details of adding <see cref="addr"/> to 
        /// this instance's history of addresses.
        /// </summary>
        /// <param name="addr"></param>
        protected internal void UpsertAddress(HomeAddress addr)
        {
            if (addr == null)
                return;
            //TODO enhance to have previous address
            _addresses.Clear();
            _addresses.Add(addr);
        }
        #endregion

        #region static api
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
            var str = canadian.GetAddressAt(null)?.HomeStreetPo;
            canadian.UpsertAddress(new HomeAddress { HomeCityArea = cpp, HomeStreetPo = str });

            return canadian;
        }

        /// <summary>
        /// Reuseable method to get the diffence, in years, between
        /// <see cref="dob"/> and <see cref="atTime"/>
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="atTime"></param>
        /// <returns>
        /// The total number of days difference 
        /// divided by <see cref="Constants.DBL_TROPICAL_YEAR"/>,
        /// rounded off.
        /// </returns>
        public static int CalcAge(DateTime dob, DateTime atTime)
        {
            return Convert.ToInt32(Math.Round((atTime - dob).TotalDays / Constants.DBL_TROPICAL_YEAR));
        }

        /// <summary>
        /// Helper method to throw an ex when the Birth Cert is 
        /// unassigned.
        /// </summary>
        /// <param name="p"></param>
        internal static void ThrowOnBirthDateNull(IPerson p)
        {
            if (p == null)
                throw new ArgumentNullException(nameof(p));
            if (p.BirthCert == null)
                throw
                    new RahRowRagee(
                        $"The random person named {p.LastName}, {p.FirstName} " +
                        "does not have a Date Of Birth assigned.");
        }
        #endregion
    }
}
