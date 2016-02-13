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
        private readonly List<Tuple<KindsOfPersonalNames, string>> _otherNames = new List<Tuple<KindsOfPersonalNames, string>>();
        private readonly List<Tuple<Address, CityArea>> _formerAddresses = new List<Tuple<Address, CityArea>>();
        private readonly List<IAsset> _assets = new List<IAsset>();
        private readonly List<ILoan> _debts = new List<ILoan>();
        private IPerson _mother;
        private IPerson _father;
        private IPerson _spouse;
        private DateTime? _dob;
        private Gender _myGender;
        private string _fname;
        private string _lname;
        private MaritialStatus _ms;
        private readonly List<IPerson> _children = new List<IPerson>();
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

            var csz = CityArea.American();
            HomeAddress = Address.American();
            HomeCityArea = csz;
            WorkAddress = Address.American();
            WorkCityArea = csz;

            var abbrv = csz.State.StateAbbrv;
            WorkPhone = Phone.American(abbrv);
            HomePhone = Phone.American(abbrv);
            CellPhone = Phone.American(abbrv);

            Ssn = new SocialSecurityNumber();
            DriversLicense = csz.State.Formats[0];

            //http://www.internic.net/zones/root.zone
            _netUris.Add(
                new Uri(string.Format("email:{0}@{1}", Environment.GetEnvironmentVariable("USERNAME"),
                    System.Net.Dns.GetHostEntry("127.0.0.1").HostName)));

            if(withWholeFamily)
                ResolveFamilyState();

            _personality = new Personality(Etx.Number(0, 10000) <= 16);
        }

        #endregion

        #region overrides

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

        #endregion

        #region properties
        public string MiddleName { get; set; }
        public List<Tuple<KindsOfPersonalNames, string>> OtherNames { get { return _otherNames; } }
        public List<Tuple<Address, CityArea>> FormerResidences { get { return _formerAddresses; } }

        public Address HomeAddress { get; set; }
        public CityArea HomeCityArea { get; set; }

        public string HomeCity { get { return HomeCityArea.AddressData.City; } }
        public string HomeState { get { return HomeCityArea.AddressData.StateAbbrv; } }
        public string HomeZip { get { return HomeCityArea.AddressData.PostalCode; } }

        public Address WorkAddress { get; set; }
        public CityArea WorkCityArea { get; set; }

        public string WorkCity { get { return WorkCityArea.AddressData.City; } }
        public string WorkState { get { return WorkCityArea.AddressData.StateAbbrv; } }
        public string WorkZip { get { return WorkCityArea.AddressData.PostalCode; } }

        public NorthAmericanPhone WorkPhone { get; set; }
        public NorthAmericanPhone HomePhone { get; set; }
        public NorthAmericanPhone CellPhone { get; set; }

        public SocialSecurityNumber Ssn { get; set; }
        public DriversLicense DriversLicense { get; set; }

        public List<IAsset> Assets { get { return _assets; }}
        public List<ILoan> Debts { get { return _debts; } }
        public Income Income { get; set; }

        public override List<IPerson> Children
        {
            get { return _children; }
        }

        #endregion

        #region internal helpers
        /// <summary>
        /// Assigns <see cref="Father"/>, <see cref="Mother"/> and if applicable <see cref="Spouse"/> and <see cref="Children"/>
        /// </summary>
        protected internal void ResolveFamilyState()
        {
            ResolveParents();
            ResolveSpouse();
            ResolveChildren();
        }

        protected internal void ResolveParents()
        {
            if (_dob == null)
                throw
                    new RahRowRagee(
                        String.Format("The random person named {0}, {1} does not have a Date Of Birth assigned.",
                            LastName, FirstName));

            _mother = _mother ?? (_mother = NAmerUtil.SolveForParent(_dob.Value, NAmerUtil.Equations.FemaleYearOfMarriage2AvgAge, Gender.Female));
            _father = _father ?? (_father = NAmerUtil.SolveForParent(_dob.Value, NAmerUtil.Equations.MaleYearOfMarriage2AvgAge, Gender.Male));

            //at time of birth
            _mother.LastName = LastName;
            _father.LastName = LastName;

            _mother.MaritalStatus = NAmerUtil.GetMaritialStatus(_mother.BirthDate.Value, Gender.Female);
            _father.MaritalStatus = _mother.MaritalStatus;

            //mother not ever married to father
            if (_mother.MaritalStatus == MaritialStatus.Single)
                _father.LastName = NAmerUtil.GetAmericanLastName();

            //mother no longer married to father
            if (_mother.MaritalStatus == MaritialStatus.Divorced ||
                _mother.MaritalStatus == MaritialStatus.Remarried ||
                _mother.MaritalStatus == MaritialStatus.Separated)
                _mother.LastName = NAmerUtil.GetAmericanLastName();

            //hea
            if (_mother.MaritalStatus == MaritialStatus.Married)
                _mother.Spouse = _father;
        }

        protected internal void ResolveSpouse()
        {
            if (_myGender == Gender.Female &&
                (MaritalStatus == MaritialStatus.Divorced || MaritalStatus == MaritialStatus.Remarried ||
                 MaritalStatus == MaritialStatus.Separated))
            {
                //previous last name from previous marriage
                OtherNames.Add(new Tuple<KindsOfPersonalNames, string>(KindsOfPersonalNames.Former, NAmerUtil.GetAmericanLastName()));
            }

            if (MaritalStatus != MaritialStatus.Married && MaritalStatus != MaritialStatus.Remarried) return;

            if (_dob == null)
                throw
                    new RahRowRagee(
                        String.Format("The random person named {0}, {1} does not have a Date Of Birth assigned.",
                            LastName, FirstName));

            _spouse = _spouse ?? NAmerUtil.SolveForSpouse(_dob.Value, _myGender);
            _spouse.Spouse = this;
            if (_myGender == Gender.Female)
            {
                _lname = _spouse.LastName;
                if (OtherNames.All(x => x.Item1 != KindsOfPersonalNames.FatherSurname))
                    OtherNames.Add(new Tuple<KindsOfPersonalNames, string>(KindsOfPersonalNames.FatherSurname, _father.LastName));
            }
        }

        protected internal void ResolveChildren()
        {
            if (MyGender == Gender.Male)
                return;

            if (_dob == null)
                throw
                    new RahRowRagee(
                        String.Format("The random person named {0}, {1} does not have a Date Of Birth assigned.",
                            LastName, FirstName));

            var numOfChildren = NAmerUtil.SolveForNumberOfChildren(_dob.Value,
                Education == null ? Convert.ToInt16(0) : Education.GetEduLevel(null), null);

            if (numOfChildren <= 0)
                return;
            for (var i = 0; i < numOfChildren; i++)
            {
                var childDob = NAmerUtil.GetChildBirthDate(_dob.Value, i, null);
                if (childDob == null)
                    continue;

                var child = new NorthAmerican(NAmerUtil.GetChildBirthDate(_dob.Value, i, null),
                    Etx.CoinToss ? Gender.Female : Gender.Male) {Mother = this};

                var childAge = child.GetAge(null);

                if (child.GetAge(null) < 18)
                {
                    child.HomeAddress = HomeAddress;
                    child.MaritalStatus = MaritialStatus.Single;
                    child.Spouse = null;
                }
                if (child.GetAge(null) < 16)
                {
                    child.WorkAddress = null;
                }

                var formerLName = OtherNames.FirstOrDefault(x => x.Item1 == KindsOfPersonalNames.Former);

                var isDaughterWithSpouse = child.MyGender == Gender.Female &&
                                           NAmerUtil.Equations.FemaleDob2MarriageAge.SolveForY(child.BirthDate.Value.Year) >=
                                           childAge && childAge >= 18;

                if(!isDaughterWithSpouse)
                    child.LastName = formerLName == null ? LastName : formerLName.Item2;
               
                _children.Add(child);
            }
        }

        protected internal void ResolveFinancialState()
        {

            throw new NotImplementedException();
        }
        #endregion
    }

    /// <summary>
    /// Catalog of static utility methods related to North American
    /// </summary>
    public static class NAmerUtil
    {
        /// <summary>
        /// Marriage Source [https://www.census.gov/population/socdemo/hh-fam/ms2.xls] (1947-2011)
        /// Age of Birth Sources (1970-2014)
        /// [http://www.cdc.gov/nchs/data/nvsr/nvsr51/nvsr51_01.pdf] (Table 1.) 
        /// [http://www.cdc.gov/nchs/data/nvsr/nvsr64/nvsr64_12_tables.pdf] (Table I-1.)
        /// </summary>
        /// <remarks></remarks>
        public static class Equations
        {
            public static LinearEquation MaleDob2MarriageAge = new LinearEquation { Intercept = -181.45, Slope = 0.1056 };

            public static LinearEquation FemaleDob2MarriageAge = new LinearEquation
            {
                Intercept = -209.41,
                Slope = 0.1187
            };

            public static LinearEquation MaleYearOfMarriage2AvgAge = new LinearEquation
            {
                Intercept = -166.24,
                Slope = 0.0965
            };

            public static LinearEquation FemaleYearOfMarriage2AvgAge = new LinearEquation
            {
                Intercept = -191.74,
                Slope = 0.1083
            };

            public static LinearEquation FemaleAge2FirstChild = new LinearEquation
            {
                Intercept = -180.32,
                Slope = 0.1026
            };

            public static LinearEquation FemaleAge2SecondChild = new LinearEquation
            {
                Intercept = -175.88,
                Slope = 0.1017
            };

            public static LinearEquation FemaleAge2ThirdChild = new LinearEquation
            {
                Intercept = -129.45,
                Slope = 0.0792
            };

            public static LinearEquation FemaleAge2ForthChild = new LinearEquation
            {
                Intercept = -78.855,
                Slope = 0.0545
            };

            /// <summary>
            /// https://en.wikipedia.org/wiki/Childfree#Statistics_and_research
            /// </summary>
            public static NaturalLogEquation FemaleYob2ProbChildless = new NaturalLogEquation
            {
                Intercept = -55.479,
                Slope = 7.336
            };

        }

        /// <summary>
        /// Returns one entry at random selecting by <see cref="Person.MyGender"/> and <see cref="Person"/>
        /// using one of top 200 names for the given decade in which the DOB is a part.
        /// </summary>
        /// <returns></returns>
        public static string GetAmericanFirstName(DateTime? dateOfBirth, Gender gender)
        {
            var dt = dateOfBirth == null ? new DateTime(2000, 1, 2) : dateOfBirth.Value;
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
        /// Returns a date within the last 18 years.
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

            //plus or minus some random days
            var randomDaysNear = Etx.Number(1, 360)*(Etx.CoinToss ? 1 : -1);

            //go back meanAge years from atTime and add\minus some days
            return motherDob.AddYears((int) Math.Round(meanAge)).AddDays(randomDaysNear);
        }

        /// <summary>
        /// Return a string as American's call Race randomly with weight based on <see cref="zipCode"/>.
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns>Will default to string 'white' if <see cref="zipCode"/> cannot be resolved.</returns>
        public static string GetAmericanRace(string zipCode)
        {
            var amRace = Etx.RandomAmericanRaceWithRespectToZip(zipCode);

            if (amRace == null)
                return "White";

            var americanRaceProbabilityRanges = new List<AmericanRaceProbabilityRange>();
            double prevFrom = 0.000001D;
            var raceHashByZip = new Dictionary<string, double>
            {
                {"AmericanIndian", amRace.AmericanIndian},
                {"Asian", amRace.Asian},
                {"Black", amRace.Black},
                {"Hispanic", amRace.Hispanic},
                {"Mixed", amRace.Mixed},
                {"Pacific", amRace.Pacific},
                {"White", amRace.White}
            };

            foreach (var key in raceHashByZip.Keys)
            {
                var val = raceHashByZip[key];
                var f = prevFrom;
                var t = (val + prevFrom);
                americanRaceProbabilityRanges.Add(new AmericanRaceProbabilityRange { Name = key, From = f, To = t });
                prevFrom = (val + prevFrom + 0.000001D);
            }

            //pick one at random with probability
            var pick = (double)(Etx.MyRand.Next(1, 99999999)) / 1000000;

            var race = "White";

            var randomRace = americanRaceProbabilityRanges.FirstOrDefault(arpr => arpr.From <= pick && arpr.To >= pick);
            if (randomRace != null)
                race = randomRace.Name;

            return race;
        }

        /// <summary>
        /// Returns the <see cref="MaritialStatus"/> based on the gender and age.
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="sex"></param>
        /// <returns></returns>
        public static MaritialStatus GetMaritialStatus(DateTime dob, Gender sex)
        {
            //these are just guesses
            const double percentDivorced = 0.44;
            const int avgLengthOfMarriage = 10;
            const double yearsBeforeNextMarriage = 3.857;
            const double percentUnmarriedWholeLife = 0.054;

            if (Etx.Number(1, 1000) <= percentUnmarriedWholeLife * 1000)
                return MaritialStatus.Single;

            var avgAgeMarriage = sex == Gender.Female
                ? Equations.FemaleDob2MarriageAge.SolveForY(dob.ToDouble())
                : Equations.MaleDob2MarriageAge.SolveForY(dob.ToDouble());
            var cdt = DateTime.Now;
            var currentAge = (cdt.Year + cdt.DayOfYear / Constants.DBL_TROPICAL_YEAR) -
                             (dob.Year + dob.DayOfYear / Constants.DBL_TROPICAL_YEAR);

            if (currentAge < avgAgeMarriage)
                return MaritialStatus.Single;

            if (currentAge > avgAgeMarriage + avgLengthOfMarriage)
            {
                //spin for divorce
                var df = Etx.Number(1, 1000) <= percentDivorced * 1000;

                if (df && currentAge < avgAgeMarriage + avgLengthOfMarriage + yearsBeforeNextMarriage)
                    return Etx.Number(1, 1000) <= 64 ? MaritialStatus.Separated : MaritialStatus.Divorced;

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
        /// <param name="myDob"></param>
        /// <param name="gender"></param>
        /// <returns>return null for <see cref="Rand.Gender.Unknown"/></returns>
        public static IPerson SolveForSpouse(DateTime myDob, Gender gender)
        {
            if (gender == Gender.Unknown)
                return null;

            var ageDiff = Etx.Number(0, 4);
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
        /// <param name="edu">
        /// https://en.wikipedia.org/wiki/Childfree#Education
        /// </param>
        /// <param name="atDateTime">Optional, will default to current system time to calc Age</param>
        /// <returns></returns>
        public static int SolveForNumberOfChildren(DateTime dob, short edu, DateTime? atDateTime)
        {
            //rand for lifetime childless
            var eduAdditive = 0.0;
            if (edu == (short) (OccidentalEdu.College | OccidentalEdu.Grad))
                eduAdditive = 0.09;
            if (edu == (short) (OccidentalEdu.College | OccidentalEdu.Some))
                eduAdditive = 0.04;
            if (edu == (short) (OccidentalEdu.HighSchool | OccidentalEdu.Grad))
                eduAdditive = 0.02;

            var probChildless = Math.Round(Equations.FemaleYob2ProbChildless.SolveForY(dob.Year), 2);

            probChildless += eduAdditive;

            if (Math.Round((double) Etx.Number(1, 100)/100, 2) <= probChildless)
                return 0;

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
    }

    //container class for Race probability tables
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class AmericanRaceProbabilityRange
    {
        internal string Name { get; set; }
        internal double From { get; set; }
        internal double To { get; set; }
    }
}