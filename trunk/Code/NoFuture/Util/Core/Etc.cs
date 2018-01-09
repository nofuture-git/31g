using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using NoFuture.Shared.Core;
using Formatting = Newtonsoft.Json.Formatting;

namespace NoFuture.Util.Core
{
    /// <summary>
    /// This is your basic tool box of mildly useful functions.
    /// </summary>
    public static class Etc
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static Tuple<char, string>[] _xmlEscStrings;
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static Tuple<int, string>[] _htmlEscStrings;
        public const string DEFAULT_NAME_PREFIX = "_u0000";
        public static string DefaultNamePrefix { get; set; } = DEFAULT_NAME_PREFIX;

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

        /// <summary>
        /// Iterates each child node looking for a node name which matches <see cref="localName"/>
        /// </summary>
        /// <param name="node"></param>
        /// <param name="localName"></param>
        /// <param name="compareOptions"></param>
        /// <returns></returns>
        public static XmlNode FirstChildNamed(this XmlNode node, string localName, StringComparison compareOptions = StringComparison.Ordinal)
        {
            if (node == null || !node.HasChildNodes)
                return null;
            var cNode = node.FirstChild;
            while (cNode != null)
            {
                if (String.Equals(cNode.Name, localName, compareOptions))
                    return cNode;
                cNode = cNode.NextSibling;

            }
            return null;
        }

        /// <summary>
        /// Simple method to convert integer to ordinal 
        /// 1 goes to 1st, 2 goes to 2nd, 3 goes to 3rd, 4 goes to 4th, etc.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string ToOrdinal(this int v)
        {
            var absV = System.Math.Abs(v);
            switch (absV)
            {
                case 1:
                    return $"{v}st";
                case 2:
                    return $"{v}nd";
                case 3:
                    return $"{v}rd";
                default:
                    return v >= 1000 ? $"{v:n0}th" : $"{v}th";
            }
        }

        /// <summary>
        /// Returns a date as a rational number (e.g. 2016.684476658052) 
        /// where day of year is divided by <see cref="Constants.DBL_TROPICAL_YEAR"/>
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double ToDouble(this DateTime d)
        {
            return (d.Year + (d.DayOfYear / Constants.DBL_TROPICAL_YEAR));
        }

