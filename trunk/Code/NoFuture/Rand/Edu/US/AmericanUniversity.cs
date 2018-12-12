using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;
using NfString = NoFuture.Util.Core.NfString;

namespace NoFuture.Rand.Edu.US
{
    [Serializable]
    public class AmericanUniversity : AmericanSchoolBase, IUniversity
    {
        public const double DF_NATL_BACHELORS_AVG = 33.0;
        private static string[] _allStateAbbreviations;

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

        public override bool Equals(object obj)
        {
            if (!(obj is AmericanUniversity univ))
                return false;

            var isNameAndStateSame = string.Equals(univ.StateName, StateName, StringComparison.OrdinalIgnoreCase)
                   && string.Equals(univ.Name, Name, StringComparison.OrdinalIgnoreCase);

            var univCampus = univ.CampusName ?? "";
            var myCampus = CampusName ?? "";
            return isNameAndStateSame && string.Equals(univCampus, myCampus, StringComparison.CurrentCulture);
        }

        public override int GetHashCode()
        {
            return (Name?.GetHashCode() ?? 1) +
                   (CampusName?.GetHashCode() ?? 1) +
                   (StateName?.GetHashCode() ?? 1);
        }

        /// <summary>
        /// [http://www.census.gov/prod/2012pubs/p20-566.pdf]
        /// </summary>
        /// <returns></returns>
        public static AmericanRacePercents NatlGradRate()
        {
            UnivXml = UnivXml ?? Core.XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_UNIVERSITY_DATA,
                Assembly.GetExecutingAssembly());
            return GetNatlGradRates(UnivXml, DF_NATL_BACHELORS_AVG);
        }

        /// <summary>
        /// Gets the universities based on a US State
        /// </summary>
        /// <param name="state">Either the name or the postal code.</param>
        /// <returns></returns>
        public static AmericanUniversity[] GetUniversitiesByState(string state)
        {
            UnivXml = UnivXml ?? Core.XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_UNIVERSITY_DATA,
                Assembly.GetExecutingAssembly());
            //this will never pass so avoid the exception
            if (UnivXml == null)
                return new AmericanUniversity[] { };

            if (String.IsNullOrWhiteSpace(state))
            {
                _allStateAbbreviations = _allStateAbbreviations ?? GetAllXmlStateAbbreviations(UnivXml);
                state = GetRandomStateAbbrev(_allStateAbbreviations);
            }

            var qryBy = "name";
            if (state.Length == 2)
                qryBy = "abbreviation";
            else
                state = string.Join(" ", NfString.DistillToWholeWords(state));

            var elements =
                UnivXml.SelectSingleNode($"//state[@{qryBy}='{state}']");
            if (elements == null || !elements.HasChildNodes)
                return new AmericanUniversity[] { };

            var tempList = new List<AmericanUniversity>();
            foreach (var elem in elements)
            {
                if (TryParseXml(elem as XmlElement, out var univOut))
                {
                    univOut.StateName = state;
                    tempList.Add(univOut);
                }
            }

            if (tempList.Count == 0)
                return new AmericanUniversity[] { };

            return tempList.ToArray();
        }

        /// <summary>
        /// Gets a university at random.
        /// </summary>
        /// <param name="state">
        /// Optional, limits the randomness to a single US State.  
        /// Use either the name or the postal code.
        /// </param>
        /// <returns></returns>
        [RandomFactory]
        public static AmericanUniversity RandomUniversity(string state = null)
        {
            IUniversity univ = null;
            int pick;

            //pick a univ from the home state
            if (!string.IsNullOrWhiteSpace(state))
            {
                var stateUnivs = GetUniversitiesByState(state);
                if (stateUnivs.Any())
                {
                    pick = Etx.RandomInteger(0, stateUnivs.Length - 1);
                    univ = stateUnivs[pick];
                }
            }

            if (univ == null)
            {
                //pick a university from anywhere in the US
                UnivXml = UnivXml ?? XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_UNIVERSITY_DATA,
                              Assembly.GetExecutingAssembly());
                var allUnivs = UnivXml?.SelectNodes("//state");
                if (allUnivs == null)
                    return null;
                pick = Etx.RandomInteger(0, allUnivs.Count - 1);
                if (!(allUnivs[pick] is XmlElement randUnivXml) || !randUnivXml.HasChildNodes)
                    return null;
                pick = Etx.RandomInteger(0, randUnivXml.ChildNodes.Count - 1);
                if (!(randUnivXml.ChildNodes[pick] is XmlElement univXmlNode))
                    return null;
                if (TryParseXml(univXmlNode, out var univOut))
                {
                    univ = univOut;
                }
            }
            return (AmericanUniversity)univ;
        }

        internal static bool TryParseXml(XmlElement node, out AmericanUniversity univ)
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

                if (!(node.ParentNode is XmlElement stateNode) || stateNode.LocalName != "state")
                    return true;

                attr = stateNode.Attributes["name"];
                univ.StateName = attr == null ? string.Empty : attr.Value;

                attr = stateNode.Attributes["abbreviation"];
                univ.StateAbbrev = attr == null ? string.Empty : attr.Value;

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
