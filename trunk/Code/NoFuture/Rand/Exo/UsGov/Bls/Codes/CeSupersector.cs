using System;
using System.Collections.Generic;
namespace NoFuture.Rand.Exo.UsGov.Bls.Codes
{
    public class CeSupersector 
    {
        public string SupersectorCode { get; set; }
        public string SupersectorName { get; set; }
        private static List<CeSupersector> _values;
        public static List<CeSupersector> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<CeSupersector>
                           {
                           
                           new CeSupersector
                           {
                               SupersectorCode = "00",
                               SupersectorName = "Total nonfarm",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "05",
                               SupersectorName = "Total private",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "06",
                               SupersectorName = "Goods-producing",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "07",
                               SupersectorName = "Service-providing",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "08",
                               SupersectorName = "Private service-providing",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "10",
                               SupersectorName = "Mining and logging",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "20",
                               SupersectorName = "Construction",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "30",
                               SupersectorName = "Manufacturing",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "31",
                               SupersectorName = "Durable Goods",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "32",
                               SupersectorName = "Nondurable Goods",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "40",
                               SupersectorName = "Trade, transportation, and utilities",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "41",
                               SupersectorName = "Wholesale trade",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "42",
                               SupersectorName = "Retail trade",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "43",
                               SupersectorName = "Transportation and warehousing",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "44",
                               SupersectorName = "Utilities",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "50",
                               SupersectorName = "Information",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "55",
                               SupersectorName = "Financial activities",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "60",
                               SupersectorName = "Professional and business services",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "65",
                               SupersectorName = "Education and health services",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "70",
                               SupersectorName = "Leisure and hospitality",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "80",
                               SupersectorName = "Other services",
                           },
                           new CeSupersector
                           {
                               SupersectorCode = "90",
                               SupersectorName = "Government",
                           },

                       };
                return _values;
            }
        }
	}//end CeSupersector
}//end NoFuture.Rand.Gov.Bls.Codes