using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;

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

        public override IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x, txtCase);
            var itemData = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(City))
                itemData.Add(textFormat(nameof(City)), City);
            if (!string.IsNullOrWhiteSpace(ProvidenceAbbrv))
                itemData.Add(textFormat(nameof(Providence)), ProvidenceAbbrv);
            if (!string.IsNullOrWhiteSpace(PostalCode))
                itemData.Add(textFormat(nameof(PostalCode)), PostalCode);
            return itemData;
        }
    }
}