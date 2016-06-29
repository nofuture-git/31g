using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Xml;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.NfHtml;
using NoFuture.Rand.Data.NfHttp;
using NoFuture.Rand.Data.NfText;
using NoFuture.Rand.Data.NfXml;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Gov.Fed;
using NoFuture.Rand.Gov.Sec;
using NoFuture.Shared;
using NoFuture.Util;

namespace NoFuture.Rand
{
    /// <summary>
    /// Represents basic 'etc' like functions for generating random data.
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
    /// </summary>
    public static class Etx
    {
        #region Fields
        private static Random _myRand;
        #endregion

        #region Constants
        private const string US_ZIP_PROB_TABLE_DATA_PATH = @"Data\Source\US_Zip_ProbTable.xml";

        #endregion

        internal static Random MyRand
        {
            get { return _myRand ?? (_myRand = new Random(Convert.ToInt32(string.Format("{0:ffffff}", DateTime.Now)))); }
        }

        #region API
        /// <summary>
        /// Selects a US Zip Code prefix at random taking into respect the population pertinent to that zip code prefix.
        /// </summary>
        public static string RandomAmericanZipWithRespectToPop()
        {
            XDocument usZips = null;
            var usZipsXmlDocument = TreeData.UsZipProbabilityTable;

            if(usZipsXmlDocument == null)
                return "100"; //New York

            usZips = usZipsXmlDocument.ToXDocument();

            double pickone = Convert.ToInt32(MyRand.Next(1, 9999999) / 100000);
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
            if (string.IsNullOrWhiteSpace(zipCode))
                return null;

            //get the data for the given zip code
            var zipStat = Data.TreeData.AmericanHighSchoolData.SelectSingleNode($"//zip-stat[@value='{zipCode}']");

            if (zipStat == null || !zipStat.HasChildNodes)
            {
                //try to find on the zip code prefix 
                var zip3 = zipCode.Substring(0, 3);
                var zipCodes =
                    Data.TreeData.AmericanHighSchoolData.SelectNodes($"//zip-code[@prefix='{zip3}']");

                if (zipCodes == null || zipCodes.Count <=0)
                    return null;

                zipStat = zipCodes.Cast<XmlElement>().FirstOrDefault(x => x.HasChildNodes);
                if (zipStat == null)
                    return null;
            }

            AmericanHighSchool hsOut;
            pick = MyRand.Next(0, zipStat.ChildNodes.Count - 1);
            var hsNode = zipStat.ChildNodes[pick];
            if (!AmericanHighSchool.TryParseXml(hsNode as XmlElement, out hsOut))
                return null;
            return hsOut.RacePercents;

        }

        /// <summary>
        /// A fifty-fifty probability.
        /// </summary>
        public static bool CoinToss => MyRand.Next(0, int.MaxValue) % 2 == 0;

        /// <summary>
        /// Same as its counterpart <see cref="CoinToss"/>
        /// only returning 1 for true and -1 for false.
        /// </summary>
        public static int PlusOrMinusOne => CoinToss ? 1 : -1;

        /// <summary>
        /// Returns a random <see cref="Int32"/> between the given range.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static int IntNumber(int from, int to)
        {
            return MyRand.Next(from, to);
        }

        /// <summary>
        /// Returns a random <see cref="Double"/>
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static double RationalNumber(int from, int to)
        {
            var someInt = IntNumber(from, to);
            return (double) someInt + MyRand.NextDouble();
        }

        /// <summary>
        /// Get a random string of characters within ASCII (256) range.
        /// </summary>
        /// <param name="asciiStart"></param>
        /// <param name="asciiEnd"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Chars(int asciiStart, int asciiEnd, int length)
        {
            var str = new StringBuilder();
            for(var i = 0; i<length;i++)
            {
                str.Append(Convert.ToChar(MyRand.Next(asciiStart, asciiEnd)));
            }
            return str.ToString();
        }

        /// <summary>
        /// Generates a random string having only alpha characters in which the 
        /// first character is upper case while the rest are in lower case.
        /// </summary>
        /// <param name="length"></param>
        /// <remarks>
        /// The first character has a fifty-fifty chance of being 
        /// a consonant or vowel - thereafter the probability is 
        /// sixty-forty.
        /// </remarks>
        /// <returns></returns>
        public static string Word(int length)
        {
            var word = new StringBuilder();

            for(var i = 0; i< length; i++)
            {
                if (i == 0)
                    word.Append(CoinToss ? Vowel(true) : Consonant(true));
                if (MyRand.Next(1, 100) <= 60)
                    word.Append(Consonant(false));
                else
                    word.Append(Vowel(false));
            }

            return word.ToString();
        }

