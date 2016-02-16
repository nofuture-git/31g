using System.Linq;

namespace NoFuture.Util.Math
{
    public static class Extensions
    {
        /// <summary>
        /// Calculates the integer value from its Roman numerical representation (e.g. XLIX is 49).
        /// </summary>
        /// <param name="someRomanNumerials">Must contain only valid Roman numerical chars</param>
        /// <returns></returns>
        /// <remarks>
        /// <![CDATA[
        /// https://social.msdn.microsoft.com/Forums/vstudio/en-US/600707d3-8053-4d4e-be0b-31c1f29690ad/c-roman-numeral-to-arabic-digits?forum=csharpgeneral
        /// ]]>
        /// </remarks>
        public static int ParseRomanNumeral(this string someRomanNumerials)
        {
            if (string.IsNullOrWhiteSpace(someRomanNumerials))
                return 0;
            
            var roman = someRomanNumerials.Trim().ToCharArray();
            var lookup = Lexicon.RomanNumerial2ArabicDigit;

            var arabic = 0;

            for (var i = 0; i < roman.Count(); i++)
            {
                if (!lookup.ContainsKey(roman[i]))
                    return 0;

                if (i == roman.Count() - 1)
                {
                    arabic += lookup[roman[i]];
                }
                else
                {
                    if (lookup[roman[i + 1]] > lookup[roman[i]]) arabic -= lookup[roman[i]];
                    else arabic += lookup[roman[i]];
                }
            }
            return arabic;

        }
    }
}
