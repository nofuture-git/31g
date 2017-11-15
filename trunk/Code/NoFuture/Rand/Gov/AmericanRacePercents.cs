using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Domus;

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
            var tbl = NAmerUtil.Tables.NorthAmericanRaceAvgs;
            return tbl.ToDictionary(k => k.Key.ToString(), k => tbl[k.Key]);
        }
    }
}