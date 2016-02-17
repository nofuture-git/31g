using System;
using System.Linq;

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

        public override bool Equals(object obj)
        {
            var ad = obj as AddressData;
            if (ad == null)
                return false;

            var p = new[]
            {
                string.Equals(ad.AddressNumber, AddressNumber, StringComparison.OrdinalIgnoreCase),
                string.Equals(ad.StreetNamePreDirectional, StreetNamePreDirectional, StringComparison.OrdinalIgnoreCase),
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

            return (AddressNumber == null ? 0 : AddressNumber.ToLower().GetHashCode()) +
                   (StreetNamePreDirectional == null ? 0 : StreetNamePreDirectional.ToLower().GetHashCode()) +
                   (StreetName == null ? 0 : StreetName.ToLower().GetHashCode()) +
                   (StreetType == null ? 0 : StreetType.ToLower().GetHashCode()) +
                   (SecondaryUnitDesignator == null ? 0 : SecondaryUnitDesignator.ToLower().GetHashCode()) +
                   (SecondaryUnitId == null ? 0 : SecondaryUnitId.ToLower().GetHashCode()) +
                   (MajorMetro == null ? 0 : MajorMetro.ToLower().GetHashCode()) +
                   (City == null ? 0 : City.ToLower().GetHashCode()) +
                   (StateAbbrv == null ? 0 : StateAbbrv.ToLower().GetHashCode()) +
                   (StateName == null ? 0 : StateName.ToLower().GetHashCode()) +
                   (PostalCode == null ? 0 : PostalCode.ToLower().GetHashCode()) +
                   (PostalCodeSuffix == null ? 0 : PostalCodeSuffix.ToLower().GetHashCode()) +
                   Lng.GetHashCode() +
                   Lat.GetHashCode();
        }
    }
}