        /// <summary>
        /// Returns at random one of the five english vowels characters.
        /// Each vowel has equal probability.
        /// </summary>
        /// <param name="upperCase"></param>
        /// <returns></returns>
        public static char Vowel(bool upperCase)
        {
            var p = MyRand.Next(1, 5);
            char sa;
            switch (p)
            {
                case 1:
                    sa = 'a';
                    break;
                case 2:
                    sa = 'e';
                    break;
                case 3:
                    sa = 'i';
                    break;
                case 4:
                    sa = 'o';
                    break;
                case 5:
                    sa = 'u';
                    break;
                default:
                    sa = 'y';
                    break;
            }
            return upperCase ? Char.ToUpper(sa) : sa;
        }

        /// <summary>
        /// Returns at random one of the twenty-one english consonant characters.
        /// Each consonant has equal probability.
        /// </summary>
        /// <param name="upperCase"></param>
        /// <returns></returns>
        public static char Consonant(bool upperCase)
        {
            var p = MyRand.Next(1, 21);
            char sa;
            switch (p)
            {
                case 1:
                    sa = 'b';
                    break;
                case 2:
                    sa = 'c';
                    break;
                case 3:
                    sa = 'd';
                    break;
                case 4:
                    sa = 'f';
                    break;
                case 5:
                    sa = 'g';
                    break;
                case 6:
                    sa = 'h';
                    break;
                case 7:
                    sa = 'j';
                    break;
                case 8:
                    sa = 'k';
                    break;
                case 9:
                    sa = 'l';
                    break;
                case 10:
                    sa = 'm';
                    break;
                case 11:
                    sa = 'n';
                    break;
                case 12:
                    sa = 'p';
                    break;
                case 13:
                    sa = 'q';
                    break;
                case 14:
                    sa = 'r';
                    break;
                case 15:
                    sa = 's';
                    break;
                case 16:
                    sa = 't';
                    break;
                case 17:
                    sa = 'v';
                    break;
                case 18:
                    sa = 'w';
                    break;
                case 19:
                    sa = 'x';
                    break;
                case 20:
                    sa = 'y';
                    break;
                default:
                    sa = 'z';
                    break;
            }
            return upperCase ? Char.ToUpper(sa) : sa;
        }

        public static double ToDouble(this DateTime d)
        {
            return (d.Year + (d.DayOfYear/Constants.DBL_TROPICAL_YEAR));
        }

        /// <summary>
        /// Returns a random date near <see cref="plusOrMinusYears"/> years ago.
        /// </summary>
        /// <param name="plusOrMinusYears"></param>
        /// <param name="fromThisDate">Optional, will default to current system time</param>
        /// <param name="maxDaysSpread">
        /// Additional number of days to further randomize the date where the final date
        /// will be plus or minus one to <see cref="maxDaysSpread"/> days. Setting this to zero 
        /// will result in exactly <see cref="fromThisDate"/> plus <see cref="plusOrMinusYears"/> number of years.
        /// </param>
        /// <returns></returns>
        public static DateTime Date(int plusOrMinusYears, DateTime? fromThisDate, int maxDaysSpread = 360)
        {
            var dt = DateTime.Now;
            if (fromThisDate != null)
                dt = fromThisDate.Value;
            //plus or minus some random days
            var randomDaysNear = Etx.IntNumber(1, 360) * PlusOrMinusOne;
            return dt.AddYears(plusOrMinusYears).AddDays(randomDaysNear);
        }

