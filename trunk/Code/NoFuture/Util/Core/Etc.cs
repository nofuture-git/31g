using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
        private const string LOREM_IPSUM_RSC = "NoFuture.Util.Core.Properties.LoremIpsum.EightParagraphs.txt";

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
        /// Helper method to transform a timespan into years
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static double ToYears(this TimeSpan ts)
        {
            if (ts == TimeSpan.MinValue)
                return 0D;

            var tr = Constants.TropicalYear;

            return ts.TotalMilliseconds / tr.TotalMilliseconds;
        }

        /// <summary>
        /// Gets the three char month abbrevation for the <see cref="d"/>
        /// similar to VB.NET DateAndTime.MonthName and R&apos;s month.abb constant
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string MonthAbbrev(this DateTime d)
        {
            var monthAbbrevs = new[]
            {
                "Jan", "Feb", "Mar",
                "Apr", "May", "Jun",
                "Jul", "Aug", "Sep",
                "Oct", "Nov", "Dec"
            };
            return monthAbbrevs[d.Month-1];
        }

        /// <summary>
        /// Utility method to convert a .NET DateTime type into Unix time
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int ToUnixTime(this DateTime dt)
        {
            var unixEpochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (Int32)(dt.ToUniversalTime().Subtract(unixEpochTime)).TotalSeconds;
        }

        /// <summary>
        /// Returns a date as a rational number (e.g. 2016.684476658052) 
        /// where day-of-year is divided by <see cref="Constants.DBL_TROPICAL_YEAR"/>
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double ToDouble(this DateTime d)
        {
            return (d.Year + (d.DayOfYear / Constants.DBL_TROPICAL_YEAR));
        }

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

        public static string[] BlankOutLines(string[] src, params int[] line2ColStartEnds)
        {
            if (src == null || !src.Any())
                return src;
            if (line2ColStartEnds == null || !line2ColStartEnds.Any())
                return src;

            var l2c = new List<Tuple<Tuple<int, int>, Tuple<int, int>>>();

            for (var i = 0; i < line2ColStartEnds.Length; i+=4)
            {
                if(i + 3 >= line2ColStartEnds.Length)
                    continue;
                var s = Tuple.Create(line2ColStartEnds[i], line2ColStartEnds[i + 1]);
                var e = Tuple.Create(line2ColStartEnds[i + 2], line2ColStartEnds[i + 3]);
                var se = Tuple.Create(s, e);
                l2c.Add(se);
            }

            return BlankOutLines(src, l2c);
        }

        /// <summary>
        /// Blanks out all characters in the source by the given start-stop pairs in <see cref="line2ColStartEnds"/>
        /// </summary>
        /// <param name="src"></param>
        /// <param name="line2ColStartEnds">
        /// A list of start and stop pairs where each item is the line-number, line-column-index 
        /// (e.g. (87,2), (138,2) would mean starting on the 87th line at character 2 blank out everything
        ///  until reaching line 138 at character 2).
        /// </param>
        /// <returns></returns>
        public static string[] BlankOutLines(string[] src, List<Tuple<Tuple<int, int>, Tuple<int, int>>> line2ColStartEnds)
        {
            const char BLANK_CHAR = ' ';
            if (src == null || !src.Any())
                return src;
            if (line2ColStartEnds == null || !line2ColStartEnds.Any())
                return src;
            var d = new SortedList<int, List<int>>();
            foreach (var ln in line2ColStartEnds.Where(x => x != null))
            {
                var startLnIdx = ln.Item1;
                var endLnIdx = ln.Item2;

                //only add them if there  is a pair
                if (startLnIdx == null || endLnIdx == null)
                    continue;

                if (!d.ContainsKey(startLnIdx.Item1))
                {
                    d.Add(startLnIdx.Item1, new List<int> { startLnIdx.Item2 });
                }
                else
                {
                    d[startLnIdx.Item1].Add(startLnIdx.Item2);
                }

                if (!d.ContainsKey(endLnIdx.Item1))
                {
                    d.Add(endLnIdx.Item1, new List<int> { endLnIdx.Item2 - 1 });
                }
                else
                {
                    d[endLnIdx.Item1].Add(endLnIdx.Item2 - 1);
                }
            }

            var lineOn = true;
            var srcLinesOut = new List<string>();
            for (var i = 0; i < src.Length; i++)
            {
                if (d.ContainsKey(i))
                {
                    var dil = d[i].OrderBy(x => x).Distinct().ToList();
                    var srcLn = src[i];
                    var newLn = new StringBuilder();
                    for (var k = 0; k < src[i].Length; k++)
                    {
                        newLn.Append(lineOn ? srcLn[k] : BLANK_CHAR);
                        if (dil.Contains(k))
                        {
                            lineOn = !lineOn;
                        }
                    }
                    srcLinesOut.Add(newLn.ToString());
                    continue;
                }
                srcLinesOut.Add(lineOn ? src[i] : new string(BLANK_CHAR, src[i].Length));
            }

            return srcLinesOut.ToArray();
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
                var newName = NfString.SafeDotNetIdentifier(k);
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

            var asType = JsonConvert.DeserializeObject(jsonString);

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
    }
}
