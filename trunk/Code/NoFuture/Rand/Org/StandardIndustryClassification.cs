﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Org
{
    /// <summary>
    /// This is an older economic grouping model which is still in use by the SEC.
    /// </summary>
    [Serializable]
    public class StandardIndustryClassification : NorthAmericanIndustryClassification
    {
        private static List<StandardIndustryClassification> _allSics;

        public override string Abbrev => "SIC";
        public bool SecResults { get; set; }

        public override bool TryThisParseXml(XmlElement elem)
        {
            const string VALUE = "value";
            const string SEC_RESULTS = "sec-results";
            if (!base.TryThisParseXml(elem))
                return false;

            var attr = elem.Attributes[VALUE];
            if (attr != null)
                Value = attr.Value;
            attr = elem.Attributes[SEC_RESULTS];
            if (attr != null)
            {
                if (bool.TryParse(attr.Value, out var nope))
                    SecResults = nope;
            }

            return true;
        }
        public override string LocalName => "sic-code";

        /// <summary>
        /// Selects one <see cref="StandardIndustryClassification"/> at random
        /// which has sec results.
        /// </summary>
        /// <param name="filterBy">
        /// Optional param to filter by
        /// </param>
        /// <returns></returns>
        [RandomFactory]
        public static StandardIndustryClassification RandomStandardIndustryClassification(
            Predicate<StandardIndustryClassification> filterBy = null)
        {
            var secSics = AllSicWithSecResults;
            if (filterBy == null)
                return secSics[Etx.RandomInteger(0, secSics.Length - 1)];

            secSics = secSics.Where(s => filterBy(s)).ToArray();
            return !secSics.Any() ? null : secSics[Etx.RandomInteger(0, secSics.Length - 1)];
        }

        /// <summary>
        /// Lists all <see cref="StandardIndustryClassification"/> which 
        /// have, in the past, returned something from an SEC Edgar search.
        /// </summary>
        public static StandardIndustryClassification[] AllSicWithSecResults
        {
            get
            {
                if (_allSics == null)
                {
                    _allSics = new List<StandardIndustryClassification>();
                    var allSectors = AllSectors;
                    if (allSectors == null)
                        return null;
                    _allSics.AddRange(AllSectors.SelectMany(
                        s =>
                            s.GetDivisions()
                                .SelectMany(
                                    ps =>
                                        ps.GetDivisions()
                                            .SelectMany(
                                                ns =>
                                                    ns.GetDivisions()
                                                        .SelectMany(
                                                            mk => mk.GetDivisions())
                                            )
                                )));
                }

                return _allSics.Where(x => x.SecResults).ToArray();
            }
        }

        /// <summary>
        /// Reverses a <see cref="StandardIndustryClassification"/> into 
        /// its NAICS groups.
        /// </summary>
        /// <param name="sic"></param>
        /// <returns></returns>
        public static Tuple<NaicsPrimarySector, NaicsSector, NaicsMarket> LookupNaicsBySic(
            StandardIndustryClassification sic)
        {
            var allSectors = AllSectors;
            if (allSectors == null)
                return null;
            foreach (var s in AllSectors)
            {
                foreach (var ps in s.GetDivisions())
                {
                    foreach (var ns in ps.GetDivisions())
                    {
                        foreach (var mk in ns.GetDivisions())
                        {
                            var ssic = mk.GetDivisions()
                                .FirstOrDefault(x => x.Equals(sic));
                            if (ssic == null)
                                continue;
                            return new Tuple<NaicsPrimarySector, NaicsSector, NaicsMarket>(ps, ns, mk);
                        }
                    }
                }
            }
            return null;
        }
    }
}