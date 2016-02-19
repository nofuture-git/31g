using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using NoFuture.Shared;

namespace NoFuture.Util
{
    /// <summary>
    /// This is your basic tool box of mildly useful functions.
    /// </summary>
    public static class Etc
    {
        /// <summary>
        /// http://stackoverflow.com/questions/1508572/converting-xdocument-to-xmldocument-and-vice-versa
        /// </summary>
        /// <param name="xDocument"></param>
        /// <returns></returns>
        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        /// <summary>
        /// http://stackoverflow.com/questions/1508572/converting-xdocument-to-xmldocument-and-vice-versa
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <returns></returns>
        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }

        private const string LOREM_IPSUM_RSC = "NoFuture.Util.LoremIpsum.EightParagraphs.txt";

        /// <summary>
        /// Utility function to get eight paragraphs of classic filler text.
        /// </summary>
        public static string LoremIpsumEightParagraphs
        {
            get
            {
                //need this to be another object each time and not just another reference
                var liSteam = Assembly.GetExecutingAssembly().GetManifestResourceStream(LOREM_IPSUM_RSC);
                if (liSteam == null)
                {
                    return LOREM_IPSUM_RSC;
                }
                var txtSr = new StreamReader(liSteam);
                return txtSr.ReadToEnd();
            }
        }

