using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Gov;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;

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

            var deathDate = UsState.GetDeathDate(p.BirthCert.DateOfBirth, p.MyGender.ToString());

            if (nullOnFutureDate && deathDate > DateTime.Now)
                return null;

            var manner = Etx.DiscreteRange(AmericanData.MannerOfDeathAvgs);
            return new AmericanDeathCert(manner, String.Join(" ", p.FirstName, p.LastName)) { DateOfDeath = deathDate };
        }

        /// <summary>
        /// Returns one entry at random selecting by <see cref="Person.MyGender"/> and <see cref="Person"/>
        /// using one of top 200 names for the given decade in which the DOB is a part.
        /// </summary>
        /// <returns></returns>
        public static string GetAmericanFirstName(DateTime? dateOfBirth, Gender gender)
        {
            var dt = dateOfBirth ?? DateTime.Today.AddYears(-18);
            var xmlData = TreeData.AmericanFirstNamesData;
            
            if (xmlData == null)
            {
                return gender == Gender.Male ? "John" : "Jane";
            }
            var firstNameNodes = xmlData.SelectNodes("//first-name");
            if (firstNameNodes == null)
            {
                return gender == Gender.Male ? "John" : "Jane";
            }
            var decadeElem = xmlData.SelectSingleNode("//first-name[last()]");
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
        public static string GetAmericanLastName()
        {
            var xmlData = TreeData.AmericanLastNamesData;
            var lnameNodes = xmlData.SelectSingleNode("//last-name");
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
        public static DateTime? GetChildBirthDate(DateTime motherDob, int numberOfSiblings, DateTime? atTime)
        {
            //put number of siblings in scope for equations
            if (numberOfSiblings <= 0)
                numberOfSiblings = 0;
            if (numberOfSiblings > 4)
                numberOfSiblings = 3;

            //default atTime to now
            var dt = atTime ?? DateTime.Now;

            motherDob = AmericanEquations.ProtectAgainstDistantTimes(motherDob);

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

            return Etx.Date((int)Math.Round(meanAge), motherDob);

        }

        /// <summary>
        /// Returns the <see cref="MaritialStatus"/> based on the gender and age.
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        public static MaritialStatus GetMaritialStatus(DateTime? dob, Gender gender)
        {
            if (Etx.MyRand.NextDouble() <= AmericanData.PERCENT_UNMARRIED_WHOLE_LIFE)
                return MaritialStatus.Single;
            dob = dob ?? UsState.GetWorkingAdultBirthDate();

            dob = AmericanEquations.ProtectAgainstDistantTimes(dob.Value);

            var avgAgeMarriage = gender == Gender.Female
                ? AmericanEquations.FemaleAge2FirstMarriage.SolveForY(dob.Value.ToDouble())
                : AmericanEquations.MaleAge2FirstMarriage.SolveForY(dob.Value.ToDouble());

            var cdt = DateTime.Now;
            var currentAge = Etc.CalcAge(dob.Value, cdt);

            if (currentAge < avgAgeMarriage)
                return MaritialStatus.Single;

            //chance for being widowed goes up exp for older 
            if (Etx.MyRand.NextDouble() <= AmericanEquations.Age2ProbWidowed.SolveForY(currentAge))
                return MaritialStatus.Widowed;

            //young first marriage
            if (!(currentAge > avgAgeMarriage + AmericanData.AVG_LENGTH_OF_MARRIAGE)) return MaritialStatus.Married;

            //spin for divorce
            var df = Etx.MyRand.NextDouble() <= AmericanData.PERCENT_DIVORCED;

            //have 'separated' (whatever it means) as low probablity
            if (df && currentAge < avgAgeMarriage + AmericanData.AVG_LENGTH_OF_MARRIAGE + AmericanData.YEARS_BEFORE_NEXT_MARRIAGE)
                return Etx.TryBelowOrAt(64, Etx.Dice.OneThousand) ? MaritialStatus.Separated : MaritialStatus.Divorced;

            //have prob of never remarry
            if (df && gender == Gender.Male)
            {
                return Etx.MyRand.NextDouble() <= AmericanData.PERCENT_OF_MEN_MARRIED_ONCE_NEVER_AGAIN
                    ? MaritialStatus.Divorced
                    : MaritialStatus.Remarried;
            }
            if (df && gender == Gender.Female)
            {
                return Etx.MyRand.NextDouble() <= AmericanData.PERCENT_OF_WOMEN_MARRIED_ONCE_NEVER_AGAIN
                    ? MaritialStatus.Divorced
                    : MaritialStatus.Remarried;
            }
            return df ? MaritialStatus.Remarried : MaritialStatus.Married;
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
        public static IPerson SolveForParent(DateTime? childDob, LinearEquation eq, Gender gender)
        {
            if (eq == null)
            {
                eq = gender == Gender.Male
                    ? AmericanEquations.MaleAge2FirstMarriage
                    : AmericanEquations.FemaleAge2FirstMarriage;
            }

            childDob = childDob ?? UsState.GetWorkingAdultBirthDate();

            //move to a date 1 - 6 years prior the Person's dob
            var dtPm = childDob.Value.AddYears(-1 * Etx.IntNumber(1, 6)).AddDays(Etx.IntNumber(1, 360));

            dtPm = AmericanEquations.ProtectAgainstDistantTimes(dtPm);

            //calc the age of marriable person at this time
            var avgAgeCouldMarry =
                eq.SolveForY(dtPm.ToDouble());

            //move the adjusted child-dob date back by calc'ed years 
            var parentDob = dtPm.AddYears(Convert.ToInt32(Math.Round(avgAgeCouldMarry, 0))*-1);

            var aParent = new American(parentDob, gender, false);
            return aParent;
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
        public static IPerson SolveForSpouse(DateTime? myDob, Gender gender, int maxAgeDiff = 4)
        {
            if (gender == Gender.Unknown)
                return null;

            myDob = myDob ?? UsState.GetWorkingAdultBirthDate();

            var ageDiff = Etx.IntNumber(0, maxAgeDiff);
            ageDiff = gender == Gender.Female ? ageDiff * -1 : ageDiff;

            //randomize dob of spouse
            var spouseDob = myDob.Value.AddYears(ageDiff).AddDays(Etx.IntNumber(1, 360) * Etx.PlusOrMinusOne);

            //define spouse
            return new American(spouseDob, gender == Gender.Female ? Gender.Male : Gender.Female);

        }

        /// <summary>
        /// Produces a random value between 0 and 4.
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="atDateTime">Optional, will default to current system time to calc Age</param>
        /// <returns></returns>
        public static int SolveForNumberOfChildren(DateTime? dob, DateTime? atDateTime)
        {
            //average to be 2.5 
            var vt = DateTime.Now;
            if (atDateTime != null)
                vt = atDateTime.Value;
            dob = dob ?? UsState.GetWorkingAdultBirthDate();
            dob = AmericanEquations.ProtectAgainstDistantTimes(dob.Value);
            var age = Etc.CalcAge(dob.Value, vt);
            var randV = Etx.IntNumber(1, 100);

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
            dob = dob ?? UsState.GetWorkingAdultBirthDate();
            var eduAdditive = 0.0;
            if (educationLevel == (OccidentalEdu.Bachelor | OccidentalEdu.Grad))
                eduAdditive = 0.09;
            if (educationLevel == (OccidentalEdu.Bachelor | OccidentalEdu.Some))
                eduAdditive = 0.04;
            if (educationLevel == (OccidentalEdu.HighSchool | OccidentalEdu.Grad))
                eduAdditive = 0.02;

            dob = AmericanEquations.ProtectAgainstDistantTimes(dob.Value);

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
        public static DateTime? SolveForMarriageDate(DateTime? dob, Gender myGender)
        {
            dob = dob ?? UsState.GetWorkingAdultBirthDate();
            dob = AmericanEquations.ProtectAgainstDistantTimes(dob.Value);
            var dt = DateTime.Now;
            var avgAgeMarriage = myGender == Gender.Female
                ? AmericanEquations.FemaleAge2FirstMarriage.SolveForY(dob.Value.ToDouble())
                : AmericanEquations.MaleAge2FirstMarriage.SolveForY(dob.Value.ToDouble());
            var currentAge = Etc.CalcAge(dob.Value, dt);

            //all other MaritialStatus imply at least one marriage in past
            var yearsMarried = currentAge - Convert.ToInt32(Math.Round(avgAgeMarriage));

            return Etx.Date(-1*yearsMarried, dt);
        }

        /// <summary>
        /// Difference of national avg to race average added to state average.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="race"></param>
        /// <param name="edu"></param>
        /// <returns></returns>
        public static double SolvePercentGradByStateAndRace(UsState state, NorthAmericanRace? race,
            OccidentalEdu edu = OccidentalEdu.HighSchool | OccidentalEdu.Grad)
        {
            AmericanRacePercents p;
            p = edu >= OccidentalEdu.Bachelor ? AmericanUniversity.NatlGradRate() : AmericanHighSchool.NatlGradRate();
            var stateAvg = p.National;
            var natlAvg = p.National;
            var stateData = UsStateData.GetStateData(state?.ToString());
            if (stateData?.PercentOfGrads != null && stateData.PercentOfGrads.Count > 0)
            {
                var f = stateData.PercentOfGrads.FirstOrDefault(x => x.Item1 == edu);
                if (f != null)
                {
                    stateAvg = Math.Round(f.Item2, 1);
                }
            }

            var raceNatlAvg = new Dictionary<NorthAmericanRace, double>
            {
                {NorthAmericanRace.AmericanIndian, p.AmericanIndian - natlAvg},
                {NorthAmericanRace.Asian, p.Asian - natlAvg},
                {NorthAmericanRace.Hispanic, p.Hispanic - natlAvg},
                {NorthAmericanRace.Black, p.Black - natlAvg},
                {NorthAmericanRace.White, p.White - natlAvg},
                {NorthAmericanRace.Pacific, p.Pacific - natlAvg},
                {NorthAmericanRace.Mixed, p.Mixed - natlAvg}
            };
            if (race == null || !raceNatlAvg.ContainsKey(race.Value))
                return Math.Round(stateAvg, 1);

            return Math.Round(stateAvg + raceNatlAvg[race.Value], 1);
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
            thisPerson._phoneNumbers.Clear();
            if (livesWithThisOne._phoneNumbers.Any(p => p.Item1 == KindsOfLabels.Home))
            {
                thisPerson._phoneNumbers.Add(livesWithThisOne._phoneNumbers.First(p => p.Item1 == KindsOfLabels.Home));
            }
            if (thisPerson.GetAgeAt(null) < 12 ||
                String.IsNullOrWhiteSpace(addrMatchTo.HomeCityArea?.GetPostalCodePrefix()))
                return;

            var mobilePhone = Phone.American(addrMatchTo.HomeCityArea.GetPostalCodePrefix());
            thisPerson._phoneNumbers.Add(new Tuple<KindsOfLabels, NorthAmericanPhone>(KindsOfLabels.Mobile,
                mobilePhone));
        }

        /// <summary>
        /// Utility method to dump all the spouse data on an instance of <see cref="American"/>
        /// </summary>
        /// <param name="nAmer"></param>
        /// <returns></returns>
        public static List<Spouse> DumpAllSpouses(American nAmer)
        {
            return nAmer._spouses.ToList();
        }
    }
}
