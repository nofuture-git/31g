using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NoFuture.Exceptions;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Domus.Pneuma;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Gov;
using NoFuture.Shared;
using NoFuture.Util;

namespace NoFuture.Rand.Domus
{
    /// <summary>
    /// The extended <see cref="Person"/> type for 
    /// a North American.
    /// </summary>
    [Serializable]
    public class NorthAmerican : Person
    {
        #region fields
        internal readonly List<Spouse> _spouses = new List<Spouse>();
        private IEducation _edu;
        internal readonly List<Tuple<KindsOfLabels, NorthAmericanPhone>> _phoneNumbers = 
            new List<Tuple<KindsOfLabels, NorthAmericanPhone>>();
        private SocialSecurityNumber _ssn;
        #endregion

        #region ctors

        /// <summary>
        /// Typical ctor to create the random instance.
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="myGender"></param>
        /// <param name="withWholeFamily">
        /// When set to true the ctor will assign parents, children and spouse at random.
        /// </param>
        public NorthAmerican(DateTime dob, Gender myGender, bool withWholeFamily = false):base(dob)
        {
            _birthCert = new AmericanBirthCert(this) { DateOfBirth = dob };
            _myGender = myGender;

            var fname = _myGender != Gender.Unknown ? NAmerUtil.GetAmericanFirstName(_birthCert.DateOfBirth, _myGender) : "Pat";
            _otherNames.Add(new Tuple<KindsOfNames, string>(KindsOfNames.First, fname));
            var lname = NAmerUtil.GetAmericanLastName();
            _otherNames.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Surname, lname));

            MiddleName = NAmerUtil.GetAmericanFirstName(_birthCert.DateOfBirth, _myGender);

            var csz = CityArea.American();
            var homeAddr = StreetPo.American();
            _addresses.Add(new ResidentAddress {HomeStreetPo = homeAddr, HomeCityArea = csz});

            var abbrv = csz?.PostalState;

            if(Etx.TryAboveOrAt(6, Etx.Dice.Ten))
                _phoneNumbers.Add(new Tuple<KindsOfLabels, NorthAmericanPhone>(KindsOfLabels.Home, Phone.American(abbrv)));

            if(GetAgeAt(null) >= 12)
                _phoneNumbers.Add(new Tuple<KindsOfLabels, NorthAmericanPhone>(KindsOfLabels.Mobile, Phone.American(abbrv)));

            _ssn = new SocialSecurityNumber();
            Race = NAmerUtil.GetAmericanRace(csz?.ZipCode);
            if (withWholeFamily)
                ResolveFamilyState();