        /// <summary>
        /// Distills the continous spaces into a single space and 
        /// replaces Cr [0x0D] and Lf [0x0A] characters with a single space.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DistillString(string value)
        {
            var sb = new StringBuilder();
            var prev = (char)0x0;
            foreach (var c in value.ToCharArray())
            {
                if ((c == (char)0x20 || c == (char)0x0D || c == (char)0x0A) && prev == (char)0x20)
                    continue;
                if (c == (char)0x0D || c == (char)0x0A)
                {
                    sb.Append((char)0x20);
                    prev = (char) 0x20;
                    continue;
                }

                sb.Append(c);

                //lastly keep a memory of what was just handled 
                prev = c;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Reduces all repeating sequence of space-characters to one.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <summary>
        /// Returns a string as a modification of the arg having all 
        /// sequence of tabs [0x09] reduced to one single space 
        /// char [0x20] regardless of the length of the sequence.
        /// </summary>
        public static string DistillTabs(string value)
        {
            while (true)
            {
                if (value.Contains("\t\t"))
                {
                    value = value.Replace("\t\t", "\t");
                    continue;
                }

                return value.Replace("\t", " ");
            }
        }

        /// <summary>
        /// Converts line endings to CrLf
        /// </summary>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        public static string ConvertToCrLf(string fileContent)
        {
            if (fileContent == null)
                return null;
            fileContent = fileContent.Replace(new string(new[] { Constants.CR, Constants.LF }), new string(new[] { Constants.LF }));
            fileContent = fileContent.Replace(new string(new[] { Constants.CR }), new string(new[] { Constants.LF }));
            fileContent = fileContent.Replace(new string(new[] { Constants.LF }), new string(new[] { Constants.CR, Constants.LF }));
            return fileContent;
        }

        /// <summary>
        /// Reduces multiple blank lines.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static string RemoveBlankLines(string[] lines)
        {
            var linesOut = new StringBuilder();

            foreach (var ln in lines)
            {
                linesOut.Append(string.IsNullOrWhiteSpace(ln)
                    ? Environment.NewLine
                    : string.Format("{0}{1}", ln, Environment.NewLine));
            }

            var value = linesOut.ToString();

            while (true)
            {
                if (value.Contains(Environment.NewLine + Environment.NewLine + Environment.NewLine))
                {
                    value = value.Replace(Environment.NewLine + Environment.NewLine + Environment.NewLine,
                        Environment.NewLine + Environment.NewLine);
                    continue;
                }
                return value;
            }
        }

        /// <summary>
        /// Splits <see cref="value"/> into an array on any readable
        /// separator - being both camel-case words or special chars.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string[] DistillToWholeWords(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return null;
            if (value.Length <= 1)
                return new[] {value};

            value = TransformScreamingCapsToCamelCase(value);
            value = TransformCamelCaseToSeparator(value, Constants.DEFAULT_CHAR_SEPARATOR);
            value = CapitalizeFirstLetterOfWholeWords(value, Constants.DEFAULT_CHAR_SEPARATOR);
            return value.Split(Constants.DEFAULT_CHAR_SEPARATOR).Distinct().ToArray();
        }

        /// <summary>
        /// Returns string <see cref="value"/> as an escape sequence 
        /// of various forms found in HTML. 
        /// Which of these forms is based on <see cref="escapeType"/>.
        /// The <see cref="value"/> is first encoded to the ISO-8859-1 standard unless the 
        /// <see cref="escapeType"/> is specified as REGEX in which the <see cref="value"/> is 
        /// encoded into UTF8.
        /// </summary>
        /// <param name="value">Any string which is to be escaped.</param>
        /// <param name="escapeType">
        /// The kind of escape sequence to encode <see cref="value"/> into.
        /// The default is REGEX.
        /// </param>
        /// <example>
        /// <![CDATA[
        /// Etc.EscapeString("I am decimal", EscapeStringType.DECIMAL); //"&#73;&#32;&#97;&#109;&#32;&#100;&#101;&#99;&#105;&#109;&#97;&#108;"
        /// 
        /// Etc.EscapeString("I am unicode", EscapeStringType.HEXDECIMAL_LONG);//"\u2049\u6D61\u7520\u696E\u6F63\u6564"
        /// 
        /// Etc.EscapeString("[regex]", EscapeStringType.REGEX);//"\x5b\x72\x65\x67\x65\x78\x5d"
        /// 
        /// Etc.EscapeString(" £¡¥§", EscapeStringType.HTML);//"&nbsp;&pound;&iexcl;&yen;&sect;"
        /// ]]>
        /// </example>
        /// <returns></returns>
        public static string EscapeString(string value, EscapeStringType escapeType = EscapeStringType.REGEX)
        {
            var data = Encoding.GetEncoding("ISO-8859-1").GetBytes(value);
            var dataOut = new StringBuilder();

            switch (escapeType)
            {
                case EscapeStringType.DECIMAL:
                    foreach (var dex in data)
                    {
                        dataOut.AppendFormat("&#{0};", dex.ToString("G"));
                    }
                    break;
                case EscapeStringType.DECIMAL_LONG:
                    foreach (var dex in data)
                    {
                        dataOut.AppendFormat("&#{0:0000000}", Int32.Parse(dex.ToString("G")));
                    }
                    break;
                case EscapeStringType.HEXDECIMAL:
                    foreach (var dex in data)
                    {
                        dataOut.AppendFormat("&#x{0}", dex.ToString("X2"));
                    }
                    break;
                case EscapeStringType.REGEX:
                    data = Encoding.UTF8.GetBytes(value);
                    foreach (var dex in data)
                    {
                        dataOut.AppendFormat(@"\x{0}", dex.ToString("x2"));
                    }
                    break;
                case EscapeStringType.UNICODE:
                    foreach(var c in value.ToCharArray())
                    {
                        dataOut.AppendFormat(@"\u{0}", Convert.ToUInt16(c).ToString("x4"));
                    }
                    break;
                case EscapeStringType.HTML:
                    var htmlEsc = Net.HtmlEscStrings;
                    foreach (var dex in data)
                    {
                        if (htmlEsc.Select(t => t.Item1).Contains(dex))
                            dataOut.Append(htmlEsc.First(t => t.Item1 == dex).Item2);
                        else
                            dataOut.AppendFormat("&#{0};", dex);
                    }
                    break;
                case EscapeStringType.BLANK:
                    return new string(' ', data.Length);
            }
            return dataOut.ToString();
        }

        /// <summary>
        /// Pads a string on both sides with spaces so that the 
        /// sum of the whole is equal to <see cref="printBlockWidth"/>.
        /// </summary>
        /// <param name="printBlockWidth"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <example>
        /// <![CDATA[
        /// PrintInCenter(22,"hello");//'         hello        ' the single-quotes are added for the example.
        /// ]]>
        /// </example>
        public static string PrintInCenter(int printBlockWidth, string text)
        {
            //validate the args
            if(printBlockWidth<0.0D)
                throw new ArgumentException("the width must be a positive integer");

            var printBlock = String.Format(("{0,-" + printBlockWidth + "}"), " ").ToCharArray();
            

            if (String.IsNullOrWhiteSpace(text))
                return new string(printBlock);

            //set the text block to have an even number of items
            var textBlock = text.ToCharArray();
            if (textBlock.Length % 2 == 1)
            {
                textBlock = String.Format(" {0}", text).ToCharArray();
            }

            if (textBlock.Length >= printBlockWidth)
                return new string(textBlock);

            //find mid-point of the block
            var blockMidPt = System.Math.Ceiling((double)printBlockWidth / 2);

            //find mid-point of the text
            var textMidPt = System.Math.Ceiling((double)text.Length / 2);

            /*
             * textBlock =  | |J|u|d|e|a|
             *               0 1 2 3 4 5
             * printBlock = | | | | | | | | | | | |
             *               0 1 2 3 4 5 6 7 8 9 A
             * pbMidPt = 5
             * tbMidPt = 3
                 
             *  0 -> printBlock[0+5] = textBlock[0+3],
             *        printBlock[5-0] = textBlock[3-0]
             *  1 -> printBlock[1+5] = textBlock[1+3],
             *        printBlock[(5-1)-1] = textBlock[(3-1)-1]
             *  2 -> printBlock[2+5] = textBlock[2+3],
             *        printBlock[(5-1)-2] = textBlock[(3-1)-2]
             */

            for (var i = 0; i < textMidPt; i++)
            {
                printBlock[i + (int) blockMidPt] = textBlock[i + (int) textMidPt];
                printBlock[(int) blockMidPt - 1 - i] = textBlock[(int) textMidPt - 1 - i];
            }

            return new string(printBlock);
        }

        /// <summary>
        /// Takes two strings and lays one atop the other by ordinal giving precedence 
        /// to characters above 0x20.
        /// </summary>
        /// <param name="primaryString"></param>
        /// <param name="secondaryString"></param>
        /// <returns></returns>
        /// <example>
        /// <![CDATA[
        /// MergeString("|  this is primary ","AND            SECONDARY HERE");//|NDthis is primaryONDARY HERE
        /// ]]>
        /// </example>
        public static string MergeString(string primaryString, string secondaryString)
        {
            if (String.IsNullOrEmpty(primaryString) && String.IsNullOrEmpty(secondaryString))
                return String.Empty;

            Func<string, int, char> valOr0 = (c, i) => String.IsNullOrEmpty(c) || c.Length <= i ? (char)0x0 : c[i];

            Func<string, string, int, char> primOrSec = (s, s1, arg3) =>
            {
                if (arg3 < 0) return (char) 0x0;

                var pChar = valOr0(primaryString, arg3);

                //if the first char did not come back as \0 then make the second char \0
                var sChar = Convert.ToInt32(pChar) != 0x0 ? (char)0x0 : valOr0(secondaryString, arg3);

                //special circumstance to return secondary when primary is a space
                if (valOr0(primaryString, arg3) <= (char)0x20 && valOr0(secondaryString, arg3) > (char)0x20)
                    return valOr0(secondaryString, arg3);

                return (char)(Convert.ToInt32(pChar) | Convert.ToInt32(sChar));
            };

            var length = primaryString.Length > secondaryString.Length ? primaryString.Length : secondaryString.Length;

            var mergedChars = new char[length];
            for (var i = 0; i < length; i++)
            {
                mergedChars[i] = primOrSec(primaryString, secondaryString, i);
            }

            return new string(mergedChars);
        }

        /// <summary>
        /// Merges two strings on a character-by-character basis at which each char with 
        /// the higher numerical value is opted for having all CTRL chars (id est. less-than 0x20) being ignored.
        /// </summary>
        /// <param name="firstString"></param>
        /// <param name="secondString"></param>
        /// <returns></returns>
        public static string BinaryMergeString(string firstString, string secondString)
        {
            if (firstString == null)
                firstString = String.Empty;
            if (secondString == null)
                secondString = String.Empty;

            var firstStringCtrlRem = new StringBuilder();
            var secondStringCtrlRem = new StringBuilder();

            foreach (var c in firstString.ToCharArray().Where(c => Convert.ToInt32(c) >= 0x20))
                firstStringCtrlRem.Append(c);

            foreach (var c in secondString.ToCharArray().Where(c => Convert.ToInt32(c) >= 0x20))
                secondStringCtrlRem.Append(c);

            firstString = firstStringCtrlRem.ToString();
            secondString = secondStringCtrlRem.ToString();

            Func<string, int, char> valOr0 =
                (c, i) => String.IsNullOrEmpty(c) || c.Length <= i ? (char) 0x20 : c[i];

            var len = firstString.Length > secondString.Length ? firstString.Length : secondString.Length;

            var binaryMergeChars = new char[len];

            for (var i = 0; i < len; i++)
            {
                var fchar = valOr0(firstString, i);
                var schar = valOr0(secondString, i);

                binaryMergeChars[i] = Convert.ToInt32(fchar) > Convert.ToInt32(schar) ? fchar : schar;
            }
            return new string(binaryMergeChars);
        }

        /// <summary>
        /// Given <see cref="someCsv"/>, being a string which will split on <see cref="delimiter"/>, this
        /// function will simply pad a space after each comma and replace the last one with " and ".
        /// </summary>
        /// <param name="someCsv"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string ToEnglishSentenceCsv(string someCsv, char delimiter)
        {
            if (String.IsNullOrEmpty(someCsv))
                return String.Empty;

            var csvItems = someCsv.Split(delimiter);
            var csvClean = csvItems.Where(s => !String.IsNullOrWhiteSpace(s)).ToList();

            for (var i = 0; i < csvClean.Count; i++)
                csvClean[i] = csvClean[i].Trim();

            var series = new StringBuilder();

            if (csvClean.Count == 1)
            {
                return csvClean[0];
            }

            if (csvClean.Count == 2)
            {
                series.Append(csvClean[0]);
                if (!String.IsNullOrEmpty(csvClean[0]) && !String.IsNullOrEmpty(csvClean[1]))
                    series.Append(" and ");
                series.Append(csvClean[1]);
                return series.ToString();
            }

            for (var i = 0; i < csvClean.Count - 1; i++)
            {
                series.Append(csvClean[i]);
                if (!String.IsNullOrEmpty(csvClean[i]) && i != csvClean.Count - 2) 
                    series.Append(",");

                series.Append(" ");
            }
            if(!String.IsNullOrEmpty(series.ToString()))
                series.Append("and ");

            series.Append(csvClean[(csvClean.Count - 1)]);
            return series.ToString();

        }

        /// <summary>
        /// Changes to Upper each character which is either the very first
        /// non-whitespace character in the string and every character which 
        /// appears after the <see cref="separator"/>.
        /// When some part of the string, split on the <see cref="separator"/>
        /// is found to be all caps then the whole thing will be lower except 
        /// for the first character.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string CapitalizeFirstLetterOfWholeWords(string name, char? separator)
        {
            if (String.IsNullOrWhiteSpace(name))
                return String.Empty;

            name = name.Trim();
            name =
                new string(
                    name.ToCharArray()
                        .Where(c => char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsPunctuation(c))
                        .ToArray());

            if (separator == null)
                separator = Constants.DefaultTypeSeparator;

            var nameArray = name.Split(separator.Value);
            var nameFormatted = new StringBuilder();
            for (var i = 0; i < nameArray.Length;i++ )
            {
                var s = nameArray[i];
                if (String.IsNullOrWhiteSpace(s) || s.Length <= 1)
                    continue;

                var firstLetter = s.Substring(0, 1).ToUpper();
                var restOfString = s.Substring(1, s.Length-1);
                //if the rest of the string is all caps the lower them otherwise leave'em as is
                if (restOfString.ToCharArray().All(Char.IsUpper))
                    restOfString = restOfString.ToLower();
                nameFormatted.Append(firstLetter);
                nameFormatted.Append(restOfString);
                if(i < nameArray.Length - 1)
                {
                    nameFormatted.Append(separator.Value);
                }
            }
            return nameFormatted.ToString();
        }

        /// <summary>
        /// Trims a string then puts to lower case the first letter found there in.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToCamelCase(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return String.Empty;

            name = name.Trim();

            var startAt = 0;
            while (!Char.IsLetter(name.ToCharArray()[startAt]))
                startAt += 1;

            var firstLetter = name.Substring(startAt, 1).ToLower();
            var restOfString = name.Substring(startAt + 1, name.Length - startAt -1);
            //if the entire name is upper case then drop everything to lower (e.g. ID we don't want iD).
            if (restOfString.ToCharArray().All(Char.IsUpper))
            {
                restOfString = restOfString.ToLower();
            }
            var nameFormatted = new StringBuilder();
            if (startAt > 0)
                nameFormatted.Append(name.ToCharArray(0, startAt));
            nameFormatted.Append(firstLetter);
            nameFormatted.Append(restOfString);
            return nameFormatted.ToString();

        }