        /// <summary>
        /// Gets a random value contrained to the normal distribution.
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="stdDev"></param>
        /// <returns></returns>
        public static double RandomValueInNormalDist(double mean, double stdDev)
        {
            var magnitude = GetMagnitudeAdjustment(mean);
            var posMean = Convert.ToInt32(Math.Round(mean * magnitude));
            var posStdDev = Convert.ToInt32(Math.Round(stdDev * magnitude));

            var minRand = posMean - (posStdDev*3);
            var maxRand = posMean + (posStdDev*3);

            //Random only works on positive ints
            if (minRand < 0 && maxRand < 0)
            {
                throw new NotImplementedException(
                    "Only supported for distributions whose entire range is positive values.");
            }

            var probTbl = Util.Math.NormalDistEquation.GetCumulativeZScore();

            var posProbTbl = probTbl.Keys.ToDictionary(p => p, p => Convert.ToInt32(probTbl[p]*10000));

            for (var i = 0; i < 1000; i++)
            {
                //guess some value w/i 3 std dev's
                var someValue = Etx.IntNumber(minRand, maxRand);

                //get the probability of it
                var z = (double)(someValue - posMean) / posStdDev;
                var zscore = Convert.ToInt32(posProbTbl.FirstOrDefault(x => x.Key >= Math.Abs(z)).Value);

                var attempt = Etx.IntNumber(1, 10000);
                //try getting a value with that probability
                var isGe = attempt >= zscore;

                //when succeded - return some value
                if (isGe)
                    return (double)someValue / magnitude;
            }
            return mean;
        }

        /// <summary>
        /// Produces an array of <see cref="Rchar"/> in a random order and random length.
        /// </summary>
        /// <returns>The min length is 5 and the max is 15 </returns>
        public static Rchar[] GetRandomRChars(bool numbersOnly = false)
        {
            var len = IntNumber(5, 15);
            var rcharsOut = new List<Rchar>();

            for (var i = 0; i <= len; i++)
            {
                if (numbersOnly)
                {
                    rcharsOut.Add(new NumericRchar(i));
                    continue;
                }

                var j = IntNumber(0, 6);
                switch (j)
                {
                    case 0:
                    case 1:
                        rcharsOut.Add(new AlphaNumericRchar(i));
                        break;
                    case 2:
                        rcharsOut.Add(new UAlphaRchar(i));
                        break;
                    case 3:
                        rcharsOut.Add(new LAlphaRchar(i));
                        break;
                    case 4:
                        rcharsOut.Add(new NumericRchar(i));
                        break;
                    case 5:
                        rcharsOut.Add(new LimitedRchar(i, Consonant(true), Consonant(true), Consonant(true),
                            Vowel(false)));
                        break;
                    case 6:
                        rcharsOut.Add( new LimitedRchar(i, '-','.',' '));
                        break;
                }
            }

            return rcharsOut.ToArray();
        }

        /// <summary>
        /// Factory method to get a concrete implementation of <see cref="INfDynData"/>
        /// based on a Uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static INfDynData DynamicDataFactory(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            if (uri.Host == "finance.yahoo.com")
            {
                if (uri.LocalPath == "/q")
                    return new YhooFinSymbolLookup(uri);
                if(uri.LocalPath == "/q/bs")
                    return new YhooFinBalanceSheet(uri);
                if (uri.LocalPath == "/q/is")
                    return new YhooFinIncomeStmt(uri);
            }
            else if (uri.Host == "www.bloomberg.com")
            {
                return new BloombergSymbolSearch(uri);
            }
            else if (uri.Host == Edgar.SEC_HOST)
            {
                if (uri.LocalPath == "/cgi-bin/srch-edgar")
                {
                    return new SecFullTxtSearch(uri);
                }
                if (uri.LocalPath == "/cgi-bin/browse-edgar" && uri.Query.StartsWith("?action=getcompany&CIK="))
                {
                    return new SecCikSearch(uri);
                }
            }
            else if (uri.Host == new Uri(FedLrgBnk.RELEASE_URL).Host)
            {
                return new FedLrgBnk();
            }
            else if (uri.Host == new Uri(Ffiec.SEARCH_URL_BASE).Host)
            {
                return new FfiecInstitProfile(uri);
            }

            throw new NotImplementedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static int GetMagnitudeAdjustment(double dbl)
        {
            var strDbl = dbl.ToString(CultureInfo.InvariantCulture);
            var numLen = strDbl.Length;

            var  magnitude = strDbl.IndexOf(".", StringComparison.Ordinal);

            if (numLen > Int32.MaxValue.ToString().Length)
                numLen = Int32.MaxValue.ToString().Length - 1;

            magnitude = numLen - magnitude;

            return Convert.ToInt32($"1{new string('0', magnitude)}");
        }
        #endregion
    }
}
