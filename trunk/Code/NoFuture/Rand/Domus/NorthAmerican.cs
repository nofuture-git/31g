using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class NorthAmerican : Person
    {
        #region fields
        private readonly List<Spouse> _spouses = new List<Spouse>();
        private Gender _myGender;
        internal readonly List<Tuple<KindsOfLabels, NorthAmericanPhone>> _phoneNumbers = 
            new List<Tuple<KindsOfLabels, NorthAmericanPhone>>();
        private SocialSecurityNumber _ssn;
        #endregion

        #region ctors

        public NorthAmerican(DateTime dob, Gender myGender, bool withWholeFamily = false):base(dob)
        {
            _birthCert = new AmericanBirthCert(this) { DateOfBirth = dob };
            _myGender = myGender;

            var fname = _myGender != Gender.Unknown ? NAmerUtil.GetAmericanFirstName(_birthCert.DateOfBirth, _myGender) : "Pat";
            _otherNames.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Firstname, fname));
            var lname = NAmerUtil.GetAmericanLastName();
            _otherNames.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Surname, lname));

            MiddleName = NAmerUtil.GetAmericanFirstName(_birthCert.DateOfBirth, _myGender);

            var csz = CityArea.American();
            var homeAddr = StreetPo.American();
            _addresses.Add(new HomeAddress {HomeStreetPo = homeAddr, HomeCityArea = csz});

            var abbrv = csz?.PostalState;

            //http://www.pewresearch.org/fact-tank/2014/07/08/two-of-every-five-u-s-households-have-only-wireless-phones/
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
        public override Gender MyGender
        {
            get { return _myGender; }
            set { _myGender = value; }
        }

        public string MiddleName
        {
            get { return _otherNames.First(x => x.Item1 == KindsOfNames.Middle).Item2; }
            set { UpsertName(KindsOfNames.Middle, value); }

        }

        public NorthAmericanPhone HomePhone
        {
            get
            {
                var hph = _phoneNumbers.FirstOrDefault(x => x.Item1 == KindsOfLabels.Home);
                return hph?.Item2;
            }
        }

        public NorthAmericanPhone CellPhone
        {
            get
            {
                var mobilePh = _phoneNumbers.FirstOrDefault(x => x.Item1 == KindsOfLabels.Mobile);
                return mobilePh?.Item2;
            }
        }

        public virtual SocialSecurityNumber Ssn { get {return _ssn;} set { _ssn = value; } }

        public NorthAmericanRace Race { get; set; }

        public DriversLicense DriversLicense => GetDriversLicenseAt(null);

        #endregion

        public virtual DriversLicense GetDriversLicenseAt(DateTime? dt)
        {
            if (GetAgeAt(dt) < UsState.MIN_AGE_FOR_DL)
                return null;

            var csz = GetAddressAt(dt)?.HomeCityArea;
            var amerCsz = csz as UsCityStateZip;
            var dl = amerCsz?.State.DriversLicenseFormats[0];
            return dl;
        }

        public override MaritialStatus GetMaritalStatusAt(DateTime? dt)
        {
            var mdt = dt ?? DateTime.Now;

            var iAmUnderage = GetAgeAt(mdt) < GetMyHomeStatesAgeOfMajority();

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

                if (spouseData.SO == null || spouseData.SO.DeathDate == null)
                    continue;

                days = (int)Math.Abs((spouseData.SO.DeathDate.Value - _birthCert.DateOfBirth).TotalDays);
                while (myTimeline.ContainsKey(days))
                    days += 1;

                myTimeline.Add(days, MaritialStatus.Widowed);
            }

            var mdtDays = (int)Math.Abs((mdt - _birthCert.DateOfBirth).TotalDays);

            return myTimeline.Any(x => x.Key >= mdtDays)
                ? myTimeline.First(x => x.Key >= mdtDays).Value
                : myTimeline.Last().Value;
        }

        public override Spouse GetSpouseAt(DateTime? dt)
        {
            var cdt = dt ?? DateTime.Now;

            if (GetAgeAt(cdt) < GetMyHomeStatesAgeOfMajority())
            {
                return null;
            }

            var spouseData = _spouses.FirstOrDefault(x => DateTime.Compare(x.MarriedOn, cdt) <= 0 && x.SeparatedOn == null);
            return spouseData;
        }

        public void AlignCohabitantsHomeDataAt(DateTime? dt, HomeAddress addr)
        {
            if (addr == null)
                return;
            UpsertAddress(addr);
            var ms = GetMaritalStatusAt(dt);
            if ((ms == MaritialStatus.Married || ms == MaritialStatus.Remarried) && GetSpouseAt(dt) != null)
            {
                NAmerUtil.SetNAmerCohabitants((NorthAmerican)GetSpouseAt(dt).SO, this);
            }
            var underAgeChildren = _children.Cast<NorthAmerican>().Where(x => x.GetAgeAt(dt) < UsState.AGE_OF_ADULT).ToList();
            if (underAgeChildren.Count <= 0)
                return;
            foreach (var child in underAgeChildren)
                NAmerUtil.SetNAmerCohabitants(child, this);
        }

        #region internal methods
        /// <summary>
        /// Assigns <see cref="Race"/> and invokes <see cref="ResolveParents"/>, <see cref="ResolveSpouse"/> and <see cref="ResolveChildren"/>.
        /// Only Parents and Race are certian the other resolutions contrained by age and randomness.
        /// </summary>
        protected internal void ResolveFamilyState()
        {
            ThrowOnBirthDateNull(this);
            var dt = DateTime.Now;
            
            ResolveParents();
            //resolve spouse to each other
            ResolveSpouse(NAmerUtil.GetMaritialStatus(_birthCert.DateOfBirth, MyGender));
            //to solve for childern when gender -eq Male
            ResolveChildren();
            AlignCohabitantsHomeDataAt(dt, GetAddressAt(null));
        }

        protected internal void ResolveParents()
        {
            ThrowOnBirthDateNull(this);

            //create current instance mother
            _mother = _mother ??
                      NAmerUtil.SolveForParent(_birthCert.DateOfBirth, NAmerUtil.Equations.FemaleYearOfMarriage2AvgAge,
                          Gender.Female);
            //line mothers last name with child
            UpsertName(KindsOfNames.Surname, _mother.LastName);

            var myMother = (NorthAmerican) _mother;
            myMother.Race = Race;
            BirthCert.Mother = _mother;

            //add self as one of mother's children
            myMother._children.Add(this);

            //TODO reslove this using data from census.gov
            ((AmericanBirthCert) BirthCert).BirthPlace = myMother.GetAddressAt(null)?.HomeCityArea as UsCityStateZip;

            //resolve mother's spouse(s)
            var motherMaritalStatus = NAmerUtil.GetMaritialStatus(_mother.BirthCert.DateOfBirth, Gender.Female);
            myMother.ResolveSpouse(motherMaritalStatus);
            
            //resolve for siblings
            myMother.ResolveChildren();

            myMother.AlignCohabitantsHomeDataAt(null, GetAddressAt(null));

            //father is whoever was married to mother at time of birth
            var myFather = myMother.GetSpouseAt(_birthCert.DateOfBirth)?.SO as NorthAmerican;

            //mother not married at time of birth
            if (motherMaritalStatus == MaritialStatus.Single || myFather == null)
            {
                //small percent of father unknown
                if (Etx.TryAboveOrAt(98, Etx.Dice.OneHundred))
                    return;
                _father =
                    _father ??
                    NAmerUtil.SolveForParent(_birthCert.DateOfBirth, NAmerUtil.Equations.MaleYearOfMarriage2AvgAge,
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

        protected internal void ResolveSpouse(MaritialStatus myMaritialStatus)
        {
            if (myMaritialStatus == MaritialStatus.Single || myMaritialStatus == MaritialStatus.Unknown)
                return;

            var dt = DateTime.Now;

            ThrowOnBirthDateNull(this);

            var avgAgeMarriage = MyGender == Gender.Female
                ? NAmerUtil.Equations.FemaleDob2MarriageAge.SolveForY(_birthCert.DateOfBirth.ToDouble())
                : NAmerUtil.Equations.MaleDob2MarriageAge.SolveForY(_birthCert.DateOfBirth.ToDouble());
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
        /// Will only function when <see cref="MyGender"/> is <see cref="Gender.Female"/> since all equations used
        /// are derived from woman-only datasets.
        /// Resolves to some random amount of <see cref="IPerson"/> 
        /// added to the Children collection.  
        /// </summary>
        protected internal void ResolveChildren()
        {
            //equations data is by women only.
            if (MyGender == Gender.Male)
            {
                foreach (var s in _spouses.Where(x => x.SO != null && x.SO.MyGender == Gender.Female))
                {
                    var nAmerSpouse = s.SO as NorthAmerican;
                    if (nAmerSpouse == null)
                        continue;
                    nAmerSpouse.ResolveChildren();

                    foreach (var childFrom in nAmerSpouse._children)
                    {
                        if (childFrom.LastName != LastName && childFrom.OtherNames.All(x => x.Item2 != LastName))
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
                Education?.GetEduLevel(null) ?? OccidentalEdu.Empty);

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
        /// Add a random <see cref="IPerson"/> to the Children collection
        /// aligning <see cref="IPerson"/> last name and father to match
        /// spouses.
        /// </summary>
        /// <param name="myChildDob">
        /// This will be adusted up by when the Birth Date would occur during the pregnancy 
        /// of a sibling unless it is the exact same date (twins).
        /// </param>
        protected internal void AddNewChildToList(DateTime myChildDob)
        {
            if (MyGender == Gender.Male)
            {
                return;
            }

            var dt = DateTime.Now;
            DateTime dtOut;
            NorthAmerican myChild = null;
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

            var isDaughterMarriedOff = myChildGender == Gender.Female &&
                                       NAmerUtil.Equations.FemaleDob2MarriageAge.SolveForY(myChildDob.Year) >=
                                       myChildeAge && myChildeAge >= GetMyHomeStatesAgeOfMajority();
            //never married
            if (!_spouses.Any())
            {
                myChild = new NorthAmerican(myChildDob, myChildGender, this, null) {LastName = LastName, Race = Race};
                //default to mother last name
                _children.Add(myChild);
                return;
            }

            //assign child father and lastname when born within range of marriage
            for (var i = 0; i <= _spouses.Max(x => x.Ordinal); i++)
            {
                var marriage = _spouses.FirstOrDefault(x => x.Ordinal == i);
                if (marriage == null)
                    continue;

                if (myChildDob.ComparedTo(marriage.MarriedOn) == ChronoCompare.After &&
                    (marriage.SeparatedOn == null ||
                     myChildDob.ComparedTo(marriage.SeparatedOn.Value) == ChronoCompare.Before))
                {

                    myChild = new NorthAmerican(myChildDob, myChildGender, this, marriage.SO) {Race = Race};
                    if (isDaughterMarriedOff)
                        myChild.OtherNames.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Maiden,
                            marriage.SO.LastName));
                    else
                        myChild.LastName = marriage.SO.LastName;
                }
            }

            //for child born in any range outside of marriage, assign lastname to maiden name
            if (myChild == null)
            {
                var maidenName = OtherNames.FirstOrDefault(x => x.Item1 == KindsOfNames.Maiden);
                if (!string.IsNullOrWhiteSpace(maidenName?.Item2))
                {
                    myChild = new NorthAmerican(myChildDob, myChildGender, this, null)
                    {
                        LastName = maidenName.Item2,
                        Race = Race
                    };
                }
                else
                {
                    myChild = new NorthAmerican(myChildDob, myChildGender, this, null)
                    {
                        LastName = LastName,
                        Race = Race
                    };
                }
            }
            _children.Add(myChild);
        }

        protected internal bool IsTwin(DateTime childDob, out DateTime minutesAfterChildDob)
        {
            var siblingsBdays = _children.Where(x => x.BirthCert != null).Select(x => x.BirthCert.DateOfBirth).ToList();

            if (siblingsBdays.Any(x => DateTime.Compare(x.Date, childDob.Date) == 0))
            {
                childDob = siblingsBdays.First(x => DateTime.Compare(x.Date, childDob.Date) == 0);
                minutesAfterChildDob = childDob.AddMinutes(Etx.IntNumber(2, 8)).AddSeconds(Etx.IntNumber(0, 59));
                return true;
            }
            minutesAfterChildDob = DateTime.MinValue;
            return false;

        }

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
                _children.Where(x => x.BirthCert != null)
                    .Select(
                        x =>
                            new Tuple<DateTime, DateTime>(x.BirthCert.DateOfBirth.AddDays(-1*PREG_DAYS),
                                x.BirthCert.DateOfBirth.AddDays(MS_DAYS))).ToList();
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
                SO = spouse,
                MarriedOn = marriedOn,
                SeparatedOn = separatedOn,
                Ordinal = _spouses.Count + 1
            });

            //recepricate to spouse
            var nAmerSpouse = spouse as NorthAmerican;
            nAmerSpouse?.AddNewSpouseToList(this, marriedOn, separatedOn);
        }

        private int GetMyHomeStatesAgeOfMajority()
        {
            if (GetAddressAt(null) == null)
                return UsState.AGE_OF_ADULT;
            var myHomeState = UsState.GetStateByPostalCode(GetAddressAt(null)?.HomeCityArea?.AddressData?.StateAbbrv);
            return myHomeState?.AgeOfMajority ?? UsState.AGE_OF_ADULT;
        }

        #endregion
    }

    //container class for Race probability tables
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class AmericanRaceProbabilityRange
    {
        internal NorthAmericanRace Name { get; set; }
        internal double From { get; set; }
        internal double To { get; set; }
    }
}