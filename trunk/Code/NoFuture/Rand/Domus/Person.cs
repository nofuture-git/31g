using System;
using System.Collections.Generic;
using System.Linq;
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
        protected internal readonly List<Tuple<KindsOfNames, string>> _otherNames =
            new List<Tuple<KindsOfNames, string>>();
        protected internal IPerson _mother;
        protected internal IPerson _father;
        protected internal readonly List<Uri> _netUris = new List<Uri>();
        protected internal readonly List<IPerson> _children = new List<IPerson>();
        protected internal Personality _personality = new Personality();
        protected internal BirthCert _birthCert;
        protected internal readonly List<HomeAddress> _addresses = new List<HomeAddress>();
        #endregion

        #region properties
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
        public virtual Gender MyGender { get; set; }
        public virtual List<Uri> NetUri => _netUris;
        public Personality Personality => _personality;
        public virtual IEducation Education { get; set; }
        public List<Tuple<KindsOfNames, string>> OtherNames => _otherNames;
        public HomeAddress Address => GetAddressAt(null);
        public Spouse Spouse => GetSpouseAt(null);
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

        public virtual List<IPerson> GetChildernAt(DateTime? dt)
        {
            return _children.Where(x => x.BirthCert.DateOfBirth >= dt.GetValueOrDefault(DateTime.Now)).ToList();
        }

        public virtual IPerson GetMother() { return _mother; }

        public virtual IPerson GetFather() { return _father; }

        public virtual HomeAddress GetAddressAt(DateTime? dt)
        {
            //TODO enhance to have previous address
            return dt == null ? _addresses.First() : _addresses.FirstOrDefault(x => x.FromDate <= dt.Value);
        }

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

        public Gender GetMyOppositeGender()
        {
            if(MyGender == Gender.Unknown)
                return Gender.Unknown;

            return MyGender == Gender.Female ? Gender.Male : Gender.Female;
        }

        protected internal void UpsertName(KindsOfNames k, string name)
        {
            var cname = _otherNames.FirstOrDefault(x => x.Item1 == k);

            if (cname != null)
            {
                _otherNames.Remove(cname);
            }
            _otherNames.Add(new Tuple<KindsOfNames, string>(k, name));
        }

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

        public static int CalcAge(DateTime dob, DateTime atTime)
        {
            return Convert.ToInt32(Math.Round((atTime - dob).TotalDays / Constants.DBL_TROPICAL_YEAR));
        }

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