        /// <summary>
        /// Simply returns the last entry in a string that was
        /// split on <see cref="separator"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ExtractLastWholeWord(string name, char? separator)
        {
            if(String.IsNullOrWhiteSpace(name))
                return String.Empty;

            if (separator == null)
                separator = Constants.DefaultTypeSeparator;

            if (!name.Contains(separator.ToString()))
                return name;

            var nameArray = name.Split(separator.Value);
            if(nameArray.Length == 1)
            {
                return name;
            }
            var lastIndex = nameArray.Length - 1;
            return nameArray[lastIndex];
        }

        /// <summary>
        /// Given a string in the form of camel-case (or Pascal case) - a 
        /// <see cref="separator"/> will be inserted between characters 
        /// which are lowercase followed by uppercase.
        /// </summary>
        /// <param name="camelCaseString"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string TransformCamelCaseToSeparator(string camelCaseString, char separator)
        {
            if (String.IsNullOrWhiteSpace(camelCaseString))
                return String.Empty;
            var separatorName = new StringBuilder();
            var charArray = camelCaseString.ToCharArray();
            for(var i = 0; i<charArray.Length;i++)
            {
                separatorName.Append(charArray[i]);
                if(i+1 < charArray.Length)
                {
                    if(Char.IsLower(charArray[i]) && Char.IsUpper(charArray[(i+1)]))
                    {
                        separatorName.Append(separator);
                    }
                }
            }
            return separatorName.ToString();
        }

