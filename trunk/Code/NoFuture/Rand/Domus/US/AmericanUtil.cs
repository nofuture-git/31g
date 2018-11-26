using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Gov;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;
using System.Reflection;
using NoFuture.Rand.Gov.US;
using NoFuture.Rand.Tele;

namespace NoFuture.Rand.Domus.US
{
    /// <summary>
    /// Catalog of static utility methods related to North American
    /// </summary>
    /// <remarks>
    /// US zip code data is derived from a list on wikipedia x-ref'ed with 
    /// 2010 census data.  Likewise, the first names data (both male and female)
    /// are from US Census website.
    /// https://www.census.gov/geo/maps-data/index.html
    /// 
    /// US High School data is a subset derived from listing:
    /// http://nces.ed.gov/ccd/pubschuniv.asp and is what is 
    /// used for American Race.
    /// </remarks>
    public static class AmericanUtil
    {
        internal const int MIN_AGE_TO_BE_PARENT = 15;
        internal const string US_FIRST_NAMES = "US_FirstNames.xml";
        internal const string US_LAST_NAMES = "US_LastNames.xml";
        internal static XmlDocument FirstNamesXml;
        internal static XmlDocument LastNamesXml;

        /// <summary>
        /// Generates a <see cref="DeathCert"/> at random based on the given <see cref="p"/>
        /// </summary>
        /// <param name="p"></param>
        /// <param name="nullOnFutureDate">
        /// Switch parameter to have null returned whenever the random date-of-death is 
        /// in the future.  
        /// </param>
        /// <returns></returns>
        public static DeathCert GetRandomDeathCert(IPerson p, bool nullOnFutureDate = true)
        {
            if (p?.BirthCert == null)
                return null;

            var deathDate = AmericanDeathCert.RandomDeathDate(p.BirthCert.DateOfBirth, p.Gender.ToString());

            if (nullOnFutureDate && deathDate > DateTime.UtcNow)
                return null;

            var manner = Etx.RandomPickOne(AmericanData.MannerOfDeathAvgs);
            return new AmericanDeathCert(manner, String.Join(" ", p.FirstName, p.LastName)) { DateOfDeath = deathDate };
        }

        /// <summary>
        /// Returns one entry at random selecting by Gender
        /// using one of top 200 names for the given decade in which the DOB is a part.
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static string RandomAmericanFirstName(Gender gender, DateTime? dateOfBirth = null)
        {
            var dt = dateOfBirth ?? DateTime.Today.AddYears(-18);
            FirstNamesXml = FirstNamesXml ?? XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_FIRST_NAMES, Assembly.GetExecutingAssembly());
            
            if (FirstNamesXml == null)
            {
                return gender == Gender.Male ? "John" : "Jane";
            }
            var firstNameNodes = FirstNamesXml.SelectNodes("//first-name");
            if (firstNameNodes == null)
            {
                return gender == Gender.Male ? "John" : "Jane";
            }
            var decadeElem = FirstNamesXml.SelectSingleNode("//first-name[last()]");
            foreach (var decadeNode in firstNameNodes)
            {
                var decade = decadeNode as XmlElement;
                if (decade == null)
                    continue;
                var start = DateTime.Parse(decade.Attributes["decade-start"].Value);
                var end = DateTime.Parse(decade.Attributes["decade-end"].Value);

                var cStart = DateTime.Compare(dt, start);//-ge 
                var cEnd = DateTime.Compare(dt, end);//-le
                if (cStart < 0 || cEnd > 0) continue;

                decadeElem = decade;
                break;
            }
            if (decadeElem == null)
                return gender == Gender.Male ? "John" : "Jane";
            var genderIdx = gender != Gender.Female ? 0 : 1;
            var mfFnames = decadeElem.ChildNodes[genderIdx];

            var p = Etx.MyRand.Next(0, mfFnames.ChildNodes.Count);
            return mfFnames.ChildNodes[p].InnerText;
        }

