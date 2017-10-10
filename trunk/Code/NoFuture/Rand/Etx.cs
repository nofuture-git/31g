using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.NfHtml;
using NoFuture.Rand.Data.NfText;
using NoFuture.Rand.Data.NfXml;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Gov.Fed;
using NoFuture.Rand.Gov.Sec;
using NoFuture.Shared;

namespace NoFuture.Rand
{
    /// <summary>
    /// Represents basic 'etc' like functions for generating random data.
    /// </summary>
    public static class Etx
    {
        #region Fields
        private static Random _myRand;
        #endregion

        internal static Random MyRand
            => _myRand ?? (_myRand = new Random(Convert.ToInt32(string.Format("{0:ffffff}", DateTime.Now))));

        #region API

        public enum Dice
        {
            Four,
            Six,
            Eight,
            Ten,
            Twelve,
            Twenty,
            OneHundred,
            OneThousand
        }

        /// <summary>
        /// Helper method for common random tests
        /// </summary>
        /// <param name="v"></param>
        /// <param name="die"></param>
        /// <returns> </returns>
        public static bool TryAboveOrAt(int v, Dice die)
        {
            Func<int, int, bool> op = (i, i1) => i >= i1;
            return TryRoll(v, die, op);
        }

        /// <summary>
        /// Helper method for common random tests
        /// </summary>
        /// <param name="v"></param>
        /// <param name="die"></param>
        /// <returns></returns>
        public static bool TryBelowOrAt(int v, Dice die)
        {
            Func<int, int, bool> op = (i, i1) => i <= i1;
            return TryRoll(v, die, op);
        }

        /// <summary>
        /// A fifty-fifty probability.
        /// </summary>
        public static bool CoinToss => MyRand.Next(0, int.MaxValue)%2 == 0;

        /// <summary>
        /// Same as its counterpart <see cref="CoinToss"/>
        /// only returning 1 for true and -1 for false.
        /// </summary>
        public static int PlusOrMinusOne => CoinToss ? 1 : -1;

        /// <summary>
        /// Returns a random <see cref="Int32"/> between the given range
        /// including the range values themselves.
        /// </summary>
        /// <param name="from">Inclusive</param>
        /// <param name="to">Inclusive</param>
        /// <returns></returns>
        public static int IntNumber(int from, int to)
        {
            if (from == to)
                return from;
            if (from <= to)
                return MyRand.Next(from, to + 1);

            //passed in backwards is ok
            var t = from;
            from = to;
            to = t;

            return MyRand.Next(from, to + 1);
        }

        /// <summary>
        /// Returns a random <see cref="Double"/>
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static double RationalNumber(int from, int to)
        {
            var someInt = IntNumber(from, to-1);
            return someInt + MyRand.NextDouble();
        }

        /// <summary>
        /// Returns a square matrix of dimension size <see cref="dim"/>
        /// have all random, less-than 1, double values.
        /// </summary>
        /// <param name="dim"></param>
        /// <returns></returns>
        public static double[,] Matrix(int dim)
        {

            var m = new double[dim, dim];
            for (var i = 0; i < dim; i++)
            {
                for (var j = 0; j < dim; j++)
                {
                    m[i, j] = MyRand.NextDouble();
                }
            }
            return m;
        }

        /// <summary>
        /// Picks a key at random from <see cref="tbl"/> where the 
        /// probablity of each is the value over the sum-of-values.
        /// </summary>
        /// <param name="tbl"></param>
        /// <returns></returns>
        public static string DiscreteRange(Dictionary<string, double> tbl)
        {
            if (tbl == null)
                return null;
            if (tbl.Count == 1)
                return tbl.Keys.First();

            var sum = tbl.Values.Sum();

            var isSumAsOne = sum < 0.99 && sum > 1.01;

            //move the dictionary into an increasing range 
            var tblCopy = new Dictionary<string, double>();
            var runningSum = 0.0D;
            foreach (var k in tbl.Keys)
            {
                var val = isSumAsOne ? tbl[k] : tbl[k]/sum;
                tblCopy[k] = val + runningSum;
                runningSum += val;
            }

            var pick = MyRand.NextDouble();

            return tblCopy.FirstOrDefault(x => x.Value >= pick).Key;
        }

