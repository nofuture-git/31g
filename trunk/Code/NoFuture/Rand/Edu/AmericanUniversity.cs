using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Gov;
using NoFuture.Util.Core.Math;

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
        public string CampusName { get; set; }
        public float? PercentOfStateStudents { get; set; }
        public float? CrimeRate { get; set; }
        public Uri Website { get; set; }
        #endregion

        #region methods


        /// <summary>
        /// [https://nscresearchcenter.org/signaturereport11/] has weighted average where Mean is 5.469 and StdDev is 0.5145
        /// however, these are current 21st century values not the averages for all post-war years.
        /// </summary>
        public static NormalDistEquation YearsInUndergradCollege = new NormalDistEquation { Mean = 4.469, StdDev = 0.5145 };

        /// <summary>
        /// Can't seem to find a straight answer on this cause post-grad can mean alot of different things
        /// </summary>
        public static NormalDistEquation YearsInPostgradCollege = new NormalDistEquation { Mean = 3.913, StdDev = 0.378 };

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
            var xml = Core.XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_UNIVERSITY_DATA,
                Assembly.GetExecutingAssembly());
            return GetNatlGradRates(xml, DF_NATL_BACHELORS_AVG);
        }

        public static AmericanUniversity[] GetUniversitiesByState(string stateName)
        {
            if(string.IsNullOrWhiteSpace(stateName))
                return new AmericanUniversity[] { };

            var xml = Core.XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_UNIVERSITY_DATA,
                Assembly.GetExecutingAssembly());
            //this will never pass so avoid the exception
            if (xml == null)
                return new AmericanUniversity[] { };

            var elements =
                xml.SelectSingleNode(
                    $"//state[@name='{stateName.ToUpper()}']") ??
                xml.SelectSingleNode($"//state[@name='{stateName}']");
            if (elements == null || !elements.HasChildNodes)
                return new AmericanUniversity[] { };

            var tempList = new List<AmericanUniversity>();
            foreach (var elem in elements)
            {
                AmericanUniversity univOut = null;
                if (TryParseXml(elem as XmlElement, out univOut))
                {
                    univOut.StateName = stateName;
                    tempList.Add(univOut);
                }
            }

            if (tempList.Count == 0)
                return new AmericanUniversity[] { };

            return tempList.ToArray();
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
                    if (float.TryParse(attr.Value, out var percentStudents))
                        univ.PercentOfStateStudents = percentStudents;
                }

                attr = node.Attributes["crime-rate"];
                if (attr != null)
                {
                    if (float.TryParse(attr.Value, out var crimeRate))
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
