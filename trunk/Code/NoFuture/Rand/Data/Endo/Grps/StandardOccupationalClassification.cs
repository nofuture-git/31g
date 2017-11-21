using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Data.Endo.Grps
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
        private const string DF_OCCUPATION = "41-2031";

        private static Dictionary<string, double> _soc2Prob = new Dictionary<string, double>();
        private static List<SocDetailedOccupation> _socs = new List<SocDetailedOccupation>();
        private static SocMajorGroup[] _majorGroups;

        public override string Abbrev => "SOC";

        public static SocMajorGroup[] AllGroups
        {
            get
            {
                if(_majorGroups != null)
                    return _majorGroups;

                if (TreeData.UsOccupations == null)
                    return null;

                var ssOut = new SocMajorGroup();

                var ssElements = TreeData.UsOccupations.SelectNodes($"//{ssOut.LocalName}");
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
        /// <returns></returns>
        public static SocDetailedOccupation RandomOccupation()
        {
            _soc2Prob = _soc2Prob == null || !_soc2Prob.Any()
                ? TreeData.GetProbTable(TreeData.UsOccupationProbTable, "occupations", "ID")
                : _soc2Prob;

            var pickOne = Etx.DiscreteRange(_soc2Prob) ?? DF_OCCUPATION;

            if (!_socs.Any())
                _socs.AddRange(AllGroups.SelectMany(g =>
                    g.Divisions.SelectMany(two => two.Divisions.SelectMany(three => three.Divisions))));

            return _socs.FirstOrDefault(s => s.Value == pickOne);
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
    }
}
