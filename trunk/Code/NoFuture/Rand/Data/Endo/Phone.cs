using System;
using System.Xml;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Data.Endo
{
    [Serializable]
    public abstract class Phone : Identifier
    {
        public abstract string Notes { get; set; }
        public abstract string Formatted { get; }
        public abstract string Unformatted { get; }

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
        public static NorthAmericanPhone American()
        {
            return new NorthAmericanPhone();
        }

        /// <summary>
        /// Gets a <see cref="NorthAmericanPhone"/> whose area code is pertinent 
        /// to the given US State.
        /// </summary>
        /// <param name="stateCode">The two-character US Postal Code for the given State.</param>
        /// <returns></returns>
        public static NorthAmericanPhone American(string stateCode)
        {
            return
                new NorthAmericanPhone(
                    new Tuple<NorthAmericanPhone.PhoneCodes, string>(NorthAmericanPhone.PhoneCodes.AreaCode,
                        GetAreaCode(Gov.NationCodes.UNITED_STATES, stateCode)));
        }

        /// <summary>
        /// Gets a <see cref="NorthAmericanPhone"/> whose area code is pertinent 
        /// to the given Canadian Providence.
        /// </summary>
        /// <param name="providenceCode">The two-character Canadian Postal Code for the given Providence.</param>
        /// <returns></returns>
        public static NorthAmericanPhone Canadian(string providenceCode)
        {
            return
                new NorthAmericanPhone(
                    new Tuple<NorthAmericanPhone.PhoneCodes, string>(NorthAmericanPhone.PhoneCodes.AreaCode,
                        GetAreaCode(Gov.NationCodes.CANADA, providenceCode)));
        }

        //same code only the resource changes
        private static string GetAreaCode(string countryCode, string stateCode)
        {
            const string AREA_CODE_PLURAL = "area-codes";
            const string STATE = "state";
            const string PROVIDENCE = "providence";
            const string ABBREVIATION = "abbreviation";
            XmlNode state;
            if (string.IsNullOrWhiteSpace(countryCode))
                countryCode = Gov.NationCodes.UNITED_STATES;

            if(countryCode.ToLower() == Gov.NationCodes.CANADA)
                state = TreeData.CanadianAreaCodeData.SelectSingleNode(
                    $"//{AREA_CODE_PLURAL}/{PROVIDENCE}[@{ABBREVIATION}='{stateCode}']");
            else
                state = TreeData.AmericanAreaCodeData.SelectSingleNode(
                    $"//{AREA_CODE_PLURAL}/{STATE}[@{ABBREVIATION}='{stateCode}']");    
            
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
