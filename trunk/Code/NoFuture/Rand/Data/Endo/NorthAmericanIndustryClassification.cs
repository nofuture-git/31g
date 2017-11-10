using System;
using System.Collections.Generic;
using System.Xml;

namespace NoFuture.Rand.Data.Endo
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

                if (TreeData.EconSectorData == null)
                    return null;

                var ssOut = new NaicsSuperSector();

                var ssElements = TreeData.EconSectorData.SelectNodes($"//{ssOut.LocalName}");
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

    [Serializable]
    public class NaicsSuperSector : ClassificationBase<NaicsPrimarySector>
    {
        public override string LocalName => "category";
    }

    /// <summary>
    /// This represents the primary grouping level of the NAICS 
    /// </summary>
    [Serializable]
    public class NaicsPrimarySector : ClassificationBase<NaicsSector>
    {
        public override string LocalName => "primary-sector";
    }

    [Serializable]
    public class NaicsSector : ClassificationBase<NaicsMarket>
    {
        public override string LocalName => "secondary-sector";
    }

    [Serializable]
    public class NaicsMarket : ClassificationBase<StandardIndustryClassification>
    {
        public override string LocalName => "ternary-sector";
    }
}