        /// <summary>
        /// Transforms a string which is delimited by a separator 
        /// transforming into a camel-case string.
        /// </summary>
        /// <param name="screamingCaps"></param>
        /// <param name="perserveDot">Set to true to have '.' perserved in results, default is false.</param>a
        /// <remarks>
        /// Will work for both screaming-caps as well as all lower-case
        /// strings.
        /// </remarks>
        /// <returns></returns>
        public static string TransformScreamingCapsToCamelCase(string screamingCaps, bool perserveDot = false)
        {
            if (String.IsNullOrWhiteSpace(screamingCaps))
                return String.Empty;
            var toCamelCase = new StringBuilder();
            var charArray = ToCamelCase(screamingCaps).ToCharArray();
            toCamelCase.Append(charArray[0]);
            for(var i=1; i<charArray.Length;i++)
            {
                //last character
                if (i + 1 >= charArray.Length)
                {
                    toCamelCase.Append(Char.ToLower(charArray[i]));
                    continue;
                }
                    
                if(Char.IsLetterOrDigit(charArray[i]) || (perserveDot && charArray[i] == '.'))
                {
                    if (!Char.IsLower(charArray[i]) && Char.IsLower(charArray[i + 1]))
                    {
                        toCamelCase.Append(charArray[i]);
                    }
                    else
                    {
                        toCamelCase.Append(Char.ToLower(charArray[i]));   
                    }
                    continue;
                }
                if(!Char.IsLetterOrDigit(charArray[i]) && Char.IsLetterOrDigit(charArray[(i+1)]))
                {
                    toCamelCase.Append(charArray[(i + 1)]);
                    i += 1;
                }
            }
            return toCamelCase.ToString();
        }

