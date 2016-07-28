using System;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NoFuture.Util.Etymological
{
    /// <summary>
    /// Reusable functions concerning specific words.
    /// </summary>
    public class En
    {
        #region EnglishWords

        /// <summary>
        /// Used in <see cref="ToPlural"/> for nouns which posses no discernable pattern
        /// for being rendered plural.
        /// </summary>
        public static readonly Hashtable IRREGULAR_PLURALS = new Hashtable()
        {
            {"Woman", "Women"},
            {"Man", "Men"},
            {"Child", "Children"},
            {"Person", "People"},
            {"Datum", "Data" },
            {"Foot", "Feet" },
            {"Goose","Geese" },
            {"Tooth","Teeth" },
            {"Mouse","Mice" },
            {"Deer", "Deer" },
            {"Moose", "Moose" },
        };

        private const string SP = @"(\s|^)";
        private const string AZ = "[a-z]";
        public const string CONSONANTS = "[b-df-hj-np-tv-z]";
        public const string VOWELS = "[aeiou]";

        public static readonly string[] GREEK_ORIGIN =
        {
            @"[a-z\s][Pp]h[a-z]+",
            $"{CONSONANTS}y{CONSONANTS}",
            $"{AZ}rrh{AZ}", $"{AZ}phth{AZ}",$"{AZ}chth{AZ}",
        };

        public static readonly string[] GREEK_PREFIX =
        {
            $"{SP}[Aa]n{AZ}",$"{SP}[Aa]mphi{AZ}",
            $"{SP}[Aa]nti{AZ}",$"{SP}[Aa]p{AZ}",
            $"{SP}[Cc]at{AZ}",$"{SP}[Dd]i{AZ}",
            $"{SP}[Dd]ys{AZ}",$"{SP}[Ee][cx]{AZ}",
            $"{SP}[Ee][lmn]{AZ}",$"{SP}[Ee]p{AZ}",
            $"{SP}[E][uv]{AZ}",$"{SP}[Ee]xo{AZ}",
            $"{SP}[Ee]cto{AZ}",$"{SP}[Hh]yper{AZ}",
            $"{SP}[Hh]py{AZ}",$"{SP}[Mm]eta{AZ}",
            $"{SP}[Pp]alin{AZ}", $"{SP}[Pp]ar{AZ}",
            $"{SP}[Pp]eri{AZ}",$"{SP}[Pp]ro{AZ}",
            $"{SP}[Ss]at{AZ}"
        };

        public static readonly string[] LATIN_PREFIX =
        {
            $"{SP}[Aa]b{AZ}",$"{SP}[Aa]d{AZ}",
            $"{SP}[Aa]mb{AZ}",$"{SP}[Aa]nte{AZ}",
            $"{SP}[Cc]ircum{AZ}",$"{SP}[Cc]o{AZ}",
            $"{SP}[Cc]ontra{AZ}",$"{SP}[Dd]e{AZ}",
            $"{SP}[Ee]f{AZ}",$"{SP}[Ee]xtr[ao]{AZ}",
            $"{SP}[Cc]ircum{AZ}",$"{SP}[Ii]ntr[ao]{AZ}",
            $"{SP}[Jj]uxta{AZ}", $"{SP}[Nn]e{AZ}",
            $"{SP}[Nn]on{AZ}", $"{SP}[Oo]b{AZ}",
            $"{SP}[Pp]er{AZ}",$"{SP}[Pp]ost{AZ}",
            $"{SP}[Pp]ra?e{AZ}", $"{SP}[Pp]reter{AZ}",
            $"{SP}[Pp]ro{AZ}", $"{SP}[Qq]uasi{AZ}",
            $"{SP}[Rr]etro{AZ}",$"{SP}[Ss]ine?{AZ}",
            $"{SP}[Ss]ub{AZ}",$"{SP}[Ss]up(er|ra)",
            $"{SP}[Tt]ran{AZ}", $"{SP}[Uu]ltra{AZ}"
        };

        public static readonly string[] LATIN_SUFFIX =
        {
            $"{AZ}[ai]ble{SP}", $"{AZ}ile{SP}",
            $"{AZ}acious{SP}", $"{AZ}id{SP}",
            $"{AZ}itious{SP}", $"{AZ}ive{SP}",
            $"{AZ}ory{SP}", $"{AZ}ul?ous{SP}",
            $"{AZ}ain{SP}",$"{AZ}[ei]al{SP}",
            $"{AZ}[ei]an{SP}", $"{AZ}ane{SP}",
            $"{AZ}ary{SP}", $"{AZ}ic{SP}",
            $"{AZ}ile?{SP}", $"{AZ}ine{SP}",
            $"{AZ}[ae]nt{SP}",
            $"{AZ}ite?{SP}", $"{AZ}lent{SP}",
            $"{AZ}ose{SP}",$"{AZ}ous{SP}",
            $"{AZ}acity{SP}",$"{AZ}acy{SP}",
            $"{AZ}nc[ey]{SP}",$"{AZ}tude{SP}",
            $"{AZ}ty{SP}",$"{AZ}[ou]lence{SP}",
            $"{AZ}imony{SP}",$"{AZ}and(um)?{SP}",
            $"{AZ}end(um)?{SP}",$"{AZ}ary{SP}",
            $"{AZ}arium{SP}", $"{AZ}ory{SP}",
            $"{AZ}orium{SP}",
            $"{AZ}ate{SP}",$"{AZ}ion{SP}",
            $"{AZ}ment?{SP}",$"{AZ}ure{SP}",
            $"{AZ}or{SP}",$"{AZ}rix{SP}",
            $"{AZ}cu?le{SP}",
            $"{AZ}el{SP}",$"{AZ}et(te)?{SP}",
            $"{AZ}le{SP}",$"{AZ}esce{SP}",
            $"{AZ}[ei]fy{SP}"
        };
        #endregion

        /// <summary>
        /// Puralize an English word - pays no respect to if word may already 
        /// be the plural version of itself, nor if the word is actually a noun.
        /// </summary>
        /// <param name="name">Expected to be a singular noun.</param>
        /// <param name="skipWordsEndingInS">Set this to true to break out when words already end with 's'.</param>
        /// <remarks>
        /// The command handles some diphthong (e.g. Tooth -> Teeth).
        /// </remarks>
        /// <returns></returns>
        public static string ToPlural(string name, bool skipWordsEndingInS = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                return name;

            name = name.Trim();

            if (name.Length <= 1)
                return name;

            if (name.EndsWith("s") && skipWordsEndingInS)
                return name;

            if (name.EndsWith("us") && name.Length > 2)
            {
                return $"{name.Substring(0, name.Length - 2)}i";
            }

            if ((name.EndsWith("s") || name.EndsWith("ch") || name.EndsWith("o")) && !name.EndsWith("es"))
            {
                return $"{name}es";
            }
            if (name.EndsWith("y"))
            {
                var nameArray = name.ToCharArray();
                var secondToLastIndex = name.Length - 2;
                if (secondToLastIndex > 0)
                {
                    if (!Regex.IsMatch(nameArray[secondToLastIndex].ToString(CultureInfo.InvariantCulture), "[aeiou]"))
                    {

                        return $"{name.Substring(0, name.Length - 1)}ies";
                    }
                }
            }
            if (name.EndsWith("fe") && name.Length > 2)
            {
                return $"{name.Substring(0, name.Length - 2)}ves";
            }
            if ((name.EndsWith("lf") || name.EndsWith("af") || name.EndsWith("ef")) && name.Length > 2)
            {
                return $"{name.Substring(0, name.Length - 1)}ves";
            }
            if (name.EndsWith("x"))
            {
                return $"{name.Substring(0, name.Length - 1)}ces";
            }

            if ((name.EndsWith("on") || name.EndsWith("um")) && name.Length > 2)
            {
                return $"{name.Substring(0, name.Length - 2)}a";
            }

            if (IRREGULAR_PLURALS.ContainsKey(name))
            {
                return IRREGULAR_PLURALS[name].ToString();
            }

            foreach (var val in IRREGULAR_PLURALS.Values)
            {
                if (val == null)
                    continue;
                if (name.EndsWith(val.ToString()))
                    return name;
            }

            return $"{name}s";
        }
    }
}
