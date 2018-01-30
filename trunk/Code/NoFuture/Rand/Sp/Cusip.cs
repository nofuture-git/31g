using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// Commitee on Uniform Securities Identification Procedures.
    /// A unique id for a financial security.
    /// Ref [https://en.wikipedia.org/wiki/CUSIP]
    /// Lists at [https://www.sec.gov/divisions/investment/13flists.htm]
    /// </summary>
    [Serializable]
    public class Cusip : RIdentifierWithChkDigit
    {
        #region constants
        public override string Abbrev => "CUSIP";
        public const string DF_ISSUE = "10";
        #endregion

        #region fields
        private static Dictionary<char, int> _letter2num = new Dictionary<char, int>();
        private static Dictionary<char, int> _num2num = new Dictionary<char, int>();

        #endregion

        #region ctor
        public Cusip()
        {
            CheckDigitFunc = CusipChkDigit;
            //the unique first chars from https://www.sec.gov/divisions/investment/13f/13flist2016q2.pdf
            var rchars = new List<Rchar>
            {
                new RcharLimited(0, '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                    'B', 'C', 'D', 'E', 'F', 'G', 'H', 'L', 'M', 'N', 'P', 'Q', 'U', 'V', 'Y')
            };
            for(var i = 1; i < 8; i++)
                rchars.Add( new RcharAlphaNumeric(i));
            format = rchars.ToArray();
        }
        #endregion

        #region properties

        public SecurityIssueGroup IssueGroup
            =>
                !string.IsNullOrWhiteSpace(Issue) && Issue.ToCharArray().Any(char.IsLetter)
                    ? SecurityIssueGroup.FixedIncome
                    : SecurityIssueGroup.Equity;

        public string Issuer => string.Join("", _value.ToCharArray().Take(5));

        /// <summary>
        /// Default value is '10' 
        /// Src [https://www.cusip.com/pdf/CUSIP_Intro_03.14.11.pdf] 
        /// at Section "Issue Identifiers for Equity Securities"
        /// </summary>
        public string Issue => string.Join("", _value.ToCharArray().Skip(6).Take(2));

        public int CheckDigit => CheckDigitFunc(string.Join("", Issuer, Issue));

        public string Name { get; set; }

        /// <summary>
        /// Src [https://www.cusip.com/pdf/CUSIP_Intro_03.14.11.pdf] 
        /// @ sub section 'Alpha characters and their equivalent numerical values'
        /// </summary>
        internal static Dictionary<char, int> Letter2Num
        {
            get
            {
                if (_letter2num.Count > 0)
                    return _letter2num;
                for (var i = 0x41; i <= 0x5A; i++)
                {
                    var letter = Convert.ToChar((byte)i);
                    var num = i - 0x37;
                    _letter2num.Add(letter, num);
                }
                return _letter2num;
            }
        }
        internal static Dictionary<char, int> Num2Num
        {
            get
            {
                if (_num2num.Count > 0)
                    return _num2num;
                for (var i = 0x30; i <= 0x39; i++)
                {
                    var charNum = Convert.ToChar((byte)i);
                    var num = i - 0x30;
                    _num2num.Add(charNum, num);
                }
                return _num2num;
            }
        }

        #endregion

        #region methods
        /// <summary>
        /// Src [https://en.wikipedia.org/wiki/CUSIP#Check_digit_pseudocode]
        /// </summary>
        /// <param name="someChars"></param>
        /// <returns></returns>
        public static int CusipChkDigit(string someChars)
        {
            if (string.IsNullOrWhiteSpace(someChars) || someChars.Length < 8)
                return 0;
            var tChars = someChars.Replace(" ",string.Empty).ToUpper().ToCharArray();
            var sum = 0;
            for (var i = 1; i <= 8; i++)
            {
                var c = tChars[i-1];
                var v = 0;
                if (char.IsDigit(c))
                    v = Num2Num[c];
                else if (char.IsLetter(c))
                    v = Letter2Num[c] + 9;
                else switch (c)
                {
                    case '*':
                        v = 36;
                        break;
                    case '@':
                        v = 37;
                        break;
                    case '#':
                        v = 38;
                        break;
                }
                if (i%2 == 0)
                    v = v*2;
                sum += (v/10) + v%10;
            }

            return (10 - (sum%10))%10;
        }

        #endregion
    }
}
