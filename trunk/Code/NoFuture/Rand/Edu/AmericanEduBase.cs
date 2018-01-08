using System;
using System.ComponentModel;
using System.Xml;
using NoFuture.Rand.Data;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Edu
{
    [Serializable]
    public abstract class AmericanEduBase
    {
        public UsState State { get; set; }
        public string StateName { get; set; }
        public string Name { get; set; }

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
            if (TreeData.AmericanHighSchoolData == null)
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
    }
}
