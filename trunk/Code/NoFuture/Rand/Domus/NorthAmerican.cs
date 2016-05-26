using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NoFuture.Exceptions;
using NoFuture.Rand.Domus.Pneuma;
using NoFuture.Rand.Domus.Sp;
using NoFuture.Rand.Gov;
using NoFuture.Shared;
using NoFuture.Util;

namespace NoFuture.Rand.Domus
{
    [Serializable]
    public class NorthAmerican : Person
    {
        #region fields
        private readonly List<Tuple<Address, CityArea>> _formerAddresses = new List<Tuple<Address, CityArea>>();
        private readonly List<ILoan> _debts = new List<ILoan>();
        private IPerson _mother;
        private IPerson _father;
        private readonly List<SpouseData> _spouses = new List<SpouseData>();
        private Gender _myGender;
        private string _fname;
        protected string _lname;
        internal readonly List<Tuple<KindsOfLabels, NorthAmericanPhone>> _phoneNumbers = new List<Tuple<KindsOfLabels, NorthAmericanPhone>>();
        #endregion

        #region ctors

        public NorthAmerican(DateTime dob, Gender myGender, bool withWholeFamily = false):base(dob)
        {
            _birthCert = new AmericanBirthCert(this) { DateOfBirth = dob };
            _myGender = myGender;

            _fname = _myGender != Gender.Unknown ? NAmerUtil.GetAmericanFirstName(_birthCert.DateOfBirth, _myGender) : "Pat";
            _lname = NAmerUtil.GetAmericanLastName();

            MiddleName = NAmerUtil.GetAmericanFirstName(_birthCert.DateOfBirth, _myGender);

            var csz = CityArea.American(null);
            HomeAddress = Address.American();
            HomeCityArea = csz;

            var abbrv = csz.State.StateAbbrv;

            //http://www.pewresearch.org/fact-tank/2014/07/08/two-of-every-five-u-s-households-have-only-wireless-phones/
            if(Etx.IntNumber(1,10)<=6)
                _phoneNumbers.Add(new Tuple<KindsOfLabels, NorthAmericanPhone>(KindsOfLabels.Home, Phone.American(abbrv)));

            if(GetAge(null) >= 12)
                _phoneNumbers.Add(new Tuple<KindsOfLabels, NorthAmericanPhone>(KindsOfLabels.Mobile, Phone.American(abbrv)));

            Ssn = new SocialSecurityNumber();
            if(GetAge(null) >= UsState.MIN_AGE_FOR_DL)
                DriversLicense = csz.State.Formats[0];

            //http://www.internic.net/zones/root.zone

            if(withWholeFamily)
                ResolveFamilyState();

            _personality = new Personality();
        }

        public NorthAmerican(DateTime dob, Gender myGender, IPerson mother, IPerson father): this(dob, myGender)
        {
            _mother = mother;
            _father = father;
            var nAmerMother = _mother as NorthAmerican;
            if (nAmerMother == null)
                return;
            ((AmericanBirthCert) _birthCert).City = nAmerMother.HomeCityArea;
        }

        #endregion

        #region properties

        public override string FirstName
        {
            get { return _fname; }
            set { _fname = value; }
        }

        public override string LastName
        {
            get { return _lname; }
            set { _lname = value; }
        }

        public override Gender MyGender
        {
            get { return _myGender; }
            set { _myGender = value; }
        }

        public override MaritialStatus GetMaritalStatus(DateTime? dt)
        {
            var mdt = dt ?? DateTime.Now;

            var iAmUnderage = GetAge(mdt) < GetMyHomeStatesAgeOfConsent();

            if (iAmUnderage || !_spouses.Any())
                return MaritialStatus.Single;

            //spread all status into single dictionary
            var myTimeline = new Dictionary<int, MaritialStatus> {{0, MaritialStatus.Single}};
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

                if (spouseData.Spouse == null || spouseData.Spouse.DeathDate == null)
                    continue;

                days = (int)Math.Abs((spouseData.Spouse.DeathDate.Value - _birthCert.DateOfBirth).TotalDays);
                while (myTimeline.ContainsKey(days))
                    days += 1;

                myTimeline.Add(days, MaritialStatus.Widowed);
            }

            var mdtDays = (int)Math.Abs((mdt - _birthCert.DateOfBirth).TotalDays);

            return myTimeline.Any(x => x.Key >= mdtDays)
                ? myTimeline.First(x => x.Key >= mdtDays).Value
                : myTimeline.Last().Value;
        }

        public override IPerson GetMother() { return _mother;}

        public override IPerson GetFather() { return _father;}

