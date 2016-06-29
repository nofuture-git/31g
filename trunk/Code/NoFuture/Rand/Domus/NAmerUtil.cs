﻿using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Util.Math;
using System.Xml;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Types;
using System.Xml.Linq;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Gov;
using NoFuture.Util;

namespace NoFuture.Rand.Domus
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
        /// [http://www.pewsocialtrends.org/2014/11/14/four-in-ten-couples-are-saying-i-do-again/st_2014-11-14_remarriage-02/]
        /// </summary>
        public const double PercentOfMenMarriedOnceNeverAgain = 0.29D;

        /// <summary>
        /// [http://www.pewsocialtrends.org/2014/11/14/four-in-ten-couples-are-saying-i-do-again/st_2014-11-14_remarriage-02/]
        /// </summary>
        public const double PercentOfWomenMarriedOnceNeverAgain = 0.15D;
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
        /// Any <see cref="RLinearEquation"/> have a random Standard Dev between 0 and 2.99_
        /// </remarks>
        public static class Equations
        {
            public static RLinearEquation MaleDob2MarriageAge = new RLinearEquation
            {
                Intercept = -181.45,
                Slope = 0.1056,
                StdDev = Etx.RationalNumber(0, 1)
            };

            public static RLinearEquation FemaleDob2MarriageAge = new RLinearEquation
            {
                Intercept = -209.41,
                Slope = 0.1187,
                StdDev = Etx.RationalNumber(0, 1)
            };

            public static RLinearEquation MaleYearOfMarriage2AvgAge = new RLinearEquation
            {
                Intercept = -166.24,
                Slope = 0.0965,
                StdDev = Etx.RationalNumber(0, 1)
            };

            public static RLinearEquation FemaleYearOfMarriage2AvgAge = new RLinearEquation
            {
                Intercept = -191.74,
                Slope = 0.1083,
                StdDev = Etx.RationalNumber(0, 2)
            };

            public static RLinearEquation FemaleAge2FirstChild = new RLinearEquation
            {
                Intercept = -176.32,//-180.32
                Slope = 0.1026,
                StdDev = Etx.RationalNumber(0, 1)
            };

            public static RLinearEquation FemaleAge2SecondChild = new RLinearEquation
            {
                Intercept = -171.88,//-175.88
                Slope = 0.1017,
                StdDev = Etx.RationalNumber(0, 1)
            };

            public static RLinearEquation FemaleAge2ThirdChild = new RLinearEquation
            {
                Intercept = -125.45,//-129.45
                Slope = 0.0792,
                StdDev = Etx.RationalNumber(0, 1)
            };

            public static RLinearEquation FemaleAge2ForthChild = new RLinearEquation
            {
                Intercept = -74.855,//-78.855
                Slope = 0.0545,
                StdDev = Etx.RationalNumber(0, 1)
            };

            /// <summary>
            /// Has no stat validity - just a guess
            /// </summary>
            public static ExponentialEquation Age2ProbWidowed = new ExponentialEquation
            {
                ConstantValue = Math.Pow(10, -13),
                Power = 6.547
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

            /// <summary>
            /// Mean is from WHO [http://apps.who.int/gho/data/node.main.688?lang=en],
            /// the StdDev is made-up.
            /// </summary>
            /// <param name="mf"></param>
            /// <returns></returns>
            public static NormalDistEquation LifeExpectancy(Gender mf)
            {
                return mf == Gender.Male
                    ? new NormalDistEquation {Mean = 76.9D, StdDev = 4.9}
                    : new NormalDistEquation {Mean = 81.6, StdDev = 3.9};
            }
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
            XmlElement decadeElem = null;
            if (xmlData == null)
            {
                return gender == Gender.Male ? "John" : "Jane";
            }
            var firstNameNodes = xmlData.SelectNodes("//first-name");
            if (firstNameNodes == null)
            {
                return gender == Gender.Male ? "John" : "Jane";
            }
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
        /// Returns a date being between 21 years ago today back to 55 years ago today.
        /// </summary>
        /// <remarks>
        /// The age is limited - generate with family to get other age sets
        /// </remarks>
        public static DateTime GetWorkingAdultBirthDate()
        {
            return DateTime.Now.AddYears(-1 * Etx.MyRand.Next(21, 55)).AddDays(Etx.IntNumber(1, 360));
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

            return Etx.Date((int)Math.Round(meanAge), motherDob);

        }

        /// <summary>
        /// Return a <see cref="NorthAmericanRace"/> randomly with weight based on <see cref="zipCode"/>.
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns>
        /// Defaults to randomly to national averages
        /// [http://kff.org/other/state-indicator/distribution-by-raceethnicity/]
        /// </returns>
        public static NorthAmericanRace GetAmericanRace(string zipCode)
        {
            //defaults
            if (string.IsNullOrWhiteSpace(zipCode))
            {
                var roll = Etx.IntNumber(0, 100);
                if(roll >= 0 && roll < 61)
                    return NorthAmericanRace.White;

                if(roll >= 61 && roll < 73)
                    return NorthAmericanRace.Black;

                if(roll >= 73 && roll < 91)
                    return NorthAmericanRace.Hispanic;

                if(roll >= 91 && roll < 97)
                    return NorthAmericanRace.Asian;

                if(roll == 97)
                    return NorthAmericanRace.Pacific;

                if(roll == 98)
                    return NorthAmericanRace.AmericanIndian;

                if(roll == 99 || roll == 100)
                    return NorthAmericanRace.Mixed;

            }
            var amRace = RandomAmericanRaceWithRespectToZip(zipCode);

            if (amRace == null)
                return NorthAmericanRace.White;

            var americanRaceProbabilityRanges = new List<AmericanRaceProbabilityRange>();
            var prevFrom = 0.000001D;
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
            if (Etx.MyRand.NextDouble() <= PercentUnmarriedWholeLife)
                return MaritialStatus.Single;

            var avgAgeMarriage = gender == Gender.Female
                ? Equations.FemaleDob2MarriageAge.SolveForY(dob.ToDouble())
                : Equations.MaleDob2MarriageAge.SolveForY(dob.ToDouble());

            var cdt = DateTime.Now;
            var currentAge = Person.CalcAge(dob, cdt);

            if (currentAge < avgAgeMarriage)
                return MaritialStatus.Single;

            //chance for being widowed goes up exp for older 
            if (Etx.MyRand.NextDouble() <= Equations.Age2ProbWidowed.SolveForY(currentAge))
                return MaritialStatus.Widowed;

            //young first marriage
            if (!(currentAge > avgAgeMarriage + AvgLengthOfMarriage)) return MaritialStatus.Married;

            //spin for divorce
            var df = Etx.MyRand.NextDouble() <= PercentDivorced;

            //have 'separated' (whatever it means) as low probablity
            if (df && currentAge < avgAgeMarriage + AvgLengthOfMarriage + YearsBeforeNextMarriage)
                return Etx.TryBelowOrAt(64, Etx.Dice.OneThousand) ? MaritialStatus.Separated : MaritialStatus.Divorced;

            //have prob of never remarry
            if (df && gender == Gender.Male)
            {
                return Etx.MyRand.NextDouble() <= PercentOfMenMarriedOnceNeverAgain
                    ? MaritialStatus.Divorced
                    : MaritialStatus.Remarried;
            }
            if (df && gender == Gender.Female)
            {
                return Etx.MyRand.NextDouble() <= PercentOfWomenMarriedOnceNeverAgain
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
        public static IPerson SolveForParent(DateTime childDob, LinearEquation eq, Gender gender)
        {
            //move to a date 1 - 6 years prior the Person's dob
            var dtPm = childDob.AddYears(-1 * Etx.IntNumber(1, 6)).AddDays(Etx.IntNumber(1, 360));

            //calc the age of marriable person at this time
            var avgAgeCouldMarry =
                eq.SolveForY(dtPm.ToDouble());

            //move the adjusted child-dob date back by calc'ed years 
            var parentDob = dtPm.AddYears(Convert.ToInt32(Math.Round(avgAgeCouldMarry, 0))*-1);

            var aParent = new NorthAmerican(parentDob, gender);
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
        /// <returns>return null for <see cref="Rand.Gender.Unknown"/></returns>
        public static IPerson SolveForSpouse(DateTime myDob, Gender gender, int maxAgeDiff = 4)
        {
            if (gender == Gender.Unknown)
                return null;

            var ageDiff = Etx.IntNumber(0, maxAgeDiff);
            ageDiff = gender == Gender.Female ? ageDiff * -1 : ageDiff;

            //randomize dob of spouse
            var spouseDob = myDob.AddYears(ageDiff).AddDays(Etx.IntNumber(1, 360) * Etx.PlusOrMinusOne);

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
            var randV = Etx.IntNumber(1, 100);

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

        /// <summary>
        /// Solves for a marriage date based on gender and date-of-birth
        /// with randomness.
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="myGender"></param>
        /// <returns></returns>
        public static DateTime? SolveForMarriageDate(DateTime? dob, Gender myGender)
        {
            if (dob == null)
                return null;
            var dt = DateTime.Now;
            var avgAgeMarriage = myGender == Gender.Female
                ? Equations.FemaleDob2MarriageAge.SolveForY(dob.Value.ToDouble())
                : Equations.MaleDob2MarriageAge.SolveForY(dob.Value.ToDouble());
            var currentAge = Person.CalcAge(dob.Value, dt);

            //all other MaritialStatus imply at least one marriage in past
            var yearsMarried = currentAge - Convert.ToInt32(Math.Round(avgAgeMarriage));

            return Etx.Date(-1*yearsMarried, dt);
        }

        /// <summary>
        /// Difference of national avg to race average added to state average.
        /// [http://nces.ed.gov/programs/coe/indicator_coi.asp]
        /// </summary>
        /// <param name="state"></param>
        /// <param name="race"></param>
        /// <returns></returns>
        public static int SolvePercentHsGradByStateAndRace(UsState state, NorthAmericanRace race)
        {
            var natlAvg = 82;
            var stateAvg = natlAvg;
            if (state?.GetStateData() != null)
            {
                var stateData = state.GetStateData();
                if (stateData.PercentOfGrads != null && stateData.PercentOfGrads.Count > 0)
                {
                    var f = stateData.PercentOfGrads.FirstOrDefault(x => x.Item1 == OccidentalEdu.HighSchool);
                    if (f != null)
                    {
                        stateAvg = (int) Math.Round(f.Item2*100);
                    }
                }
            }
            
            var raceNatlAvg = new Dictionary<NorthAmericanRace, int>()
            {
                {NorthAmericanRace.White, 87 - natlAvg},
                {NorthAmericanRace.Black, 73 - natlAvg },
                {NorthAmericanRace.Hispanic, 76 - natlAvg },
                {NorthAmericanRace.Asian, 89 - natlAvg },
                {NorthAmericanRace.Pacific, 89 - natlAvg },
                {NorthAmericanRace.AmericanIndian, 70 - natlAvg },
                {NorthAmericanRace.Mixed, 0 }
            };
            if(!raceNatlAvg.ContainsKey(race))
                return stateAvg;

            return stateAvg + raceNatlAvg[race];
        }

        /// <summary>
        /// Sets <see cref="thisPerson"/> home-related data to the same values of <see cref="livesWithThisOne"/>
        /// </summary>
        /// <param name="thisPerson"></param>
        /// <param name="livesWithThisOne"></param>
        public static void SetNAmerCohabitants(NorthAmerican thisPerson, NorthAmerican livesWithThisOne)
        {
            if (thisPerson == null || livesWithThisOne == null)
                return;
            var addrMatchTo = livesWithThisOne.GetAddressAt(null);
            if (addrMatchTo == null)
                return;
            thisPerson.UpsertAddress(addrMatchTo);
            thisPerson._phoneNumbers.Clear();
            if (livesWithThisOne._phoneNumbers.Any(p => p.Item1 == KindsOfLabels.Home))
            {
                thisPerson._phoneNumbers.Add(livesWithThisOne._phoneNumbers.First(p => p.Item1 == KindsOfLabels.Home));
            }
            if (thisPerson.GetAgeAt(null) >= 12 && !String.IsNullOrWhiteSpace(addrMatchTo.HomeCityArea?.GetPostalCodePrefix()))
                thisPerson._phoneNumbers.Add(new Tuple<KindsOfLabels, NorthAmericanPhone>(KindsOfLabels.Mobile,
                    Phone.American(addrMatchTo.HomeCityArea.GetPostalCodePrefix())));
        }

        /// <summary>
        /// Selects a US Zip Code prefix at random taking into respect the population pertinent to that zip code prefix.
        /// </summary>
        public static string RandomAmericanZipWithRespectToPop()
        {
            XDocument usZips = null;
            var usZipsXmlDocument = TreeData.UsZipProbabilityTable;

            if (usZipsXmlDocument == null)
                return "100"; //New York

            usZips = usZipsXmlDocument.ToXDocument();

            double pickone = Convert.ToInt32(Etx.MyRand.Next(1, 9999999) / 100000);
            var randnode =
                usZips.Descendants("zip-code").FirstOrDefault(
                    x =>
                        Convert.ToDouble(x.Attribute("weight").Value) > pickone);
            if (randnode == null)
                return "100"; //New York

            return randnode.Attribute("prefix").Value;
        }

        /// <summary>
        /// Returns a hashtable whose keys as American's call Race based on the given <see cref="zipCode"/>
        /// </summary>
        /// <param name="zipCode"></param>
        public static AmericanRacePercents RandomAmericanRaceWithRespectToZip(string zipCode)
        {

            var pick = 0;
            //if calling assembly passed in no-args then return all zeros
            if (String.IsNullOrWhiteSpace(zipCode))
                return null;

            //get the data for the given zip code
            var zipStat = Data.TreeData.AmericanHighSchoolData.SelectSingleNode($"//zip-stat[@value='{zipCode}']");

            if (zipStat == null || !zipStat.HasChildNodes)
            {
                //try to find on the zip code prefix 
                var zip3 = zipCode.Substring(0, 3);
                var zipCodes =
                    Data.TreeData.AmericanHighSchoolData.SelectNodes($"//zip-code[@prefix='{zip3}']");

                if (zipCodes == null || zipCodes.Count <= 0)
                    return null;

                zipStat = zipCodes.Cast<XmlElement>().FirstOrDefault(x => x.HasChildNodes);
                if (zipStat == null)
                    return null;
            }

            AmericanHighSchool hsOut;
            pick = Etx.MyRand.Next(0, zipStat.ChildNodes.Count - 1);
            var hsNode = zipStat.ChildNodes[pick];
            if (!AmericanHighSchool.TryParseXml(hsNode as XmlElement, out hsOut))
                return null;
            return hsOut.RacePercents;

        }
    }
}
