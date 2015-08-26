using System;

namespace NoFuture.Rand.Data.Types
{
    [Serializable]
    public class AddressData
    {
        public string AddressNumber { get; set; }
        public string StreetNamePreDirectional { get; set; }
        public string StreetName { get; set; }
        public string StreetType { get; set; }
        public string SecondaryUnitDesignator { get; set; }
        public string SecondaryUnitId { get; set; }
        public string MajorMetro { get; set; }
        public string City { get; set; }
        public string StateAbbrv { get; set; }
        public string StateName { get; set; }
        public string PostalCode { get; set; }
        public string PostalCodeSuffix { get; set; }
        public decimal Lng { get; set; }
        public decimal Lat { get; set; }
    }
}
