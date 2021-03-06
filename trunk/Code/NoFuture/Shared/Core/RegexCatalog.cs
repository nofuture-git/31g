﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace NoFuture.Shared.Core
{
    public class RegexCatalog
    {
        private const string REGEX_LITERALS = "NoFuture.Shared.Core.Properties.RegexLiterals.txt";
        private static Dictionary<string, string> _regexLiterals = new Dictionary<string, string>();

        public static RegexOptions MyRegexOptions { get; set; } = RegexOptions.IgnoreCase;

        public string CppClassMember => GetRegexLiteral("CppClassMember");
        public string CsClass => GetRegexLiteral("CsClass");
        public string CsClassMember => GetRegexLiteral("CsClassMember");
        public string CsFunction => GetRegexLiteral("CsFunction");
        public string EmailAddress => GetRegexLiteral("EmailAddress");
        public string EmbeddedHtml => GetRegexLiteral("EmbeddedHtml");
        public string IPv4 => GetRegexLiteral("IPv4");
        public string JsFunction => GetRegexLiteral("JsFunction");
        public string PhoneNumber01 => GetRegexLiteral("PhoneNumber01");
        public string PhoneNumber02 => GetRegexLiteral("PhoneNumber02");
        public string SqlSelectValues => GetRegexLiteral("SqlSelectValues");
        public string SqlServerTableName => GetRegexLiteral("SqlServerTableName");
        public string SSN => GetRegexLiteral("SSN");
        public string StringIsRegex => GetRegexLiteral("StringIsRegex");
        public string StringLiteral => GetRegexLiteral("StringLiteral");
        public string TimeValue => GetRegexLiteral("TimeValue");
        public string Uri => GetRegexLiteral("Uri");
        public string Url => GetRegexLiteral("Url");
        public string USD => GetRegexLiteral("USD");
        public string UsZipcode => GetRegexLiteral("UsZipcode");
        public string VbClassMember => GetRegexLiteral("VbClassMember");
        public string WindowsRootedPath => GetRegexLiteral("WindowsRootedPath");
        public string LongDate => GetRegexLiteral("LongDate");
        public string UrlClassicAmerican => GetRegexLiteral("UrlClassicAmerican");

        internal static string GetRegexLiteral(string name)
        {
            if (_regexLiterals.Any())
                return _regexLiterals.FirstOrDefault(x => x.Key == name).Value ?? "";

            var liSteam = Assembly.GetExecutingAssembly().GetManifestResourceStream(REGEX_LITERALS);
            if (liSteam == null)
            {
                return string.Empty;
            }
            var txtSr = new StreamReader(liSteam);
            var content = txtSr.ReadToEnd();

            var lines = content.Split(Constants.LF);
            foreach (var ln in lines)
            {
                var keyValue = ln.Split('\t');
                if (keyValue.Length < 2)
                    continue;
                var key = keyValue[0]?.Trim() ?? "";
                var value = keyValue[1]?.Trim() ?? "";
                if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
                    continue;
                if (_regexLiterals.ContainsKey(key))
                    continue;
                _regexLiterals.Add(key, value);
            }

            return _regexLiterals.FirstOrDefault(x => x.Key == name).Value ?? "";
            
        }

        /// <summary>
        /// A global container for assigning idiosyncratic pattern-value pairs.
        /// </summary>
        public static Hashtable MyRegex2Values { get; set; } = new Hashtable();

        /// <summary>
        /// Test the <see cref="subject"/> against the <see cref="pattern"/>
        /// using the <see cref="MyRegexOptions"/>.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="pattern"></param>
        /// <param name="match"></param>
        /// <param name="matchGroup">The index value of the Groups array within the first Matches</param>
        /// <returns>
        /// The value of the first group.
        /// </returns>
        public static bool IsRegexMatch(string subject, string pattern, out string match, int matchGroup = 0)
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
            match = grp.Groups[matchGroup].Value;
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
            const string X2_A_X28_X28 = "*((";
            const string X29_X3_F_X29 = ")?)";

            foreach (var c in someText.ToCharArray())
            {
                var isNumOrLetter = char.IsNumber(c) || char.IsLetter(c);
                var firstWholeWord = !regexPattern.ToString().Contains("*");
                var cPattern = regexPattern.ToString();
                var noTextBetwixt = cPattern.EndsWith(X2_A_X28_X28) || cPattern.EndsWith(X29_X3_F_X29);

                if (!firstWholeWord && !isNumOrLetter && !noTextBetwixt)
                    regexPattern.Append(X29_X3_F_X29);// )?)
                if (!isNumOrLetter && !noTextBetwixt)
                    regexPattern.Append(X2_A_X28_X28);// *((

                if (isNumOrLetter)
                    regexPattern.Append(c);
            }
            if (regexPattern.ToString().ToCharArray().Count(x => x == '*') >
                regexPattern.ToString().ToCharArray().Count(x => x == '?'))
                regexPattern.Append(X29_X3_F_X29);
            //clean up the tail
            return regexPattern.ToString().Replace(X2_A_X28_X28 + X29_X3_F_X29, string.Empty);
        }
    }
}
