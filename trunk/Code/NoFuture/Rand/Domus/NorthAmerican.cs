using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NoFuture.Exceptions;
using NoFuture.Rand.Domus.Pneuma;
using NoFuture.Rand.Domus.Sp;
using NoFuture.Rand.Gov;
using NoFuture.Util.Math;
using System.Xml;
using NoFuture.Rand.Data;
using NoFuture.Shared;

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
        private readonly List<Tuple<KindsOfLabels, NorthAmericanPhone>> _phoneNumbers = new List<Tuple<KindsOfLabels, NorthAmericanPhone>>();
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
            _phoneNumbers.Add(new Tuple<KindsOfLabels, NorthAmericanPhone>(KindsOfLabels.Home, Phone.American(abbrv)));
            _phoneNumbers.Add(new Tuple<KindsOfLabels, NorthAmericanPhone>(KindsOfLabels.Mobile, Phone.American(abbrv)));

            Ssn = new SocialSecurityNumber();
            if(GetAge(null) >= 16)
                DriversLicense = csz.State.Formats[0];

            //http://www.internic.net/zones/root.zone

            if(withWholeFamily)
                ResolveFamilyState();

            _personality = new Personality(Etx.Number(0, 10000) <= 16);
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
        public Income Income { get; set; }

        public override List<IPerson> Children
        {
            get { return _children; }
        }
        public NorthAmericanRace Race { get; set; }
        
        #endregion

        #region family
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
                myMother.HomeCityArea = myFather.HomeCityArea;
                myMother.HomeAddress = myFather.HomeAddress;
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
                spouse.DeathDate = Etx.Date(Etx.Number(1, d), null);
            }


            //assign reciprocial
            spouse.Spouse = this;
            _spouse = spouse;

            spouse.HomeCityArea = HomeCityArea;
            spouse.HomeAddress = HomeAddress;

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

                //detach these reciprocial
                _spouse.Spouse = null;
                _spouse.MaritalStatus = MaritalStatus;
                _spouse = null;
                HomeCityArea = CityArea.American(HomeCityArea.GetPostalCodePrefix());
                HomeAddress = Address.American();

                //leave when no second spouse applicable
                if (MaritalStatus != MaritialStatus.Remarried) return;

                var ageSpread = 6;
                if (MyGender == Gender.Male)
                    ageSpread = 10;

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
                secondSpouse.HomeCityArea = HomeCityArea;
                secondSpouse.HomeAddress = HomeAddress;

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
            var randItoM = Etx.Number(1, 1000);

            //far high-end is no children for whole life
            if (randItoM >= propLifetimeChildless)
                return;

            //other extreme is teenage preg
            if (randItoM <= propTeenagePreg)
            {
                var teenPregChildDob = Etx.Date(Etx.Number(15, 19), _dob);
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
                child.HomeAddress = HomeAddress;
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

            var teenageYear = _dob.Value.AddYears(Etx.Number(15, 19)).Year;

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

            return invalidRange.BirthDate.Value.AddDays(Etx.Number(5, 30));
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

    /// <summary>
    /// Catalog of static utility methods related to North American
    /// </summary>
    public static class NAmerUtil
    {
        #region constants
        /// <summary>
        /// Has no stat validity - just a guess
        /// </summary>
        public const double PercentDivorced = 0.44;
        /// <summary>
        /// Has no stat validity - just a guess
        /// </summary>
        public const int AvgLengthOfMarriage = 10;
        /// <summary>
        /// Has no stat validity - just a guess
        /// </summary>
        public const double YearsBeforeNextMarriage = 3.857;
        /// <summary>
        /// Has no stat validity - just a guess
        /// </summary>
        public const double PercentUnmarriedWholeLife = 0.054;
        /// <summary>
        /// Has no stat validity - just a guess
        /// </summary>
        public const double EquationStdDev = 1.0D;
        #endregion

        /// <summary>
        /// Marriage Source [https://www.census.gov/population/socdemo/hh-fam/ms2.xls] (1947-2011)
        /// Age of Birth Sources (1970-2014)
        /// [http://www.cdc.gov/nchs/data/nvsr/nvsr51/nvsr51_01.pdf] (Table 1.) 
        /// [http://www.cdc.gov/nchs/data/nvsr/nvsr64/nvsr64_12_tables.pdf] (Table I-1.)
        /// </summary>
        /// <remarks>
        /// FemaleAge2*Child have had thier intercepts up'ed by 4.  The real data produces 
        /// a condition where first born's are always before marriage.
        /// </remarks>
        public static class Equations
        {
            public static RLinearEquation MaleDob2MarriageAge = new RLinearEquation
            {
                Intercept = -181.45,
                Slope = 0.1056,
                StdDev = EquationStdDev 
            };

            public static RLinearEquation FemaleDob2MarriageAge = new RLinearEquation
            {
                Intercept = -209.41,
                Slope = 0.1187,
                StdDev = EquationStdDev 
            };

            public static RLinearEquation MaleYearOfMarriage2AvgAge = new RLinearEquation
            {
                Intercept = -166.24,
                Slope = 0.0965,
                StdDev = EquationStdDev 
            };

            public static RLinearEquation FemaleYearOfMarriage2AvgAge = new RLinearEquation
            {
                Intercept = -191.74,
                Slope = 0.1083,
                StdDev = EquationStdDev 
            };

            public static RLinearEquation FemaleAge2FirstChild = new RLinearEquation
            {
                Intercept = -176.32,//-180.32
                Slope = 0.1026,
                StdDev = EquationStdDev 
            };

            public static RLinearEquation FemaleAge2SecondChild = new RLinearEquation
            {
                Intercept = -171.88,//-175.88
                Slope = 0.1017,
                StdDev = EquationStdDev
            };

            public static RLinearEquation FemaleAge2ThirdChild = new RLinearEquation
            {
                Intercept = -125.45,//-129.45
                Slope = 0.0792,
                StdDev = EquationStdDev
            };

            public static RLinearEquation FemaleAge2ForthChild = new RLinearEquation
            {
                Intercept = -74.855,//-78.855
                Slope = 0.0545,
                StdDev = EquationStdDev
            };

            /// <summary>
            /// https://en.wikipedia.org/wiki/Childfree#Statistics_and_research
            /// </summary>
            public static NaturalLogEquation FemaleYob2ProbChildless = new NaturalLogEquation
            {
                Intercept = -55.479,
                Slope = 7.336
            };

            /// <summary>
            /// [http://www.hhs.gov/ash/oah/adolescent-health-topics/reproductive-health/teen-pregnancy/trends.html]
            /// </summary>
            /// <param name="race"></param>
            /// <returns></returns>
            public static RLinearEquation GetProbTeenPregnancyByRace(NorthAmericanRace race)
            {
                switch (race)
                {
                    case NorthAmericanRace.Black:
                        return new RLinearEquation
                        {
                            Intercept = 6.8045,
                            Slope = -0.0034,
                        };
                    case NorthAmericanRace.Hispanic:
                        return new RLinearEquation
                        {
                            Intercept = 5.1231,
                            Slope = -0.0025,
                        };
                }
                return new RLinearEquation
                {
                    Intercept = 2.1241,
                    Slope = -0.001,
                };
            }
        }

        /// <summary>
        /// Returns one entry at random selecting by <see cref="Person.MyGender"/> and <see cref="Person"/>
        /// using one of top 200 names for the given decade in which the DOB is a part.
        /// </summary>
        /// <returns></returns>
        public static string GetAmericanFirstName(DateTime? dateOfBirth, Gender gender)
        {
            var dt = dateOfBirth ?? new DateTime(2000, 1, 2);
            var xmlData = TreeData.AmericanFirstNamesData;
            XmlElement decadeNode = null;
            foreach (var decade in xmlData.SelectNodes("//first-name").Cast<XmlElement>())
            {
                var start = DateTime.Parse(decade.Attributes["decade-start"].Value);
                var end = DateTime.Parse(decade.Attributes["decade-end"].Value);

                var cStart = DateTime.Compare(dt, start);//-ge 
                var cEnd = DateTime.Compare(dt, end);//-le
                if (cStart < 0 || cEnd > 0) continue;

                decadeNode = decade;
                break;
            }
            if (decadeNode == null)
                return "Jane";
            var genderIdx = gender != Gender.Female ? 0 : 1;
            var mfFnames = decadeNode.ChildNodes[genderIdx];

            var p = Etx.MyRand.Next(0, mfFnames.ChildNodes.Count);
            return mfFnames.ChildNodes[p].InnerText;
        }

        /// <summary>
        /// Return one of 500 selected lastnames - source is unknown.
        /// </summary>
        /// <returns></returns>
        public static string GetAmericanLastName()
        {
            var xmlData = TreeData.AmericanLastNamesData;
            var lnameNodes = xmlData.SelectSingleNode("//last-name");
            var p = Etx.MyRand.Next(0, lnameNodes.ChildNodes.Count);
            return lnameNodes.ChildNodes[p].InnerText;
        }

        /// <summary>
        /// Returns a date being between 18 years ago today back to 68 years ago today.
        /// </summary>
        public static DateTime GetWorkingAdultBirthDate()
        {
            return DateTime.Now.AddYears(-1 * Etx.MyRand.Next(18, 68)).AddDays(Etx.Number(1, 360));
        }

        /// <summary>
        /// Generates a random past date based on the <see cref="numberOfSiblings"/> and the Mother's Date of Birth.
        /// </summary>
        /// <returns></returns>
        public static DateTime? GetChildBirthDate(DateTime motherDob, int numberOfSiblings, DateTime? atTime)
        {
            //put number of siblings in scope for equations
            if (numberOfSiblings <= 0)
                numberOfSiblings = 0;
            if (numberOfSiblings > 4)
                numberOfSiblings = 3;

            //default atTime to now
            var dt = DateTime.Now;
            if (atTime != null)
                dt = atTime.Value;

            var motherAge = Person.CalcAge(motherDob, dt);

            var meanAge = Equations.FemaleAge2FirstChild.SolveForY(motherDob.Year);

            //mother is too young for children
            if (motherAge < meanAge)
                return null;

            switch (numberOfSiblings)
            {
                case 2:
                    meanAge = Equations.FemaleAge2SecondChild.SolveForY(motherDob.Year);
                    if (motherAge < meanAge)
                        return null;
                    break;
                case 3:
                    meanAge = Equations.FemaleAge2ThirdChild.SolveForY(motherDob.Year);
                    if (motherAge < meanAge)
                        return null;
                    break;
                case 4:
                    meanAge = Equations.FemaleAge2ForthChild.SolveForY(motherDob.Year);
                    if (motherAge < meanAge)
                        return null;
                    break;
            }

            return Etx.Date((int) Math.Round(meanAge), motherDob);

        }

        /// <summary>
        /// Return a <see cref="NorthAmericanRace"/> randomly with weight based on <see cref="zipCode"/>.
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns>Will default to string <see cref="NorthAmericanRace.White"/> if <see cref="zipCode"/> cannot be resolved.</returns>
        public static NorthAmericanRace GetAmericanRace(string zipCode)
        {
            var amRace = Etx.RandomAmericanRaceWithRespectToZip(zipCode);

            if (amRace == null)
                return NorthAmericanRace.White;

            var americanRaceProbabilityRanges = new List<AmericanRaceProbabilityRange>();
            double prevFrom = 0.000001D;
            var raceHashByZip = new Dictionary<NorthAmericanRace, double>
            {
                {NorthAmericanRace.AmericanIndian, amRace.AmericanIndian},
                {NorthAmericanRace.Asian, amRace.Asian},
                {NorthAmericanRace.Black, amRace.Black},
                {NorthAmericanRace.Hispanic, amRace.Hispanic},
                {NorthAmericanRace.Mixed, amRace.Mixed},
                {NorthAmericanRace.Pacific, amRace.Pacific},
                {NorthAmericanRace.White, amRace.White}
            };

            foreach (var key in raceHashByZip.Keys)
            {
                var val = raceHashByZip[key];
                var f = prevFrom;
                var t = (val + prevFrom);
                americanRaceProbabilityRanges.Add(new AmericanRaceProbabilityRange
                {
                    Name = key,
                    From = f,
                    To = t
                });
                prevFrom = (val + prevFrom + 0.000001D);
            }

            //pick one at random with probability
            var pick = (double)(Etx.MyRand.Next(1, 99999999)) / 1000000;

            var race = NorthAmericanRace.White;

            var randomRace = americanRaceProbabilityRanges.FirstOrDefault(arpr => arpr.From <= pick && arpr.To >= pick);
            if (randomRace != null)
                race = randomRace.Name;

            return race;
        }

        /// <summary>
        /// Returns the <see cref="MaritialStatus"/> based on the gender and age.
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        public static MaritialStatus GetMaritialStatus(DateTime dob, Gender gender)
        {
            if (Etx.Number(1, 1000) <= PercentUnmarriedWholeLife * 1000)
                return MaritialStatus.Single;

            var avgAgeMarriage = gender == Gender.Female
                ? Equations.FemaleDob2MarriageAge.SolveForY(dob.ToDouble())
                : Equations.MaleDob2MarriageAge.SolveForY(dob.ToDouble());
            var cdt = DateTime.Now;
            var currentAge = Person.CalcAge(dob, cdt);

            if (currentAge < avgAgeMarriage)
                return MaritialStatus.Single;

            if (currentAge > avgAgeMarriage + AvgLengthOfMarriage)
            {
                //spin for divorce
                var df = Etx.Number(1, 1000) <= PercentDivorced * 1000;

                if (df && currentAge < avgAgeMarriage + AvgLengthOfMarriage + YearsBeforeNextMarriage)
                    return Etx.Number(1, 1000) <= 64 ? MaritialStatus.Separated : MaritialStatus.Divorced;

                if(df)
                    return MaritialStatus.Remarried;
            }

            //in the mix with very low probability
            if (Etx.Number(1, 1000) <= 10)
                return MaritialStatus.Widowed;

            return MaritialStatus.Married;
        }

        /// <summary>
        /// Returns a new <see cref="IPerson"/> representing a 
        /// parent of the specified <see cref="gender"/> having a realistic age 
        /// from the <see cref="childDob"/>.
        /// </summary>
        /// <param name="childDob"></param>
        /// <param name="eq"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        public static IPerson SolveForParent(DateTime childDob, LinearEquation eq, Gender gender)
        {
            //assume mother & father married between 1 - 6 years prior the Person's dob
            var dtPm = childDob.AddYears(-1 * Etx.Number(1, 6)).AddDays(Etx.Number(1, 360));

            var ageWhenParentsMarried =
                eq.SolveForY(dtPm.ToDouble());

            var aParent =
                new NorthAmerican(
                    dtPm.AddYears(Convert.ToInt32(Math.Round(ageWhenParentsMarried, 0)) * -1),
                    gender);
            return aParent;
        }

        /// <summary>
        /// Returns a new <see cref="IPerson"/> having the opposite gender
        /// of <see cref="gender"/> with a similar <see cref="IPerson.BirthDate"/>
        /// </summary>
        /// <param name="myDob">
        /// Only used to generate a Birth Date around the <see cref="maxAgeDiff"/>
        /// </param>
        /// <param name="gender"></param>
        /// <param name="maxAgeDiff">Optional difference in age of spouse.</param>
        /// <returns>return null for <see cref="Rand.Gender.Unknown"/></returns>
        public static IPerson SolveForSpouse(DateTime myDob, Gender gender, int maxAgeDiff = 4)
        {
            if (gender == Gender.Unknown)
                return null;

            var ageDiff = Etx.Number(0, maxAgeDiff);
            ageDiff = gender == Gender.Female ? ageDiff * -1 : ageDiff;

            //randomize dob of spouse
            var spouseDob = myDob.AddYears(ageDiff).AddDays(Etx.Number(1, 360) * Etx.PlusOrMinusOne);

            //define spouse
            return new NorthAmerican(spouseDob, gender == Gender.Female ? Gender.Male : Gender.Female);

        }

        /// <summary>
        /// Produces a random value between 0 and 4.
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="atDateTime">Optional, will default to current system time to calc Age</param>
        /// <returns></returns>
        public static int SolveForNumberOfChildren(DateTime dob, DateTime? atDateTime)
        {
            //average to be 2.5 
            var vt = DateTime.Now;
            if (atDateTime != null)
                vt = atDateTime.Value;

            var age = Person.CalcAge(dob, vt);
            var randV = Etx.Number(1, 100);

            var meanAge = Math.Round(Equations.FemaleAge2ForthChild.SolveForY(vt.Year));

            if (randV >= 84 && age >= meanAge)
                return 4;

            meanAge = Math.Round(Equations.FemaleAge2ThirdChild.SolveForY(vt.Year));

            if (randV >= 66 && age >= meanAge)
                return 3;

            meanAge = Math.Round(Equations.FemaleAge2SecondChild.SolveForY(vt.Year));

            if (randV >= 33 && age >= meanAge)
                return 2;

            meanAge = Math.Round(Equations.FemaleAge2FirstChild.SolveForY(vt.Year));

            return age >= meanAge ? 1 : 0;
        }

        /// <summary>
        /// Produces the probability of childlessness
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="educationLevel">
        /// https://en.wikipedia.org/wiki/Childfree#Education
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// Data regarded women-only and did not incorporate <see cref="NorthAmericanRace"/>
        /// </remarks>
        public static double SolveForProbabilityChildless(DateTime dob, OccidentalEdu educationLevel)
        {
            var eduAdditive = 0.0;
            if (educationLevel == (OccidentalEdu.College | OccidentalEdu.Grad))
                eduAdditive = 0.09;
            if (educationLevel == (OccidentalEdu.College | OccidentalEdu.Some))
                eduAdditive = 0.04;
            if (educationLevel == (OccidentalEdu.HighSchool | OccidentalEdu.Grad))
                eduAdditive = 0.02;

            var probChildless = Math.Round(Equations.FemaleYob2ProbChildless.SolveForY(dob.Year), 2);

            probChildless += eduAdditive;

            return probChildless;
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