        public override SpouseData GetSpouse(DateTime? dt)
        {
            var cdt = dt ?? DateTime.Now;

            if (GetAge(cdt) < GetMyHomeStatesAgeOfConsent())
            {
                return null;
            }

            var spouseData = _spouses.FirstOrDefault(x => DateTime.Compare(x.MarriedOn, cdt) <= 0 && x.SeparatedOn == null);
            return spouseData != null ? spouseData : null;
        }

        public string MiddleName { get; set; }
        
        public List<Tuple<Address, CityArea>> FormerResidences { get { return _formerAddresses; } }

        public Address HomeAddress { get; set; }

        public CityArea HomeCityArea { get; set; }

        public string HomeCity { get { return HomeCityArea.AddressData.City; } }
        public string HomeState { get { return HomeCityArea.AddressData.StateAbbrv; } }
        public string HomeZip { get { return HomeCityArea.AddressData.PostalCode; } }

        public NorthAmericanPhone HomePhone {
            get
            {
                var hph = _phoneNumbers.FirstOrDefault(x => x.Item1 == KindsOfLabels.Home);
                return hph != null ? hph.Item2 : null;
            }
        }
        public NorthAmericanPhone CellPhone
        {
            get
            {
                var mobilePh = _phoneNumbers.FirstOrDefault(x => x.Item1 == KindsOfLabels.Mobile);
                return mobilePh != null ? mobilePh.Item2 : null;
            }
        }

        public SocialSecurityNumber Ssn { get; set; }
        public DriversLicense DriversLicense { get; set; }

        public List<ILoan> Debts { get { return _debts; } }

        public override List<IPerson> Children
        {
            get { return _children; }
        }
        public NorthAmericanRace Race { get; set; }
        
        #endregion

        #region family

        public void AlignCohabitantsHomeData(DateTime? dt)
        {
            var ms = GetMaritalStatus(dt);
            if ((ms == MaritialStatus.Married || ms == MaritialStatus.Remarried) && GetSpouse(dt) != null)
            {
                NAmerUtil.SetNAmerCohabitants((NorthAmerican)GetSpouse(dt).Spouse, this);
            }
            var underAgeChildren = Children.Cast<NorthAmerican>().Where(x => x.GetAge(dt) < UsState.AGE_OF_ADULT).ToList();
            if (underAgeChildren.Count <= 0)
                return;
            foreach(var child in underAgeChildren)
                NAmerUtil.SetNAmerCohabitants(child, this);
        }

        /// <summary>
        /// Assigns <see cref="Race"/> and invokes <see cref="ResolveParents"/>, <see cref="ResolveSpouse"/> and <see cref="ResolveChildren"/>.
        /// Only Parents and Race are certian the other resolutions contrained by age and randomness.
        /// </summary>
        protected internal void ResolveFamilyState()
        {
            ThrowOnBirthDateNull(this);
            var dt = DateTime.Now;
            Race = NAmerUtil.GetAmericanRace(HomeZip);
            ResolveParents();
            //resolve spouse to each other
            ResolveSpouse(NAmerUtil.GetMaritialStatus(_birthCert.DateOfBirth, MyGender));
            //to solve for childern when gender -eq Male
            ResolveChildren();
            AlignCohabitantsHomeData(dt);
        }