        /// <summary>
        /// Return one of 500 selected lastnames - source is unknown.
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static string RandomAmericanLastName()
        {
            LastNamesXml = LastNamesXml ?? XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_LAST_NAMES, Assembly.GetExecutingAssembly());
            var lnameNodes = LastNamesXml.SelectSingleNode("//last-name");
            if (lnameNodes == null)
                return "Doe";
            var p = Etx.MyRand.Next(0, lnameNodes.ChildNodes.Count);
            return lnameNodes.ChildNodes[p].InnerText;
        }

        /// <summary>
        /// Generates a random past date based on the <see cref="numberOfSiblings"/> and the Mother's Date of Birth.
        /// </summary>
        /// <param name="motherDob"></param>
        /// <param name="numberOfSiblings">this should be the current number of children of the mother</param>
        /// <param name="atTime">Defaults to the current time</param>
        /// <returns></returns>
        [RandomFactory]
        public static DateTime? RandomChildBirthDate(DateTime motherDob, int numberOfSiblings, DateTime? atTime = null)
        {
            //put number of siblings in scope for equations
            if (numberOfSiblings <= 0)
                numberOfSiblings = 0;
            if (numberOfSiblings > 4)
                numberOfSiblings = 3;

            //default atTime to now
            var dt = atTime ?? DateTime.UtcNow;

            var motherAge = Etc.CalcAge(motherDob, dt);

            var meanAge = AmericanEquations.FemaleAge2FirstChild.SolveForY(motherDob.Year);

            //mother is too young for children
            if (motherAge < meanAge)
                return null;

            switch (numberOfSiblings)
            {
                case 2:
                    meanAge = AmericanEquations.FemaleAge2SecondChild.SolveForY(motherDob.Year);
                    if (motherAge < meanAge)
                        return null;
                    break;
                case 3:
                    meanAge = AmericanEquations.FemaleAge2ThirdChild.SolveForY(motherDob.Year);
                    if (motherAge < meanAge)
                        return null;
                    break;
                case 4:
                    meanAge = AmericanEquations.FemaleAge2ForthChild.SolveForY(motherDob.Year);
                    if (motherAge < meanAge)
                        return null;
                    break;
            }

            return Etx.RandomDate((int)Math.Round(meanAge), motherDob);

        }

        /// <summary>
        /// Returns a new <see cref="IPerson"/> representing a 
        /// parent of the specified <see cref="parentGender"/> having a realistic age 
        /// from the <see cref="childDob"/>.
        /// </summary>
        /// <param name="childDob"></param>
        /// <param name="parentGender"></param>
        /// <param name="age2FirstMarriageEq">
        /// Optional, linear equation to solve for average age of first marriage
        /// </param>
        /// <returns></returns>
        [RandomFactory]
        public static IPerson RandomParent(DateTime? childDob, Gender parentGender, LinearEquation age2FirstMarriageEq = null)
        {
            var parentDob = RandomParentBirthDate(childDob, parentGender, age2FirstMarriageEq);

            var aParent = American.RandomAmerican(parentDob, parentGender, false);
            return aParent;
        }

        /// <summary>
        /// Returns a date-of-birth for the parent of the person born on <see cref="childDob"/> at random.
        /// </summary>
        /// <param name="childDob">
        /// Optional, defaults to a random adult birth date
        /// </param>
        /// <param name="parentGender">
        /// Optional, will coin-toss if its null.
        /// </param>
        /// <param name="age2FirstMarriageEq">
        /// Optional, allow caller to control equation used.
        /// </param>
        /// <returns></returns>
        [RandomFactory]
        public static DateTime RandomParentBirthDate(DateTime? childDob = null, Gender? parentGender = null,
            IEquation age2FirstMarriageEq = null)
        {
            parentGender = parentGender ?? (Etx.RandomCoinToss() ? Gender.Female : Gender.Male);

            if (age2FirstMarriageEq == null)
            {
                age2FirstMarriageEq = parentGender == Gender.Male
                    ? AmericanEquations.MaleAge2FirstMarriage
                    : AmericanEquations.FemaleAge2FirstMarriage;
            }

            childDob = childDob ?? Etx.RandomAdultBirthDate();

            //move to a date 1 - 6 years prior the Person's dob
            var dtPm = childDob.Value.AddYears(-1 * Etx.RandomInteger(1, 6)).AddDays(Etx.RandomInteger(1, 360));

            //calc the age of marriable person at this time
            var avgAgeCouldMarry =
                age2FirstMarriageEq.SolveForY(dtPm.ToDouble());

            //move the adjusted child-dob date back by calc'ed years 
            var parentDob = dtPm.AddYears(Convert.ToInt32(Math.Round(avgAgeCouldMarry, 0)) * -1);

            return parentDob;
        }

        /// <summary>
        /// Returns a new <see cref="IPerson"/> having the opposite gender
        /// of <see cref="gender"/> with a similar <see cref="IPerson.BirthCert"/>
        /// </summary>
        /// <param name="myDob">
        /// Only used to generate a Birth Date around the <see cref="maxAgeDiff"/>
        /// </param>
        /// <param name="gender"></param>
        /// <param name="maxAgeDiff">Optional difference in age of spouse.</param>
        /// <returns>return null for <see cref="Gender.Unknown"/></returns>
        [RandomFactory]
        public static IPerson RandomSpouse(DateTime? myDob, Gender gender, int maxAgeDiff = 4)
        {
            if (gender == Gender.Unknown)
                return null;

            myDob = myDob ?? Etx.RandomAdultBirthDate();

            var ageDiff = Etx.RandomInteger(0, maxAgeDiff);
            ageDiff = gender == Gender.Female ? ageDiff * -1 : ageDiff;

            //randomize dob of spouse
            var spouseDob = myDob.Value.AddYears(ageDiff).AddDays(Etx.RandomInteger(1, 360) * Etx.RandomPlusOrMinus());

            //define spouse
            return American.RandomAmerican(spouseDob, gender == Gender.Female ? Gender.Male : Gender.Female);

        }

        /// <summary>
        /// Produces a random value between 0 and 4.
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="atDateTime">Optional, will default to current system time to calc Age</param>
        /// <returns></returns>
        [RandomFactory]
        public static int RandomNumberOfChildren(DateTime? dob, DateTime? atDateTime = null)
        {
            //average to be 2.5 
            var vt = DateTime.UtcNow;
            if (atDateTime != null)
                vt = atDateTime.Value;
            dob = dob ?? Etx.RandomAdultBirthDate();
            var age = Etc.CalcAge(dob.Value, vt);
            var randV = Etx.RandomInteger(1, 100);

            var meanAge = Math.Round(AmericanEquations.FemaleAge2ForthChild.SolveForY(vt.Year));

            if (randV >= 84 && age >= meanAge)
                return 4;

            meanAge = Math.Round(AmericanEquations.FemaleAge2ThirdChild.SolveForY(vt.Year));

            if (randV >= 66 && age >= meanAge)
                return 3;

            meanAge = Math.Round(AmericanEquations.FemaleAge2SecondChild.SolveForY(vt.Year));

            if (randV >= 33 && age >= meanAge)
                return 2;

            meanAge = Math.Round(AmericanEquations.FemaleAge2FirstChild.SolveForY(vt.Year));

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
        public static double SolveForProbabilityChildless(DateTime? dob,
            OccidentalEdu educationLevel = OccidentalEdu.HighSchool | OccidentalEdu.Grad)
        {
            dob = dob ?? Etx.RandomAdultBirthDate();
            var eduAdditive = 0.0;
            if (educationLevel == (OccidentalEdu.Bachelor | OccidentalEdu.Grad))
                eduAdditive = 0.09;
            if (educationLevel == (OccidentalEdu.Bachelor | OccidentalEdu.Some))
                eduAdditive = 0.04;
            if (educationLevel == (OccidentalEdu.HighSchool | OccidentalEdu.Grad))
                eduAdditive = 0.02;

            var probChildless = Math.Round(AmericanEquations.FemaleYob2ProbChildless.SolveForY(dob.Value.Year), 2);

            probChildless += eduAdditive;

            return probChildless;
        }

        /// <summary>
        /// Solves for a marriage date based on gender and date-of-birth
        /// with randomness.
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="myGender"></param>
        /// <returns></returns>
        [RandomFactory]
        public static DateTime? RandomMarriageDate(DateTime? dob, Gender myGender)
        {
            dob = dob ?? Etx.RandomAdultBirthDate();
            var dt = DateTime.UtcNow;
            var avgAgeMarriage = myGender == Gender.Female
                ? AmericanEquations.FemaleAge2FirstMarriage.SolveForY(dob.Value.ToDouble())
                : AmericanEquations.MaleAge2FirstMarriage.SolveForY(dob.Value.ToDouble());
            var currentAge = Etc.CalcAge(dob.Value, dt);

            //all other MaritialStatus imply at least one marriage in past
            var yearsMarried = currentAge - Convert.ToInt32(Math.Round(avgAgeMarriage));

            var marriedOn = Etx.RandomDate(-1*yearsMarried, dt);

            //zero-out the time
            marriedOn = new DateTime(marriedOn.Year, marriedOn.Month, marriedOn.Day);
            return marriedOn;
        }

        /// <summary>
        /// Sets <see cref="thisPerson"/> home-related data to the same values of <see cref="livesWithThisOne"/>
        /// </summary>
        /// <param name="thisPerson"></param>
        /// <param name="livesWithThisOne"></param>
        public static void SetNAmerCohabitants(American thisPerson, American livesWithThisOne)
        {
            if (thisPerson == null || livesWithThisOne == null)
                return;
            var addrMatchTo = livesWithThisOne.GetAddressAt(null);
            if (addrMatchTo == null)
                return;
            thisPerson.AddAddress(addrMatchTo);
            thisPerson.GetPhoneNumbers().Clear();
            if (livesWithThisOne.GetPhoneNumbers().Any(p => p.Descriptor == KindsOfLabels.Home))
            {
                thisPerson.AddPhone(livesWithThisOne.GetPhoneNumbers().First(p => p.Descriptor == KindsOfLabels.Home));
            }
            if (thisPerson.GetAgeAt(null) < 12 ||
                String.IsNullOrWhiteSpace(addrMatchTo.CityArea?.GetPostalCodePrefix()))
                return;

            var mobilePhone = NorthAmericanPhone.RandomAmericanPhone(addrMatchTo.CityArea.GetPostalCodePrefix());
            mobilePhone.Descriptor = KindsOfLabels.Mobile;
            thisPerson.AddPhone(mobilePhone);
        }

        /// <summary>
        /// Utility method to dump all the spouse data on an instance of <see cref="American"/>
        /// </summary>
        /// <param name="nAmer"></param>
        /// <returns></returns>
        public static List<Spouse> DumpAllSpouses(American nAmer)
        {
            return nAmer.GetSpouses().ToList();
        }
    }
}
