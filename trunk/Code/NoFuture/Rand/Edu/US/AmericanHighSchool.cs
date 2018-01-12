using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using NoFuture.Rand.Gov;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Edu
{
    [Serializable]
    public class AmericanHighSchool : AmericanSchoolBase, IHighSchool
    {
        public const float DF_NATL_AVG = 82.0f;

        private static AmericanHighSchool _dfHs;

        #region properties
        public string PostalCode { get; set; }
        public UrbanCentric UrbanCentric { get; set; }
        public double TotalTeachers { get; set; }
        public int TotalStudents { get; set; }
        #endregion

        #region methods
        public override string ToString()
        {
            return Name;
        }


        /// <summary>
        /// Common knowledge values of 4 years in high school
        /// </summary>
        public static NormalDistEquation YearsInHighSchool = new NormalDistEquation { Mean = 4, StdDev = 0.25 };

        /// <summary>
        /// [http://nces.ed.gov/programs/coe/indicator_coi.asp]
        /// </summary>
        /// <returns></returns>
        public static AmericanRacePercents NatlGradRate()
        {
            HsXml = HsXml ?? Core.XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_HIGH_SCHOOL_DATA,
                Assembly.GetExecutingAssembly());
            return GetNatlGradRates(HsXml, DF_NATL_AVG);
        }

        public static AmericanHighSchool GetDefaultHs()
        {
            return _dfHs ?? (_dfHs = new AmericanHighSchool
            {
                State = UsState.GetStateByPostalCode("DC"),
                StateName = "",
                Name = "G.E.D.",
                PostalCode = "20024",
                TotalTeachers = -1,
                UrbanCentric = UrbanCentric.City | UrbanCentric.Large,
                TotalStudents = -1
            });
        }

        /// <summary>
        /// Uses the data in american high school data file
        /// </summary>
        public static AmericanHighSchool[] GetHighSchoolsByState(string stateName)
        {
            if(string.IsNullOrWhiteSpace(stateName))
                return new AmericanHighSchool[] { };
            HsXml = HsXml ?? Core.XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_HIGH_SCHOOL_DATA,
                Assembly.GetExecutingAssembly());
            if (HsXml == null)
                return new AmericanHighSchool[] { };
            stateName = string.Join(" ", Etc.DistillToWholeWords(stateName));
            var elements =
                HsXml.SelectNodes($"//state[@name='{stateName}']//high-school");
            if (elements == null || elements.Count <= 0)
                return new AmericanHighSchool[] { };

            var tempList = new List<AmericanHighSchool>();
            foreach (var elem in elements)
            {
                if (TryParseXml(elem as XmlElement, out var hsOut))
                {
                    hsOut.StateName = stateName;
                    tempList.Add(hsOut);
                }
            }
            if (tempList.Count == 0)
                return new AmericanHighSchool[] { };

            return tempList.ToArray();
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
                hs.Name = attr == null ? string.Empty : attr.Value;

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
                    if (double.TryParse(attr.Value, out var dblOut))
                        hs.TotalTeachers = dblOut;
                }
                attr = node.Attributes["total-students"];
                if (attr != null)
                {
                    if (int.TryParse(attr.Value, out var intOut))
                        hs.TotalStudents = intOut;
                }

                if (node.ParentNode == null || node.ParentNode.LocalName != "zip-stat" ||
                    node.ParentNode.Attributes?["value"] == null)
                    return true;
                var zipStatNode = node.ParentNode;
                if (zipStatNode.Attributes != null) attr = zipStatNode.Attributes["value"];
                hs.PostalCode = attr?.Value;

                return true;
            }
            catch
            {
                hs = null;
                return false;
            }
        }
        #endregion
    }
}
