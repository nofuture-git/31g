using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Org
{
    /// <summary>
    /// The 2010 Standard Occupational Classification (SOC) system is used by 
    /// Federal statistical agencies to classify workers into occupational 
    /// categories for the purpose of collecting, calculating, or disseminating 
    /// data.
    /// </summary>
    /// <remarks>
    /// https://www.bls.gov/soc/
    /// </remarks>
    [Serializable]
    public abstract class StandardOccupationalClassification : ClassificationBase<SocMajorGroup>
    {
        //the highest percent 
        private const string DF_OCCUPATION = "41-2031";

        private static Dictionary<string, double> _soc2Prob = new Dictionary<string, double>();
        private static List<SocDetailedOccupation> _socs = new List<SocDetailedOccupation>();
        private static SocMajorGroup[] _majorGroups;
        private const string US_OCCUPATIONS_DATA_FILE = "US_Occupations_Data.xml";
        private const string US_OCCUPATIONS_PROB_TABLE = "US_Occupations_ProbTable.xml";
        internal static XmlDocument UsOccupationDataXml;
        internal static XmlDocument UsProbTableOccupationsXml;

        public override string Abbrev => "SOC";

        public static SocMajorGroup[] AllGroups
        {
            get
            {
                if(_majorGroups != null)
                    return _majorGroups;

                UsOccupationDataXml = UsOccupationDataXml ??
                                      GetEmbeddedXmlDoc(US_OCCUPATIONS_DATA_FILE, Assembly.GetExecutingAssembly());
                if (UsOccupationDataXml == null)
                    return null;

                var ssOut = new SocMajorGroup();

                var ssElements = UsOccupationDataXml.SelectNodes($"//{ssOut.LocalName}");
                if (ssElements == null || ssElements.Count == 0)
                    return null;

                var tempList = new List<SocMajorGroup>();
                foreach (var node in ssElements)
                {
                    var ssElem = node as XmlElement;
                    if (ssElem == null)
                        continue;
                    ssOut = new SocMajorGroup();
                    if (ssOut.TryThisParseXml(ssElem))
                        tempList.Add(ssOut);
                }
                _majorGroups = tempList.ToArray();
                return _majorGroups;
            }
        }

        /// <summary>
        /// Selects one <see cref="SocDetailedOccupation"/> at random
        /// </summary>
        /// <param name="filterBy">Optional predicate to limit the choices by something (e.g. Degree required).</param>
        /// <returns></returns>
        [RandomFactory]
        public static SocDetailedOccupation RandomOccupation(Predicate<SocDetailedOccupation> filterBy = null)
        {
            UsProbTableOccupationsXml = UsOccupationDataXml ??
                                        GetEmbeddedXmlDoc(US_OCCUPATIONS_PROB_TABLE, Assembly.GetExecutingAssembly());
            _soc2Prob = _soc2Prob == null || !_soc2Prob.Any()
                ? GetProbTable(UsProbTableOccupationsXml, "occupations", "ID")
                : _soc2Prob;

            if (!_socs.Any())
                _socs.AddRange(AllGroups.SelectMany(g =>
                    g.Divisions.SelectMany(two => two.Divisions.SelectMany(three => three.Divisions))));

            var pickOne = Etx.DiscreteRange(_soc2Prob) ?? DF_OCCUPATION;
            if(filterBy == null)
                return _socs.FirstOrDefault(s => s.Value == pickOne);

            var occ = _socs.FirstOrDefault(s => s.Value == pickOne);
            while (!filterBy(occ))
            {
                pickOne = Etx.DiscreteRange(_soc2Prob) ?? DF_OCCUPATION;
                occ = _socs.FirstOrDefault(s => s.Value == pickOne);
            }
            return occ;
        }

        /// <summary>
        /// Gets the occupation by its SOC id (e.g. 41-3011)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SocDetailedOccupation GetById(string id)
        {
            if (!_socs.Any())
                _socs.AddRange(AllGroups.SelectMany(g =>
                    g.Divisions.SelectMany(two => two.Divisions.SelectMany(three => three.Divisions))));

            return _socs.FirstOrDefault(s => s.Value == id);
        }

        /// <summary>
        /// Asserts if the given occupation is likely paid in wages
        /// </summary>
        /// <param name="soc"></param>
        /// <returns></returns>
        public static bool IsWages(SocDetailedOccupation soc)
        {
            return new[]
            {
                "27-10", "27-20", "27-402", "37", "35-20",
                "35-302", "35-9", "39-30", "39-50", "39-60","39-70",
                "39-90", "41", "43", "47", "49", "51", "53"
            }.Any(s =>
                soc.Value.StartsWith(s));
        }

        /// <summary>
        /// Asserts if the given occupation is likely paid in commission
        /// </summary>
        /// <param name="soc"></param>
        /// <returns></returns>
        public static bool IsCommissions(SocDetailedOccupation soc)
        {
            return new[] { "41-203", "41-30", "41-40", "41-90" }.Any(s => soc.Value.StartsWith(s));
        }

        /// <summary>
        /// Asserts if the given occupation is likely paid in tips
        /// </summary>
        /// <param name="soc"></param>
        /// <returns></returns>
        public static bool IsTips(SocDetailedOccupation soc)
        {
            return new[] { "35-301", "35-303", "35-304", "39-30" }.Any(s => soc.Value.StartsWith(s));
        }

        public static bool IsLaborUnion(SocDetailedOccupation soc)
        {
            return new[]
            {
                "25", "27-20", "29-11", "29-20", "47-20", "47-50", "49-30",
                "51-2", "51", "53-3", "53-4"
            }.Any(s => soc.Value.StartsWith(s));
        }

        public static bool IsDegreeRequired(SocDetailedOccupation soc)
        {
            return new[]
            {
                "11-903", "11-904", "11-911", "11-912", "13-201", "15-20", "17-20", "17-21", "17-1011", "19-102",
                "19-104", "19-20", "19-303", "19-304", "19-309", "21-201", "23-101", "25-1", "25-2", "25-402", "29-102",
                "29-104", "29-105", "29-106", "29-113", "29-114"
            }.Any(s => soc.Value.StartsWith(s));
        }
    }
}
