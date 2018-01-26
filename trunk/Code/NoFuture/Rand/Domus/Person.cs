using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Domus.Pneuma;
using NoFuture.Rand.Domus.US;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Gov;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Domus
{
    [Serializable]
    public abstract class Person : VocaBase, IPerson
    {
        #region constants
        internal const int PREG_DAYS = 280;
        internal const int MS_DAYS = 28;
        #endregion

        #region fields
        private readonly HashSet<Uri> _netUris = new HashSet<Uri>();
        private readonly HashSet<Child> _children = new HashSet<Child>();
        private readonly Personality _personality = Personality.RandomPersonality();
        private readonly List<PostalAddress> _addresses = new List<PostalAddress>();
        private Gender _myGender;
        private readonly HashSet<Parent> _parents = new HashSet<Parent>();
        #endregion

        #region properties
        public Gender MyGender
        {
            get => _myGender;
            set => _myGender = value;
        }
        public BirthCert BirthCert { get; set; }
        public virtual DeathCert DeathCert { get; set; }
        public virtual string FirstName
        {
            get => GetName(KindsOfNames.First);
            set => UpsertName(KindsOfNames.First, value);
        }
        public virtual string LastName
        {
            get => GetName(KindsOfNames.Surname);
            set => UpsertName(KindsOfNames.Surname, value);
        }
        public virtual IEnumerable<Uri> NetUri => _netUris;
        public Personality Personality => _personality;
        public abstract IEducation Education { get; set; }
        public PostalAddress Address => GetAddressAt(null);
        public IRelation Spouse => GetSpouseAt(null);
        public int Age => GetAgeAt(null);
        public MaritialStatus MaritialStatus => GetMaritalStatusAt(null);
        public IEnumerable<Child> Children => _children;
        public IEnumerable<Parent> Parents => _parents;
        public virtual string FullName => string.Join(" ", FirstName, LastName);
        #endregion

        #region methods
        public abstract MaritialStatus GetMaritalStatusAt(DateTime? dt);

        public abstract Spouse GetSpouseAt(DateTime? dt);

        public abstract Spouse GetSpouseNear(DateTime? dt);

        public virtual List<Child> GetChildrenAt(DateTime? dt)
        {
            var ddt = dt.GetValueOrDefault(DateTime.Now);

            return
                _children.Where(
                    x => x.Est.BirthCert != null && ddt > x.Est.BirthCert.DateOfBirth).ToList();
        }

        public abstract IEducation GetEducationAt(DateTime? dt);

        public virtual PostalAddress GetAddressAt(DateTime? dt)
        {
            _addresses.Sort(new TemporeComparer());
            return dt == null
                ? _addresses.LastOrDefault()
                : _addresses.LastOrDefault(a => a.IsInRange(dt.Value));
        }

        public int GetAgeAt(DateTime? atTime)
        {
            var dt = atTime ?? DateTime.Now;
            ThrowOnBirthDateNull(this);

            if (DeathCert != null && dt > DeathCert.DateOfDeath)
                dt = DeathCert.DateOfDeath;

            return Util.Core.Etc.CalcAge(BirthCert.DateOfBirth, dt);
        }

        public void AddAddress(PostalAddress addr)
        {
            if (addr == null)
                return;
            _addresses.Add(addr);
        }

        public void AddParent(IPerson parent, KindsOfNames parentalTitle)
        {
            if (parent?.BirthCert == null || parent.BirthCert.DateOfBirth == DateTime.MinValue || parent.Age <= 0)
                return;
            var diffInAge = parent.Age - Age;
            if (diffInAge < AmericanUtil.MIN_AGE_TO_BE_PARENT)
                return;
            _parents.Add(new Parent(parent, parentalTitle));
        }

        public void AddChild(IPerson child)
        {
            if (child?.BirthCert == null || child.BirthCert.DateOfBirth == DateTime.MinValue || child.Age <= 0)
                return;
            var diffInAge = Age - child.Age;
            if (diffInAge < AmericanUtil.MIN_AGE_TO_BE_PARENT)
                return;
            _children.Add(new Child(child));
        }

        public void AddUri(Uri uri)
        {
            if(uri != null)
                _netUris.Add(uri);
        }

        protected internal abstract bool IsLegalAdult(DateTime? dt);

        /// <summary>
        /// Gets the Biological Mother
        /// </summary>
        /// <returns></returns>
        protected internal IPerson GetMother()
        {
            return GetParent(KindsOfNames.Mother | KindsOfNames.Biological);
        }

        /// <summary>
        /// Gets the father, as another 
        /// instance of <see cref="IPerson"/>, of this instance.
        /// </summary>
        /// <returns></returns>
        protected internal IPerson GetFather()
        {
            return GetParent(KindsOfNames.Father | KindsOfNames.Biological);
        }

        protected internal IPerson GetParent(KindsOfNames parentalTitle)
        {
            var parent = Parents.FirstOrDefault(p => p.AnyOfKind(parentalTitle));

            if (parent?.Est == null)
            {
                parent = Parents.FirstOrDefault(p =>
                    p.AnyOfKindContaining(parentalTitle));
            }

            return parent?.Est;
        }

        protected internal List<PostalAddress> GetAddresses()
        {
            return _addresses;
        }

        #endregion

        #region static api
        /// <summary>
        /// Returns a new <see cref="US.American"/> with all values selected at random.
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static American RandomAmerican()
        {
            return new American(Etx.RandomAdultBirthDate(), Etx.RandomCoinToss() ? Gender.Female : Gender.Male,
                true);
        }

        /// <summary>
        /// Returns a new <see cref="US.American"/> with all values select at random.
        /// The City, Providence and Postal Code are limited to the major Canadian cities.
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static American RandomCanadian()
        {
            var canadian = RandomAmerican();
            var cpp = CityArea.RandomCanadianCity();
            var str = canadian.GetAddressAt(null)?.HomeStreetPo;
            canadian.AddAddress(new PostalAddress { HomeCityArea = cpp, HomeStreetPo = str });

            return canadian;
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
