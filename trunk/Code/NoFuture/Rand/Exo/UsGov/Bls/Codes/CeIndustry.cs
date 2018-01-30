using System;
using System.Collections.Generic;
namespace NoFuture.Rand.Exo.UsGov.Bls.Codes
{
    public class CeIndustry 
    {
        public string DisplayLevel { get; set; }
        public string IndustryCode { get; set; }
        public string IndustryName { get; set; }
        public string NaicsCode { get; set; }
        public string PublishingStatus { get; set; }
        public string Selectable { get; set; }
        public string SortSequence { get; set; }
        private static List<CeIndustry> _values;
        public static List<CeIndustry> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<CeIndustry>
                           {
                           
                           new CeIndustry
                           {
                               IndustryCode = "00000000",
                               IndustryName = "Total nonfarm",
                               Selectable = "T",
                               DisplayLevel = "0",
                               SortSequence = "1",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "05000000",
                               IndustryName = "Total private",
                               Selectable = "T",
                               DisplayLevel = "1",
                               SortSequence = "2",
                               NaicsCode = "-",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "06000000",
                               IndustryName = "Goods-producing",
                               Selectable = "T",
                               DisplayLevel = "1",
                               SortSequence = "3",
                               NaicsCode = "-",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "07000000",
                               IndustryName = "Service-providing",
                               Selectable = "T",
                               DisplayLevel = "1",
                               SortSequence = "4",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "08000000",
                               IndustryName = "Private service-providing",
                               Selectable = "T",
                               DisplayLevel = "1",
                               SortSequence = "5",
                               NaicsCode = "-",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "10000000",
                               IndustryName = "Mining and logging",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "6",
                               NaicsCode = "-",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "10113300",
                               IndustryName = "Logging",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "7",
                               NaicsCode = "1133",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "10210000",
                               IndustryName = "Mining",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "8",
                               NaicsCode = "21",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "10211000",
                               IndustryName = "Oil and gas extraction",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "9",
                               NaicsCode = "211",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "10212000",
                               IndustryName = "Mining, except oil and gas",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "10",
                               NaicsCode = "212",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "10212100",
                               IndustryName = "Coal mining",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "11",
                               NaicsCode = "2121",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "10212111",
                               IndustryName = "Bituminous coal and lignite surface mining",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "12",
                               NaicsCode = "212111",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "10212113",
                               IndustryName = "Bituminous coal underground mining and anthracite mining",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "13",
                               NaicsCode = "212112,3",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "10212200",
                               IndustryName = "Metal ore mining",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "14",
                               NaicsCode = "2122",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "10212300",
                               IndustryName = "Nonmetallic mineral mining and quarrying",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "15",
                               NaicsCode = "2123",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "10212310",
                               IndustryName = "Stone mining and quarrying",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "16",
                               NaicsCode = "21231",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "10212312",
                               IndustryName = "Crushed and broken limestone mining",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "17",
                               NaicsCode = "212312",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "10212319",
                               IndustryName = "Other stone mining and quarrying",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "18",
                               NaicsCode = "212311,3,9",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "10212320",
                               IndustryName = "Sand, gravel, clay, and refractory mining",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "19",
                               NaicsCode = "21232",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "10212321",
                               IndustryName = "Construction sand and gravel mining",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "20",
                               NaicsCode = "212321",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "10212390",
                               IndustryName = "Other nonmetallic mineral mining",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "21",
                               NaicsCode = "21239",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "10213000",
                               IndustryName = "Support activities for mining",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "22",
                               NaicsCode = "213",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "10213112",
                               IndustryName = "Support activities for oil and gas operations",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "23",
                               NaicsCode = "213112",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20000000",
                               IndustryName = "Construction",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "24",
                               NaicsCode = "23",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20236000",
                               IndustryName = "Construction of buildings",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "25",
                               NaicsCode = "236",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20236100",
                               IndustryName = "Residential building",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "26",
                               NaicsCode = "2361",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20236115",
                               IndustryName = "New single-family general contractors",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "27",
                               NaicsCode = "236115",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20236116",
                               IndustryName = "New multifamily general contractors",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "28",
                               NaicsCode = "236116",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20236117",
                               IndustryName = "New housing operative builders",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "29",
                               NaicsCode = "236117",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20236118",
                               IndustryName = "Residential remodelers",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "30",
                               NaicsCode = "236118",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20236200",
                               IndustryName = "Nonresidential building",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "31",
                               NaicsCode = "2362",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20236210",
                               IndustryName = "Industrial building",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "32",
                               NaicsCode = "23621",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20236220",
                               IndustryName = "Commercial building",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "33",
                               NaicsCode = "23622",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20237000",
                               IndustryName = "Heavy and civil engineering construction",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "34",
                               NaicsCode = "237",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20237100",
                               IndustryName = "Utility system construction",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "35",
                               NaicsCode = "2371",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20237110",
                               IndustryName = "Water and sewer system construction",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "36",
                               NaicsCode = "23711",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20237120",
                               IndustryName = "Oil and gas pipeline construction",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "37",
                               NaicsCode = "23712",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20237130",
                               IndustryName = "Power and communication system construction",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "38",
                               NaicsCode = "23713",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20237200",
                               IndustryName = "Land subdivision",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "39",
                               NaicsCode = "2372",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20237300",
                               IndustryName = "Highway, street, and bridge construction",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "40",
                               NaicsCode = "2373",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20237900",
                               IndustryName = "Other heavy construction",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "41",
                               NaicsCode = "2379",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238000",
                               IndustryName = "Specialty trade contractors",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "42",
                               NaicsCode = "238",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238001",
                               IndustryName = "Residential specialty trade contractors",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "43",
                               NaicsCode = "PART 238",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238002",
                               IndustryName = "Nonresidential specialty trade contractors",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "44",
                               NaicsCode = "PART 238",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238100",
                               IndustryName = "Building foundation and exterior contractors",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "45",
                               NaicsCode = "2381",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238101",
                               IndustryName = "Residential building foundation and exterior contractors",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "46",
                               NaicsCode = "PART 2381",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238102",
                               IndustryName = "Nonresidential building foundation and exterior contractors",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "47",
                               NaicsCode = "PART 2381",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238110",
                               IndustryName = "Poured concrete structure contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "48",
                               NaicsCode = "23811",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238120",
                               IndustryName = "Steel and precast concrete contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "49",
                               NaicsCode = "23812",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238130",
                               IndustryName = "Framing contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "50",
                               NaicsCode = "23813",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238140",
                               IndustryName = "Masonry contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "51",
                               NaicsCode = "23814",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238150",
                               IndustryName = "Glass and glazing contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "52",
                               NaicsCode = "23815",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238160",
                               IndustryName = "Roofing contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "53",
                               NaicsCode = "23816",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238170",
                               IndustryName = "Siding contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "54",
                               NaicsCode = "23817",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238190",
                               IndustryName = "Other building exterior contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "55",
                               NaicsCode = "23819",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238200",
                               IndustryName = "Building equipment contractors",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "56",
                               NaicsCode = "2382",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238201",
                               IndustryName = "Residential building equipment contractors",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "57",
                               NaicsCode = "PART 2382",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238202",
                               IndustryName = "Nonresidential building equipment contractors",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "58",
                               NaicsCode = "PART 2382",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238210",
                               IndustryName = "Electrical contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "59",
                               NaicsCode = "23821",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238220",
                               IndustryName = "Plumbing and HVAC contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "60",
                               NaicsCode = "23822",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238290",
                               IndustryName = "Other building equipment contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "61",
                               NaicsCode = "23829",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238300",
                               IndustryName = "Building finishing contractors",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "62",
                               NaicsCode = "2383",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238301",
                               IndustryName = "Residential building finishing contractors",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "63",
                               NaicsCode = "PART 2383",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238302",
                               IndustryName = "Nonresidential building finishing contractors",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "64",
                               NaicsCode = "PART 2383",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238310",
                               IndustryName = "Drywall and insulation contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "65",
                               NaicsCode = "23831",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238320",
                               IndustryName = "Painting and wall covering contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "66",
                               NaicsCode = "23832",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238330",
                               IndustryName = "Flooring contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "67",
                               NaicsCode = "23833",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238340",
                               IndustryName = "Tile and terrazzo contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "68",
                               NaicsCode = "23834",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238350",
                               IndustryName = "Finish carpentry contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "69",
                               NaicsCode = "23835",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238390",
                               IndustryName = "Other building finishing contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "70",
                               NaicsCode = "23839",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238900",
                               IndustryName = "Other specialty trade contractors",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "71",
                               NaicsCode = "2389",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238901",
                               IndustryName = "Other residential trade contractors",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "72",
                               NaicsCode = "PART 2389",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238902",
                               IndustryName = "Other nonresidential trade contractors",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "73",
                               NaicsCode = "PART 2389",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238910",
                               IndustryName = "Site preparation contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "74",
                               NaicsCode = "23891",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "20238990",
                               IndustryName = "All other specialty trade contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "75",
                               NaicsCode = "23899",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "30000000",
                               IndustryName = "Manufacturing",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "76",
                               NaicsCode = "-",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31000000",
                               IndustryName = "Durable goods",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "77",
                               NaicsCode = "-",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31321000",
                               IndustryName = "Wood products",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "78",
                               NaicsCode = "321",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31321100",
                               IndustryName = "Sawmills and wood preservation",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "79",
                               NaicsCode = "3211",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31321200",
                               IndustryName = "Plywood and engineered wood products",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "80",
                               NaicsCode = "3212",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31321212",
                               IndustryName = "Hardwood and softwood veneer and plywood",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "81",
                               NaicsCode = "321211,2",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31321214",
                               IndustryName = "All other plywood and engineered wood products",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "82",
                               NaicsCode = "321213,4,9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31321900",
                               IndustryName = "Other wood products",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "83",
                               NaicsCode = "3219",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31321910",
                               IndustryName = "Millwork",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "84",
                               NaicsCode = "32191",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31321911",
                               IndustryName = "Wood windows and doors",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "85",
                               NaicsCode = "321911",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31321918",
                               IndustryName = "Cut stock, resawing lumber, planing, and other millwork, including flooring",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "86",
                               NaicsCode = "321912,8",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31321920",
                               IndustryName = "Wood containers and pallets",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "87",
                               NaicsCode = "32192",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31321990",
                               IndustryName = "All other wood products",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "88",
                               NaicsCode = "32199",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31327000",
                               IndustryName = "Nonmetallic mineral products",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "89",
                               NaicsCode = "327",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31327100",
                               IndustryName = "Clay products and refractories",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "90",
                               NaicsCode = "3271",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31327200",
                               IndustryName = "Glass and glass products",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "91",
                               NaicsCode = "3272",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31327212",
                               IndustryName = "Flat glass and other pressed and blown glass and glassware",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "92",
                               NaicsCode = "327211,2",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31327215",
                               IndustryName = "Glass containers and products made of purchased glass",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "93",
                               NaicsCode = "327213,5",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31327300",
                               IndustryName = "Cement and concrete products",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "94",
                               NaicsCode = "3273",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31327320",
                               IndustryName = "Ready-mix concrete",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "95",
                               NaicsCode = "32732",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31327390",
                               IndustryName = "Other cement and concrete products",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "96",
                               NaicsCode = "32731,3,9",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31327900",
                               IndustryName = "Lime, gypsum, and other nonmetallic mineral products",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "97",
                               NaicsCode = "3274,9",
                               PublishingStatus = "ET",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31331000",
                               IndustryName = "Primary metals",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "98",
                               NaicsCode = "331",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31331100",
                               IndustryName = "Iron and steel mills and ferroalloy production",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "99",
                               NaicsCode = "3311",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31331200",
                               IndustryName = "Steel products from purchased steel",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "100",
                               NaicsCode = "3312",
                               PublishingStatus = "ET",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31331300",
                               IndustryName = "Alumina and aluminum production",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "101",
                               NaicsCode = "3313",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31331400",
                               IndustryName = "Other nonferrous metal production",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "102",
                               NaicsCode = "3314",
                               PublishingStatus = "ET",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31331500",
                               IndustryName = "Foundries",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "103",
                               NaicsCode = "3315",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31331510",
                               IndustryName = "Ferrous metal foundries",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "104",
                               NaicsCode = "33151",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31331511",
                               IndustryName = "Iron foundries",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "105",
                               NaicsCode = "331511",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31331520",
                               IndustryName = "Nonferrous metal foundries",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "106",
                               NaicsCode = "33152",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332000",
                               IndustryName = "Fabricated metal products",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "107",
                               NaicsCode = "332",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332100",
                               IndustryName = "Forging and stamping",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "108",
                               NaicsCode = "3321",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332200",
                               IndustryName = "Cutlery and hand tools",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "109",
                               NaicsCode = "3322",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332300",
                               IndustryName = "Architectural and structural metals",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "110",
                               NaicsCode = "3323",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332310",
                               IndustryName = "Plate work and fabricated structural products",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "111",
                               NaicsCode = "33231",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332312",
                               IndustryName = "Fabricated structural metal products",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "112",
                               NaicsCode = "332312",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332313",
                               IndustryName = "Prefabricated metal buildings, components, and plate work",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "113",
                               NaicsCode = "332311,3",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332320",
                               IndustryName = "Ornamental and architectural metal products",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "114",
                               NaicsCode = "33232",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332321",
                               IndustryName = "Metal windows and doors",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "115",
                               NaicsCode = "332321",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332322",
                               IndustryName = "Sheet metal work",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "116",
                               NaicsCode = "332322",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332323",
                               IndustryName = "Ornamental and architectural metal work",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "117",
                               NaicsCode = "332323",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332400",
                               IndustryName = "Boilers, tanks, and shipping containers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "118",
                               NaicsCode = "3324",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332600",
                               IndustryName = "Hardware, spring, and wire products",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "119",
                               NaicsCode = "3325,6",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332700",
                               IndustryName = "Machine shops and threaded products",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "120",
                               NaicsCode = "3327",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332710",
                               IndustryName = "Machine shops",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "121",
                               NaicsCode = "33271",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332720",
                               IndustryName = "Turned products and screws, nuts, and bolts",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "122",
                               NaicsCode = "33272",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332721",
                               IndustryName = "Precision turned products",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "123",
                               NaicsCode = "332721",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332722",
                               IndustryName = "Bolts, nuts, screws, rivets, and washers",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "124",
                               NaicsCode = "332722",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332800",
                               IndustryName = "Coating, engraving, and heat treating metals",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "125",
                               NaicsCode = "3328",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332812",
                               IndustryName = "Metal heat treating and coating and nonprecious engraving",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "126",
                               NaicsCode = "332811,2",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332813",
                               IndustryName = "Electroplating, anodizing, and coloring metals",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "127",
                               NaicsCode = "332813",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332900",
                               IndustryName = "Other fabricated metal products",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "128",
                               NaicsCode = "3329",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332910",
                               IndustryName = "Metal valves",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "129",
                               NaicsCode = "33291",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332912",
                               IndustryName = "Fluid power valves and hose fittings",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "130",
                               NaicsCode = "332912",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332919",
                               IndustryName = "All other metal valves",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "131",
                               NaicsCode = "332911,3,9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332990",
                               IndustryName = "All other fabricated metal products",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "132",
                               NaicsCode = "33299",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332994",
                               IndustryName = "Small arms, ammunition, and other ordnance and accessories",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "133",
                               NaicsCode = "332992,3,4",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31332999",
                               IndustryName = "Miscellaneous fabricated metal products and ball and roller bearings",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "134",
                               NaicsCode = "332991,6,9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31333000",
                               IndustryName = "Machinery",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "135",
                               NaicsCode = "333",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31333100",
                               IndustryName = "Agricultural, construction, and mining machinery",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "136",
                               NaicsCode = "3331",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31333110",
                               IndustryName = "Agricultural implements",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "137",
                               NaicsCode = "33311",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31333111",
                               IndustryName = "Farm machinery and equipment",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "138",
                               NaicsCode = "333111",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31333120",
                               IndustryName = "Construction machinery",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "139",
                               NaicsCode = "33312",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31333130",
                               IndustryName = "Mining and oil and gas field machinery",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "140",
                               NaicsCode = "33313",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31333200",
                               IndustryName = "Industrial machinery",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "141",
                               NaicsCode = "3332",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31333300",
                               IndustryName = "Commercial and service industry machinery",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "142",
                               NaicsCode = "3333",
                               PublishingStatus = "AT",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31333400",
                               IndustryName = "HVAC and commercial refrigeration equipment",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "143",
                               NaicsCode = "3334",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31333415",
                               IndustryName = "AC, refrigeration, and forced air heating",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "144",
                               NaicsCode = "333415",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31333500",
                               IndustryName = "Metalworking machinery",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "145",
                               NaicsCode = "3335",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31333511",
                               IndustryName = "Industrial molds",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "146",
                               NaicsCode = "333511",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31333514",
                               IndustryName = "Special tools, dies, jigs, and fixtures",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "147",
                               NaicsCode = "333514",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31333517",
                               IndustryName = "Machine tool manufacturing",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "148",
                               NaicsCode = "333517",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31333519",
                               IndustryName = "Miscellaneous metalworking machinery",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "149",
                               NaicsCode = "333515,9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31333600",
                               IndustryName = "Turbine and power transmission equipment",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "150",
                               NaicsCode = "3336",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31333900",
                               IndustryName = "Other general purpose machinery",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "151",
                               NaicsCode = "3339",
                               PublishingStatus = "AT",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31333910",
                               IndustryName = "Pumps and compressors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "152",
                               NaicsCode = "33391",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31333920",
                               IndustryName = "Material handling equipment",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "153",
                               NaicsCode = "33392",
                               PublishingStatus = "ET",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31333990",
                               IndustryName = "All other general purpose machinery",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "154",
                               NaicsCode = "33399",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31334000",
                               IndustryName = "Computer and electronic products",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "155",
                               NaicsCode = "334",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31334100",
                               IndustryName = "Computer and peripheral equipment",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "156",
                               NaicsCode = "3341",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31334111",
                               IndustryName = "Electronic computers",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "157",
                               NaicsCode = "334111",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31334118",
                               IndustryName = "Computer storage devices, terminals, and other peripheral equipment",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "158",
                               NaicsCode = "334112,8",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31334200",
                               IndustryName = "Communications equipment",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "159",
                               NaicsCode = "3342",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31334210",
                               IndustryName = "Telephone apparatus",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "160",
                               NaicsCode = "33421",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31334220",
                               IndustryName = "Broadcast and wireless communications equipment",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "161",
                               NaicsCode = "33422",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31334400",
                               IndustryName = "Semiconductors and electronic components",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "162",
                               NaicsCode = "3344",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31334412",
                               IndustryName = "Bare printed circuit boards",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "163",
                               NaicsCode = "334412",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31334413",
                               IndustryName = "Semiconductors and related devices",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "164",
                               NaicsCode = "334413",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31334418",
                               IndustryName = "Printed circuit assemblies",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "165",
                               NaicsCode = "334418",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31334419",
                               IndustryName = "Electronic connectors and misc. electronic components",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "166",
                               NaicsCode = "334416,7,9",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31334500",
                               IndustryName = "Electronic instruments",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "167",
                               NaicsCode = "3345",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31334510",
                               IndustryName = "Electromedical apparatus",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "168",
                               NaicsCode = "334510",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31334511",
                               IndustryName = "Search, detection, and navigation instruments",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "169",
                               NaicsCode = "334511",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31334513",
                               IndustryName = "Industrial process variable instruments",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "170",
                               NaicsCode = "334513",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31334515",
                               IndustryName = "Electricity and signal testing instruments",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "171",
                               NaicsCode = "334515",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31334519",
                               IndustryName = "Miscellaneous electronic instruments",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "172",
                               NaicsCode = "334512,4,6-9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31334600",
                               IndustryName = "Miscellaneous computer and electronic products",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "173",
                               NaicsCode = "3343,6",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31335000",
                               IndustryName = "Electrical equipment and appliances",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "174",
                               NaicsCode = "335",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31335100",
                               IndustryName = "Electric lighting equipment",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "175",
                               NaicsCode = "3351",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31335200",
                               IndustryName = "Household appliances",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "176",
                               NaicsCode = "3352",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31335300",
                               IndustryName = "Electrical equipment",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "177",
                               NaicsCode = "3353",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31335311",
                               IndustryName = "Electric power and specialty transformers",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "178",
                               NaicsCode = "335311",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31335312",
                               IndustryName = "Motors and generators",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "179",
                               NaicsCode = "335312",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31335313",
                               IndustryName = "Switchgear and switchboard apparatus",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "180",
                               NaicsCode = "335313",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31335314",
                               IndustryName = "Relays and industrial controls",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "181",
                               NaicsCode = "335314",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31335900",
                               IndustryName = "Other electrical equipment and components",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "182",
                               NaicsCode = "3359",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31335910",
                               IndustryName = "Batteries",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "183",
                               NaicsCode = "33591",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31335930",
                               IndustryName = "Wiring devices",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "184",
                               NaicsCode = "33593",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31335990",
                               IndustryName = "All other electrical equipment and components",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "185",
                               NaicsCode = "33592,9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336000",
                               IndustryName = "Transportation equipment",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "186",
                               NaicsCode = "336",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336001",
                               IndustryName = "Motor vehicles and parts",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "187",
                               NaicsCode = "3361,2,3",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336100",
                               IndustryName = "Motor vehicles",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "188",
                               NaicsCode = "3361",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336110",
                               IndustryName = "Automobiles and light trucks",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "189",
                               NaicsCode = "33611",
                               PublishingStatus = "G",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336111",
                               IndustryName = "Automobiles",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "190",
                               NaicsCode = "336111",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336112",
                               IndustryName = "Light trucks and utility vehicles",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "191",
                               NaicsCode = "336112",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336120",
                               IndustryName = "Heavy duty trucks",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "192",
                               NaicsCode = "33612",
                               PublishingStatus = "G",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336200",
                               IndustryName = "Motor vehicle bodies and trailers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "193",
                               NaicsCode = "3362",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336211",
                               IndustryName = "Motor vehicle bodies",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "194",
                               NaicsCode = "336211",
                               PublishingStatus = "ET",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336214",
                               IndustryName = "Truck trailers, motor homes, travel trailers, and campers",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "195",
                               NaicsCode = "336212,3,4",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336300",
                               IndustryName = "Motor vehicle parts",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "196",
                               NaicsCode = "3363",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336310",
                               IndustryName = "Motor vehicle gasoline engine and parts",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "197",
                               NaicsCode = "33631",
                               PublishingStatus = "AP",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336320",
                               IndustryName = "Motor vehicle electric equipment",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "198",
                               NaicsCode = "33632",
                               PublishingStatus = "ET",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336330",
                               IndustryName = "Motor vehicle steering and suspension parts",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "199",
                               NaicsCode = "33633",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336340",
                               IndustryName = "Motor vehicle brake systems",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "200",
                               NaicsCode = "33634",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336350",
                               IndustryName = "Motor vehicle power train components",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "201",
                               NaicsCode = "33635",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336360",
                               IndustryName = "Motor vehicle seating and interior trim",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "202",
                               NaicsCode = "33636",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336370",
                               IndustryName = "Motor vehicle metal stamping",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "203",
                               NaicsCode = "33637",
                               PublishingStatus = "AP",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336390",
                               IndustryName = "All other motor vehicle parts",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "204",
                               NaicsCode = "33639",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336400",
                               IndustryName = "Aerospace products and parts",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "205",
                               NaicsCode = "3364",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336411",
                               IndustryName = "Aircraft",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "206",
                               NaicsCode = "336411",
                               PublishingStatus = "D",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336412",
                               IndustryName = "Aircraft engines and engine parts",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "207",
                               NaicsCode = "336412",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336413",
                               IndustryName = "Other aircraft parts and equipment",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "208",
                               NaicsCode = "336413",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336419",
                               IndustryName = "Guided missiles, space vehicles, and parts",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "209",
                               NaicsCode = "336414,5,9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336600",
                               IndustryName = "Ship and boat building",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "210",
                               NaicsCode = "3366",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336611",
                               IndustryName = "Ship building and repairing",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "211",
                               NaicsCode = "336611",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336612",
                               IndustryName = "Boat building",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "212",
                               NaicsCode = "336612",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31336900",
                               IndustryName = "Railroad rolling stock and other transportation equipment",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "213",
                               NaicsCode = "3365,9",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31337000",
                               IndustryName = "Furniture and related products",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "214",
                               NaicsCode = "337",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31337100",
                               IndustryName = "Household and institutional furniture",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "215",
                               NaicsCode = "3371",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31337110",
                               IndustryName = "Wood kitchen cabinets and countertops",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "216",
                               NaicsCode = "33711",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31337120",
                               IndustryName = "Other household and institutional furniture",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "217",
                               NaicsCode = "33712",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31337121",
                               IndustryName = "Upholstered household furniture",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "218",
                               NaicsCode = "337121",
                               PublishingStatus = "AT",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31337122",
                               IndustryName = "Nonupholstered wood household furniture",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "219",
                               NaicsCode = "337122",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31337127",
                               IndustryName = "Miscellaneous household and institutional furniture",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "220",
                               NaicsCode = "337124,5,7",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31337200",
                               IndustryName = "Office furniture and fixtures",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "221",
                               NaicsCode = "3372",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31337212",
                               IndustryName = "Wood office furniture and custom architectural woodwork and millwork",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "222",
                               NaicsCode = "337211,2",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31337214",
                               IndustryName = "Office furniture, except wood",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "223",
                               NaicsCode = "337214",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31337215",
                               IndustryName = "Showcases, partitions, shelving, and lockers",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "224",
                               NaicsCode = "337215",
                               PublishingStatus = "ET",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31337900",
                               IndustryName = "Other furniture-related products",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "225",
                               NaicsCode = "3379",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31339000",
                               IndustryName = "Miscellaneous durable goods manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "226",
                               NaicsCode = "339",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31339100",
                               IndustryName = "Medical equipment and supplies",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "227",
                               NaicsCode = "3391",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31339112",
                               IndustryName = "Surgical and medical instruments",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "228",
                               NaicsCode = "339112",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31339113",
                               IndustryName = "Surgical appliances and supplies",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "229",
                               NaicsCode = "339113",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31339116",
                               IndustryName = "Dental laboratories",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "230",
                               NaicsCode = "339116",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31339900",
                               IndustryName = "Other miscellaneous durable goods manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "231",
                               NaicsCode = "3399",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31339910",
                               IndustryName = "Jewelry and silverware",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "232",
                               NaicsCode = "33991",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31339920",
                               IndustryName = "Sporting and athletic goods",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "233",
                               NaicsCode = "33992",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31339950",
                               IndustryName = "Signs",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "234",
                               NaicsCode = "33995",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "31339990",
                               IndustryName = "All other miscellaneous durable goods manufacturing",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "235",
                               NaicsCode = "33993,4,9",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32000000",
                               IndustryName = "Nondurable goods",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "236",
                               NaicsCode = "-",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311000",
                               IndustryName = "Food manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "237",
                               NaicsCode = "311",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311100",
                               IndustryName = "Animal food",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "238",
                               NaicsCode = "3111",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311200",
                               IndustryName = "Grain and oilseed milling",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "239",
                               NaicsCode = "3112",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311220",
                               IndustryName = "Flour milling, malt, starch, and vegetable oil",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "240",
                               NaicsCode = "31121,2",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311230",
                               IndustryName = "Breakfast cereal",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "241",
                               NaicsCode = "31123",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311300",
                               IndustryName = "Sugar and confectionery products",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "242",
                               NaicsCode = "3113",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311350",
                               IndustryName = "Chocolate and confectionery manufacturing",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "243",
                               NaicsCode = "31135",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311400",
                               IndustryName = "Fruit and vegetable preserving and specialty",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "244",
                               NaicsCode = "3114",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311410",
                               IndustryName = "Frozen food",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "245",
                               NaicsCode = "31141",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311411",
                               IndustryName = "Frozen fruits and vegetables",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "246",
                               NaicsCode = "311411",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311412",
                               IndustryName = "Frozen specialty food",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "247",
                               NaicsCode = "311412",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311420",
                               IndustryName = "Fruit and vegetable canning and drying",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "248",
                               NaicsCode = "31142",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311500",
                               IndustryName = "Dairy products",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "249",
                               NaicsCode = "3115",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311511",
                               IndustryName = "Fluid milk",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "250",
                               NaicsCode = "311511",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311600",
                               IndustryName = "Animal slaughtering and processing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "251",
                               NaicsCode = "3116",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311611",
                               IndustryName = "Animal, except poultry, slaughtering",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "252",
                               NaicsCode = "311611",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311613",
                               IndustryName = "Meat processed from carcasses, and rendering and meat byproduct processing",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "253",
                               NaicsCode = "311612,3",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311615",
                               IndustryName = "Poultry processing",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "254",
                               NaicsCode = "311615",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311700",
                               IndustryName = "Seafood product preparation and packaging",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "255",
                               NaicsCode = "3117",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311800",
                               IndustryName = "Bakeries and tortilla manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "256",
                               NaicsCode = "3118",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311810",
                               IndustryName = "Bread and bakery products",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "257",
                               NaicsCode = "31181",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311811",
                               IndustryName = "Retail bakeries",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "258",
                               NaicsCode = "311811",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311813",
                               IndustryName = "Commercial bakeries and frozen cakes and other pastry products",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "259",
                               NaicsCode = "311812,3",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311830",
                               IndustryName = "Cookies, crackers, pasta, and tortillas",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "260",
                               NaicsCode = "31182,3",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311900",
                               IndustryName = "Other food products",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "261",
                               NaicsCode = "3119",
                               PublishingStatus = "ET",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311910",
                               IndustryName = "Snack food",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "262",
                               NaicsCode = "31191",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32311990",
                               IndustryName = "Miscellaneous food products",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "263",
                               NaicsCode = "31192,3,4,9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32313000",
                               IndustryName = "Textile mills",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "264",
                               NaicsCode = "313",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32313100",
                               IndustryName = "Fiber, yarn, and thread mills",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "265",
                               NaicsCode = "3131",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32313200",
                               IndustryName = "Fabric mills",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "266",
                               NaicsCode = "3132",
                               PublishingStatus = "ET",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32313210",
                               IndustryName = "Broadwoven fabric mills",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "267",
                               NaicsCode = "31321",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32313300",
                               IndustryName = "Textile and fabric finishing mills",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "268",
                               NaicsCode = "3133",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32314000",
                               IndustryName = "Textile product mills",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "269",
                               NaicsCode = "314",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32314100",
                               IndustryName = "Textile furnishings mills",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "270",
                               NaicsCode = "3141",
                               PublishingStatus = "ET",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32314900",
                               IndustryName = "Other textile product mills",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "271",
                               NaicsCode = "3149",
                               PublishingStatus = "ET",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32314910",
                               IndustryName = "Textile bag and canvas mills",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "272",
                               NaicsCode = "31491",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32314990",
                               IndustryName = "All other textile product mills",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "273",
                               NaicsCode = "31499",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32315000",
                               IndustryName = "Apparel",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "274",
                               NaicsCode = "315",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32315200",
                               IndustryName = "Cut and sew apparel",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "275",
                               NaicsCode = "3152",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32315210",
                               IndustryName = "Cut and sew apparel contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "276",
                               NaicsCode = "31521",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32315280",
                               IndustryName = "Cut and sew apparel, except contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "277",
                               NaicsCode = "31522,4,8",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32315900",
                               IndustryName = "All other apparel manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "278",
                               NaicsCode = "3151,9",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32322000",
                               IndustryName = "Paper and paper products",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "279",
                               NaicsCode = "322",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32322100",
                               IndustryName = "Pulp, paper, and paperboard mills",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "280",
                               NaicsCode = "3221",
                               PublishingStatus = "AT",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32322120",
                               IndustryName = "Pulp mills and paper mills",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "281",
                               NaicsCode = "32211,2",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32322130",
                               IndustryName = "Paperboard mills",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "282",
                               NaicsCode = "32213",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32322200",
                               IndustryName = "Converted paper products",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "283",
                               NaicsCode = "3222",
                               PublishingStatus = "AT",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32322210",
                               IndustryName = "Paperboard containers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "284",
                               NaicsCode = "32221",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32322211",
                               IndustryName = "Corrugated and solid fiber boxes",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "285",
                               NaicsCode = "322211",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32322219",
                               IndustryName = "Folding boxes and miscellaneous paperboard containers",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "286",
                               NaicsCode = "322212,9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32322220",
                               IndustryName = "Paper bags and coated and treated paper",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "287",
                               NaicsCode = "32222",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32322290",
                               IndustryName = "Stationery and other converted paper products",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "288",
                               NaicsCode = "32223,9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32323000",
                               IndustryName = "Printing and related support activities",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "289",
                               NaicsCode = "323",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32323110",
                               IndustryName = "Printing",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "290",
                               NaicsCode = "32311",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32323113",
                               IndustryName = "Commercial screen printing",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "291",
                               NaicsCode = "323113",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32323117",
                               IndustryName = "Commercial printing, except screen",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "292",
                               NaicsCode = "323111,7",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32323120",
                               IndustryName = "Support activities for printing",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "293",
                               NaicsCode = "32312",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32324000",
                               IndustryName = "Petroleum and coal products",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "294",
                               NaicsCode = "324",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32324110",
                               IndustryName = "Petroleum refineries",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "295",
                               NaicsCode = "32411",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32324190",
                               IndustryName = "Asphalt paving and roofing materials and other petroleum and coal products",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "296",
                               NaicsCode = "32412,9",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32325000",
                               IndustryName = "Chemicals",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "297",
                               NaicsCode = "325",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32325100",
                               IndustryName = "Basic chemicals",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "298",
                               NaicsCode = "3251",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32325130",
                               IndustryName = "Petrochemicals, industrial gases, synthetic dyes, and pigments",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "299",
                               NaicsCode = "32511,2,3",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32325180",
                               IndustryName = "Other basic inorganic chemicals",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "300",
                               NaicsCode = "32518",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32325190",
                               IndustryName = "Other basic organic chemicals",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "301",
                               NaicsCode = "32519",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32325200",
                               IndustryName = "Resin, rubber, and artificial fibers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "302",
                               NaicsCode = "3252",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32325211",
                               IndustryName = "Plastics material and resin",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "303",
                               NaicsCode = "325211",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32325300",
                               IndustryName = "Agricultural chemicals",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "304",
                               NaicsCode = "3253",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32325400",
                               IndustryName = "Pharmaceuticals and medicines",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "305",
                               NaicsCode = "3254",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32325412",
                               IndustryName = "Pharmaceutical preparations",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "306",
                               NaicsCode = "325412",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32325414",
                               IndustryName = "Miscellaneous medicinal and biological products",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "307",
                               NaicsCode = "325411,3,4",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32325500",
                               IndustryName = "Paints, coatings, and adhesives",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "308",
                               NaicsCode = "3255",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32325510",
                               IndustryName = "Paints and coatings",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "309",
                               NaicsCode = "32551",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32325600",
                               IndustryName = "Soaps, cleaning compounds, and toiletries",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "310",
                               NaicsCode = "3256",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32325610",
                               IndustryName = "Soaps and cleaning compounds",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "311",
                               NaicsCode = "32561",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32325620",
                               IndustryName = "Toilet preparations",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "312",
                               NaicsCode = "32562",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32325900",
                               IndustryName = "Other chemical products and preparations",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "313",
                               NaicsCode = "3259",
                               PublishingStatus = "ET",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32326000",
                               IndustryName = "Plastics and rubber products",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "314",
                               NaicsCode = "326",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32326100",
                               IndustryName = "Plastics products",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "315",
                               NaicsCode = "3261",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32326110",
                               IndustryName = "Plastics packaging materials, film, and sheet",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "316",
                               NaicsCode = "32611",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32326113",
                               IndustryName = "Nonpackaging plastics film and sheet",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "317",
                               NaicsCode = "326113",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32326120",
                               IndustryName = "Plastics pipe, fittings, and profile shapes",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "318",
                               NaicsCode = "32612",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32326150",
                               IndustryName = "Foam products",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "319",
                               NaicsCode = "32614,5",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32326160",
                               IndustryName = "Plastics bottles and laminated plastics plate, sheet, and shapes",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "320",
                               NaicsCode = "32613,6",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32326190",
                               IndustryName = "Other plastics products",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "321",
                               NaicsCode = "32619",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32326200",
                               IndustryName = "Rubber products",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "322",
                               NaicsCode = "3262",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32326210",
                               IndustryName = "Tires",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "323",
                               NaicsCode = "32621",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32326290",
                               IndustryName = "All other rubber products",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "324",
                               NaicsCode = "32622,9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32329000",
                               IndustryName = "Miscellaneous nondurable goods manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "325",
                               NaicsCode = "312,6",
                               PublishingStatus = "AO",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32329100",
                               IndustryName = "Beverages",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "326",
                               NaicsCode = "3121",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32329110",
                               IndustryName = "Soft drinks and ice",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "327",
                               NaicsCode = "31211",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32329111",
                               IndustryName = "Soft drinks",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "328",
                               NaicsCode = "312111",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32329140",
                               IndustryName = "Breweries, wineries, and distilleries",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "329",
                               NaicsCode = "31212,3,4",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32329200",
                               IndustryName = "Tobacco and tobacco products",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "330",
                               NaicsCode = "3122",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "32329300",
                               IndustryName = "Leather and allied products",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "331",
                               NaicsCode = "316",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "40000000",
                               IndustryName = "Trade, transportation, and utilities",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "332",
                               NaicsCode = "-",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41420000",
                               IndustryName = "Wholesale trade",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "333",
                               NaicsCode = "42",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423000",
                               IndustryName = "Durable goods",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "334",
                               NaicsCode = "423",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423100",
                               IndustryName = "Motor vehicles and parts",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "335",
                               NaicsCode = "4231",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423110",
                               IndustryName = "Motor vehicles",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "336",
                               NaicsCode = "42311",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423120",
                               IndustryName = "New motor vehicle parts",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "337",
                               NaicsCode = "42312",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423200",
                               IndustryName = "Furniture and furnishings",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "338",
                               NaicsCode = "4232",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423210",
                               IndustryName = "Furniture",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "339",
                               NaicsCode = "42321",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423220",
                               IndustryName = "Home furnishings",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "340",
                               NaicsCode = "42322",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423300",
                               IndustryName = "Lumber and construction supplies",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "341",
                               NaicsCode = "4233",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423310",
                               IndustryName = "Lumber and wood",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "342",
                               NaicsCode = "42331",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423320",
                               IndustryName = "Masonry materials",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "343",
                               NaicsCode = "42332",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423390",
                               IndustryName = "Roofing, siding, and other construction materials",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "344",
                               NaicsCode = "42333,9",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423400",
                               IndustryName = "Commercial equipment",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "345",
                               NaicsCode = "4234",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423420",
                               IndustryName = "Office equipment",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "346",
                               NaicsCode = "42342",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423430",
                               IndustryName = "Computer and software",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "347",
                               NaicsCode = "42343",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423450",
                               IndustryName = "Medical equipment",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "348",
                               NaicsCode = "42345",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423490",
                               IndustryName = "Miscellaneous professional and commercial equipment",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "349",
                               NaicsCode = "42341,4,6,9",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423500",
                               IndustryName = "Metals and minerals",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "350",
                               NaicsCode = "4235",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423600",
                               IndustryName = "Electric goods",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "351",
                               NaicsCode = "4236",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423610",
                               IndustryName = "Electrical equipment and wiring",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "352",
                               NaicsCode = "42361",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423690",
                               IndustryName = "Electric appliances and other electronic parts",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "353",
                               NaicsCode = "42362,9",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423700",
                               IndustryName = "Hardware and plumbing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "354",
                               NaicsCode = "4237",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423710",
                               IndustryName = "Hardware",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "355",
                               NaicsCode = "42371",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423720",
                               IndustryName = "Plumbing equipment",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "356",
                               NaicsCode = "42372",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423740",
                               IndustryName = "HVAC and refrigeration equipment",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "357",
                               NaicsCode = "42373,4",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423800",
                               IndustryName = "Machinery and supplies",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "358",
                               NaicsCode = "4238",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423810",
                               IndustryName = "Construction equipment",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "359",
                               NaicsCode = "42381",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423820",
                               IndustryName = "Farm and garden equipment",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "360",
                               NaicsCode = "42382",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423830",
                               IndustryName = "Industrial machinery",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "361",
                               NaicsCode = "42383",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423840",
                               IndustryName = "Industrial supplies",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "362",
                               NaicsCode = "42384",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423850",
                               IndustryName = "Service establishment equipment",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "363",
                               NaicsCode = "42385",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423860",
                               IndustryName = "Other transportation goods",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "364",
                               NaicsCode = "42386",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423900",
                               IndustryName = "Miscellaneous durable goods",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "365",
                               NaicsCode = "4239",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423910",
                               IndustryName = "Sporting goods",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "366",
                               NaicsCode = "42391",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423930",
                               IndustryName = "Recyclable materials",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "367",
                               NaicsCode = "42393",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423940",
                               IndustryName = "Jewelry",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "368",
                               NaicsCode = "42394",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41423990",
                               IndustryName = "Toy, hobby, and other durable goods",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "369",
                               NaicsCode = "42392,9",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424000",
                               IndustryName = "Nondurable goods",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "370",
                               NaicsCode = "424",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424100",
                               IndustryName = "Paper and paper products",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "371",
                               NaicsCode = "4241",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424120",
                               IndustryName = "Printing and writing paper and office supplies",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "372",
                               NaicsCode = "42411,2",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424130",
                               IndustryName = "Industrial paper",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "373",
                               NaicsCode = "42413",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424200",
                               IndustryName = "Druggists' goods",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "374",
                               NaicsCode = "4242",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424300",
                               IndustryName = "Apparel and piece goods",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "375",
                               NaicsCode = "4243",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424330",
                               IndustryName = "Women's and children's clothing",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "376",
                               NaicsCode = "42433",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424400",
                               IndustryName = "Grocery and related products",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "377",
                               NaicsCode = "4244",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424410",
                               IndustryName = "General line grocery",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "378",
                               NaicsCode = "42441",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424480",
                               IndustryName = "Fruits and vegetables",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "379",
                               NaicsCode = "42448",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424500",
                               IndustryName = "Farm product raw materials",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "380",
                               NaicsCode = "4245",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424510",
                               IndustryName = "Grains and field beans",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "381",
                               NaicsCode = "42451",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424600",
                               IndustryName = "Chemicals",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "382",
                               NaicsCode = "4246",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424700",
                               IndustryName = "Petroleum",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "383",
                               NaicsCode = "4247",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424800",
                               IndustryName = "Alcoholic beverages",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "384",
                               NaicsCode = "4248",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424810",
                               IndustryName = "Beer and ale",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "385",
                               NaicsCode = "42481",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424820",
                               IndustryName = "Wine and spirits",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "386",
                               NaicsCode = "42482",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424900",
                               IndustryName = "Misc. nondurable goods",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "387",
                               NaicsCode = "4249",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424910",
                               IndustryName = "Farm supplies",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "388",
                               NaicsCode = "42491",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424920",
                               IndustryName = "Books and periodicals",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "389",
                               NaicsCode = "42492",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424930",
                               IndustryName = "Nursery stock and florists' supplies",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "390",
                               NaicsCode = "42493",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41424990",
                               IndustryName = "All other nondurable goods wholesalers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "391",
                               NaicsCode = "42494,5,9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41425000",
                               IndustryName = "Electronic markets and agents and brokers",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "392",
                               NaicsCode = "425",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41425110",
                               IndustryName = "Business to business electronic markets",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "393",
                               NaicsCode = "42511",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "41425120",
                               IndustryName = "Wholesale trade agents and brokers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "394",
                               NaicsCode = "42512",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42000000",
                               IndustryName = "Retail trade",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "395",
                               NaicsCode = "-",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42441000",
                               IndustryName = "Motor vehicle and parts dealers",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "396",
                               NaicsCode = "441",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42441100",
                               IndustryName = "Automobile dealers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "397",
                               NaicsCode = "4411",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42441110",
                               IndustryName = "New car dealers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "398",
                               NaicsCode = "44111",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42441120",
                               IndustryName = "Used car dealers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "399",
                               NaicsCode = "44112",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42441200",
                               IndustryName = "Other motor vehicle dealers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "400",
                               NaicsCode = "4412",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42441210",
                               IndustryName = "Recreational vehicle dealers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "401",
                               NaicsCode = "44121",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42441220",
                               IndustryName = "Motorcycle, boat, and other vehicle dealers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "402",
                               NaicsCode = "44122",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42441300",
                               IndustryName = "Auto parts, accessories, and tire stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "403",
                               NaicsCode = "4413",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42441310",
                               IndustryName = "Automotive parts and accessories stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "404",
                               NaicsCode = "44131",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42441320",
                               IndustryName = "Tire dealers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "405",
                               NaicsCode = "44132",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42442000",
                               IndustryName = "Furniture and home furnishings stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "406",
                               NaicsCode = "442",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42442100",
                               IndustryName = "Furniture stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "407",
                               NaicsCode = "4421",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42442200",
                               IndustryName = "Home furnishings stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "408",
                               NaicsCode = "4422",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42442210",
                               IndustryName = "Floor covering stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "409",
                               NaicsCode = "44221",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42442290",
                               IndustryName = "Other home furnishings stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "410",
                               NaicsCode = "44229",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42443000",
                               IndustryName = "Electronics and appliance stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "411",
                               NaicsCode = "443",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42443141",
                               IndustryName = "Household appliance stores",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "412",
                               NaicsCode = "443141",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42443142",
                               IndustryName = "Electronics stores",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "413",
                               NaicsCode = "443142",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42444000",
                               IndustryName = "Building material and garden supply stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "414",
                               NaicsCode = "444",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42444100",
                               IndustryName = "Building material and supplies dealers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "415",
                               NaicsCode = "4441",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42444110",
                               IndustryName = "Home centers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "416",
                               NaicsCode = "44411",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42444120",
                               IndustryName = "Paint and wallpaper stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "417",
                               NaicsCode = "44412",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42444130",
                               IndustryName = "Hardware stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "418",
                               NaicsCode = "44413",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42444190",
                               IndustryName = "Other building material dealers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "419",
                               NaicsCode = "44419",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42444200",
                               IndustryName = "Lawn and garden equipment and supplies stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "420",
                               NaicsCode = "4442",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42444210",
                               IndustryName = "Outdoor power equipment stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "421",
                               NaicsCode = "44421",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42444220",
                               IndustryName = "Nursery, garden, and farm supply stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "422",
                               NaicsCode = "44422",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42445000",
                               IndustryName = "Food and beverage stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "423",
                               NaicsCode = "445",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42445100",
                               IndustryName = "Grocery stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "424",
                               NaicsCode = "4451",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42445110",
                               IndustryName = "Supermarkets and other grocery stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "425",
                               NaicsCode = "44511",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42445120",
                               IndustryName = "Convenience stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "426",
                               NaicsCode = "44512",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42445200",
                               IndustryName = "Specialty food stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "427",
                               NaicsCode = "4452",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42445220",
                               IndustryName = "Meat markets and fish and seafood markets",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "428",
                               NaicsCode = "44521,2",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42445230",
                               IndustryName = "Fruit and vegetable markets",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "429",
                               NaicsCode = "44523",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42445290",
                               IndustryName = "Other specialty food stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "430",
                               NaicsCode = "44529",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42445300",
                               IndustryName = "Beer, wine, and liquor stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "431",
                               NaicsCode = "4453",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42446000",
                               IndustryName = "Health and personal care stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "432",
                               NaicsCode = "446",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42446110",
                               IndustryName = "Pharmacies and drug stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "433",
                               NaicsCode = "44611",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42446120",
                               IndustryName = "Cosmetic and beauty supply stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "434",
                               NaicsCode = "44612",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42446130",
                               IndustryName = "Optical goods stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "435",
                               NaicsCode = "44613",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42446190",
                               IndustryName = "Other health and personal care stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "436",
                               NaicsCode = "44619",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42446191",
                               IndustryName = "Food (health) supplement stores",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "437",
                               NaicsCode = "446191",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42446199",
                               IndustryName = "All other health and personal care stores",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "438",
                               NaicsCode = "446199",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42447000",
                               IndustryName = "Gasoline stations",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "439",
                               NaicsCode = "447",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42447110",
                               IndustryName = "Gasoline stations with convenience stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "440",
                               NaicsCode = "44711",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42447190",
                               IndustryName = "Other gasoline stations",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "441",
                               NaicsCode = "44719",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42448000",
                               IndustryName = "Clothing and clothing accessories stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "442",
                               NaicsCode = "448",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42448100",
                               IndustryName = "Clothing stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "443",
                               NaicsCode = "4481",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42448110",
                               IndustryName = "Men's clothing stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "444",
                               NaicsCode = "44811",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42448120",
                               IndustryName = "Women's clothing stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "445",
                               NaicsCode = "44812",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42448130",
                               IndustryName = "Children's and infants' clothing stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "446",
                               NaicsCode = "44813",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42448140",
                               IndustryName = "Family clothing stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "447",
                               NaicsCode = "44814",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42448150",
                               IndustryName = "Clothing accessories stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "448",
                               NaicsCode = "44815",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42448190",
                               IndustryName = "Other clothing stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "449",
                               NaicsCode = "44819",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42448200",
                               IndustryName = "Shoe stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "450",
                               NaicsCode = "4482",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42448300",
                               IndustryName = "Jewelry, luggage, and leather goods stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "451",
                               NaicsCode = "4483",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42451000",
                               IndustryName = "Sporting goods, hobby, book, and music stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "452",
                               NaicsCode = "451",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42451100",
                               IndustryName = "Sporting goods and musical instrument stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "453",
                               NaicsCode = "4511",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42451110",
                               IndustryName = "Sporting goods stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "454",
                               NaicsCode = "45111",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42451120",
                               IndustryName = "Hobby, toy, and game stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "455",
                               NaicsCode = "45112",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42451130",
                               IndustryName = "Sewing, needlework, and piece goods stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "456",
                               NaicsCode = "45113",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42451140",
                               IndustryName = "Musical instrument and supplies stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "457",
                               NaicsCode = "45114",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42451200",
                               IndustryName = "Book stores and news dealers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "458",
                               NaicsCode = "4512",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42452000",
                               IndustryName = "General merchandise stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "459",
                               NaicsCode = "452",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42452100",
                               IndustryName = "Department stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "460",
                               NaicsCode = "4521",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42452111",
                               IndustryName = "Department stores, except discount",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "461",
                               NaicsCode = "452111",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42452112",
                               IndustryName = "Discount department stores",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "462",
                               NaicsCode = "452112",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42452900",
                               IndustryName = "Other general merchandise stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "463",
                               NaicsCode = "4529",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42452910",
                               IndustryName = "Warehouse clubs and supercenters",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "464",
                               NaicsCode = "45291",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42452990",
                               IndustryName = "All other general merchandise stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "465",
                               NaicsCode = "45299",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42453000",
                               IndustryName = "Miscellaneous store retailers",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "466",
                               NaicsCode = "453",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42453100",
                               IndustryName = "Florists",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "467",
                               NaicsCode = "4531",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42453200",
                               IndustryName = "Office supplies, stationery, and gift stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "468",
                               NaicsCode = "4532",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42453210",
                               IndustryName = "Office supplies and stationery stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "469",
                               NaicsCode = "45321",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42453220",
                               IndustryName = "Gift, novelty, and souvenir stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "470",
                               NaicsCode = "45322",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42453300",
                               IndustryName = "Used merchandise stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "471",
                               NaicsCode = "4533",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42453900",
                               IndustryName = "Other miscellaneous store retailers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "472",
                               NaicsCode = "4539",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42453910",
                               IndustryName = "Pet and pet supplies stores",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "473",
                               NaicsCode = "45391",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42453920",
                               IndustryName = "Art dealers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "474",
                               NaicsCode = "45392",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42453990",
                               IndustryName = "All other miscellaneous store retailers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "475",
                               NaicsCode = "45393,9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42454000",
                               IndustryName = "Nonstore retailers",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "476",
                               NaicsCode = "454",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42454100",
                               IndustryName = "Electronic shopping and mail-order houses",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "477",
                               NaicsCode = "4541",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42454112",
                               IndustryName = "Electronic shopping and electronic auctions",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "478",
                               NaicsCode = "454111,2",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42454113",
                               IndustryName = "Mail-order houses",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "479",
                               NaicsCode = "454113",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42454200",
                               IndustryName = "Vending machine operators",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "480",
                               NaicsCode = "4542",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42454300",
                               IndustryName = "Direct selling establishments",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "481",
                               NaicsCode = "4543",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42454310",
                               IndustryName = "Fuel dealers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "482",
                               NaicsCode = "45431",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "42454390",
                               IndustryName = "Other direct selling establishments",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "483",
                               NaicsCode = "45439",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43000000",
                               IndustryName = "Transportation and warehousing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "484",
                               NaicsCode = "-",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43481000",
                               IndustryName = "Air transportation",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "485",
                               NaicsCode = "481",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43481100",
                               IndustryName = "Scheduled air transportation",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "486",
                               NaicsCode = "4811",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43481200",
                               IndustryName = "Nonscheduled air transportation",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "487",
                               NaicsCode = "4812",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43482000",
                               IndustryName = "Rail transportation",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "488",
                               NaicsCode = "482",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43483000",
                               IndustryName = "Water transportation",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "489",
                               NaicsCode = "483",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43484000",
                               IndustryName = "Truck transportation",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "490",
                               NaicsCode = "484",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43484100",
                               IndustryName = "General freight trucking",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "491",
                               NaicsCode = "4841",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43484110",
                               IndustryName = "General freight trucking, local",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "492",
                               NaicsCode = "48411",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43484120",
                               IndustryName = "General freight trucking, long-distance",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "493",
                               NaicsCode = "48412",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43484121",
                               IndustryName = "General freight trucking, long-distance TL",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "494",
                               NaicsCode = "484121",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43484122",
                               IndustryName = "General freight trucking, long-distance LTL",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "495",
                               NaicsCode = "484122",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43484200",
                               IndustryName = "Specialized freight trucking",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "496",
                               NaicsCode = "4842",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43484210",
                               IndustryName = "Used household and office goods moving",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "497",
                               NaicsCode = "48421",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43484220",
                               IndustryName = "Other specialized trucking, local",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "498",
                               NaicsCode = "48422",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43484230",
                               IndustryName = "Other specialized trucking, long-distance",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "499",
                               NaicsCode = "48423",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43485000",
                               IndustryName = "Transit and ground passenger transportation",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "500",
                               NaicsCode = "485",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43485300",
                               IndustryName = "Taxi and limousine service",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "501",
                               NaicsCode = "4853",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43485310",
                               IndustryName = "Taxi service",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "502",
                               NaicsCode = "48531",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43485320",
                               IndustryName = "Limousine service",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "503",
                               NaicsCode = "48532",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43485400",
                               IndustryName = "School and employee bus transportation",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "504",
                               NaicsCode = "4854",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43485500",
                               IndustryName = "Urban, interurban, rural, and charter bus transportation",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "505",
                               NaicsCode = "4851,2,5",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43485900",
                               IndustryName = "Other ground passenger transportation",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "506",
                               NaicsCode = "4859",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43486000",
                               IndustryName = "Pipeline transportation",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "507",
                               NaicsCode = "486",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43487000",
                               IndustryName = "Scenic and sightseeing transportation",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "508",
                               NaicsCode = "487",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43488000",
                               IndustryName = "Support activities for transportation",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "509",
                               NaicsCode = "488",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43488100",
                               IndustryName = "Support activities for air transportation",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "510",
                               NaicsCode = "4881",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43488110",
                               IndustryName = "Airport operations",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "511",
                               NaicsCode = "48811",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43488300",
                               IndustryName = "Support activities for water transportation",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "512",
                               NaicsCode = "4883",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43488320",
                               IndustryName = "Marine cargo handling",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "513",
                               NaicsCode = "48832",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43488390",
                               IndustryName = "Support activities for water transportation, except marine cargo",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "514",
                               NaicsCode = "48831,3,9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43488400",
                               IndustryName = "Support activities for road transportation",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "515",
                               NaicsCode = "4884",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43488410",
                               IndustryName = "Motor vehicle towing",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "516",
                               NaicsCode = "48841",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43488500",
                               IndustryName = "Freight transportation arrangement",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "517",
                               NaicsCode = "4885",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43488900",
                               IndustryName = "Support activities for other transportation, including rail",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "518",
                               NaicsCode = "4882,9",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43492000",
                               IndustryName = "Couriers and messengers",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "519",
                               NaicsCode = "492",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43492100",
                               IndustryName = "Couriers and express delivery services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "520",
                               NaicsCode = "4921",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43492200",
                               IndustryName = "Local messengers and delivery and private postal service",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "521",
                               NaicsCode = "49111,221",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43493000",
                               IndustryName = "Warehousing and storage",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "522",
                               NaicsCode = "493",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43493110",
                               IndustryName = "General warehousing and storage",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "523",
                               NaicsCode = "49311",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43493120",
                               IndustryName = "Refrigerated warehousing and storage",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "524",
                               NaicsCode = "49312",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "43493190",
                               IndustryName = "Miscellaneous warehousing and storage",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "525",
                               NaicsCode = "49313,9",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "44220000",
                               IndustryName = "Utilities",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "526",
                               NaicsCode = "22",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "44221100",
                               IndustryName = "Power generation and supply",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "527",
                               NaicsCode = "2211",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "44221110",
                               IndustryName = "Electric power generation",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "528",
                               NaicsCode = "22111",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "44221112",
                               IndustryName = "Fossil fuel electric power generation",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "529",
                               NaicsCode = "221112",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "44221118",
                               IndustryName = "Nuclear and other electric power generation",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "530",
                               NaicsCode = "221111,3-8",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "44221120",
                               IndustryName = "Electric power transmission and distribution",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "531",
                               NaicsCode = "22112",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "44221121",
                               IndustryName = "Electric bulk power transmission and control",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "532",
                               NaicsCode = "221121",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "44221122",
                               IndustryName = "Electric power distribution",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "533",
                               NaicsCode = "221122",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "44221200",
                               IndustryName = "Natural gas distribution",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "534",
                               NaicsCode = "2212",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "44221300",
                               IndustryName = "Water, sewage and other systems",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "535",
                               NaicsCode = "2213",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50000000",
                               IndustryName = "Information",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "536",
                               NaicsCode = "51",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50511000",
                               IndustryName = "Publishing industries, except Internet",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "537",
                               NaicsCode = "511",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50511100",
                               IndustryName = "Newspaper, book, and directory publishers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "538",
                               NaicsCode = "5111",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50511110",
                               IndustryName = "Newspaper publishers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "539",
                               NaicsCode = "51111",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50511120",
                               IndustryName = "Periodical publishers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "540",
                               NaicsCode = "51112",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50511130",
                               IndustryName = "Book publishers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "541",
                               NaicsCode = "51113",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50511140",
                               IndustryName = "Directory and mailing list publishers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "542",
                               NaicsCode = "51114",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50511190",
                               IndustryName = "Other publishers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "543",
                               NaicsCode = "51119",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50511200",
                               IndustryName = "Software publishers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "544",
                               NaicsCode = "5112",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50512000",
                               IndustryName = "Motion picture and sound recording industries",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "545",
                               NaicsCode = "512",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50512110",
                               IndustryName = "Motion picture and video production",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "546",
                               NaicsCode = "51211",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50512130",
                               IndustryName = "Motion picture and video exhibition",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "547",
                               NaicsCode = "51213",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50515000",
                               IndustryName = "Broadcasting, except Internet",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "548",
                               NaicsCode = "515",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50515100",
                               IndustryName = "Radio and television broadcasting",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "549",
                               NaicsCode = "5151",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50515110",
                               IndustryName = "Radio broadcasting",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "550",
                               NaicsCode = "51511",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50515120",
                               IndustryName = "Television broadcasting",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "551",
                               NaicsCode = "51512",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50515200",
                               IndustryName = "Cable and other subscription programming",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "552",
                               NaicsCode = "5152",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50517000",
                               IndustryName = "Telecommunications",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "553",
                               NaicsCode = "517",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50517100",
                               IndustryName = "Wired telecommunications carriers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "554",
                               NaicsCode = "5171",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50517200",
                               IndustryName = "Wireless telecommunications carriers (except satellite)",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "555",
                               NaicsCode = "5172",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50517900",
                               IndustryName = "Other telecommunications",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "556",
                               NaicsCode = "5174,9",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50517911",
                               IndustryName = "Telecommunications resellers",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "557",
                               NaicsCode = "517911",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50518000",
                               IndustryName = "Data processing, hosting and related services",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "558",
                               NaicsCode = "518",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50519000",
                               IndustryName = "Other information services",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "559",
                               NaicsCode = "519",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50519130",
                               IndustryName = "Internet publishing and broadcasting and web search portals",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "560",
                               NaicsCode = "51913",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "50519190",
                               IndustryName = "All other information services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "561",
                               NaicsCode = "51911,2,9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55000000",
                               IndustryName = "Financial activities",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "562",
                               NaicsCode = "-",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55520000",
                               IndustryName = "Finance and insurance",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "563",
                               NaicsCode = "52",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55521000",
                               IndustryName = "Monetary authorities - central bank",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "564",
                               NaicsCode = "521",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55522000",
                               IndustryName = "Credit intermediation and related activities",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "565",
                               NaicsCode = "522",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55522100",
                               IndustryName = "Depository credit intermediation",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "566",
                               NaicsCode = "5221",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55522110",
                               IndustryName = "Commercial banking",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "567",
                               NaicsCode = "52211",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55522120",
                               IndustryName = "Savings institutions",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "568",
                               NaicsCode = "52212",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55522190",
                               IndustryName = "Credit unions and other depository credit intermediation",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "569",
                               NaicsCode = "52213,9",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55522200",
                               IndustryName = "Nondepository credit intermediation",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "570",
                               NaicsCode = "5222",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55522210",
                               IndustryName = "Credit card issuing",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "571",
                               NaicsCode = "52221",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55522220",
                               IndustryName = "Sales financing",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "572",
                               NaicsCode = "52222",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55522290",
                               IndustryName = "Other nondepository credit intermediation",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "573",
                               NaicsCode = "52229",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55522291",
                               IndustryName = "Consumer lending",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "574",
                               NaicsCode = "522291",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55522292",
                               IndustryName = "Real estate credit",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "575",
                               NaicsCode = "522292",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55522298",
                               IndustryName = "Miscellaneous nondepository credit intermediation",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "576",
                               NaicsCode = "522293,4,8",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55522300",
                               IndustryName = "Activities related to credit intermediation",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "577",
                               NaicsCode = "5223",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55522310",
                               IndustryName = "Mortgage and nonmortgage loan brokers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "578",
                               NaicsCode = "52231",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55522320",
                               IndustryName = "Financial transaction processing and clearing",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "579",
                               NaicsCode = "52232",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55522390",
                               IndustryName = "Other credit intermediation activities",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "580",
                               NaicsCode = "52239",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55523000",
                               IndustryName = "Securities, commodity contracts, investments, and funds and trusts",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "581",
                               NaicsCode = "523,5",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55523120",
                               IndustryName = "Securities brokerage",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "582",
                               NaicsCode = "52312",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55523200",
                               IndustryName = "Securities and commodity contracts brokerage and exchanges",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "583",
                               NaicsCode = "5231,2",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55523900",
                               IndustryName = "Other financial investment activities, including funds and trusts",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "584",
                               NaicsCode = "5239; 525",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55523910",
                               IndustryName = "Miscellaneous intermediation",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "585",
                               NaicsCode = "52391",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55523920",
                               IndustryName = "Portfolio management",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "586",
                               NaicsCode = "52392",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55523930",
                               IndustryName = "Investment advice",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "587",
                               NaicsCode = "52393",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55523990",
                               IndustryName = "All other financial investment activities, including funds and trusts",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "588",
                               NaicsCode = "52399; 525",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55524000",
                               IndustryName = "Insurance carriers and related activities",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "589",
                               NaicsCode = "524",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55524100",
                               IndustryName = "Insurance carriers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "590",
                               NaicsCode = "5241",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55524110",
                               IndustryName = "Direct life and health insurance carriers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "591",
                               NaicsCode = "52411",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55524113",
                               IndustryName = "Direct life insurance carriers",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "592",
                               NaicsCode = "524113",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55524114",
                               IndustryName = "Direct health and medical insurance carriers",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "593",
                               NaicsCode = "524114",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55524120",
                               IndustryName = "Direct insurers, except life and health",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "594",
                               NaicsCode = "52412",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55524126",
                               IndustryName = "Direct property and casualty insurers",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "595",
                               NaicsCode = "524126",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55524128",
                               IndustryName = "Direct title insurance and other direct insurance carriers",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "596",
                               NaicsCode = "524127,8",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55524130",
                               IndustryName = "Reinsurance carriers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "597",
                               NaicsCode = "52413",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55524200",
                               IndustryName = "Insurance agencies, brokerages, and related services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "598",
                               NaicsCode = "5242",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55524210",
                               IndustryName = "Insurance agencies and brokerages",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "599",
                               NaicsCode = "52421",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55524290",
                               IndustryName = "Other insurance-related activities",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "600",
                               NaicsCode = "52429",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55524291",
                               IndustryName = "Claims adjusting",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "601",
                               NaicsCode = "524291",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55524292",
                               IndustryName = "Third-party administration of insurance funds",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "602",
                               NaicsCode = "524292",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55524298",
                               IndustryName = "All other insurance-related activities",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "603",
                               NaicsCode = "524298",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55530000",
                               IndustryName = "Real estate and rental and leasing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "604",
                               NaicsCode = "53",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55531000",
                               IndustryName = "Real estate",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "605",
                               NaicsCode = "531",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55531100",
                               IndustryName = "Lessors of real estate",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "606",
                               NaicsCode = "5311",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55531110",
                               IndustryName = "Lessors of residential buildings",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "607",
                               NaicsCode = "53111",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55531120",
                               IndustryName = "Lessors of nonresidential buildings",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "608",
                               NaicsCode = "53112",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55531130",
                               IndustryName = "Miniwarehouse and self-storage unit operators",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "609",
                               NaicsCode = "53113",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55531190",
                               IndustryName = "Lessors of other real estate property",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "610",
                               NaicsCode = "53119",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55531200",
                               IndustryName = "Offices of real estate agents and brokers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "611",
                               NaicsCode = "5312",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55531300",
                               IndustryName = "Activities related to real estate",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "612",
                               NaicsCode = "5313",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55531310",
                               IndustryName = "Real estate property managers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "613",
                               NaicsCode = "53131",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55531311",
                               IndustryName = "Residential property managers",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "614",
                               NaicsCode = "531311",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55531312",
                               IndustryName = "Nonresidential property managers",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "615",
                               NaicsCode = "531312",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55531320",
                               IndustryName = "Offices of real estate appraisers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "616",
                               NaicsCode = "53132",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55531390",
                               IndustryName = "Other activities related to real estate",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "617",
                               NaicsCode = "53139",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55532000",
                               IndustryName = "Rental and leasing services",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "618",
                               NaicsCode = "532",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55532100",
                               IndustryName = "Automotive equipment rental and leasing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "619",
                               NaicsCode = "5321",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55532110",
                               IndustryName = "Passenger car rental and leasing",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "620",
                               NaicsCode = "53211",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55532120",
                               IndustryName = "Truck, trailer, and RV rental and leasing",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "621",
                               NaicsCode = "53212",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55532200",
                               IndustryName = "Consumer goods rental",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "622",
                               NaicsCode = "5322",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55532230",
                               IndustryName = "Video tape and disc rental",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "623",
                               NaicsCode = "53223",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55532290",
                               IndustryName = "Miscellaneous consumer goods rental",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "624",
                               NaicsCode = "53221,2,9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55532291",
                               IndustryName = "Home health equipment rental",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "625",
                               NaicsCode = "532291",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55532300",
                               IndustryName = "General rental centers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "626",
                               NaicsCode = "5323",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55532400",
                               IndustryName = "Machinery and equipment rental and leasing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "627",
                               NaicsCode = "5324",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55532410",
                               IndustryName = "Heavy machinery rental and leasing",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "628",
                               NaicsCode = "53241",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55532490",
                               IndustryName = "Office equipment and other machinery rental and leasing",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "629",
                               NaicsCode = "53242,9",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "55533000",
                               IndustryName = "Lessors of nonfinancial intangible assets",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "630",
                               NaicsCode = "533",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60000000",
                               IndustryName = "Professional and business services",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "631",
                               NaicsCode = "-",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60540000",
                               IndustryName = "Professional and technical services",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "632",
                               NaicsCode = "54",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541100",
                               IndustryName = "Legal services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "633",
                               NaicsCode = "5411",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541110",
                               IndustryName = "Offices of lawyers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "634",
                               NaicsCode = "54111",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541190",
                               IndustryName = "Other legal services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "635",
                               NaicsCode = "54119",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541191",
                               IndustryName = "Title abstract and settlement offices",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "636",
                               NaicsCode = "541191",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541200",
                               IndustryName = "Accounting and bookkeeping services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "637",
                               NaicsCode = "5412",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541211",
                               IndustryName = "Offices of certified public accountants",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "638",
                               NaicsCode = "541211",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541213",
                               IndustryName = "Tax preparation services",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "639",
                               NaicsCode = "541213",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541214",
                               IndustryName = "Payroll services",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "640",
                               NaicsCode = "541214",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541219",
                               IndustryName = "Other accounting services",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "641",
                               NaicsCode = "541219",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541300",
                               IndustryName = "Architectural and engineering services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "642",
                               NaicsCode = "5413",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541310",
                               IndustryName = "Architectural services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "643",
                               NaicsCode = "54131",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541320",
                               IndustryName = "Landscape architectural services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "644",
                               NaicsCode = "54132",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541340",
                               IndustryName = "Engineering and drafting services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "645",
                               NaicsCode = "54133,4",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541370",
                               IndustryName = "Building inspection, surveying, and mapping services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "646",
                               NaicsCode = "54135,6,7",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541380",
                               IndustryName = "Testing laboratories",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "647",
                               NaicsCode = "54138",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541400",
                               IndustryName = "Specialized design services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "648",
                               NaicsCode = "5414",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541410",
                               IndustryName = "Interior design services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "649",
                               NaicsCode = "54141",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541430",
                               IndustryName = "Graphic design services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "650",
                               NaicsCode = "54143",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541500",
                               IndustryName = "Computer systems design and related services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "651",
                               NaicsCode = "5415",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541511",
                               IndustryName = "Custom computer programming services",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "652",
                               NaicsCode = "541511",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541512",
                               IndustryName = "Computer systems design services",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "653",
                               NaicsCode = "541512",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541513",
                               IndustryName = "Computer facilities management services",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "654",
                               NaicsCode = "541513",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541519",
                               IndustryName = "Other computer-related services",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "655",
                               NaicsCode = "541519",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541600",
                               IndustryName = "Management and technical consulting services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "656",
                               NaicsCode = "5416",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541610",
                               IndustryName = "Management consulting services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "657",
                               NaicsCode = "54161",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541611",
                               IndustryName = "Administrative management consulting services",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "658",
                               NaicsCode = "541611",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541612",
                               IndustryName = "Human resource consulting services",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "659",
                               NaicsCode = "541612",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541613",
                               IndustryName = "Marketing consulting services",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "660",
                               NaicsCode = "541613",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541614",
                               IndustryName = "Process and logistics consulting services",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "661",
                               NaicsCode = "541614",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541618",
                               IndustryName = "Other management consulting services",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "662",
                               NaicsCode = "541618",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541620",
                               IndustryName = "Environmental consulting services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "663",
                               NaicsCode = "54162",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541690",
                               IndustryName = "Other technical consulting services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "664",
                               NaicsCode = "54169",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541700",
                               IndustryName = "Scientific research and development services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "665",
                               NaicsCode = "5417",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541710",
                               IndustryName = "Research and development in the physical, engineering, and life sciences",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "666",
                               NaicsCode = "54171",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541711",
                               IndustryName = "Biotechnology research",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "667",
                               NaicsCode = "541711",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541712",
                               IndustryName = "Physical, engineering, and life sciences research",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "668",
                               NaicsCode = "541712",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541720",
                               IndustryName = "Social science and humanities research",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "669",
                               NaicsCode = "54172",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541800",
                               IndustryName = "Advertising and related services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "670",
                               NaicsCode = "5418",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541810",
                               IndustryName = "Advertising agencies",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "671",
                               NaicsCode = "54181",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541820",
                               IndustryName = "Public relations agencies",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "672",
                               NaicsCode = "54182",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541840",
                               IndustryName = "Media buying agencies and media representatives",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "673",
                               NaicsCode = "54183,4",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541850",
                               IndustryName = "Display advertising",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "674",
                               NaicsCode = "54185",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541860",
                               IndustryName = "Direct mail advertising",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "675",
                               NaicsCode = "54186",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541890",
                               IndustryName = "Advertising material distribution and other advertising services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "676",
                               NaicsCode = "54187,9",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541900",
                               IndustryName = "Other professional and technical services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "677",
                               NaicsCode = "5419",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541910",
                               IndustryName = "Marketing research and public opinion polling",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "678",
                               NaicsCode = "54191",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541920",
                               IndustryName = "Photographic services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "679",
                               NaicsCode = "54192",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541940",
                               IndustryName = "Veterinary services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "680",
                               NaicsCode = "54194",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60541990",
                               IndustryName = "Miscellaneous professional and technical services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "681",
                               NaicsCode = "54193,9",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60550000",
                               IndustryName = "Management of companies and enterprises",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "682",
                               NaicsCode = "55",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60551112",
                               IndustryName = "Offices of bank holding companies and of other holding companies",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "683",
                               NaicsCode = "551111,2",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60551114",
                               IndustryName = "Managing offices",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "684",
                               NaicsCode = "551114",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60560000",
                               IndustryName = "Administrative and waste services",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "685",
                               NaicsCode = "56",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561000",
                               IndustryName = "Administrative and support services",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "686",
                               NaicsCode = "561",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561100",
                               IndustryName = "Office administrative services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "687",
                               NaicsCode = "5611",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561200",
                               IndustryName = "Facilities support services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "688",
                               NaicsCode = "5612",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561300",
                               IndustryName = "Employment services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "689",
                               NaicsCode = "5613",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561310",
                               IndustryName = "Employment placement agencies and executive search services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "690",
                               NaicsCode = "56131",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561311",
                               IndustryName = "Employment placement agencies",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "691",
                               NaicsCode = "561311",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561312",
                               IndustryName = "Executive search services",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "692",
                               NaicsCode = "561312",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561320",
                               IndustryName = "Temporary help services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "693",
                               NaicsCode = "56132",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561330",
                               IndustryName = "Professional employer organizations",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "694",
                               NaicsCode = "56133",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561400",
                               IndustryName = "Business support services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "695",
                               NaicsCode = "5614",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561410",
                               IndustryName = "Document preparation services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "696",
                               NaicsCode = "56141",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561420",
                               IndustryName = "Telephone call centers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "697",
                               NaicsCode = "56142",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561421",
                               IndustryName = "Telephone answering services",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "698",
                               NaicsCode = "561421",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561422",
                               IndustryName = "Telemarketing bureaus",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "699",
                               NaicsCode = "561422",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561430",
                               IndustryName = "Business service centers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "700",
                               NaicsCode = "56143",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561440",
                               IndustryName = "Collection agencies",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "701",
                               NaicsCode = "56144",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561450",
                               IndustryName = "Credit bureaus",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "702",
                               NaicsCode = "56145",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561490",
                               IndustryName = "Other business support services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "703",
                               NaicsCode = "56149",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561500",
                               IndustryName = "Travel arrangement and reservation services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "704",
                               NaicsCode = "5615",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561510",
                               IndustryName = "Travel agencies",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "705",
                               NaicsCode = "56151",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561520",
                               IndustryName = "Tour operators",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "706",
                               NaicsCode = "56152",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561590",
                               IndustryName = "Other travel arrangement services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "707",
                               NaicsCode = "56159",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561600",
                               IndustryName = "Investigation and security services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "708",
                               NaicsCode = "5616",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561610",
                               IndustryName = "Security and armored car services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "709",
                               NaicsCode = "56161",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561611",
                               IndustryName = "Investigation services",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "710",
                               NaicsCode = "561611",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561613",
                               IndustryName = "Security guards and patrols and armored car services",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "711",
                               NaicsCode = "561612,3",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561620",
                               IndustryName = "Security systems services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "712",
                               NaicsCode = "56162",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561700",
                               IndustryName = "Services to buildings and dwellings",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "713",
                               NaicsCode = "5617",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561710",
                               IndustryName = "Exterminating and pest control services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "714",
                               NaicsCode = "56171",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561720",
                               IndustryName = "Janitorial services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "715",
                               NaicsCode = "56172",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561730",
                               IndustryName = "Landscaping services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "716",
                               NaicsCode = "56173",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561740",
                               IndustryName = "Carpet and upholstery cleaning services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "717",
                               NaicsCode = "56174",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561790",
                               IndustryName = "Other services to buildings and dwellings",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "718",
                               NaicsCode = "56179",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561900",
                               IndustryName = "Other support services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "719",
                               NaicsCode = "5619",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561910",
                               IndustryName = "Packaging and labeling services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "720",
                               NaicsCode = "56191",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561920",
                               IndustryName = "Convention and trade show organizers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "721",
                               NaicsCode = "56192",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60561990",
                               IndustryName = "All other support services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "722",
                               NaicsCode = "56199",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60562000",
                               IndustryName = "Waste management and remediation services",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "723",
                               NaicsCode = "562",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60562100",
                               IndustryName = "Waste collection",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "724",
                               NaicsCode = "5621",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60562200",
                               IndustryName = "Waste treatment and disposal",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "725",
                               NaicsCode = "5622",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60562211",
                               IndustryName = "Hazardous waste treatment and disposal",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "726",
                               NaicsCode = "562211",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60562219",
                               IndustryName = "Nonhazardous waste treatment and disposal",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "727",
                               NaicsCode = "562212,3,9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60562900",
                               IndustryName = "Remediation and other waste services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "728",
                               NaicsCode = "5629",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60562910",
                               IndustryName = "Remediation services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "729",
                               NaicsCode = "56291",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "60562990",
                               IndustryName = "Materials recovery facilities and other waste management services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "730",
                               NaicsCode = "56292,9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65000000",
                               IndustryName = "Education and health services",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "731",
                               NaicsCode = "-",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65610000",
                               IndustryName = "Educational services",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "732",
                               NaicsCode = "61",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65611100",
                               IndustryName = "Elementary and secondary schools",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "733",
                               NaicsCode = "6111",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65611200",
                               IndustryName = "Junior colleges",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "734",
                               NaicsCode = "6112",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65611300",
                               IndustryName = "Colleges and universities",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "735",
                               NaicsCode = "6113",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65611400",
                               IndustryName = "Business, computer, and management training",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "736",
                               NaicsCode = "6114",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65611420",
                               IndustryName = "Business and secretarial schools and computer training",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "737",
                               NaicsCode = "61141,2",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65611430",
                               IndustryName = "Management training",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "738",
                               NaicsCode = "61143",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65611500",
                               IndustryName = "Technical and trade schools",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "739",
                               NaicsCode = "6115",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65611600",
                               IndustryName = "Other schools and instruction",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "740",
                               NaicsCode = "6116",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65611610",
                               IndustryName = "Fine arts schools",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "741",
                               NaicsCode = "61161",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65611620",
                               IndustryName = "Sports and recreation instruction",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "742",
                               NaicsCode = "61162",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65611690",
                               IndustryName = "Miscellaneous schools and instruction",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "743",
                               NaicsCode = "61163,9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65611700",
                               IndustryName = "Educational support services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "744",
                               NaicsCode = "6117",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65620000",
                               IndustryName = "Health care and social assistance",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "745",
                               NaicsCode = "62",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65620001",
                               IndustryName = "Health care",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "746",
                               NaicsCode = "621,2,3",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621000",
                               IndustryName = "Ambulatory health care services",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "747",
                               NaicsCode = "621",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621100",
                               IndustryName = "Offices of physicians",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "748",
                               NaicsCode = "6211",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621111",
                               IndustryName = "Offices of physicians, except mental health",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "749",
                               NaicsCode = "621111",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621112",
                               IndustryName = "Offices of mental health physicians",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "750",
                               NaicsCode = "621112",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621200",
                               IndustryName = "Offices of dentists",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "751",
                               NaicsCode = "6212",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621300",
                               IndustryName = "Offices of other health practitioners",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "752",
                               NaicsCode = "6213",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621310",
                               IndustryName = "Offices of chiropractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "753",
                               NaicsCode = "62131",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621320",
                               IndustryName = "Offices of optometrists",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "754",
                               NaicsCode = "62132",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621330",
                               IndustryName = "Offices of mental health practitioners",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "755",
                               NaicsCode = "62133",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621340",
                               IndustryName = "Offices of specialty therapists",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "756",
                               NaicsCode = "62134",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621390",
                               IndustryName = "Offices of all other health practitioners",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "757",
                               NaicsCode = "62139",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621391",
                               IndustryName = "Offices of podiatrists",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "758",
                               NaicsCode = "621391",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621399",
                               IndustryName = "Offices of miscellaneous health practitioners",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "759",
                               NaicsCode = "621399",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621400",
                               IndustryName = "Outpatient care centers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "760",
                               NaicsCode = "6214",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621420",
                               IndustryName = "Outpatient mental health centers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "761",
                               NaicsCode = "62142",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621490",
                               IndustryName = "Outpatient care centers, except mental health",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "762",
                               NaicsCode = "62141,9",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621491",
                               IndustryName = "HMO medical centers",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "763",
                               NaicsCode = "621491",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621492",
                               IndustryName = "Kidney dialysis centers",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "764",
                               NaicsCode = "621492",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621493",
                               IndustryName = "Freestanding emergency medical centers",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "765",
                               NaicsCode = "621493",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621498",
                               IndustryName = "Miscellaneous outpatient care centers",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "766",
                               NaicsCode = "621410,98",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621500",
                               IndustryName = "Medical and diagnostic laboratories",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "767",
                               NaicsCode = "6215",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621511",
                               IndustryName = "Medical laboratories",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "768",
                               NaicsCode = "621511",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621512",
                               IndustryName = "Diagnostic imaging centers",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "769",
                               NaicsCode = "621512",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621600",
                               IndustryName = "Home health care services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "770",
                               NaicsCode = "6216",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621900",
                               IndustryName = "Other ambulatory health care services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "771",
                               NaicsCode = "6219",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621910",
                               IndustryName = "Ambulance services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "772",
                               NaicsCode = "62191",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621990",
                               IndustryName = "All other ambulatory health care services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "773",
                               NaicsCode = "62199",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621991",
                               IndustryName = "Blood and organ banks",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "774",
                               NaicsCode = "621991",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65621999",
                               IndustryName = "Miscellaneous ambulatory health care services",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "775",
                               NaicsCode = "621999",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65622000",
                               IndustryName = "Hospitals",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "776",
                               NaicsCode = "622",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65622100",
                               IndustryName = "General medical and surgical hospitals",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "777",
                               NaicsCode = "6221",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65622200",
                               IndustryName = "Psychiatric and substance abuse hospitals",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "778",
                               NaicsCode = "6222",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65622300",
                               IndustryName = "Other hospitals",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "779",
                               NaicsCode = "6223",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65623000",
                               IndustryName = "Nursing and residential care facilities",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "780",
                               NaicsCode = "623",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65623100",
                               IndustryName = "Nursing care facilities",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "781",
                               NaicsCode = "6231",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65623200",
                               IndustryName = "Residential mental health facilities",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "782",
                               NaicsCode = "6232",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65623210",
                               IndustryName = "Residential intellectual and developmental disability facilities",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "783",
                               NaicsCode = "62321",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65623220",
                               IndustryName = "Residential mental health and substance abuse facilites",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "784",
                               NaicsCode = "62322",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65623300",
                               IndustryName = "Community care facilities for the elderly",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "785",
                               NaicsCode = "6233",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65623311",
                               IndustryName = "Continuing care retirement communities",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "786",
                               NaicsCode = "623311",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65623312",
                               IndustryName = "Assisted living facilities for the elderly",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "787",
                               NaicsCode = "623312",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65623900",
                               IndustryName = "Other residential care facilities",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "788",
                               NaicsCode = "6239",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65624000",
                               IndustryName = "Social assistance",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "789",
                               NaicsCode = "624",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65624100",
                               IndustryName = "Individual and family services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "790",
                               NaicsCode = "6241",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65624110",
                               IndustryName = "Child and youth services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "791",
                               NaicsCode = "62411",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65624120",
                               IndustryName = "Services for the elderly and persons with disabilities",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "792",
                               NaicsCode = "62412",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65624190",
                               IndustryName = "Other individual and family services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "793",
                               NaicsCode = "62419",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65624200",
                               IndustryName = "Emergency and other relief services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "794",
                               NaicsCode = "6242",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65624210",
                               IndustryName = "Community food services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "795",
                               NaicsCode = "62421",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65624230",
                               IndustryName = "Community housing, emergency, and relief services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "796",
                               NaicsCode = "62422,3",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65624300",
                               IndustryName = "Vocational rehabilitation services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "797",
                               NaicsCode = "6243",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "65624400",
                               IndustryName = "Child day care services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "798",
                               NaicsCode = "6244",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70000000",
                               IndustryName = "Leisure and hospitality",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "799",
                               NaicsCode = "-",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70710000",
                               IndustryName = "Arts, entertainment, and recreation",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "800",
                               NaicsCode = "71",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70711000",
                               IndustryName = "Performing arts and spectator sports",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "801",
                               NaicsCode = "711",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70711100",
                               IndustryName = "Performing arts companies",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "802",
                               NaicsCode = "7111",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70711130",
                               IndustryName = "Musical groups and artists",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "803",
                               NaicsCode = "71113",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70711190",
                               IndustryName = "Theater, dance, and other performing arts companies",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "804",
                               NaicsCode = "71111,2,9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70711200",
                               IndustryName = "Spectator sports",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "805",
                               NaicsCode = "7112",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70711211",
                               IndustryName = "Sports teams and clubs",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "806",
                               NaicsCode = "711211",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70711212",
                               IndustryName = "Racetracks",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "807",
                               NaicsCode = "711212",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70711219",
                               IndustryName = "Other spectator sports",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "808",
                               NaicsCode = "711219",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70711300",
                               IndustryName = "Arts and sports promoters and agents and managers for public figures",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "809",
                               NaicsCode = "7113,4",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70711500",
                               IndustryName = "Independent artists, writers, and performers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "810",
                               NaicsCode = "7115",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70712000",
                               IndustryName = "Museums, historical sites, and similar institutions",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "811",
                               NaicsCode = "712",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70712110",
                               IndustryName = "Museums",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "812",
                               NaicsCode = "71211",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70712190",
                               IndustryName = "Historical sites and other similar institutions",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "813",
                               NaicsCode = "71212,3,9",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70713000",
                               IndustryName = "Amusements, gambling, and recreation",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "814",
                               NaicsCode = "713",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70713100",
                               IndustryName = "Amusement parks and arcades",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "815",
                               NaicsCode = "7131",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70713200",
                               IndustryName = "Gambling industries",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "816",
                               NaicsCode = "7132",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70713210",
                               IndustryName = "Casinos, except casino hotels",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "817",
                               NaicsCode = "71321",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70713290",
                               IndustryName = "Other gambling industries",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "818",
                               NaicsCode = "71329",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70713900",
                               IndustryName = "Other amusement and recreation industries",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "819",
                               NaicsCode = "7139",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70713910",
                               IndustryName = "Golf courses and country clubs",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "820",
                               NaicsCode = "71391",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70713920",
                               IndustryName = "Skiing facilities",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "821",
                               NaicsCode = "71392",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70713930",
                               IndustryName = "Marinas",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "822",
                               NaicsCode = "71393",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70713940",
                               IndustryName = "Fitness and recreational sports centers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "823",
                               NaicsCode = "71394",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70713950",
                               IndustryName = "Bowling centers",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "824",
                               NaicsCode = "71395",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70713990",
                               IndustryName = "All other amusement and recreation industries",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "825",
                               NaicsCode = "71399",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70720000",
                               IndustryName = "Accommodation and food services",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "826",
                               NaicsCode = "72",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70721000",
                               IndustryName = "Accommodation",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "827",
                               NaicsCode = "721",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70721100",
                               IndustryName = "Traveler accommodation",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "828",
                               NaicsCode = "7211",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70721110",
                               IndustryName = "Hotels and motels, except casino hotels",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "829",
                               NaicsCode = "72111",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70721120",
                               IndustryName = "Casino hotels",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "830",
                               NaicsCode = "72112",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70721190",
                               IndustryName = "Other traveler accommodation",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "831",
                               NaicsCode = "72119",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70721191",
                               IndustryName = "Bed-and-breakfast inns",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "832",
                               NaicsCode = "721191",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70721199",
                               IndustryName = "All other traveler accommodation and rooming and boarding houses",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "833",
                               NaicsCode = "721310,199",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70721200",
                               IndustryName = "RV parks and recreational camps",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "834",
                               NaicsCode = "7212",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70721211",
                               IndustryName = "RV parks and campgrounds",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "835",
                               NaicsCode = "721211",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70721214",
                               IndustryName = "Recreational and vacation camps",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "836",
                               NaicsCode = "721214",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70722000",
                               IndustryName = "Food services and drinking places",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "837",
                               NaicsCode = "722",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70722300",
                               IndustryName = "Special food services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "838",
                               NaicsCode = "7223",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70722310",
                               IndustryName = "Food service contractors",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "839",
                               NaicsCode = "72231",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70722330",
                               IndustryName = "Caterers and mobile food services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "840",
                               NaicsCode = "72232,3",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70722400",
                               IndustryName = "Drinking places, alcoholic beverages",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "841",
                               NaicsCode = "7224",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70722500",
                               IndustryName = "Restaurants and other eating places",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "842",
                               NaicsCode = "7225",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70722511",
                               IndustryName = "Full-service restaurants",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "843",
                               NaicsCode = "722511",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70722513",
                               IndustryName = "Limited-service restaurants",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "844",
                               NaicsCode = "722513",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70722514",
                               IndustryName = "Cafeterias, grill buffets, and buffets",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "845",
                               NaicsCode = "722514",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "70722515",
                               IndustryName = "Snack and nonalcoholic beverage bars",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "846",
                               NaicsCode = "722515",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80000000",
                               IndustryName = "Other services",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "847",
                               NaicsCode = "81",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80811000",
                               IndustryName = "Repair and maintenance",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "848",
                               NaicsCode = "811",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80811100",
                               IndustryName = "Automotive repair and maintenance",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "849",
                               NaicsCode = "8111",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80811110",
                               IndustryName = "Automotive mechanical and electrical repair",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "850",
                               NaicsCode = "81111",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80811111",
                               IndustryName = "General automotive repair",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "851",
                               NaicsCode = "811111",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80811113",
                               IndustryName = "Automotive exhaust system and transmission repair",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "852",
                               NaicsCode = "811112,3",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80811118",
                               IndustryName = "Other automotive mechanical and elec. repair",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "853",
                               NaicsCode = "811118",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80811120",
                               IndustryName = "Automotive body, interior, and glass repair",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "854",
                               NaicsCode = "81112",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80811121",
                               IndustryName = "Automotive body and interior repair",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "855",
                               NaicsCode = "811121",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80811122",
                               IndustryName = "Automotive glass replacement shops",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "856",
                               NaicsCode = "811122",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80811190",
                               IndustryName = "Other automotive repair and maintenance",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "857",
                               NaicsCode = "81119",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80811192",
                               IndustryName = "Car washes",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "858",
                               NaicsCode = "811192",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80811198",
                               IndustryName = "Auto oil change shops and all other auto repair and maintenance",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "859",
                               NaicsCode = "811191,8",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80811200",
                               IndustryName = "Electronic equipment repair and maintenance",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "860",
                               NaicsCode = "8112",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80811212",
                               IndustryName = "Computer and office machine repair",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "861",
                               NaicsCode = "811212",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80811219",
                               IndustryName = "Miscellaneous electronic equipment repair and maintenance",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "862",
                               NaicsCode = "811211,3,9",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80811300",
                               IndustryName = "Commercial machinery repair and maintenance",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "863",
                               NaicsCode = "8113",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80811400",
                               IndustryName = "Household goods repair and maintenance",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "864",
                               NaicsCode = "8114",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80812000",
                               IndustryName = "Personal and laundry services",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "865",
                               NaicsCode = "812",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80812100",
                               IndustryName = "Personal care services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "866",
                               NaicsCode = "8121",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80812110",
                               IndustryName = "Hair, nail, and skin care services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "867",
                               NaicsCode = "81211",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80812112",
                               IndustryName = "Barber shops and beauty salons",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "868",
                               NaicsCode = "812111,2",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80812113",
                               IndustryName = "Nail salons",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "869",
                               NaicsCode = "812113",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80812190",
                               IndustryName = "Other personal care services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "870",
                               NaicsCode = "81219",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80812200",
                               IndustryName = "Death care services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "871",
                               NaicsCode = "8122",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80812210",
                               IndustryName = "Funeral homes and funeral services",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "872",
                               NaicsCode = "81221",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80812220",
                               IndustryName = "Cemeteries and crematories",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "873",
                               NaicsCode = "81222",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80812300",
                               IndustryName = "Drycleaning and laundry services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "874",
                               NaicsCode = "8123",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80812310",
                               IndustryName = "Coin-operated laundries and drycleaners",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "875",
                               NaicsCode = "81231",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80812320",
                               IndustryName = "Drycleaning and laundry services, except coin-operated",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "876",
                               NaicsCode = "81232",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80812330",
                               IndustryName = "Linen and uniform supply",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "877",
                               NaicsCode = "81233",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80812331",
                               IndustryName = "Linen supply",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "878",
                               NaicsCode = "812331",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80812332",
                               IndustryName = "Industrial launderers",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "879",
                               NaicsCode = "812332",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80812900",
                               IndustryName = "Other personal services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "880",
                               NaicsCode = "8129",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80812910",
                               IndustryName = "Pet care services, except veterinary",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "881",
                               NaicsCode = "81291",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80812930",
                               IndustryName = "Parking lots and garages",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "882",
                               NaicsCode = "81293",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80812990",
                               IndustryName = "All other personal services, including photofinishing",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "883",
                               NaicsCode = "81292,9",
                               PublishingStatus = "C",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80813000",
                               IndustryName = "Membership associations and organizations",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "884",
                               NaicsCode = "813",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80813200",
                               IndustryName = "Grantmaking and giving services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "885",
                               NaicsCode = "8132",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80813211",
                               IndustryName = "Grantmaking foundations",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "886",
                               NaicsCode = "813211",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80813212",
                               IndustryName = "Voluntary health organizations",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "887",
                               NaicsCode = "813212",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80813219",
                               IndustryName = "Other grantmaking and giving services",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "888",
                               NaicsCode = "813219",
                               PublishingStatus = "E",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80813300",
                               IndustryName = "Social advocacy organizations",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "889",
                               NaicsCode = "8133",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80813311",
                               IndustryName = "Human rights organizations",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "890",
                               NaicsCode = "813311",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80813319",
                               IndustryName = "Environment, conservation, and other social advocacy organizations",
                               Selectable = "T",
                               DisplayLevel = "7",
                               SortSequence = "891",
                               NaicsCode = "813312,9",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80813400",
                               IndustryName = "Civic and social organizations",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "892",
                               NaicsCode = "8134",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80813900",
                               IndustryName = "Professional and similar organizations",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "893",
                               NaicsCode = "8139",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80813910",
                               IndustryName = "Business associations",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "894",
                               NaicsCode = "81391",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80813920",
                               IndustryName = "Professional organizations",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "895",
                               NaicsCode = "81392",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80813930",
                               IndustryName = "Labor unions and similar labor organizations",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "896",
                               NaicsCode = "81393",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "80813990",
                               IndustryName = "Miscellaneous professional and similar organizations",
                               Selectable = "T",
                               DisplayLevel = "6",
                               SortSequence = "897",
                               NaicsCode = "81394,9",
                               PublishingStatus = "A",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90000000",
                               IndustryName = "Government",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "898",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90910000",
                               IndustryName = "Federal",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "899",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90911000",
                               IndustryName = "Federal, except U.S. Postal Service",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "900",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90916220",
                               IndustryName = "Federal hospitals",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "901",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90919110",
                               IndustryName = "Department of Defense",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "902",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90919120",
                               IndustryName = "U.S. Postal Service",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "903",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90919999",
                               IndustryName = "Other Federal government",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "904",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90920000",
                               IndustryName = "State government",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "905",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90921611",
                               IndustryName = "State government education",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "906",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90922000",
                               IndustryName = "State government, excluding education",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "907",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90922622",
                               IndustryName = "State hospitals",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "908",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90922920",
                               IndustryName = "State government general administration",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "909",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90922999",
                               IndustryName = "Other State government",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "910",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90930000",
                               IndustryName = "Local government",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "911",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90931611",
                               IndustryName = "Local government education",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "912",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90932000",
                               IndustryName = "Local government, excluding education",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "913",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90932221",
                               IndustryName = "Local government utilities",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "914",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90932480",
                               IndustryName = "Local government transportation",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "915",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90932622",
                               IndustryName = "Local hospitals",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "916",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90932920",
                               IndustryName = "Local government general administration",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "917",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },
                           new CeIndustry
                           {
                               IndustryCode = "90932999",
                               IndustryName = "Other local government",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "918",
                               NaicsCode = "-",
                               PublishingStatus = "B",
                           },

                       };
                return _values;
            }
        }
	}//end CeIndustry
}//end NoFuture.Rand.Gov.Bls.Codes