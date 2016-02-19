using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NoFuture.Exceptions;
using NoFuture.Rand.Domus.Pneuma;
using NoFuture.Rand.Domus.Sp;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Domus
{
    [Serializable]
    public class NorthAmerican : Person
    {
        #region fields
        private readonly List<Tuple<Address, CityArea>> _formerAddresses = new List<Tuple<Address, CityArea>>();
        private readonly List<IAsset> _assets = new List<IAsset>();
        private readonly List<ILoan> _debts = new List<ILoan>();
        private IPerson _mother;
        private IPerson _father;
        private IPerson _spouse;
        private readonly List<SpouseData> _spouses = new List<SpouseData>();
        private DateTime? _dob;
        private Gender _myGender;
        private string _fname;
        private string _lname;
        private MaritialStatus _ms;
        internal readonly List<Tuple<KindsOfLabels, NorthAmericanPhone>> _phoneNumbers = new List<Tuple<KindsOfLabels, NorthAmericanPhone>>();
        #endregion

        #region ctors

        public NorthAmerican(DateTime? dob, Gender myGender, bool withWholeFamily = false)
        {
            if(dob == null)
                throw new ArgumentNullException("dob");
            _dob = dob;
            _myGender = myGender;

            _fname = _myGender != Gender.Unknown ? NAmerUtil.GetAmericanFirstName(_dob, _myGender) : "Pat";
            _ms = _myGender != Gender.Unknown ? NAmerUtil.GetMaritialStatus(_dob.Value, _myGender) : MaritialStatus.Unknown;
            _lname = NAmerUtil.GetAmericanLastName();

            MiddleName = NAmerUtil.GetAmericanFirstName(_dob, _myGender);

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
            if(GetAge(null) >= 16)
                DriversLicense = csz.State.Formats[0];

            //http://www.internic.net/zones/root.zone

            if(withWholeFamily)
                ResolveFamilyState();

            _personality = new Personality(Etx.IntNumber(0, 10000) <= 16);
        }

        #endregion

        #region properties

        public override DateTime? BirthDate
        {
            get { return _dob; }

            set { _dob = value; }
        }

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

        public override MaritialStatus MaritalStatus
        {
            get { return _ms; }
            set { _ms = value; }
        }

        public override IPerson Mother
        {
            get { return _mother; }
            set { _mother = value; }
        }

        public override IPerson Father
        {
            get { return _father; }
            set { _father = value; }
        }

        public override IPerson Spouse
        {
            get { return _spouse; }
            set { _spouse = value; }
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

        public List<IAsset> Assets { get { return _assets; }}
        public List<ILoan> Debts { get { return _debts; } }
        public IIncome Income { get; set; }

        public override List<IPerson> Children
        {
            get { return _children; }
        }
        public NorthAmericanRace Race { get; set; }
        
        #endregion

        #region family

        public void AlignCohabitantsHomeData()
        {
            if ((MaritalStatus == MaritialStatus.Married || MaritalStatus == MaritialStatus.Remarried) && Spouse != null)
            {
                NAmerUtil.SetNAmerCohabitants((NorthAmerican) Spouse, this);
            }
            var underAgeChildren = Children.Cast<NorthAmerican>().Where(x => x.GetAge(null) < 18).ToList();
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
            Race = NAmerUtil.GetAmericanRace(HomeZip);
            ResolveParents();
            ResolveSpouse();
            ResolveChildren();
        }

        protected internal void ResolveParents()
        {
            ThrowOnBirthDateNull();

            var myMother = (NorthAmerican)(_mother ?? (_mother = NAmerUtil.SolveForParent(_dob.Value, NAmerUtil.Equations.FemaleYearOfMarriage2AvgAge, Gender.Female)));
            var myFather = (NorthAmerican)(_father ?? (_father = NAmerUtil.SolveForParent(_dob.Value, NAmerUtil.Equations.MaleYearOfMarriage2AvgAge, Gender.Male)));

            //at time of birth
            myMother.LastName = LastName;
            myFather.LastName = LastName;

            myMother.MaritalStatus = NAmerUtil.GetMaritialStatus(myMother.BirthDate.Value, Gender.Female);
            myFather.MaritalStatus = _mother.MaritalStatus;

            //mother not ever married to father
            if (myMother.MaritalStatus == MaritialStatus.Single)
                myFather.LastName = NAmerUtil.GetAmericanLastName();

            //mother no longer married to father
            if (myMother.MaritalStatus == MaritialStatus.Divorced ||
                myMother.MaritalStatus == MaritialStatus.Remarried ||
                myMother.MaritalStatus == MaritialStatus.Separated)
            {

                myMother.LastName = NAmerUtil.GetAmericanLastName();
                myMother.OtherNames.Add(
                    new Tuple<KindsOfPersonalNames, string>(
                        KindsOfPersonalNames.Surname | KindsOfPersonalNames.Former | KindsOfPersonalNames.Spouse,
                        myFather.LastName));
            }

            //hea
            if (myMother.MaritalStatus == MaritialStatus.Married)
            {
                myMother.Spouse = myFather;
                myFather.Spouse = myMother;
                NAmerUtil.SetNAmerCohabitants(myMother, myFather);
            }

            myMother.OtherNames.Add(new Tuple<KindsOfPersonalNames, string>(KindsOfPersonalNames.Father,
                NAmerUtil.GetAmericanLastName()));

            _father = myFather;
            _mother = myMother;
        }

        protected internal void ResolveSpouse()
        {
            if (MaritalStatus == MaritialStatus.Single || MaritalStatus == MaritialStatus.Unknown)
                return;

            var dt = DateTime.Now;

            ThrowOnBirthDateNull();

            var avgAgeMarriage = MyGender == Gender.Female
                ? NAmerUtil.Equations.FemaleDob2MarriageAge.SolveForY(_dob.Value.ToDouble())
                : NAmerUtil.Equations.MaleDob2MarriageAge.SolveForY(_dob.Value.ToDouble());
            var currentAge = Person.CalcAge(_dob.Value, dt);

            //all other MaritialStatus imply at least one marriage in past
            var yearsMarried = currentAge - Convert.ToInt32(Math.Round(avgAgeMarriage));

            var marriedOn = Etx.Date(-1*yearsMarried, dt);

            var spouse = (NorthAmerican)NAmerUtil.SolveForSpouse(_dob.Value, MyGender);

            //add maiden name, set new last name to match husband
            if (_myGender == Gender.Female)
            {
                LastName = spouse.LastName;
                if (OtherNames.All(x => x.Item1 != KindsOfPersonalNames.Father))
                    OtherNames.Add(new Tuple<KindsOfPersonalNames, string>(KindsOfPersonalNames.Father, _father.LastName));
            }

            //set death date if widowed
            if (MaritalStatus == MaritialStatus.Widowed)
            {
                var d = Convert.ToInt32(Math.Round(GetAge(null) * 0.15));
                spouse.DeathDate = Etx.Date(Etx.IntNumber(1, d), null);
            }


            //assign reciprocial
            spouse.Spouse = this;
            _spouse = spouse;

            NAmerUtil.SetNAmerCohabitants(spouse, this);

            if (MaritalStatus != MaritialStatus.Divorced && MaritalStatus != MaritialStatus.Remarried &&
                MaritalStatus != MaritialStatus.Separated)
            {
                //add internal date-range for resolution of children
                _spouses.Add(new SpouseData{Spouse = _spouse, MarriedOn = marriedOn, Ordinal = 0});
            }
            else
            {
                //take date of marriage and add avg length of marriage
                var separatedDate = Etx.Date(NAmerUtil.AvgLengthOfMarriage, marriedOn);

                //reset date-range with separated date
                _spouses.Clear();
                _spouses.Add(new SpouseData { Spouse = _spouse, MarriedOn = marriedOn, SeparatedOn = separatedDate, Ordinal = 0 });

                //add ex-husband last name to list
                if (MyGender == Gender.Female)
                {
                    OtherNames.Add(
                        new Tuple<KindsOfPersonalNames, string>(
                            KindsOfPersonalNames.Former | KindsOfPersonalNames.Surname | KindsOfPersonalNames.Spouse,
                            _spouse.LastName));

                    //set back to maiden name
                    LastName = _father.LastName;
                }

                //separate and move out
                _spouse.Spouse = null;
                _spouse.MaritalStatus = MaritalStatus;
                _spouse = null;
                HomeCityArea = CityArea.American(HomeCityArea.GetPostalCodePrefix());
                HomeAddress = Address.American();

                //leave when no second spouse applicable
                if (MaritalStatus != MaritialStatus.Remarried) return;

                var ageSpread = 6;
                if (MyGender == Gender.Male)
                    ageSpread = 10;//he likes'em young...

                //get a second spouse
                var secondSpouse = (NorthAmerican)NAmerUtil.SolveForSpouse(_dob.Value, MyGender, ageSpread);

                //random second marriage date
                var remarriedOn = Etx.Date(Convert.ToInt32(Math.Round(NAmerUtil.YearsBeforeNextMarriage)),
                    separatedDate);

                //add second date-range for resolution of children
                _spouses.Add(new SpouseData {Spouse = secondSpouse, MarriedOn = remarriedOn, Ordinal = 1});

                //change female last name to match second husband
                if (_myGender == Gender.Female)
                    LastName = secondSpouse.LastName;

                //assign these reciprocial
                _spouse = secondSpouse;
                secondSpouse.Spouse = _spouse;
                NAmerUtil.SetNAmerCohabitants(secondSpouse, this);
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
                return;

            ThrowOnBirthDateNull();

            var currentNumChildren = 0;

            //two extremes
            var propTeenagePreg = Math.Round(GetProbabilityTeenagePreg() * 1000);
            var propLifetimeChildless = 1000 - Math.Round(GetProbabilityLifetimeChildless()*1000);

            //random value within range of two extremes
            var randItoM = Etx.IntNumber(1, 1000);

            //far high-end is no children for whole life
            if (randItoM >= propLifetimeChildless)
                return;

            //other extreme is teenage preg
            if (randItoM <= propTeenagePreg)
            {
                var teenPregChildDob = Etx.Date(Etx.IntNumber(15, 19), _dob);
                AddNewChildToList(teenPregChildDob);
                currentNumChildren += 1;
            }
            
            //last is averages
            var numOfChildren = NAmerUtil.SolveForNumberOfChildren(_dob.Value, null);

            if (numOfChildren <= 0)
                return;

            for (var i = currentNumChildren; i < numOfChildren; i++)
            {
                var childDob = NAmerUtil.GetChildBirthDate(_dob.Value, i, null);
                if (childDob == null)
                    continue;


                AddNewChildToList(childDob);
            }
        }

        /// <summary>
        /// Add a random <see cref="IPerson"/> to the <see cref="Children"/> collection
        /// aligning <see cref="IPerson"/> last name and father to match
        /// spouses.
        /// </summary>
        /// <param name="childDob">
        /// This will be adusted up by when the Birth Date would occur during the pregnancy 
        /// of a sibling unless it is the exact same date (twins).
        /// </param>
        protected internal void AddNewChildToList(DateTime? childDob)
        {
            if (childDob == null || MyGender == Gender.Male)
                return;

            //child dob must not be during the 280 day of pregnancy of next sibling unless twins
            childDob = AdjustBirthDateWhenDuringAnotherPregnancy(childDob);

            var child = new NorthAmerican(childDob,
                Etx.CoinToss ? Gender.Female : Gender.Male) { Mother = this };

            var childAge = child.GetAge(null);

            //set underage child living with mother
            if (child.GetAge(null) < 18)
            {
                NAmerUtil.SetNAmerCohabitants(child, this);
                child.MaritalStatus = MaritialStatus.Single;
                child.Spouse = null;
            }


            var isDaughterWithSpouse = child.MyGender == Gender.Female &&
                                       NAmerUtil.Equations.FemaleDob2MarriageAge.SolveForY(childDob.Value.Year) >=
                                       childAge && childAge >= 18;
            //never married
            if (!_spouses.Any())
            {
                //default to mother last name
                child.LastName = LastName;
                _children.Add(child);
                return;
            }

            //assign child father and lastname when born within range of marriage
            var assignedLname = false;
            for (var i = 0; i <= _spouses.Max(x => x.Ordinal); i++)
            {
                var marriage = _spouses.FirstOrDefault(x => x.Ordinal == i);
                if (marriage == null)
                    continue;

                if (DateTime.Compare(childDob.Value, marriage.MarriedOn) >= 0 &&
                    (marriage.SeparatedOn == null ||
                     DateTime.Compare(childDob.Value, marriage.SeparatedOn.Value) < 0))
                {
                    if (!isDaughterWithSpouse)
                        child.LastName = marriage.Spouse.LastName;

                    assignedLname = true;
                    child.Father = marriage.Spouse;
                }
            }

            //for child born in any range outside of marriage, assign lastname to maiden name
            if (!assignedLname)
            {
                var maidenName = OtherNames.FirstOrDefault(x => x.Item1 == KindsOfPersonalNames.Father);
                if (maidenName != null && !string.IsNullOrWhiteSpace(maidenName.Item2))
                {
                    child.LastName = maidenName.Item2;
                }
                else
                {
                    //default to mother last name
                    child.LastName = LastName;
                }
            }
            _children.Add(child);
        }

        protected internal double GetProbabilityLifetimeChildless()
        {
            ThrowOnBirthDateNull();

            return NAmerUtil.SolveForProbabilityChildless(_dob.Value,
                Education == null ? OccidentalEdu.Empty : Education.GetEduLevel(null));
        }

        protected internal double GetProbabilityTeenagePreg()
        {
            ThrowOnBirthDateNull();

            var teenPregEquation = NAmerUtil.Equations.GetProbTeenPregnancyByRace(Race);

            var teenageYear = _dob.Value.AddYears(Etx.IntNumber(15, 19)).Year;

            return teenPregEquation.SolveForY(teenageYear);
        }

        protected internal DateTime? AdjustBirthDateWhenDuringAnotherPregnancy(DateTime? childDob)
        {
            if (childDob == null)
                return null;

            //dob must not, unless a twin, be during the pregnancy of a sibling.
            var invalidRange = _children.FirstOrDefault(
                x =>
                    x.BirthDate.HasValue && DateTime.Compare(x.BirthDate.Value.Date, childDob.Value.Date) != 0 &&
                    (x.BirthDate.Value.AddDays(-280) - childDob.Value).Days < 280);

            if (invalidRange == null)
                return childDob;

            return invalidRange.BirthDate.Value.AddDays(Etx.IntNumber(5, 30));
        }

        private void ThrowOnBirthDateNull()
        {
            if (_dob == null)
                throw
                    new RahRowRagee(
                        String.Format("The random person named {0}, {1} does not have a Date Of Birth assigned.",
                            LastName, FirstName));
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

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class SpouseData
    {
        internal IPerson Spouse;
        internal DateTime MarriedOn;
        internal DateTime? SeparatedOn;
        internal int Ordinal;
    }

}