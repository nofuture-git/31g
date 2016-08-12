using System;
using System.Collections.Generic;
using System.Xml;

namespace NoFuture.Rand.Data.Types
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
    public abstract class NorthAmericanIndustryClassification : XmlDocXrefIdentifier
    {
        #region fields

        protected readonly List<NorthAmericanIndustryClassification> divisions =
            new List<NorthAmericanIndustryClassification>();
        private static NaicsSuperSector[] _superSectors;
        #endregion

        #region properties
        public string Description { get; set; }
        public bool IsSuperSector => Value.Length == 2;
        public bool IsSector => Value.Length == 3;
        public bool IsIndustry => Value.Length == 4;
        public virtual List<NorthAmericanIndustryClassification> Divisions => divisions;
        public override string Abbrev => "NAICS";

        #endregion

        #region methods

        public override string ToString()
        {
            //Value Description
            return string.Join("-", Value, Description);
        }

        /// <summary>
        /// Translates the XML into the strongly typed <see cref="NorthAmericanIndustryClassification"/>
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public override bool TryThisParseXml(XmlElement elem)
        {
            if (!base.TryThisParseXml(elem))
                return false;

            var attr = elem.Attributes["Description"];
            if (attr != null)
                Description = attr.Value;
            return true;
        }

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
    public class NaicsSuperSector : NorthAmericanIndustryClassification
    {
        public override bool TryThisParseXml(XmlElement elem)
        {
            if (!base.TryThisParseXml(elem))
                return false;

            if (!elem.HasChildNodes)
                return true;

            foreach (var cNode in elem.ChildNodes)
            {
                var cElem = cNode as XmlElement;
                if (cElem == null)
                    continue;
                var sector = new NaicsPrimarySector();
                if (sector.TryThisParseXml(cElem))
                    divisions.Add(sector);
            }

            return true;
        }

        public override string LocalName => "category";
    }

    /// <summary>
    /// This represents the primary grouping level of the NAICS 
    /// </summary>
    [Serializable]
    public class NaicsPrimarySector : NorthAmericanIndustryClassification
    {

        public override bool TryThisParseXml(XmlElement elem)
        {
            if (!base.TryThisParseXml(elem))
                return false;

            if (!elem.HasChildNodes)
                return true;

            foreach (var cNode in elem.ChildNodes)
            {
                var cElem = cNode as XmlElement;
                if (cElem == null)
                    continue;
                var sector = new NaicsSector();
                if (sector.TryThisParseXml(cElem))
                    divisions.Add(sector);
            }

            return true;
        }

        public override string LocalName => "primary-sector";
    }

    [Serializable]
    public class NaicsSector : NorthAmericanIndustryClassification
    {
        public override bool TryThisParseXml(XmlElement elem)
        {
            if (!base.TryThisParseXml(elem))
                return false;
            if (!elem.HasChildNodes)
                return true;

            foreach (var cNode in elem.ChildNodes)
            {
                var cElem = cNode as XmlElement;
                if (cElem == null)
                    continue;
                var market = new NaicsMarket();
                if (market.TryThisParseXml(cElem))
                    divisions.Add(market);
            }

            return true;
        }

        public override string LocalName => "secondary-sector";
    }

    [Serializable]
    public class NaicsMarket : NorthAmericanIndustryClassification
    {
        public override bool TryThisParseXml(XmlElement elem)
        {
            if (!base.TryThisParseXml(elem))
                return false;
            if (!elem.HasChildNodes)
                return true;

            foreach (var cNode in elem.ChildNodes)
            {
                var cElem = cNode as XmlElement;
                if (cElem == null)
                    continue;
                var sic = new StandardIndustryClassification();
                if (sic.TryThisParseXml(cElem))
                    divisions.Add(sic);
            }

            return true;
        }
        public override string LocalName => "ternary-sector";
    }
}
