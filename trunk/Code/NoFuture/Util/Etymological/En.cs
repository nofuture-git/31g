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
            {"Datum", "Data" }
        };
        #endregion

        /// <summary>
        /// Puralize an English word - pays no respect to if word may already 
        /// be the plural version of itself, nor if the word is actually a noun.
        /// </summary>
        /// <param name="name">Expected to be a singular noun.</param>
        /// <param name="skipWordsEndingInS">Set this to true to break out when words already end with 's'.</param>
        /// <remarks>
        /// The command does not handle diphthong (e.g. Tooth -> Teeth).
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

                        return $"{name.Substring(0, (name.Length - 1))}ies";
                    }
                }
            }
            if (name.EndsWith("fe") && name.Length > 2)
            {
                return $"{name.Substring(0, (name.Length - 2))}ves";
            }
            if (name.EndsWith("x"))
            {
                return $"{name.Substring(0, name.Length - 1)}ces";
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
