using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Edu.US;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Geo.US;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US;
using NoFuture.Rand.Pneuma;
using NoFuture.Rand.Tele;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Domus.US
{
    /// <inheritdoc cref="Person" />
    /// <summary>
    /// The extended of <see cref="Person"/> type for 
    /// a North American.
    /// </summary>
    [Serializable]
    public class American : Person
    {
        #region fields

        private IEducation _edu;
        private readonly List<NorthAmericanPhone> _phoneNumbers = new List<NorthAmericanPhone>();
        private SocialSecurityNumber _ssn;
        private DriversLicense _dl;
        private DeathCert _deathCert;
        #endregion

        #region properties

        public override string FullName => String.Join(" ", FirstName, MiddleName, LastName);

        public override IEducation Education
        {
            get => _edu;
            set => _edu = value;
        }

        public override IEnumerable<Phone> PhoneNumbers => _phoneNumbers;

        /// <summary>
        /// Gets the phone home phone of the given person if any.
        /// </summary>
        public NorthAmericanPhone HomePhone => _phoneNumbers.FirstOrDefault(x => x.Descriptor == KindsOfLabels.Home);

        /// <summary>
        /// Gets the mobile phone of the given person if any.
        /// </summary>
        public NorthAmericanPhone MobilePhone => _phoneNumbers.FirstOrDefault(x => x.Descriptor == KindsOfLabels.Mobile);

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
        /// Get the <see cref="Gov.US.DriversLicense"/> at the current time.
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

        #region methods
        /// <summary>
        /// Gets this person's <see cref="MaritialStatus"/> at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt">Null for the current time</param>
        /// <returns></returns>
        public override MaritialStatus GetMaritalStatusAt(DateTime? dt)
        {
            var mdt = dt ?? DateTime.Now;
            var spouses = GetSpouses();
            if (!IsLegalAdult(mdt) ||!spouses.Any() || spouses.All(s => s.MarriedOn > mdt))
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
                GetSpouses().FirstOrDefault(
                    x =>
                        x.MarriedOn.Date <= dt &&
                        (x.Terminus == null || x.Terminus.Value.Date > dt));
            return spouseData;
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
                return null;
            }

            if (dt == null)
                return _edu;

            if(_edu.EduFlag == OccidentalEdu.None)
                return _edu;

            var hsGradDt = _edu.HighSchool == null ? dt.Value.AddDays(-1) : _edu.HighSchool.Item2;

            if (hsGradDt < dt)
            {
                return new AmericanEducation(new Tuple<IHighSchool, DateTime?>(_edu?.HighSchool?.Item1, null));
            }

            var univGradDt = _edu.College == null ? dt.Value.AddDays(-1) : _edu.College.Item2;

            if (univGradDt < dt)
            {
                return new AmericanEducation(new Tuple<IUniversity, DateTime?>(_edu?.College?.Item1, null),
                    new Tuple<IHighSchool, DateTime?>(_edu?.HighSchool?.Item1, _edu?.HighSchool?.Item2));
            }

            return _edu;

        }

        public override void AddPhone(Phone phone)
        {
            if (!(phone is NorthAmericanPhone namerPhone))
                return;
            
            _phoneNumbers.Add(namerPhone);
        }

        public override void AddPhone(string phoneNumber, KindsOfLabels? descriptor = null)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return;
            if (!NorthAmericanPhone.TryParse(phoneNumber, out var namerPhone))
                return;

            namerPhone.Descriptor = descriptor;
            _phoneNumbers.Add(namerPhone);
        }

        /// <summary>
        /// Gets an education at random using this persons data.
        /// </summary>
        /// <returns></returns>
        protected internal AmericanEducation GetRandomEducation()
        {
            var dob = BirthCert?.DateOfBirth ?? Etx.RandomAdultBirthDate();

            //determine where amer lived when they were 18
            var mother = BirthCert == null
                ? AmericanUtil.RandomParent(dob,
                    Gender.Female) as American
                : GetBiologicalMother() as American;
            var dtAtAge18 = dob.AddYears(UsState.AGE_OF_ADULT);
            var homeCityArea = mother?.GetAddressAt(dtAtAge18)?.HomeCityArea as UsCityStateZip ?? CityArea.RandomAmericanCity();
            return AmericanEducation.RandomEducation(dob, homeCityArea.StateAbbrev, homeCityArea.ZipCode);
        }

        /// <summary>
        /// Utility method to assign the <see cref="addr"/> to this intance, 
        /// the spouse and children who are under <see cref="UsState.AGE_OF_ADULT"/> 
        /// given the <see cref="dt"/>.
        /// </summary>
        /// <param name="dt">Null for current time.</param>
        /// <param name="addr"></param>
        public void AlignCohabitantsHomeDataAt(DateTime? dt, PostalAddress addr)
        {
            if (addr == null)
                return;
            dt = dt ?? DateTime.Now;
            AddAddress(addr);

            Func<Child, bool> isUnderageChild =
                child => child?.Est is American && !((American) child.Est).IsLegalAdult(dt);

            var underAgeChildren =Children.Where(x => isUnderageChild(x)).ToList();

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
                                Etx.RandomRollAboveOrAt(AmericanData.PERCENT_DIVORCED_CHILDREN_LIVE_WITH_MOTHER+1,
                                    Etx.Dice.OneHundred)
                    ? namerChild.GetBiologicalFather()
                    : namerChild.GetBiologicalMother();

                var namerLivesWith = livesWith as American;
                if (namerLivesWith == null)
                    continue;

                if (namerLivesWith.Address == null)
                    namerLivesWith.AddAddress(
                        PostalAddress.RandomAmericanAddress(Address.HomeCityArea.GetPostalCodePrefix()));

                AmericanUtil.SetNAmerCohabitants(namerChild, namerLivesWith);
            }
        }

        /// <summary>
        /// Resolves the <see cref="Gov.US.DriversLicense"/> which was 
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
            var dlFormats = UsState.GetState(amerCsz?.StateName)?.DriversLicenseFormats;
            if (dlFormats == null || !dlFormats.Any())
                return null;
            _dl = dlFormats[0].IssueNewLicense(dt);
            _dl.Dob = BirthCert.DateOfBirth;
            _dl.FullLegalName = String.Join(" ", FirstName.ToUpper(), MiddleName.ToUpper(),
                LastName.ToUpper());
            _dl.Gender = MyGender.ToString();
            _dl.PrincipalResidence = Address.ToString();
            return _dl;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is IPerson))
                return false;

            //must be an american
            if (!(obj is American amer))
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
            ResolveSpouse(AmericanUtil.RandomMaritialStatus(BirthCert.DateOfBirth, MyGender));
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
            var mother = GetBiologicalMother() ??
                      AmericanUtil.RandomParent(BirthCert.DateOfBirth, Gender.Female);
            //line mothers last name with child
            UpsertName(KindsOfNames.Surname, mother.LastName);

            var myMother = (American)mother;
            myMother.Race = Race;
            BirthCert.MotherName = mother.FullName;

            //add self as one of mother's children
            myMother.AddChild(this);

            //TODO reslove this using data from census.gov
            var birthCity = myMother.GetAddressAt(BirthCert.DateOfBirth)?.HomeCityArea as UsCityStateZip;
            if (birthCity != null)
            {
                ((AmericanBirthCert) BirthCert).City = birthCity.City;
                ((AmericanBirthCert)BirthCert).State = birthCity.StateAbbrev;
            }

            //resolve mother's spouse(s)
            var motherMaritalStatus = AmericanUtil.RandomMaritialStatus(mother.BirthCert.DateOfBirth, Gender.Female);
            myMother.ResolveSpouse(motherMaritalStatus);
            
            //resolve for siblings
            myMother.ResolveChildren();

            AddParent(myMother, KindsOfNames.Mother | KindsOfNames.Biological);

            //father is whoever was married to mother around time of birth
            var myFather = myMother.GetSpouseNear(BirthCert.DateOfBirth)?.Est as American;

            //mother not married at time of birth
            if (motherMaritalStatus == MaritialStatus.Single || myFather == null)
            {
                //small percent of father unknown
                if (Etx.RandomRollAboveOrAt(98, Etx.Dice.OneHundred))
                    return;
                AddParent(AmericanUtil.RandomParent(BirthCert.DateOfBirth, Gender.Male),
                    KindsOfNames.Biological | KindsOfNames.Father);
                return;
            }
            //mother will receive last name of spouse
            myFather.Race = Race;
            BirthCert.FatherName = myFather.FullName;
            UpsertName(KindsOfNames.Surname, myFather.LastName);
            myFather.AddChild(this);
            AddParent(myFather, KindsOfNames.Biological | KindsOfNames.Father);
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

            var equationDt = AmericanEquations.ProtectAgainstDistantTimes(BirthCert.DateOfBirth);

            var avgAgeMarriage = MyGender == Gender.Female
                ? AmericanEquations.FemaleAge2FirstMarriage.SolveForY(equationDt.ToDouble())
                : AmericanEquations.MaleAge2FirstMarriage.SolveForY(equationDt.ToDouble());
            var currentAge = Etc.CalcAge(BirthCert.DateOfBirth, dt);

            //all other MaritialStatus imply at least one marriage in past
            var yearsMarried = currentAge - Convert.ToInt32(Math.Round(avgAgeMarriage));

            var marriedOn = Etx.RandomDate(-1*yearsMarried, dt).Date.AddHours(12);

            var spouse = (American)AmericanUtil.RandomSpouse(BirthCert.DateOfBirth, MyGender);

            //set death date if widowed
            if (myMaritialStatus == MaritialStatus.Widowed || spouse.DeathCert != null)
            {
                var d = Convert.ToInt32(Math.Round(GetAgeAt(null) * 0.15));
                myMaritialStatus = MaritialStatus.Widowed;
                spouse.DeathCert = spouse.DeathCert ??
                                   new AmericanDeathCert(Etx.RandomPickOne(AmericanData.MannerOfDeathAvgs),
                                       spouse.FullName)
                                   {
                                       DateOfDeath = Etx.RandomDate(Etx.RandomInteger(1, d) * -1)
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
                var separatedDate = Etx.RandomDate(AmericanData.AVG_LENGTH_OF_MARRIAGE, marriedOn);

                //reset date-range with separated date
                AddSpouse(spouse, marriedOn, separatedDate);

                //leave when no second spouse applicable
                if (myMaritialStatus != MaritialStatus.Remarried)
                    return;

                var ageSpread = 6;
                if (MyGender == Gender.Male)
                    ageSpread = 10;

                //get a second spouse
                var secondSpouse = (American)AmericanUtil.RandomSpouse(BirthCert.DateOfBirth, MyGender, ageSpread);

                //random second marriage date
                var remarriedOn = Etx.RandomDate(Convert.ToInt32(Math.Round(AmericanData.YEARS_BEFORE_NEXT_MARRIAGE)),
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
                foreach (var s in GetSpouses().Where(x => x.Est != null && x.Est.MyGender == Gender.Female))
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
            var teenageAge = Etx.RandomInteger(AmericanUtil.MIN_AGE_TO_BE_PARENT, 19);
            var teenageYear = BirthCert.DateOfBirth.AddYears(teenageAge).Year;
            var propTeenagePreg = teenPregEquation.SolveForY(teenageYear);

            var propLifetimeChildless = AmericanUtil.SolveForProbabilityChildless(BirthCert.DateOfBirth,
                Education?.EduFlag ?? OccidentalEdu.None);

            //far high-end is no children for whole life
            if (Etx.MyRand.NextDouble() <= propLifetimeChildless)
                return;

            //other extreme is teenage preg
            if (Etx.MyRand.NextDouble() <= propTeenagePreg)
            {
                var teenPregChildDob = Etx.RandomDate(teenageAge, BirthCert.DateOfBirth);
                AddChild(teenPregChildDob);
                currentNumChildren += 1;
            }
            
            //last is averages
            var numOfChildren = AmericanUtil.RandomNumberOfChildren(BirthCert.DateOfBirth, null);

            if (numOfChildren <= 0)
                return;

            for (var i = currentNumChildren; i < numOfChildren; i++)
            {
                var childDob = AmericanUtil.RandomChildBirthDate(BirthCert.DateOfBirth, i, null);
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
            var myChildGender = Etx.RandomCoinToss() ? Gender.Female : Gender.Male;
            var isChildAdult = myChildeAge >= GetMyHomeStatesAgeOfMajority();

            //look for spouse at and around Dob
            var spouseAtChildDob = GetSpouseNear(myChildDob);

            var childLastName = String.IsNullOrWhiteSpace(spouseAtChildDob?.Est?.LastName) ||
                                spouseAtChildDob.Est?.MyGender == Gender.Female
                                ? GetName(KindsOfNames.Maiden) ?? LastName
                                : spouseAtChildDob.Est?.LastName;

            var childRace = Race | (spouseAtChildDob?.Est as American)?.Race ?? Race;
            var nAmerChild = RandomAmerican(myChildDob, myChildGender, this, spouseAtChildDob?.Est);
            nAmerChild.LastName = childLastName;
            nAmerChild.Race = childRace;

            //check that child does not share the same first name as a sibling
            while (Children.Any(x => x.Est.FirstName == nAmerChild.FirstName))
            {
                nAmerChild.UpsertName(KindsOfNames.First,
                    AmericanUtil.RandomAmericanFirstName(nAmerChild.MyGender, myChildDob));
            }

            //child has ref to father, father needs ref to child
            if (spouseAtChildDob?.Est is American nAmerFather && nAmerFather.MyGender == Gender.Male
                                                              && nAmerFather.Children.All(
                                                                  x => !nAmerChild.Equals(x.Est)))
            {
                nAmerFather.AddChild(nAmerChild);
            }

            //resolve spouse, no grand-children
            if (isChildAdult)
            {
                nAmerChild.ResolveSpouse(AmericanUtil.RandomMaritialStatus(myChildDob, myChildGender));
                nAmerChild.AlignCohabitantsHomeDataAt(DateTime.Now, nAmerChild.GetAddressAt(null));
            }

            AddChild(nAmerChild);
            if (MyGender == Gender.Female)
                nAmerChild.AddParent(this, KindsOfNames.Mother | KindsOfNames.Biological);
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
                AddUri(
                    Email.RandomEmail(GetAgeAt(null) >= UsState.AGE_OF_ADULT, GetName(KindsOfNames.First),
                        MiddleName, GetName(KindsOfNames.Surname)).ToUri());
        }

        //min. age a person could be married at
        protected internal int GetMyHomeStatesAgeOfMajority()
        {
            if (GetAddressAt(null) == null)
                return UsState.AGE_OF_ADULT;
            var myHomeState = UsState.GetStateByPostalCode(GetAddressAt(null)?.HomeCityArea?.AddressData?.StateAbbrev);
            return myHomeState?.AgeOfMajority ?? UsState.AGE_OF_ADULT;
        }

        protected internal List<NorthAmericanPhone> GetPhoneNumbers()
        {
            return _phoneNumbers;
        }

        /// <summary>
        /// Gets an <see cref="American"/> with all values selected at random.
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static American RandomAmerican()
        {
            return RandomAmerican(Etx.RandomAdultBirthDate(), Etx.RandomCoinToss() ? Gender.Female : Gender.Male, true);
        }

        /// <summary>
        /// Gets an <see cref="American"/> at random with the given birth date and gender
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="myGender"></param>
        /// <returns></returns>
        [RandomFactory]
        public static American RandomAmerican(DateTime dob, Gender myGender)
        {
            var cDob = dob;
            var cGender = myGender;
            var dobAddr = CityArea.RandomAmericanCity();
            var amer = new American();
            amer.BirthCert = new AmericanBirthCert(amer.FullName)
            {
                DateOfBirth = cDob,
                City = dobAddr.City,
                State = dobAddr.StateAbbrev
            };
            amer.Personality = Personality.RandomPersonality();

            //almost always returns null
            amer.DeathCert = AmericanUtil.GetRandomDeathCert(amer);
            amer.MyGender = cGender;

            amer.FirstName = amer.MyGender != Gender.Unknown
                ? AmericanUtil.RandomAmericanFirstName(amer.MyGender, amer.BirthCert.DateOfBirth)
                : "Pat";

            amer.LastName = AmericanUtil.RandomAmericanLastName();

            amer.MiddleName = AmericanUtil.RandomAmericanFirstName(amer.MyGender, amer.BirthCert.DateOfBirth);
            while (String.Equals(amer.FirstName, amer.MiddleName, StringComparison.OrdinalIgnoreCase))
            {
                amer.MiddleName = AmericanUtil.RandomAmericanFirstName(amer.MyGender, amer.BirthCert.DateOfBirth);
            }
            amer.Ssn = SocialSecurityNumber.RandomSsn();
            if (amer.Race <= 0)
                amer.Race = Etx.RandomPickOne(AmericanRacePercents.NorthAmericanRaceAvgs);

            return amer;
        }

        /// <summary>
        /// Gets an <see cref="American"/> at random with the given birth date and gender along with 
        /// their family, education, address, etc.
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="myGender"></param>
        /// <param name="withFamily"></param>
        /// <returns></returns>
        [RandomFactory]
        public static American RandomAmerican(DateTime dob, Gender myGender, bool withFamily)
        {
            var amer = RandomAmerican(dob, myGender);
            if (amer.DeathCert != null)
                return amer;

            var homeAddr = PostalAddress.RandomAmericanAddress();
            amer.GetAddresses().Add(homeAddr);
            var csz = homeAddr.HomeCityArea as UsCityStateZip;

            var abbrv = csz?.StateAbbrev;

            //may be null
            //[http://www.pewresearch.org/fact-tank/2014/07/08/two-of-every-five-u-s-households-have-only-wireless-phones/]
            if (Etx.RandomRollAboveOrAt(6, Etx.Dice.Ten))
            {
                var hmPhone = NorthAmericanPhone.RandomAmericanPhone(abbrv);
                hmPhone.Descriptor = KindsOfLabels.Home;
                amer.AddPhone(hmPhone);
            }

            var isSmallChild = amer.GetAgeAt(null) < 12;

            if (!isSmallChild)
            {
                var mobilePh = NorthAmericanPhone.RandomAmericanPhone(abbrv);
                mobilePh.Descriptor = KindsOfLabels.Mobile;
                amer.AddPhone(mobilePh);
            }

            amer.Race = UsCityStateZip.GetAmericanRace(csz?.ZipCode);
            if (amer.Race <= 0)
                amer.Race = Etx.RandomPickOne(AmericanRacePercents.NorthAmericanRaceAvgs);

            if (withFamily)
                amer.ResolveFamilyState();

            amer.AddEmailAddress();

            amer.Education = amer.GetRandomEducation();

            return amer;
        }

        /// <summary>
        /// Gets an <see cref="American"/> at random with the given birth date, gender and parents.
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="myGender"></param>
        /// <param name="mother"></param>
        /// <param name="father"></param>
        /// <returns></returns>
        [RandomFactory]
        public static American RandomAmerican(DateTime dob, Gender myGender, IPerson mother, IPerson father)
        {
            var amer = RandomAmerican(dob, myGender);

            amer.AddParent(mother, KindsOfNames.Biological | KindsOfNames.Mother);
            amer.AddParent(father, KindsOfNames.Biological | KindsOfNames.Father);

            amer.BirthCert.MotherName = amer.GetBiologicalMother()?.FullName;
            amer.BirthCert.FatherName = amer.GetBiologicalFather()?.FullName;
            if (!(mother is American nAmerMother))
                return amer;
            var americanBirthCert = (AmericanBirthCert)amer.BirthCert;
            var nAmerFather = father as American;

            var birthPlace = nAmerMother.GetAddressAt(dob)?.HomeCityArea as UsCityStateZip ??
                             nAmerFather?.GetAddressAt(dob)?.HomeCityArea as UsCityStateZip ??
                             CityArea.RandomAmericanCity();

            americanBirthCert.City = birthPlace.City;
            americanBirthCert.State = birthPlace.StateAbbrev;
            amer.Race = nAmerMother.Race;
            return amer;
        }

        #endregion
    }
}