using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NfString = NoFuture.Util.Core.NfString;

namespace NoFuture.Rand.Tele
{
    /// <inheritdoc />
    /// <summary>
    /// Telephone numbers in North America based on the  North American Numbering Plan
    /// </summary>
    /// <remarks>
    /// see [https://en.wikipedia.org/wiki/North_American_Numbering_Plan]
    /// </remarks>
    [Serializable]
    public class NorthAmericanPhone : Phone
    {
        #region inner types
        [Serializable]
        public enum PhoneCodes
        {
            AreaCode,
            CentralOfficeCode,
            SubscriberCode
        }
        #endregion

        #region ctor

        public NorthAmericanPhone(params Tuple<PhoneCodes, string>[] parts)
        {
            AreaCode = parts.FirstOrDefault(x => x.Item1 == PhoneCodes.AreaCode)?.Item2 ?? "";
            CentralOfficeCode = parts.FirstOrDefault(x => x.Item1 == PhoneCodes.CentralOfficeCode)?.Item2 ??
                                 "";
            SubscriberNumber = parts.FirstOrDefault(x => x.Item1 == PhoneCodes.SubscriberCode)?.Item2 ??
                              "";
        }

        public NorthAmericanPhone()
        {
        }
        #endregion

        #region properties
        /// <summary>
        /// The first 3 digits of a telephone number
        /// </summary>
        public string AreaCode { get; private set; }

        /// <summary>
        /// The middle 3 digits of a telephone number
        /// </summary>
        public string CentralOfficeCode { get; private set; }

        /// <summary>
        /// The last 4 digits of a telephone number
        /// </summary>
        public string SubscriberNumber { get; private set; }

        public static IEnumerable<string> TollFreeAreaCodes = new[] { "800", "888", "877", "866", "855", "844" };

        public override string Value
        {
            get => Unformatted;
            set
            {
                if(!TryParse(value, out var phout))
                    throw new InvalidOperationException($"Cannot parse the value '{value}' into " +
                                                        "a North American phone number");
                AreaCode = phout.AreaCode;
                CentralOfficeCode = phout.CentralOfficeCode;
                SubscriberNumber = phout.SubscriberNumber;
            }
        }

        public override string Abbrev => "PH";

        /// <summary>
        /// Formatted as (###) ###-####
        /// the typical &apos;1&apos; before the area code is never included
        /// </summary>
        public override string Formatted => $"({AreaCode}) {CentralOfficeCode}-{SubscriberNumber}";

        /// <summary>
        /// All 10 digits of the telephone number with no additional chars
        /// </summary>
        public override string Unformatted => $"{AreaCode}{CentralOfficeCode}{SubscriberNumber}";

        public override string Notes { get; set; }
        #endregion

        #region methods

        public override bool Equals(object obj)
        {
            if (!(obj is NorthAmericanPhone ph))
                return false;

            var p = new[]
            {
                string.Equals(ph.AreaCode, AreaCode, StringComparison.OrdinalIgnoreCase),
                string.Equals(ph.CentralOfficeCode, CentralOfficeCode, StringComparison.OrdinalIgnoreCase),
                string.Equals(ph.SubscriberNumber, SubscriberNumber, StringComparison.OrdinalIgnoreCase)
            };
            return p.All(x => x);
        }

        public override int GetHashCode()
        {
            return (AreaCode?.ToLower().GetHashCode() ?? 0) +
                   (CentralOfficeCode?.ToLower().GetHashCode() ?? 0) +
                   (SubscriberNumber?.ToLower().GetHashCode() ?? 0);
        }

