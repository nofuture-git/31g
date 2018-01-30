using System;
using System.Collections.Generic;
namespace NoFuture.Rand.Data.Exo.UsGov.Bls.Codes
{
    public class WpGroup 
    {
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        private static List<WpGroup> _values;
        public static List<WpGroup> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<WpGroup>
                           {
                           
                           new WpGroup
                           {
                               GroupName = "All commodities",
                               GroupCode = "00",
                           },
                           new WpGroup
                           {
                               GroupName = "Farm products",
                               GroupCode = "01",
                           },
                           new WpGroup
                           {
                               GroupName = "Processed foods and feeds",
                               GroupCode = "02",
                           },
                           new WpGroup
                           {
                               GroupName = "Textile products and apparel",
                               GroupCode = "03",
                           },
                           new WpGroup
                           {
                               GroupName = "Hides, skins, leather, and related products",
                               GroupCode = "04",
                           },
                           new WpGroup
                           {
                               GroupName = "Fuels and related products and power",
                               GroupCode = "05",
                           },
                           new WpGroup
                           {
                               GroupName = "Chemicals and allied products",
                               GroupCode = "06",
                           },
                           new WpGroup
                           {
                               GroupName = "Rubber and plastic products",
                               GroupCode = "07",
                           },
                           new WpGroup
                           {
                               GroupName = "Lumber and wood products",
                               GroupCode = "08",
                           },
                           new WpGroup
                           {
                               GroupName = "Pulp, paper, and allied products",
                               GroupCode = "09",
                           },
                           new WpGroup
                           {
                               GroupName = "Metals and metal products",
                               GroupCode = "10",
                           },
                           new WpGroup
                           {
                               GroupName = "Machinery and equipment",
                               GroupCode = "11",
                           },
                           new WpGroup
                           {
                               GroupName = "Furniture and household durables",
                               GroupCode = "12",
                           },
                           new WpGroup
                           {
                               GroupName = "Nonmetallic mineral products",
                               GroupCode = "13",
                           },
                           new WpGroup
                           {
                               GroupName = "Transportation equipment",
                               GroupCode = "14",
                           },
                           new WpGroup
                           {
                               GroupName = "Miscellaneous products",
                               GroupCode = "15",
                           },
                           new WpGroup
                           {
                               GroupName = "Transportation services",
                               GroupCode = "30",
                           },
                           new WpGroup
                           {
                               GroupName = "Services related to transportation activities",
                               GroupCode = "31",
                           },
                           new WpGroup
                           {
                               GroupName = "Warehousing, storage, and related services",
                               GroupCode = "32",
                           },
                           new WpGroup
                           {
                               GroupName = "Publishing sales, excluding software",
                               GroupCode = "33",
                           },
                           new WpGroup
                           {
                               GroupName = "Software publishing",
                               GroupCode = "34",
                           },
                           new WpGroup
                           {
                               GroupName = "Network compensation from broadcast and cable television and radio",
                               GroupCode = "35",
                           },
                           new WpGroup
                           {
                               GroupName = "Advertising space and time sales",
                               GroupCode = "36",
                           },
                           new WpGroup
                           {
                               GroupName = "Telecommunication, cable, and internet user services",
                               GroupCode = "37",
                           },
                           new WpGroup
                           {
                               GroupName = "Data processing and related services",
                               GroupCode = "38",
                           },
                           new WpGroup
                           {
                               GroupName = "Credit intermediation services (partial)",
                               GroupCode = "39",
                           },
                           new WpGroup
                           {
                               GroupName = "Investment services",
                               GroupCode = "40",
                           },
                           new WpGroup
                           {
                               GroupName = "Insurance and annuities",
                               GroupCode = "41",
                           },
                           new WpGroup
                           {
                               GroupName = "Commissions and fees from sales and administration of insurance policies (partial)",
                               GroupCode = "42",
                           },
                           new WpGroup
                           {
                               GroupName = "Real estate services (partial)",
                               GroupCode = "43",
                           },
                           new WpGroup
                           {
                               GroupName = "Rental and leasing of goods (partial)",
                               GroupCode = "44",
                           },
                           new WpGroup
                           {
                               GroupName = "Professional services (partial)",
                               GroupCode = "45",
                           },
                           new WpGroup
                           {
                               GroupName = "Employment services",
                               GroupCode = "46",
                           },
                           new WpGroup
                           {
                               GroupName = "Travel arrangement services  (partial)",
                               GroupCode = "47",
                           },
                           new WpGroup
                           {
                               GroupName = "Selected security services (partial)",
                               GroupCode = "48",
                           },
                           new WpGroup
                           {
                               GroupName = "Cleaning and building maintenance services (partial)",
                               GroupCode = "49",
                           },
                           new WpGroup
                           {
                               GroupName = "Waste collection and remediation services (partial)",
                               GroupCode = "50",
                           },
                           new WpGroup
                           {
                               GroupName = "Health care services",
                               GroupCode = "51",
                           },
                           new WpGroup
                           {
                               GroupName = "Educational services (partial)",
                               GroupCode = "52",
                           },
                           new WpGroup
                           {
                               GroupName = "Accommodation services",
                               GroupCode = "53",
                           },
                           new WpGroup
                           {
                               GroupName = "Food and beverage for immediate consumption services (partial)",
                               GroupCode = "54",
                           },
                           new WpGroup
                           {
                               GroupName = "Repair and maintenance services (partial)",
                               GroupCode = "55",
                           },
                           new WpGroup
                           {
                               GroupName = "Entertainment services (partial)",
                               GroupCode = "56",
                           },
                           new WpGroup
                           {
                               GroupName = "Wholesale trade services",
                               GroupCode = "57",
                           },
                           new WpGroup
                           {
                               GroupName = "Retail trade services",
                               GroupCode = "58",
                           },
                           new WpGroup
                           {
                               GroupName = "Metal treatment services",
                               GroupCode = "59",
                           },
                           new WpGroup
                           {
                               GroupName = "Mining services",
                               GroupCode = "60",
                           },
                           new WpGroup
                           {
                               GroupName = "Contract work on textile products, apparel, and leather",
                               GroupCode = "61",
                           },
                           new WpGroup
                           {
                               GroupName = "Construction (partial)",
                               GroupCode = "80",
                           },
                           new WpGroup
                           {
                               GroupName = "Durability of product",
                               GroupCode = "DUR",
                           },
                           new WpGroup
                           {
                               GroupName = "Final demand",
                               GroupCode = "FD",
                           },
                           new WpGroup
                           {
                               GroupName = "Intermediate demand by production flow",
                               GroupCode = "ID5",
                           },
                           new WpGroup
                           {
                               GroupName = "Intermediate demand by commodity type",
                               GroupCode = "ID6",
                           },
                           new WpGroup
                           {
                               GroupName = "Industrial Commodities less fuels",
                               GroupCode = "ILF",
                           },
                           new WpGroup
                           {
                               GroupName = "Industrial Commodities",
                               GroupCode = "IND",
                           },
                           new WpGroup
                           {
                               GroupName = "Inputs to industries",
                               GroupCode = "IP",
                           },
                           new WpGroup
                           {
                               GroupName = "Farm products, processed foods and feeds",
                               GroupCode = "PFF",
                           },
                           new WpGroup
                           {
                               GroupName = "Special indexes",
                               GroupCode = "SI",
                           },
                           new WpGroup
                           {
                               GroupName = "Stage of processing",
                               GroupCode = "SOP",
                           },

                       };
                return _values;
            }
        }
	}//end WpGroup
}//end NoFuture.Rand.Gov.Bls.Codes