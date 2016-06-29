using System;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Rand.Domus
{
    public class HomeAddress
    {
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public StreetPo HomeStreetPo { get; set; }
        public CityArea HomeCityArea { get; set; }

        public override string ToString()
        {
            return string.Join(" ", HomeStreetPo, HomeCityArea);
        }
    }
}
