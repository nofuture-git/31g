using System;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Data.Endo
{
    [Serializable]
    public abstract class CityArea : ICited
    {
        #region constants

        protected const string ZIP_CODE_SINGULAR = "zip-code";
        protected const string ZIP_CODE_PLURAL = "zip-codes";
        protected const string NAME = "name";
        protected const string VALUE = "value";
        protected const string PREFIX = "prefix";
        protected const string MSA_CODE = "msa-code";
        protected const string CBSA_CODE = "cbsa-code";
        protected const string ZIP_STAT = "zip-stat";

        #endregion

        #region fields
        protected readonly AddressData data;
        #endregion

        #region ctor
        protected CityArea(AddressData d)
        {
            data = d;
        }
        #endregion

        #region properties
        public virtual string Src { get; set; }
        public virtual AddressData AddressData => data;
        #endregion

        #region methods
        public virtual string GetPostalCodePrefix()
        {
            if (String.IsNullOrWhiteSpace(data?.PostalCode) || data.PostalCode.Length < 3)
                return null;
            return data.PostalCode.Substring(0, 3);

        }

        public override bool Equals(object obj)
        {
            var ca = obj as CityArea;
            return ca != null && AddressData.Equals(ca.AddressData);
        }

        public override int GetHashCode()
        {
            return AddressData.GetHashCode();
        }

        /// <summary>
        /// Fetches, at random, a <see cref="UsCityStateZip"/> by zip code prefix
        /// </summary>
        /// <param name="zipCodePrefix">
        /// Optional, will be picked at random with respect to population if whitespace or null.
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// Ranking is by population totals sourced from
        /// https://www.census.gov/geo/maps-data/data/docs/rel/zcta_cbsa_rel_10.txt
        /// </remarks>
        public static UsCityStateZip American(string zipCodePrefix = null)
        {
            const string HAS_HIGH_SCHOOL = "has-high-school";
            const string VALUE = "value";
            //set defaults
            var ctz = new AddressData
            {
                PostalCode = $"{UsCityStateZip.DF_ZIPCODE_PREFIX}{Etx.IntNumber(1, 99):00}",
                StateAbbrv = UsCityStateZip.DF_STATE_ABBREV
            };

            //pick a zip code prefix at random
            if (String.IsNullOrWhiteSpace(zipCodePrefix))
                zipCodePrefix = UsCityStateZip.RandomAmericanZipWithRespectToPop() ?? UsCityStateZip.DF_ZIPCODE_PREFIX;

            //x-ref it to the zip code data
            var xpathString = $"//{ZIP_CODE_PLURAL}//{ZIP_CODE_SINGULAR}[@{PREFIX}='{zipCodePrefix}']";
            var randZipCode =
                TreeData.AmericanZipCodeData.SelectSingleNode(xpathString);
            if (randZipCode?.ParentNode?.Attributes?[NAME] == null)
            {
                ctz.City = UsCityStateZip.DF_CITY_NAME;
                return new UsCityStateZip(ctz);
            }

            //get the containing us state
            ctz.StateName =  randZipCode.ParentNode.Attributes[NAME].Value;
            var nfState = UsState.GetStateByName(ctz.StateName) ??
                          UsState.GetStateByPostalCode(UsCityStateZip.DF_STATE_ABBREV);

            ctz.StateAbbrv = nfState.StateAbbrv ?? UsCityStateZip.DF_STATE_ABBREV;
            ctz.PostalCodeSuffix = $"{Etx.MyRand.Next(1, 9999):0000}";

            if (!randZipCode.HasChildNodes)
            {
                ctz.PostalCode = $"{zipCodePrefix}{Etx.IntNumber(1, 99):00}";
            }
            else
            {
                //pick a particular zip-code (ZIP5)
                var zipCodes =
                    randZipCode.ChildNodes.Cast<XmlElement>()
                        .Where(
                            x =>
                                x.Attributes[HAS_HIGH_SCHOOL] != null &&
                                x.Attributes[HAS_HIGH_SCHOOL].Value == bool.TrueString)
                        .Select(x => x.Attributes[VALUE].Value).ToArray();
                if (zipCodes.Length <= 0)
                    return new UsCityStateZip(ctz);
                var pickNum = Etx.IntNumber(0, zipCodes.Length - 1);
                ctz.PostalCode = zipCodes[pickNum];
            }
            return new UsCityStateZip(ctz);
        }

        /// <summary>
        /// Fetches, at random, a <see cref="Canadian"/> in which the values are only somewhat related.
        /// The Providence and City are related while the only first three characters of the Postal Code are actually realted.
        /// The last three are always '4Z4'.
        /// </summary>
        /// <returns></returns>
        public static CaCityProvidencePost Canadian()
        {
            const string POSTAL_CODE = "postal-code";
            const string DF_FIRST_THREE_CHARS = "M5A";
            const string DF_CITY = "Toronto";
            const string DF_LAST_THREE_CHARS = "4Z4";
            const string MUNICIPALITY = "municipality";
            const string ABBREVIATION = "abbreviation";

            var ctz = new AddressData();
            var postalCodes = TreeData.CanadianPostalCodeData.SelectNodes($"//{POSTAL_CODE}");
            var dfReturn = new CaCityProvidencePost(ctz);
            if (postalCodes == null)
                return dfReturn;

            var pickOne = Etx.MyRand.Next(0, postalCodes.Count);

            var randPostalCode = postalCodes[pickOne] as XmlElement;
            if(randPostalCode == null)
                return dfReturn;

            var providenceElem = randPostalCode.ParentNode as XmlElement;
            if (providenceElem == null)
                return dfReturn;

            ctz.StateAbbrv = providenceElem.GetAttribute(ABBREVIATION);
            ctz.StateName = providenceElem.GetAttribute(NAME);
            
            var postalPrefix = randPostalCode.GetAttribute(PREFIX);
            postalPrefix = string.IsNullOrWhiteSpace(postalPrefix) ? DF_FIRST_THREE_CHARS : postalPrefix;
            ctz.PostalCode = $"{postalPrefix} {DF_LAST_THREE_CHARS}";

            var municipalityNode = randPostalCode.ChildNodes.OfType<XmlElement>()
                .FirstOrDefault(x => x.LocalName == MUNICIPALITY && !string.IsNullOrWhiteSpace(x.InnerText));

            ctz.City = municipalityNode?.InnerText ?? DF_CITY;

            return new CaCityProvidencePost(ctz);
        }
        #endregion
    }
}