        /// <summary>
        /// Picks at random from a discrete list - its 
        /// assumed that each is equal prob.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string DiscreteRange(string[] list)
        {
            if (list == null || list.Length < 0)
                return null;
            if (list.Length == 1)
                return list[0];
            var pick = IntNumber(0, list.Length - 1);
            return list[pick];
        }

        /// <summary>
        /// Gets a random Uri host.
        /// </summary>
        /// <returns></returns>
        public static string RandomUriHost(bool withSubDomain = true)
        {
            var webDomains = ListData.UsWebmailDomains;
            var host = new StringBuilder();

            if (withSubDomain)
            {
                var subdomain = DiscreteRange(ListData.Subdomains);
                host.Append(subdomain + ".");
            }

            if (webDomains != null)
            {
                host.Append(webDomains[IntNumber(0, webDomains.Length - 1)]);
            }
            else
            {
                host.Append(Word());
                host.Append(DiscreteRange(new[] { ".com", ".net", ".edu", ".org" }));
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
            var pathSegLen = IntNumber(0, 5);
            for (var i = 0; i < pathSegLen; i++)
            {
                DiscreteRange(new Dictionary<string, double>()
                {
                    {Word(), 72},
                    {Consonant(false).ToString(), 11},
                    {IntNumber(1, 9999).ToString(), 17}
                });
                pathSeg.Add(Word());
            }

            if (CoinToss)
            {
                pathSeg.Add(Word() + DiscreteRange(new [] { ".php",".aspx",".html",".txt", ".asp"}));
            }

            var uri = new UriBuilder
            {
                Scheme = useHttps ? "https" : "http",
                Host = RandomUriHost(),
                Path =  string.Join("/", pathSeg.ToArray())
            };

            if (!addQry)
                return uri.Uri;

            var qry = new List<string>();
            var qryParms = IntNumber(1, 5);
            for (var i = 0; i < qryParms; i++)
            {
                var len = IntNumber(1, 4);
                var qryParam = new List<string>();
                for (var j = 0; j < len; j++)
                {
                    if (CoinToss)
                    {
                        qryParam.Add(Word());
                        continue;
                    }
                    if (CoinToss)
                    {
                        qryParam.Add(IntNumber(0,99999).ToString());
                        continue;
                    }
                    qryParam.Add(Consonant(CoinToss).ToString());

                }
                qry.Add(string.Join("_", qryParam) + "=" + SupriseMe());
            }

            uri.Query = string.Join("&", qry);
            return uri.Uri;
        }

        /// <summary>
        /// Returns some string taking various forms from the other
        /// random functions herein
        /// </summary>
        /// <returns></returns>
        public static string SupriseMe()
        {
            var pick = IntNumber(0, 10);
            switch (pick)
            {
                case 0:
                    return Guid.NewGuid().ToString();
                case 1:
                    return IntNumber(0, 99999).ToString();
                case 2:
                    return Convert.ToBase64String(Encoding.UTF8.GetBytes(Path.GetRandomFileName()));
                case 3:
                    return Word(IntNumber(5, 10));
                case 4:
                    return Consonant(CoinToss).ToString();
                case 5:
                    return Vowel(CoinToss).ToString();
                case 6:
                    return JsonConvert.SerializeObject(Etx.Date(-1*IntNumber(0, 4), null));
                default:
                    return Word();

            }
        }

        /// <summary>
        /// Creates a random email address 
        /// </summary>
        /// <returns></returns>
        public static string RandomEmailUri(string username = "")
        {
            var host = RandomUriHost(false);
            if (!string.IsNullOrWhiteSpace(username))
                return string.Join("@", username, host);
            var bunchOfWords = new List<string>();
            for (var i = 0; i < 4; i++)
            {
                bunchOfWords.Add(Util.Etc.CapWords(Word(), ' '));
                bunchOfWords.Add(Domus.NAmerUtil.GetAmericanFirstName(DateTime.Today, CoinToss ? Gender.Male : Gender.Female));
            }
            username = string.Join((CoinToss ? "." : "_"), DiscreteRange(bunchOfWords.ToArray()),
                DiscreteRange(bunchOfWords.ToArray()));
            return string.Join("@", username, host);
        }

        /// <summary>
        /// Creates a random email address in a typical format
        /// </summary>
        /// <param name="names"></param>
        /// <param name="isProfessional">
        /// set this to true to have the username look unprofessional
        /// </param>
        /// <returns></returns>
        public static string RandomEmailUri(string[] names, bool isProfessional = true)
        {
            if(names == null || !names.Any())
                return RandomEmailUri();

            //get childish username
            if (!isProfessional)
            {
                var shortWords = TreeData.EnglishWords.Where(x => x.Item1.Length <= 3).Select(x => x.Item1).ToArray();
                var shortWordList = new List<string>();
                for (var i = 0; i < 3; i++)
                {
                    var withUcase = Util.Etc.CapWords(DiscreteRange(shortWords), ' ');
                    shortWordList.Add(withUcase);
                }
                shortWordList.Add((CoinToss ? "_" : "") + IntNumber(100, 9999));
                return RandomEmailUri(string.Join("", shortWordList));
            }

            var fname = names.First().ToLower();
            var lname = names.Last().ToLower();
            string mi = null;
            if (names.Length > 2)
            {
                mi = names[1].ToLower();
                mi = CoinToss ? mi.First().ToString() : mi;
            }

            var unParts = new List<string> {CoinToss ? fname : fname.First().ToString(), mi, lname};
            var totalLength = unParts.Sum(x => x.Length);
            if (totalLength <= 7)
                return RandomEmailUri(string.Join(CoinToss ? "" : "_", string.Join(CoinToss ? "." : "_", unParts),
                    IntNumber(100, 9999)));
            return
                RandomEmailUri(totalLength > 20
                    ? string.Join(CoinToss ? "." : "_", unParts.Take(2))
                    : string.Join(CoinToss ? "." : "_", unParts));
        }

        /// <summary>
        /// Fisher-Yates random shuffle
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="vals"></param>
        /// <returns></returns>
        public static T[] RandShuffle<T>(T[] vals)
        {
            if (vals == null || !vals.Any())
                return vals;
            var arr = new T[vals.Length];
            Array.Copy(vals, arr, vals.Length);
            for (var i = vals.Length - 1; i > 1; i--)
            {
                var temp = arr[i];
                var j = MyRand.Next(0, i+1);
                arr[i] = arr[j];
                arr[j] = temp;
            }
            return arr;
        }

        /// <summary>
        /// Produces an array of percents whose sum is 1 
        /// where the number of percents therein is <see cref="numOfDiv"/>
        /// </summary>
        /// <param name="numOfDiv"></param>
        /// <returns></returns>
        public static double[] RandomPortions(int numOfDiv)
        {
            numOfDiv = Math.Abs(numOfDiv);
            var df = new[] {1.0D};
            if (numOfDiv == 0 || numOfDiv == 1)
                return df;

            var someNums = new List<int>();
            for (var i = 0; i < numOfDiv; i++)
                someNums.Add(IntNumber(1,99999));

            return someNums.Select(num => Math.Round((double) num/someNums.Sum(), 8)).ToArray();
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
            for (var i = 0; i < length; i++)
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

            for (var i = 0; i < length; i++)
            {
                if (i == 0)
                {
                    word.Append(CoinToss ? Vowel(true) : Consonant(true));
                    continue;
                }
                word.Append(MyRand.Next(1, 100) <= 60 ? Consonant(false) : Vowel(false));
            }

            return word.ToString();
        }

        /// <summary>
        /// Attempts to return a valid english word using 
        /// 'English_Words.xml' at <see cref="BinDirectories.DataRoot"/>,
        /// failing that defaults back to its overload.
        /// </summary>
        /// <returns></returns>
        public static string Word()
        {
            var enWords = TreeData.EnglishWords;
            if (enWords == null || enWords.Count <= 0)
                return Word(8);
            var pick = IntNumber(0, enWords.Count - 1);
            var enWord = enWords[pick]?.Item1;
            return !string.IsNullOrWhiteSpace(enWord)
                ? enWord
                : Word(8);
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

        /// <summary>
        /// Returns a date as a rational number (e.g. 2016.684476658052) 
        /// where day of year is divided by <see cref="Constants.DBL_TROPICAL_YEAR"/>
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
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
            var randomDaysNear = Etx.IntNumber(1, maxDaysSpread) *PlusOrMinusOne;
            return dt.AddYears(plusOrMinusYears).AddDays(randomDaysNear);
        }

        /// <summary>
        /// Gets a random value contrained to the normal distribution.
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="stdDev"></param>
        /// <param name="sigma">The z-score table only goes up to the 3rd sigma</param>
        /// <returns></returns>
        public static double RandomValueInNormalDist(double mean, double stdDev, int sigma = 3)
        {
            var minRand = mean - (stdDev * sigma);
            var maxRand = mean + (stdDev * sigma);

            if(minRand < int.MinValue || maxRand > int.MaxValue)
                throw new ArgumentException("The random number generator is limited to int max 2^31 value.");

            var eq = new Util.Math.NormalDistEquation {Mean = mean, StdDev = stdDev};

            for (var i = 0; i < 1024; i++)
            {
                //guess some value w/i 3 std dev's
                var someValue = Etx.RationalNumber(Convert.ToInt32(minRand), Convert.ToInt32(maxRand));

                //get the probability of that guess
                var zscore = eq.GetZScoreFor(someValue);

                //zscore
                var attempt = Etx.RationalNumber(0, 5)*0.1;

                //try getting a value with that probability
                var isGe = attempt >= zscore;

                //when succeded - return some value
                if (isGe)
                    return someValue;
            }
            return mean;
        }

        /// <summary>
        /// Produces an array of <see cref="Rchar"/> in a random order and random length.
        /// </summary>
        /// <returns>The min length is 5 and the max is 15 </returns>
        public static Rchar[] GetRandomRChars(bool numbersOnly = false, int? exactLen = null, int startAtIdx = 0)
        {
            var len = exactLen.GetValueOrDefault(IntNumber(5, 15));
            var rcharsOut = new List<Rchar>();

            for (var i = startAtIdx; i < len + startAtIdx; i++)
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
                        rcharsOut.Add(new LimitedRchar(i, Consonant(true), Consonant(true), Consonant(true), Vowel(false)));
                        break;
                    case 6:
                        rcharsOut.Add(new LimitedRchar(i, '-', '.', ' '));
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

            if (uri.Host == "www.bloomberg.com")
            {
                return new BloombergSymbolSearch(uri);
            }
            if (uri.Host == Edgar.SEC_HOST)
            {
                if (uri.LocalPath == "/cgi-bin/srch-edgar")
                {
                    return new SecFullTxtSearch(uri);
                }
                if (uri.LocalPath == "/cgi-bin/browse-edgar" && uri.Query.StartsWith("?action=getcompany&CIK="))
                {
                    return new SecCikSearch(uri);
                }
                if (uri.LocalPath.StartsWith("/Archives/edgar/data"))
                {
                    if(uri.LocalPath.EndsWith("index.htm"))
                        return new SecGetXbrlUri(uri);
                    if(uri.LocalPath.EndsWith(".xml"))
                        return new SecXbrlInstanceFile(uri);
                }
            }
            if (uri.Host == new Uri(FedLrgBnk.RELEASE_URL).Host)
            {
                return new FedLrgBnk();
            }
            if (uri.Host == new Uri(Ffiec.SEARCH_URL_BASE).Host)
            {
                return new FfiecInstitProfile(uri);
            }

            throw new NotImplementedException();
        }
        #endregion

        [EditorBrowsable(EditorBrowsableState.Never)]
        private static bool TryRoll(int v, Dice die, Func<int, int, bool> op)
        {
            if (v <= 0)
                return false;
            switch (die)
            {
                case Dice.Four:
                    return v > 4 || op(IntNumber(1, 4), v);
                case Dice.Six:
                    return v > 6 || op(IntNumber(1, 6), v);
                case Dice.Eight:
                    return v > 8 || op(IntNumber(1, 8), v);
                case Dice.Ten:
                    return v > 10 || op(IntNumber(1, 10), v);
                case Dice.Twelve:
                    return v > 12 || op(IntNumber(1, 12), v);
                case Dice.Twenty:
                    return v > 20 || op(IntNumber(1, 20), v);
                case Dice.OneHundred:
                    return v > 100 || op(IntNumber(1, 100), v);
                case Dice.OneThousand:
                    return v > 1000 || op(IntNumber(1, 1000), v);
                default:
                    throw new ArgumentOutOfRangeException(nameof(die), die, @"No implementation for this dice");
            }
        }

    }
}