        /// <summary>
        /// Given some string <see cref="line"/> the function will 
        /// determine the number of occurances of <see cref="c"/> when 
        /// it is the case that it is not enclosed in single nor double quotes.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static int FindCountOfCharNotInQuotes(string line, char? c)
        {
            if (c == null)
                c = Constants.DEFAULT_CHAR_SEPARATOR;
            var tokenCount = 0;
            var inDoubleQuotes = false;
            var inSingleQuotes = false;
            var buffer = line.ToCharArray();

            for (var j = 0; j < buffer.Length; j++)
            {
                //flag entry and exit into string literals
                if (buffer[j] == '"' && j - 1 > 0 && buffer[j - 1] != '\\')
                    inDoubleQuotes = !inDoubleQuotes;
                if (buffer[j] == '\'' && !inDoubleQuotes && j - 1 > 0 && buffer[j - 1] != '\\')
                    inSingleQuotes = !inSingleQuotes;
                if (buffer[j] == c && (!inDoubleQuotes || !inSingleQuotes))
                    tokenCount += 1;
            }
            return tokenCount;            
        }

        /// <summary>
        /// Ref. [http://en.wikipedia.org/wiki/Luhn_algorithm]
        /// </summary>
        /// <param name="somevalue"></param>
        /// <returns></returns>
        public static int CalcLuhnCheckDigit(string somevalue)
        {
            if (String.IsNullOrWhiteSpace(somevalue))
                return -1;

            var dblEveryOtherSum = 0;
            var valueChars = somevalue.ToCharArray().Where(Char.IsNumber).ToArray();
            if (valueChars.Length <= 0)
                return -1;

            for (var i = valueChars.Length - 1; i >= 0; i--)
            {
                var valAti = 0;
                if (!Int32.TryParse(new string(new[] {valueChars[i]}), out valAti))
                    continue;
                if (i % 2 == 0)
                {
                    dblEveryOtherSum += valAti;
                    continue;
                }

                var dblValAti = valAti * 2;
                switch (dblValAti)
                {
                    case 10:
                        dblEveryOtherSum += 1;
                        break;
                    case 12:
                        dblEveryOtherSum += 3;
                        break;
                    case 14:
                        dblEveryOtherSum += 5;
                        break;
                    case 16:
                        dblEveryOtherSum += 7;
                        break;
                    case 18:
                        dblEveryOtherSum += 9;
                        break;
                    default:
                        dblEveryOtherSum += dblValAti;
                        break;
                }
            }

            return 10 - (dblEveryOtherSum % 10);
        }

