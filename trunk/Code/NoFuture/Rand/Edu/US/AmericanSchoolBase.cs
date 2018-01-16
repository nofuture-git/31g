using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US;

namespace NoFuture.Rand.Edu.US
{
    [Serializable]
    public abstract class AmericanSchoolBase
    {
        public UsState State { get; set; }
        public string StateName { get; set; }
        public string Name { get; set; }

        internal const string US_HIGH_SCHOOL_DATA = "US_HighSchools_Data.xml";
        internal const string US_UNIVERSITY_DATA = "US_Universities_Data.xml";
        internal static XmlDocument HsXml;
        internal static XmlDocument UnivXml;

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static AmericanRacePercents GetNatlGradRates(XmlDocument src, double avg)
        {
            var p = new AmericanRacePercents
            {
                Asian = avg,
                AmericanIndian = avg,
                Hispanic = avg,
                Black = avg,
                Mixed = avg,
                National = avg,
                Pacific = avg,
                White = avg
            };

            if (src == null)
                return p;

            double natlAvg, amerIndian, asian, hispanic, black, white, pacific, mixed;
            //read data from source
            var node = src.SelectSingleNode("//national/avg-graduation-rate");
            var elem = node as XmlElement;
            var attr = elem?.Attributes["natl-percent"]?.Value;
            if (attr == null || !double.TryParse(attr, out natlAvg))
                return p;
            attr = elem.Attributes["american-indian"]?.Value;
            if (attr == null || !double.TryParse(attr, out amerIndian))
                return p;
            attr = elem.Attributes["asian"]?.Value;
            if (attr == null || !double.TryParse(attr, out asian))
                return p;
            attr = elem.Attributes["hispanic"]?.Value;
            if (attr == null || !double.TryParse(attr, out hispanic))
                return p;
            attr = elem.Attributes["black"]?.Value;
            if (attr == null || !double.TryParse(attr, out black))
                return p;
            attr = elem.Attributes["white"]?.Value;
            if (attr == null || !double.TryParse(attr, out white))
                return p;
            attr = elem.Attributes["pacific"]?.Value;
            if (attr == null || !double.TryParse(attr, out pacific))
                return p;
            attr = elem.Attributes["mixed-race"]?.Value;
            if (attr == null || !double.TryParse(attr, out mixed))
                return p;

            p.National = natlAvg;
            p.AmericanIndian = amerIndian;
            p.Asian = asian;
            p.Hispanic = hispanic;
            p.Black = black;
            p.White = white;
            p.Pacific = pacific;
            p.Mixed = mixed;

            return p;
        }


        /// <summary>
        /// Difference of national avg to race average added to state average.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="race"></param>
        /// <param name="edu"></param>
        /// <returns></returns>
        public static double SolvePercentGradByStateAndRace(string state, NorthAmericanRace? race,
            OccidentalEdu edu = OccidentalEdu.HighSchool | OccidentalEdu.Grad)
        {
            AmericanRacePercents p;
            p = edu >= OccidentalEdu.Bachelor ? AmericanUniversity.NatlGradRate() : AmericanHighSchool.NatlGradRate();
            var stateAvg = p.National;
            var natlAvg = p.National;
            var stateData = UsStateData.GetStateData(state);
            if (stateData?.PercentOfGrads != null && stateData.PercentOfGrads.Count > 0)
            {
                var f = stateData.PercentOfGrads.FirstOrDefault(x => x.Item1 == edu);
                if (f != null)
                {
                    stateAvg = Math.Round(f.Item2, 1);
                }
            }

            var raceNatlAvg = new Dictionary<NorthAmericanRace, double>
            {
                {NorthAmericanRace.AmericanIndian, p.AmericanIndian - natlAvg},
                {NorthAmericanRace.Asian, p.Asian - natlAvg},
                {NorthAmericanRace.Hispanic, p.Hispanic - natlAvg},
                {NorthAmericanRace.Black, p.Black - natlAvg},
                {NorthAmericanRace.White, p.White - natlAvg},
                {NorthAmericanRace.Pacific, p.Pacific - natlAvg},
                {NorthAmericanRace.Mixed, p.Mixed - natlAvg}
            };
            if (race == null || !raceNatlAvg.ContainsKey(race.Value))
                return Math.Round(stateAvg, 1);

            return Math.Round(stateAvg + raceNatlAvg[race.Value], 1);
        }

        
        /// <summary>
        /// Helper method to get all US State abbreviations which are present in the data file.
        /// </summary>
        /// <returns></returns>
        internal static string[] GetAllXmlStateAbbreviations(XmlDocument xmlDoc)
        {
            const string ABBREV = "abbreviation";
            if (xmlDoc == null)
                return new string[] { };
            var allStateAbbrevs = new List<string>();
            var stateNodes = xmlDoc.SelectNodes("//state");
            if (stateNodes == null)
                return new string[] { };

            for (var i = 0; i <= stateNodes.Count; i++)
            {
                var snode = stateNodes.Item(i);
                if (!(snode is XmlElement stateElem))
                    continue;

                if (!stateElem.HasAttributes)
                    continue;
                var abbrevAttr = stateElem.Attributes[ABBREV];
                if (String.IsNullOrWhiteSpace(abbrevAttr?.Value))
                    continue;
                if (allStateAbbrevs.Contains(abbrevAttr.Value))
                    continue;
                allStateAbbrevs.Add(abbrevAttr.Value);
            }
            return allStateAbbrevs.ToArray();
        }

        internal static string GetRandomStateAbbrev(string[] stateAbbrev)
        {
            if (stateAbbrev == null || !stateAbbrev.Any())
                return "NY";
            var pickone = Etx.IntNumber(0, stateAbbrev.Length - 1);
            return stateAbbrev[pickone];
        }
    }
}
