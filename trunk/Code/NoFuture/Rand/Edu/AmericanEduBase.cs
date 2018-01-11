using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.ComponentModel;
using System.Reflection;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Edu
{
    [Serializable]
    public abstract class AmericanEduBase
    {
        public UsState State { get; set; }
        public string StateName { get; set; }
        public string Name { get; set; }

        internal const string US_HIGH_SCHOOL_DATA = "US_HighSchools_Data.xml";
        internal const string US_UNIVERSITY_DATA = "US_Universities_Data.xml";

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
            var xml = Core.XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_HIGH_SCHOOL_DATA, Assembly.GetExecutingAssembly());
            if (xml == null)//TreeData.AmericanHighSchoolData
                return p;
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
        public static double SolvePercentGradByStateAndRace(UsState state, NorthAmericanRace? race,
            OccidentalEdu edu = OccidentalEdu.HighSchool | OccidentalEdu.Grad)
        {
            AmericanRacePercents p;
            p = edu >= OccidentalEdu.Bachelor ? AmericanUniversity.NatlGradRate() : AmericanHighSchool.NatlGradRate();
            var stateAvg = p.National;
            var natlAvg = p.National;
            var stateData = UsStateData.GetStateData(state?.ToString());
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
    }
}
