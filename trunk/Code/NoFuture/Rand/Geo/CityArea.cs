using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Geo.CA;
using NoFuture.Rand.Geo.US;
using NoFuture.Rand.Gov.US;

namespace NoFuture.Rand.Geo
{
    /// <inheritdoc cref="GeoBase"/>
    /// <summary>
    /// Base type representing the second half of a typical Postal Address
    /// </summary>
    [Serializable]
    public abstract class CityArea : GeoBase, ICited, IObviate
    {
        #region constants
        internal const string NAME = "name";
        internal const string VALUE = "value";
        internal const string PREFIX = "prefix";
        #endregion

        protected CityArea(AddressData d) :base(d)
        {
        }

        #region properties

        public virtual string Src { get; set; }
        public string City => GetData().Locality;
        public string Country => GetData().NationState;

        #endregion

        #region methods

        public virtual string GetRegionAbbrev()
        {
            return GetData().RegionAbbrev ?? "";
        }

        public virtual string GetPostalCodePrefix()
        {
            if (String.IsNullOrWhiteSpace(GetData().PostalCode) || GetData().PostalCode.Length < 3)
                return null;
            return GetData().PostalCode.Substring(0, 3);

        }

        public override bool Equals(object obj)
        {
            var ca = obj as CityArea;
            return ca != null && GetData().Equals(ca.GetData());
        }

        public override int GetHashCode()
        {
            return GetData().GetHashCode();
        }

        public abstract IDictionary<string, object> ToData(KindsOfTextCase txtCase);


        /// <summary>
        /// Fetches, at random, a <see cref="UsCityStateZip"/> by zip code prefix
        /// </summary>
        /// <param name="zipCodePrefix">
        /// Optional, will be picked at random with respect to population if whitespace or null.
        /// </param>
        /// <param name="pickSuburbAtRandom">
        /// Optional, will pick the actual metro city itself or one if its surrounding suburbs at random.
        /// Set to false to force just the major metro city.
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// Ranking is by population totals sourced from
        /// https://www.census.gov/geo/maps-data/data/docs/rel/zcta_cbsa_rel_10.txt
        /// </remarks>
        [RandomFactory]
        public static UsCityStateZip RandomAmericanCity(string zipCodePrefix = null, bool pickSuburbAtRandom = true)
        {
            const string HAS_HIGH_SCHOOL = "has-high-school";
            const string VALUE = "value";
            //set defaults
            var ctz = new AddressData
            {
                PostalCode = $"{UsCityStateZip.DF_ZIPCODE_PREFIX}{Etx.RandomInteger(1, 99):00}",
                RegionAbbrev = UsCityStateZip.DF_STATE_ABBREV
            };

            //pick a zip code prefix at random
            if (String.IsNullOrWhiteSpace(zipCodePrefix))
                zipCodePrefix = UsCityStateZip.RandomAmericanPartialZipCode() ?? UsCityStateZip.DF_ZIPCODE_PREFIX;

            //x-ref it to the zip code data
            var xpathString = $"//{UsCityStateZip.ZIP_CODE_PLURAL}//{UsCityStateZip.ZIP_CODE_SINGULAR}[@{PREFIX}='{zipCodePrefix}']";
            UsCityStateZip.UsZipCodeXml = UsCityStateZip.UsZipCodeXml ??
                            XmlDocXrefIdentifier.GetEmbeddedXmlDoc(UsCityStateZip.US_ZIP_CODE_DATA, Assembly.GetExecutingAssembly());
            if (UsCityStateZip.UsZipCodeXml == null)
                return null;
            var randZipCode = UsCityStateZip.UsZipCodeXml.SelectSingleNode(xpathString);
            if (randZipCode?.ParentNode?.Attributes?[NAME] == null)
            {
                ctz.Locality = UsCityStateZip.DF_CITY_NAME;
                return new UsCityStateZip(ctz);
            }

            //get the containing us state
            ctz.RegionName =  randZipCode.ParentNode.Attributes[NAME].Value;
            var nfState = UsState.GetStateByName(ctz.RegionName) ??
                          UsState.GetStateByPostalCode(UsCityStateZip.DF_STATE_ABBREV);

            ctz.RegionAbbrev = nfState.StateAbbrev ?? UsCityStateZip.DF_STATE_ABBREV;
            ctz.SortingCode = $"{Etx.MyRand.Next(1, 9999):0000}";

            if (!randZipCode.HasChildNodes)
            {
                ctz.PostalCode = $"{zipCodePrefix}{Etx.RandomInteger(1, 99):00}";
            }
            else
            {
                //pick a particular zip-code (ZIP5)
                var zipCodes =
                    randZipCode.ChildNodes.Cast<XmlElement>()
                        .Where(
                            x =>
                                x.Attributes[HAS_HIGH_SCHOOL] != null &&
                                x.Attributes[HAS_HIGH_SCHOOL].Value == Boolean.TrueString)
                        .Select(x => x.Attributes[VALUE].Value).ToArray();
                if (zipCodes.Length <= 0)
                    return new UsCityStateZip(ctz);
                var pickNum = Etx.RandomInteger(0, zipCodes.Length - 1);
                ctz.PostalCode = zipCodes[pickNum];
            }
            var rr =  new UsCityStateZip(ctz);

            rr.SetAddrDataToXmlValues(pickSuburbAtRandom);
            return rr;
        }

        /// <summary>
        /// Fetches, at random, a <see cref="RandomCanadianCity"/> in which the values are only somewhat related.
        /// The Providence and City are related while the only first three characters of the Postal Code are actually realted.
        /// The last three are always '4Z4'.
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static CaCityProvidencePost RandomCanadianCity()
        {
            var ctz = new AddressData();
            CaCityProvidencePost.CaPostCodeXml = CaCityProvidencePost.CaPostCodeXml ??
                             XmlDocXrefIdentifier.GetEmbeddedXmlDoc(CaCityProvidencePost.CA_POST_CODE_DATA, Assembly.GetExecutingAssembly());
            if (CaCityProvidencePost.CaPostCodeXml == null)
                return null;
            var postalCodes = CaCityProvidencePost.CaPostCodeXml.SelectNodes($"//{CaCityProvidencePost.POSTAL_CODE}");
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

            ctz.RegionAbbrev = providenceElem.GetAttribute(CaCityProvidencePost.ABBREVIATION);
            ctz.RegionName = providenceElem.GetAttribute(NAME);
            
            var postalPrefix = randPostalCode.GetAttribute(PREFIX);
            postalPrefix = String.IsNullOrWhiteSpace(postalPrefix) ? CaCityProvidencePost.DF_FIRST_THREE_CHARS : postalPrefix;
            ctz.PostalCode = $"{postalPrefix} {CaCityProvidencePost.DF_LAST_THREE_CHARS}";

            var municipalityNode = randPostalCode.ChildNodes.OfType<XmlElement>()
                .FirstOrDefault(x => x.LocalName == CaCityProvidencePost.MUNICIPALITY && !string.IsNullOrWhiteSpace(x.InnerText));

            ctz.Locality = municipalityNode?.InnerText ?? CaCityProvidencePost.DF_CITY;

            return new CaCityProvidencePost(ctz);
        }
        #endregion
    }
}
