using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Geo
{
    /// <summary>
    /// A composition type to contain a home address.
    /// </summary>
    [Serializable]
    public class PostalAddress : DiachronIdentifier
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
        [RandomFactory]
        public static PostalAddress RandomAmericanAddress(string zipCodePrefix = null)
        {
            var csz = CityArea.RandomAmericanCity(zipCodePrefix);
            var homeAddr = StreetPo.RandomAmericanStreet();
            return new PostalAddress { HomeStreetPo = homeAddr, HomeCityArea = csz };
        }
    }
}