        /// <summary>
        /// Attempts to parse a string into a <see cref="NorthAmericanPhone"/>
        /// </summary>
        /// <param name="phNumber"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool TryParse(string phNumber, out NorthAmericanPhone phone)
        {
            string con;
            string sn;
            if (string.IsNullOrWhiteSpace(phNumber))
            {
                phone = null;
                return false;
            }

            var numerials = new List<char>();
            foreach (var c in phNumber.ToCharArray())
            {
                if (!char.IsNumber(c))
                    continue;
                numerials.Add(c);
            }

            if (numerials.Count < 7)
            {
                phone = null;
                return false;
            }

            if (numerials.Count == 7)
            {
                con = new string(numerials.GetRange(0, 3).ToArray());
                sn = new string(numerials.GetRange(3, 4).ToArray());
                phone = new NorthAmericanPhone(new Tuple<PhoneCodes, string>(PhoneCodes.CentralOfficeCode, con),
                    new Tuple<PhoneCodes, string>(PhoneCodes.SubscriberCode, sn));
                return true;
            }

            if (numerials.Count == 11 && numerials[0] == '1')
                numerials = numerials.GetRange(1, 10);

            var ac = new string(numerials.GetRange(0, 3).ToArray());
            con = new string(numerials.GetRange(3, 3).ToArray());
            sn = new string(numerials.GetRange(6, 4).ToArray());
            phone = new NorthAmericanPhone(new Tuple<PhoneCodes, string>(PhoneCodes.AreaCode, ac),
                new Tuple<PhoneCodes, string>(PhoneCodes.CentralOfficeCode, con),
                new Tuple<PhoneCodes, string>(PhoneCodes.SubscriberCode, sn));

            return true;
        }

        /// <summary>
        /// Attempts to parse a Uri into a <see cref="NorthAmericanPhone"/>
        /// </summary>
        /// <param name="phUri"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool TryParse(Uri phUri, out NorthAmericanPhone phone)
        {
            phone = null;
            if (phUri == null || phUri.Scheme != URI_SCHEMA_TELEPHONE)
                return false;

            var absPath = phUri.AbsolutePath;
            if (string.IsNullOrWhiteSpace(absPath))
                return false;
            var p = $";{PHONE_CONTEXT}=".EscapeString();
            p = $@"([0-9]+){p}([a-zA-Z0-9_\.\-]+)";

            var hasPhoneNumber = RegexCatalog.IsRegexMatch(absPath, p, out var phNum, 1);
            var hasDescriptor = RegexCatalog.IsRegexMatch(absPath, p, out var descriptor, 2);
            
            if (!hasPhoneNumber)
                return false;
            
            var isNAmerPhone = TryParse(phNum, out phone);
            if (!isNAmerPhone)
                return false;

            if (!hasDescriptor || string.IsNullOrWhiteSpace(descriptor))
                return true;

            descriptor = descriptor.Split('.').First();
            if (Enum.TryParse(descriptor, true, out KindsOfLabels lbl))
            {
                phone.Descriptor = lbl;
            }
            else
            {
                phone.Notes = descriptor;
            }

            return true;
        }

        /// <summary>
        /// Creates a random Area Code.
        /// Does not return any value ending in &apos;11&apos; nor
        /// any value in the <see cref="TollFreeAreaCodes"/>
        /// </summary>
        /// <returns></returns>
        protected internal static string GetRandomAreaCode()
        {
            var phstr = new StringBuilder();
            phstr.Append($"{Etx.MyRand.Next(2, 9)}");
            phstr.Append($"{Etx.MyRand.Next(12, 99):00}");
            var areaCode = phstr.ToString();
            if(TollFreeAreaCodes.Any(x => x == areaCode))
                areaCode = (Convert.ToInt32(areaCode) - 1).ToString(CultureInfo.InvariantCulture);

            return areaCode;
        }

        /// <summary>
        /// Creates a random Central Office Code.
        /// Does not return any value ending in &apos;11&apos;
        /// </summary>
        /// <returns></returns>
        protected internal static string GetRandomCentralOfficeCode()
        {
            var phstr = new StringBuilder();
            phstr.Append($"{Etx.MyRand.Next(2, 9)}");
            var subscriberRand = Etx.MyRand.Next(1, 100);
            switch (subscriberRand)
            {
                case 100:
                    phstr.Append("00");
                    break;
                case 1 - 49:
                    phstr.Append($"{Etx.MyRand.Next(1, 9)}");
                    phstr.Append($"{Etx.MyRand.Next(2, 9)}");
                    break;
                default:
                    phstr.Append($"{Etx.MyRand.Next(2, 9)}");
                    phstr.Append($"{Etx.MyRand.Next(1, 9)}");
                    break;
            }
            return phstr.ToString();
        }