        protected internal void ResolveParents()
        {
            ThrowOnBirthDateNull(this);

            var myMother =
                (NorthAmerican)
                    (_mother ??
                     (_mother =
                         NAmerUtil.SolveForParent(_birthCert.DateOfBirth,
                             NAmerUtil.Equations.FemaleYearOfMarriage2AvgAge, Gender.Female)));
            var myFather =
                (NorthAmerican)
                    (_father ??
                     (_father =
                         NAmerUtil.SolveForParent(_birthCert.DateOfBirth, NAmerUtil.Equations.MaleYearOfMarriage2AvgAge,
                             Gender.Male)));

            //at time of birth
            myMother.LastName = LastName;
            myFather.LastName = LastName;

            BirthCert.Father = myFather;
            BirthCert.Mother = myMother;
            ((AmericanBirthCert) BirthCert).City = myMother.HomeCityArea;

            var myParentsAtMyBirth = NAmerUtil.GetMaritialStatus(myMother.BirthCert.DateOfBirth, Gender.Female);

            //mother not ever married to father
            if (myParentsAtMyBirth == MaritialStatus.Single)
                myFather.LastName = NAmerUtil.GetAmericanLastName();

            //mother no longer married to father
            if (myParentsAtMyBirth == MaritialStatus.Divorced ||
                myParentsAtMyBirth == MaritialStatus.Remarried ||
                myParentsAtMyBirth == MaritialStatus.Separated)
            {

                myMother.LastName = NAmerUtil.GetAmericanLastName();
                myMother.OtherNames.Add(
                    new Tuple<KindsOfPersonalNames, string>(
                        KindsOfPersonalNames.Surname | KindsOfPersonalNames.Former | KindsOfPersonalNames.Spouse,
                        myFather.LastName));
            }

            //hea
            if (myParentsAtMyBirth == MaritialStatus.Married)
            {
                NAmerUtil.SetNAmerCohabitants(myMother, myFather);
                var marriedOn = NAmerUtil.SolveForMarriageDate(myFather.BirthCert.DateOfBirth, Gender.Male);
                if (marriedOn != null)
                {
                    myMother.AddNewSpouseToList(myFather, marriedOn.Value);
                    myFather.AddNewSpouseToList(myMother, marriedOn.Value);
                }
            }

            myMother.OtherNames.Add(new Tuple<KindsOfPersonalNames, string>(KindsOfPersonalNames.Father,
                NAmerUtil.GetAmericanLastName()));

            _father = myFather;
            _mother = myMother;
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

            //add maiden name, set new last name to match husband
            if (_myGender == Gender.Female)
            {
                if (OtherNames.All(x => x.Item1 != KindsOfPersonalNames.Father))
                    OtherNames.Add(new Tuple<KindsOfPersonalNames, string>(KindsOfPersonalNames.Father, _father.LastName));
            }

            //set death date if widowed
            if (myMaritialStatus == MaritialStatus.Widowed)
            {
                var d = Convert.ToInt32(Math.Round(GetAge(null) * 0.15));
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
                if (myMaritialStatus != MaritialStatus.Remarried) return;

                var ageSpread = 6;
                if (MyGender == Gender.Male)
                    ageSpread = 10;//he likes'em young...

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
        /// added to the <see cref="Children"/> collection.  
        /// </summary>
        protected internal void ResolveChildren()
        {
            //equations data is by women only.
            if (MyGender == Gender.Male)
            {
                foreach (var s in _spouses.Where(x => x.Spouse != null && x.Spouse.MyGender == Gender.Female))
                {
                    var nAmerSpouse = s.Spouse as NorthAmerican;
                    if (nAmerSpouse == null)
                        continue;
                    nAmerSpouse.ResolveChildren();

                    foreach (var childFrom in nAmerSpouse.Children)
                    {
                        if (childFrom.LastName != LastName && childFrom.OtherNames.All(x => x.Item2 != LastName))
                            continue;
                        Children.Add(childFrom);
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
        /// Add a random <see cref="IPerson"/> to the <see cref="Children"/> collection
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
                                       myChildeAge && myChildeAge >= GetMyHomeStatesAgeOfConsent();
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

                    myChild = new NorthAmerican(myChildDob, myChildGender, this, marriage.Spouse) {Race = Race};
                    if (isDaughterMarriedOff)
                        myChild.OtherNames.Add(new Tuple<KindsOfPersonalNames, string>(KindsOfPersonalNames.Father,
                            marriage.Spouse.LastName));
                    else
                        myChild.LastName = marriage.Spouse.LastName;
                }
            }

            //for child born in any range outside of marriage, assign lastname to maiden name
            if (myChild == null)
            {
                var maidenName = OtherNames.FirstOrDefault(x => x.Item1 == KindsOfPersonalNames.Father);
                if (maidenName != null && !string.IsNullOrWhiteSpace(maidenName.Item2))
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
                    string.Format(
                        "The Child Date-of-Birth, {0}, does not fall " +
                        "within a rational range given the mother's Date-of-Birth of {1}",
                        childDob, BirthCert.DateOfBirth));
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
                if (_myGender == Gender.Female)
                    LastName = spouse.LastName;
            }
            else if (MyGender == Gender.Female)
            {
                //add ex-husband last name to list
                OtherNames.Add(
                    new Tuple<KindsOfPersonalNames, string>(
                        KindsOfPersonalNames.Former | KindsOfPersonalNames.Surname | KindsOfPersonalNames.Spouse,
                        spouse.LastName));

                //set back to maiden name
                LastName = _father == null ? NAmerUtil.GetAmericanLastName() : _father.LastName;
            }
            _spouses.Add(new SpouseData
            {
                Spouse = spouse,
                MarriedOn = marriedOn,
                SeparatedOn = separatedOn,
                Ordinal = _spouses.Count + 1
            });
            var nAmerSpouse = spouse as NorthAmerican;
            if (nAmerSpouse == null)
                return;

            nAmerSpouse.AddNewSpouseToList(this, marriedOn, separatedOn);
        }

        private int GetMyHomeStatesAgeOfConsent()
        {
            if (HomeCityArea == null)
                return UsState.AGE_OF_ADULT;
            var myHomeState = UsState.GetStateByPostalCode(HomeState);
            return myHomeState == null ? UsState.AGE_OF_ADULT : myHomeState.AgeOfConsent;
        }

        #endregion

        protected internal void ResolveFinancialState()
        {

            throw new NotImplementedException();
        }
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