            _personality = new Personality();
        }

        /// <summary>
        /// Ctor to directly assign the parents of the given instance
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="myGender"></param>
        /// <param name="mother"></param>
        /// <param name="father"></param>
        public NorthAmerican(DateTime dob, Gender myGender, IPerson mother, IPerson father): this(dob, myGender)
        {
            _mother = mother;
            _father = father;
            _birthCert.Mother = _mother;
            _birthCert.Father = _father;
            var nAmerMother = _mother as NorthAmerican;
            if (nAmerMother == null)
                return;
            ((AmericanBirthCert) _birthCert).BirthPlace = nAmerMother.GetAddressAt(dob)?.HomeCityArea as UsCityStateZip;
            Race = nAmerMother.Race;
        }

        #endregion

        #region properties
        /// <summary>
        /// Helper method to get the middle name which is mostly a North American thing.
        /// </summary>
        public string MiddleName
        {
            get { return _otherNames.First(x => x.Item1 == KindsOfNames.Middle).Item2; }
            set { UpsertName(KindsOfNames.Middle, value); }

        }

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
        }

        /// <summary>
        /// Get or sets the United States <see cref="SocialSecurityNumber"/> 
        /// of this instance.
        /// </summary>
        public virtual SocialSecurityNumber Ssn { get {return _ssn;} set { _ssn = value; } }

        /// <summary>
        /// Gets or sets the <see cref="NorthAmericanRace"/> of this instance.
        /// </summary>
        public NorthAmericanRace Race { get; set; }

        /// <summary>
        /// Get the <see cref="Gov.DriversLicense"/> at the current time.
        /// </summary>
        public DriversLicense DriversLicense => GetDriversLicenseAt(null);

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

            var iAmUnderage = !IsLegalAdult(mdt);

            if (iAmUnderage || !_spouses.Any())
                return MaritialStatus.Single;

            //spread all status into single dictionary
            var myTimeline = new Dictionary<int, MaritialStatus> { { 0, MaritialStatus.Single } };
            foreach (var spouseData in _spouses)
            {
                var days = (int)Math.Abs((spouseData.MarriedOn - _birthCert.DateOfBirth).TotalDays);
                while (myTimeline.ContainsKey(days))
                    days += 1;

                if (myTimeline.Values.Any(x => x == MaritialStatus.Married))
                {
                    myTimeline.Add(days, MaritialStatus.Remarried);
                }
                else
                {
                    myTimeline.Add(days, MaritialStatus.Married);
                }

                if (spouseData.SeparatedOn == null)
                    continue;
                days = (int)Math.Abs((spouseData.SeparatedOn.Value - _birthCert.DateOfBirth).TotalDays);
                while (myTimeline.ContainsKey(days))
                    days += 1;

                myTimeline.Add(days, MaritialStatus.Divorced);

                if (spouseData.Est == null || spouseData.Est.DeathDate == null)
                    continue;

                days = (int)Math.Abs((spouseData.Est.DeathDate.Value - _birthCert.DateOfBirth).TotalDays);
                while (myTimeline.ContainsKey(days))
                    days += 1;

                myTimeline.Add(days, MaritialStatus.Widowed);
            }

            var mdtDays = (int)Math.Abs((mdt - _birthCert.DateOfBirth).TotalDays);

            return myTimeline.Any(x => x.Key >= mdtDays)
                ? myTimeline.First(x => x.Key >= mdtDays).Value
                : myTimeline.Last().Value;
        }

        /// <summary>
        /// Gets the <see cref="Spouse"/> as they were at the given time of <see cref="dt"/>
        /// </summary>
        /// <param name="dt">Null for the current time</param>
        /// <returns></returns>
        public override Spouse GetSpouseAt(DateTime? dt)
        {
            if (!IsLegalAdult(dt))
            {
                return null;
            }

            var spouseData = _spouses.FirstOrDefault(x => x.MarriedOn <= (dt ?? DateTime.Now) && x.SeparatedOn == null);
            return spouseData;
        }

        /// <summary>
        /// Gets the <see cref="IEducation"/> at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt">Null for the current time</param>
        /// <returns></returns>
        public override IEducation GetEducationAt(DateTime? dt)
        {
            if(_edu == null)
                _edu = new NorthAmericanEdu(this);

            if (dt == null)
                return _edu;

            if(_edu.EduLevel == OccidentalEdu.None)
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
            UpsertAddress(addr);
            var ms = GetMaritalStatusAt(dt);
            if ((ms == MaritialStatus.Married || ms == MaritialStatus.Remarried) && GetSpouseAt(dt) != null)
            {
                NAmerUtil.SetNAmerCohabitants((NorthAmerican)GetSpouseAt(dt).Est, this);
            }
            var underAgeChildren =
                _children.Where(
                    x => x.Est is NorthAmerican && ((NorthAmerican) x.Est).GetAgeAt(dt) < UsState.AGE_OF_ADULT)
                    .ToList();
            if (underAgeChildren.Count <= 0)
                return;
            foreach (var child in underAgeChildren)
                NAmerUtil.SetNAmerCohabitants((NorthAmerican)child.Est, this);
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

            var csz = GetAddressAt(dt)?.HomeCityArea;
            var amerCsz = csz as UsCityStateZip;
            var dl = amerCsz?.State.DriversLicenseFormats[0];
            return dl;
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
            
            if(!skipParents)
                ResolveParents();

            //resolve spouse to each other
            ResolveSpouse(NAmerUtil.GetMaritialStatus(_birthCert.DateOfBirth, MyGender));
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
                      NAmerUtil.SolveForParent(_birthCert.DateOfBirth, NAmerUtil.Equations.FemaleAge2FirstMarriage,
                          Gender.Female);
            //line mothers last name with child
            UpsertName(KindsOfNames.Surname, _mother.LastName);

            var myMother = (NorthAmerican) _mother;
            myMother.Race = Race;
            BirthCert.Mother = _mother;

            //add self as one of mother's children
            myMother._children.Add(new Child(this));

            //TODO reslove this using data from census.gov
            ((AmericanBirthCert) BirthCert).BirthPlace =
                myMother.GetAddressAt(_birthCert.DateOfBirth)?.HomeCityArea as UsCityStateZip;

            //resolve mother's spouse(s)
            var motherMaritalStatus = NAmerUtil.GetMaritialStatus(_mother.BirthCert.DateOfBirth, Gender.Female);
            myMother.ResolveSpouse(motherMaritalStatus);
            
            //resolve for siblings
            myMother.ResolveChildren();

            myMother.AlignCohabitantsHomeDataAt(null, GetAddressAt(null));

            //father is whoever was married to mother at time of birth
            var myFather = myMother.GetSpouseAt(_birthCert.DateOfBirth)?.Est as NorthAmerican;

            //mother not married at time of birth
            if (motherMaritalStatus == MaritialStatus.Single || myFather == null)
            {
                //small percent of father unknown
                if (Etx.TryAboveOrAt(98, Etx.Dice.OneHundred))
                    return;
                _father =
                    _father ??
                    NAmerUtil.SolveForParent(_birthCert.DateOfBirth, NAmerUtil.Equations.MaleAge2FirstMarriage,
                        Gender.Male);
                return;
            }
            //mother will receive last name of spouse
            myFather.Race = Race;
            _father = myFather;
            _birthCert.Father = _father;

            //last name assigned from birth father
            if(_father != null)
                UpsertName(KindsOfNames.Surname, _father.LastName);
        }

        /// <summary>
        /// Will create a current and, possiably, past spouses for this instance.
        /// Calc' will be based on DOB and <see cref="IPerson.MyGender"/>.
        /// </summary>
        /// <param name="myMaritialStatus"></param>
        protected internal void ResolveSpouse(MaritialStatus myMaritialStatus)
        {
            if (myMaritialStatus == MaritialStatus.Single || myMaritialStatus == MaritialStatus.Unknown)
                return;

            var dt = DateTime.Now;

            ThrowOnBirthDateNull(this);

            var avgAgeMarriage = MyGender == Gender.Female
                ? NAmerUtil.Equations.FemaleAge2FirstMarriage.SolveForY(_birthCert.DateOfBirth.ToDouble())
                : NAmerUtil.Equations.MaleAge2FirstMarriage.SolveForY(_birthCert.DateOfBirth.ToDouble());
            var currentAge = Person.CalcAge(_birthCert.DateOfBirth, dt);

            //all other MaritialStatus imply at least one marriage in past
            var yearsMarried = currentAge - Convert.ToInt32(Math.Round(avgAgeMarriage));

            var marriedOn = Etx.Date(-1*yearsMarried, dt);

            var spouse = (NorthAmerican)NAmerUtil.SolveForSpouse(_birthCert.DateOfBirth, MyGender);

            //set death date if widowed
            if (myMaritialStatus == MaritialStatus.Widowed)
            {
                var d = Convert.ToInt32(Math.Round(GetAgeAt(null) * 0.15));
                spouse.DeathDate = Etx.Date(Etx.IntNumber(1, d), null);
            }

            if (myMaritialStatus != MaritialStatus.Divorced && myMaritialStatus != MaritialStatus.Remarried &&
                myMaritialStatus != MaritialStatus.Separated)
            {
                //add internal date-range for resolution of children
                AddNewSpouseToList(spouse, marriedOn);
            }
            else
            {
                //take date of marriage and add avg length of marriage
                var separatedDate = Etx.Date(NAmerUtil.AvgLengthOfMarriage, marriedOn);

                //reset date-range with separated date
                AddNewSpouseToList(spouse, marriedOn, separatedDate);

                //leave when no second spouse applicable
                if (myMaritialStatus != MaritialStatus.Remarried)
                    return;

                var ageSpread = 6;
                if (MyGender == Gender.Male)
                    ageSpread = 10;

                //get a second spouse
                var secondSpouse = (NorthAmerican)NAmerUtil.SolveForSpouse(_birthCert.DateOfBirth, MyGender, ageSpread);

                //random second marriage date
                var remarriedOn = Etx.Date(Convert.ToInt32(Math.Round(NAmerUtil.YearsBeforeNextMarriage)),
                    separatedDate);

                //add second date-range for resolution of children
                AddNewSpouseToList(secondSpouse, remarriedOn);
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
                    var nAmerSpouse = s.Est as NorthAmerican;
                    if (nAmerSpouse == null)
                        continue;
                    nAmerSpouse.ResolveChildren();

                    foreach (var childFrom in nAmerSpouse._children)
                    {
                        if (childFrom.Est.LastName != LastName && childFrom.Est.OtherNames.All(x => x.Item2 != LastName))
                            continue;
                        _children.Add(childFrom);
                    }
                }
                return;
            }

            ThrowOnBirthDateNull(this);

            var currentNumChildren = 0;

            //two extremes
            var teenPregEquation = NAmerUtil.Equations.GetProbTeenPregnancyByRace(Race);
            var teenageAge = Etx.IntNumber(15, 19);
            var teenageYear = _birthCert.DateOfBirth.AddYears(teenageAge).Year;
            var propTeenagePreg = teenPregEquation.SolveForY(teenageYear);

            var propLifetimeChildless = NAmerUtil.SolveForProbabilityChildless(_birthCert.DateOfBirth,
                Education?.EduLevel ?? OccidentalEdu.None);

            //far high-end is no children for whole life
            if (Etx.MyRand.NextDouble() <= propLifetimeChildless)
                return;

            //other extreme is teenage preg
            if (Etx.MyRand.NextDouble() <= propTeenagePreg)
            {
                var teenPregChildDob = Etx.Date(teenageAge, _birthCert.DateOfBirth);
                AddNewChildToList(teenPregChildDob);
                currentNumChildren += 1;
            }
            
            //last is averages
            var numOfChildren = NAmerUtil.SolveForNumberOfChildren(_birthCert.DateOfBirth, null);

            if (numOfChildren <= 0)
                return;

            for (var i = currentNumChildren; i < numOfChildren; i++)
            {
                var childDob = NAmerUtil.GetChildBirthDate(_birthCert.DateOfBirth, i, null);
                if (childDob == null)
                    continue;

                AddNewChildToList(childDob.Value);
            }
        }

        /// <summary>
        /// Add a random <see cref="IPerson"/> to the Children collection.
        /// </summary>
        /// <param name="myChildDob">
        /// This will be adusted up by when the Birth Date would occur during the pregnancy 
        /// of a sibling unless it is the exact same date (twins).
        /// </param>
        protected internal void AddNewChildToList(DateTime myChildDob)
        {
            if (MyGender == Gender.Male)
                return;

            var dt = DateTime.Now;
            DateTime dtOut;
            if (IsTwin(myChildDob, out dtOut) && DateTime.Compare(dtOut, DateTime.MinValue) != 0)
            {
                myChildDob = dtOut;
            }
            else
            {
                //throws ex when exceed mothers age.
                while (!IsValidDobOfChild(myChildDob))
                {
                    myChildDob = myChildDob.AddDays(PREG_DAYS + MS_DAYS);
                }  
            }

            var myChildeAge = CalcAge(myChildDob, dt);
            var myChildGender = Etx.CoinToss ? Gender.Female : Gender.Male;
            var isChildAdult = myChildeAge >= GetMyHomeStatesAgeOfMajority();
            var spouseAtChildDob = GetSpouseAt(myChildDob);

            var childLastName = string.IsNullOrWhiteSpace(spouseAtChildDob?.Est?.LastName) ||
                                spouseAtChildDob.Est?.MyGender == Gender.Female
                                ? LastName
                                : spouseAtChildDob.Est?.LastName;

            var nAmerChild = new NorthAmerican(myChildDob, myChildGender, this, spouseAtChildDob?.Est)
            {
                LastName = childLastName
            };
            if (isChildAdult)
                nAmerChild.ResolveFamilyState(true);
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

            if (childDob.ComparedTo(minDate) == ChronoCompare.Before ||
                childDob.ComparedTo(maxDate) == ChronoCompare.After)
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
        protected internal void AddNewSpouseToList(IPerson spouse, DateTime marriedOn, DateTime? separatedOn = null)
        {
            //we need this or will will blow out the stack 
            if (_spouses.Any(x => DateTime.Compare(x.MarriedOn, marriedOn) == 0))
                return;

            if (separatedOn == null)
            {
                //when this is the bride
                if (_myGender == Gender.Female)
                {
                    if (LastName != null && OtherNames.All(x => x.Item1 != KindsOfNames.Maiden))
                        UpsertName(KindsOfNames.Maiden, BirthCert.Father?.LastName ?? LastName);

                    LastName = spouse.LastName;
                }
            }
            else if (MyGender == Gender.Female)
            {
                //add ex-husband last name to list
                UpsertName(KindsOfNames.Former | KindsOfNames.Surname | KindsOfNames.Spouse, spouse.LastName);

                //set back to maiden name
                var maidenName = OtherNames.FirstOrDefault(x => x.Item1 == KindsOfNames.Maiden);
                if (!string.IsNullOrWhiteSpace(maidenName?.Item2))
                    LastName = maidenName.Item2;
            }
            _spouses.Add(new Spouse
            {
                Est = spouse,
                MarriedOn = marriedOn,
                SeparatedOn = separatedOn,
                Ordinal = _spouses.Count + 1
            });

            //recepricate to spouse
            var nAmerSpouse = spouse as NorthAmerican;
            nAmerSpouse?.AddNewSpouseToList(this, marriedOn, separatedOn);
        }

        protected override bool IsLegalAdult(DateTime? dt)
        {
            return GetAgeAt(dt) > GetMyHomeStatesAgeOfMajority();
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