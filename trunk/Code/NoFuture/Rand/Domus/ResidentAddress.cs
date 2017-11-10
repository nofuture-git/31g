using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Endo;

namespace NoFuture.Rand.Domus
{
    [Serializable]
    public class ResidentAddress : DiachronIdentifier
    {
        public StreetPo HomeStreetPo { get; set; }
        public CityArea HomeCityArea { get; set; }
        public bool IsLeased { get; set; }

        public override string Abbrev => "Addr";

        public override string ToString()
        {
            return string.Join(" ", HomeStreetPo, HomeCityArea);
        }

        /// <summary>
        /// Helper factory method to create an american address in one call.
        /// </summary>
        /// <returns></returns>
        public static ResidentAddress GetRandomAmericanAddr(string zipCodePrefix = null)
        {
            var csz = CityArea.American(zipCodePrefix);
            var homeAddr = StreetPo.American();
            return new ResidentAddress { HomeStreetPo = homeAddr, HomeCityArea = csz };
        }
    }
}
