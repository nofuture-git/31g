using System;
using System.Collections.Generic;
using System.Xml;
using NoFuture.Rand.Com;

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
        protected readonly List<NorthAmericanIndustryClassification> divisions  = new List<NorthAmericanIndustryClassification>();
        private static NaicsSuperSector[] _superSectors;
        #endregion

        #region properties
        public string Description { get; set; }
        public bool IsSuperSector { get { return Value.Length == 2; } }
        public bool IsSector { get { return Value.Length == 3; } }
        public bool IsIndustry { get { return Value.Length == 4; } }
        public virtual List<NorthAmericanIndustryClassification> Divisions { get { return divisions; } }
        public override string Abbrev { get { return "NAICS"; } }
        #endregion

        #region methods
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
        /// <remarks>
        /// Like all methods which are depenedent on XML data sources the 
        /// path must be resolved from the <see cref="NoFuture.BinDirectories.Root"/>.
        /// </remarks>
        public static NaicsSuperSector[] AllSectors
        {
            get
            {
                //return this a singleton
                if (_superSectors != null)
                    return _superSectors;

                //have to be able to resolve path to .xml to continue
                if (string.IsNullOrWhiteSpace(BinDirectories.Root))
                    return null;

                if (Data.TreeData.EconSectorData == null)
                    return null;

                var ssOut = new NaicsSuperSector();

                var ssElements = Data.TreeData.EconSectorData.SelectNodes(string.Format("//{0}", ssOut.LocalName));
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

    /// <summary>
    /// This represents the highest grouping level of the NAICS - this
    /// grouping is also applicable to the Bureau of Labor Statistics.
    /// </summary>
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
                var sector = new NaicsSector();
                if (sector.TryThisParseXml(cElem))
                    divisions.Add(sector);
            }

            return true;
        }

        public override string LocalName
        {
            get { return "primary-sector"; }
        }
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

        public override string LocalName
        {
            get { return "secondary-sector"; }
        }

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
        public override string LocalName
        {
            get { return "ternary-sector"; }
        }
    }
}
