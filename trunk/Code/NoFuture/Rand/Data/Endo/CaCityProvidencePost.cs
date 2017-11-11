﻿using System;

namespace NoFuture.Rand.Data.Endo
{
    [Serializable]
    public class CaCityProvidencePost : CityArea
    {
        public CaCityProvidencePost(AddressData d) : base(d) { }
        public string ProvidenceAbbrv => data.StateAbbrv;
        public string Providence => data.StateName;
        public string City => data.City;
        public string PostalCode => data.PostalCode;

        public override string ToString()
        {
            return $"{City} {ProvidenceAbbrv}, {PostalCode}";
        }
    }
}