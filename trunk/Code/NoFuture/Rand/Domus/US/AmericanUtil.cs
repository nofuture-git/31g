using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Cc;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus.Opes;
using NoFuture.Rand.Domus.Pneuma;
using NoFuture.Rand.Domus.US;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.Fed;
using NoFuture.Rand.Gov.Nhtsa;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;

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
    public static class AmericanUtil
    {
        /*
         TODO static factories pulled from NoFuture.Rand.Data.Sp
         */

        public static IMereo GetMereoById(Identifier property, string prefix = null)
        {
            switch (property)
            {
                case null:
                    return new Mereo(prefix);
                case ResidentAddress residenceLoan:
                    return residenceLoan.IsLeased
                        ? new Mereo(String.Join(" ", prefix, "Rent Payment"))
                        : new Mereo(String.Join(" ", prefix, "Mortgage Payment"));
                case Vin _:
                    return new Mereo(String.Join(" ", prefix, "Vehicle Payment"));
                case CreditCardNumber _:
                    return new Mereo(String.Join(" ", prefix, "Cc Payment"));
                case AccountId _:
                    return new Mereo(String.Join(" ", prefix, "Bank Account Transfer"));
            }

            return new Mereo(prefix);
        }


        /// <summary>
        /// Randomly gen's one of the concrete types of <see cref="CreditCardAccount"/>.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ccScore">
        /// Optional, if given then will generate an interest-rate and cc-max 
        /// in accordance with the score.
        /// </param>
        /// <param name="baseInterestRate">
        /// This is the lowest possiable interest rate for the random generators
        /// </param>
        /// <param name="minPmtPercent">
        /// The value used to calc a minimum monthly payment
        /// </param>
        /// <returns></returns>
        public static CreditCardAccount GetRandomCcAcct(IPerson p, CreditScore ccScore,
            float baseInterestRate = 10.1F + RiskFreeInterestRate.DF_VALUE,
            float minPmtPercent = CreditCardAccount.DF_MIN_PMT_RATE)
        {
            if (ccScore == null && p is American)
            {
                var northAmerican = (American)p;
                ccScore = new PersonalCreditScore()
                {
                    GetAgeAt = northAmerican.GetAgeAt,
                    OpennessZscore = northAmerican?.Personality?.Openness?.Value?.Zscore ?? 0D,
                    ConscientiousnessZscore = northAmerican.Personality?.Conscientiousness?.Value?.Zscore ?? 0D
                };
            }

            var cc = GetRandomCreditCard(p);
            var max = ccScore == null ? new Pecuniam(1000) : ccScore.GetRandomMax(cc.CardHolderSince);
            var randRate = ccScore?.GetRandomInterestRate(cc.CardHolderSince, baseInterestRate) * 0.01 ?? baseInterestRate;
            var ccAcct = new CreditCardAccount(cc, minPmtPercent, max) { Rate = (float)randRate };
            return ccAcct;
        }


        /// <summary>
        /// Creates a random email address in a typical format
        /// </summary>
        /// <param name="names"></param>
        /// <param name="isProfessional">
        /// set this to true to have the username look unprofessional
        /// </param>
        /// <param name="usCommonOnly">
        /// true uses <see cref="ListData.UsWebmailDomains"/>
        /// false uses <see cref="ListData.WebmailDomains"/>
        /// </param>
        /// <returns></returns>
        public static string RandomEmailUri(string[] names, bool isProfessional = true, bool usCommonOnly = true)
        {
            if (names == null || !names.Any())
                return RandomEmailUri();

            //get childish username
            if (!isProfessional)
            {
                var shortWords = TreeData.EnglishWords.Where(x => x.Item1.Length <= 3).Select(x => x.Item1).ToArray();
                var shortWordList = new List<string>();
                for (var i = 0; i < 3; i++)
                {
                    var withUcase = Etc.CapWords(Etx.DiscreteRange(shortWords), ' ');
                    shortWordList.Add(withUcase);
                }
                shortWordList.Add((Etx.CoinToss ? "_" : "") + Etx.IntNumber(100, 9999));
                return RandomEmailUri(String.Join("", shortWordList), usCommonOnly);
            }

            var fname = names.First().ToLower();
            var lname = names.Last().ToLower();
            string mi = null;
            if (names.Length > 2)
            {
                mi = names[1].ToLower();
                mi = Etx.CoinToss ? mi.First().ToString() : mi;
            }

            var unParts = new List<string> { Etx.CoinToss ? fname : fname.First().ToString(), mi, lname };
            var totalLength = unParts.Sum(x => x.Length);
            if (totalLength <= 7)
                return RandomEmailUri(String.Join(Etx.CoinToss ? "" : "_", String.Join(Etx.CoinToss ? "." : "_", unParts),
                    Etx.IntNumber(100, 9999)), usCommonOnly);
            return RandomEmailUri(totalLength > 20
                ? String.Join(Etx.CoinToss ? "." : "_", unParts.Take(2))
                : String.Join(Etx.CoinToss ? "." : "_", unParts), usCommonOnly);
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
                return AmericanRacePercents.GetNatlAvg();

            //get the data for the given zip code
            var zipStatElem = TreeData.AmericanHighSchoolData.SelectSingleNode($"//{CityArea.ZIP_STAT}[@{CityArea.VALUE}='{zipCode}']");

            if (zipStatElem == null || !zipStatElem.HasChildNodes)
            {
                //try to find on the zip code prefix 
                var zip3 = zipCode.Substring(0, 3);
                var zipCodeElem =
                    TreeData.AmericanHighSchoolData.SelectSingleNode($"//{CityArea.ZIP_CODE_SINGULAR}[@{CityArea.PREFIX}='{zip3}']");

                if (zipCodeElem == null || !zipCodeElem.HasChildNodes)
                    return AmericanRacePercents.GetNatlAvg();

                pick = Etx.MyRand.Next(0, zipCodeElem.ChildNodes.Count - 1);

                zipStatElem = zipCodeElem.ChildNodes[pick];
                if (zipStatElem == null)
                    return AmericanRacePercents.GetNatlAvg();
            }

            pick = Etx.MyRand.Next(0, zipStatElem.ChildNodes.Count - 1);
            var hsNode = zipStatElem.ChildNodes[pick];
            if (!AmericanHighSchool.TryParseXml(hsNode as XmlElement, out var hsOut))
                return AmericanRacePercents.GetNatlAvg();
            return hsOut.RacePercents;
        }

        /// <summary>
        /// Creates a random email address 
        /// </summary>
        /// <returns></returns>
        public static string RandomEmailUri(string username = "", bool usCommonOnly = false)
        {
            var host = Facit.RandomUriHost(false, usCommonOnly);
            if (!String.IsNullOrWhiteSpace(username))
                return String.Join("@", username, host);
            var bunchOfWords = new List<string>();
            for (var i = 0; i < 4; i++)
            {
                bunchOfWords.Add(Etc.CapWords(Facit.Word(), ' '));
                bunchOfWords.Add(AmericanUtil.GetAmericanFirstName(DateTime.Today,
                    Etx.CoinToss ? Gender.Male : Gender.Female));
            }
            username = String.Join((Etx.CoinToss ? "." : "_"), Etx.DiscreteRange(bunchOfWords.ToArray()),
                Etx.DiscreteRange(bunchOfWords.ToArray()));
            return String.Join("@", username, host);
        }

        /// <summary>
        /// Returs a new, randomly gen'ed, concrete instance of <see cref="ICreditCard"/>
        /// </summary>
        /// <param name="p"></param>
        /// <param name="opennedDate"></param>
        /// <returns></returns>
        public static ICreditCard GetRandomCreditCard(IPerson p, DateTime? opennedDate = null)
        {
            var fk = Etx.IntNumber(0, 3);
            var dt = opennedDate ?? Etx.Date(-3, null);

            switch (fk)
            {
                case 0:
                    return new MasterCardCc(p, dt, dt.AddYears(3));
                case 2:
                    return new AmexCc(p, dt, dt.AddYears(3));
                case 3:
                    return new DiscoverCc(p, dt, dt.AddYears(3));
                default:
                    return new VisaCc(p, dt, dt.AddYears(3));
            }
        }


        /// <summary>
        /// Factory method to generate a <see cref="Rent"/> instance at random with a payment history.
        /// </summary>
        /// <param name="property">The property identifier on which is being leased</param>
        /// <param name="homeDebtFactor">The home debt factor based on the renter's age, gender, edu, etc.</param>
        /// <param name="renterPersonality">Optional, used when creating a history of payments.</param>
        /// <param name="stdDevAsPercent">Optional, the stdDev around the mean.</param>
        /// <returns></returns>
        public static Rent GetRandomRentWithHistory(Identifier property, double homeDebtFactor, Personality renterPersonality = null,
            double stdDevAsPercent = WealthBase.DF_STD_DEV_PERCENT)
        {
            //create a rent object
            renterPersonality = renterPersonality ?? new Personality();

            var rent = GetRandomRent(property, homeDebtFactor, stdDevAsPercent);
            var randDate = rent.SigningDate;
            var randRent = rent.MonthlyPmt;
            //create payment history until current
            var firstPmt = rent.GetMinPayment(randDate);
            rent.PayRent(randDate.AddDays(1), firstPmt, GetMereoById(property, "First Rent Payment"));

            var rentDueDate = randDate.Month == 12
                ? new DateTime(randDate.Year + 1, 1, 1)
                : new DateTime(randDate.Year, randDate.Month + 1, 1);

            while (rentDueDate < DateTime.Today)
            {
                var paidRentOn = rentDueDate;
                //move the date rent was paid to some late-date when person acts irresponsible
                if (renterPersonality.GetRandomActsIrresponsible())
                    paidRentOn = paidRentOn.AddDays(Etx.IntNumber(5, 15));

                rent.PayRent(paidRentOn, randRent, GetMereoById(rent.Id));
                rentDueDate = rentDueDate.AddMonths(1);
            }
            return rent;
        }

        /// <summary>
        /// Factory method to generate a <see cref="Rent"/> instance at random.
        /// </summary>
        /// <param name="property">The property identifier on which is being leased</param>
        /// <param name="homeDebtFactor">The home debt factor based on the renter's age, gender, edu, etc.</param>
        /// <param name="stdDevAsPercent">Optional, the stdDev around the mean.</param>
        /// <param name="totalYearlyRent">
        /// Optional, allows the calling assembly to specify this, default 
        /// is calculated from <see cref="Rent.GetAvgAmericanRentByYear"/>
        /// </param>
        /// <returns></returns>
        public static Rent GetRandomRent(Identifier property, double homeDebtFactor,
            double stdDevAsPercent = WealthBase.DF_STD_DEV_PERCENT, double? totalYearlyRent = null)
        {
            var avgRent = totalYearlyRent ?? (double)Rent.GetAvgAmericanRentByYear(null).Amount;
            var randRent = new Pecuniam(
                (decimal)
                AmericanFactors.GetRandomFactorValue(FactorTables.HomeDebt, homeDebtFactor,
                    stdDevAsPercent, avgRent));
            var randTerm = Etx.DiscreteRange(new[] { 24, 18, 12, 6 });
            var randDate = Etx.Date(0, DateTime.Today.AddDays(-2), true);
            var randDepositAmt = (int)Math.Round((randRent.Amount - randRent.Amount % 250) / 2);
            var randDeposit = new Pecuniam(randDepositAmt);

            var rent = new Rent(property, randDate, randTerm, randRent, randDeposit);
            return rent;
        }

        /// <summary>
        /// Same as its counterpart <see cref="GetRandomLoan"/> only it also produces a history of transactions.
        /// </summary>
        /// <param name="property">The property on which the loan is secured.</param>
        /// <param name="remainingCost">The balance which currently remains.</param>
        /// <param name="totalCost">
        /// The original value of the loan, the difference between 
        /// this and the <see cref="remainingCost"/> determines how much history is needed.
        /// </param>
        /// <param name="rate">The interest rate</param>
        /// <param name="termInYears"></param>
        /// <param name="minPmt"></param>
        /// <param name="borrowerPersonality">Optional, used when creating a history of payments.</param>
        /// <returns></returns>
        public static SecuredFixedRateLoan GetRandomLoanWithHistory(Identifier property, Pecuniam remainingCost,
            Pecuniam totalCost, float rate, int termInYears, out Pecuniam minPmt, IPersonality borrowerPersonality = null)
        {

            var loan = GetRandomLoan(property, remainingCost, totalCost, rate, termInYears, out minPmt);

            var pmtNote = GetMereoById(property);
            //makes the fake history more colorful
            borrowerPersonality = borrowerPersonality ?? new Personality();

            var dtIncrement = loan.Inception.AddMonths(1);
            while (loan.GetValueAt(dtIncrement) > remainingCost)
            {
                if (dtIncrement >= DateTime.Now)
                    break;
                var paidOnDate = dtIncrement;
                if (borrowerPersonality.GetRandomActsIrresponsible())
                    paidOnDate = paidOnDate.AddDays(Etx.IntNumber(5, 15));

                //is this the payoff
                var isPayoff = loan.GetValueAt(paidOnDate) <= minPmt;
                if (isPayoff)
                {
                    minPmt = loan.GetValueAt(paidOnDate);
                }

                loan.Push(paidOnDate, minPmt, pmtNote, Pecuniam.Zero);
                if (isPayoff)
                {
                    loan.Terminus = paidOnDate;
                    loan.Closure = ClosedCondition.ClosedWithZeroBalance;
                    break;
                }
                dtIncrement = dtIncrement.AddMonths(1);
            }

            //insure that the gen'ed history doesn't start before the year of make
            if (property is Vin)
            {
                var maxYear = loan.Inception.Year;
                loan.PropertyId = Facit.GetRandomVin(remainingCost.ToDouble() <= 2000.0D, maxYear);
            }

            return loan;
        }

        /// <summary>
        /// Produces a random <see cref="SecuredFixedRateLoan"/>.
        /// </summary>
        /// <param name="property">The property on which the loan is secured.</param>
        /// <param name="remainingCost">The balance which currently remains.</param>
        /// <param name="totalCost">
        /// The original value of the loan, the difference between 
        /// this and the <see cref="remainingCost"/> determines how far in the past the loan would
        /// have been openned.
        /// </param>
        /// <param name="rate">The interest rate</param>
        /// <param name="termInYears"></param>
        /// <param name="minPmt"></param>
        /// <returns></returns>
        public static SecuredFixedRateLoan GetRandomLoan(Identifier property, Pecuniam remainingCost,
            Pecuniam totalCost, float rate, int termInYears, out Pecuniam minPmt)
        {
            var isMortgage = property is ResidentAddress;

            //if no or nonsense values given, change to some default
            if (totalCost == null || totalCost < Pecuniam.Zero)
                totalCost = new Pecuniam(2000);
            if (remainingCost == null || remainingCost < Pecuniam.Zero)
                remainingCost = Pecuniam.Zero;

            //remaining must always be less than the total 
            if (remainingCost > totalCost)
                totalCost = remainingCost + Pecuniam.GetRandPecuniam(1000, 3000);

            //interest rate must be a positive number
            rate = Math.Abs(rate);

            //handle if caller passes in rate like 5.5 meaning they wanted 0.055
            if (rate > 1)
                rate = Convert.ToSingle(Math.Round(rate / 100, 4));

            //calc the monthly payment
            var fv = totalCost.Amount.PerDiemInterest(rate, Constants.TropicalYear.TotalDays * termInYears);
            minPmt = new Pecuniam(Math.Round(fv / (termInYears * 12), 2));
            var minPmtRate = fv == 0 ? CreditCardAccount.DF_MIN_PMT_RATE : (float)Math.Round(minPmt.Amount / fv, 6);

            //given this value and rate - calc the timespan needed to have aquired this amount of equity
            var firstOfYear = new DateTime(DateTime.Today.Year, 1, 1);
            var loan = new SecuredFixedRateLoan(property, firstOfYear, minPmtRate, totalCost)
            {
                Rate = rate
            };

            var dtIncrement = firstOfYear.AddMonths(1);
            while (loan.GetValueAt(dtIncrement) > remainingCost)
            {
                if (dtIncrement > DateTime.Now.AddYears(termInYears))
                    break;
                loan.Push(dtIncrement, minPmt);
                dtIncrement = dtIncrement.AddMonths(1);
            }

            //repeat process from calc'ed past date to create a history
            var calcPurchaseDt = DateTime.Today.AddDays(-1 * (dtIncrement - firstOfYear).Days);
            loan = isMortgage
                ? new Mortgage(property, calcPurchaseDt, rate, totalCost)
                : new SecuredFixedRateLoan(property, calcPurchaseDt, minPmtRate, totalCost)
                {
                    Rate = rate
                };

            loan.FormOfCredit = property is ResidentAddress
                ? FormOfCredit.Mortgage
                : FormOfCredit.Installment;

            return loan;
        }

        /// <summary>
        /// Creates a new random Checking Account
        /// </summary>
        /// <param name="p"></param>
        /// <param name="dt">Date account was openned, default to now.</param>
        /// <param name="debitPin">
        /// Optional, when present and random instance of <see cref="CheckingAccount.DebitCard"/> is created with 
        /// this as its PIN.
        /// </param>
        /// <returns></returns>
        public static CheckingAccount GetRandomCheckingAcct(IPerson p, DateTime? dt = null, string debitPin = null)
        {
            var dtd = dt.GetValueOrDefault(DateTime.Now);
            var accountId = new AccountId(Etx.GetRandomRChars(true));
            return CheckingAccount.IsPossiablePin(debitPin)
                ? new CheckingAccount(accountId, dtd,
                    new Tuple<ICreditCard, string>(GetRandomCreditCard(p), debitPin))
                : new CheckingAccount(accountId, dtd);
        }

        public static SavingsAccount GetRandomSavingAcct(IPerson p, DateTime? dt = null)
        {
            var dtd = dt.GetValueOrDefault(DateTime.Now);
            var accountId = new AccountId(Etx.GetRandomRChars(true));
            return new SavingsAccount(accountId, dtd);
        }

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

            var deathDate = AmericanUtil.GetDeathDate(p.BirthCert.DateOfBirth, p.MyGender);

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
        /// Gets a date of death based on the <see cref="Equations.LifeExpectancy"/>
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        public static DateTime GetDeathDate(DateTime dob, Gender gender)
        {
            var normDist = AmericanEquations.LifeExpectancy(gender);
            var ageAtDeath = Etx.RandomValueInNormalDist(normDist.Mean, normDist.StdDev);
            var years = (int)Math.Floor(ageAtDeath);
            var days = (int)Math.Round((ageAtDeath - years)*Constants.DBL_TROPICAL_YEAR);

            var deathDate =
                dob.AddYears(years)
                    .AddDays(days)
                    .AddHours(Etx.IntNumber(0, 12))
                    .AddMinutes(Etx.IntNumber(0, 59))
                    .AddSeconds(Etx.IntNumber(0, 59));
            return deathDate;
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
        /// Return a <see cref="NorthAmericanRace"/> randomly with weight based on <see cref="zipCode"/>.
        /// </summary>
        /// <param name="zipCode">Null to get natl averages.</param>
        /// <returns>
        /// Defaults to randomly to national averages
        /// [http://kff.org/other/state-indicator/distribution-by-raceethnicity/]
        /// </returns>
        public static NorthAmericanRace GetAmericanRace(string zipCode)
        {
            var amRace = RandomAmericanRaceWithRespectToZip(zipCode);

            var raceHashByZip = amRace != null
                ? new Dictionary<string, double>
                {
                    {Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.AmericanIndian), amRace.AmericanIndian},
                    {Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.Asian), amRace.Asian},
                    {Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.Black), amRace.Black},
                    {Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.Hispanic), amRace.Hispanic},
                    {Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.Mixed), amRace.Mixed},
                    {Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.Pacific), amRace.Pacific},
                    {Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.White), amRace.White}
                }
                : AmericanRacePercents.GetNatlAvgAsDict();

            var randPick = Etx.DiscreteRange(raceHashByZip);

            Enum.TryParse(randPick, out NorthAmericanRace pickOut);

            return pickOut;

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

            var aParent = new American(parentDob, gender, false, false);
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
