using System.Collections.Generic;

namespace NoFuture.Rand.Data.Exo.UsGov.Bea.Parameters.GDPbyIndustry
{
    public class Industry : BeaParameter
    {
        public override string Description { get; set; }
        public override string Val { get; set; }
        private static List<Industry> _values;
        public static List<Industry> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<Industry>
                           {
                           
                           new Industry
                           {
                               Val = "GDP",
                               Description = "Gross domestic product (A,Q)",
                           },
                           new Industry
                           {
                               Val = "II",
                               Description = "All industries (A,Q)",
                           },
                           new Industry
                           {
                               Val = "PVT",
                               Description = "Private industries (A,Q)",
                           },
                           new Industry
                           {
                               Val = "11",
                               Description = "Agriculture, forestry, fishing, and hunting (A,Q)",
                           },
                           new Industry
                           {
                               Val = "111CA",
                               Description = "Farms (A)",
                           },
                           new Industry
                           {
                               Val = "113FF",
                               Description = "Forestry, fishing, and related activities (A)",
                           },
                           new Industry
                           {
                               Val = "21",
                               Description = "Mining (A,Q)",
                           },
                           new Industry
                           {
                               Val = "211",
                               Description = "Oil and gas extraction (A)",
                           },
                           new Industry
                           {
                               Val = "212",
                               Description = "Mining, except oil and gas (A)",
                           },
                           new Industry
                           {
                               Val = "213",
                               Description = "Support activities for mining (A)",
                           },
                           new Industry
                           {
                               Val = "22",
                               Description = "Utilities (A,Q)",
                           },
                           new Industry
                           {
                               Val = "23",
                               Description = "Construction (A,Q)",
                           },
                           new Industry
                           {
                               Val = "31G",
                               Description = "Manufacturing (A,Q)",
                           },
                           new Industry
                           {
                               Val = "33DG",
                               Description = "Durable goods (A,Q)",
                           },
                           new Industry
                           {
                               Val = "321",
                               Description = "Wood products (A)",
                           },
                           new Industry
                           {
                               Val = "327",
                               Description = "Nonmetallic mineral products (A)",
                           },
                           new Industry
                           {
                               Val = "331",
                               Description = "Primary metals (A)",
                           },
                           new Industry
                           {
                               Val = "332",
                               Description = "Fabricated metal products (A)",
                           },
                           new Industry
                           {
                               Val = "333",
                               Description = "Machinery (A)",
                           },
                           new Industry
                           {
                               Val = "334",
                               Description = "Computer and electronic products (A)",
                           },
                           new Industry
                           {
                               Val = "335",
                               Description = "Electrical equipment, appliances, and components (A)",
                           },
                           new Industry
                           {
                               Val = "3361MV",
                               Description = "Motor vehicles, bodies and trailers, and parts (A)",
                           },
                           new Industry
                           {
                               Val = "3364OT",
                               Description = "Other transportation equipment (A)",
                           },
                           new Industry
                           {
                               Val = "337",
                               Description = "Furniture and related products (A)",
                           },
                           new Industry
                           {
                               Val = "339",
                               Description = "Miscellaneous manufacturing (A)",
                           },
                           new Industry
                           {
                               Val = "31ND",
                               Description = "Nondurable goods (A,Q)",
                           },
                           new Industry
                           {
                               Val = "311FT",
                               Description = "Food and beverage and tobacco products (A)",
                           },
                           new Industry
                           {
                               Val = "313TT",
                               Description = "Textile mills and textile product mills (A)",
                           },
                           new Industry
                           {
                               Val = "315AL",
                               Description = "Apparel and leather and allied products (A)",
                           },
                           new Industry
                           {
                               Val = "322",
                               Description = "Paper products (A)",
                           },
                           new Industry
                           {
                               Val = "323",
                               Description = "Printing and related support activities (A)",
                           },
                           new Industry
                           {
                               Val = "324",
                               Description = "Petroleum and coal products (A)",
                           },
                           new Industry
                           {
                               Val = "325",
                               Description = "Chemical products (A)",
                           },
                           new Industry
                           {
                               Val = "326",
                               Description = "Plastics and rubber products (A)",
                           },
                           new Industry
                           {
                               Val = "42",
                               Description = "Wholesale trade (A,Q)",
                           },
                           new Industry
                           {
                               Val = "44RT",
                               Description = "Retail trade (A,Q)",
                           },
                           new Industry
                           {
                               Val = "441",
                               Description = "Motor vehicle and parts dealers (A)",
                           },
                           new Industry
                           {
                               Val = "445",
                               Description = "Food and beverage stores (A)",
                           },
                           new Industry
                           {
                               Val = "452",
                               Description = "General merchandise stores (A)",
                           },
                           new Industry
                           {
                               Val = "4A0",
                               Description = "Other retail (A)",
                           },
                           new Industry
                           {
                               Val = "48TW",
                               Description = "Transportation and warehousing (A,Q)",
                           },
                           new Industry
                           {
                               Val = "481",
                               Description = "Air transportation (A)",
                           },
                           new Industry
                           {
                               Val = "482",
                               Description = "Rail transportation (A)",
                           },
                           new Industry
                           {
                               Val = "483",
                               Description = "Water transportation (A)",
                           },
                           new Industry
                           {
                               Val = "484",
                               Description = "Truck transportation (A)",
                           },
                           new Industry
                           {
                               Val = "485",
                               Description = "Transit and ground passenger transportation (A)",
                           },
                           new Industry
                           {
                               Val = "486",
                               Description = "Pipeline transportation (A)",
                           },
                           new Industry
                           {
                               Val = "487OS",
                               Description = "Other transportation and support activities (A)",
                           },
                           new Industry
                           {
                               Val = "493",
                               Description = "Warehousing and storage (A)",
                           },
                           new Industry
                           {
                               Val = "51",
                               Description = "Information (A,Q)",
                           },
                           new Industry
                           {
                               Val = "511",
                               Description = "Publishing industries, except internet (includes software) (A)",
                           },
                           new Industry
                           {
                               Val = "512",
                               Description = "Motion picture and sound recording industries (A)",
                           },
                           new Industry
                           {
                               Val = "513",
                               Description = "Broadcasting and telecommunications (A)",
                           },
                           new Industry
                           {
                               Val = "514",
                               Description = "Data processing, internet publishing, and other information services (A)",
                           },
                           new Industry
                           {
                               Val = "FIRE",
                               Description = "Finance, insurance, real estate, rental, and leasing (A,Q)",
                           },
                           new Industry
                           {
                               Val = "52",
                               Description = "Finance and insurance (A,Q)",
                           },
                           new Industry
                           {
                               Val = "521CI",
                               Description = "Federal Reserve banks, credit intermediation, and related activities (A)",
                           },
                           new Industry
                           {
                               Val = "523",
                               Description = "Securities, commodity contracts, and investments (A)",
                           },
                           new Industry
                           {
                               Val = "524",
                               Description = "Insurance carriers and related activities (A)",
                           },
                           new Industry
                           {
                               Val = "525",
                               Description = "Funds, trusts, and other financial vehicles (A)",
                           },
                           new Industry
                           {
                               Val = "53",
                               Description = "Real estate and rental and leasing (A,Q)",
                           },
                           new Industry
                           {
                               Val = "531",
                               Description = "Real estate (A)",
                           },
                           new Industry
                           {
                               Val = "HS",
                               Description = "Housing (A)",
                           },
                           new Industry
                           {
                               Val = "ORE",
                               Description = "Other real estate (A)",
                           },
                           new Industry
                           {
                               Val = "532RL",
                               Description = "Rental and leasing services and lessors of intangible assets (A)",
                           },
                           new Industry
                           {
                               Val = "PROF",
                               Description = "Professional and business services (A,Q)",
                           },
                           new Industry
                           {
                               Val = "54",
                               Description = "Professional, scientific, and technical services (A,Q)",
                           },
                           new Industry
                           {
                               Val = "5411",
                               Description = "Legal services (A)",
                           },
                           new Industry
                           {
                               Val = "5415",
                               Description = "Computer systems design and related services (A)",
                           },
                           new Industry
                           {
                               Val = "5412OP",
                               Description = "Miscellaneous professional, scientific, and technical services (A)",
                           },
                           new Industry
                           {
                               Val = "55",
                               Description = "Management of companies and enterprises (A,Q)",
                           },
                           new Industry
                           {
                               Val = "56",
                               Description = "Administrative and waste management services (A,Q)",
                           },
                           new Industry
                           {
                               Val = "561",
                               Description = "Administrative and support services (A)",
                           },
                           new Industry
                           {
                               Val = "562",
                               Description = "Waste management and remediation services (A)",
                           },
                           new Industry
                           {
                               Val = "6",
                               Description = "Educational services, health care, and social assistance (A,Q)",
                           },
                           new Industry
                           {
                               Val = "61",
                               Description = "Educational services (A,Q)",
                           },
                           new Industry
                           {
                               Val = "62",
                               Description = "Health care and social assistance (A,Q)",
                           },
                           new Industry
                           {
                               Val = "621",
                               Description = "Ambulatory health care services (A)",
                           },
                           new Industry
                           {
                               Val = "622",
                               Description = "Hospitals (A)",
                           },
                           new Industry
                           {
                               Val = "623",
                               Description = "Nursing and residential care facilities (A)",
                           },
                           new Industry
                           {
                               Val = "624",
                               Description = "Social assistance (A)",
                           },
                           new Industry
                           {
                               Val = "7",
                               Description = "Arts, entertainment, recreation, accommodation, and food services (A,Q)",
                           },
                           new Industry
                           {
                               Val = "71",
                               Description = "Arts, entertainment, and recreation (A,Q)",
                           },
                           new Industry
                           {
                               Val = "711AS",
                               Description = "Performing arts, spectator sports, museums, and related activities (A)",
                           },
                           new Industry
                           {
                               Val = "713",
                               Description = "Amusements, gambling, and recreation industries (A)",
                           },
                           new Industry
                           {
                               Val = "72",
                               Description = "Accommodation and food services (A,Q)",
                           },
                           new Industry
                           {
                               Val = "721",
                               Description = "Accommodation (A)",
                           },
                           new Industry
                           {
                               Val = "722",
                               Description = "Food services and drinking places (A)",
                           },
                           new Industry
                           {
                               Val = "81",
                               Description = "Other services, except government (A,Q)",
                           },
                           new Industry
                           {
                               Val = "G",
                               Description = "Government (A,Q)",
                           },
                           new Industry
                           {
                               Val = "GF",
                               Description = "Federal (A,Q)",
                           },
                           new Industry
                           {
                               Val = "GFG",
                               Description = "General government (A)",
                           },
                           new Industry
                           {
                               Val = "GFGD",
                               Description = "National defense (A)",
                           },
                           new Industry
                           {
                               Val = "GFGN",
                               Description = "Nondefense (A)",
                           },
                           new Industry
                           {
                               Val = "GFE",
                               Description = "Government enterprises (A)",
                           },
                           new Industry
                           {
                               Val = "GSL",
                               Description = "State and local (A,Q)",
                           },
                           new Industry
                           {
                               Val = "GSLG",
                               Description = "General government (A)",
                           },
                           new Industry
                           {
                               Val = "GSLE",
                               Description = "Government enterprises (A)",
                           },
                           new Industry
                           {
                               Val = "NABI",
                               Description = "Not allocated by industry [1] (A,Q)",
                           },
                           new Industry
                           {
                               Val = "PGOOD",
                               Description = "Private goods-producing industries [2] (A,Q)",
                           },
                           new Industry
                           {
                               Val = "PSERV",
                               Description = "Private services-producing industries [3] (A,Q)",
                           },
                           new Industry
                           {
                               Val = "ICT",
                               Description = "Information-communications-technology-producing industries [4] (A)",
                           },

                       };
                return _values;
            }
        }
	}//end Industry
}//end NoFuture.Rand.Gov.Bea.Parameters.GDPbyIndustry