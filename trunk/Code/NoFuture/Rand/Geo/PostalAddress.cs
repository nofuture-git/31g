using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Geo.US;

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

        /// <summary>
        /// Helper method to quickly go from string literals to a <see cref="PostalAddress"/>
        /// </summary>
        /// <param name="postOfficeLine1"></param>
        /// <param name="cityStateZipLine2"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool TryParseAmericanAddress(string postOfficeLine1, string cityStateZipLine2,
            out PostalAddress address)
        {
            address = null;
            if (string.IsNullOrWhiteSpace(postOfficeLine1) || string.IsNullOrWhiteSpace(cityStateZipLine2))
                return false;

            if (!UsStreetPo.TryParse(postOfficeLine1, out var po))
                return false;

            if (!UsCityStateZip.TryParse(cityStateZipLine2, out var csz))
                return false;

            address = new PostalAddress {HomeStreetPo = po, HomeCityArea = csz};
            return true;
        }
    }
}
