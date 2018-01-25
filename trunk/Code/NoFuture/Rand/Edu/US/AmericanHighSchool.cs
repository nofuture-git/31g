using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Edu.US
{
    [Serializable]
    public class AmericanHighSchool : AmericanSchoolBase, IHighSchool
    {
        public const float DF_NATL_AVG = 82.0f;
        private static AmericanHighSchool _dfHs;
        private static string[] _allStateAbbreviations;

        #region properties
        public string PostalCode { get; set; }
        public UrbanCentric UrbanCentric { get; set; }
        public double TotalTeachers { get; set; }
        public int TotalStudents { get; set; }

        /// <summary>
        /// Common knowledge values of 4 years in high school
        /// </summary>
        public static NormalDistEquation YearsInHighSchool = new NormalDistEquation { Mean = 4, StdDev = 0.25 };
        #endregion

        #region methods
        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AmericanHighSchool hs))
                return false;

            var opc = hs.PostalCode ?? "";
            var pc = PostalCode ?? "";
            var oName = hs.Name ?? "";
            var name = Name ?? "";
            var oStateName = hs.StateName ?? "";
            var stateName = StateName ?? "";

            return string.Equals(opc, pc, StringComparison.OrdinalIgnoreCase)
                   && string.Equals(oName, name, StringComparison.OrdinalIgnoreCase)
                   && string.Equals(oStateName, stateName, StringComparison.OrdinalIgnoreCase)
                ;
        }

        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 1 +
                   PostalCode?.GetHashCode() ?? 1 +
                   StateName?.GetHashCode() ?? 1
                ;
        }

        /// <summary>
        /// [http://nces.ed.gov/programs/coe/indicator_coi.asp]
        /// </summary>
        /// <returns></returns>
        public static AmericanRacePercents NatlGradRate()
        {
            HsXml = HsXml ?? XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_HIGH_SCHOOL_DATA,
                Assembly.GetExecutingAssembly());
            return GetNatlGradRates(HsXml, DF_NATL_AVG);
        }

        public static AmericanHighSchool GetDefaultHs()
        {
            return _dfHs ?? (_dfHs = new AmericanHighSchool
            {
                StateName = "DC",
                Name = "G.E.D.",
                PostalCode = "20024",
                TotalTeachers = -1,
                UrbanCentric = UrbanCentric.City | UrbanCentric.Large,
                TotalStudents = -1
            });
        }

        /// <summary>
        /// Gets High Schools based on US State
        /// </summary>
        /// <param name="state">
        /// Gets either a specific states high schools or picks a state at random if missing.
        /// Use either the name or the postal code.
        /// </param>
        /// <returns></returns>
        public static AmericanHighSchool[] GetHighSchoolsByState(string state = null)
        {
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff} Start GetHighSchoolsByState");
            HsXml = HsXml ?? XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_HIGH_SCHOOL_DATA,
                Assembly.GetExecutingAssembly());
            if (HsXml == null)
                return new[] { GetDefaultHs() };
            if (String.IsNullOrWhiteSpace(state))
            {
                _allStateAbbreviations = _allStateAbbreviations ?? GetAllXmlStateAbbreviations(HsXml);
                state = GetRandomStateAbbrev(_allStateAbbreviations);
            }

            //have this deal with if its a postal code or a full name
            var usState = UsState.GetState(state);
            if(usState == null)
                return new[] { GetDefaultHs() };

            var elements =
                HsXml.SelectNodes($"//state[@name='{usState}']//high-school");

            var parsedList = ParseHighSchoolsFromNodeList(elements);
            foreach (var hs in parsedList)
            {
                hs.StateAbbrev = usState.StateAbbrev;
                hs.StateName = usState.ToString();
            }

            System.Diagnostics.Debug.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff} End GetHighSchoolsByState");
            return parsedList;
        }

        /// <summary>
        /// Gets High Schools based on US Postal Zip Code 
        /// </summary>
        /// <param name="zipCode">
        /// Either a 3 digit prefix or the full 5 digit code.
        /// </param>
        /// <returns></returns>
        public static AmericanHighSchool[] GetHighSchoolsByZipCode(string zipCode)
        {
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff} Start GetHighSchoolsByZipCode");
            zipCode = zipCode ?? "";
            zipCode = new string(zipCode.ToCharArray().Where(char.IsDigit).ToArray());
            if (string.IsNullOrWhiteSpace(zipCode))
                return GetHighSchoolsByState();

            HsXml = HsXml ?? XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_HIGH_SCHOOL_DATA,
                        Assembly.GetExecutingAssembly());
            if (HsXml == null)
                return new[] { GetDefaultHs() };

            var xpath = zipCode.Length == 3 
                        ? $"//zip-code[@prefix='{zipCode}']//high-school" 
                        : $"//zip-stat[@value='{zipCode}']/high-school";

            //just get this once for all 
            var stateXpath = $"{xpath}/../../..";
            var stateName = "";
            var stateAbbrev = "";
            var stateNode = HsXml.SelectSingleNode(stateXpath);
            if (stateNode?.Attributes != null)
            {
                var attr = stateNode.Attributes["name"];
                stateName = attr.Value;

                attr = stateNode.Attributes["abbreviation"];
                stateAbbrev = attr.Value;
            }

            var elements = HsXml.SelectNodes(xpath);
            if(elements == null || elements.Count <= 0)
                return GetHighSchoolsByState();

            var parsedList = ParseHighSchoolsFromNodeList(elements);
            foreach (var hs in parsedList)
            {
                hs.StateAbbrev = stateAbbrev;
                hs.StateName = stateName;
            }
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff} End GetHighSchoolsByZipCode");
            return parsedList;
        }

        /// <summary>
        /// Gets a highschool at random.
        /// </summary>
        /// <param name="state">
        /// Optional, limits the randomness to a single US State.  
        /// Use either the name or the postal code.
        /// </param>
        /// <returns></returns>
        [RandomFactory]
        public static AmericanHighSchool RandomHighSchool(string state = null)
        {
            var hss = GetHighSchoolsByState(state);
            if (!hss.Any())
                return GetDefaultHs();

            var pickOne = Etx.RandomInteger(0, hss.Length - 1);
            return hss[pickOne];
        }

        internal static bool TryParseXml(XmlElement node, out AmericanHighSchool hs)
        {
            try
            {
                hs = new AmericanHighSchool();
                if (node == null)
                    return false;

                if (node.LocalName != "high-school")
                    return false;
                var attr = node.Attributes["name"];
                hs.Name = attr == null ? String.Empty : attr.Value;
                
                attr = node.Attributes["urban-centric"];
                if (attr != null)
                {
                    switch (attr.Value)
                    {
                        case "City-Large":
                            hs.UrbanCentric = UrbanCentric.City | UrbanCentric.Large;
                            break;
                        case "City-Midsize":
                            hs.UrbanCentric = UrbanCentric.City | UrbanCentric.Large;
                            break;
                        case "City-Small":
                            hs.UrbanCentric = UrbanCentric.City | UrbanCentric.Small;
                            break;
                        case "Rural-Distant":
                            hs.UrbanCentric = UrbanCentric.Rural | UrbanCentric.Distant;
                            break;
                        case "Rural-Fringe":
                            hs.UrbanCentric = UrbanCentric.Rural | UrbanCentric.Fringe;
                            break;
                        case "Rural-Remote":
                            hs.UrbanCentric = UrbanCentric.Rural | UrbanCentric.Remote;
                            break;
                        case "Suburb-Large":
                            hs.UrbanCentric = UrbanCentric.Suburb | UrbanCentric.Large;
                            break;
                        case "Suburb-Midsize":
                            hs.UrbanCentric = UrbanCentric.Suburb | UrbanCentric.Midsize;
                            break;
                        case "Suburb-Small":
                            hs.UrbanCentric = UrbanCentric.Suburb | UrbanCentric.Small;
                            break;
                        case "Town-Distant":
                            hs.UrbanCentric = UrbanCentric.Town | UrbanCentric.Distant;
                            break;
                        case "Town-Fringe":
                            hs.UrbanCentric = UrbanCentric.Town | UrbanCentric.Fringe;
                            break;
                        case "Town-Remote":
                            hs.UrbanCentric = UrbanCentric.Town | UrbanCentric.Remote;
                            break;
                    }
                }

                attr = node.Attributes["teachers"];
                if (attr != null)
                {
                    if (Double.TryParse(attr.Value, out var dblOut))
                        hs.TotalTeachers = dblOut;
                }
                attr = node.Attributes["total-students"];
                if (attr != null)
                {
                    if (Int32.TryParse(attr.Value, out var intOut))
                        hs.TotalStudents = intOut;
                }

                if (node.ParentNode == null || node.ParentNode.LocalName != "zip-stat" ||
                    node.ParentNode.Attributes?["value"] == null)
                    return true;
                var zipStatNode = node.ParentNode;
                if (zipStatNode.Attributes != null)
                    attr = zipStatNode.Attributes["value"];
                hs.PostalCode = attr?.Value;

                hs.Name = Etc.CapWords(hs.Name, ' ');
                return true;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"{ex.Message}\n{ex.StackTrace}");
                hs = null;
                return false;
            }
        }

        private static AmericanHighSchool[] ParseHighSchoolsFromNodeList(XmlNodeList elements)
        {
            if (elements == null || elements.Count <= 0)
                return new[] { GetDefaultHs() };

            var tempList = new List<AmericanHighSchool>();
            foreach (var elem in elements)
            {
                if (TryParseXml(elem as XmlElement, out var hsOut))
                {
                    tempList.Add(hsOut);
                }
            }
            if (tempList.Count == 0)
                return new[] { GetDefaultHs() };

            return tempList.ToArray();
        }
        #endregion
    }
}
