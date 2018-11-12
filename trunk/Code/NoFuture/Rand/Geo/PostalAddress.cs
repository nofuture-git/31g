using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Geo.US;

namespace NoFuture.Rand.Geo
{
    /// <summary>
    /// A composition type to contain a postal address.
    /// </summary>
    [Serializable]
    public class PostalAddress : DiachronIdentifier, IObviate
    {
        public StreetPo Street { get; set; }
        public CityArea CityArea { get; set; }

        public override string Abbrev => "Addr";

        public override string Value
        {
            get => ToString();
            set => throw new NotImplementedException(
                "Full address parser not supported, caller " +
                $"needs to assign the {nameof(Street)} and {nameof(CityArea)}");
        }

        public override string ToString()
        {
            return string.Join("  ", Street, CityArea);
        }

        public IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            var itemData = Street?.ToData(txtCase) ?? new Dictionary<string, object>();
            var moreData = CityArea?.ToData(txtCase) ?? new Dictionary<string, object>();
            if(moreData.Any())
                foreach(var k in moreData.Keys)
                    itemData.Add(k,moreData[k]);
            return itemData;
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
            return new PostalAddress { Street = homeAddr, CityArea = csz };
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

            address = new PostalAddress {Street = po, CityArea = csz};
            return true;
        }
    }
}
