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
            var nm = new SocDetailedOccupation().LocalName;
            _soc2Prob = _soc2Prob ?? TreeData.GetProbTable(TreeData.UsOccupationProbTable, "occupations", nm);

            var pickOne = Etx.DiscreteRange(_soc2Prob) ?? DF_OCCUPATION;

            if (!_socs.Any())
                _socs.AddRange(AllGroups.SelectMany(g =>
                    g.Divisions.SelectMany(two => two.Divisions.SelectMany(three => three.Divisions))));

            return _socs.FirstOrDefault(s => s.Value == pickOne);
        }
    }
}
