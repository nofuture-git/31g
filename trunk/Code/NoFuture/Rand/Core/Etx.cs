using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Core
{
    /// <summary>
    /// Represents basic 'etc' like functions for generating random data.
    /// </summary>
    public static class Etx
    {
        #region Fields
        private static Random _myRand;
        private static List<Tuple<string, double>> _enWords;
        internal const string ENGLISH_WORDS = "English_Words.xml";
        internal static XmlDocument EnWordsXml;
        #endregion

        public static Random MyRand
            => _myRand ?? (_myRand = new Random(Convert.ToInt32(String.Format("{0:ffffff}", DateTime.Now))));

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
        public static bool CoinToss => MyRand.Next(0, Int32.MaxValue)%2 == 0;

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
            var someInt = IntNumber(@from, to-1);
            return someInt + MyRand.NextDouble();
        }

        /// <summary>
        /// Returns a random <see cref="Double"/>
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static double RationalNumber(double from, double to)
        {
            const int FACTOR = 1000000;
            
            var fromWholeNum = Convert.ToInt32(Math.Truncate(@from));
            var toWholeNum = Convert.ToInt32(Math.Truncate(to));

            if (fromWholeNum > 0 && toWholeNum > 0)
                return RationalNumber(Convert.ToInt32(@from), Convert.ToInt32(to));

            var fromRationNum = Convert.ToInt32((@from - fromWholeNum)* FACTOR);
            var toRationNum = Convert.ToInt32((to - toWholeNum) * FACTOR);

            var rimex = IntNumber(fromRationNum, toRationNum);

            return Math.Round((double)rimex / FACTOR, 8);
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
        public static T DiscreteRange<T>(Dictionary<T, double> tbl)
        {
            if (tbl == null)
                return default(T);
            if (tbl.Count == 1)
                return tbl.Keys.First();

            var sum = tbl.Values.Sum();

            var isSumAsOne = sum < 0.99 && sum > 1.01;

            //move the dictionary into an increasing range 
            var tblCopy = new Dictionary<T, double>();
            var runningSum = 0.0D;
            foreach (var kt in tbl.Keys)
            {
                if (Equals(kt, null))
                    continue;
                
                var val = isSumAsOne ? tbl[kt] : tbl[kt]/sum;
                tblCopy[kt] = val + runningSum;
                runningSum += val;
            }

            var pick = MyRand.NextDouble();

            tblCopy = tblCopy.OrderBy(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            return tblCopy.FirstOrDefault(x => x.Value >= pick).Key;
        }

        /// <summary>
        /// Picks at random from a discrete list - its 
        /// assumed that each is equal prob.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T DiscreteRange<T>(T[] list)
        {
            if (list == null || list.Length < 0)
                return default(T);
            if (list.Length == 1)
                return list[0];
            var pick = IntNumber(0, list.Length - 1);
            return list[pick];
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
                default:
                    return Vowel(CoinToss).ToString();

            }
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
        /// Like its counterpart <see cref="RandomPortions"/> being ordered and 
        /// having an exponential drop-off within the first 3 to 4 entries.
        /// </summary>
        /// <param name="numOfDiv"></param>
        /// <param name="derivativeSlope">
        /// Controls &apos;speed&apos; of the exponential drop-off - the closer to zero
        /// it gets the faster it drops off
        /// </param>
        /// <returns></returns>
        public static double[] DiminishingPortions(int numOfDiv, double derivativeSlope = -1.0D)
        {
            numOfDiv = Math.Abs(numOfDiv);
            if (numOfDiv == 0 || numOfDiv == 1)
                return new[] { 1.0D };

            //make sure this is always negative despite what the calling assembly gave
            derivativeSlope = Math.Abs(derivativeSlope) * -1;

            var someNums = new List<int>();
            for (var i = 0; i < numOfDiv; i++)
            {
                var uDenom = Math.Abs(i - derivativeSlope);
                var lDenom = Math.Abs(i - derivativeSlope) * 2;
                var u = (int) Math.Round(99999D / uDenom);
                var l = (int) Math.Round(99999D / lDenom);

                someNums.Add(IntNumber(l,u));
            }

            var listOut = someNums.Select(num => Math.Round((double)num / someNums.Sum(), 8)).ToList();

            listOut.Sort();
            listOut.Reverse();

            return listOut.ToArray();
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
        /// Returns a random date near <see cref="plusOrMinusYears"/> years ago.
        /// </summary>
        /// <param name="plusOrMinusYears"></param>
        /// <param name="fromThisDate">Optional, will default to current system time</param>
        /// <param name="forceInPast"
        /// >Optional switch to force the random date to only occur in the past from <see cref="fromThisDate"/>
        /// </param>
        /// <param name="maxDaysSpread">
        /// Additional number of days to further randomize the date where the final date
        /// will be plus or minus one to <see cref="maxDaysSpread"/> days. Setting this to zero 
        /// will result in exactly <see cref="fromThisDate"/> plus <see cref="plusOrMinusYears"/> number of years.
        /// </param>
        /// <returns></returns>
        public static DateTime Date(int plusOrMinusYears, DateTime? fromThisDate, bool forceInPast = false, int maxDaysSpread = 360)
        {
            var dt = DateTime.Now;
            if (fromThisDate != null)
                dt = fromThisDate.Value;
            //plus or minus some random days
            var pom = forceInPast ? -1 : PlusOrMinusOne;
            var randomDaysNear = Etx.IntNumber(1, maxDaysSpread) * pom;
            return dt.AddYears(plusOrMinusYears).AddDays(randomDaysNear);
        }

        /// <summary>
        /// Gets a random value contrained to the normal distribution.
        /// </summary>
        /// <param name="eq"></param>
        /// <param name="sigma">The z-score table only goes up to the 3rd sigma</param>
        /// <returns></returns>
        public static double RandomValueInNormalDist(NormalDistEquation eq, int sigma = 3)
        {
           if(eq == null)
                throw new ArgumentNullException(nameof(eq));

            var minRand = eq.Mean - (eq.StdDev * sigma);
            var maxRand = eq.Mean + (eq.StdDev * sigma);

            if (minRand < Int32.MinValue || maxRand > Int32.MaxValue)
                throw new ArgumentException("The random number generator is limited to int max 2^31 value.");

            for (var i = 0; i < 1024; i++)
            {
                //guess some value w/i 3 std dev's
                var someValue = Etx.RationalNumber(minRand, maxRand);

                //get the probability of that guess
                var zscore = eq.GetZScoreFor(someValue);

                //zscore
                var attempt = Etx.RationalNumber(0, 5) * 0.1;

                //try getting a value with that probability
                var isGe = attempt >= zscore;

                //when succeded - return some value
                if (isGe)
                    return someValue;
            }
            return eq.Mean;
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
           return RandomValueInNormalDist(new NormalDistEquation { Mean = mean, StdDev = stdDev }, sigma);
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
                    rcharsOut.Add(new RcharNumeric(i));
                    continue;
                }

                var j = IntNumber(0, 6);
                switch (j)
                {
                    case 0:
                    case 1:
                        rcharsOut.Add(new RcharAlphaNumeric(i));
                        break;
                    case 2:
                        rcharsOut.Add(new RcharUAlpha(i));
                        break;
                    case 3:
                        rcharsOut.Add(new RcharLAlpha(i));
                        break;
                    case 4:
                        rcharsOut.Add(new RcharNumeric(i));
                        break;
                    case 5:
                        rcharsOut.Add(new RcharLimited(i, Consonant(true), Consonant(true), Consonant(true), Vowel(false)));
                        break;
                    case 6:
                        rcharsOut.Add(new RcharLimited(i, '-', '.', ' '));
                        break;
                }
            }

            return rcharsOut.ToArray();
        }

        /// <summary>
        /// Attempts to return a common english
        /// </summary>
        /// <returns></returns>
        public static string Word()
        {
            var enWords = EnglishWords;
            if (enWords == null || enWords.Count <= 0)
                return Etx.Word(8);
            var pick = Etx.IntNumber(0, enWords.Count - 1);
            var enWord = enWords[pick]?.Item1;
            return !String.IsNullOrWhiteSpace(enWord)
                ? enWord
                : Etx.Word(8);
        }

        /// <summary>
        /// Loads the <see cref="DataFiles.ENGLISH_WORDS_DATA_FILE"/> data into a <see cref="XmlDocument"/> document.
        /// </summary>
        public static List<Tuple<string, double>> EnglishWords
        {
            get
            {
                const string WORD = "word";
                const string LANG = "lang";
                const string COUNT = "count";
                try
                {
                    if (_enWords != null && _enWords.Count > 0)
                        return _enWords;

                    EnWordsXml = EnWordsXml ?? XmlDocXrefIdentifier.GetEmbeddedXmlDoc(ENGLISH_WORDS,
                        Assembly.GetExecutingAssembly());
                    var wordNodes = EnWordsXml?.SelectNodes($"//{WORD}[@{LANG}='en']");
                    if (wordNodes == null)
                        return null;

                    _enWords = new List<Tuple<string, double>>();
                    foreach (var node in wordNodes)
                    {
                        var elem = node as XmlElement;
                        var word = elem?.InnerText;
                        if (String.IsNullOrWhiteSpace(word))
                            continue;
                        var countStr = elem.Attributes[COUNT]?.Value;
                        if (String.IsNullOrWhiteSpace(countStr))
                            continue;
                        if (!Double.TryParse(countStr, out var count))
                            continue;
                        _enWords.Add(new Tuple<string, double>(word, count));
                    }
                    return _enWords;
                }
                catch (Exception ex)//keep this contained
                {
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine(ex.StackTrace);
                }
                return null;
            }
        }

        /// <summary>
        /// Returns a date being between <see cref="min"/> years ago today back to <see cref="max"/> years ago today.
        /// </summary>
        /// <remarks>
        /// The age is limited to min,max of 18,67 - generate with family to get other age sets
        /// </remarks>
        public static DateTime GetWorkingAdultBirthDate(int min = 21, int max = 67, int ageOfAdult = 18)
        {
            if (ageOfAdult <= 14)
                ageOfAdult = 18;

            if (min < ageOfAdult)
                min = ageOfAdult;
            if (max > 67)
                max = 67;
            return DateTime.Now.AddYears(-1 * Etx.MyRand.Next(min, max)).AddDays(Etx.IntNumber(1, 360));
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
