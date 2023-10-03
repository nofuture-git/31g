using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NoFuture.Law.US.CaseReading
{
    public static class Citation
    {

        public static List<string> StopWords => new List<string> { "of", "&", "&amp;", "and", "in", "see", "to", "at" };

        public static bool TryFindCaseName(string someLine, out string caseid)
        {
            caseid = null;
            if (string.IsNullOrWhiteSpace(someLine))
                return false;

            //expect it to end with a year in 18th, 19th, 20th or 21st century and closing parenth
            var closingRegex = @"[^0-9](17|18|19|20)[0-9][0-9]\x29";
            var yearMatch = GetMatch(someLine, closingRegex);
            if (yearMatch == null)
                return false;
            var idxEnd = someLine.IndexOf(yearMatch) + yearMatch.Length;

            //figure start of name to left of "v." 
            var isContainingVdot = Regex.IsMatch(someLine, @"[^a-z]v\x2E");

            if (!isContainingVdot)
            {
                //In re in the case name
                var workingWord = GetMatch(someLine, @"In\x20re\x20");
                if (workingWord == null)
                    return false;
                var idxStart = someLine.IndexOf(workingWord);
                idxStart = idxStart < 0 ? 0 : idxStart;
                caseid = someLine.Substring(idxStart, idxEnd - idxStart);
                return !string.IsNullOrWhiteSpace(caseid);
            };
            caseid = GetProperName2LeftOfToken(someLine, "v.", idxEnd);
            return !string.IsNullOrWhiteSpace(caseid);
        }

        public static bool TryFindStatute(string someLine, out string caseid)
        {
            caseid = null;
            //most common indicators
            var sectionChar = @"\u00A7";
            var numColonNum = @"[0-9](\x3A)[0-9]";

            var token = GetMatch(someLine, sectionChar) ?? GetMatch(someLine, numColonNum, 1);
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            var workingName = GetStatuteTokens(someLine, token);

            if (!workingName.Any())
                return false;
            if (!IsKeepFirstWord(someLine, workingName))
                workingName = workingName.Skip(1).Take(workingName.Count - 1).ToList();
            workingName = TrimStopWords(workingName);

            caseid = string.Join(" ", workingName);
            if (token == ":")
                caseid = caseid.Replace(" : ", ":");
            caseid = RemoveNonsenseStopWords(someLine, caseid);
            return !string.IsNullOrWhiteSpace(caseid);
        }

        internal static List<string> GetStatuteTokens(string someLine, string token)
        {
            var workingName = new List<string>();
            if (string.IsNullOrWhiteSpace(someLine) || string.IsNullOrWhiteSpace(token))
                return workingName;

            var tokenIdx = someLine.IndexOf(token);
            if (tokenIdx <= 0)
                return workingName;

            var leftOfToken = someLine.Substring(0, tokenIdx).Split(' ');
            var rightOfToken = someLine.Substring(tokenIdx + token.Length).Split(' ');


            workingName.AddRange(leftOfToken.Where(w => IsNumberEsque(w)
                                                        || IsProperName(w)
                                                        || IsSectionSymbol(w)
                                                        || IsValidStopWord(w, workingName)));
            workingName.Add(token);
            workingName.AddRange(rightOfToken.Where(w => IsNumberEsque(w)
                                                         || IsProperName(w)
                                                         || IsSectionSymbol(w)
                                                         || IsValidStopWord(w, workingName)));
            return workingName;
        }

        internal static List<string> TrimStopWords(List<string> workingWords)
        {
            //trim of stop words
            var temp00 = new List<string>();
            foreach (var t in workingWords)
            {
                if (!IsStopWord(t) || IsValidStopWord(t, temp00))
                    temp00.Add(t);
            }

            var temp01 = new List<string>();
            temp00.Reverse();
            foreach (var t in temp00)
            {
                if (!IsStopWord(t) || IsValidStopWord(t, temp01))
                    temp01.Add(t);
            }

            temp01.Reverse();
            return temp01;
        }

        /// <summary>
        /// We will not keep stop words when how they appear in the <see cref="caseid"/>
        /// is nothing like any part of <see cref="someLine"/>
        /// </summary>
        /// <returns></returns>
        internal static string RemoveNonsenseStopWords(string someLine, string caseid)
        {
            if (string.IsNullOrWhiteSpace(someLine) || string.IsNullOrWhiteSpace(caseid))
                return caseid;

            var caseIdParts = caseid.Split(' ').ToList();
            var remainingId = new List<string> { caseIdParts[0] };

            for (var i = 1; i < caseIdParts.Count; i++)
            {
                var center = caseIdParts[i];
                if (!IsStopWord(center))
                {
                    remainingId.Add(center);
                    continue;
                }
                var left = caseIdParts.Skip(i - 1).Take(1).FirstOrDefault() ?? "";
                var right = caseIdParts.Skip(i + 1).Take(1).FirstOrDefault() ?? "";
                if (someLine.Contains($"{left} {center} {right}"))
                    remainingId.Add(center);
            }

            return string.Join(" ", remainingId);
        }

        internal static string GetProperName2LeftOfToken(string someLine, string token, int? idxEnd = null)
        {
            if (string.IsNullOrWhiteSpace(someLine) || string.IsNullOrWhiteSpace(token))
                return null;
            var idxStart = 0;
            var workingWord = "";
            var vDotIdx = someLine.IndexOf(token);
            if (vDotIdx <= 0)
                return null;

            var leftPart = someLine.Substring(0, vDotIdx);
            if (string.IsNullOrWhiteSpace(leftPart))
                return null;

            var leftChars = leftPart.ToCharArray();

            var workingName = new List<string>();
            for (var i = leftPart.Length - 1; i >= 0; i--)
            {
                var c = leftChars[i];

                //decide only when at a space, otherwise, add and go on
                if (c != (char)0x20)
                {
                    workingWord += c;
                    continue;
                }

                //if we don't have anything yet then just go to the next char
                if (workingWord.Length <= 0)
                    continue;

                workingWord = new string(workingWord.Reverse().ToArray());

                var isWorkingWordProperName = IsProperName(workingWord);
                var isWorkingWordStopWord = IsValidStopWord(workingWord, workingName);
                var lastWordInName = workingName.LastOrDefault();
                var isLastWordInNameStopWord = StopWords.Any(sw => string.Equals(sw, lastWordInName));

                if (isWorkingWordProperName || (isWorkingWordStopWord && !isLastWordInNameStopWord))
                {
                    workingName.Add($"{workingWord}");
                    workingWord = "";
                    continue;
                }

                break;
            }

            //when we go to the start of the line
            workingWord = new string(workingWord.Reverse().ToArray());
            if (char.IsUpper(workingWord.ToCharArray()[0]))
            {
                workingName.Add(workingWord);
            }

            workingName.Reverse();

            workingName = TrimStopWords(workingName);
            var intIdxEnd = idxEnd.GetValueOrDefault(someLine.Length - 1);
            workingWord = string.Join(" ", workingName);
            idxStart = someLine.IndexOf(workingWord);
            idxStart = idxStart < 0 ? 0 : idxStart;
            var caseid = someLine.Substring(idxStart, intIdxEnd - idxStart);
            return caseid;
        }

        internal static bool IsProperName(string workingWord)
        {
            if (string.IsNullOrWhiteSpace(workingWord))
                return false;
            var wordChars = workingWord.ToCharArray();
            var startsWithCapLetter = char.IsUpper(wordChars[0]);
            var allowOtherwise = new[] { '[', '(', '{' }.Any(c => c == wordChars[0]) && char.IsUpper(wordChars[1]);
            return startsWithCapLetter || allowOtherwise;
        }

        /// <summary>
        /// Idea is that we may not want first word since every sentence appears like
        /// a proper name.  We only want this first word when its part of a longer continous
        /// proper name.
        /// </summary>
        /// <param name="someLine"></param>
        /// <param name="workingName"></param>
        /// <returns></returns>
        internal static bool IsKeepFirstWord(string someLine, List<string> workingName)
        {
            //remove trival cases from function
            if (workingName.Count == 1)
            {
                return true;
            }
            var firstWorkingWord = workingName.First();
            if (IsNumberEsque(firstWorkingWord)
                || IsAcryonm(firstWorkingWord)
                )
                return true;

            var splitLine = someLine.Split(' ').Where(v => !string.IsNullOrWhiteSpace(v)).ToList();
            var firstActualWord = splitLine.FirstOrDefault() ?? "";

            var secondWorkingWord = workingName.Skip(1).Take(1).FirstOrDefault() ?? "";

            var secondActualWord = splitLine.Skip(1).Take(1).FirstOrDefault() ?? "";

            return string.Equals(firstActualWord, firstWorkingWord) &&
                   secondActualWord.StartsWith(secondWorkingWord);
        }

        internal static bool IsValidStopWord(string workingWord, List<string> workingName, params string[] additionalStopWords)
        {
            //is this even a stop word
            if (!IsStopWord(workingWord, additionalStopWords))
                return false;

            //the first word cannot be a stop word
            var isFirstWordIn = workingName.Count == 0;
            if (isFirstWordIn)
                return false;

            //a repeated stop word is invalid (e.g. "and and")
            var lastWord = workingName.LastOrDefault() ?? "";
            if (string.Equals(CleanUpStopWord(workingWord), CleanUpStopWord(lastWord),
                StringComparison.OrdinalIgnoreCase))
                return false;

            return true;
        }

        private static string CleanUpStopWord(string someWord)
        {
            if (someWord.EndsWith(","))
                someWord = someWord.Substring(0, someWord.Length - 1);
            return someWord;
        }

        internal static bool IsStopWord(string workingWord, params string[] additionalStopWords)
        {
            workingWord = CleanUpStopWord(workingWord);
            var stopWords = new List<string>();
            stopWords.AddRange(StopWords);
            if (additionalStopWords != null && additionalStopWords.Any())
                stopWords.AddRange(additionalStopWords);

            return !string.IsNullOrWhiteSpace(workingWord) && stopWords.Any(sw => string.Equals(sw, workingWord));
        }

        internal static bool IsSectionSymbol(string workingWord)
        {
            return !string.IsNullOrWhiteSpace(workingWord) && Regex.IsMatch(workingWord, @"\u00A7");
        }

        internal static bool IsNumberEsque(string workingWord)
        {
            if (int.TryParse(workingWord, out int _))
                return true;

            var numberesquePattern = @"^[0-9\-][0-9a-fA-F\x2D\x28\x29\x2E\x2C\x3A]+[(\x3B|\x5D)]?$";
            return !string.IsNullOrWhiteSpace(workingWord) && Regex.IsMatch(workingWord, numberesquePattern);
        }

        internal static bool IsAcryonm(string workingWord)
        {
            return !string.IsNullOrWhiteSpace(workingWord) && Regex.IsMatch(workingWord, @"^[A-Z\x2E]+$");
        }

        internal static string GetMatch(string input, string pattern, int matchGroup = 0)
        {
            if (input == null || pattern == null)
            {
                return null;
            }

            var closingYearRegex = new Regex(pattern);
            if (!closingYearRegex.IsMatch(input))
                return null;
            var grp = closingYearRegex.Matches(input)[0];
            if (!grp.Success)
                return null;

            return grp.Groups[matchGroup].Value;
        }

    }
}
