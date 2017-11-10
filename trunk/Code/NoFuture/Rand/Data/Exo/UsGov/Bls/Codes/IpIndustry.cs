using System;
using System.Collections.Generic;
namespace NoFuture.Rand.Data.Exo.UsGov.Bls.Codes
{
    public class IpIndustry 
    {
        public string DisplayLevel { get; set; }
        public string IndustryCode { get; set; }
        public string IndustryText { get; set; }
        public string NaicsCode { get; set; }
        public string Selectable { get; set; }
        public string SortSequence { get; set; }
        private static List<IpIndustry> _values;
        public static List<IpIndustry> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<IpIndustry>
                           {
                           
                           new IpIndustry
                           {
                               IndustryText = "Mining",
                               Selectable = "F",
                               DisplayLevel = "0",
                               SortSequence = "1",
                               IndustryCode = "D21____",
                               NaicsCode = "D21",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Manufacturing",
                               Selectable = "F",
                               DisplayLevel = "0",
                               SortSequence = "21",
                               IndustryCode = "D31_33_",
                               NaicsCode = "D31,32,33",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Wholesale trade",
                               Selectable = "F",
                               DisplayLevel = "0",
                               SortSequence = "236",
                               IndustryCode = "D42____",
                               NaicsCode = "D42",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Retail trade",
                               Selectable = "F",
                               DisplayLevel = "0",
                               SortSequence = "262",
                               IndustryCode = "D44_45_",
                               NaicsCode = "D44,45",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Transportation and warehouseing",
                               Selectable = "F",
                               DisplayLevel = "0",
                               SortSequence = "377",
                               IndustryCode = "D48_49_",
                               NaicsCode = "D48,49",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Motion picture and sound recording industries",
                               Selectable = "F",
                               DisplayLevel = "2",
                               SortSequence = "410",
                               IndustryCode = "D512___",
                               NaicsCode = "D512",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Telecommunications",
                               Selectable = "F",
                               DisplayLevel = "2",
                               SortSequence = "418",
                               IndustryCode = "D517___",
                               NaicsCode = "D517",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Information",
                               Selectable = "F",
                               DisplayLevel = "0",
                               SortSequence = "398",
                               IndustryCode = "D51____",
                               NaicsCode = "D51",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Finance and insurance",
                               Selectable = "F",
                               DisplayLevel = "0",
                               SortSequence = "425",
                               IndustryCode = "D52____",
                               NaicsCode = "D52",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Real estate and rental and leasing",
                               Selectable = "F",
                               DisplayLevel = "0",
                               SortSequence = "428",
                               IndustryCode = "D53____",
                               NaicsCode = "D53",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Photographic Services",
                               Selectable = "F",
                               DisplayLevel = "4",
                               SortSequence = "445",
                               IndustryCode = "D54192_",
                               NaicsCode = "D54192",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Employment services",
                               Selectable = "F",
                               DisplayLevel = "3",
                               SortSequence = "448",
                               IndustryCode = "D5613__",
                               NaicsCode = "D5613",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Services to buildings and dwellings",
                               Selectable = "F",
                               DisplayLevel = "3",
                               SortSequence = "453",
                               IndustryCode = "D5617__",
                               NaicsCode = "D5617",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Administrative and support and waste management and remediation services",
                               Selectable = "F",
                               DisplayLevel = "0",
                               SortSequence = "447",
                               IndustryCode = "D56____",
                               NaicsCode = "D56",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Health care and social assistance",
                               Selectable = "F",
                               DisplayLevel = "0",
                               SortSequence = "456",
                               IndustryCode = "D62____",
                               NaicsCode = "D62",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Arts, entertainment, and recreation",
                               Selectable = "F",
                               DisplayLevel = "0",
                               SortSequence = "461",
                               IndustryCode = "D71____",
                               NaicsCode = "D71",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Accommodation and food services",
                               Selectable = "F",
                               DisplayLevel = "0",
                               SortSequence = "471",
                               IndustryCode = "D72____",
                               NaicsCode = "D72",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Death care services",
                               Selectable = "F",
                               DisplayLevel = "3",
                               SortSequence = "492",
                               IndustryCode = "D8122__",
                               NaicsCode = "D8122",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other personal services",
                               Selectable = "F",
                               DisplayLevel = "3",
                               SortSequence = "501",
                               IndustryCode = "D8129__",
                               NaicsCode = "D8129",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other services (except public administration)",
                               Selectable = "F",
                               DisplayLevel = "0",
                               SortSequence = "486",
                               IndustryCode = "D81____",
                               NaicsCode = "D81",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Oil and gas extraction",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "5",
                               IndustryCode = "N21111_",
                               NaicsCode = "21111",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Oil and gas extraction",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "4",
                               IndustryCode = "N2111__",
                               NaicsCode = "2111",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Oil and gas extraction",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "3",
                               IndustryCode = "N211___",
                               NaicsCode = "211",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Coal mining",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "8",
                               IndustryCode = "N21211_",
                               NaicsCode = "21211",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Coal mining",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "7",
                               IndustryCode = "N2121__",
                               NaicsCode = "2121",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Metal ore mining",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "9",
                               IndustryCode = "N2122__",
                               NaicsCode = "2122",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Nonmetallic mineral mining and quarrying",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "10",
                               IndustryCode = "N2123__",
                               NaicsCode = "2123",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Mining (except oil and gas)",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "6",
                               IndustryCode = "N212___",
                               NaicsCode = "212",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Support activities for mining",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "13",
                               IndustryCode = "N21311_",
                               NaicsCode = "21311",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Support activities for mining",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "12",
                               IndustryCode = "N2131__",
                               NaicsCode = "2131",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Support activities for mining",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "11",
                               IndustryCode = "N213___",
                               NaicsCode = "213",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Mining",
                               Selectable = "T",
                               DisplayLevel = "1",
                               SortSequence = "2",
                               IndustryCode = "N21____",
                               NaicsCode = "21",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Electric power generation, transmission and distribution",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "16",
                               IndustryCode = "N2211__",
                               NaicsCode = "2211",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Natural gas distribution",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "19",
                               IndustryCode = "N221210",
                               NaicsCode = "221210",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Natural gas distribution",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "18",
                               IndustryCode = "N22121_",
                               NaicsCode = "22121",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Natural gas distribution",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "17",
                               IndustryCode = "N2212__",
                               NaicsCode = "2212",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Water, sewage and other systems",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "20",
                               IndustryCode = "N2213__",
                               NaicsCode = "2213",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Utilities",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "15",
                               IndustryCode = "N221___",
                               NaicsCode = "221",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Utilities",
                               Selectable = "T",
                               DisplayLevel = "1",
                               SortSequence = "14",
                               IndustryCode = "N22____",
                               NaicsCode = "22",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Animal food manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "24",
                               IndustryCode = "N31111_",
                               NaicsCode = "31111",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Animal food manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "23",
                               IndustryCode = "N3111__",
                               NaicsCode = "3111",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Grain and oilseed milling",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "25",
                               IndustryCode = "N3112__",
                               NaicsCode = "3112",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Sugar and confectionery product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "26",
                               IndustryCode = "N3113__",
                               NaicsCode = "3113",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Frozen food manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "28",
                               IndustryCode = "N31141_",
                               NaicsCode = "31141",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Fruit and vegetable canning, pickling, and drying",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "29",
                               IndustryCode = "N31142_",
                               NaicsCode = "31142",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Fruit and vegetable preserving and specialty food manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "27",
                               IndustryCode = "N3114__",
                               NaicsCode = "3114",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Dairy product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "30",
                               IndustryCode = "N3115__",
                               NaicsCode = "3115",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Animal (except poultry) slaughtering",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "33",
                               IndustryCode = "N311611",
                               NaicsCode = "311611",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Poultry processing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "34",
                               IndustryCode = "N311615",
                               NaicsCode = "311615",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Animal slaughtering and processing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "32",
                               IndustryCode = "N31161_",
                               NaicsCode = "31161",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Animal slaughtering and processing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "31",
                               IndustryCode = "N3116__",
                               NaicsCode = "3116",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Seafood product preparation and packaging",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "36",
                               IndustryCode = "N31171_",
                               NaicsCode = "31171",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Seafood product preparation and packaging",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "35",
                               IndustryCode = "N3117__",
                               NaicsCode = "3117",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Retail bakeries",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "39",
                               IndustryCode = "N311811",
                               NaicsCode = "311811",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Bread and bakery product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "38",
                               IndustryCode = "N31181_",
                               NaicsCode = "31181",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Bakeries and tortilla manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "37",
                               IndustryCode = "N3118__",
                               NaicsCode = "3118",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other food manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "40",
                               IndustryCode = "N3119__",
                               NaicsCode = "3119",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Food manufacturing",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "22",
                               IndustryCode = "N311___",
                               NaicsCode = "311",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Beverage manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "42",
                               IndustryCode = "N3121__",
                               NaicsCode = "3121",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Tobacco manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "43",
                               IndustryCode = "N3122__",
                               NaicsCode = "3122",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Beverage and tobacco product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "41",
                               IndustryCode = "N312___",
                               NaicsCode = "312",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Fiber, yarn, and thread mills",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "46",
                               IndustryCode = "N31311_",
                               NaicsCode = "31311",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Fiber, yarn, and thread mills",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "45",
                               IndustryCode = "N3131__",
                               NaicsCode = "3131",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Fabric mills",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "47",
                               IndustryCode = "N3132__",
                               NaicsCode = "3132",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Textile and fabric finishing and fabric coating mills",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "48",
                               IndustryCode = "N3133__",
                               NaicsCode = "3133",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Textile mills",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "44",
                               IndustryCode = "N313___",
                               NaicsCode = "313",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Textile furnishings mills",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "50",
                               IndustryCode = "N3141__",
                               NaicsCode = "3141",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other textile product mills",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "51",
                               IndustryCode = "N3149__",
                               NaicsCode = "3149",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Textile product mills",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "49",
                               IndustryCode = "N314___",
                               NaicsCode = "314",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Apparel knitting mills",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "53",
                               IndustryCode = "N3151__",
                               NaicsCode = "3151",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Cut and sew apparel manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "54",
                               IndustryCode = "N3152__",
                               NaicsCode = "3152",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Apparel accessories and other apparel manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "56",
                               IndustryCode = "N31599_",
                               NaicsCode = "31599",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Apparel accessories and other apparel manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "55",
                               IndustryCode = "N3159__",
                               NaicsCode = "3159",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Apparel manufacturing",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "52",
                               IndustryCode = "N315___",
                               NaicsCode = "315",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Leather and hide tanning and finishing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "60",
                               IndustryCode = "N316110",
                               NaicsCode = "316110",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Leather and hide tanning and finishing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "59",
                               IndustryCode = "N31611_",
                               NaicsCode = "31611",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Leather and hide tanning and finishing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "58",
                               IndustryCode = "N3161__",
                               NaicsCode = "3161",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Footwear manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "62",
                               IndustryCode = "N31621_",
                               NaicsCode = "31621",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Footwear manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "61",
                               IndustryCode = "N3162__",
                               NaicsCode = "3162",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other leather and allied product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "64",
                               IndustryCode = "N31699_",
                               NaicsCode = "31699",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other leather and allied product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "63",
                               IndustryCode = "N3169__",
                               NaicsCode = "3169",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Leather and allied product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "57",
                               IndustryCode = "N316___",
                               NaicsCode = "316",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Sawmills and wood preservation",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "67",
                               IndustryCode = "N32111_",
                               NaicsCode = "32111",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Sawmills and wood preservation",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "66",
                               IndustryCode = "N3211__",
                               NaicsCode = "3211",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Veneer, plywood, and engineered wood product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "69",
                               IndustryCode = "N32121_",
                               NaicsCode = "32121",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Veneer, plywood, and engineered wood product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "68",
                               IndustryCode = "N3212__",
                               NaicsCode = "3212",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Wood windows and doors",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "72",
                               IndustryCode = "N321911",
                               NaicsCode = "321911",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Millwork",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "71",
                               IndustryCode = "N32191_",
                               NaicsCode = "32191",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Wood container and pallet manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "74",
                               IndustryCode = "N321920",
                               NaicsCode = "321920",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Wood container and pallet manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "73",
                               IndustryCode = "N32192_",
                               NaicsCode = "32192",
                           },
                           new IpIndustry
                           {
                               IndustryText = "All other wood product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "75",
                               IndustryCode = "N32199_",
                               NaicsCode = "32199",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other wood product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "70",
                               IndustryCode = "N3219__",
                               NaicsCode = "3219",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Wood product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "65",
                               IndustryCode = "N321___",
                               NaicsCode = "321",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Pulp, paper, and paperboard mills",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "77",
                               IndustryCode = "N3221__",
                               NaicsCode = "3221",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Corrugated and solid fiber boxes",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "80",
                               IndustryCode = "N322211",
                               NaicsCode = "322211",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Paperboard container manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "79",
                               IndustryCode = "N32221_",
                               NaicsCode = "32221",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Paper bag and coated and treated paper manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "81",
                               IndustryCode = "N32222_",
                               NaicsCode = "32222",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Converted paper product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "78",
                               IndustryCode = "N3222__",
                               NaicsCode = "3222",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Paper manufacturing",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "76",
                               IndustryCode = "N322___",
                               NaicsCode = "322",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Printing and related support activities",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "83",
                               IndustryCode = "N3231__",
                               NaicsCode = "3231",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Printing and related support activities",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "82",
                               IndustryCode = "N323___",
                               NaicsCode = "323",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Petroleum refineries",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "87",
                               IndustryCode = "N324110",
                               NaicsCode = "324110",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Petroleum refineries",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "86",
                               IndustryCode = "N32411_",
                               NaicsCode = "32411",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Petroleum and coal products manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "85",
                               IndustryCode = "N3241__",
                               NaicsCode = "3241",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Petroleum and coal products manufacturing",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "84",
                               IndustryCode = "N324___",
                               NaicsCode = "324",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Basic chemical manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "89",
                               IndustryCode = "N3251__",
                               NaicsCode = "3251",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Resin, synthetic rubber, and artificial synthetic fibers and filaments manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "90",
                               IndustryCode = "N3252__",
                               NaicsCode = "3252",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Pesticide, fertilizer, and other agricultural chemical manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "91",
                               IndustryCode = "N3253__",
                               NaicsCode = "3253",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Pharmaceutical and medicine manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "93",
                               IndustryCode = "N32541_",
                               NaicsCode = "32541",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Pharmaceutical and medicine manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "92",
                               IndustryCode = "N3254__",
                               NaicsCode = "3254",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Paint, coating, and adhesive manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "94",
                               IndustryCode = "N3255__",
                               NaicsCode = "3255",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Soap, cleaning compound, and toilet preparation manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "95",
                               IndustryCode = "N3256__",
                               NaicsCode = "3256",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other chemical product and preparation manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "96",
                               IndustryCode = "N3259__",
                               NaicsCode = "3259",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Chemical manufacturing",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "88",
                               IndustryCode = "N325___",
                               NaicsCode = "325",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Plastics packaging materials and unlaminated film and sheet manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "99",
                               IndustryCode = "N32611_",
                               NaicsCode = "32611",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Plastics pipe, pipe fitting, and unlaminated profile shape manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "100",
                               IndustryCode = "N32612_",
                               NaicsCode = "32612",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other plastics product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "101",
                               IndustryCode = "N32619_",
                               NaicsCode = "32619",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Plastics product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "98",
                               IndustryCode = "N3261__",
                               NaicsCode = "3261",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Rubber product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "102",
                               IndustryCode = "N3262__",
                               NaicsCode = "3262",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Plastics and rubber products manufacturing",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "97",
                               IndustryCode = "N326___",
                               NaicsCode = "326",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Clay product and refractory manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "104",
                               IndustryCode = "N3271__",
                               NaicsCode = "3271",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Glass and glass product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "106",
                               IndustryCode = "N32721_",
                               NaicsCode = "32721",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Glass and glass product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "105",
                               IndustryCode = "N3272__",
                               NaicsCode = "3272",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Ready-mix concrete manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "109",
                               IndustryCode = "N327320",
                               NaicsCode = "327320",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Ready-mix concrete manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "108",
                               IndustryCode = "N32732_",
                               NaicsCode = "32732",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Cement and concrete product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "107",
                               IndustryCode = "N3273__",
                               NaicsCode = "3273",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Lime and gypsum product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "110",
                               IndustryCode = "N3274__",
                               NaicsCode = "3274",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other nonmetallic mineral product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "111",
                               IndustryCode = "N3279__",
                               NaicsCode = "3279",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Nonmetallic mineral product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "103",
                               IndustryCode = "N327___",
                               NaicsCode = "327",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Iron and steel mills and ferroalloy manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "114",
                               IndustryCode = "N33111_",
                               NaicsCode = "33111",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Iron and steel mills and ferroalloy manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "113",
                               IndustryCode = "N3311__",
                               NaicsCode = "3311",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Steel product manufacturing from purchased steel",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "115",
                               IndustryCode = "N3312__",
                               NaicsCode = "3312",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Alumina and aluminum production and processing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "117",
                               IndustryCode = "N33131_",
                               NaicsCode = "33131",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Alumina and aluminum production and processing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "116",
                               IndustryCode = "N3313__",
                               NaicsCode = "3313",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Nonferrous metal (except aluminum) production and processing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "118",
                               IndustryCode = "N3314__",
                               NaicsCode = "3314",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Foundries",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "119",
                               IndustryCode = "N3315__",
                               NaicsCode = "3315",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Primary metal manufacturing",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "112",
                               IndustryCode = "N331___",
                               NaicsCode = "331",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Forging and stamping",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "122",
                               IndustryCode = "N33211_",
                               NaicsCode = "33211",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Forging and stamping",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "121",
                               IndustryCode = "N3321__",
                               NaicsCode = "3321",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Cutlery and handtool manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "124",
                               IndustryCode = "N33221_",
                               NaicsCode = "33221",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Cutlery and handtool manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "123",
                               IndustryCode = "N3322__",
                               NaicsCode = "3322",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Fabricated structural metals",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "127",
                               IndustryCode = "N332312",
                               NaicsCode = "332312",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Plate work and fabricated structural product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "126",
                               IndustryCode = "N33231_",
                               NaicsCode = "33231",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Metal windows and doors",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "129",
                               IndustryCode = "N332321",
                               NaicsCode = "332321",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Sheet metal work",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "130",
                               IndustryCode = "N332322",
                               NaicsCode = "332322",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Ornamental and architectural metal work",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "131",
                               IndustryCode = "N332323",
                               NaicsCode = "332323",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Ornamental and architectural metal products manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "128",
                               IndustryCode = "N33232_",
                               NaicsCode = "33232",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Architectural and structural metals manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "125",
                               IndustryCode = "N3323__",
                               NaicsCode = "3323",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Boiler, tank, and shipping container manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "132",
                               IndustryCode = "N3324__",
                               NaicsCode = "3324",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Hardware manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "135",
                               IndustryCode = "N332510",
                               NaicsCode = "332510",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Hardware manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "134",
                               IndustryCode = "N33251_",
                               NaicsCode = "33251",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Hardware manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "133",
                               IndustryCode = "N3325__",
                               NaicsCode = "3325",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Spring and wire product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "137",
                               IndustryCode = "N33261_",
                               NaicsCode = "33261",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Spring and wire product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "136",
                               IndustryCode = "N3326__",
                               NaicsCode = "3326",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Machine shops",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "140",
                               IndustryCode = "N332710",
                               NaicsCode = "332710",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Machine shops",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "139",
                               IndustryCode = "N33271_",
                               NaicsCode = "33271",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Turned product and screw, nut, and bolt manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "141",
                               IndustryCode = "N33272_",
                               NaicsCode = "33272",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Machine shops; turned product; and screw, nut, and bolt manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "138",
                               IndustryCode = "N3327__",
                               NaicsCode = "3327",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Electroplating, plating, polishing, anodizing and coloring",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "144",
                               IndustryCode = "N332813",
                               NaicsCode = "332813",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Coating, engraving, heat treating, and allied activities",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "143",
                               IndustryCode = "N33281_",
                               NaicsCode = "33281",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Coating, engraving, heat treating, and allied activities",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "142",
                               IndustryCode = "N3328__",
                               NaicsCode = "3328",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Metal valve manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "146",
                               IndustryCode = "N33291_",
                               NaicsCode = "33291",
                           },
                           new IpIndustry
                           {
                               IndustryText = "All other fabricated metal product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "147",
                               IndustryCode = "N33299_",
                               NaicsCode = "33299",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other fabricated metal product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "145",
                               IndustryCode = "N3329__",
                               NaicsCode = "3329",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Fabricated metal product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "120",
                               IndustryCode = "N332___",
                               NaicsCode = "332",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Farm machinery and equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "150",
                               IndustryCode = "N333111",
                               NaicsCode = "333111",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Agriculture, construction, and mining machinery",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "149",
                               IndustryCode = "N3331__",
                               NaicsCode = "3331",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Industrial machinery manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "151",
                               IndustryCode = "N3332__",
                               NaicsCode = "3332",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Commercial and service industry machinery",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "153",
                               IndustryCode = "N33331_",
                               NaicsCode = "33331",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Commercial and service industry machinery manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "152",
                               IndustryCode = "N3333__",
                               NaicsCode = "3333",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Ventilation, heating, air-conditioning, and commercial refrigeration equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "155",
                               IndustryCode = "N33341_",
                               NaicsCode = "33341",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Ventilation, heating, air-conditioning, and commercial refrigeration equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "154",
                               IndustryCode = "N3334__",
                               NaicsCode = "3334",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Special die and tool, die set, jig, and fixture manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "158",
                               IndustryCode = "N333514",
                               NaicsCode = "333514",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Machine tool manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "159",
                               IndustryCode = "N333517",
                               NaicsCode = "333517",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Metalworking machinery manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "157",
                               IndustryCode = "N33351_",
                               NaicsCode = "33351",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Metalworking machinery manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "156",
                               IndustryCode = "N3335__",
                               NaicsCode = "3335",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Engine, turbine, and power transmission equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "161",
                               IndustryCode = "N33361_",
                               NaicsCode = "33361",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Engine, turbine, and power transmission equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "160",
                               IndustryCode = "N3336__",
                               NaicsCode = "3336",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Pump and compressor manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "163",
                               IndustryCode = "N33391_",
                               NaicsCode = "33391",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Material handling equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "164",
                               IndustryCode = "N33392_",
                               NaicsCode = "33392",
                           },
                           new IpIndustry
                           {
                               IndustryText = "All other general purpose machinery manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "165",
                               IndustryCode = "N33399_",
                               NaicsCode = "33399",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other general purpose machinery manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "162",
                               IndustryCode = "N3339__",
                               NaicsCode = "3339",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Machinery manufacturing",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "148",
                               IndustryCode = "N333___",
                               NaicsCode = "333",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Computer and peripheral equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "168",
                               IndustryCode = "N33411_",
                               NaicsCode = "33411",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Computer and peripheral equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "167",
                               IndustryCode = "N3341__",
                               NaicsCode = "3341",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Radio and television broadcasting and wireless communications equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "171",
                               IndustryCode = "N334220",
                               NaicsCode = "334220",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Radio and television broadcasting and wireless communications equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "170",
                               IndustryCode = "N33422_",
                               NaicsCode = "33422",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Communications equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "169",
                               IndustryCode = "N3342__",
                               NaicsCode = "3342",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Audio and video equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "174",
                               IndustryCode = "N334310",
                               NaicsCode = "334310",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Audio and video equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "173",
                               IndustryCode = "N33431_",
                               NaicsCode = "33431",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Audio and video equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "172",
                               IndustryCode = "N3343__",
                               NaicsCode = "3343",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Semiconductor and related device manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "177",
                               IndustryCode = "N334413",
                               NaicsCode = "334413",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Semiconductor and other electronic component manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "176",
                               IndustryCode = "N33441_",
                               NaicsCode = "33441",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Semiconductor and other electronic component manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "175",
                               IndustryCode = "N3344__",
                               NaicsCode = "3344",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Industrial process variable instruments",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "180",
                               IndustryCode = "N334513",
                               NaicsCode = "334513",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Navigational, measuring, electromedical, and control instruments manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "179",
                               IndustryCode = "N33451_",
                               NaicsCode = "33451",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Navigational, measuring, electromedical, and control instruments manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "178",
                               IndustryCode = "N3345__",
                               NaicsCode = "3345",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Manufacturing and reproducing magnetic and optical media",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "182",
                               IndustryCode = "N33461_",
                               NaicsCode = "33461",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Manufacturing and reproducing magnetic and optical media",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "181",
                               IndustryCode = "N3346__",
                               NaicsCode = "3346",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Computer and electronic product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "166",
                               IndustryCode = "N334___",
                               NaicsCode = "334",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Electric lighting equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "184",
                               IndustryCode = "N3351__",
                               NaicsCode = "3351",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Household appliance manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "185",
                               IndustryCode = "N3352__",
                               NaicsCode = "3352",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Electrical equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "187",
                               IndustryCode = "N33531_",
                               NaicsCode = "33531",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Electrical equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "186",
                               IndustryCode = "N3353__",
                               NaicsCode = "3353",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other electrical equipment and component manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "188",
                               IndustryCode = "N3359__",
                               NaicsCode = "3359",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Electrical equipment, appliance, and component manufacturing",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "183",
                               IndustryCode = "N335___",
                               NaicsCode = "335",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Automobile manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "192",
                               IndustryCode = "N336111",
                               NaicsCode = "336111",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Light truck and utility vehicle manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "193",
                               IndustryCode = "N336112",
                               NaicsCode = "336112",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Automobile and light duty motor vehicle manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "191",
                               IndustryCode = "N33611_",
                               NaicsCode = "33611",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Heavy duty truck manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "195",
                               IndustryCode = "N336120",
                               NaicsCode = "336120",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Heavy duty truck manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "194",
                               IndustryCode = "N33612_",
                               NaicsCode = "33612",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Motor vehicle manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "190",
                               IndustryCode = "N3361__",
                               NaicsCode = "3361",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Motor vehicle body manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "198",
                               IndustryCode = "N336211",
                               NaicsCode = "336211",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Motor vehicle body and trailer manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "197",
                               IndustryCode = "N33621_",
                               NaicsCode = "33621",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Motor vehicle body and trailer manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "196",
                               IndustryCode = "N3362__",
                               NaicsCode = "3362",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Motor vehicle gasoline engine and engine parts manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "200",
                               IndustryCode = "N33631_",
                               NaicsCode = "33631",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Motor vehicle electrical and electronic equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "201",
                               IndustryCode = "N33632_",
                               NaicsCode = "33632",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Motor vehicle transmission and power train parts manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "203",
                               IndustryCode = "N336350",
                               NaicsCode = "336350",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Motor vehicle transmission and power train parts manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "202",
                               IndustryCode = "N33635_",
                               NaicsCode = "33635",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Motor vehicle metal stamping",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "205",
                               IndustryCode = "N336370",
                               NaicsCode = "336370",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Motor vehicle metal stamping",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "204",
                               IndustryCode = "N33637_",
                               NaicsCode = "33637",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other motor vehicle parts manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "206",
                               IndustryCode = "N33639_",
                               NaicsCode = "33639",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Motor vehicle parts manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "199",
                               IndustryCode = "N3363__",
                               NaicsCode = "3363",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Aircraft manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "209",
                               IndustryCode = "N336411",
                               NaicsCode = "336411",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Aerospace product and parts manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "208",
                               IndustryCode = "N33641_",
                               NaicsCode = "33641",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Aerospace product and parts manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "207",
                               IndustryCode = "N3364__",
                               NaicsCode = "3364",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Railroad rolling stock manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "212",
                               IndustryCode = "N336510",
                               NaicsCode = "336510",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Railroad rolling stock manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "211",
                               IndustryCode = "N33651_",
                               NaicsCode = "33651",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Railroad rolling stock manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "210",
                               IndustryCode = "N3365__",
                               NaicsCode = "3365",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Ship and boat building",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "214",
                               IndustryCode = "N33661_",
                               NaicsCode = "33661",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Ship and boat building",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "213",
                               IndustryCode = "N3366__",
                               NaicsCode = "3366",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other transportation equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "216",
                               IndustryCode = "N33699_",
                               NaicsCode = "33699",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other transportation equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "215",
                               IndustryCode = "N3369__",
                               NaicsCode = "3369",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Transportation equipment manufacturing",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "189",
                               IndustryCode = "N336___",
                               NaicsCode = "336",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Wood kitchen cabinet and countertop manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "220",
                               IndustryCode = "N337110",
                               NaicsCode = "337110",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Wood kitchen cabinet and countertop manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "219",
                               IndustryCode = "N33711_",
                               NaicsCode = "33711",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Upholstered household furniture manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "222",
                               IndustryCode = "N337121",
                               NaicsCode = "337121",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Nonupholstered wood household furniture manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "223",
                               IndustryCode = "N337122",
                               NaicsCode = "337122",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Household and institutional furniture manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "221",
                               IndustryCode = "N33712_",
                               NaicsCode = "33712",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Household and institutional furniture and kitchen cabinet manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "218",
                               IndustryCode = "N3371__",
                               NaicsCode = "3371",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Showcase, partition, shelving, and locker manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "226",
                               IndustryCode = "N337215",
                               NaicsCode = "337215",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Office furniture (including fixtures) manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "225",
                               IndustryCode = "N33721_",
                               NaicsCode = "33721",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Office furniture (including fixtures) manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "224",
                               IndustryCode = "N3372__",
                               NaicsCode = "3372",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other furniture related product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "227",
                               IndustryCode = "N3379__",
                               NaicsCode = "3379",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Furniture and related product manufacturing",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "217",
                               IndustryCode = "N337___",
                               NaicsCode = "337",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Surgical appliance and supplies manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "231",
                               IndustryCode = "N339113",
                               NaicsCode = "339113",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Dental laboratories",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "232",
                               IndustryCode = "N339116",
                               NaicsCode = "339116",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Medical equipment and supplies manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "230",
                               IndustryCode = "N33911_",
                               NaicsCode = "33911",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Medical equipment and supplies manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "229",
                               IndustryCode = "N3391__",
                               NaicsCode = "3391",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Sign manufacturing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "235",
                               IndustryCode = "N339950",
                               NaicsCode = "339950",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Sign manufacturing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "234",
                               IndustryCode = "N33995_",
                               NaicsCode = "33995",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other miscellaneous manufacturing",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "233",
                               IndustryCode = "N3399__",
                               NaicsCode = "3399",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Miscellaneous manufacturing",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "228",
                               IndustryCode = "N339___",
                               NaicsCode = "339",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Motor vehicle and motor vehicle parts and supplies merchant wholesalers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "239",
                               IndustryCode = "N4231__",
                               NaicsCode = "4231",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Furniture and home furnishing merchant wholesalers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "240",
                               IndustryCode = "N4232__",
                               NaicsCode = "4232",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Lumber and other construction materials merchant wholesalers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "241",
                               IndustryCode = "N4233__",
                               NaicsCode = "4233",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Professional and commercial equipment and supplies merchant wholesalers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "242",
                               IndustryCode = "N4234__",
                               NaicsCode = "4234",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Metal and mineral (except petroleum) merchant wholesalers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "243",
                               IndustryCode = "N4235__",
                               NaicsCode = "4235",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Electrical and electronic goods merchant wholesalers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "244",
                               IndustryCode = "N4236__",
                               NaicsCode = "4236",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Hardware, and plumbing and heating equipment and supplies merchant wholesalers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "245",
                               IndustryCode = "N4237__",
                               NaicsCode = "4237",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Machinery, equipment, and supplies merchant wholesalers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "246",
                               IndustryCode = "N4238__",
                               NaicsCode = "4238",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Miscellaneous durable goods merchant wholesalers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "247",
                               IndustryCode = "N4239__",
                               NaicsCode = "4239",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Merchant wholesalers, durable goods",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "238",
                               IndustryCode = "N423___",
                               NaicsCode = "423",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Paper and paper product merchant wholesalers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "249",
                               IndustryCode = "N4241__",
                               NaicsCode = "4241",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Drugs and druggists' sundries merchant wholesalers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "252",
                               IndustryCode = "N424210",
                               NaicsCode = "424210",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Drugs and druggists' sundries merchant wholesalers",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "251",
                               IndustryCode = "N42421_",
                               NaicsCode = "42421",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Drugs and druggists' sundries merchant wholesalers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "250",
                               IndustryCode = "N4242__",
                               NaicsCode = "4242",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Apparel, piece goods, and notions merchant wholesalers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "253",
                               IndustryCode = "N4243__",
                               NaicsCode = "4243",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Grocery and related product wholesalers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "254",
                               IndustryCode = "N4244__",
                               NaicsCode = "4244",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Farm product raw material merchant wholesalers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "255",
                               IndustryCode = "N4245__",
                               NaicsCode = "4245",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Chemical and allied products merchant wholesalers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "256",
                               IndustryCode = "N4246__",
                               NaicsCode = "4246",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Petroleum and petroleum products merchant wholesalers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "257",
                               IndustryCode = "N4247__",
                               NaicsCode = "4247",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Beer, wine, and distilled alcoholic beverage merchant wholesalers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "258",
                               IndustryCode = "N4248__",
                               NaicsCode = "4248",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Miscellaneous nondurable goods merchant wholesalers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "259",
                               IndustryCode = "N4249__",
                               NaicsCode = "4249",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Merchant wholesalers, nondurable goods",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "248",
                               IndustryCode = "N424___",
                               NaicsCode = "424",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Wholesale electronic markets and agents and brokers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "261",
                               IndustryCode = "N4251__",
                               NaicsCode = "4251",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Wholesale electronic markets and agents and brokers",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "260",
                               IndustryCode = "N425___",
                               NaicsCode = "425",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Wholesale trade",
                               Selectable = "T",
                               DisplayLevel = "1",
                               SortSequence = "237",
                               IndustryCode = "N42____",
                               NaicsCode = "42",
                           },
                           new IpIndustry
                           {
                               IndustryText = "New car dealers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "267",
                               IndustryCode = "N441110",
                               NaicsCode = "441110",
                           },
                           new IpIndustry
                           {
                               IndustryText = "New car dealers",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "266",
                               IndustryCode = "N44111_",
                               NaicsCode = "44111",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Used car dealers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "269",
                               IndustryCode = "N441120",
                               NaicsCode = "441120",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Used car dealers",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "268",
                               IndustryCode = "N44112_",
                               NaicsCode = "44112",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Automobile dealers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "265",
                               IndustryCode = "N4411__",
                               NaicsCode = "4411",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other motor vehicle dealers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "270",
                               IndustryCode = "N4412__",
                               NaicsCode = "4412",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Automotive parts and accessories stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "273",
                               IndustryCode = "N441310",
                               NaicsCode = "441310",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Automotive parts and accessories stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "272",
                               IndustryCode = "N44131_",
                               NaicsCode = "44131",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Tire dealers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "275",
                               IndustryCode = "N441320",
                               NaicsCode = "441320",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Tire dealers",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "274",
                               IndustryCode = "N44132_",
                               NaicsCode = "44132",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Automotive parts, accessories, and tire stores",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "271",
                               IndustryCode = "N4413__",
                               NaicsCode = "4413",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Motor vehicle and parts dealers",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "264",
                               IndustryCode = "N441___",
                               NaicsCode = "441",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Furniture stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "279",
                               IndustryCode = "N442110",
                               NaicsCode = "442110",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Furniture stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "278",
                               IndustryCode = "N44211_",
                               NaicsCode = "44211",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Furniture stores",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "277",
                               IndustryCode = "N4421__",
                               NaicsCode = "4421",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Floor covering stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "282",
                               IndustryCode = "N442210",
                               NaicsCode = "442210",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Floor covering stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "281",
                               IndustryCode = "N44221_",
                               NaicsCode = "44221",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other home furnishings stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "283",
                               IndustryCode = "N44229_",
                               NaicsCode = "44229",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Home furnishings stores",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "280",
                               IndustryCode = "N4422__",
                               NaicsCode = "4422",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Furniture and home furnishings stores",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "276",
                               IndustryCode = "N442___",
                               NaicsCode = "442",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Household appliance stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "287",
                               IndustryCode = "N443141",
                               NaicsCode = "443141",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Electronics stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "288",
                               IndustryCode = "N443142",
                               NaicsCode = "443142",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Electronics and appliance stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "286",
                               IndustryCode = "N44314_",
                               NaicsCode = "44314",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Electronics and appliance stores",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "285",
                               IndustryCode = "N4431__",
                               NaicsCode = "4431",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Electronics and appliance stores",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "284",
                               IndustryCode = "N443___",
                               NaicsCode = "443",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Home centers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "292",
                               IndustryCode = "N444110",
                               NaicsCode = "444110",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Home centers",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "291",
                               IndustryCode = "N44411_",
                               NaicsCode = "44411",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Hardware stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "294",
                               IndustryCode = "N444130",
                               NaicsCode = "444130",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Hardware stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "293",
                               IndustryCode = "N44413_",
                               NaicsCode = "44413",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Building material and supplies dealers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "290",
                               IndustryCode = "N4441__",
                               NaicsCode = "4441",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Outdoor power equipment stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "297",
                               IndustryCode = "N444210",
                               NaicsCode = "444210",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Outdoor power equipment stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "296",
                               IndustryCode = "N44421_",
                               NaicsCode = "44421",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Nursery, garden center, and farm supply stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "299",
                               IndustryCode = "N444220",
                               NaicsCode = "444220",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Nursery, garden center, and farm supply stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "298",
                               IndustryCode = "N44422_",
                               NaicsCode = "44422",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Lawn and garden equipment and supplies stores",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "295",
                               IndustryCode = "N4442__",
                               NaicsCode = "4442",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Building material and garden equipment and supplies dealers",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "289",
                               IndustryCode = "N444___",
                               NaicsCode = "444",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Supermarkets and other grocery (except convenience) stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "303",
                               IndustryCode = "N445110",
                               NaicsCode = "445110",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Supermarkets and other grocery (except convenience) stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "302",
                               IndustryCode = "N44511_",
                               NaicsCode = "44511",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Convenience stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "305",
                               IndustryCode = "N445120",
                               NaicsCode = "445120",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Convenience stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "304",
                               IndustryCode = "N44512_",
                               NaicsCode = "44512",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Grocery stores",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "301",
                               IndustryCode = "N4451__",
                               NaicsCode = "4451",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Specialty food stores",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "306",
                               IndustryCode = "N4452__",
                               NaicsCode = "4452",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Beer, wine, and liquor stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "309",
                               IndustryCode = "N445310",
                               NaicsCode = "445310",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Beer, wine, and liquor stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "308",
                               IndustryCode = "N44531_",
                               NaicsCode = "44531",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Beer, wine, and liquor stores",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "307",
                               IndustryCode = "N4453__",
                               NaicsCode = "4453",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Food and beverage stores",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "300",
                               IndustryCode = "N445___",
                               NaicsCode = "445",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Pharmacies and drug stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "313",
                               IndustryCode = "N446110",
                               NaicsCode = "446110",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Pharmacies and drug stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "312",
                               IndustryCode = "N44611_",
                               NaicsCode = "44611",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Cosmetics, beauty supplies, and perfume stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "315",
                               IndustryCode = "N446120",
                               NaicsCode = "446120",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Cosmetics, beauty supplies, and perfume stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "314",
                               IndustryCode = "N44612_",
                               NaicsCode = "44612",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Optical goods stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "317",
                               IndustryCode = "N446130",
                               NaicsCode = "446130",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Optical goods stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "316",
                               IndustryCode = "N44613_",
                               NaicsCode = "44613",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other health and personal care stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "318",
                               IndustryCode = "N44619_",
                               NaicsCode = "44619",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Health and personal care stores",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "311",
                               IndustryCode = "N4461__",
                               NaicsCode = "4461",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Health and personal care stores",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "310",
                               IndustryCode = "N446___",
                               NaicsCode = "446",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Gasoline stations with convenience stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "322",
                               IndustryCode = "N447110",
                               NaicsCode = "447110",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Gasoline stations with convenience stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "321",
                               IndustryCode = "N44711_",
                               NaicsCode = "44711",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other gasoline stations",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "324",
                               IndustryCode = "N447190",
                               NaicsCode = "447190",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other gasoline stations",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "323",
                               IndustryCode = "N44719_",
                               NaicsCode = "44719",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Gasoline stations",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "320",
                               IndustryCode = "N4471__",
                               NaicsCode = "4471",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Gasoline stations",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "319",
                               IndustryCode = "N447___",
                               NaicsCode = "447",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Men's clothing stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "328",
                               IndustryCode = "N448110",
                               NaicsCode = "448110",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Men's clothing stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "327",
                               IndustryCode = "N44811_",
                               NaicsCode = "44811",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Women's clothing stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "330",
                               IndustryCode = "N448120",
                               NaicsCode = "448120",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Women's clothing stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "329",
                               IndustryCode = "N44812_",
                               NaicsCode = "44812",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Family clothing stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "332",
                               IndustryCode = "N448140",
                               NaicsCode = "448140",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Family clothing stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "331",
                               IndustryCode = "N44814_",
                               NaicsCode = "44814",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Clothing accessories stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "334",
                               IndustryCode = "N448150",
                               NaicsCode = "448150",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Clothing accessories stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "333",
                               IndustryCode = "N44815_",
                               NaicsCode = "44815",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Clothing stores",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "326",
                               IndustryCode = "N4481__",
                               NaicsCode = "4481",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Shoe stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "337",
                               IndustryCode = "N448210",
                               NaicsCode = "448210",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Shoe stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "336",
                               IndustryCode = "N44821_",
                               NaicsCode = "44821",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Shoe stores",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "335",
                               IndustryCode = "N4482__",
                               NaicsCode = "4482",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Jewelry, luggage, and leather goods stores",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "338",
                               IndustryCode = "N4483__",
                               NaicsCode = "4483",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Clothing and clothing accessories stores",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "325",
                               IndustryCode = "N448___",
                               NaicsCode = "448",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Retail trade",
                               Selectable = "T",
                               DisplayLevel = "1",
                               SortSequence = "263",
                               IndustryCode = "N44_45_",
                               NaicsCode = "44,45",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Sporting goods stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "342",
                               IndustryCode = "N451110",
                               NaicsCode = "451110",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Sporting goods stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "341",
                               IndustryCode = "N45111_",
                               NaicsCode = "45111",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Hobby, toy, and game stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "344",
                               IndustryCode = "N451120",
                               NaicsCode = "451120",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Hobby, toy, and game stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "343",
                               IndustryCode = "N45112_",
                               NaicsCode = "45112",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Sporting goods, hobby, and musical instrument stores",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "340",
                               IndustryCode = "N4511__",
                               NaicsCode = "4511",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Book, periodical, and music stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "346",
                               IndustryCode = "N45121_",
                               NaicsCode = "45121",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Book, periodical, and music stores",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "345",
                               IndustryCode = "N4512__",
                               NaicsCode = "4512",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Sporting goods, hobby, book, and music stores",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "339",
                               IndustryCode = "N451___",
                               NaicsCode = "451",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Department stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "349",
                               IndustryCode = "N45211_",
                               NaicsCode = "45211",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Department stores",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "348",
                               IndustryCode = "N4521__",
                               NaicsCode = "4521",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other general merchandise stores",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "350",
                               IndustryCode = "N4529__",
                               NaicsCode = "4529",
                           },
                           new IpIndustry
                           {
                               IndustryText = "General merchandise stores",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "347",
                               IndustryCode = "N452___",
                               NaicsCode = "452",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Florists",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "354",
                               IndustryCode = "N453110",
                               NaicsCode = "453110",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Florists",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "353",
                               IndustryCode = "N45311_",
                               NaicsCode = "45311",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Florists",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "352",
                               IndustryCode = "N4531__",
                               NaicsCode = "4531",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Office supplies and stationery stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "357",
                               IndustryCode = "N453210",
                               NaicsCode = "453210",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Office supplies and stationery stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "356",
                               IndustryCode = "N45321_",
                               NaicsCode = "45321",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Gift, novelty, and souvenir stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "359",
                               IndustryCode = "N453220",
                               NaicsCode = "453220",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Gift, novelty, and souvenir stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "358",
                               IndustryCode = "N45322_",
                               NaicsCode = "45322",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Office supplies, stationery, and gift stores",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "355",
                               IndustryCode = "N4532__",
                               NaicsCode = "4532",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Used merchandise stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "362",
                               IndustryCode = "N453310",
                               NaicsCode = "453310",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Used merchandise stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "361",
                               IndustryCode = "N45331_",
                               NaicsCode = "45331",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Used merchandise stores",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "360",
                               IndustryCode = "N4533__",
                               NaicsCode = "4533",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Pet and pet supplies stores",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "365",
                               IndustryCode = "N453910",
                               NaicsCode = "453910",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Pet and pet supplies stores",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "364",
                               IndustryCode = "N45391_",
                               NaicsCode = "45391",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other miscellaneous store retailers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "363",
                               IndustryCode = "N4539__",
                               NaicsCode = "4539",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Miscellaneous store retailers",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "351",
                               IndustryCode = "N453___",
                               NaicsCode = "453",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Electronic shopping and mail-order houses",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "368",
                               IndustryCode = "N45411_",
                               NaicsCode = "45411",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Electronic shopping and mail-order houses",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "367",
                               IndustryCode = "N4541__",
                               NaicsCode = "4541",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Vending machine operators",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "371",
                               IndustryCode = "N454210",
                               NaicsCode = "454210",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Vending machine operators",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "370",
                               IndustryCode = "N45421_",
                               NaicsCode = "45421",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Vending machine operators",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "369",
                               IndustryCode = "N4542__",
                               NaicsCode = "4542",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Fuel dealers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "374",
                               IndustryCode = "N454310",
                               NaicsCode = "454310",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Fuel dealers",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "373",
                               IndustryCode = "N45431_",
                               NaicsCode = "45431",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other direct selling establishments",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "376",
                               IndustryCode = "N454390",
                               NaicsCode = "454390",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other direct selling establishments",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "375",
                               IndustryCode = "N45439_",
                               NaicsCode = "45439",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Direct selling establishments",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "372",
                               IndustryCode = "N4543__",
                               NaicsCode = "4543",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Nonstore retailers",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "366",
                               IndustryCode = "N454___",
                               NaicsCode = "454",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Air transportation",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "378",
                               IndustryCode = "N481___",
                               NaicsCode = "481",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Line-haul railroads",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "379",
                               IndustryCode = "N482111",
                               NaicsCode = "482111",
                           },
                           new IpIndustry
                           {
                               IndustryText = "General freight trucking, local",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "383",
                               IndustryCode = "N484110",
                               NaicsCode = "484110",
                           },
                           new IpIndustry
                           {
                               IndustryText = "General freight trucking, local",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "382",
                               IndustryCode = "N48411_",
                               NaicsCode = "48411",
                           },
                           new IpIndustry
                           {
                               IndustryText = "General freight trucking, long-distance",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "384",
                               IndustryCode = "N48412_",
                               NaicsCode = "48412",
                           },
                           new IpIndustry
                           {
                               IndustryText = "General freight trucking",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "381",
                               IndustryCode = "N4841__",
                               NaicsCode = "4841",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Used household and office goods moving",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "386",
                               IndustryCode = "N484210",
                               NaicsCode = "484210",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Used household and office goods moving",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "385",
                               IndustryCode = "N48421_",
                               NaicsCode = "48421",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Truck transportation",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "380",
                               IndustryCode = "N484___",
                               NaicsCode = "484",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Postal service",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "390",
                               IndustryCode = "N491110",
                               NaicsCode = "491110",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Postal service",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "389",
                               IndustryCode = "N49111_",
                               NaicsCode = "49111",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Postal service",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "388",
                               IndustryCode = "N4911__",
                               NaicsCode = "4911",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Postal service",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "387",
                               IndustryCode = "N491___",
                               NaicsCode = "491",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Couriers and messengers",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "391",
                               IndustryCode = "N492___",
                               NaicsCode = "492",
                           },
                           new IpIndustry
                           {
                               IndustryText = "General warehousing and storage",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "395",
                               IndustryCode = "N493110",
                               NaicsCode = "493110",
                           },
                           new IpIndustry
                           {
                               IndustryText = "General warehousing and storage",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "394",
                               IndustryCode = "N49311_",
                               NaicsCode = "49311",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Refrigerated warehousing and storage",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "397",
                               IndustryCode = "N493120",
                               NaicsCode = "493120",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Refrigerated warehousing and storage",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "396",
                               IndustryCode = "N49312_",
                               NaicsCode = "49312",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Warehousing and storage",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "393",
                               IndustryCode = "N4931__",
                               NaicsCode = "4931",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Warehousing and storage",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "392",
                               IndustryCode = "N493___",
                               NaicsCode = "493",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Newspaper publishers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "402",
                               IndustryCode = "N511110",
                               NaicsCode = "511110",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Newspaper publishers",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "401",
                               IndustryCode = "N51111_",
                               NaicsCode = "51111",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Periodical publishers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "404",
                               IndustryCode = "N511120",
                               NaicsCode = "511120",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Periodical publishers",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "403",
                               IndustryCode = "N51112_",
                               NaicsCode = "51112",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Book publishers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "406",
                               IndustryCode = "N511130",
                               NaicsCode = "511130",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Book publishers",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "405",
                               IndustryCode = "N51113_",
                               NaicsCode = "51113",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Newspaper, periodical, book, and directory publishers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "400",
                               IndustryCode = "N5111__",
                               NaicsCode = "5111",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Software publishers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "409",
                               IndustryCode = "N511210",
                               NaicsCode = "511210",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Software publishers",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "408",
                               IndustryCode = "N51121_",
                               NaicsCode = "51121",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Software publishers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "407",
                               IndustryCode = "N5112__",
                               NaicsCode = "5112",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Publishing industries (except internet)",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "399",
                               IndustryCode = "N511___",
                               NaicsCode = "511",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Motion picture and video exhibition",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "411",
                               IndustryCode = "N51213_",
                               NaicsCode = "51213",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Radio broadcasting",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "414",
                               IndustryCode = "N51511_",
                               NaicsCode = "51511",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Radio and television broadcasting",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "413",
                               IndustryCode = "N5151__",
                               NaicsCode = "5151",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Cable and other subscription programming",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "417",
                               IndustryCode = "N515210",
                               NaicsCode = "515210",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Cable and other subscription programming",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "416",
                               IndustryCode = "N51521_",
                               NaicsCode = "51521",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Cable and other subscription programming",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "415",
                               IndustryCode = "N5152__",
                               NaicsCode = "5152",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Broadcasting (except internet)",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "412",
                               IndustryCode = "N515___",
                               NaicsCode = "515",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Wired telecommunications carriers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "421",
                               IndustryCode = "N517110",
                               NaicsCode = "517110",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Wired telecommunications carriers",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "420",
                               IndustryCode = "N51711_",
                               NaicsCode = "51711",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Wired telecommunications carriers",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "419",
                               IndustryCode = "N5171__",
                               NaicsCode = "5171",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Wireless telecommunications carriers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "424",
                               IndustryCode = "N517210",
                               NaicsCode = "517210",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Wireless telecommunications carriers (except satellite)",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "423",
                               IndustryCode = "N51721_",
                               NaicsCode = "51721",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Wireless telecommunications carriers (except satellite)",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "422",
                               IndustryCode = "N5172__",
                               NaicsCode = "5172",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Commercial banking",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "427",
                               IndustryCode = "N522110",
                               NaicsCode = "522110",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Commercial banking",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "426",
                               IndustryCode = "N52211_",
                               NaicsCode = "52211",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Passenger car rental",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "429",
                               IndustryCode = "N532111",
                               NaicsCode = "532111",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Truck, utility trailer, and rv (recreational vehicle) rental and leasing",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "431",
                               IndustryCode = "N532120",
                               NaicsCode = "532120",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Truck, utility trailer, and rv (recreational vehicle) rental and leasing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "430",
                               IndustryCode = "N53212_",
                               NaicsCode = "53212",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Video tape and disc rental",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "433",
                               IndustryCode = "N532230",
                               NaicsCode = "532230",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Video tape and disc rental",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "432",
                               IndustryCode = "N53223_",
                               NaicsCode = "53223",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Offices of certified public accountants",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "436",
                               IndustryCode = "N541211",
                               NaicsCode = "541211",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Tax preparation services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "437",
                               IndustryCode = "N541213",
                               NaicsCode = "541213",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Other accounting services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "438",
                               IndustryCode = "N541219",
                               NaicsCode = "541219",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Accounting, tax preparation, bookkeeping, and payroll services",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "435",
                               IndustryCode = "N54121_",
                               NaicsCode = "54121",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Accounting, tax preparation, bookkeeping, and payroll services",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "434",
                               IndustryCode = "N5412__",
                               NaicsCode = "5412",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Architectural services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "440",
                               IndustryCode = "N541310",
                               NaicsCode = "541310",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Architectural services",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "439",
                               IndustryCode = "N54131_",
                               NaicsCode = "54131",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Engineering services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "442",
                               IndustryCode = "N541330",
                               NaicsCode = "541330",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Engineering services",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "441",
                               IndustryCode = "N54133_",
                               NaicsCode = "54133",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Advertising agencies",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "444",
                               IndustryCode = "N541810",
                               NaicsCode = "541810",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Advertising agencies",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "443",
                               IndustryCode = "N54181_",
                               NaicsCode = "54181",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Photography studios, portrait",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "446",
                               IndustryCode = "N541921",
                               NaicsCode = "541921",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Employment placement agencies and executive search services",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "449",
                               IndustryCode = "N56131_",
                               NaicsCode = "56131",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Travel agencies",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "452",
                               IndustryCode = "N561510",
                               NaicsCode = "561510",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Travel agencies",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "451",
                               IndustryCode = "N56151_",
                               NaicsCode = "56151",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Travel arrangement and reservation services",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "450",
                               IndustryCode = "N5615__",
                               NaicsCode = "5615",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Janitorial services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "455",
                               IndustryCode = "N561720",
                               NaicsCode = "561720",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Janitorial services",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "454",
                               IndustryCode = "N56172_",
                               NaicsCode = "56172",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Medical laboratories",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "459",
                               IndustryCode = "N621511",
                               NaicsCode = "621511",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Diagnostic imaging centers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "460",
                               IndustryCode = "N621512",
                               NaicsCode = "621512",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Medical and diagnostic laboratories",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "458",
                               IndustryCode = "N62151_",
                               NaicsCode = "62151",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Medical and diagnostic laboratories",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "457",
                               IndustryCode = "N6215__",
                               NaicsCode = "6215",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Amusement and theme parks",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "463",
                               IndustryCode = "N713110",
                               NaicsCode = "713110",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Amusement and theme parks",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "462",
                               IndustryCode = "N71311_",
                               NaicsCode = "71311",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Gambling industries",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "464",
                               IndustryCode = "N7132__",
                               NaicsCode = "7132",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Golf courses and country clubs",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "466",
                               IndustryCode = "N713910",
                               NaicsCode = "713910",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Golf courses and country clubs",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "465",
                               IndustryCode = "N71391_",
                               NaicsCode = "71391",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Fitness and recreational sports centers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "468",
                               IndustryCode = "N713940",
                               NaicsCode = "713940",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Fitness and recreational sports centers",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "467",
                               IndustryCode = "N71394_",
                               NaicsCode = "71394",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Bowling centers",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "470",
                               IndustryCode = "N713950",
                               NaicsCode = "713950",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Bowling centers",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "469",
                               IndustryCode = "N71395_",
                               NaicsCode = "71395",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Hotels (except casino hotels) and motels",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "476",
                               IndustryCode = "N721110",
                               NaicsCode = "721110",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Hotels (except casino hotels) and motels",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "475",
                               IndustryCode = "N72111_",
                               NaicsCode = "72111",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Traveler accommodation",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "474",
                               IndustryCode = "N7211__",
                               NaicsCode = "7211",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Accommodation",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "473",
                               IndustryCode = "N721___",
                               NaicsCode = "721",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Special food services",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "478",
                               IndustryCode = "N7223__",
                               NaicsCode = "7223",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Drinking places (alcoholic beverages)",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "481",
                               IndustryCode = "N722410",
                               NaicsCode = "722410",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Drinking places (alcoholic beverages)",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "480",
                               IndustryCode = "N72241_",
                               NaicsCode = "72241",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Drinking places (alcoholic beverages)",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "479",
                               IndustryCode = "N7224__",
                               NaicsCode = "7224",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Full-service restaurants",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "484",
                               IndustryCode = "N722511",
                               NaicsCode = "722511",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Limited-service eating places",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "485",
                               IndustryCode = "N72251A",
                               NaicsCode = "72251A",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Restaurants and other eating places",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "483",
                               IndustryCode = "N72251_",
                               NaicsCode = "72251",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Restaurants and other eating places",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "482",
                               IndustryCode = "N7225__",
                               NaicsCode = "7225",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Food services and drinking places",
                               Selectable = "T",
                               DisplayLevel = "2",
                               SortSequence = "477",
                               IndustryCode = "N722___",
                               NaicsCode = "722",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Accommodation and food services",
                               Selectable = "T",
                               DisplayLevel = "1",
                               SortSequence = "472",
                               IndustryCode = "N72____",
                               NaicsCode = "72",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Automotive repair and maintenance",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "487",
                               IndustryCode = "N8111__",
                               NaicsCode = "8111",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Reupholstery and furniture repair",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "489",
                               IndustryCode = "N811420",
                               NaicsCode = "811420",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Reupholstery and furniture repair",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "488",
                               IndustryCode = "N81142_",
                               NaicsCode = "81142",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Hair, nail, and skin care services",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "491",
                               IndustryCode = "N81211_",
                               NaicsCode = "81211",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Personal care services",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "490",
                               IndustryCode = "N8121__",
                               NaicsCode = "8121",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Funeral homes and funeral services",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "494",
                               IndustryCode = "N812210",
                               NaicsCode = "812210",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Funeral homes and funeral services",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "493",
                               IndustryCode = "N81221_",
                               NaicsCode = "81221",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Coin-operated laundries and drycleaners",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "497",
                               IndustryCode = "N812310",
                               NaicsCode = "812310",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Coin-operated laundries and drycleaners",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "496",
                               IndustryCode = "N81231_",
                               NaicsCode = "81231",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Drycleaning and laundry services (except coin-operated)",
                               Selectable = "T",
                               DisplayLevel = "5",
                               SortSequence = "499",
                               IndustryCode = "N812320",
                               NaicsCode = "812320",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Drycleaning and laundry services (except coin-operated)",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "498",
                               IndustryCode = "N81232_",
                               NaicsCode = "81232",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Linen and uniform supply",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "500",
                               IndustryCode = "N81233_",
                               NaicsCode = "81233",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Drycleaning and laundry services",
                               Selectable = "T",
                               DisplayLevel = "3",
                               SortSequence = "495",
                               IndustryCode = "N8123__",
                               NaicsCode = "8123",
                           },
                           new IpIndustry
                           {
                               IndustryText = "Photofinishing",
                               Selectable = "T",
                               DisplayLevel = "4",
                               SortSequence = "502",
                               IndustryCode = "N81292_",
                               NaicsCode = "81292",
                           },

                       };
                return _values;
            }
        }
	}//end IpIndustry
}//end NoFuture.Rand.Gov.Bls.Codes