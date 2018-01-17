using System;
using System.Reflection;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Tele
{
    /// <inheritdoc />
    /// <summary>
    /// Base type to represent a telephone number
    /// </summary>
    [Serializable]
    public abstract class Phone : Identifier
    {
        public abstract string Notes { get; set; }
        public abstract string Formatted { get; }
        public abstract string Unformatted { get; }

        internal const string US_AREA_CODE_DATA = "US_AreaCode_Data.xml";
        internal const string CA_AREA_CODE_DATA = "CA_AreaCode_Data.xml";
        internal static XmlDocument UsAreaCodeXml;
        internal static XmlDocument CaAreaCodeXml;

        /// <summary>
        /// Gets a <see cref="NorthAmericanPhone"/> whose Area Code, Central Office and 
        /// Subscriber number are all random values.
        /// </summary>
        /// <remarks>
        /// Area Codes are of grammer [2-9][1-9][2-9].
        /// Central Office are of grammer [2-9] ('0' '0' | [1-9] [2-9] | [2-9] [1-9] )
        /// Subscriber number is of grammer [0-9][0-9][0-9][1-9]
        /// </remarks>
        /// <returns></returns>
        [RandomFactory]
        public static NorthAmericanPhone RandomAmericanPhone()
        {
            return new NorthAmericanPhone();
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
            return
                new NorthAmericanPhone(
                    new Tuple<NorthAmericanPhone.PhoneCodes, string>(NorthAmericanPhone.PhoneCodes.AreaCode,
                        GetRandomAreaCode("us", state)));
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
            return
                new NorthAmericanPhone(
                    new Tuple<NorthAmericanPhone.PhoneCodes, string>(NorthAmericanPhone.PhoneCodes.AreaCode,
                        GetRandomAreaCode("ca", providence)));
        }

        //same code only the resource changes
        private static string GetRandomAreaCode(string countryCode, string stateQry)
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
                stateQry = string.Join(" ", Etc.DistillToWholeWords(stateQry));
            }

            if(countryCode.ToLower() == "ca")
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
    }
}
