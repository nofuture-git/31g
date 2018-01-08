using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Cc;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus;
using NoFuture.Rand.Domus.Opes;
using NoFuture.Rand.Domus.Pneuma;
using NoFuture.Rand.Gov.Fed;
using NoFuture.Rand.Gov.Nhtsa;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Data
{
    /// <summary>
    /// A factory for various values whose randomness is dependent of 
    /// Data contained within Rand.Data.Source
    /// </summary>
    public static class Facit
    {
        /// <summary>
        /// Gets a random Uri host.
        /// </summary>
        /// <returns></returns>
        public static string RandomUriHost(bool withSubDomain = true, bool usCommonOnly = false)
        {
            var webDomains = usCommonOnly ? ListData.UsWebmailDomains : ListData.WebmailDomains;
            var host = new StringBuilder();

            if (withSubDomain)
            {
                var subdomain = Etx.DiscreteRange(ListData.Subdomains);
                host.Append(subdomain + ".");
            }

            if (webDomains != null)
            {
                host.Append(webDomains[Etx.IntNumber(0, webDomains.Length - 1)]);
            }
            else
            {
                host.Append(Word());
                host.Append(Etx.DiscreteRange(new[] { ".com", ".net", ".edu", ".org" }));
            }
            return host.ToString();
        }

        /// <summary>
        /// Create a random http scheme uri with optional query string.
        /// </summary>
        /// <param name="useHttps"></param>
        /// <param name="addQry"></param>
        /// <returns></returns>
        public static Uri RandomHttpUri(bool useHttps = false, bool addQry = false)
        {

            var pathSeg = new List<string>();
            var pathSegLen = Etx.IntNumber(0, 5);
            for (var i = 0; i < pathSegLen; i++)
            {
                Etx.DiscreteRange(new Dictionary<string, double>()
                {
                    {Word(), 72},
                    {Etx.Consonant(false).ToString(), 11},
                    {Etx.IntNumber(1, 9999).ToString(), 17}
                });
                pathSeg.Add(Word());
            }

            if (Etx.CoinToss)
            {
                pathSeg.Add(Word() + Etx.DiscreteRange(new[] {".php", ".aspx", ".html", ".txt", ".asp"}));
            }

            var uri = new UriBuilder
            {
                Scheme = useHttps ? "https" : "http",
                Host = RandomUriHost(),
                Path = String.Join("/", pathSeg.ToArray())
            };

            if (!addQry)
                return uri.Uri;

            var qry = new List<string>();
            var qryParms = Etx.IntNumber(1, 5);
            for (var i = 0; i < qryParms; i++)
            {
                var len = Etx.IntNumber(1, 4);
                var qryParam = new List<string>();
                for (var j = 0; j < len; j++)
                {
                    if (Etx.CoinToss)
                    {
                        qryParam.Add(Word());
                        continue;
                    }
                    if (Etx.CoinToss)
                    {
                        qryParam.Add(Etx.IntNumber(0, 99999).ToString());
                        continue;
                    }
                    qryParam.Add(Etx.Consonant(Etx.CoinToss).ToString());

                }
                qry.Add(String.Join("_", qryParam) + "=" + Etx.SupriseMe());
            }

            uri.Query = String.Join("&", qry);
            return uri.Uri;
        }

        /// <summary>
        /// Creates a random email address 
        /// </summary>
        /// <returns></returns>
        public static string RandomEmailUri(string username = "", bool usCommonOnly = false)
        {
            var host = RandomUriHost(false, usCommonOnly);
            if (!String.IsNullOrWhiteSpace(username))
                return String.Join("@", username, host);
            var bunchOfWords = new List<string>();
            for (var i = 0; i < 4; i++)
            {
                bunchOfWords.Add(Etc.CapWords(Word(), ' '));
                bunchOfWords.Add(AmericanUtil.GetAmericanFirstName(DateTime.Today,
                    Etx.CoinToss ? Gender.Male : Gender.Female));
            }
            username = String.Join((Etx.CoinToss ? "." : "_"), Etx.DiscreteRange(bunchOfWords.ToArray()),
                Etx.DiscreteRange(bunchOfWords.ToArray()));
            return String.Join("@", username, host);
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
        /// Attempts to return a common english
        /// </summary>
        /// <returns></returns>
        public static string Word()
        {
            var enWords = TreeData.EnglishWords;
            if (enWords == null || enWords.Count <= 0)
                return Etx.Word(8);
            var pick = Etx.IntNumber(0, enWords.Count - 1);
            var enWord = enWords[pick]?.Item1;
            return !String.IsNullOrWhiteSpace(enWord)
                ? enWord
                : Etx.Word(8);
        }

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
            if (ccScore == null && p is NorthAmerican)
            {
                var northAmerican = (NorthAmerican) p;
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
                NorthAmericanFactors.GetRandomFactorValue(FactorTables.HomeDebt, homeDebtFactor,
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
                loan.PropertyId = Vin.GetRandomVin(remainingCost.ToDouble() <= 2000.0D, maxYear);
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
    }
}