        /// <summary>
        /// Simple helper to calculate the values used in PowerShell's 
        /// Write-Progress cmdlet.
        /// </summary>
        /// <param name="counter"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static int CalcProgressCounter(int counter, int total)
        {
            if (total <= 0)
                return 0;
            var valout = (int)System.Math.Ceiling((counter / (double)(total)) * 100);
            return valout > 100 ? 100 : valout;
        }

        /// <summary>
        /// To insure that a CSV files headers are all unique and simple, propertyesque, names.
        /// </summary>
        /// <param name="someStrings"></param>
        /// <returns></returns>
        public static string[] FormatCsvHeaders(string[] someStrings)
        {
            var newNames = new List<string>();
            var names2Count = new Dictionary<string, int>();
            foreach (var s in someStrings)
            {
                if (names2Count.ContainsKey(s))
                    names2Count[s] += 1;
                else
                    names2Count.Add(s, 1);
            }

            foreach (var k in names2Count.Keys)
            {
                var repeated = names2Count[k];
                var newName = TypeName.SafeDotNetIdentifier(k);
                if (repeated > 1)
                {
                    for (var i = 0; i < names2Count[k]; i++)
                    {
                        newNames.Add(string.Format("{0}{1:00}", newName, i + 1));
                    }
                }
                else
                {
                    newNames.Add(newName);
                }
            }
            return newNames.ToArray();
        }

