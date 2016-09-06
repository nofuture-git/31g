using System;
using NoFuture.Rand.Data;

namespace NoFuture.Rand.Edu
{
    public interface IUniversity
    {
        string Name { get; set; }
        string CampusName { get; set; }
        float? CrimeRate { get; set; }
        Uri Website { get; set; }
    }

    [Serializable]
    public class AmericanUniversity : AmericanEduBase, IUniversity
    {
        public const float DF_NATL_AVG = 30.0f;

        #region properties
        public Gov.UsState State { get; set; }
        public string Name { get; set; }
        public string CampusName { get; set; }
        public float? PercentOfStateStudents { get; set; }
        public float? CrimeRate { get; set; }
        public Uri Website { get; set; }
        #endregion

        #region methods

        public override string ToString()
        {
            return string.Join(" ", Name, CampusName);
        }

        /// <summary>
        /// [http://www.census.gov/prod/2012pubs/p20-566.pdf]
        /// </summary>
        /// <returns></returns>
        public static AmericanRacePercents NatlGradRate()
        {
            return GetNatlGradRates(TreeData.AmericanUniversityData, DF_NATL_AVG);
        }

        public static bool TryParseXml(System.Xml.XmlElement node, out AmericanUniversity univ)
        {
            try
            {
                univ = null;
                if (node == null)
                    return false;
                if (node.LocalName != "college-univ")
                    return false;

                univ = new AmericanUniversity();
                var attr = node.Attributes["name"];
                univ.Name = attr == null ? string.Empty : attr.Value;

                attr = node.Attributes["campus"];
                univ.CampusName = attr == null ? string.Empty : attr.Value;

                attr = node.Attributes["percent-of-state-students"];

                if (attr != null)
                {
                    float percentStudents;
                    if (float.TryParse(attr.Value, out percentStudents))
                        univ.PercentOfStateStudents = percentStudents;
                }

                attr = node.Attributes["crime-rate"];
                if (attr != null)
                {
                    float crimeRate;
                    if (float.TryParse(attr.Value, out crimeRate))
                        univ.CrimeRate = crimeRate;
                }

                return true;
            }
            catch
            {
                univ = null;
                return false;
            }
        }
        #endregion
    }

    public class PublicAmericanUniversity : AmericanUniversity { }
    public class PrivateAmericanUniversity : AmericanUniversity { }
}