        /// <summary>
        /// Gets a <see cref="NorthAmericanPhone"/> whose Area Code, Central Office and 
        /// Subscriber number are all random values.
        /// </summary>
        /// <remarks>
        /// Area Codes are of grammar [2-9][1-9][2-9].
        /// Central Office are of grammar [2-9] ('0' '0' | [1-9] [2-9] | [2-9] [1-9] )
        /// Subscriber number is of grammar [0-9][0-9][0-9][1-9]
        /// </remarks>
        /// <returns></returns>
        [RandomFactory]
        public static NorthAmericanPhone RandomAmericanPhone()
        {
            var d = new Tuple<PhoneCodes, string>(PhoneCodes.AreaCode, GetRandomAreaCode());
            var coc = new Tuple<PhoneCodes, string>(PhoneCodes.CentralOfficeCode, GetRandomCentralOfficeCode());
            var sc = new Tuple<PhoneCodes, string>(PhoneCodes.SubscriberCode, $"{Etx.MyRand.Next(1, 9999):0000}");

            return new NorthAmericanPhone(d, coc, sc);
        }

        /// <summary>
        /// Gets a <see cref="NorthAmericanPhone"/> whose area code is pertinent 
        /// to the given US State.
        /// </summary>
        /// <param name="state">Works with both the postal abbreviation and the full name</param>
        /// <returns></returns>
        [RandomFactory]
        public static NorthAmericanPhone RandomAmericanPhone(string state)
        {
            var d = new Tuple<PhoneCodes, string>(PhoneCodes.AreaCode, GetRandomAreaCode("us", state));
            var coc = new Tuple<PhoneCodes, string>(PhoneCodes.CentralOfficeCode, GetRandomCentralOfficeCode());
            var sc = new Tuple<PhoneCodes, string>(PhoneCodes.SubscriberCode, $"{Etx.MyRand.Next(1, 9999):0000}");

            return new NorthAmericanPhone(d, coc, sc); 
        }

        /// <summary>
        /// Gets a <see cref="NorthAmericanPhone"/> whose area code is pertinent 
        /// to the given Canadian Providence.
        /// </summary>
        /// <param name="providence">
        /// Works with both the postal abbreviation and the full name.
        /// Default is Ontario src [https://en.wikipedia.org/wiki/List_of_Canadian_provinces_and_territories_by_population]
        /// </param>
        /// <returns></returns>
        [RandomFactory]
        public static NorthAmericanPhone RandomCanadianPhone(string providence = "ON")
        {
            var d = new Tuple<PhoneCodes, string>(PhoneCodes.AreaCode, GetRandomAreaCode("ca", providence));
            var coc = new Tuple<PhoneCodes, string>(PhoneCodes.CentralOfficeCode, GetRandomCentralOfficeCode());
            var sc = new Tuple<PhoneCodes, string>(PhoneCodes.SubscriberCode, $"{Etx.MyRand.Next(1, 9999):0000}");

            return new NorthAmericanPhone(d, coc, sc);
        }

        //same code only the resource changes
        internal static string GetRandomAreaCode(string countryCode, string stateQry)
        {
            const string AREA_CODE_PLURAL = "area-codes";
            const string STATE = "state";
            const string PROVIDENCE = "providence";
            const string ABBREVIATION = "abbreviation";
            const string NAME = "name";

            XmlNode state;
            if (string.IsNullOrWhiteSpace(countryCode))
                countryCode = "us";

            var qryBy = stateQry.Length == 2 ? ABBREVIATION : NAME;

            if (qryBy == NAME)
            {
                stateQry = string.Join(" ", NfString.DistillToWholeWords(stateQry));
            }

            if (countryCode.ToLower() == "ca")
            {
                CaAreaCodeXml = CaAreaCodeXml ??
                                XmlDocXrefIdentifier.GetEmbeddedXmlDoc(CA_AREA_CODE_DATA,
                                    Assembly.GetExecutingAssembly());
                if (CaAreaCodeXml == null)
                    return null;
                state = CaAreaCodeXml.SelectSingleNode($"//{AREA_CODE_PLURAL}/{PROVIDENCE}[@{qryBy}='{stateQry}']");
            }
            else
            {
                UsAreaCodeXml = UsAreaCodeXml ??
                                XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_AREA_CODE_DATA,
                                    Assembly.GetExecutingAssembly());
                if (UsAreaCodeXml == null)
                    return null;
                state = UsAreaCodeXml.SelectSingleNode($"//{AREA_CODE_PLURAL}/{STATE}[@{qryBy}='{stateQry}']");
            }

            if (state == null)
                return null;

            var areaCodes = state.ChildNodes;
            if (areaCodes.Count == 0)
                return null;

            var pickone = Etx.MyRand.Next(0, areaCodes.Count);
            return areaCodes[pickone].InnerText;
        }

        #endregion
    }
}