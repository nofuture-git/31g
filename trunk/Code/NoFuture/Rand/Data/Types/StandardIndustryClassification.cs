using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace NoFuture.Rand.Data.Types
{
    /// <summary>
    /// This is an older economic grouping model which is still in use by the SEC.
    /// </summary>
    [Serializable]
    public class StandardIndustryClassification : NorthAmericanIndustryClassification
    {
        private static List<StandardIndustryClassification> _allSics;

        public override string Abbrev { get { return "SIC"; } }
        public bool SecResults { get; set; }

        public override bool TryThisParseXml(XmlElement elem)
        {
            if (!base.TryThisParseXml(elem))
                return false;

            var attr = elem.Attributes["value"];
            if (attr != null)
                Value = attr.Value;
            attr = elem.Attributes["sec-results"];
            if (attr != null)
            {
                bool nope;
                if (bool.TryParse(attr.Value, out nope))
                    SecResults = nope;
            }

            return true;
        }
        public override string XmlLocalName
        {
            get { return "sic-code"; }
        }

        /// <summary>
        /// Selects one <see cref="StandardIndustryClassification"/> at random
        /// which has sec results.
        /// </summary>
        /// <returns></returns>
        public static StandardIndustryClassification RandomSic()
        {
            if (_allSics == null)
            {
                _allSics = new List<StandardIndustryClassification>();
                var allSectors = AllSectors;
                if (allSectors == null)
                    return null;
                _allSics.AddRange(AllSectors.SelectMany(
                    s =>
                        s.Divisions.Cast<NaicsSector>()
                            .SelectMany(
                                ns =>
                                    ns.Divisions.Cast<NaicsMarket>()
                                        .SelectMany(mk => mk.Divisions.Cast<StandardIndustryClassification>()))));
            }

            var withSecRslts = _allSics.Where(x => x.SecResults).ToArray();
            return withSecRslts[(Etx.Number(0, (withSecRslts.Length - 1)))];
        }

        public static Tuple<NaicsSuperSector, NaicsSector, NaicsMarket> LookupNaicsBySic(
            StandardIndustryClassification sic)
        {
            var allSectors = AllSectors;
            if (allSectors == null)
                return null;
            foreach (var s in AllSectors)
            {
                foreach (var ns in s.Divisions.Cast<NaicsSector>())
                {
                    foreach (var mk in ns.Divisions.Cast<NaicsMarket>())
                    {
                        var ssic = mk.Divisions.Cast<StandardIndustryClassification>()
                            .FirstOrDefault(x => x.Equals(sic));
                        if (ssic == null)
                            continue;
                        return new Tuple<NaicsSuperSector, NaicsSector, NaicsMarket>(s, ns, mk);
                    }
                }
            }
            return null;
        }
    }
}