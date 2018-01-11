using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace NoFuture.Rand.Org
{
    /// <summary>
    /// The NAICS is a model of economic sector groupings used by the US Census Bureau 
    /// which replaces the older Standard Industry Classification (SIC).  However,
    /// the SEC still groups firms by SIC codes. 
    /// </summary>
    /// <remarks>
    /// http://www.census.gov/cgi-bin/sssd/naics/naicsrch?chart=2012
    /// </remarks>
    [Serializable]
    public abstract class NorthAmericanIndustryClassification : ClassificationBase<NaicsSuperSector>
    {
        #region fields
        private static NaicsSuperSector[] _superSectors;
        private const string US_ECON_SECTOR_DATA_FILE = "US_EconSectors_Data.xml";
        internal static XmlDocument EconSectorXml;
        #endregion

        #region properties
        public bool IsSuperSector => Value.Length == 2;
        public bool IsSector => Value.Length == 3;
        public bool IsIndustry => Value.Length == 4;
        public override string Abbrev => "NAICS";

        #endregion

        #region methods


        /// <summary>
        /// Helper method that parses all the data within the 'US_EconSectors.xml' into the 
        /// strongly typed <see cref="NorthAmericanIndustryClassification"/>.
        /// </summary>
        public static NaicsSuperSector[] AllSectors
        {
            get
            {
                //return this a singleton
                if (_superSectors != null)
                    return _superSectors;
                EconSectorXml = EconSectorXml ??
                                GetEmbeddedXmlDoc(US_ECON_SECTOR_DATA_FILE, Assembly.GetExecutingAssembly());
                if (EconSectorXml == null)
                    return null;

                var ssOut = new NaicsSuperSector();

                var ssElements = EconSectorXml.SelectNodes($"//{ssOut.LocalName}");
                if (ssElements == null || ssElements.Count == 0)
                    return null;

                var tempList = new List<NaicsSuperSector>();
                foreach (var node in ssElements)
                {
                    var ssElem = node as XmlElement;
                    if (ssElem == null)
                        continue;
                    ssOut = new NaicsSuperSector();
                    if (ssOut.TryThisParseXml(ssElem))
                        tempList.Add(ssOut);
                }
                _superSectors = tempList.ToArray();
                return _superSectors;
            }
        }
        #endregion
    }
}