        /// <summary>
        /// Helper method to remove and replace specific line numbers with new content
        /// </summary>
        /// <param name="dido">The keys Item pair is the index and length, same as in the common <see cref="System.String.Substring(int, int)"/></param>
        /// <param name="srcFile"></param>
        /// <returns></returns>
        public static string[] ReplaceOriginalContent(Dictionary<Tuple<int, int>, string[]> dido, string[] srcFile)
        {
            var buffer = new List<string>();
            if (srcFile == null || srcFile.Length <= 0)
                return srcFile;

            var redux = 0;

            var leftPaddingByIdx = new Dictionary<Tuple<int, int>, string>();
            foreach (var nk in dido.Keys)
            {
                if (srcFile.Length <= nk.Item1 || nk.Item1 < 0)
                    continue;
                var targetedLine = srcFile[nk.Item1];
                var firstNonWsChar = targetedLine.ToCharArray().FirstOrDefault(x => !char.IsWhiteSpace(x));
                var lwsLen = firstNonWsChar != '\0' ? targetedLine.IndexOf(firstNonWsChar) : 8;
                var lws = new string((char)0x20, lwsLen);
                leftPaddingByIdx.Add(nk, lws);
            }

            foreach (var nk in dido.Keys)
            {
                //want to get this back to where Item1 would have been prior
                var topTake = nk.Item1 - redux; 
                var bottomTake = nk.Item2;

                buffer.AddRange(srcFile.Take(topTake));
                
                var lws = leftPaddingByIdx.ContainsKey(nk) ? leftPaddingByIdx[nk] : new string((char) 0x20, 8);

                var newLines = dido[nk];
                if (newLines != null)
                    buffer.AddRange(newLines.Select(x => string.Format("{0}{1}", lws, x)));
                buffer.AddRange(srcFile.Skip(topTake + bottomTake).Take(srcFile.Length - (topTake + bottomTake)));
                srcFile = buffer.ToArray();

                //this is the sum of all lines removed so far
                redux += (nk.Item2) //num of line just removed
                    - (newLines == null ? 0 : newLines.Length); //num of lines added back in
                buffer = new List<string>();
            }
            return srcFile;
        }

        /// <summary>
        /// Simple content spliter. Gets the content from the first line containing <see cref="startLnContains"/>
        /// up to the line containing <see cref="endLnContains"/>
        /// </summary>
        /// <param name="content"></param>
        /// <param name="startLnContains"></param>
        /// <param name="endLnContains">This line will NOT be included in the results</param>
        /// <returns></returns>
        public static string[] GetContentBetweenMarkers(string[] content, string startLnContains, string endLnContains)
        {
            var contentBetween = new List<string>();
            var getContent = false;
            foreach (var ln in content)
            {
                if (ln.Contains(startLnContains))
                    getContent = true;
                if (getContent && ln.Contains(endLnContains))
                    getContent = false;
                if (getContent)
                    contentBetween.Add(ln);
            }

            return contentBetween.ToArray();
        }
    }
}
