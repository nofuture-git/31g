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
        private string _locality;

        /// <summary>
        /// Typical American address 
        /// as &apos;1600 Pennsylvania Ave NW, Washington, DC 20500&apos;
        /// this is the &apos;1600&apos;
        /// </summary>
        public string ThoroughfareNumber { get; set; }

        /// <summary>
        /// Typically the &apos;NW&apos;, &apos;S&apos; part of an address 
        /// immediately after the AddressNumber
        /// </summary>
        public string ThoroughfareDirectional { get; set; }
        public string ThoroughfareName { get; set; }

        /// <summary>
        /// The &apos;Drive&apos;, &apos;AVE.&apos; part of an address
        /// </summary>
        public string ThoroughfareType { get; set; }
        public string SecondaryUnitDesignator { get; set; }
        public string SecondaryUnitId { get; set; }
        public string MajorMetro { get; set; }

        public string Locality
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_locality) && _locality.Contains(","))
                    _locality = _locality.Replace(",", "");
                return _locality;
            }
            set => _locality = value;
        }
        public string RegionAbbrev { get; set; }
        public string RegionName { get; set; }
        public string PostalCode { get; set; }
        public string SortingCode { get; set; }
        public string NationState { get; set; }

        public decimal Lng { get; set; }
        public decimal Lat { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is AddressData ad))
                return false;

            var p = new[]
            {
                string.Equals(ad.NationState, NationState, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.ThoroughfareNumber, ThoroughfareNumber, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.ThoroughfareDirectional, ThoroughfareDirectional, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.ThoroughfareName, ThoroughfareName, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.ThoroughfareType, ThoroughfareType, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.SecondaryUnitDesignator, SecondaryUnitDesignator, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.SecondaryUnitId, SecondaryUnitId, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.MajorMetro, MajorMetro, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.Locality, Locality, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.RegionAbbrev, RegionAbbrev, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.RegionName, RegionName, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.PostalCode, PostalCode, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.SortingCode, SortingCode, StringComparison.OrdinalIgnoreCase),
                ad.Lat == Lat, ad.Lng == Lng
            };

            return p.All(x => x);
        }

        public override int GetHashCode()
        {

            return (NationState?.ToLower().GetHashCode() ?? 0) + 
                   (ThoroughfareNumber?.ToLower().GetHashCode() ?? 0) +
                   (ThoroughfareDirectional?.ToLower().GetHashCode() ?? 0) +
                   (ThoroughfareName?.ToLower().GetHashCode() ?? 0) +
                   (ThoroughfareType?.ToLower().GetHashCode() ?? 0) +
                   (SecondaryUnitDesignator?.ToLower().GetHashCode() ?? 0) +
                   (SecondaryUnitId?.ToLower().GetHashCode() ?? 0) +
                   (MajorMetro?.ToLower().GetHashCode() ?? 0) +
                   (Locality?.ToLower().GetHashCode() ?? 0) +
                   (RegionAbbrev?.ToLower().GetHashCode() ?? 0) +
                   (RegionName?.ToLower().GetHashCode() ?? 0) +
                   (PostalCode?.ToLower().GetHashCode() ?? 0) +
                   (SortingCode?.ToLower().GetHashCode() ?? 0) +
                   Lng.GetHashCode() +
                   Lat.GetHashCode();
        }

    }
}
