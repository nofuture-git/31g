using System;
using System.Linq;

namespace NoFuture.Rand.Geo
{
    /// <summary>
    /// An underlying data store which allows for a postal address 
    /// to be expressed in various forms.
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
        /// immediately after the ThoroughfareNumber
        /// </summary>
        public string ThoroughfareDirectional { get; set; }

        /// <summary>
        /// Typical American address 
        /// as &apos;1600 Pennsylvania Ave NW, Washington, DC 20500&apos;
        /// this is the &apos;Pennsylvania&apos; (a.k.a. the street name).
        /// </summary>
        public string ThoroughfareName { get; set; }

        /// <summary>
        /// The &apos;Drive&apos;, &apos;AVE.&apos; part of an address
        /// </summary>
        public string ThoroughfareType { get; set; }
        public string SecondaryUnitDesignator { get; set; }
        public string SecondaryUnitId { get; set; }

        /// <summary>
        /// Typical American address 
        /// as &apos;1600 Pennsylvania Ave NW, Washington, DC 20500&apos;
        /// this is the &apos;Washington&apos; (a.k.a. the City name).
        /// </summary>
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
        /// <summary>
        /// Typical American address 
        /// as &apos;1600 Pennsylvania Ave NW, Washington, DC 20500&apos;
        /// this is the &apos;DC&apos; (a.k.a. the State name).
        /// </summary>
        /// <remarks>
        /// xref ISO 3166-2
        /// Also-known-as:
        /// districts, regions, &apos;Lander&apos;, 
        /// municipalities, provinces, states, departments, 
        /// prefectures, commune, cantons, territories, 
        /// parishes, dependencies, sectors, governorates, 
        /// quarters, zones, outlying areas, rayons, and more.
        /// </remarks>
        public string RegionAbbrev { get; set; }

        /// <summary>
        /// The full name of <see cref="RegionAbbrev"/>
        /// </summary>
        public string RegionName { get; set; }
        public string PostalCode { get; set; }
        public string SortingCode { get; set; }

        /// <summary>
        /// The name of the country (e.g. US, Mexico, GB, Japan, etc.)
        /// </summary>
        public string NationState { get; set; }

        public decimal Lng { get; set; }
        public decimal Lat { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is AddressData ad))
                return false;
            const StringComparison OPT = StringComparison.OrdinalIgnoreCase;
            var p = new[]
            {
                string.Equals(ad.NationState, NationState, OPT),
                string.Equals(ad.ThoroughfareNumber, ThoroughfareNumber, OPT),
                string.Equals(ad.ThoroughfareDirectional, ThoroughfareDirectional, OPT),
                string.Equals(ad.ThoroughfareName, ThoroughfareName, OPT),
                string.Equals(ad.ThoroughfareType, ThoroughfareType, OPT),
                string.Equals(ad.SecondaryUnitDesignator, SecondaryUnitDesignator, OPT),
                string.Equals(ad.SecondaryUnitId, SecondaryUnitId, OPT),
                string.Equals(ad.Locality, Locality, OPT),
                string.Equals(ad.RegionAbbrev, RegionAbbrev, OPT),
                string.Equals(ad.RegionName, RegionName, OPT),
                string.Equals(ad.PostalCode, PostalCode, OPT),
                string.Equals(ad.SortingCode, SortingCode, OPT),
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
