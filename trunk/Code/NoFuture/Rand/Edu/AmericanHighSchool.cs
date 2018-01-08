using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Edu
{
    [Serializable]
    public class AmericanHighSchool : AmericanEduBase, IHighSchool
    {
        public const float DF_NATL_AVG = 82.0f;

        private static AmericanHighSchool _dfHs;

        #region properties
        public string PostalCode { get; set; }
        public UrbanCentric UrbanCentric { get; set; }
        public double TotalTeachers { get; set; }
        public int TotalStudents { get; set; }
        public AmericanRacePercents RacePercents { get; set; }
        #endregion

        #region methods
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// [http://nces.ed.gov/programs/coe/indicator_coi.asp]
        /// </summary>
        /// <returns></returns>
        public static AmericanRacePercents NatlGradRate()
        {
            return GetNatlGradRates(TreeData.AmericanHighSchoolData, DF_NATL_AVG);
        }

        public static AmericanHighSchool GetDefaultHs()
        {
            return _dfHs ?? (_dfHs = new AmericanHighSchool
            {
                State = UsState.GetStateByPostalCode("DC"),
                StateName = "",
                Name = "G.E.D.",
                RacePercents = AmericanRacePercents.GetNatlAvg(),
                PostalCode = "20024",
                TotalTeachers = -1,
                UrbanCentric = UrbanCentric.City | UrbanCentric.Large,
                TotalStudents = -1
            });
        }

        /// <summary>
        /// Uses the data in <see cref="TreeData.AmericanHighSchoolData"/>.
        /// </summary>
        public static AmericanHighSchool[] GetHighSchoolsByState(string stateName)
        {

            if(string.IsNullOrWhiteSpace(stateName))
                return new AmericanHighSchool[] { };

            if (TreeData.AmericanHighSchoolData == null)
                return new AmericanHighSchool[] { };

            var elements =
                TreeData.AmericanHighSchoolData.SelectNodes($"//state[@name='{stateName}']//high-school");
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

        public static bool TryParseXml(XmlElement node, out AmericanHighSchool hs)
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

                hs.RacePercents = new AmericanRacePercents();
                attr = node.Attributes["american-indian"];
                if (attr != null)
                {
                    if (double.TryParse(attr.Value, out var dblOut))
                        hs.RacePercents.AmericanIndian = dblOut;
                }
                attr = node.Attributes["asian"];
                if (attr != null)
                {
                    if (double.TryParse(attr.Value, out var dblOut))
                        hs.RacePercents.Asian = dblOut;
                }
                attr = node.Attributes["hispanic"];
                if (attr != null)
                {
                    if (double.TryParse(attr.Value, out var dblOut))
                        hs.RacePercents.Hispanic = dblOut;
                }
                attr = node.Attributes["black"];
                if (attr != null)
                {
                    if (double.TryParse(attr.Value, out var dblOut))
                        hs.RacePercents.Black = dblOut;
                }
                attr = node.Attributes["white"];
                if (attr != null)
                {
                    if (double.TryParse(attr.Value, out var dblOut))
                        hs.RacePercents.White = dblOut;
                }
                attr = node.Attributes["pacific"];
                if (attr != null)
                {
                    if (double.TryParse(attr.Value, out var dblOut))
                        hs.RacePercents.Pacific = dblOut;
                }
                attr = node.Attributes["mixed-race"];
                if (attr != null)
                {
                    if (double.TryParse(attr.Value, out var dblOut))
                        hs.RacePercents.Mixed = dblOut;
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
