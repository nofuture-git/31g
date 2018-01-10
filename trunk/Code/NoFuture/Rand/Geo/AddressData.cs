using System;
using System.Linq;

namespace NoFuture.Rand.Geo
{
    /// <summary>
    /// An underlying data store which allows for a postal address 
    /// to be expressed in various forms
    /// </summary>
    [Serializable]
    public class AddressData
    {
        private string _city;

        /// <summary>
        /// Typical American address 
        /// as &apos;1600 Pennsylvania Ave NW, Washington, DC 20500&apos;
        /// this is the &apos;1600&apos;
        /// </summary>
        public string AddressNumber { get; set; }
        public string StreetNameDirectional { get; set; }
        public string StreetName { get; set; }
        public string StreetType { get; set; }
        public string SecondaryUnitDesignator { get; set; }
        public string SecondaryUnitId { get; set; }
        public string MajorMetro { get; set; }

        public string City
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_city) && _city.Contains(","))
                    _city = _city.Replace(",", "");
                return _city;
            }
            set => _city = value;
        }
        public string StateAbbrv { get; set; }
        public string StateName { get; set; }
        public string PostalCode { get; set; }
        public string PostalCodeSuffix { get; set; }
        public decimal Lng { get; set; }
        public decimal Lat { get; set; }

        public override bool Equals(object obj)
        {
            var ad = obj as AddressData;
            if (ad == null)
                return false;

            var p = new[]
            {
                string.Equals(ad.AddressNumber, AddressNumber, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.StreetNameDirectional, StreetNameDirectional, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.StreetName, StreetName, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.StreetType, StreetType, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.SecondaryUnitDesignator, SecondaryUnitDesignator, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.SecondaryUnitId, SecondaryUnitId, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.MajorMetro, MajorMetro, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.City, City, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.StateAbbrv, StateAbbrv, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.StateName, StateName, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.PostalCode, PostalCode, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.PostalCodeSuffix, PostalCodeSuffix, StringComparison.OrdinalIgnoreCase),
                ad.Lat == Lat, ad.Lng == Lng
            };

            return p.All(x => x);
        }

        public override int GetHashCode()
        {

            return (AddressNumber?.ToLower().GetHashCode() ?? 0) +
                   (StreetNameDirectional?.ToLower().GetHashCode() ?? 0) +
                   (StreetName?.ToLower().GetHashCode() ?? 0) +
                   (StreetType?.ToLower().GetHashCode() ?? 0) +
                   (SecondaryUnitDesignator?.ToLower().GetHashCode() ?? 0) +
                   (SecondaryUnitId?.ToLower().GetHashCode() ?? 0) +
                   (MajorMetro?.ToLower().GetHashCode() ?? 0) +
                   (City?.ToLower().GetHashCode() ?? 0) +
                   (StateAbbrv?.ToLower().GetHashCode() ?? 0) +
                   (StateName?.ToLower().GetHashCode() ?? 0) +
                   (PostalCode?.ToLower().GetHashCode() ?? 0) +
                   (PostalCodeSuffix?.ToLower().GetHashCode() ?? 0) +
                   Lng.GetHashCode() +
                   Lat.GetHashCode();
        }
    }
}
