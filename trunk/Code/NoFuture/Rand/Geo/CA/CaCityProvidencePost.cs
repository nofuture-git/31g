using System;

namespace NoFuture.Rand.Geo.CA
{
    /// <summary>
    /// Postal address for Canada
    /// </summary>
    [Serializable]
    public class CaCityProvidencePost : CityArea
    {
        public CaCityProvidencePost(AddressData d) : base(d)
        {
            GetData().NationState = "CA";
        }
        public string ProvidenceAbbrv => GetData().RegionAbbrev;
        public string Providence => GetData().RegionName;
        public string PostalCode => GetData().PostalCode;

        public override string ToString()
        {
            return $"{City} {ProvidenceAbbrv}, {PostalCode}";
        }
    }
}