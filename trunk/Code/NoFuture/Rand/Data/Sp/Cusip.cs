using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// A unique id for a financial security.
    /// Ref [https://en.wikipedia.org/wiki/CUSIP]
    /// Lists at [https://www.sec.gov/divisions/investment/13flists.htm]
    /// </summary>
    [Serializable]
    public class Cusip : RIdentifierWithChkDigit
    {
        #region constants
        public override string Abbrev => "CUSIP";
        #endregion

        #region fields
        private static Dictionary<char, int> _letter2num = new Dictionary<char, int>();
        private static Dictionary<char, int> _num2num = new Dictionary<char, int>();
        #endregion

        #region ctor
        public Cusip()
        {
            var rchars = new List<Rchar>();
            for(var i = 0; i < 8; i++)
                rchars.Add( new AlphaNumericRchar(i));
            format = rchars.ToArray();
            CheckDigitFunc = CusipChkDigit;
        }
        #endregion

        #region properties
        internal static Dictionary<char, int> Letter2Num
        {
            get
            {
                if (_letter2num.Count > 0)
                    return _letter2num;
                for (var i = 0x41; i <= 0x5A; i++)
                {
                    var letter = Convert.ToChar((byte)i);
                    var num = i - 0x40;
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
