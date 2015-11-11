using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NoFuture.Shared
{
    public class RegexCatalog
    {
        private static RegexOptions _myRegexOptions = RegexOptions.IgnoreCase;
        public static RegexOptions MyRegexOptions { get { return _myRegexOptions; } set { _myRegexOptions = value; } }

        public string CppClassMember { get { return Properties.Resources.CppClassMember; } }
        public string CsClass { get { return Properties.Resources.CsClass; } }
        public string CsClassMember { get { return Properties.Resources.CsClassMember; } }
        public string CsFunction { get { return Properties.Resources.CsFunction; } }
        public string EmailAddress { get { return Properties.Resources.EmailAddress; } }
        public string EmbeddedHtml { get { return Properties.Resources.EmbeddedHtml; } }
        public string IPv4 { get { return Properties.Resources.IPv4; } }
        public string JsFunction { get { return Properties.Resources.JsFunction; } }
        public string PhoneNumber01 { get { return Properties.Resources.PhoneNumber01; } }
        public string PhoneNumber02 { get { return Properties.Resources.PhoneNumber02; } }
        public string SqlSelectValues { get { return Properties.Resources.SqlSelectValues; } }
        public string SqlServerTableName { get { return Properties.Resources.SqlServerTableName; } }
        public string SSN { get { return Properties.Resources.SSN; } }
        public string StringIsRegex { get { return Properties.Resources.StringIsRegex; } }
        public string StringLiteral { get { return Properties.Resources.StringLiteral; } }
        public string TimeValue { get { return Properties.Resources.TimeValue; } }
        public string Uri { get { return Properties.Resources.Uri; } }
        public string Url { get { return Properties.Resources.Url; } }
        public string USD { get { return Properties.Resources.USD; } }
        public string UsZipcode { get { return Properties.Resources.UsZipcode; } }
        public string VbClassMember { get { return Properties.Resources.VbClassMember; } }
        public string WindowsRootedPath { get { return Properties.Resources.WindowsRootedPath; } }
        public string LongDate { get { return Properties.Resources.LongDate; } }
        public string UrlClassicAmerican { get { return Properties.Resources.UrlClassicAmerican; } }

        private static Hashtable _myRegex2Values = new Hashtable();

        /// <summary>
        /// A global container for assigning idiosyncratic pattern-value pairs.
        /// </summary>
        public static Hashtable MyRegex2Values
        {
            get { return _myRegex2Values; }
            set { _myRegex2Values = value; }
        }

        /// <summary>
        /// Test the <see cref="subject"/> against the <see cref="pattern"/>
        /// using the <see cref="MyRegexOptions"/>.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="pattern"></param>
        /// <param name="match"></param>
        /// <returns>
        /// The value of the first group.
        /// </returns>
        public static bool IsRegexMatch(string subject, string pattern, out string match)
        {
            match = null;
            if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(pattern))
                return false;

            var regex = new Regex(pattern, MyRegexOptions);

            if (!regex.IsMatch(subject))
                return false;

            var grp = regex.Matches(subject)[0];
            if (!grp.Success)
                return false;
            match = grp.Groups[0].Value;
            return true;

        }

        /// <summary>
        /// Asserts that at least one of the patterns in <see cref="regexii"/> 
        /// matches the <see cref="subject"/> using <see cref="MyRegexOptions"/>
        /// for each test therein.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="regexii"></param>
        /// <returns></returns>
        public static bool AreAnyRegexMatch(string subject, string[] regexii)
        {
            if (string.IsNullOrEmpty(subject))
                return false;
            if (regexii == null || regexii.Length <= 0)
                return false;

            foreach (var regexPattern in regexii)
            {
                if (string.IsNullOrWhiteSpace(regexPattern))
                    continue;
                if (Regex.IsMatch(subject, regexPattern, MyRegexOptions))
                    return true;
            }

            return false;
            //return regexii.Where(x => !string.IsNullOrWhiteSpace(x)).Any(x => Regex.IsMatch(subject, x, MyRegexOptions));
        }

        /// <summary>
        /// Applies each matching regex pattern in <see cref="regex2Values"/> Keys
        /// building up successful replacements of Values assigning the combined
        /// result to <see cref="subject"/>
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="regex2Values"></param>
        public static void AppropriateAllRegex(ref string subject, Hashtable regex2Values)
        {
            if (string.IsNullOrEmpty(subject))
                return;

            foreach (var regexKey in regex2Values.Keys.Cast<string>())
            {
                string matchedVal;

                var isRegexKeyMatch = IsRegexMatch(subject, regexKey, out matchedVal);

                if (!isRegexKeyMatch)
                    continue;

                subject = subject.Replace(matchedVal, regex2Values[regexKey].ToString());
            }
        }

        /// <summary>
        /// Constructs a basic regex pattern on the <see cref="someText"/> whole words.
        /// </summary>
        /// <param name="someText"></param>
        /// <returns></returns>
        /// <example>
        /// <![CDATA[
        /// var testInput = "Dependent (18 yrs +)";
        /// var testResult = NoFuture.Shared.RegexCatalog.ToRegexExpression(testInput);
        /// 
        /// System.Diagnostics.Debug.WriteLine(testResult); //Dependent*((18)?)*((yrs)?)
        /// ]]>
        /// </example>
        public static string ToRegexExpression(string someText)
        {
            if (string.IsNullOrEmpty(someText))
                return someText;

            var regexPattern = new StringBuilder();
            //this is probably just one single whole word
            if (someText.ToCharArray().All(x => char.IsNumber(x) || char.IsLetter(x)))
                return someText;
            const string _x2a_x28_x28 = "*((";
            const string _x29_x3f_x29 = ")?)";

            foreach (var c in someText.ToCharArray())
            {
                var isNumOrLetter = char.IsNumber(c) || char.IsLetter(c);
                var firstWholeWord = !regexPattern.ToString().Contains("*");
                var cPattern = regexPattern.ToString();
                var noTextBetwixt = cPattern.EndsWith(_x2a_x28_x28) || cPattern.EndsWith(_x29_x3f_x29);

                if (!firstWholeWord && !isNumOrLetter && !noTextBetwixt)
                    regexPattern.Append(_x29_x3f_x29);// )?)
                if (!isNumOrLetter && !noTextBetwixt)
                    regexPattern.Append(_x2a_x28_x28);// *((

                if (isNumOrLetter)
                    regexPattern.Append(c);
            }
            if (regexPattern.ToString().ToCharArray().Count(x => x == '*') >
                regexPattern.ToString().ToCharArray().Count(x => x == '?'))
                regexPattern.Append(_x29_x3f_x29);
            //clean up the tail
            return regexPattern.ToString().Replace(_x2a_x28_x28 + _x29_x3f_x29, string.Empty);
        }
    }
}
