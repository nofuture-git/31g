using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Data.Endo.Enums;

namespace NoFuture.Rand.Gov
{
    [Serializable]
    public class AmericanRacePercents
    {
        #region fields
        private static readonly string _ai =
            Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.AmericanIndian) ?? "AmericanIndian";
        private static readonly string _p = 
            Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.Pacific) ?? "Pacific";
        private static readonly string _m = 
            Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.Mixed) ?? "Mixed";
        private static readonly string _a = 
            Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.Asian) ?? "Asian";
        private static readonly string _h = 
            Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.Hispanic) ?? "Hispanic";
        private static readonly string _b = 
            Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.Black) ?? "Black";
        private static readonly string _w = 
            Enum.GetName(typeof(NorthAmericanRace), NorthAmericanRace.White) ?? "White";
        #endregion

        public double National { get; set; }
        public double AmericanIndian { get; set; }
        public double Asian { get; set; }
        public double Hispanic { get; set; }
        public double Black { get; set; }
        public double White { get; set; }
        public double Pacific { get; set; }
        public double Mixed { get; set; }

        public override string ToString()
        {
            return string.Join(" ", _ai, AmericanIndian, _a, Asian, _b, Black, _h, Hispanic,
                _m, Mixed, _p, Pacific, _w, White);
        }

        public static AmericanRacePercents GetNatlAvg()
        {
            var dict = GetNatlAvgAsDict();
            return new AmericanRacePercents
            {
                AmericanIndian = dict[_ai],
                Pacific = dict[_p],
                Mixed = dict[_m],
                Asian = dict[_a],
                Hispanic = dict[_h],
                Black = dict[_b],
                White = dict[_w]
            };
        }

        public static Dictionary<string, double> GetNatlAvgAsDict()
        {
            var tbl = NorthAmericanRaceAvgs;
            return tbl.ToDictionary(k => k.Key.ToString(), k => tbl[k.Key]);
        }

        public static Dictionary<NorthAmericanRace, double> NorthAmericanRaceAvgs { get; } = new Dictionary<NorthAmericanRace, double>
        {
            {NorthAmericanRace.AmericanIndian, 1.0D },
            {NorthAmericanRace.Asian, 6.0D },
            {NorthAmericanRace.Hispanic, 18.0D },
            {NorthAmericanRace.Black, 12.0D },
            {NorthAmericanRace.White, 61.0D },
            {NorthAmericanRace.Pacific, 1.0D },
            {NorthAmericanRace.Mixed, 2.0D }
        };
    }
}