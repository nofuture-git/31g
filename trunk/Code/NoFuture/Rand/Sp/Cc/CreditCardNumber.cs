using System;
using NoFuture.Rand.Core;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Sp.Cc
{
    /// <summary>
    /// Represents a credit card number with algo for check digit
    /// </summary>
    /// <remarks>
    /// Given the format as an ordered-array of <see cref="Rchar"/>
    /// this type can both create random values and validate them.
    /// </remarks>
    [Serializable]
    public class CreditCardNumber : RIdentifierWithChkDigit
    {
        private readonly string _abbrev;

        public CreditCardNumber(string id, string abbrev = null)
        {
            CheckDigitFunc = Etc.CalcLuhnCheckDigit;
            format = DeriveFromValue(id);
            _value = id;
            _abbrev = abbrev;
        }

        public CreditCardNumber(Rchar[] format, string abbrev = null)
        {
            CheckDigitFunc = Etc.CalcLuhnCheckDigit;
            this.format = format;
            _abbrev = abbrev;
        }
        public override string Abbrev => _abbrev ?? "CC Num";

    }
}