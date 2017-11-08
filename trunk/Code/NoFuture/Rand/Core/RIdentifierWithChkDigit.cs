using System;
using System.Linq;

namespace NoFuture.Rand.Core
{
    /// <summary>
    /// An extension of <see cref="RIdentifier"/> having a f(x) pointer 
    /// to some algo for a check digit.
    /// </summary>
    [Serializable]
    public abstract class RIdentifierWithChkDigit : RIdentifier
    {
        protected internal Func<string, int> CheckDigitFunc;

        public override string GetRandom()
        {
            var valLessCk = base.GetRandom();
            if (CheckDigitFunc == null)
                return valLessCk;

            var chkDigit = CheckDigitFunc(valLessCk);
            var rVal = $"{valLessCk}{chkDigit}";
            return rVal;
        }

        public override bool Validate(string value)
        {
            if (value == null)
                return false;
            var lastChar = value.ToCharArray().Last(char.IsDigit);
            var lessChk = value.Substring(0, value.Length - 1);
            if (!base.Validate(lessChk))
            {
                return false;
            }
            int chkDigit;
            if (!int.TryParse(lastChar.ToString(), out chkDigit))
            {
                return false;
            }
            var calcChkDigit = CheckDigitFunc(lessChk);
            return chkDigit == calcChkDigit;
        }
    }
}