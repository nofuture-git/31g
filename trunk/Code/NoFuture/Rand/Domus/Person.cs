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
using NoFuture.Util.Core;

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
            if (!IsValidDobOfChild(child.BirthCert.DateOfBirth))
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

        /// <summary>
        /// Tests if the <see cref="childDob"/> is a real possiablity 
        /// given the presence of siblings and thier date-of-birth.
        /// </summary>
        /// <param name="childDob"></param>
        /// <returns>
        /// True when the <see cref="childDob"/> is possiable given 
        /// the this instance's age at this time and the date-of-birth
        /// of siblings.
        /// </returns>
        /// <remarks>
        /// Is coded with implicit presumption of <see cref="IPerson.MyGender"/> 
        /// being <see cref="Gender.Female"/>, but does not test as such.
        /// </remarks>
        protected internal bool IsValidDobOfChild(DateTime childDob)
        {
            ThrowOnBirthDateNull(this);

            var maxDate = BirthCert.DateOfBirth.AddYears(55);
            var minDate = BirthCert.DateOfBirth.AddYears(13);

            if (childDob < minDate || childDob > maxDate)
            {
                throw new RahRowRagee(
                    $"The Child Date-of-Birth, {childDob}, does not fall " +
                    $"within a rational range given the mother's Date-of-Birth of {BirthCert.DateOfBirth}");
            }
            var clildDobTuple = new Tuple<DateTime, DateTime>(childDob.AddDays(-1 * PREG_DAYS), childDob);

            var bdayTuples =
                Children.Where(x => x.Est.BirthCert != null)
                    .Select(
                        x =>
                            new Tuple<DateTime, DateTime>(x.Est.BirthCert.DateOfBirth.AddDays(-1*(PREG_DAYS + MS_DAYS)),
                                x.Est.BirthCert.DateOfBirth.AddDays(MS_DAYS))).ToList();
            foreach (var s in bdayTuples)
            {
                var xDoC = s.Item1;
                var xDoB = s.Item2;

                var yDoC = clildDobTuple.Item1;
                var yDoB = clildDobTuple.Item2;

                //neither of the (y) values can appear between the x values
                if (yDoC.IsBetween(xDoC, xDoB) || yDoB.IsBetween(xDoC, xDoB))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Handle detail for random <see cref="childDob"/> being the same date as 
        /// an existing sibling.  
        /// </summary>
        /// <param name="childDob"></param>
        /// <param name="minutesAfterChildDob"></param>
        /// <returns>
        /// False when no sibling shares the same DOB (year, month, day)
        /// True when one or more siblings shares this DOB.
        /// The <see cref="minutesAfterChildDob"/> will be equal to the
        /// sibling's DOB plus 120 to 539 seconds.
        /// </returns>
        protected internal bool IsTwin(DateTime childDob, out DateTime minutesAfterChildDob)
        {
            var siblingsBdays = Children.Where(x => x.Est.BirthCert != null).Select(x => x.Est.BirthCert.DateOfBirth).ToList();

            if (siblingsBdays.Any(x => DateTime.Compare(x.Date, childDob.Date) == 0))
            {
                childDob = siblingsBdays.Last(x => DateTime.Compare(x.Date, childDob.Date) == 0);
                minutesAfterChildDob = childDob.AddMinutes(Etx.RandomInteger(2, 8)).AddSeconds(Etx.RandomInteger(0, 59));
                return true;
            }
            minutesAfterChildDob = DateTime.MinValue;
            return false;
        }

        #endregion

    }
}
