using System;
using System.Linq;

namespace NoFuture.Rand.Edu
{
    public interface IHighSchool
    {
        string Name { get; set; }
        UrbanCentric UrbanCentric { get; set; }
        double TotalTeachers { get; set; }
        int TotalStudents { get; set; }
    }

    [Serializable]
    public class AmericanHighSchool : IHighSchool
    {
        public Gov.UsState State { get; set; }
        public string PostalCode { get; set; }
        public string Name { get; set; }
        public UrbanCentric UrbanCentric { get; set; }
        public double TotalTeachers { get; set; }
        public int TotalStudents { get; set; }
        public AmericanRacePercents RacePercents { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public static bool TryParseXml(System.Xml.XmlElement node, out AmericanHighSchool hs)
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
                            hs.UrbanCentric = UrbanCentric.CityLarge;
                            break;
                        case "City-Midsize":
                            hs.UrbanCentric = UrbanCentric.CityLarge;
                            break;
                        case "City-Small":
                            hs.UrbanCentric = UrbanCentric.CitySmall;
                            break;
                        case "Rural-Distant":
                            hs.UrbanCentric = UrbanCentric.RuralDistant;
                            break;
                        case "Rural-Fringe":
                            hs.UrbanCentric = UrbanCentric.RuralFringe;
                            break;
                        case "Rural-Remote":
                            hs.UrbanCentric = UrbanCentric.RuralRemote;
                            break;
                        case "Suburb-Large":
                            hs.UrbanCentric = UrbanCentric.SuburbLarge;
                            break;
                        case "Suburb-Midsize":
                            hs.UrbanCentric = UrbanCentric.SuburbMidsize;
                            break;
                        case "Suburb-Small":
                            hs.UrbanCentric = UrbanCentric.SuburbSmall;
                            break;
                        case "Town-Distant":
                            hs.UrbanCentric = UrbanCentric.TownDistant;
                            break;
                        case "Town-Fringe":
                            hs.UrbanCentric = UrbanCentric.TownFringe;
                            break;
                        case "Town-Remote":
                            hs.UrbanCentric = UrbanCentric.TownRemote;
                            break;
                    }
                }

                attr = node.Attributes["teachers"];
                if (attr != null)
                {
                    double dblOut;
                    if (double.TryParse(attr.Value, out dblOut))
                        hs.TotalTeachers = dblOut;
                }
                attr = node.Attributes["total-students"];
                if (attr != null)
                {
                    int intOut;
                    if (int.TryParse(attr.Value, out intOut))
                        hs.TotalStudents = intOut;
                }

                hs.RacePercents = new AmericanRacePercents();
                attr = node.Attributes["american-indian"];
                if (attr != null)
                {
                    double dblOut;
                    if (double.TryParse(attr.Value, out dblOut))
                        hs.RacePercents.AmericanIndian = dblOut;
                }
                attr = node.Attributes["asian"];
                if (attr != null)
                {
                    double dblOut;
                    if (double.TryParse(attr.Value, out dblOut))
                        hs.RacePercents.Asian = dblOut;
                }
                attr = node.Attributes["hispanic"];
                if (attr != null)
                {
                    double dblOut;
                    if (double.TryParse(attr.Value, out dblOut))
                        hs.RacePercents.Hispanic = dblOut;
                }
                attr = node.Attributes["black"];
                if (attr != null)
                {
                    double dblOut;
                    if (double.TryParse(attr.Value, out dblOut))
                        hs.RacePercents.Black = dblOut;
                }
                attr = node.Attributes["white"];
                if (attr != null)
                {
                    double dblOut;
                    if (double.TryParse(attr.Value, out dblOut))
                        hs.RacePercents.White = dblOut;
                }
                attr = node.Attributes["pacific"];
                if (attr != null)
                {
                    double dblOut;
                    if (double.TryParse(attr.Value, out dblOut))
                        hs.RacePercents.Pacific = dblOut;
                }
                attr = node.Attributes["mixed-race"];
                if (attr != null)
                {
                    double dblOut;
                    if (double.TryParse(attr.Value, out dblOut))
                        hs.RacePercents.Mixed = dblOut;
                }

                if (node.ParentNode == null || node.ParentNode.LocalName != "zip-stat" ||
                    node.ParentNode.Attributes == null || node.ParentNode.Attributes["value"] == null) return true;
                var zipStatNode = node.ParentNode;
                attr = zipStatNode.Attributes["value"];
                hs.PostalCode = attr.Value;

                return true;
            }
            catch
            {
                hs = null;
                return false;
            }
        }
    }


}
