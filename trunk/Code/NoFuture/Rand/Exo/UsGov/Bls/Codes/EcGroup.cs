using System;
using System.Collections.Generic;
namespace NoFuture.Rand.Exo.UsGov.Bls.Codes
{
    public class EcGroup 
    {
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        private static List<EcGroup> _values;
        public static List<EcGroup> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<EcGroup>
                           {
                           
                           new EcGroup
                           {
                               GroupName = "All workers",
                               GroupCode = "000",
                           },
                           new EcGroup
                           {
                               GroupName = "Production and non-supervisory occupations",
                               GroupCode = "101",
                           },
                           new EcGroup
                           {
                               GroupName = "All workers, excluding sales occupations",
                               GroupCode = "106",
                           },
                           new EcGroup
                           {
                               GroupName = "White-collar occupations, excluding sales occupations",
                               GroupCode = "107",
                           },
                           new EcGroup
                           {
                               GroupName = "White-collar occupations",
                               GroupCode = "110",
                           },
                           new EcGroup
                           {
                               GroupName = "Executive, administrative, and managerial occupations",
                               GroupCode = "111",
                           },
                           new EcGroup
                           {
                               GroupName = "Professional, specialty, and technical occupations",
                               GroupCode = "112",
                           },
                           new EcGroup
                           {
                               GroupName = "Sales occupations",
                               GroupCode = "113",
                           },
                           new EcGroup
                           {
                               GroupName = "Administrative support, including clerical, occupations",
                               GroupCode = "114",
                           },
                           new EcGroup
                           {
                               GroupName = "Blue-collar occupations",
                               GroupCode = "120",
                           },
                           new EcGroup
                           {
                               GroupName = "Precision, production, craft, and repair occupations",
                               GroupCode = "121",
                           },
                           new EcGroup
                           {
                               GroupName = "Machine operators, assemblers, and inspectors occupations",
                               GroupCode = "122",
                           },
                           new EcGroup
                           {
                               GroupName = "Transportation and material moving occupations",
                               GroupCode = "123",
                           },
                           new EcGroup
                           {
                               GroupName = "Handlers, equipment cleaners, helpers, and laborers occupations",
                               GroupCode = "124",
                           },
                           new EcGroup
                           {
                               GroupName = "Service occupations",
                               GroupCode = "130",
                           },
                           new EcGroup
                           {
                               GroupName = "All workers, excluding sales occupations",
                               GroupCode = "131",
                           },
                           new EcGroup
                           {
                               GroupName = "White-collar occupations, excluding sales occupations",
                               GroupCode = "132",
                           },
                           new EcGroup
                           {
                               GroupName = "Wholesale and retail trade; excluding sales occupations",
                               GroupCode = "137",
                           },
                           new EcGroup
                           {
                               GroupName = "Finance, insurance, and real estate; excluding sales occupations",
                               GroupCode = "138",
                           },
                           new EcGroup
                           {
                               GroupName = "Wholesale trade; excluding sales occupations",
                               GroupCode = "140",
                           },
                           new EcGroup
                           {
                               GroupName = "Goods-producing industries",
                               GroupCode = "200",
                           },
                           new EcGroup
                           {
                               GroupName = "Goods-producing industries; excluding sales occupations",
                               GroupCode = "201",
                           },
                           new EcGroup
                           {
                               GroupName = "Goods-producing industries; White-collar occupations",
                               GroupCode = "202",
                           },
                           new EcGroup
                           {
                               GroupName = "Goods-producing industries; White-collar occupations,excluding sales occupations",
                               GroupCode = "203",
                           },
                           new EcGroup
                           {
                               GroupName = "Goods-producing industries; Blue-collar occupations",
                               GroupCode = "204",
                           },
                           new EcGroup
                           {
                               GroupName = "Goods-producing industries; Service occupations",
                               GroupCode = "206",
                           },
                           new EcGroup
                           {
                               GroupName = "Service-producing industries",
                               GroupCode = "210",
                           },
                           new EcGroup
                           {
                               GroupName = "Service-producing industries; excluding sales occupations",
                               GroupCode = "211",
                           },
                           new EcGroup
                           {
                               GroupName = "Service-producing industries; White-collar occupations",
                               GroupCode = "212",
                           },
                           new EcGroup
                           {
                               GroupName = "Service-producing industries; White-collar occupations,excluding sales occupations",
                               GroupCode = "213",
                           },
                           new EcGroup
                           {
                               GroupName = "Service-producing industries; Blue-collar occupations",
                               GroupCode = "214",
                           },
                           new EcGroup
                           {
                               GroupName = "Service-producing industries; Service occupations",
                               GroupCode = "216",
                           },
                           new EcGroup
                           {
                               GroupName = "Non-manufacturing industries",
                               GroupCode = "220",
                           },
                           new EcGroup
                           {
                               GroupName = "Non-manufacturing industries; White-collar occupations",
                               GroupCode = "221",
                           },
                           new EcGroup
                           {
                               GroupName = "Non-manufacturing industries; White-collar occupations,excluding sales occupations",
                               GroupCode = "222",
                           },
                           new EcGroup
                           {
                               GroupName = "Non-manufacturing industries; Blue-collar occupations",
                               GroupCode = "223",
                           },
                           new EcGroup
                           {
                               GroupName = "Non-manufacturing industries; Service occupations",
                               GroupCode = "225",
                           },
                           new EcGroup
                           {
                               GroupName = "Construction",
                               GroupCode = "230",
                           },
                           new EcGroup
                           {
                               GroupName = "Manufacturing",
                               GroupCode = "240",
                           },
                           new EcGroup
                           {
                               GroupName = "Manufacturing - durable goods",
                               GroupCode = "241",
                           },
                           new EcGroup
                           {
                               GroupName = "Manufacturing - non-durable goods",
                               GroupCode = "242",
                           },
                           new EcGroup
                           {
                               GroupName = "Manufacturing; White-collar occupations",
                               GroupCode = "243",
                           },
                           new EcGroup
                           {
                               GroupName = "Manufacturing; White-collar occupations, excludingsales occupations",
                               GroupCode = "244",
                           },
                           new EcGroup
                           {
                               GroupName = "Manufacturing; Blue-collar occupations",
                               GroupCode = "245",
                           },
                           new EcGroup
                           {
                               GroupName = "Manufacturing; Service occupations",
                               GroupCode = "247",
                           },
                           new EcGroup
                           {
                               GroupName = "Transportation and Public Utilities",
                               GroupCode = "250",
                           },
                           new EcGroup
                           {
                               GroupName = "Transportation",
                               GroupCode = "251",
                           },
                           new EcGroup
                           {
                               GroupName = "Public utilities",
                               GroupCode = "252",
                           },
                           new EcGroup
                           {
                               GroupName = "Communications",
                               GroupCode = "253",
                           },
                           new EcGroup
                           {
                               GroupName = "Electric, gas, and sanitary services",
                               GroupCode = "254",
                           },
                           new EcGroup
                           {
                               GroupName = "Wholesale and retail trade",
                               GroupCode = "260",
                           },
                           new EcGroup
                           {
                               GroupName = "Wholesale trade",
                               GroupCode = "261",
                           },
                           new EcGroup
                           {
                               GroupName = "Retail trade",
                               GroupCode = "262",
                           },
                           new EcGroup
                           {
                               GroupName = "General merchandise stores",
                               GroupCode = "263",
                           },
                           new EcGroup
                           {
                               GroupName = "Food stores",
                               GroupCode = "264",
                           },
                           new EcGroup
                           {
                               GroupName = "Finance, insurance, and real estate",
                               GroupCode = "270",
                           },
                           new EcGroup
                           {
                               GroupName = "Banking, savings and loan, and other credit agencies",
                               GroupCode = "271",
                           },
                           new EcGroup
                           {
                               GroupName = "Insurance carriers, agents, brokers, and service",
                               GroupCode = "272",
                           },
                           new EcGroup
                           {
                               GroupName = "Insurance, excluding sales occupations",
                               GroupCode = "273",
                           },
                           new EcGroup
                           {
                               GroupName = "Services industries",
                               GroupCode = "280",
                           },
                           new EcGroup
                           {
                               GroupName = "Schools",
                               GroupCode = "281",
                           },
                           new EcGroup
                           {
                               GroupName = "Elementary and secondary schools",
                               GroupCode = "282",
                           },
                           new EcGroup
                           {
                               GroupName = "Services industries, excluding schools",
                               GroupCode = "283",
                           },
                           new EcGroup
                           {
                               GroupName = "Health services",
                               GroupCode = "284",
                           },
                           new EcGroup
                           {
                               GroupName = "Hospitals",
                               GroupCode = "285",
                           },
                           new EcGroup
                           {
                               GroupName = "Nursing and personal care facilities",
                               GroupCode = "286",
                           },
                           new EcGroup
                           {
                               GroupName = "Business services",
                               GroupCode = "287",
                           },
                           new EcGroup
                           {
                               GroupName = "Educational services",
                               GroupCode = "289",
                           },
                           new EcGroup
                           {
                               GroupName = "Public administration",
                               GroupCode = "290",
                           },
                           new EcGroup
                           {
                               GroupName = "Colleges and universities",
                               GroupCode = "299",
                           },
                           new EcGroup
                           {
                               GroupName = "Northeast region",
                               GroupCode = "310",
                           },
                           new EcGroup
                           {
                               GroupName = "South region",
                               GroupCode = "320",
                           },
                           new EcGroup
                           {
                               GroupName = "Midwest region",
                               GroupCode = "330",
                           },
                           new EcGroup
                           {
                               GroupName = "West region",
                               GroupCode = "340",
                           },
                           new EcGroup
                           {
                               GroupName = "Union",
                               GroupCode = "400",
                           },
                           new EcGroup
                           {
                               GroupName = "Union; Blue-collar occupations",
                               GroupCode = "402",
                           },
                           new EcGroup
                           {
                               GroupName = "Union; Manufacturing; Blue-collar occupations",
                               GroupCode = "406",
                           },
                           new EcGroup
                           {
                               GroupName = "Union; Manufacturing",
                               GroupCode = "410",
                           },
                           new EcGroup
                           {
                               GroupName = "Union; Non-manufacturing industries",
                               GroupCode = "420",
                           },
                           new EcGroup
                           {
                               GroupName = "Union; Goods-producing industries",
                               GroupCode = "430",
                           },
                           new EcGroup
                           {
                               GroupName = "Union; Service-producing industries",
                               GroupCode = "440",
                           },
                           new EcGroup
                           {
                               GroupName = "Non-union",
                               GroupCode = "500",
                           },
                           new EcGroup
                           {
                               GroupName = "Non-union; Blue-collar occupations",
                               GroupCode = "502",
                           },
                           new EcGroup
                           {
                               GroupName = "Non-union; Manufacturing; Blue-collar occupations",
                               GroupCode = "506",
                           },
                           new EcGroup
                           {
                               GroupName = "Non-union; Manufacturing",
                               GroupCode = "510",
                           },
                           new EcGroup
                           {
                               GroupName = "Non-union; Non-manufacturing",
                               GroupCode = "520",
                           },
                           new EcGroup
                           {
                               GroupName = "Non-union; Goods-producing industries",
                               GroupCode = "530",
                           },
                           new EcGroup
                           {
                               GroupName = "Non-union; Service-producing industries",
                               GroupCode = "540",
                           },
                           new EcGroup
                           {
                               GroupName = "Metropolitan areas",
                               GroupCode = "600",
                           },
                           new EcGroup
                           {
                               GroupName = "Non-metropolitan areas",
                               GroupCode = "700",
                           },
                           new EcGroup
                           {
                               GroupName = "Aircraft and parts manufacturing (SIC 372)",
                               GroupCode = "800",
                           },
                           new EcGroup
                           {
                               GroupName = "Aircraft and parts manufacturing (SIC 372); White-collar occupations",
                               GroupCode = "801",
                           },
                           new EcGroup
                           {
                               GroupName = "Aircraft and parts manufacturing (SIC 372);Professional, specialty, and technical occupations",
                               GroupCode = "802",
                           },
                           new EcGroup
                           {
                               GroupName = "Aircraft and parts manufacturing (SIC 372); Executive,administrative, and managerial occupations",
                               GroupCode = "805",
                           },
                           new EcGroup
                           {
                               GroupName = "Aircraft and parts manufacturing (SIC 372);Administrative support, including clerical, occupations",
                               GroupCode = "806",
                           },
                           new EcGroup
                           {
                               GroupName = "Aircraft and parts manufacturing (SIC 372); Blue-collar occupations",
                               GroupCode = "807",
                           },
                           new EcGroup
                           {
                               GroupName = "Aircraft and parts manufacturing (SIC 372); Precision,production, craft, and repair occupations",
                               GroupCode = "808",
                           },
                           new EcGroup
                           {
                               GroupName = "Aircraft and parts manufacturing (SIC 372); Machine operators, assemblers, and inspectors cupations",
                               GroupCode = "809",
                           },
                           new EcGroup
                           {
                               GroupName = "Aircraft manufacturing (SIC 3721)",
                               GroupCode = "810",
                           },
                           new EcGroup
                           {
                               GroupName = "Aircraft manufacturing (SIC 3721); White-collar occupations",
                               GroupCode = "811",
                           },
                           new EcGroup
                           {
                               GroupName = "Aircraft manufacturing (SIC 3721); Blue-collar occupations",
                               GroupCode = "812",
                           },
                           new EcGroup
                           {
                               GroupName = "Aircraft engines and engine parts (SIC 3724)",
                               GroupCode = "820",
                           },
                           new EcGroup
                           {
                               GroupName = "Aircraft engines and engine parts (SIC 3724); White-collar occupations",
                               GroupCode = "821",
                           },
                           new EcGroup
                           {
                               GroupName = "Aircraft engines and engine parts (SIC 3724); Blue-collar occupations",
                               GroupCode = "822",
                           },
                           new EcGroup
                           {
                               GroupName = "Aircraft parts and equipment, NEC (SIC 3728)",
                               GroupCode = "830",
                           },
                           new EcGroup
                           {
                               GroupName = "Aircraft parts and equipment, NEC (SIC 3728); White-collar occupations",
                               GroupCode = "831",
                           },
                           new EcGroup
                           {
                               GroupName = "Aircraft parts and equipment, NEC (SIC 3728); Blue-collar occupations",
                               GroupCode = "832",
                           },
                           new EcGroup
                           {
                               GroupName = "Guided missiles and space vehicles manufacturing (SIC 3761)",
                               GroupCode = "840",
                           },
                           new EcGroup
                           {
                               GroupName = "Guided missiles and space vehicles manufacturing (SIC 3761); White-collar occupations",
                               GroupCode = "841",
                           },
                           new EcGroup
                           {
                               GroupName = "Guided missiles and space vehicles manufacturing (SIC 3761); Blue-collar occupations",
                               GroupCode = "842",
                           },
                           new EcGroup
                           {
                               GroupName = "",
                               GroupCode = ".",
                           },

                       };
                return _values;
            }
        }
	}//end EcGroup
}//end NoFuture.Rand.Gov.Bls.Codes