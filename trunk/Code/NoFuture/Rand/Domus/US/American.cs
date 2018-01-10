using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Gov;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Domus.US
{
    /// <summary>
    /// The extended <see cref="Person"/> type for 
    /// a North American.
    /// </summary>
    [Serializable]
    public class American : Person
    {
        #region fields
        internal readonly HashSet<Spouse> _spouses = new HashSet<Spouse>();
        private IEducation _edu;
        internal readonly List<Tuple<KindsOfLabels, NorthAmericanPhone>> _phoneNumbers = 
            new List<Tuple<KindsOfLabels, NorthAmericanPhone>>();
        private SocialSecurityNumber _ssn;
        private DriversLicense _dl;
        private DeathCert _deathCert;
        #endregion

        #region ctors

        /// <summary>
        /// Creates a new instance with names only.
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="myGender"></param>
        public American(DateTime dob, Gender myGender)
        {
            var dobAddr = CityArea.American();
            _birthCert = new AmericanBirthCert(FullName)
            {
                DateOfBirth = dob,
                City = dobAddr.City,
                State = dobAddr.State?.StateAbbrv
            };

            //almost always returns null
            _deathCert = AmericanUtil.GetRandomDeathCert(this);
            _myGender = myGender;

            var fname = _myGender != Gender.Unknown
                ? AmericanUtil.GetAmericanFirstName(_birthCert.DateOfBirth, _myGender)
                : "Pat";

            UpsertName(KindsOfNames.First, fname);
            var lname = AmericanUtil.GetAmericanLastName();
            UpsertName(KindsOfNames.Surname, lname);
            
            MiddleName = AmericanUtil.GetAmericanFirstName(_birthCert.DateOfBirth, _myGender);
            while (string.Equals(fname, MiddleName, StringComparison.OrdinalIgnoreCase))
            {
                MiddleName = AmericanUtil.GetAmericanFirstName(_birthCert.DateOfBirth, _myGender);
            }
            _ssn = new SocialSecurityNumber();
            if (Race <= 0)
                Race = Etx.DiscreteRange(AmericanRacePercents.NorthAmericanRaceAvgs);
        }

        /// <summary>
        /// Create new instance with both names and address, race and phone
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="myGender"></param>
        /// <param name="withWholeFamily">
        /// When set to true the ctor will assign parents, children and spouse at random.
        /// </param>
        public American(DateTime dob, Gender myGender, bool withWholeFamily):this(dob,myGender)
        {
            var isDead = _deathCert != null;
            if (isDead)
                return;
            var homeAddr = ResidentAddress.GetRandomAmericanAddr();
            _addresses.Add(homeAddr);
            var csz = homeAddr.HomeCityArea as UsCityStateZip;

            var abbrv = csz?.PostalState;

            if(Etx.TryAboveOrAt(6, Etx.Dice.Ten))
                _phoneNumbers.Add(new Tuple<KindsOfLabels, NorthAmericanPhone>(KindsOfLabels.Home, Phone.American(abbrv)));

            var isSmallChild = GetAgeAt(null) < 12;

            if (!isSmallChild)
                _phoneNumbers.Add(new Tuple<KindsOfLabels, NorthAmericanPhone>(KindsOfLabels.Mobile, Phone.American(abbrv)));
            
            Race = AmericanUtil.GetAmericanRace(csz?.ZipCode);
            if (Race <= 0)
                Race = Etx.DiscreteRange(AmericanRacePercents.NorthAmericanRaceAvgs);

            if (withWholeFamily)
                ResolveFamilyState();

            AddEmailAddress();
        }

        /// <summary>
        /// Ctor to directly assign the parents of the given instance
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="myGender"></param>
        /// <param name="mother"></param>
        /// <param name="father"></param>
        public American(DateTime dob, Gender myGender, IPerson mother, IPerson father): this(dob, myGender)
        {
            _mother = mother;
            _father = father;
            _birthCert.MotherName = string.Join(" ", _mother?.FirstName, _mother?.LastName);
            _birthCert.FatherName = string.Join(" ", _father?.FirstName, _father?.LastName);
            var nAmerMother = _mother as American;
            if (nAmerMother == null)
                return;
            var americanBirthCert = (AmericanBirthCert) _birthCert;
            var nAmerFather = _father as American;

            var birthPlace = nAmerMother.GetAddressAt(dob)?.HomeCityArea as UsCityStateZip ??
                             nAmerFather?.GetAddressAt(dob)?.HomeCityArea as UsCityStateZip ??
                             CityArea.American();

            americanBirthCert.City = birthPlace.City;
            americanBirthCert.State = birthPlace.State?.StateAbbrv;
            Race = nAmerMother.Race;
            AddEmailAddress();
        }

        #endregion

        #region properties

        /// <summary>
        /// Helper property to get the an American&apos;s full name.
        /// </summary>
        public string FullName => string.Join(" ", FirstName, MiddleName, LastName);

        /// <summary>
        /// Helper method to get a home phone.
        /// May be null:
        /// [http://www.pewresearch.org/fact-tank/2014/07/08/two-of-every-five-u-s-households-have-only-wireless-phones/]
        /// </summary>
        public NorthAmericanPhone HomePhone
        {
            get
            {
                var hph = _phoneNumbers.FirstOrDefault(x => x.Item1 == KindsOfLabels.Home);
                return hph?.Item2;
            }
            set => _phoneNumbers.Add(new Tuple<KindsOfLabels, NorthAmericanPhone>(KindsOfLabels.Home, value));
        }

        /// <summary>
        /// Helper method to get the mobile phone.
        /// </summary>
        public NorthAmericanPhone MobilePhone
        {
            get
            {
                var mobilePh = _phoneNumbers.FirstOrDefault(x => x.Item1 == KindsOfLabels.Mobile);
                return mobilePh?.Item2;
            }
            set => _phoneNumbers.Add(new Tuple<KindsOfLabels, NorthAmericanPhone>(KindsOfLabels.Mobile, value));
        }

        /// <summary>
        /// Get or sets the United States <see cref="SocialSecurityNumber"/> 
        /// of this instance.
        /// </summary>
        public virtual SocialSecurityNumber Ssn
        {
            get => _ssn;
            set => _ssn = value;
        }

        /// <summary>
        /// Gets or sets the <see cref="NorthAmericanRace"/> of this instance.
        /// </summary>
        public NorthAmericanRace Race { get; set; }

        /// <summary>
        /// Get the <see cref="Gov.DriversLicense"/> at the current time.
        /// </summary>
        public DriversLicense DriversLicense => GetDriversLicenseAt(null);

        /// <summary>
        /// Helper method to get the middle name which is mostly a North American thing.
        /// </summary>
        public string MiddleName
        {
            get => GetName(KindsOfNames.Middle);
            set => UpsertName(KindsOfNames.Middle, value);
        }

        public override DeathCert DeathCert
        {
            get => _deathCert;
            set => _deathCert = value;
        }

        #endregion

        #region public methods
        /// <summary>
        /// Gets this person's <see cref="MaritialStatus"/> at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt">Null for the current time</param>
        /// <returns></returns>
        public override MaritialStatus GetMaritalStatusAt(DateTime? dt)
        {
            var mdt = dt ?? DateTime.Now;

            if (!IsLegalAdult(mdt) ||!_spouses.Any() || _spouses.All(s => s.MarriedOn > mdt))
                return MaritialStatus.Single;

            var spAtDt = GetSpouseAt(mdt);
            if (spAtDt == null)
                return MaritialStatus.Divorced;

            if(spAtDt.Est?.DeathCert?.DateOfDeath != null && mdt >= spAtDt.Est?.DeathCert?.DateOfDeath)
                return  MaritialStatus.Widowed;

            return spAtDt.Ordinal > 1 ? MaritialStatus.Remarried : MaritialStatus.Married;
        }

        /// <summary>
        /// Gets the <see cref="Spouse"/> as they were at the given date-and-only-date of <see cref="dt"/>
        /// </summary>
        /// <param name="dt">Null for the current time</param>
        /// <returns></returns>
        public override Spouse GetSpouseAt(DateTime? dt)
        {
            if (!IsLegalAdult(dt))
            {
                return null;
            }

            dt = (dt ?? DateTime.Now).Date;

            var spouseData =
                _spouses.FirstOrDefault(
                    x =>
                        x.MarriedOn.Date <= dt &&
                        (x.Terminus == null || x.Terminus.Value.Date > dt));
            return spouseData;
        }

        /// <summary>
        /// Gets the <see cref="Spouse"/> at 
        /// <see cref="dt"/>, or 308 days befor or after <see cref="dt"/>
        /// </summary>
        /// <param name="dt">Null for the current time</param>
        /// <returns></returns>
        public override Spouse GetSpouseNear(DateTime? dt)
        {
            var ddt = (dt ?? DateTime.Now).Date;

            return GetSpouseAt(ddt) ??
                   GetSpouseAt(ddt.AddDays(-1*(PREG_DAYS + MS_DAYS))) ??
                   GetSpouseAt(ddt.AddDays(PREG_DAYS + MS_DAYS));
        }

        /// <summary>
        /// Gets the <see cref="IEducation"/> at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt">Null for the current time</param>
        /// <returns></returns>
        public override IEducation GetEducationAt(DateTime? dt)
        {
            if (_edu == null)
            {
                _edu = GetEducationByPerson();
            }

            if (dt == null)
                return _edu;

            if(_edu.EduFlag == OccidentalEdu.None)
                return _edu;

            var hsGradDt = _edu.HighSchool == null ? dt.Value.AddDays(-1) : _edu.HighSchool.Item2;

            if (hsGradDt < dt)
            {
                return new NorthAmericanEdu(new Tuple<IHighSchool, DateTime?>(_edu?.HighSchool?.Item1, null));
            }

            var univGradDt = _edu.College == null ? dt.Value.AddDays(-1) : _edu.College.Item2;

            if (univGradDt < dt)
            {
                return new NorthAmericanEdu(new Tuple<IUniversity, DateTime?>(_edu?.College?.Item1, null),
                    new Tuple<IHighSchool, DateTime?>(_edu?.HighSchool?.Item1, _edu?.HighSchool?.Item2));
            }

            return _edu;

        }

        protected internal NorthAmericanEdu GetEducationByPerson()
        {
            var dob = BirthCert?.DateOfBirth ?? UsState.GetWorkingAdultBirthDate();

            //determine where amer lived when they were 18
            var mother = BirthCert == null
                ? AmericanUtil.SolveForParent(dob,
                    AmericanEquations.FemaleAge2FirstMarriage,
                    Gender.Female) as American
                : GetMother() as American;
            var dtAtAge18 = dob.AddYears(UsState.AGE_OF_ADULT);
            var homeCityArea = mother?.GetAddressAt(dtAtAge18)?.HomeCityArea as UsCityStateZip ?? CityArea.American();
            return new NorthAmericanEdu(dob, homeCityArea);
        }

        /// <summary>
        /// Utility method to assign the <see cref="addr"/> to this intance, 
        /// the spouse and children who are under <see cref="UsState.AGE_OF_ADULT"/> 
        /// given the <see cref="dt"/>.
        /// </summary>
        /// <param name="dt">Null for current time.</param>
        /// <param name="addr"></param>
        public void AlignCohabitantsHomeDataAt(DateTime? dt, ResidentAddress addr)
        {
            if (addr == null)
                return;
            dt = dt ?? DateTime.Now;
            AddAddress(addr);

            Func<Child, bool> isUnderageChild =
                child => child?.Est is American && !((American) child.Est).IsLegalAdult(dt);

            var underAgeChildren =_children.Where(x => isUnderageChild(x)).ToList();

            var ms = GetMaritalStatusAt(dt);
            if ((ms == MaritialStatus.Married || ms == MaritialStatus.Remarried) && GetSpouseAt(dt).Est is American)
            {
                var spAtDt = (American) GetSpouseAt(dt).Est;
                AmericanUtil.SetNAmerCohabitants(spAtDt, this);
                underAgeChildren.AddRange(spAtDt.Children.Where(x => isUnderageChild(x)));
            }
            if (underAgeChildren.Count <= 0)
                return;

            //limit it down to just the distinct list of children 
            underAgeChildren = underAgeChildren.Distinct().ToList();

            foreach (var child in underAgeChildren)
            {
                var namerChild = child.Est as American;
                if (namerChild == null)
                    continue;

                var livesWith = MyGender == Gender.Male &&
                                Etx.TryAboveOrAt(AmericanData.PERCENT_DIVORCED_CHILDREN_LIVE_WITH_MOTHER+1,
                                    Etx.Dice.OneHundred)
                    ? namerChild.GetFather()
                    : namerChild.GetMother();

                var namerLivesWith = livesWith as American;
                if (namerLivesWith == null)
                    continue;

                if (namerLivesWith.Address == null)
                    namerLivesWith.AddAddress(
                        ResidentAddress.GetRandomAmericanAddr(Address.HomeCityArea.GetPostalCodePrefix()));

                AmericanUtil.SetNAmerCohabitants(namerChild, namerLivesWith);
            }
        }

        /// <summary>
        /// Resolves the <see cref="Gov.DriversLicense"/> which was 
        /// current at <see cref="dt"/>
        /// </summary>
        /// <param name="dt">Null for the current time</param>
        /// <returns></returns>
        public virtual DriversLicense GetDriversLicenseAt(DateTime? dt)
        {
            if (GetAgeAt(dt) < UsState.MIN_AGE_FOR_DL)
                return null;

            if (_dl != null)
                return _dl;

            var csz = GetAddressAt(dt)?.HomeCityArea;
            var amerCsz = csz as UsCityStateZip;
            var dlFormats = amerCsz?.State.DriversLicenseFormats;
            if (dlFormats == null || !dlFormats.Any())
                return null;
            _dl = dlFormats[0].IssueNewLicense(dt);
            _dl.Dob = BirthCert.DateOfBirth;
            _dl.FullLegalName = string.Join(" ", FirstName.ToUpper(), MiddleName.ToUpper(),
                LastName.ToUpper());
            _dl.Gender = MyGender.ToString();
            _dl.PrincipalResidence = Address.ToString();
            return _dl;
        }

        public override bool Equals(object obj)
        {
            var person = obj as IPerson;
            if (person == null)
                return false;

            //must be an american
            var amer = obj as American;
            if (amer == null)
                return false;
            var isSameBday = amer.BirthCert?.ToString() == BirthCert?.ToString();
            var isSameDl = amer.DriversLicense == DriversLicense;
            var isSameSsn = amer.Ssn == Ssn;

            return isSameBday && isSameDl && isSameSsn;
        }

        public override int GetHashCode()
        {
            var p = BirthCert?.ToString().GetHashCode() ?? 0;
            var q = DriversLicense?.ToString().GetHashCode() ?? 0;
            var m = Ssn?.ToString().GetHashCode() ?? 0;

            return p + q + m;
        }

        #endregion

        #region internal methods

        /// <summary>
        /// Invokes <see cref="ResolveParents"/>, <see cref="ResolveSpouse"/> and <see cref="ResolveChildren"/>.
        /// Only Parents are certian the other resolutions contrained by age and randomness.
        /// </summary>
        protected internal void ResolveFamilyState(bool skipParents = false)
        {
            ThrowOnBirthDateNull(this);
            var dt = DateTime.Now;

            if (!skipParents)
                ResolveParents();

            //resolve spouse to each other
            ResolveSpouse(AmericanUtil.GetMaritialStatus(_birthCert.DateOfBirth, MyGender));
            //to solve for childern when gender -eq Male
            ResolveChildren();
            AlignCohabitantsHomeDataAt(dt, GetAddressAt(null));
        }

        /// <summary>
        /// Assigns the parents as <see cref="IPerson"/> to this instance
        /// </summary>
        /// <remarks>
        /// The mother will always be assigned as such and present on <see cref="AmericanBirthCert"/>
        /// while the father <i>may be</i> likewise, <i>may just</i> be assigned 
        /// but not on the <see cref="AmericanBirthCert"/> or, lastly, <i>may be null</i>.
        /// </remarks>
        protected internal void ResolveParents()
        {
            ThrowOnBirthDateNull(this);

            //create current instance mother
            _mother = _mother ??
                      AmericanUtil.SolveForParent(_birthCert.DateOfBirth, AmericanEquations.FemaleAge2FirstMarriage,
                          Gender.Female);
            //line mothers last name with child
            UpsertName(KindsOfNames.Surname, _mother.LastName);

            var myMother = (American) _mother;
            myMother.Race = Race;
            BirthCert.MotherName = string.Join(" ", _mother.FirstName, _mother.LastName);

            //add self as one of mother's children
            myMother._children.Add(new Child(this));

            //TODO reslove this using data from census.gov
            var birthCity = myMother.GetAddressAt(_birthCert.DateOfBirth)?.HomeCityArea as UsCityStateZip;
            if (birthCity != null)
            {
                ((AmericanBirthCert) BirthCert).City = birthCity.City;
                ((AmericanBirthCert)BirthCert).State = birthCity.State.StateAbbrv;
            }

            //resolve mother's spouse(s)
            var motherMaritalStatus = AmericanUtil.GetMaritialStatus(_mother.BirthCert.DateOfBirth, Gender.Female);
            myMother.ResolveSpouse(motherMaritalStatus);
            
            //resolve for siblings
            myMother.ResolveChildren();

            //father is whoever was married to mother around time of birth
            var myFather = myMother.GetSpouseNear(_birthCert.DateOfBirth)?.Est as American;

            //mother not married at time of birth
            if (motherMaritalStatus == MaritialStatus.Single || myFather == null)
            {
                //small percent of father unknown
                if (Etx.TryAboveOrAt(98, Etx.Dice.OneHundred))
                    return;
                _father =
                    _father ??
                    AmericanUtil.SolveForParent(_birthCert.DateOfBirth, AmericanEquations.MaleAge2FirstMarriage,
                        Gender.Male);
                return;
            }
            //mother will receive last name of spouse
            myFather.Race = Race;
            _father = myFather;
            _birthCert.FatherName = string.Join(" ", _father.FirstName, _father.LastName);

            //last name assigned from birth father
            if (_father != null)
            {
                UpsertName(KindsOfNames.Surname, _father.LastName);
                ((American) _father)._children.Add(new Child(this));
            }
        }

        /// <summary>
        /// Will create a current and, possiably, past spouses for this instance.
        /// Calc will be based on DOB and <see cref="IPerson.MyGender"/>.
        /// </summary>
        /// <param name="myMaritialStatus"></param>
        /// <param name="atDate">Optional, defaults to now</param>
        protected internal void ResolveSpouse(MaritialStatus myMaritialStatus, DateTime? atDate = null)
        {
            if (myMaritialStatus == MaritialStatus.Single || myMaritialStatus == MaritialStatus.Unknown)
                return;

            var dt = atDate ?? DateTime.Now;

            ThrowOnBirthDateNull(this);

            var equationDt = AmericanEquations.ProtectAgainstDistantTimes(_birthCert.DateOfBirth);

            var avgAgeMarriage = MyGender == Gender.Female
                ? AmericanEquations.FemaleAge2FirstMarriage.SolveForY(equationDt.ToDouble())
                : AmericanEquations.MaleAge2FirstMarriage.SolveForY(equationDt.ToDouble());
            var currentAge = Etc.CalcAge(_birthCert.DateOfBirth, dt);

            //all other MaritialStatus imply at least one marriage in past
            var yearsMarried = currentAge - Convert.ToInt32(Math.Round(avgAgeMarriage));

            var marriedOn = Etx.Date(-1*yearsMarried, dt).Date.AddHours(12);

            var spouse = (American)AmericanUtil.SolveForSpouse(_birthCert.DateOfBirth, MyGender);

            //set death date if widowed
            if (myMaritialStatus == MaritialStatus.Widowed || spouse.DeathCert != null)
            {
                var d = Convert.ToInt32(Math.Round(GetAgeAt(null) * 0.15));
                myMaritialStatus = MaritialStatus.Widowed;
                spouse.DeathCert = spouse.DeathCert ??
                                   new AmericanDeathCert(AmericanDeathCert.MannerOfDeath.Natural, spouse.FullName)
                                   {
                                       DateOfDeath = Etx.Date(Etx.IntNumber(1, d)*-1, null)
                                   };
            }

            if (myMaritialStatus != MaritialStatus.Divorced && myMaritialStatus != MaritialStatus.Remarried &&
                myMaritialStatus != MaritialStatus.Separated)
            {
                //add internal date-range for resolution of children
                AddSpouse(spouse, marriedOn);
            }
            else
            {
                //take date of marriage and add avg length of marriage
                var separatedDate = Etx.Date(AmericanData.AVG_LENGTH_OF_MARRIAGE, marriedOn);

                //reset date-range with separated date
                AddSpouse(spouse, marriedOn, separatedDate);

                //leave when no second spouse applicable
                if (myMaritialStatus != MaritialStatus.Remarried)
                    return;

                var ageSpread = 6;
                if (MyGender == Gender.Male)
                    ageSpread = 10;

                //get a second spouse
                var secondSpouse = (American)AmericanUtil.SolveForSpouse(_birthCert.DateOfBirth, MyGender, ageSpread);

                //random second marriage date
                var remarriedOn = Etx.Date(Convert.ToInt32(Math.Round(AmericanData.YEARS_BEFORE_NEXT_MARRIAGE)),
                    separatedDate);

                //add second date-range for resolution of children
                AddSpouse(secondSpouse, remarriedOn);
            }
        }

        /// <summary>
        /// Instantiates and assigns zero to many <see cref="IPerson"/> as children 
        /// of this instance at random.
        /// </summary>
        protected internal void ResolveChildren()
        {
            //equations data is by women only.
            if (MyGender == Gender.Male)
            {
                foreach (var s in _spouses.Where(x => x.Est != null && x.Est.MyGender == Gender.Female))
                {
                    var nAmerSpouse = s.Est as American;
                    nAmerSpouse?.ResolveChildren();
                }
                return;
            }

            ThrowOnBirthDateNull(this);

            var currentNumChildren = 0;

            //two extremes
            var teenPregEquation = AmericanEquations.GetProbTeenPregnancyByRace(Race);
            var teenageAge = Etx.IntNumber(15, 19);
            var teenageYear = _birthCert.DateOfBirth.AddYears(teenageAge).Year;
            var propTeenagePreg = teenPregEquation.SolveForY(teenageYear);

            var propLifetimeChildless = AmericanUtil.SolveForProbabilityChildless(_birthCert.DateOfBirth,
                Education?.EduFlag ?? OccidentalEdu.None);

            //far high-end is no children for whole life
            if (Etx.MyRand.NextDouble() <= propLifetimeChildless)
                return;

            //other extreme is teenage preg
            if (Etx.MyRand.NextDouble() <= propTeenagePreg)
            {
                var teenPregChildDob = Etx.Date(teenageAge, _birthCert.DateOfBirth);
                AddChild(teenPregChildDob);
                currentNumChildren += 1;
            }
            
            //last is averages
            var numOfChildren = AmericanUtil.SolveForNumberOfChildren(_birthCert.DateOfBirth, null);

            if (numOfChildren <= 0)
                return;

            for (var i = currentNumChildren; i < numOfChildren; i++)
            {
                var childDob = AmericanUtil.GetChildBirthDate(_birthCert.DateOfBirth, i, null);
                if (childDob == null)
                    continue;

                AddChild(childDob.Value);
            }
        }

        /// <summary>
        /// Add a random <see cref="IPerson"/> to the Children collection.
        /// </summary>
        /// <param name="myChildDob">
        /// This will be adusted up by when the Birth Date would occur during the pregnancy 
        /// of a sibling unless it is the exact same date (twins).
        /// </param>
        protected internal void AddChild(DateTime myChildDob)
        {
            if (MyGender == Gender.Male)
                return;
            
            //check is alive
            if(DeathCert != null && myChildDob > DeathCert.DateOfDeath)
                return;

            var dt = DateTime.Now;
            if (IsTwin(myChildDob, out var dtOut) && DateTime.Compare(dtOut, DateTime.MinValue) != 0)
            {
                myChildDob = dtOut;
            }
            else
            {
                //need to move Dob based on timing of siblings and biology of mother
                while (!IsValidDobOfChild(myChildDob))
                {
                    myChildDob = myChildDob.AddDays(PREG_DAYS + MS_DAYS);
                }  
            }

            var myChildeAge = Etc.CalcAge(myChildDob, dt);
            var myChildGender = Etx.CoinToss ? Gender.Female : Gender.Male;
            var isChildAdult = myChildeAge >= GetMyHomeStatesAgeOfMajority();

            //look for spouse at and around Dob
            var spouseAtChildDob = GetSpouseNear(myChildDob);

            var childLastName = string.IsNullOrWhiteSpace(spouseAtChildDob?.Est?.LastName) ||
                                spouseAtChildDob.Est?.MyGender == Gender.Female
                                ? GetName(KindsOfNames.Maiden) ?? LastName
                                : spouseAtChildDob.Est?.LastName;

            var childRace = Race | (spouseAtChildDob?.Est as American)?.Race ?? Race;
            var nAmerChild = new American(myChildDob, myChildGender, this, spouseAtChildDob?.Est)
            {
                LastName = childLastName,
                Race = childRace
            };

            //check that child does not share the same first name as a sibling
            while (_children.Any(x => x.Est.FirstName == nAmerChild.FirstName))
            {
                nAmerChild.UpsertName(KindsOfNames.First,
                    AmericanUtil.GetAmericanFirstName(myChildDob, nAmerChild.MyGender));
            }

            //child has ref to father, father needs ref to child
            var nAmerFather = spouseAtChildDob?.Est as American;
            if (nAmerFather != null && nAmerFather.MyGender == Gender.Male 
                && nAmerFather.Children.All(x => !nAmerChild.Equals(x.Est)))
            {
                nAmerFather._children.Add(new Child(nAmerChild));
            }
            //resolve spouse, no grand-children
            if (isChildAdult)
            {
                nAmerChild.ResolveSpouse(AmericanUtil.GetMaritialStatus(myChildDob, myChildGender));
                nAmerChild.AlignCohabitantsHomeDataAt(DateTime.Now, nAmerChild.GetAddressAt(null));
            }
            
            _children.Add(new Child(nAmerChild));
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
            var siblingsBdays = _children.Where(x => x.Est.BirthCert != null).Select(x => x.Est.BirthCert.DateOfBirth).ToList();

            if (siblingsBdays.Any(x => DateTime.Compare(x.Date, childDob.Date) == 0))
            {
                childDob = siblingsBdays.Last(x => DateTime.Compare(x.Date, childDob.Date) == 0);
                minutesAfterChildDob = childDob.AddMinutes(Etx.IntNumber(2, 8)).AddSeconds(Etx.IntNumber(0, 59));
                return true;
            }
            minutesAfterChildDob = DateTime.MinValue;
            return false;
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
                _children.Where(x => x.Est.BirthCert != null)
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
        /// Handles detail of adding a assigning a spouse to this instance and 
        /// reciprocating such assignment to the <see cref="spouse"/>.  Additionaly
        /// handles the assignment of names (i.e. <see cref="KindsOfNames.Surname"/> 
        /// and <see cref="KindsOfNames.Maiden"/>).
        /// </summary>
        /// <param name="spouse"></param>
        /// <param name="marriedOn"></param>
        /// <param name="separatedOn"></param>
        protected internal void AddSpouse(IPerson spouse, DateTime marriedOn, DateTime? separatedOn = null)
        {
            //we need this or will will blow out the stack 
            if (_spouses.Any(x => DateTime.Compare(x.MarriedOn.Date, marriedOn.Date) == 0))
                return;

            //check that everyone is alive
            if (DeathCert != null && marriedOn > DeathCert.DateOfDeath)
                return;

            if (spouse.DeathCert != null && marriedOn > spouse.DeathCert.DateOfDeath)
                return;

            if (separatedOn == null)
            {
                //when this is the bride
                if (_myGender == Gender.Female && DateTime.Now >= marriedOn)
                {
                    if (LastName != null && !AnyOfKindOfName(KindsOfNames.Maiden))
                        UpsertName(KindsOfNames.Maiden, BirthCert.GetFatherLastName() ?? LastName);

                    LastName = spouse.LastName;
                }
            }
            else if (MyGender == Gender.Female && DateTime.Now >= separatedOn.Value)
            {
                //add ex-husband last name to list
                UpsertName(KindsOfNames.Former | KindsOfNames.Surname | KindsOfNames.Spouse, spouse.LastName);

                //set back to maiden name
                var maidenName = GetName(KindsOfNames.Maiden);
                if (!string.IsNullOrWhiteSpace(maidenName))
                    LastName = maidenName;
            }
            var nAmerSpouse = (American)spouse;
            nAmerSpouse.Race = nAmerSpouse.Race <= 0 ? Race : nAmerSpouse.Race;
            _spouses.Add(new Spouse(this, nAmerSpouse, marriedOn, separatedOn, _spouses.Count + 1));

            //recepricate to spouse
            nAmerSpouse.AddSpouse(this, marriedOn, separatedOn);
        }

        protected internal override bool IsLegalAdult(DateTime? dt)
        {
            return GetAgeAt(dt) >= GetMyHomeStatesAgeOfMajority();
        }

        /// <summary>
        /// Adds a full URI encoded email address to the <see cref="Person.NetUri"/> list
        /// when current age appropriate.
        /// </summary>
        protected internal void AddEmailAddress()
        {
            if (GetAgeAt(null) > 10)
                _netUris.Add(
                    new Uri("emailto:" +
                            Facit.RandomEmailUri(new[]
                            {
                                GetName(KindsOfNames.First), MiddleName,
                                GetName(KindsOfNames.Surname)
                            }, GetAgeAt(null) >= UsState.AGE_OF_ADULT)));
        }

        //min. age a person could be married at
        private int GetMyHomeStatesAgeOfMajority()
        {
            if (GetAddressAt(null) == null)
                return UsState.AGE_OF_ADULT;
            var myHomeState = UsState.GetStateByPostalCode(GetAddressAt(null)?.HomeCityArea?.AddressData?.StateAbbrv);
            return myHomeState?.AgeOfMajority ?? UsState.AGE_OF_ADULT;
        }
        #endregion
    }
}