        private const string LOREM_IPSUM_RSC = "NoFuture.Util.Core.Properties.LoremIpsum.EightParagraphs.txt";

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
        public static string DistillString(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return null;
            var sb = new StringBuilder();
            var prev = (char)0x0;
            foreach (var c in value.ToCharArray())
            {
                if ((c == (char)0x20 || c == Constants.CR || c == Constants.LF) && prev == (char)0x20)
                    continue;
                if (c == Constants.CR || c == Constants.LF)
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
        /// Reduces all repeating sequence of tab-characters to a single-space.
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
            if (String.IsNullOrWhiteSpace(value))
                return null;
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
            fileContent = fileContent.Replace(new string(new[] {Constants.CR, Constants.LF}),
                new string(new[] {Constants.LF}));
            fileContent = fileContent.Replace(new string(new[] {Constants.CR}), new string(new[] {Constants.LF}));
            fileContent = fileContent.Replace(new string(new[] {Constants.LF}),
                new string(new[] {Constants.CR, Constants.LF}));
            return fileContent;
        }

        /// <summary>
        /// Reduces multiple blank lines interleaving one blank between 
        /// each line with content.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static string ToDoubleSpaceLines(string[] lines)
        {
            var linesOut = new StringBuilder();

            foreach (var ln in lines)
            {
                linesOut.Append(String.IsNullOrWhiteSpace(ln)
                    ? Environment.NewLine
                    : $"{ln}{Environment.NewLine}");
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

            value = ToPascelCase(value);
            value = TransformCaseToSeparator(value, NfConfig.DefaultCharSeparator);
            return value.Split(NfConfig.DefaultCharSeparator).Distinct().ToArray();
        }

        /// <summary>
        /// Returns string <see cref="value"/> as an escape sequence.
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
        /// Etc.EscapeString("I am unicode", EscapeStringType.UNICODE);//"\u2049\u6D61\u7520\u696E\u6F63\u6564"
        /// 
        /// Etc.EscapeString("[regex]", EscapeStringType.REGEX);//"\x5b\x72\x65\x67\x65\x78\x5d"
        /// 
        /// Etc.EscapeString(" £¡¥§", EscapeStringType.HTML);//"&nbsp;&pound;&iexcl;&yen;&sect;"
        /// ]]>
        /// </example>
        /// <returns></returns>
        public static string EscapeString(this string value, EscapeStringType escapeType = EscapeStringType.REGEX)
        {
            var data = Encoding.GetEncoding("ISO-8859-1").GetBytes(value);
            var dataOut = new StringBuilder();

            if (String.IsNullOrEmpty(value))
                return String.Empty;

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
                        dataOut.AppendFormat(@"\u{0}", Convert.ToUInt16(c).ToString("X4"));
                    }
                    break;
                case EscapeStringType.HTML:
                    var htmlEsc = HtmlEscStrings;
                    foreach (var dex in data)
                    {
                        if (htmlEsc.Select(t => t.Item1).Contains(dex))
                            dataOut.Append(htmlEsc.First(t => t.Item1 == dex).Item2);
                        else
                            dataOut.AppendFormat("&#{0};", dex);
                    }
                    break;
                case EscapeStringType.XML:
                    //turn any existing esc-seq back into char literals
                    var xmlEsc = XmlEscStrings;
                    foreach (var t in xmlEsc)
                    {
                        if (value.Contains(t.Item2))
                            value = value.Replace(t.Item2, t.Item1.ToString());
                    }

                    var chars = value.ToCharArray();
                    
                    foreach (var c in chars)
                    {
                        if (xmlEsc.Select(x => x.Item1).Contains(c))
                            dataOut.Append(xmlEsc.First(x => x.Item1 == c).Item2);
                        else
                            dataOut.Append(c);
                    }
                    break;
                case EscapeStringType.URI:
                    return HttpUtility.UrlEncode(value);
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

            var length = (primaryString?.Length ?? 0) > secondaryString.Length
                ? (primaryString?.Length ?? 0)
                : secondaryString.Length;

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
        public static string CapWords(string name, char? separator)
        {
            if (String.IsNullOrWhiteSpace(name))
                return String.Empty;

            if (separator == null)
                separator = NfConfig.DefaultTypeSeparator;

            name = name.Trim();
            name =
                new string(
                    name.ToCharArray()
                        .Where(
                            c =>
                                Char.IsLetterOrDigit(c) || Char.IsPunctuation(c) ||
                                (Char.IsWhiteSpace(c) && Char.IsWhiteSpace(separator.Value)))
                        .ToArray());


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
        /// Transforms a string of mixed case into standard camel-case (e.g. userName)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="perserveSep"></param>
        /// <returns></returns>
        public static string ToCamelCase(string name, bool perserveSep = false)
        {
            //is empty
            if (String.IsNullOrWhiteSpace(name))
                return String.Empty;

            name = name.Trim();

            //has no letters at all
            if (name.ToCharArray().All(x => !Char.IsLetter(x)))
                return name;

            //is all caps
            if (name.ToCharArray().Where(Char.IsLetter).All(Char.IsUpper))
                return name.ToLower();

            var nameFormatted = new StringBuilder();
            var markStart = false;
            var nameChars = name.ToCharArray();
            var sepChars = NfConfig.PunctuationChars.ToList();
            sepChars.Add(' ');
            for (var i = 0; i < nameChars.Length; i++)
            {
                var c = nameChars[i];

                if (sepChars.Contains(c))
                {
                    if (perserveSep)
                    {
                        nameFormatted.Append(c);
                        continue;
                    }
                    if (i + 1 < nameChars.Length)
                    {
                        nameChars[i + 1] = Char.ToUpper(nameChars[i + 1]);
                    }
                    continue;
                }

                if (!markStart)
                {
                    markStart = true;
                    nameFormatted.Append(c.ToString().ToLower());
                    continue;
                }

                if (i>0 &&  Char.IsUpper(nameChars[i-1]))
                {
                    nameFormatted.Append(c.ToString().ToLower());
                    continue;
                }

                nameFormatted.Append(c);

            }
            return nameFormatted.ToString();
        }

        /// <summary>
        /// Transforms <see cref="name"/> into Pascel case
        /// </summary>
        /// <param name="name"></param>
        /// <param name="perserveSep">Optional, set to true to have punctuation marks perserved</param>a
        /// <returns></returns>
        public static string ToPascelCase(string name, bool perserveSep = false)
        {
            if (String.IsNullOrWhiteSpace(name))
                return String.Empty;
            var toCamelCase = new StringBuilder();
            var charArray = ToCamelCase(name, perserveSep).ToCharArray();
            toCamelCase.Append(Char.ToUpper(charArray[0]));
            for (var i = 1; i < charArray.Length; i++)
            {
                toCamelCase.Append(charArray[i]);
            }
            return toCamelCase.ToString();
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
                separator = NfConfig.DefaultTypeSeparator;

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
        public static string TransformCaseToSeparator(string camelCaseString, char separator)
        {
            if (String.IsNullOrWhiteSpace(camelCaseString))
                return String.Empty;
            var separatorName = new StringBuilder();
            var charArray = camelCaseString.ToCharArray();
            for(var i = 0; i<charArray.Length;i++)
            {
                separatorName.Append(charArray[i]);
                if (i + 1 >= charArray.Length)
                    continue;
                if(Char.IsLower(charArray[i]) && Char.IsUpper(charArray[i+1]))
                {
                    separatorName.Append(separator);
                }
            }
            return separatorName.ToString();
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
            valueChars = valueChars.Reverse().ToArray();
            Debug.WriteLine($"Value passed into ChkDigit Fx in Reverse {String.Join("", valueChars)}");
            for (var i = 0; i < valueChars.Length; i++)
            {
                int valAti;
                if (!Int32.TryParse(valueChars[i].ToString(), out valAti))
                    continue;
                if ((i+1) % 2 == 0)
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

            var calc = 10 - (dblEveryOtherSum % 10);
            return calc == 10 ? 0 : calc;
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
        /// Reuseable method to get the diffence, in years, between
        /// <see cref="dob"/> and <see cref="atTime"/>
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="atTime"></param>
        /// <returns>
        /// The total number of days difference 
        /// divided by <see cref="Constants.DBL_TROPICAL_YEAR"/>,
        /// rounded off.
        /// </returns>
        public static int CalcAge(DateTime dob, DateTime? atTime = null )
        {
            var attTime = atTime.GetValueOrDefault(DateTime.Now);

            return Convert.ToInt32(System.Math.Round((attTime - dob).TotalDays / Constants.DBL_TROPICAL_YEAR));
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
                var newName = SafeDotNetIdentifier(k);
                if (repeated > 1)
                {
                    for (var i = 0; i < names2Count[k]; i++)
                    {
                        newNames.Add(String.Format("{0}{1:00}", newName, i + 1));
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
        /// Formats <see cref="jsonString"/> using Newtonsoft
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="replaceDblQuoteToSingle">
        /// Optional, to have existing single-quotes escaped followed by 
        /// having all double-quotes replaced to single-quotes.
        /// </param>
        /// <returns></returns>
        public static string FormatJson(string jsonString, bool replaceDblQuoteToSingle = false)
        {
            if (String.IsNullOrWhiteSpace(jsonString))
                return null;

            var hasOpenSqrBrace = jsonString.Contains("[");
            var hasCloseSqrBrace = jsonString.Contains("]");
            var hasOpenCurlyBrace = jsonString.Contains("{");
            var hasCloseCurlyBrace = jsonString.Contains("}");

            //may have in pairs or not at all
            var isValid = !(hasOpenSqrBrace ^ hasCloseSqrBrace) && !(hasOpenCurlyBrace ^ hasCloseCurlyBrace);
            if (!isValid)
                return null;

            var asType = JsonConvert.DeserializeObject<dynamic>(jsonString);

            jsonString = JsonConvert.SerializeObject(asType, Formatting.Indented);

            if (!replaceDblQuoteToSingle)
                return jsonString;

            //un-esc single-quote, esc single-quote, replace all dbl-quote to single
            var replacements = new List<Tuple<string, string>>
            {
                new Tuple<string, string>(@"\'", "'"),
                new Tuple<string, string>("'", @"\'"),
                new Tuple<string, string>("\"", "'")
            };
            foreach (var r in replacements)
                jsonString = jsonString.Replace(r.Item1, r.Item2);
            return jsonString;
        }

        /// <summary>
        /// Formats <see cref="xmlString"/> to the common look with new-lines and tabs.
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static string FormatXml(string xmlString)
        {
            if (String.IsNullOrWhiteSpace(xmlString))
                return null;

            var xml = new XmlDocument();
            xml.LoadXml(xmlString);
            using (var ms = new MemoryStream())
            {
                var xmlSettings = new XmlWriterSettings {Encoding = Encoding.UTF8, Indent = true};
                using (var xmlWriter = XmlWriter.Create(ms, xmlSettings))
                {
                    xml.WriteContentTo(xmlWriter);
                    xmlWriter.Flush();
                    xmlWriter.Close();
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        public static bool IsBetween(this DateTime t0, DateTime t1, DateTime t2)
        {
            var afterOrOnFromDt = t1 <= t0;
            var beforeOrOnToDt = t2 >= t0;
            return afterOrOnFromDt && beforeOrOnToDt;
        }

        /// <summary>
        /// The Jaro–Winkler distance (Winkler, 1990) is a measure of similarity between two strings.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="mWeightThreshold"></param>
        /// <param name="mNumChars"></param>
        /// <returns>1 means a perfect match, 0 means not match what-so-ever</returns>
        /// <remarks>
        /// [http://stackoverflow.com/questions/19123506/jaro-winkler-distance-algorithm-in-c-sharp]
        /// </remarks>
        public static double JaroWinklerDistance(this string a, string b, double mWeightThreshold = 0.7D,
            int mNumChars = 4)
        {
            a = a ?? "";
            b = b ?? "";
            var aLen = a.Length;
            var bLen = b.Length;
            if (aLen == 0)
                return bLen == 0 ? 1.0 : 0.0;

            var searchRng = System.Math.Max(0, System.Math.Max(aLen, bLen)/2 - 1);

            // default initialized to false
            var aMatched = new bool[aLen];
            var bMatched = new bool[bLen];

            var lNumCommon = 0;
            for (var i = 0; i < aLen; ++i)
            {
                var lStart = System.Math.Max(0, i - searchRng);
                var lEnd = System.Math.Min(i + searchRng + 1, bLen);
                for (var j = lStart; j < lEnd; ++j)
                {
                    if (bMatched[j])
                        continue;
                    if (a[i] != b[j])
                        continue;
                    aMatched[i] = true;
                    bMatched[j] = true;
                    ++lNumCommon;
                    break;
                }
            }
            if (lNumCommon == 0)
                return 0.0;

            var lNumHalfTrans = 0;
            var k = 0;
            for (var i = 0; i < aLen; ++i)
            {
                if (!aMatched[i])
                    continue;
                while (!bMatched[k]) ++k;
                {
                    if (a[i] != b[k])
                    {
                        ++lNumHalfTrans;
                    }
                }
                ++k;
            }
            // System.Diagnostics.Debug.WriteLine("numHalfTransposed=" + numHalfTransposed);
            var lNumTransposed = lNumHalfTrans/2;

            // System.Diagnostics.Debug.WriteLine("numCommon=" + numCommon + " numTransposed=" + numTransposed);
            double lNumCommonD = lNumCommon;
            var lWeight = (lNumCommonD/aLen
                           + lNumCommonD/bLen
                           + (lNumCommon - lNumTransposed)/lNumCommonD)/3.0;

            if (lWeight <= mWeightThreshold)
                return lWeight;
            var lMax = System.Math.Min(mNumChars, System.Math.Min(a.Length, b.Length));
            var lPos = 0;
            while (lPos < lMax && a[lPos] == b[lPos])
                ++lPos;
            if (lPos == 0)
                return lWeight;
            return lWeight + 0.1*lPos*(1.0 - lWeight);

        }

        /// <summary>
        /// A string metric for measuring the difference between two sequences.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <param name="asRatioOfMax">
        /// Optional, returns, as a ratio, as the difference from 1 the quotient 
        /// of the distance over the max possiable distance.
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Levenshtein_distance#Iterative_with_two_matrix_rows
        /// </remarks>
        public static double LevenshteinDistance(string s, string t, bool asRatioOfMax = false)
        {
            s = s ?? String.Empty;
            t = t ?? String.Empty;

            // degenerate cases
            if (s == t) return 0;
            if (s.Length == 0) return t.Length;
            if (t.Length == 0) return s.Length;

            // create two work vectors of integer distances
            var v0 = new int[t.Length + 1];
            var v1 = new int[t.Length + 1];

            // initialize v0 (the previous row of distances)
            // this row is A[0][i]: edit distance for an empty s
            // the distance is just the number of characters to delete from t
            for (var i = 0; i < v0.Length; i++)
                v0[i] = i;


            var ss = s.ToCharArray();
            var tt = t.ToCharArray();

            for (var i = 0; i < ss.Length; i++)
            {
                // calculate v1 (current row distances) from the previous row v0
                // first element of v1 is A[i+1][0]
                //   edit distance is delete (i+1) chars from s to match empty t
                v1[0] = i + 1;
                var si = ss[i];
                // use formula to fill in the rest of the row
                for (var j = 0; j < tt.Length; j++)
                {
                    var cost = si == tt[j] ? 0 : 1;
                    var j1 = v1[j] + 1;
                    var j2 = v0[j + 1] + 1;
                    var j3 = v0[j] + cost;

                    if (j1 < j2 && j1 < j3)
                    {
                        v1[j + 1] = j1;
                        continue;
                    }

                    v1[j + 1] = j2 < j3 ? j2 : j3;
                }

                // copy v1 (current row) to v0 (previous row) for next iteration
                Array.Copy(v1, v0, v0.Length);

            }

            if (!asRatioOfMax)
                return v1[t.Length];

            return 1D - (double)v1[t.Length]/new[] {t.Length, s.Length}.Max();
        }

        /// <summary>
        /// Of the possiable <see cref="candidates"/> returns those with the 
        /// shortest distance from <see cref="s"/> using the <see cref="LevenshteinDistance"/>
        /// algo.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="candidates"></param>
        /// <returns></returns>
        public static string[] ShortestDistance(this string s, IEnumerable<string> candidates)
        {
            if (String.IsNullOrWhiteSpace(s))
                return null;
            if (candidates == null || !candidates.Any())
                return null;
            var dict = new Dictionary<string, int>();

            foreach (var c in candidates.Distinct())
                dict.Add(c, (int)LevenshteinDistance(s, c));

            var minValue = dict.Values.Min();

            var df = dict.Where(x => x.Value == minValue);

            return df.Select(x => x.Key).ToArray();
        }

        /// <summary>
        /// Uses <see cref="name"/> allowing for only '.', '_', numbers and letters where the 
        /// first char must be '_' or a letter.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string SafeDotNetTypeName(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return GetNfRandomName();

            var nameArray = name.ToCharArray();
            var csId = new StringBuilder();

            if (!Char.IsLetter(nameArray[0]) || Convert.ToUInt16(nameArray[0]) > 0x7F)
                csId.Append(DefaultNamePrefix);

            if (Char.IsNumber(nameArray[0]) || nameArray[0] == '_' ||
                (Char.IsLetter(nameArray[0]) && Convert.ToUInt16(nameArray[0]) <= 0x7F))
                csId.Append(nameArray[0]);

            for (var i = 1; i < nameArray.Length; i++)
            {
                char cChar = nameArray[i];
                if ((Char.IsLetter(cChar) && Convert.ToUInt16(cChar) <= 0x7F) || Char.IsNumber(cChar) ||
                    cChar == '_' || cChar == '.') //expecting ns qualified ids
                    csId.Append(cChar);
            }

            return csId.ToString();
        }

        /// <summary>
        /// Uses <see cref="someString"/> allowing for only '_', numbers and letters where the 
        /// first char must be or a letter (the prefix 'nf' is used should that not be the case).
        /// </summary>
        /// <param name="someString"></param>
        /// <param name="replaceInvalidsWithUnicodeEsc">
        /// Set this to true if you want, for example, the <see cref="someString"/> 
        /// with a value of say 'Personal Ph #' to be returned as 'Personal_u0020Ph_u0020_u0023'
        /// </param>
        /// <param name="maxLen">
        /// A max length of the output, if the input string will 
        /// clearly exceed this then some random text will be added as the last 11 chars
        /// </param>
        /// <returns></returns>
        public static string SafeDotNetIdentifier(string someString, bool replaceInvalidsWithUnicodeEsc = false, int maxLen = 80)
        {
            if (String.IsNullOrWhiteSpace(someString))
                return GetNfRandomName();

            var strChars = someString.ToCharArray();
            var strOut = new StringBuilder();
            if (!Char.IsLetter(strChars[0]) || (Char.IsLetter(strChars[0]) && Convert.ToUInt16(strChars[0]) > 0x7F))
                strOut.Append(DefaultNamePrefix);
            var iequals = 0;
            if (!replaceInvalidsWithUnicodeEsc &&
                (Char.IsNumber(strChars[0]) || strChars[0] == '_' ||
                 (Char.IsLetter(strChars[0]) && Convert.ToUInt16(strChars[0]) <= 0x7F)))
            {
                strOut.Append(strChars[0]);
                iequals = 1;
            }

            var randSuffix = "_" + GetNfRandomName();

            for (var i = iequals; i < strChars.Length; i++)
            {
                if (strOut.Length + randSuffix.Length >= maxLen - 1)
                {
                    strOut.Append(randSuffix);
                    break;

                }
                if ((Char.IsLetterOrDigit(strChars[i]) && Convert.ToUInt16(strChars[i]) <= 0x7F) ||
                    (strChars[i] == '_' && !replaceInvalidsWithUnicodeEsc))
                {
                    strOut.Append(strChars[i]);
                    continue;
                }

                if (!replaceInvalidsWithUnicodeEsc)
                    continue;

                strOut.Append($"_u{Convert.ToUInt16(strChars[i]).ToString("x4")}");
            }

            return strOut.ToString();
        }

        public static string GetNfRandomName()
        {
            var randchars = Path.GetRandomFileName().Replace(".", "_");
            return $"{DefaultNamePrefix}_{randchars}";
        }


        /// <summary>
        /// Gets the five reserved chars of XML
        /// </summary>
        public static Tuple<char, string>[] XmlEscStrings
        {
            get
            {
                if (_xmlEscStrings == null)
                    _xmlEscStrings = new[]
                    {
                        new Tuple<char, string>('<', "&lt;"),
                        new Tuple<char, string>('>', "&gt;"),
                        new Tuple<char, string>('&', "&amp;"),
                        new Tuple<char, string>('"', "&quot;"),
                        new Tuple<char, string>('\'', "&apos;")
                    };
                return _xmlEscStrings;
            }
        }

        /// <summary>
        /// Gets HTML escape sequences as data bundle of integer value to escape sequence.
        /// <example>
        /// <![CDATA[
        /// 160 = "&nbsp;"
        /// ]]>
        /// </example>
        /// </summary>
        public static Tuple<int, string>[] HtmlEscStrings
        {
            get
            {
                if (_htmlEscStrings == null)
                    _htmlEscStrings = new[]
                    {
                        new Tuple<int, string>(162, "&cent;"),
                        new Tuple<int, string>(161, "&iexcl;"),
                        new Tuple<int, string>(160, "&nbsp;"),
                        new Tuple<int, string>(229, "&aring;"),
                        new Tuple<int, string>(202, "&Ecirc;"),
                        new Tuple<int, string>(227, "&atilde;"),
                        new Tuple<int, string>(197, "&Aring;"),
                        new Tuple<int, string>(225, "&aacute;"),
                        new Tuple<int, string>(255, "&yuml"),
                        new Tuple<int, string>(254, "&thorn;"),
                        new Tuple<int, string>(253, "&yacute;"),
                        new Tuple<int, string>(252, "&uuml;"),
                        new Tuple<int, string>(251, "&ucirc;"),
                        new Tuple<int, string>(250, "&uacute;"),
                        new Tuple<int, string>(249, "&ugrave;"),
                        new Tuple<int, string>(248, "&oslash;"),
                        new Tuple<int, string>(247, "&divide;"),
                        new Tuple<int, string>(246, "&ouml;"),
                        new Tuple<int, string>(245, "&otilde;"),
                        new Tuple<int, string>(244, "&ocirc;"),
                        new Tuple<int, string>(243, "&oacute;"),
                        new Tuple<int, string>(242, "&ograve;"),
                        new Tuple<int, string>(241, "&ntilde;"),
                        new Tuple<int, string>(240, "&eth;"),
                        new Tuple<int, string>(239, "&iuml;"),
                        new Tuple<int, string>(238, "&icirc;"),
                        new Tuple<int, string>(237, "&iacute;"),
                        new Tuple<int, string>(236, "&igrave;"),
                        new Tuple<int, string>(235, "&euml;"),
                        new Tuple<int, string>(234, "&ecirc;"),
                        new Tuple<int, string>(233, "&eacute;"),
                        new Tuple<int, string>(232, "&egrave;"),
                        new Tuple<int, string>(231, "&ccedil;"),
                        new Tuple<int, string>(230, "&aelig;"),
                        new Tuple<int, string>(223, "&szlig;"),
                        new Tuple<int, string>(228, "&auml;"),
                        new Tuple<int, string>(64, "&amp;"),
                        new Tuple<int, string>(226, "&acirc;"),
                        new Tuple<int, string>(62, "&gt;"),
                        new Tuple<int, string>(224, "&agrave;"),
                        new Tuple<int, string>(60, "&lt;"),
                        new Tuple<int, string>(222, "&THORN;"),
                        new Tuple<int, string>(221, "&Yacute;"),
                        new Tuple<int, string>(220, "&Uuml;"),
                        new Tuple<int, string>(219, "&Ucirc;"),
                        new Tuple<int, string>(218, "&Uacute;"),
                        new Tuple<int, string>(217, "&Ugrave;"),
                        new Tuple<int, string>(216, "&Oslash;"),
                        new Tuple<int, string>(215, "&times;"),
                        new Tuple<int, string>(214, "&Ouml;"),
                        new Tuple<int, string>(213, "&Otilde;"),
                        new Tuple<int, string>(212, "&Ocirc;"),
                        new Tuple<int, string>(211, "&Oacute;"),
                        new Tuple<int, string>(210, "&Ograve;"),
                        new Tuple<int, string>(209, "&Ntilde;"),
                        new Tuple<int, string>(208, "&ETH;"),
                        new Tuple<int, string>(207, "&Iuml;"),
                        new Tuple<int, string>(206, "&Icirc;"),
                        new Tuple<int, string>(205, "&Iacute;"),
                        new Tuple<int, string>(204, "&Igrave;"),
                        new Tuple<int, string>(203, "&Euml;"),
                        new Tuple<int, string>(39, "&apos;"),
                        new Tuple<int, string>(201, "&Eacute;"),
                        new Tuple<int, string>(200, "&Egrave;"),
                        new Tuple<int, string>(199, "&Ccedil;"),
                        new Tuple<int, string>(198, "&AElig;"),
                        new Tuple<int, string>(34, "&quot;"),
                        new Tuple<int, string>(196, "&Auml;"),
                        new Tuple<int, string>(195, "&Atilde;"),
                        new Tuple<int, string>(194, "&Acirc;"),
                        new Tuple<int, string>(193, "&Aacute;"),
                        new Tuple<int, string>(192, "&Agrave;"),
                        new Tuple<int, string>(191, "&iquest;"),
                        new Tuple<int, string>(190, "&frac34;"),
                        new Tuple<int, string>(189, "&frac12;"),
                        new Tuple<int, string>(188, "&frac14;"),
                        new Tuple<int, string>(187, "&raquo;"),
                        new Tuple<int, string>(186, "&ordm;"),
                        new Tuple<int, string>(185, "&sup1;"),
                        new Tuple<int, string>(184, "&cedil;"),
                        new Tuple<int, string>(183, "&middot;"),
                        new Tuple<int, string>(182, "&para;"),
                        new Tuple<int, string>(181, "&micro;"),
                        new Tuple<int, string>(180, "&acute;"),
                        new Tuple<int, string>(179, "&sup3;"),
                        new Tuple<int, string>(178, "&sup2;"),
                        new Tuple<int, string>(177, "&plusmn;"),
                        new Tuple<int, string>(176, "&deg;"),
                        new Tuple<int, string>(175, "&macr;"),
                        new Tuple<int, string>(174, "&reg;"),
                        new Tuple<int, string>(173, "&shy;"),
                        new Tuple<int, string>(172, "&not;"),
                        new Tuple<int, string>(171, "&laquo;"),
                        new Tuple<int, string>(170, "&ordf;"),
                        new Tuple<int, string>(169, "&copy;"),
                        new Tuple<int, string>(168, "&uml;"),
                        new Tuple<int, string>(167, "&sect;"),
                        new Tuple<int, string>(166, "&brvbar;"),
                        new Tuple<int, string>(165, "&yen;"),
                        new Tuple<int, string>(164, "&curren;"),
                        new Tuple<int, string>(163, "&pound;")
                    };
                return _htmlEscStrings;
            }
        }
    }
}
