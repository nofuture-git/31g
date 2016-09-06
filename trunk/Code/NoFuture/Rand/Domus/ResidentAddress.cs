using System;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Rand.Domus
{
    [Serializable]
    public class ResidentAddress : Identifier
    {
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public StreetPo HomeStreetPo { get; set; }
        public CityArea HomeCityArea { get; set; }
        public bool IsLeased { get; set; }

        public override string Abbrev => "Addr";

        public override string ToString()
        {
            return string.Join(" ", HomeStreetPo, HomeCityArea);
        }
    }
}
