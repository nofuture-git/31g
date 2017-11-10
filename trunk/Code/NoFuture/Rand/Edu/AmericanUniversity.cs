using System;
using System.Collections.Generic;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Edu
{
    [Serializable]
    public class AmericanUniversity : AmericanEduBase, IUniversity
    {

        public const double DF_NATL_BACHELORS_AVG = 33.0;

        /// <summary>
        /// src https://en.wikipedia.org/wiki/Educational_attainment_in_the_United_States
        /// </summary>
        public static Dictionary<OccidentalEdu, double> DefaultNationalAvgs = new Dictionary<OccidentalEdu, double>
        {
            {OccidentalEdu.Assoc | OccidentalEdu.Grad, 42.985},
            {OccidentalEdu.Bachelor | OccidentalEdu.Grad, DF_NATL_BACHELORS_AVG},
            {OccidentalEdu.Master | OccidentalEdu.Grad,  9.67 },
            {OccidentalEdu.Doctorate | OccidentalEdu.Grad, 2.485 }
        };

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
            return GetNatlGradRates(TreeData.AmericanUniversityData, DF_NATL_BACHELORS_AVG);
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
}
