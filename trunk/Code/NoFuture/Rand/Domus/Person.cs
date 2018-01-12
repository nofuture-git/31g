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
using NoFuture.Rand.Gov.US;
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
        protected internal IPerson _mother;
        protected internal IPerson _father;
        protected internal readonly List<Uri> _netUris = new List<Uri>();
        protected internal readonly HashSet<Child> _children = new HashSet<Child>();
        private readonly Personality _personality = new Personality();
        protected internal BirthCert _birthCert;
        protected internal readonly List<PostalAddress> _addresses = new List<PostalAddress>();
        protected internal Gender _myGender;
        protected internal List<Tuple<KindsOfNames, Parent>> _parents = new List<Tuple<KindsOfNames, Parent>>();
        #endregion

        #region properties
        public virtual Gender MyGender
        {
            get => _myGender;
            set => _myGender = value;
        }
        public virtual BirthCert BirthCert => _birthCert;
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
        public virtual IEducation Education => GetEducationAt(null);
        public PostalAddress Address => GetAddressAt(null);
        public IRelation Spouse => GetSpouseAt(null);
        public int Age => GetAgeAt(null);
        public MaritialStatus MaritialStatus => GetMaritalStatusAt(null);
        public IEnumerable<Child> Children => _children;
        public IEnumerable<Tuple<KindsOfNames, Parent>> Parents
        {
            get
            {
                if(_parents.Count > 0)
                    return _parents;

                var f = GetFather();
                var m = GetMother();
                if (f == null && m == null)
                    return _parents;

                var lbl = KindsOfNames.Biological;
                if (!IsLegalAdult(null))
                    lbl = KindsOfNames.Legal | lbl;

                _parents.Add(new Tuple<KindsOfNames, Parent>(lbl | KindsOfNames.Father,
                    new Parent(f)));
                _parents.Add(new Tuple<KindsOfNames, Parent>(lbl | KindsOfNames.Mother,
                    new Parent(m)));

                Action<IPerson> addStepParent = person => _parents.Add(
                    new Tuple<KindsOfNames, Parent>(
                        KindsOfNames.Step |
                        (person.GetSpouseAt(null).Est.MyGender == Gender.Female
                            ? KindsOfNames.Mother
                            : KindsOfNames.Father),
                        new Parent(person.GetSpouseAt(null).Est)));

                Func<IPerson, IPerson, bool> isParentSpouseStepParent =
                    (p0, p1) => p0 != null && p0.MaritialStatus == MaritialStatus.Remarried && p0.GetSpouseAt(null).Est.Equals(p1);

                if(isParentSpouseStepParent(f,m))
                    addStepParent(f);
                if (isParentSpouseStepParent(m,f))
                    addStepParent(m);

                return _parents;
            }
        }

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

        /// <summary>
        /// Resolves the <see cref="PostalAddress"/> which was current 
        /// at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt">Null for the current time right now.</param>
        /// <returns></returns>
        public virtual PostalAddress GetAddressAt(DateTime? dt)
        {
            //TODO enhance to have previous address
            return dt == null
                ? _addresses.FirstOrDefault()
                : (_addresses.FirstOrDefault(x => x.Inception <= dt.Value) ?? _addresses.FirstOrDefault());
        }

        /// <summary>
        /// Helper method to get the age at time <see cref="atTime"/>
        /// </summary>
        /// <param name="atTime">Null for the current time right now.</param>
        /// <returns></returns>
        public int GetAgeAt(DateTime? atTime)
        {
            var dt = atTime ?? DateTime.Now;
            ThrowOnBirthDateNull(this);

            if (DeathCert != null && dt > DeathCert.DateOfDeath)
                dt = DeathCert.DateOfDeath;

            return Util.Core.Etc.CalcAge(BirthCert.DateOfBirth, dt);
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
        /// Handles details of adding <see cref="addr"/> to 
        /// this instance's history of addresses.
        /// </summary>
        /// <param name="addr"></param>
        public void AddAddress(PostalAddress addr)
        {
            if (addr == null)
                return;
            //TODO enhance to have previous address
            _addresses.Clear();
            _addresses.Add(addr);
        }

        protected internal abstract bool IsLegalAdult(DateTime? dt);

        /// <summary>
        /// Gets the mother, as another 
        /// instance of <see cref="IPerson"/>, of this instance.
        /// </summary>
        /// <returns></returns>
        protected internal virtual IPerson GetMother() { return _mother; }

        /// <summary>
        /// Gets the father, as another 
        /// instance of <see cref="IPerson"/>, of this instance.
        /// </summary>
        /// <returns></returns>
        protected internal virtual IPerson GetFather() { return _father; }
        #endregion

        #region static api
        /// <summary>
        /// Returns a new <see cref="US.American"/> with all values selected at random.
        /// </summary>
        /// <returns></returns>
        public static American American()
        {
            return new American(UsState.GetWorkingAdultBirthDate(), Etx.CoinToss ? Gender.Female : Gender.Male,
                true);
        }

        /// <summary>
        /// Returns a new <see cref="US.American"/> with all values select at random.
        /// The City, Providence and Postal Code are limited to the major Canadian cities.
        /// </summary>
        /// <returns></returns>
        public static American Canadian()
        {
            var canadian = American();
            var cpp = CityArea.Canadian();
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
