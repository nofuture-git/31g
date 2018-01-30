using System;
using System.Collections.Generic;
namespace NoFuture.Rand.Data.Exo.UsGov.Bls.Codes
{
    public class CuItem 
    {
        public string DisplayLevel { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Selectable { get; set; }
        public string SortSequence { get; set; }
        private static List<CuItem> _values;
        public static List<CuItem> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<CuItem>
                           {
                           
                           new CuItem
                           {
                               SortSequence = "2",
                               ItemName = "All items - old base",
                               DisplayLevel = "0",
                               ItemCode = "AA0",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "399",
                               ItemName = "Purchasing power of the consumer dollar - old base",
                               DisplayLevel = "0",
                               ItemCode = "AA0R",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "1",
                               ItemName = "All items",
                               DisplayLevel = "0",
                               ItemCode = "SA0",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "374",
                               ItemName = "Energy",
                               DisplayLevel = "1",
                               ItemCode = "SA0E",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "358",
                               ItemName = "All items less food",
                               DisplayLevel = "1",
                               ItemCode = "SA0L1",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "361",
                               ItemName = "All items less food and shelter",
                               DisplayLevel = "1",
                               ItemCode = "SA0L12",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "362",
                               ItemName = "All items less food, shelter, and energy",
                               DisplayLevel = "1",
                               ItemCode = "SA0L12E",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "363",
                               ItemName = "All items less food, shelter, energy, and used cars and trucks",
                               DisplayLevel = "1",
                               ItemCode = "SA0L12E4",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "359",
                               ItemName = "All items less food and energy",
                               DisplayLevel = "1",
                               ItemCode = "SA0L1E",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "360",
                               ItemName = "All items less shelter",
                               DisplayLevel = "1",
                               ItemCode = "SA0L2",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "356",
                               ItemName = "All items  less medical care",
                               DisplayLevel = "1",
                               ItemCode = "SA0L5",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "357",
                               ItemName = "All items less energy",
                               DisplayLevel = "1",
                               ItemCode = "SA0LE",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "398",
                               ItemName = "Purchasing power of the consumer dollar",
                               DisplayLevel = "0",
                               ItemCode = "SA0R",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "364",
                               ItemName = "Apparel less footwear",
                               DisplayLevel = "1",
                               ItemCode = "SA311",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "187",
                               ItemName = "Apparel",
                               DisplayLevel = "0",
                               ItemCode = "SAA",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "188",
                               ItemName = "Men's and boys' apparel",
                               DisplayLevel = "1",
                               ItemCode = "SAA1",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "195",
                               ItemName = "Women's and girls' apparel",
                               DisplayLevel = "1",
                               ItemCode = "SAA2",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "365",
                               ItemName = "Commodities",
                               DisplayLevel = "1",
                               ItemCode = "SAC",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "375",
                               ItemName = "Energy commodities",
                               DisplayLevel = "1",
                               ItemCode = "SACE",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "366",
                               ItemName = "Commodities less food",
                               DisplayLevel = "1",
                               ItemCode = "SACL1",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "367",
                               ItemName = "Commodities less food and beverages",
                               DisplayLevel = "1",
                               ItemCode = "SACL11",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "368",
                               ItemName = "Commodities less food and energy commodities",
                               DisplayLevel = "1",
                               ItemCode = "SACL1E",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "369",
                               ItemName = "Commodities less food, energy, and used cars and trucks",
                               DisplayLevel = "1",
                               ItemCode = "SACL1E4",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "371",
                               ItemName = "Durables",
                               DisplayLevel = "1",
                               ItemCode = "SAD",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "311",
                               ItemName = "Education and communication",
                               DisplayLevel = "0",
                               ItemCode = "SAE",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "312",
                               ItemName = "Education",
                               DisplayLevel = "1",
                               ItemCode = "SAE1",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "320",
                               ItemName = "Communication",
                               DisplayLevel = "1",
                               ItemCode = "SAE2",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "324",
                               ItemName = "Information and information processing",
                               DisplayLevel = "2",
                               ItemCode = "SAE21",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "372",
                               ItemName = "Education and communication commodities",
                               DisplayLevel = "1",
                               ItemCode = "SAEC",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "373",
                               ItemName = "Education and communication services",
                               DisplayLevel = "1",
                               ItemCode = "SAES",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "3",
                               ItemName = "Food and beverages",
                               DisplayLevel = "0",
                               ItemCode = "SAF",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "4",
                               ItemName = "Food",
                               DisplayLevel = "1",
                               ItemCode = "SAF1",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "5",
                               ItemName = "Food at home",
                               DisplayLevel = "2",
                               ItemCode = "SAF11",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "6",
                               ItemName = "Cereals and bakery products",
                               DisplayLevel = "3",
                               ItemCode = "SAF111",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "24",
                               ItemName = "Meats, poultry, fish, and eggs",
                               DisplayLevel = "3",
                               ItemCode = "SAF112",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "25",
                               ItemName = "Meats, poultry, and fish",
                               DisplayLevel = "4",
                               ItemCode = "SAF1121",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "26",
                               ItemName = "Meats",
                               DisplayLevel = "5",
                               ItemCode = "SAF11211",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "63",
                               ItemName = "Fruits and vegetables",
                               DisplayLevel = "3",
                               ItemCode = "SAF113",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "64",
                               ItemName = "Fresh fruits and vegetables",
                               DisplayLevel = "4",
                               ItemCode = "SAF1131",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "84",
                               ItemName = "Nonalcoholic beverages and beverage materials",
                               DisplayLevel = "3",
                               ItemCode = "SAF114",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "94",
                               ItemName = "Other food at home",
                               DisplayLevel = "3",
                               ItemCode = "SAF115",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "125",
                               ItemName = "Alcoholic beverages",
                               DisplayLevel = "2",
                               ItemCode = "SAF116",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "335",
                               ItemName = "Other goods and services",
                               DisplayLevel = "0",
                               ItemCode = "SAG",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "339",
                               ItemName = "Personal care",
                               DisplayLevel = "1",
                               ItemCode = "SAG1",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "383",
                               ItemName = "Other goods",
                               DisplayLevel = "1",
                               ItemCode = "SAGC",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "384",
                               ItemName = "Other personal services",
                               DisplayLevel = "1",
                               ItemCode = "SAGS",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "136",
                               ItemName = "Housing",
                               DisplayLevel = "0",
                               ItemCode = "SAH",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "137",
                               ItemName = "Shelter",
                               DisplayLevel = "1",
                               ItemCode = "SAH1",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "145",
                               ItemName = "Fuels and utilities",
                               DisplayLevel = "1",
                               ItemCode = "SAH2",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "146",
                               ItemName = "Household energy",
                               DisplayLevel = "2",
                               ItemCode = "SAH21",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "156",
                               ItemName = "Household furnishings and operations",
                               DisplayLevel = "1",
                               ItemCode = "SAH3",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "376",
                               ItemName = "Household furnishings and supplies",
                               DisplayLevel = "1",
                               ItemCode = "SAH31",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "250",
                               ItemName = "Medical care",
                               DisplayLevel = "0",
                               ItemCode = "SAM",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "251",
                               ItemName = "Medical care commodities",
                               DisplayLevel = "1",
                               ItemCode = "SAM1",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "256",
                               ItemName = "Medical care services",
                               DisplayLevel = "1",
                               ItemCode = "SAM2",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "378",
                               ItemName = "Nondurables",
                               DisplayLevel = "1",
                               ItemCode = "SAN",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "370",
                               ItemName = "Domestically produced farm food",
                               DisplayLevel = "1",
                               ItemCode = "SAN1D",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "379",
                               ItemName = "Nondurables less food",
                               DisplayLevel = "1",
                               ItemCode = "SANL1",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "381",
                               ItemName = "Nondurables less food and beverages",
                               DisplayLevel = "1",
                               ItemCode = "SANL11",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "382",
                               ItemName = "Nondurables less food, beverages, and apparel",
                               DisplayLevel = "1",
                               ItemCode = "SANL113",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "380",
                               ItemName = "Nondurables less food and apparel",
                               DisplayLevel = "1",
                               ItemCode = "SANL13",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "269",
                               ItemName = "Recreation",
                               DisplayLevel = "0",
                               ItemCode = "SAR",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "386",
                               ItemName = "Recreation commodities",
                               DisplayLevel = "1",
                               ItemCode = "SARC",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "387",
                               ItemName = "Recreation services",
                               DisplayLevel = "1",
                               ItemCode = "SARS",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "389",
                               ItemName = "Services",
                               DisplayLevel = "1",
                               ItemCode = "SAS",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "395",
                               ItemName = "Utilities and public transportation",
                               DisplayLevel = "1",
                               ItemCode = "SAS24",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "388",
                               ItemName = "Rent of shelter",
                               DisplayLevel = "1",
                               ItemCode = "SAS2RS",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "385",
                               ItemName = "Other services",
                               DisplayLevel = "1",
                               ItemCode = "SAS367",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "394",
                               ItemName = "Transportation services",
                               DisplayLevel = "1",
                               ItemCode = "SAS4",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "392",
                               ItemName = "Services less rent of shelter",
                               DisplayLevel = "1",
                               ItemCode = "SASL2RS",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "391",
                               ItemName = "Services less medical care services",
                               DisplayLevel = "1",
                               ItemCode = "SASL5",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "390",
                               ItemName = "Services less energy services",
                               DisplayLevel = "1",
                               ItemCode = "SASLE",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "210",
                               ItemName = "Transportation",
                               DisplayLevel = "0",
                               ItemCode = "SAT",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "211",
                               ItemName = "Private transportation",
                               DisplayLevel = "1",
                               ItemCode = "SAT1",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "393",
                               ItemName = "Transportation commodities less motor fuel",
                               DisplayLevel = "1",
                               ItemCode = "SATCLTB",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "189",
                               ItemName = "Men's apparel",
                               DisplayLevel = "2",
                               ItemCode = "SEAA",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "190",
                               ItemName = "Men's suits, sport coats, and outerwear",
                               DisplayLevel = "3",
                               ItemCode = "SEAA01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "191",
                               ItemName = "Men's furnishings",
                               DisplayLevel = "3",
                               ItemCode = "SEAA02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "192",
                               ItemName = "Men's shirts and sweaters",
                               DisplayLevel = "3",
                               ItemCode = "SEAA03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "193",
                               ItemName = "Men's pants and shorts",
                               DisplayLevel = "3",
                               ItemCode = "SEAA04",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "194",
                               ItemName = "Boys' apparel",
                               DisplayLevel = "2",
                               ItemCode = "SEAB",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "196",
                               ItemName = "Women's apparel",
                               DisplayLevel = "2",
                               ItemCode = "SEAC",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "197",
                               ItemName = "Women's outerwear",
                               DisplayLevel = "3",
                               ItemCode = "SEAC01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "198",
                               ItemName = "Women's dresses",
                               DisplayLevel = "3",
                               ItemCode = "SEAC02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "199",
                               ItemName = "Women's suits and separates",
                               DisplayLevel = "3",
                               ItemCode = "SEAC03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "200",
                               ItemName = "Women's underwear, nightwear, sportswear and accessories",
                               DisplayLevel = "3",
                               ItemCode = "SEAC04",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "201",
                               ItemName = "Girls' apparel",
                               DisplayLevel = "2",
                               ItemCode = "SEAD",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "202",
                               ItemName = "Footwear",
                               DisplayLevel = "1",
                               ItemCode = "SEAE",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "203",
                               ItemName = "Men's footwear",
                               DisplayLevel = "2",
                               ItemCode = "SEAE01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "204",
                               ItemName = "Boys' and girls' footwear",
                               DisplayLevel = "2",
                               ItemCode = "SEAE02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "205",
                               ItemName = "Women's footwear",
                               DisplayLevel = "2",
                               ItemCode = "SEAE03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "206",
                               ItemName = "Infants' and toddlers' apparel",
                               DisplayLevel = "1",
                               ItemCode = "SEAF",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "207",
                               ItemName = "Jewelry and watches",
                               DisplayLevel = "1",
                               ItemCode = "SEAG",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "208",
                               ItemName = "Watches",
                               DisplayLevel = "2",
                               ItemCode = "SEAG01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "209",
                               ItemName = "Jewelry",
                               DisplayLevel = "2",
                               ItemCode = "SEAG02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "313",
                               ItemName = "Educational books and supplies",
                               DisplayLevel = "2",
                               ItemCode = "SEEA",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "315",
                               ItemName = "Tuition, other school fees, and childcare",
                               DisplayLevel = "2",
                               ItemCode = "SEEB",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "316",
                               ItemName = "College tuition and fees",
                               DisplayLevel = "3",
                               ItemCode = "SEEB01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "317",
                               ItemName = "Elementary and high school tuition and fees",
                               DisplayLevel = "3",
                               ItemCode = "SEEB02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "318",
                               ItemName = "Child care and nursery school",
                               DisplayLevel = "3",
                               ItemCode = "SEEB03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "319",
                               ItemName = "Technical and business school tuition and fees",
                               DisplayLevel = "3",
                               ItemCode = "SEEB04",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "321",
                               ItemName = "Postage and delivery services",
                               DisplayLevel = "2",
                               ItemCode = "SEEC",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "322",
                               ItemName = "Postage",
                               DisplayLevel = "3",
                               ItemCode = "SEEC01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "323",
                               ItemName = "Delivery services",
                               DisplayLevel = "3",
                               ItemCode = "SEEC02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "325",
                               ItemName = "Telephone services",
                               DisplayLevel = "3",
                               ItemCode = "SEED",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "326",
                               ItemName = "Wireless telephone services",
                               DisplayLevel = "4",
                               ItemCode = "SEED03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "327",
                               ItemName = "Land-line telephone services",
                               DisplayLevel = "4",
                               ItemCode = "SEED04",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "330",
                               ItemName = "Information technology, hardware and services",
                               DisplayLevel = "2",
                               ItemCode = "SEEE",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "331",
                               ItemName = "Personal computers and peripheral equipment",
                               DisplayLevel = "3",
                               ItemCode = "SEEE01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "332",
                               ItemName = "Computer software and accessories",
                               DisplayLevel = "3",
                               ItemCode = "SEEE02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "333",
                               ItemName = "Internet services and electronic information providers",
                               DisplayLevel = "3",
                               ItemCode = "SEEE03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "334",
                               ItemName = "Telephone hardware, calculators, and other consumer information items",
                               DisplayLevel = "3",
                               ItemCode = "SEEE04",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "377",
                               ItemName = "Information technology commodities",
                               DisplayLevel = "1",
                               ItemCode = "SEEEC",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "7",
                               ItemName = "Cereals and cereal products",
                               DisplayLevel = "4",
                               ItemCode = "SEFA",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "8",
                               ItemName = "Flour and prepared flour mixes",
                               DisplayLevel = "5",
                               ItemCode = "SEFA01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "9",
                               ItemName = "Breakfast cereal",
                               DisplayLevel = "5",
                               ItemCode = "SEFA02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "10",
                               ItemName = "Rice, pasta, cornmeal",
                               DisplayLevel = "5",
                               ItemCode = "SEFA03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "12",
                               ItemName = "Bakery products",
                               DisplayLevel = "4",
                               ItemCode = "SEFB",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "13",
                               ItemName = "Bread",
                               DisplayLevel = "5",
                               ItemCode = "SEFB01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "16",
                               ItemName = "Fresh biscuits, rolls, muffins",
                               DisplayLevel = "5",
                               ItemCode = "SEFB02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "17",
                               ItemName = "Cakes, cupcakes, and cookies",
                               DisplayLevel = "5",
                               ItemCode = "SEFB03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "20",
                               ItemName = "Other bakery products",
                               DisplayLevel = "5",
                               ItemCode = "SEFB04",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "27",
                               ItemName = "Beef and veal",
                               DisplayLevel = "6",
                               ItemCode = "SEFC",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "28",
                               ItemName = "Uncooked ground beef",
                               DisplayLevel = "7",
                               ItemCode = "SEFC01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "29",
                               ItemName = "Uncooked beef roasts",
                               DisplayLevel = "7",
                               ItemCode = "SEFC02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "30",
                               ItemName = "Uncooked beef steaks",
                               DisplayLevel = "7",
                               ItemCode = "SEFC03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "31",
                               ItemName = "Uncooked other beef and veal",
                               DisplayLevel = "7",
                               ItemCode = "SEFC04",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "32",
                               ItemName = "Pork",
                               DisplayLevel = "6",
                               ItemCode = "SEFD",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "33",
                               ItemName = "Bacon, breakfast sausage, and related products",
                               DisplayLevel = "7",
                               ItemCode = "SEFD01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "36",
                               ItemName = "Ham",
                               DisplayLevel = "7",
                               ItemCode = "SEFD02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "38",
                               ItemName = "Pork chops",
                               DisplayLevel = "7",
                               ItemCode = "SEFD03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "39",
                               ItemName = "Other pork including roasts and picnics",
                               DisplayLevel = "7",
                               ItemCode = "SEFD04",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "40",
                               ItemName = "Other meats",
                               DisplayLevel = "6",
                               ItemCode = "SEFE",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "45",
                               ItemName = "Poultry",
                               DisplayLevel = "6",
                               ItemCode = "SEFF",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "46",
                               ItemName = "Chicken",
                               DisplayLevel = "7",
                               ItemCode = "SEFF01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "49",
                               ItemName = "Other poultry including turkey",
                               DisplayLevel = "6",
                               ItemCode = "SEFF02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "50",
                               ItemName = "Fish and seafood",
                               DisplayLevel = "5",
                               ItemCode = "SEFG",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "51",
                               ItemName = "Fresh fish and seafood",
                               DisplayLevel = "6",
                               ItemCode = "SEFG01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "52",
                               ItemName = "Processed fish and seafood",
                               DisplayLevel = "6",
                               ItemCode = "SEFG02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "55",
                               ItemName = "Eggs",
                               DisplayLevel = "4",
                               ItemCode = "SEFH",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "56",
                               ItemName = "Dairy and related products",
                               DisplayLevel = "3",
                               ItemCode = "SEFJ",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "57",
                               ItemName = "Milk",
                               DisplayLevel = "4",
                               ItemCode = "SEFJ01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "60",
                               ItemName = "Cheese and related products",
                               DisplayLevel = "4",
                               ItemCode = "SEFJ02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "61",
                               ItemName = "Ice cream and related products",
                               DisplayLevel = "4",
                               ItemCode = "SEFJ03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "62",
                               ItemName = "Other dairy and related products",
                               DisplayLevel = "4",
                               ItemCode = "SEFJ04",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "65",
                               ItemName = "Fresh fruits",
                               DisplayLevel = "5",
                               ItemCode = "SEFK",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "66",
                               ItemName = "Apples",
                               DisplayLevel = "6",
                               ItemCode = "SEFK01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "67",
                               ItemName = "Bananas",
                               DisplayLevel = "6",
                               ItemCode = "SEFK02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "68",
                               ItemName = "Citrus fruits",
                               DisplayLevel = "6",
                               ItemCode = "SEFK03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "70",
                               ItemName = "Other fresh fruits",
                               DisplayLevel = "6",
                               ItemCode = "SEFK04",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "71",
                               ItemName = "Fresh vegetables",
                               DisplayLevel = "5",
                               ItemCode = "SEFL",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "72",
                               ItemName = "Potatoes",
                               DisplayLevel = "6",
                               ItemCode = "SEFL01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "73",
                               ItemName = "Lettuce",
                               DisplayLevel = "6",
                               ItemCode = "SEFL02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "74",
                               ItemName = "Tomatoes",
                               DisplayLevel = "6",
                               ItemCode = "SEFL03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "75",
                               ItemName = "Other fresh vegetables",
                               DisplayLevel = "6",
                               ItemCode = "SEFL04",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "76",
                               ItemName = "Processed fruits and vegetables",
                               DisplayLevel = "4",
                               ItemCode = "SEFM",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "77",
                               ItemName = "Canned fruits and vegetables",
                               DisplayLevel = "5",
                               ItemCode = "SEFM01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "80",
                               ItemName = "Frozen fruits and vegetables",
                               DisplayLevel = "5",
                               ItemCode = "SEFM02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "82",
                               ItemName = "Other processed fruits and vegetables including dried",
                               DisplayLevel = "5",
                               ItemCode = "SEFM03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "85",
                               ItemName = "Juices and nonalcoholic drinks",
                               DisplayLevel = "4",
                               ItemCode = "SEFN",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "86",
                               ItemName = "Carbonated drinks",
                               DisplayLevel = "5",
                               ItemCode = "SEFN01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "87",
                               ItemName = "Frozen noncarbonated juices and drinks",
                               DisplayLevel = "5",
                               ItemCode = "SEFN02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "88",
                               ItemName = "Nonfrozen noncarbonated juices and drinks",
                               DisplayLevel = "5",
                               ItemCode = "SEFN03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "89",
                               ItemName = "Beverage materials including coffee and tea",
                               DisplayLevel = "4",
                               ItemCode = "SEFP",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "90",
                               ItemName = "Coffee",
                               DisplayLevel = "5",
                               ItemCode = "SEFP01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "93",
                               ItemName = "Other beverage materials including tea",
                               DisplayLevel = "5",
                               ItemCode = "SEFP02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "95",
                               ItemName = "Sugar and sweets",
                               DisplayLevel = "4",
                               ItemCode = "SEFR",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "96",
                               ItemName = "Sugar and artificial sweeteners",
                               DisplayLevel = "5",
                               ItemCode = "SEFR01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "97",
                               ItemName = "Candy and chewing gum",
                               DisplayLevel = "5",
                               ItemCode = "SEFR02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "98",
                               ItemName = "Other sweets",
                               DisplayLevel = "5",
                               ItemCode = "SEFR03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "99",
                               ItemName = "Fats and oils",
                               DisplayLevel = "4",
                               ItemCode = "SEFS",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "100",
                               ItemName = "Butter and margarine",
                               DisplayLevel = "5",
                               ItemCode = "SEFS01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "103",
                               ItemName = "Salad dressing",
                               DisplayLevel = "5",
                               ItemCode = "SEFS02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "104",
                               ItemName = "Other fats and oils including peanut butter",
                               DisplayLevel = "5",
                               ItemCode = "SEFS03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "106",
                               ItemName = "Other foods",
                               DisplayLevel = "4",
                               ItemCode = "SEFT",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "107",
                               ItemName = "Soups",
                               DisplayLevel = "5",
                               ItemCode = "SEFT01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "108",
                               ItemName = "Frozen and freeze dried prepared foods",
                               DisplayLevel = "5",
                               ItemCode = "SEFT02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "109",
                               ItemName = "Snacks",
                               DisplayLevel = "5",
                               ItemCode = "SEFT03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "110",
                               ItemName = "Spices, seasonings, condiments, sauces",
                               DisplayLevel = "5",
                               ItemCode = "SEFT04",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "115",
                               ItemName = "Baby food",
                               DisplayLevel = "5",
                               ItemCode = "SEFT05",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "116",
                               ItemName = "Other miscellaneous foods",
                               DisplayLevel = "5",
                               ItemCode = "SEFT06",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "118",
                               ItemName = "Food away from home",
                               DisplayLevel = "2",
                               ItemCode = "SEFV",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "119",
                               ItemName = "Full service meals and snacks",
                               DisplayLevel = "3",
                               ItemCode = "SEFV01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "120",
                               ItemName = "Limited service meals and snacks",
                               DisplayLevel = "3",
                               ItemCode = "SEFV02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "121",
                               ItemName = "Food at employee sites and schools",
                               DisplayLevel = "3",
                               ItemCode = "SEFV03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "123",
                               ItemName = "Food from vending machines and mobile vendors",
                               DisplayLevel = "3",
                               ItemCode = "SEFV04",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "124",
                               ItemName = "Other food away from home",
                               DisplayLevel = "3",
                               ItemCode = "SEFV05",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "126",
                               ItemName = "Alcoholic beverages at home",
                               DisplayLevel = "3",
                               ItemCode = "SEFW",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "127",
                               ItemName = "Beer, ale, and other malt beverages at home",
                               DisplayLevel = "4",
                               ItemCode = "SEFW01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "128",
                               ItemName = "Distilled spirits at home",
                               DisplayLevel = "4",
                               ItemCode = "SEFW02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "131",
                               ItemName = "Wine at home",
                               DisplayLevel = "4",
                               ItemCode = "SEFW03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "132",
                               ItemName = "Alcoholic beverages away from home",
                               DisplayLevel = "3",
                               ItemCode = "SEFX",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "336",
                               ItemName = "Tobacco and smoking products",
                               DisplayLevel = "1",
                               ItemCode = "SEGA",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "337",
                               ItemName = "Cigarettes",
                               DisplayLevel = "2",
                               ItemCode = "SEGA01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "338",
                               ItemName = "Tobacco products other than cigarettes",
                               DisplayLevel = "2",
                               ItemCode = "SEGA02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "340",
                               ItemName = "Personal care products",
                               DisplayLevel = "2",
                               ItemCode = "SEGB",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "341",
                               ItemName = "Hair, dental, shaving, and miscellaneous personal care products",
                               DisplayLevel = "3",
                               ItemCode = "SEGB01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "342",
                               ItemName = "Cosmetics, perfume, bath, nail preparations and implements",
                               DisplayLevel = "3",
                               ItemCode = "SEGB02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "343",
                               ItemName = "Personal care services",
                               DisplayLevel = "2",
                               ItemCode = "SEGC",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "344",
                               ItemName = "Haircuts and other personal care services",
                               DisplayLevel = "3",
                               ItemCode = "SEGC01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "345",
                               ItemName = "Miscellaneous personal services",
                               DisplayLevel = "2",
                               ItemCode = "SEGD",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "346",
                               ItemName = "Legal services",
                               DisplayLevel = "3",
                               ItemCode = "SEGD01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "347",
                               ItemName = "Funeral expenses",
                               DisplayLevel = "3",
                               ItemCode = "SEGD02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "348",
                               ItemName = "Laundry and dry cleaning services",
                               DisplayLevel = "3",
                               ItemCode = "SEGD03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "349",
                               ItemName = "Apparel services other than laundry and dry cleaning",
                               DisplayLevel = "3",
                               ItemCode = "SEGD04",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "350",
                               ItemName = "Financial services",
                               DisplayLevel = "3",
                               ItemCode = "SEGD05",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "353",
                               ItemName = "Miscellaneous personal goods",
                               DisplayLevel = "2",
                               ItemCode = "SEGE",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "138",
                               ItemName = "Rent of primary residence",
                               DisplayLevel = "2",
                               ItemCode = "SEHA",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "139",
                               ItemName = "Lodging away from home",
                               DisplayLevel = "2",
                               ItemCode = "SEHB",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "140",
                               ItemName = "Housing at school, excluding board",
                               DisplayLevel = "3",
                               ItemCode = "SEHB01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "141",
                               ItemName = "Other lodging away from home including hotels and motels",
                               DisplayLevel = "3",
                               ItemCode = "SEHB02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "142",
                               ItemName = "Owners' equivalent rent of residences",
                               DisplayLevel = "2",
                               ItemCode = "SEHC",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "143",
                               ItemName = "Owners' equivalent rent of primary residence",
                               DisplayLevel = "3",
                               ItemCode = "SEHC01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "144",
                               ItemName = "Tenants' and household insurance",
                               DisplayLevel = "2",
                               ItemCode = "SEHD",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "147",
                               ItemName = "Fuel oil and other fuels",
                               DisplayLevel = "3",
                               ItemCode = "SEHE",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "148",
                               ItemName = "Fuel oil",
                               DisplayLevel = "4",
                               ItemCode = "SEHE01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "149",
                               ItemName = "Propane, kerosene, and firewood",
                               DisplayLevel = "4",
                               ItemCode = "SEHE02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "150",
                               ItemName = "Energy services",
                               DisplayLevel = "3",
                               ItemCode = "SEHF",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "151",
                               ItemName = "Electricity",
                               DisplayLevel = "4",
                               ItemCode = "SEHF01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "152",
                               ItemName = "Utility (piped) gas service",
                               DisplayLevel = "4",
                               ItemCode = "SEHF02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "153",
                               ItemName = "Water and sewer and trash collection services",
                               DisplayLevel = "2",
                               ItemCode = "SEHG",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "154",
                               ItemName = "Water and sewerage maintenance",
                               DisplayLevel = "3",
                               ItemCode = "SEHG01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "155",
                               ItemName = "Garbage and trash collection",
                               DisplayLevel = "3",
                               ItemCode = "SEHG02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "157",
                               ItemName = "Window and floor coverings and other linens",
                               DisplayLevel = "2",
                               ItemCode = "SEHH",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "158",
                               ItemName = "Floor coverings",
                               DisplayLevel = "3",
                               ItemCode = "SEHH01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "159",
                               ItemName = "Window coverings",
                               DisplayLevel = "3",
                               ItemCode = "SEHH02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "160",
                               ItemName = "Other linens",
                               DisplayLevel = "3",
                               ItemCode = "SEHH03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "161",
                               ItemName = "Furniture and bedding",
                               DisplayLevel = "2",
                               ItemCode = "SEHJ",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "162",
                               ItemName = "Bedroom furniture",
                               DisplayLevel = "3",
                               ItemCode = "SEHJ01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "163",
                               ItemName = "Living room, kitchen, and dining room furniture",
                               DisplayLevel = "3",
                               ItemCode = "SEHJ02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "164",
                               ItemName = "Other furniture",
                               DisplayLevel = "3",
                               ItemCode = "SEHJ03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "166",
                               ItemName = "Appliances",
                               DisplayLevel = "2",
                               ItemCode = "SEHK",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "167",
                               ItemName = "Major appliances",
                               DisplayLevel = "3",
                               ItemCode = "SEHK01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "169",
                               ItemName = "Other appliances",
                               DisplayLevel = "3",
                               ItemCode = "SEHK02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "170",
                               ItemName = "Other household equipment and furnishings",
                               DisplayLevel = "2",
                               ItemCode = "SEHL",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "171",
                               ItemName = "Clocks, lamps, and decorator items",
                               DisplayLevel = "3",
                               ItemCode = "SEHL01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "172",
                               ItemName = "Indoor plants and flowers",
                               DisplayLevel = "3",
                               ItemCode = "SEHL02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "173",
                               ItemName = "Dishes and flatware",
                               DisplayLevel = "3",
                               ItemCode = "SEHL03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "174",
                               ItemName = "Nonelectric cookware and tableware",
                               DisplayLevel = "3",
                               ItemCode = "SEHL04",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "175",
                               ItemName = "Tools, hardware, outdoor equipment and supplies",
                               DisplayLevel = "2",
                               ItemCode = "SEHM",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "176",
                               ItemName = "Tools, hardware and supplies",
                               DisplayLevel = "3",
                               ItemCode = "SEHM01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "177",
                               ItemName = "Outdoor equipment and supplies",
                               DisplayLevel = "3",
                               ItemCode = "SEHM02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "178",
                               ItemName = "Housekeeping supplies",
                               DisplayLevel = "2",
                               ItemCode = "SEHN",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "179",
                               ItemName = "Household cleaning products",
                               DisplayLevel = "3",
                               ItemCode = "SEHN01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "180",
                               ItemName = "Household paper products",
                               DisplayLevel = "3",
                               ItemCode = "SEHN02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "181",
                               ItemName = "Miscellaneous household products",
                               DisplayLevel = "3",
                               ItemCode = "SEHN03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "182",
                               ItemName = "Household operations",
                               DisplayLevel = "2",
                               ItemCode = "SEHP",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "183",
                               ItemName = "Domestic services",
                               DisplayLevel = "3",
                               ItemCode = "SEHP01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "184",
                               ItemName = "Gardening and lawncare services",
                               DisplayLevel = "3",
                               ItemCode = "SEHP02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "185",
                               ItemName = "Moving, storage, freight expense",
                               DisplayLevel = "3",
                               ItemCode = "SEHP03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "186",
                               ItemName = "Repair of household items",
                               DisplayLevel = "3",
                               ItemCode = "SEHP04",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "257",
                               ItemName = "Professional services",
                               DisplayLevel = "2",
                               ItemCode = "SEMC",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "258",
                               ItemName = "Physicians' services",
                               DisplayLevel = "3",
                               ItemCode = "SEMC01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "259",
                               ItemName = "Dental services",
                               DisplayLevel = "3",
                               ItemCode = "SEMC02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "260",
                               ItemName = "Eyeglasses and eye care",
                               DisplayLevel = "3",
                               ItemCode = "SEMC03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "261",
                               ItemName = "Services by other medical professionals",
                               DisplayLevel = "3",
                               ItemCode = "SEMC04",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "262",
                               ItemName = "Hospital and related services",
                               DisplayLevel = "2",
                               ItemCode = "SEMD",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "263",
                               ItemName = "Hospital services",
                               DisplayLevel = "3",
                               ItemCode = "SEMD01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "266",
                               ItemName = "Nursing homes and adult day services",
                               DisplayLevel = "3",
                               ItemCode = "SEMD02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "267",
                               ItemName = "Care of invalids and elderly at home",
                               DisplayLevel = "3",
                               ItemCode = "SEMD03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "268",
                               ItemName = "Health insurance",
                               DisplayLevel = "2",
                               ItemCode = "SEME",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "252",
                               ItemName = "Medicinal drugs",
                               DisplayLevel = "2",
                               ItemCode = "SEMF",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "253",
                               ItemName = "Prescription drugs",
                               DisplayLevel = "3",
                               ItemCode = "SEMF01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "254",
                               ItemName = "Nonprescription drugs",
                               DisplayLevel = "3",
                               ItemCode = "SEMF02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "255",
                               ItemName = "Medical equipment and supplies",
                               DisplayLevel = "2",
                               ItemCode = "SEMG",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "270",
                               ItemName = "Video and audio",
                               DisplayLevel = "1",
                               ItemCode = "SERA",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "271",
                               ItemName = "Televisions",
                               DisplayLevel = "2",
                               ItemCode = "SERA01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "272",
                               ItemName = "Cable and satellite television and radio service",
                               DisplayLevel = "2",
                               ItemCode = "SERA02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "273",
                               ItemName = "Other video equipment",
                               DisplayLevel = "2",
                               ItemCode = "SERA03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "274",
                               ItemName = "Video discs and other media, including rental of video and audio",
                               DisplayLevel = "2",
                               ItemCode = "SERA04",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "277",
                               ItemName = "Audio equipment",
                               DisplayLevel = "2",
                               ItemCode = "SERA05",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "278",
                               ItemName = "Audio discs, tapes and other media",
                               DisplayLevel = "2",
                               ItemCode = "SERA06",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "396",
                               ItemName = "Video and audio products",
                               DisplayLevel = "1",
                               ItemCode = "SERAC",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "397",
                               ItemName = "Video and audio services",
                               DisplayLevel = "1",
                               ItemCode = "SERAS",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "279",
                               ItemName = "Pets, pet products and services",
                               DisplayLevel = "1",
                               ItemCode = "SERB",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "280",
                               ItemName = "Pets and pet products",
                               DisplayLevel = "2",
                               ItemCode = "SERB01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "283",
                               ItemName = "Pet services including veterinary",
                               DisplayLevel = "2",
                               ItemCode = "SERB02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "286",
                               ItemName = "Sporting goods",
                               DisplayLevel = "1",
                               ItemCode = "SERC",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "287",
                               ItemName = "Sports vehicles including bicycles",
                               DisplayLevel = "2",
                               ItemCode = "SERC01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "288",
                               ItemName = "Sports equipment",
                               DisplayLevel = "2",
                               ItemCode = "SERC02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "289",
                               ItemName = "Photography",
                               DisplayLevel = "1",
                               ItemCode = "SERD",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "290",
                               ItemName = "Photographic equipment and supplies",
                               DisplayLevel = "2",
                               ItemCode = "SERD01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "293",
                               ItemName = "Photographers and film processing",
                               DisplayLevel = "2",
                               ItemCode = "SERD02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "296",
                               ItemName = "Other recreational goods",
                               DisplayLevel = "1",
                               ItemCode = "SERE",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "297",
                               ItemName = "Toys",
                               DisplayLevel = "2",
                               ItemCode = "SERE01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "300",
                               ItemName = "Sewing machines, fabric and supplies",
                               DisplayLevel = "2",
                               ItemCode = "SERE02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "301",
                               ItemName = "Music instruments and accessories",
                               DisplayLevel = "2",
                               ItemCode = "SERE03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "302",
                               ItemName = "Other recreation services",
                               DisplayLevel = "1",
                               ItemCode = "SERF",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "303",
                               ItemName = "Club dues and fees for participant sports and group exercises",
                               DisplayLevel = "2",
                               ItemCode = "SERF01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "304",
                               ItemName = "Admissions",
                               DisplayLevel = "2",
                               ItemCode = "SERF02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "307",
                               ItemName = "Fees for lessons or instructions",
                               DisplayLevel = "2",
                               ItemCode = "SERF03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "308",
                               ItemName = "Recreational reading materials",
                               DisplayLevel = "1",
                               ItemCode = "SERG",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "309",
                               ItemName = "Newspapers and magazines",
                               DisplayLevel = "2",
                               ItemCode = "SERG01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "310",
                               ItemName = "Recreational books",
                               DisplayLevel = "2",
                               ItemCode = "SERG02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "212",
                               ItemName = "New and used motor vehicles",
                               DisplayLevel = "2",
                               ItemCode = "SETA",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "213",
                               ItemName = "New vehicles",
                               DisplayLevel = "3",
                               ItemCode = "SETA01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "218",
                               ItemName = "Used cars and trucks",
                               DisplayLevel = "3",
                               ItemCode = "SETA02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "219",
                               ItemName = "Leased cars and trucks",
                               DisplayLevel = "3",
                               ItemCode = "SETA03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "220",
                               ItemName = "Car and truck rental",
                               DisplayLevel = "3",
                               ItemCode = "SETA04",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "221",
                               ItemName = "Motor fuel",
                               DisplayLevel = "2",
                               ItemCode = "SETB",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "222",
                               ItemName = "Gasoline (all types)",
                               DisplayLevel = "3",
                               ItemCode = "SETB01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "226",
                               ItemName = "Other motor fuels",
                               DisplayLevel = "3",
                               ItemCode = "SETB02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "227",
                               ItemName = "Motor vehicle parts and equipment",
                               DisplayLevel = "2",
                               ItemCode = "SETC",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "228",
                               ItemName = "Tires",
                               DisplayLevel = "3",
                               ItemCode = "SETC01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "229",
                               ItemName = "Vehicle accessories other than tires",
                               DisplayLevel = "3",
                               ItemCode = "SETC02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "232",
                               ItemName = "Motor vehicle maintenance and repair",
                               DisplayLevel = "2",
                               ItemCode = "SETD",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "233",
                               ItemName = "Motor vehicle body work",
                               DisplayLevel = "3",
                               ItemCode = "SETD01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "234",
                               ItemName = "Motor vehicle maintenance and servicing",
                               DisplayLevel = "3",
                               ItemCode = "SETD02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "235",
                               ItemName = "Motor vehicle repair",
                               DisplayLevel = "3",
                               ItemCode = "SETD03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "236",
                               ItemName = "Motor vehicle insurance",
                               DisplayLevel = "2",
                               ItemCode = "SETE",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "237",
                               ItemName = "Motor vehicle fees",
                               DisplayLevel = "2",
                               ItemCode = "SETF",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "238",
                               ItemName = "State motor vehicle registration and license fees",
                               DisplayLevel = "3",
                               ItemCode = "SETF01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "239",
                               ItemName = "Parking and other fees",
                               DisplayLevel = "3",
                               ItemCode = "SETF03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "242",
                               ItemName = "Public transportation",
                               DisplayLevel = "1",
                               ItemCode = "SETG",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "243",
                               ItemName = "Airline fare",
                               DisplayLevel = "2",
                               ItemCode = "SETG01",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "244",
                               ItemName = "Other intercity transportation",
                               DisplayLevel = "2",
                               ItemCode = "SETG02",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "248",
                               ItemName = "Intracity transportation",
                               DisplayLevel = "2",
                               ItemCode = "SETG03",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "11",
                               ItemName = "Rice",
                               DisplayLevel = "6",
                               ItemCode = "SS01031",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "14",
                               ItemName = "White bread",
                               DisplayLevel = "6",
                               ItemCode = "SS02011",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "15",
                               ItemName = "Bread other than white",
                               DisplayLevel = "6",
                               ItemCode = "SS02021",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "18",
                               ItemName = "Fresh cakes and cupcakes",
                               DisplayLevel = "6",
                               ItemCode = "SS02041",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "19",
                               ItemName = "Cookies",
                               DisplayLevel = "6",
                               ItemCode = "SS02042",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "21",
                               ItemName = "Fresh sweetrolls, coffeecakes, doughnuts",
                               DisplayLevel = "6",
                               ItemCode = "SS02063",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "22",
                               ItemName = "Crackers, bread, and cracker products",
                               DisplayLevel = "6",
                               ItemCode = "SS0206A",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "23",
                               ItemName = "Frozen and refrigerated bakery products, pies, tarts, turnovers",
                               DisplayLevel = "6",
                               ItemCode = "SS0206B",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "34",
                               ItemName = "Bacon and related products",
                               DisplayLevel = "8",
                               ItemCode = "SS04011",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "35",
                               ItemName = "Breakfast sausage and related products",
                               DisplayLevel = "8",
                               ItemCode = "SS04012",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "37",
                               ItemName = "Ham, excluding canned",
                               DisplayLevel = "8",
                               ItemCode = "SS04031",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "41",
                               ItemName = "Frankfurters",
                               DisplayLevel = "7",
                               ItemCode = "SS05011",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "43",
                               ItemName = "Lamb and organ meats",
                               DisplayLevel = "7",
                               ItemCode = "SS05014",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "44",
                               ItemName = "Lamb and mutton",
                               DisplayLevel = "7",
                               ItemCode = "SS05015",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "42",
                               ItemName = "Lunchmeats",
                               DisplayLevel = "7",
                               ItemCode = "SS0501A",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "47",
                               ItemName = "Fresh whole chicken",
                               DisplayLevel = "7",
                               ItemCode = "SS06011",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "48",
                               ItemName = "Fresh and frozen chicken parts",
                               DisplayLevel = "7",
                               ItemCode = "SS06021",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "53",
                               ItemName = "Shelf stable fish and seafood",
                               DisplayLevel = "7",
                               ItemCode = "SS07011",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "54",
                               ItemName = "Frozen fish and seafood",
                               DisplayLevel = "7",
                               ItemCode = "SS07021",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "58",
                               ItemName = "Fresh whole milk",
                               DisplayLevel = "5",
                               ItemCode = "SS09011",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "59",
                               ItemName = "Fresh milk other than whole",
                               DisplayLevel = "5",
                               ItemCode = "SS09021",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "101",
                               ItemName = "Butter",
                               DisplayLevel = "6",
                               ItemCode = "SS10011",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "69",
                               ItemName = "Oranges, including tangerines",
                               DisplayLevel = "7",
                               ItemCode = "SS11031",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "78",
                               ItemName = "Canned fruits",
                               DisplayLevel = "6",
                               ItemCode = "SS13031",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "81",
                               ItemName = "Frozen vegetables",
                               DisplayLevel = "6",
                               ItemCode = "SS14011",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "79",
                               ItemName = "Canned vegetables",
                               DisplayLevel = "6",
                               ItemCode = "SS14021",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "83",
                               ItemName = "Dried beans, peas, and lentils",
                               DisplayLevel = "6",
                               ItemCode = "SS14022",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "102",
                               ItemName = "Margarine",
                               DisplayLevel = "6",
                               ItemCode = "SS16011",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "105",
                               ItemName = "Peanut butter",
                               DisplayLevel = "6",
                               ItemCode = "SS16014",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "91",
                               ItemName = "Roasted coffee",
                               DisplayLevel = "6",
                               ItemCode = "SS17031",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "92",
                               ItemName = "Instant and freeze dried coffee",
                               DisplayLevel = "6",
                               ItemCode = "SS17032",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "111",
                               ItemName = "Salt and other seasonings and spices",
                               DisplayLevel = "6",
                               ItemCode = "SS18041",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "112",
                               ItemName = "Olives, pickles, relishes",
                               DisplayLevel = "6",
                               ItemCode = "SS18042",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "113",
                               ItemName = "Sauces and gravies",
                               DisplayLevel = "6",
                               ItemCode = "SS18043",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "114",
                               ItemName = "Other condiments",
                               DisplayLevel = "6",
                               ItemCode = "SS1804B",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "117",
                               ItemName = "Prepared salads",
                               DisplayLevel = "6",
                               ItemCode = "SS18064",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "129",
                               ItemName = "Whiskey at home",
                               DisplayLevel = "5",
                               ItemCode = "SS20021",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "130",
                               ItemName = "Distilled spirits, excluding whiskey, at home",
                               DisplayLevel = "5",
                               ItemCode = "SS20022",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "133",
                               ItemName = "Beer, ale, and other malt beverages away from home",
                               DisplayLevel = "4",
                               ItemCode = "SS20051",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "134",
                               ItemName = "Wine away from home",
                               DisplayLevel = "4",
                               ItemCode = "SS20052",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "135",
                               ItemName = "Distilled spirits away from home",
                               DisplayLevel = "4",
                               ItemCode = "SS20053",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "328",
                               ItemName = "Land-line interstate toll calls",
                               DisplayLevel = "5",
                               ItemCode = "SS27051",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "329",
                               ItemName = "Land-line intrastate toll calls",
                               DisplayLevel = "5",
                               ItemCode = "SS27061",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "168",
                               ItemName = "Laundry equipment",
                               DisplayLevel = "4",
                               ItemCode = "SS30021",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "275",
                               ItemName = "Video discs and other media",
                               DisplayLevel = "3",
                               ItemCode = "SS31022",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "299",
                               ItemName = "Video game hardware, software and accessories",
                               DisplayLevel = "3",
                               ItemCode = "SS31023",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "354",
                               ItemName = "Stationery, stationery supplies, gift wrap",
                               DisplayLevel = "3",
                               ItemCode = "SS33032",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "215",
                               ItemName = "New cars",
                               DisplayLevel = "4",
                               ItemCode = "SS45011",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "214",
                               ItemName = "New cars and trucks",
                               DisplayLevel = "4",
                               ItemCode = "SS4501A",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "216",
                               ItemName = "New trucks",
                               DisplayLevel = "4",
                               ItemCode = "SS45021",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "217",
                               ItemName = "New motorcycles",
                               DisplayLevel = "4",
                               ItemCode = "SS45031",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "223",
                               ItemName = "Gasoline, unleaded regular",
                               DisplayLevel = "4",
                               ItemCode = "SS47014",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "224",
                               ItemName = "Gasoline, unleaded midgrade",
                               DisplayLevel = "4",
                               ItemCode = "SS47015",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "225",
                               ItemName = "Gasoline, unleaded premium",
                               DisplayLevel = "4",
                               ItemCode = "SS47016",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "231",
                               ItemName = "Motor oil, coolant, and fluids",
                               DisplayLevel = "4",
                               ItemCode = "SS47021",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "230",
                               ItemName = "Vehicle parts and equipment other than tires",
                               DisplayLevel = "4",
                               ItemCode = "SS48021",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "240",
                               ItemName = "Parking fees and tolls",
                               DisplayLevel = "4",
                               ItemCode = "SS52051",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "245",
                               ItemName = "Intercity bus fare",
                               DisplayLevel = "3",
                               ItemCode = "SS53021",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "246",
                               ItemName = "Intercity train fare",
                               DisplayLevel = "3",
                               ItemCode = "SS53022",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "247",
                               ItemName = "Ship fare",
                               DisplayLevel = "3",
                               ItemCode = "SS53023",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "249",
                               ItemName = "Intracity mass transit",
                               DisplayLevel = "3",
                               ItemCode = "SS53031",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "264",
                               ItemName = "Inpatient hospital services",
                               DisplayLevel = "4",
                               ItemCode = "SS5702",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "265",
                               ItemName = "Outpatient hospital services",
                               DisplayLevel = "4",
                               ItemCode = "SS5703",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "298",
                               ItemName = "Toys, games, hobbies and playground equipment",
                               DisplayLevel = "3",
                               ItemCode = "SS61011",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "291",
                               ItemName = "Film and photographic supplies",
                               DisplayLevel = "3",
                               ItemCode = "SS61021",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "292",
                               ItemName = "Photographic equipment",
                               DisplayLevel = "3",
                               ItemCode = "SS61023",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "281",
                               ItemName = "Pet food",
                               DisplayLevel = "3",
                               ItemCode = "SS61031",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "282",
                               ItemName = "Purchase of pets, pet supplies, accessories",
                               DisplayLevel = "3",
                               ItemCode = "SS61032",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "241",
                               ItemName = "Automobile service clubs",
                               DisplayLevel = "4",
                               ItemCode = "SS62011",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "305",
                               ItemName = "Admission to movies, theaters, and concerts",
                               DisplayLevel = "3",
                               ItemCode = "SS62031",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "306",
                               ItemName = "Admission to sporting events",
                               DisplayLevel = "3",
                               ItemCode = "SS62032",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "294",
                               ItemName = "Photographer fees",
                               DisplayLevel = "3",
                               ItemCode = "SS62051",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "295",
                               ItemName = "Film processing",
                               DisplayLevel = "3",
                               ItemCode = "SS62052",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "284",
                               ItemName = "Pet services",
                               DisplayLevel = "3",
                               ItemCode = "SS62053",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "285",
                               ItemName = "Veterinarian services",
                               DisplayLevel = "3",
                               ItemCode = "SS62054",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "276",
                               ItemName = "Rental of video or audio discs and other media",
                               DisplayLevel = "3",
                               ItemCode = "SS62055",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "351",
                               ItemName = "Checking account and other bank services",
                               DisplayLevel = "4",
                               ItemCode = "SS68021",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "352",
                               ItemName = "Tax return preparation and other accounting fees",
                               DisplayLevel = "4",
                               ItemCode = "SS68023",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "314",
                               ItemName = "College textbooks",
                               DisplayLevel = "3",
                               ItemCode = "SSEA011",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "122",
                               ItemName = "Food at elementary and secondary schools",
                               DisplayLevel = "3",
                               ItemCode = "SSFV031A",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "355",
                               ItemName = "Infants' equipment",
                               DisplayLevel = "3",
                               ItemCode = "SSGE013",
                               Selectable = "T",
                           },
                           new CuItem
                           {
                               SortSequence = "165",
                               ItemName = "Infants' furniture",
                               DisplayLevel = "3",
                               ItemCode = "SSHJ031",
                               Selectable = "T",
                           },

                       };
                return _values;
            }
        }
	}//end CuItem
}//end NoFuture.Rand.Gov.Bls.Codes