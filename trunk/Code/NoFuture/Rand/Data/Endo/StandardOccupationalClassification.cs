using System;
using System.Collections.Generic;
using System.Xml;

namespace NoFuture.Rand.Data.Types
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
    }

    [Serializable]
    public class SocMajorGroup : ClassificationBase<SocMinorGroup>
    {
        public override string LocalName => "category";
    }

    [Serializable]
    public class SocMinorGroup : ClassificationBase<SocBoardGroup>
    {
        public override string LocalName => "primary-group";
    }

    [Serializable]
    public class SocBoardGroup : ClassificationBase<SocDetailedOccupation>
    {
        public override string LocalName => "secondary-group";
    }

    [Serializable]
    public class SocDetailedOccupation : ClassificationBase<ClassificationOfInstructionalPrograms>
    {
        public override string LocalName => "ternary-group";
    }

    /// <summary>
    /// The Classification of Instructional Programs (CIP) provides a taxonomic scheme 
    /// that supports the accurate tracking and reporting of fields of study 
    /// and program completions activity.
    /// </summary>
    /// <remarks>
    /// [https://nces.ed.gov/ipeds/cipcode/Default.aspx?y=55]
    /// </remarks>
    [Serializable]
    public class ClassificationOfInstructionalPrograms : StandardOccupationalClassification
    {
        public override string LocalName => "cip-code";

        public override string Abbrev => "CIP";
    }
}
