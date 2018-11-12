using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Domus.US;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Pneuma;
using NoFuture.Rand.Tele;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Domus
{
    /// <inheritdoc cref="IPerson" />
    /// <inheritdoc cref="VocaBase" />
    [Serializable]
    public abstract class Person : VocaBase, IPerson
    {
        #region constants
        internal const int PREG_DAYS = 280;
        internal const int MS_DAYS = 28;
        #endregion

        #region fields
        private readonly HashSet<NetUri> _netUris = new HashSet<NetUri>();
        private readonly HashSet<Child> _children = new HashSet<Child>();
        private BirthCert _myBirthCert;
        private DeathCert _myDeathCert;
        private readonly List<PostalAddress> _addresses = new List<PostalAddress>();
        private Gender _myGender;
        private readonly HashSet<Parent> _parents = new HashSet<Parent>();
        private readonly HashSet<Spouse> _spouses = new HashSet<Spouse>();
        
        #endregion

        #region properties
        public Gender Gender
        {
            get => _myGender;
            set => _myGender = value;
        }

        public BirthCert BirthCert
        {
            get => _myBirthCert;
            set
            {
                _myBirthCert = value;
                ThrowOnDeathBeforeBirth();
                SyncBirthCert2Person();
            }
        }

        public virtual DeathCert DeathCert
        {
            get => _myDeathCert;
            set
            {
                _myDeathCert = value;
                ThrowOnDeathBeforeBirth();
            }
        }
        public virtual string FirstName
        {
            get => GetName(KindsOfNames.First);
            set => AddName(KindsOfNames.First, value);
        }
        public virtual string LastName
        {
            get => GetName(KindsOfNames.Surname);
            set => AddName(KindsOfNames.Surname, value);
        }
        public virtual IEnumerable<NetUri> NetUris => _netUris;
        public Personality Personality { get; set; }
        public abstract IEducation Education { get; set; }
        public PostalAddress Address => GetAddressAt(null);
        public IRelation Spouse => GetSpouseAt(null);
        public int Age => GetAgeAt(null);
        public MaritialStatus MaritialStatus => GetMaritalStatusAt(null);
        public IEnumerable<Child> Children => _children;
        public IEnumerable<Parent> Parents => _parents;
        public virtual string FullName => string.Join(" ", FirstName, LastName);
        public abstract IEnumerable<Phone> PhoneNumbers { get; }
        #endregion

        #region methods
        public abstract MaritialStatus GetMaritalStatusAt(DateTime? dt);

        public abstract Spouse GetSpouseAt(DateTime? dt);

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

            return Etc.CalcAge(BirthCert.DateOfBirth, dt);
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
            if (!(parent is Person pparent))
                return;

            //I may have already been added to parent's children so my DoB would always looks invalid
            var isThisMe = pparent.Children.Any(c =>
                c.Est.BirthCert.DateOfBirth == BirthCert.DateOfBirth && c.Est.Equals(this));

            //insure that my DOB is in a rational range of the parents
            if (!isThisMe && !pparent.IsValidDobOfChild(BirthCert.DateOfBirth, parentalTitle))
                return;

            _parents.Add(new Parent(parent, parentalTitle));
        }

        public void AddChild(IPerson child, KindsOfNames? myParentalTitle = null)
        {
            if (child?.BirthCert == null || child.BirthCert.DateOfBirth == DateTime.MinValue || child.Age <= 0)
                return;
            var title = myParentalTitle ?? (Gender == Gender.Female ? KindsOfNames.Mother : KindsOfNames.Father) |
                        KindsOfNames.Biological;

            if (!IsValidDobOfChild(child.BirthCert.DateOfBirth, title))
                return;

            _children.Add(new Child(child));
        }

        public void AddSpouse(IPerson spouse, DateTime marriedOn, DateTime? separatedOn = null)
        {
            //we need this or will will blow out the stack 
            if (GetSpouses().Any(x => DateTime.Compare(x.MarriedOn.Date, marriedOn.Date) == 0))
                return;
            var spouses = GetSpouses();
            //check that everyone is alive
            if (DeathCert != null && marriedOn > DeathCert.DateOfDeath)
                return;

            if (spouse.DeathCert != null && marriedOn > spouse.DeathCert.DateOfDeath)
                return;

            if (separatedOn == null)
            {
                //when this is the bride
                if (Gender == Gender.Female && DateTime.Now >= marriedOn)
                {
                    if (LastName != null && !AnyOfKind(KindsOfNames.Maiden))
                        AddName(KindsOfNames.Maiden, BirthCert.GetFatherSurname() ?? LastName);

                    LastName = spouse.LastName;
                }
            }
            else if (Gender == Gender.Female && DateTime.Now >= separatedOn.Value)
            {
                //add ex-husband last name to list
                AddName(KindsOfNames.Former | KindsOfNames.Surname | KindsOfNames.Spouse, spouse.LastName);

                //set back to maiden name
                var maidenName = GetName(KindsOfNames.Maiden);
                if (!String.IsNullOrWhiteSpace(maidenName))
                    LastName = maidenName;
            }
            spouses.Add(new Spouse(this, spouse, marriedOn, separatedOn, spouses.Count + 1));

            //recepricate to spouse
            spouse.AddSpouse(this, marriedOn, separatedOn);
        }

        public abstract void AddPhone(Phone phone);

        public abstract void AddPhone(string phoneNumber, KindsOfLabels? descriptor = null);

        public void AddUri(NetUri uri)
        {
            //don't allow callers to add telephone Uri's since there is another storage place for those
            if(uri != null && uri.ToUri()?.Scheme != Phone.URI_SCHEMA_TELEPHONE)
                _netUris.Add(uri);
        }

        public virtual void AddUri(string uri, KindsOfLabels? descriptor = null)
        {
            if (string.IsNullOrWhiteSpace(uri))
                return;

            if (!uri.StartsWith(Uri.UriSchemeMailto) &&
                System.Text.RegularExpressions.Regex.IsMatch(uri, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
                uri = $"{Uri.UriSchemeMailto}:{uri}";
            if (!Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out var oUri))
                return;

            AddUri(new NetUri() {Descriptor = descriptor, Value = oUri.ToString()});
        }

        protected internal HashSet<Spouse> GetSpouses()
        {
            return _spouses;
        }

        public virtual Spouse GetSpouseNear(DateTime? dt, int days = PREG_DAYS + MS_DAYS)
        {
            var ddt = (dt ?? DateTime.Now).Date;
            days = Math.Abs(days);
            return GetSpouseAt(ddt) ??
                   GetSpouseAt(ddt.AddDays(-1 * days)) ??
                   GetSpouseAt(ddt.AddDays(days));
        }

        protected internal abstract bool IsLegalAdult(DateTime? dt);

        /// <summary>
        /// Gets the Biological Mother
        /// </summary>
        /// <returns></returns>
        protected internal IPerson GetBiologicalMother()
        {
            return GetParent(KindsOfNames.Mother | KindsOfNames.Biological);
        }

        /// <summary>
        /// Gets the father, as another 
        /// instance of <see cref="IPerson"/>, of this instance.
        /// </summary>
        /// <returns></returns>
        protected internal IPerson GetBiologicalFather()
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
        /// <param name="myParentalTitle"></param>
        /// <returns>
        /// True when the <see cref="childDob"/> is possiable given 
        /// the this instance's age at this time and the date-of-birth
        /// of siblings.
        /// </returns>
        /// <remarks>
        /// Is coded with implicit presumption of <see cref="IPerson.Gender"/> 
        /// being <see cref="Gov.Gender.Female"/>, but does not test as such.
        /// </remarks>
        protected internal bool IsValidDobOfChild(DateTime childDob, KindsOfNames? myParentalTitle = null)
        {
            ThrowOnBirthDateNull(this);
            var title = myParentalTitle ?? (Gender == Gender.Female ? KindsOfNames.Mother : KindsOfNames.Father) |
                        KindsOfNames.Biological;
            var p2C = IsValidDobOfChild2Parent(childDob);
            if (!p2C)
                return false;

            return !ToDiscreteKindsOfNames(title).Contains(KindsOfNames.Biological) ||
                   IsValidDobChild2Siblings(childDob);
        }

        /// <summary>
        /// Checks that a child&apos;s birth date is in a rational range considering the 
        /// birth dates of their siblings
        /// </summary>
        /// <param name="childDob"></param>
        /// <returns></returns>
        protected internal bool IsValidDobChild2Siblings(DateTime childDob)
        {
            //there are not siblings
            if (!Children.Any())
                return true;

            //check the childs dob against their siblings
            var clildDobTuple = new Tuple<DateTime, DateTime>(childDob.AddDays(-1 * PREG_DAYS), childDob);

            var bdayTuples =
                Children.Where(x => x.Est.BirthCert != null)
                    .Select(
                        x =>
                            new Tuple<DateTime, DateTime>(
                                x.Est.BirthCert.DateOfBirth.AddDays(-1 * (PREG_DAYS + MS_DAYS)),
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
        /// Validates that the child&apos;s birth date is rational compared to the parent&apos;s
        /// </summary>
        /// <param name="childDob"></param>
        protected internal bool IsValidDobOfChild2Parent(DateTime childDob)
        {
            //check the childs dob against the parent
            var maxDate = Gender == Gender.Female
                ? BirthCert.DateOfBirth.AddYears(55)
                : BirthCert.DateOfBirth.AddYears(80);
            var minDate = BirthCert.DateOfBirth.AddYears(AmericanUtil.MIN_AGE_TO_BE_PARENT);

            if (childDob < minDate || childDob > maxDate)
            {
                return false;
            }

            return DeathCert == null || DeathCert.DateOfDeath >= childDob;
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

        /// <summary>
        /// Helper method to keep the same data, stored in two independent places, in sync with each other.
        /// </summary>
        protected internal void SyncBirthCert2Person()
        {
            if (_myBirthCert == null)
                return;

            if (string.IsNullOrWhiteSpace(_myBirthCert.PersonFullName))
                _myBirthCert.PersonFullName = FullName;

            var mother = GetBiologicalMother();
            _myBirthCert.MotherName = mother?.FullName ?? _myBirthCert.MotherName;

            var father = GetBiologicalFather();
            _myBirthCert.FatherName = father?.FullName ?? _myBirthCert.FatherName;
        }

        protected internal void ThrowOnDeathBeforeBirth()
        {
            //check that death date is not before birth date
            if (_myDeathCert != null && _myBirthCert != null && _myBirthCert.DateOfBirth > _myDeathCert.DateOfDeath)
                throw new InvalidOperationException($"This person {FullName} is assigned a date " +
                                                    $"of death as {_myDeathCert.DateOfDeath} and a birth " +
                                                    $"date of {_myBirthCert.DateOfBirth} which is impossiable.");
        }

        public override IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => TransformText(x, txtCase);
            var itemData = new Dictionary<string, object>();

            if (BirthCert != null)
                AddOrReplace(itemData, BirthCert.ToData(txtCase));

            if (DeathCert != null)
                AddOrReplace(itemData, DeathCert.ToData(txtCase));

            if(!string.IsNullOrWhiteSpace(FirstName))
                itemData.Add(textFormat(nameof(FirstName)), FirstName);

            if(!string.IsNullOrWhiteSpace(LastName))
                itemData.Add(textFormat(nameof(LastName)), LastName);

            itemData.Add(textFormat(nameof(Gender)), Gender);
            itemData.Add(textFormat(nameof(MaritialStatus)), MaritialStatus.ToString());
            itemData.Add(textFormat(nameof(Age)), Age);

            foreach (var ph in PhoneNumbers)
                AddOrReplace(itemData, ph.ToData(txtCase));

            foreach (var nuri in NetUris)
                AddOrReplace(itemData, nuri.ToData(txtCase));

            var addr = Address;
            if(addr != null)
                AddOrReplace(itemData, addr.ToData(txtCase));

            var personality = Personality;
            if(personality != null)
                AddOrReplace(itemData, personality.ToData(txtCase));

            var edu = Education;
            if(edu != null)
                AddOrReplace(itemData, edu.ToData(txtCase));

            return itemData;
        }

        #endregion

    }
}
