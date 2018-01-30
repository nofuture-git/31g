using System;
using System.Collections.Generic;
namespace NoFuture.Rand.Data.Exo.UsGov.Bls.Codes
{
    public class CuArea 
    {
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public string DisplayLevel { get; set; }
        public string Selectable { get; set; }
        public string SortSequence { get; set; }
        private static List<CuArea> _values;
        public static List<CuArea> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<CuArea>
                           {
                           
                           new CuArea
                           {
                               AreaCode = "0000",
                               AreaName = "U.S. city average",
                               DisplayLevel = "0",
                               SortSequence = "1",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "0100",
                               AreaName = "Northeast urban",
                               DisplayLevel = "0",
                               SortSequence = "5",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "0200",
                               AreaName = "Midwest urban",
                               DisplayLevel = "0",
                               SortSequence = "12",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "0300",
                               AreaName = "South urban",
                               DisplayLevel = "0",
                               SortSequence = "24",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "0400",
                               AreaName = "West urban",
                               DisplayLevel = "0",
                               SortSequence = "34",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A000",
                               AreaName = "Size Class A (more than 1,500,000)",
                               DisplayLevel = "0",
                               SortSequence = "2",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A100",
                               AreaName = "Northeast urban - Size Class A",
                               DisplayLevel = "1",
                               SortSequence = "10",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A101",
                               AreaName = "New York-Northern New Jersey-Long Island, NY-NJ-CT-PA",
                               DisplayLevel = "1",
                               SortSequence = "6",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A102",
                               AreaName = "Philadelphia-Wilmington-Atlantic City, PA-NJ-DE-MD",
                               DisplayLevel = "1",
                               SortSequence = "7",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A103",
                               AreaName = "Boston-Brockton-Nashua, MA-NH-ME-CT",
                               DisplayLevel = "1",
                               SortSequence = "8",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A104",
                               AreaName = "Pittsburgh, PA",
                               DisplayLevel = "1",
                               SortSequence = "9",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A200",
                               AreaName = "Midwest - Size Class A",
                               DisplayLevel = "1",
                               SortSequence = "21",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A207",
                               AreaName = "Chicago-Gary-Kenosha, IL-IN-WI",
                               DisplayLevel = "1",
                               SortSequence = "13",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A208",
                               AreaName = "Detroit-Ann Arbor-Flint, MI",
                               DisplayLevel = "1",
                               SortSequence = "16",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A209",
                               AreaName = "St. Louis, MO-IL",
                               DisplayLevel = "1",
                               SortSequence = "20",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A210",
                               AreaName = "Cleveland-Akron, OH",
                               DisplayLevel = "1",
                               SortSequence = "15",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A211",
                               AreaName = "Minneapolis-St. Paul, MN-WI",
                               DisplayLevel = "1",
                               SortSequence = "19",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A212",
                               AreaName = "Milwaukee-Racine, WI",
                               DisplayLevel = "1",
                               SortSequence = "18",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A213",
                               AreaName = "Cincinnati-Hamilton, OH-KY-IN",
                               DisplayLevel = "1",
                               SortSequence = "14",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A214",
                               AreaName = "Kansas City, MO-KS",
                               DisplayLevel = "1",
                               SortSequence = "17",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A300",
                               AreaName = "South - Size Class A",
                               DisplayLevel = "1",
                               SortSequence = "31",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A311",
                               AreaName = "Washington-Baltimore, DC-MD-VA-WV",
                               DisplayLevel = "1",
                               SortSequence = "30",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A316",
                               AreaName = "Dallas-Fort Worth, TX",
                               DisplayLevel = "1",
                               SortSequence = "26",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A318",
                               AreaName = "Houston-Galveston-Brazoria, TX",
                               DisplayLevel = "1",
                               SortSequence = "27",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A319",
                               AreaName = "Atlanta, GA",
                               DisplayLevel = "1",
                               SortSequence = "25",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A320",
                               AreaName = "Miami-Fort Lauderdale, FL",
                               DisplayLevel = "1",
                               SortSequence = "28",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A321",
                               AreaName = "Tampa-St. Petersburg-Clearwater, FL",
                               DisplayLevel = "1",
                               SortSequence = "29",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A400",
                               AreaName = "West - Size Class A",
                               DisplayLevel = "1",
                               SortSequence = "44",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A421",
                               AreaName = "Los Angeles-Riverside-Orange County, CA",
                               DisplayLevel = "1",
                               SortSequence = "38",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A422",
                               AreaName = "San Francisco-Oakland-San Jose, CA",
                               DisplayLevel = "1",
                               SortSequence = "42",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A423",
                               AreaName = "Seattle-Tacoma-Bremerton, WA",
                               DisplayLevel = "1",
                               SortSequence = "43",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A424",
                               AreaName = "San Diego, CA",
                               DisplayLevel = "1",
                               SortSequence = "41",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A425",
                               AreaName = "Portland-Salem, OR-WA",
                               DisplayLevel = "1",
                               SortSequence = "40",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A426",
                               AreaName = "Honolulu, HI",
                               DisplayLevel = "1",
                               SortSequence = "37",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A427",
                               AreaName = "Anchorage, AK",
                               DisplayLevel = "1",
                               SortSequence = "35",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A429",
                               AreaName = "Phoenix-Mesa, AZ",
                               DisplayLevel = "1",
                               SortSequence = "39",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "A433",
                               AreaName = "Denver-Boulder-Greeley, CO",
                               DisplayLevel = "1",
                               SortSequence = "36",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "D000",
                               AreaName = "Size Class D (under 50,000)",
                               DisplayLevel = "0",
                               SortSequence = "4",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "D200",
                               AreaName = "Midwest - Size Class D",
                               DisplayLevel = "1",
                               SortSequence = "23",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "D300",
                               AreaName = "South - Size Class D",
                               DisplayLevel = "1",
                               SortSequence = "33",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "X000",
                               AreaName = "Size Class B/C (between 50,000 and 1,500,000)",
                               DisplayLevel = "0",
                               SortSequence = "3",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "X100",
                               AreaName = "Northeast urban - Size Class B/C",
                               DisplayLevel = "1",
                               SortSequence = "11",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "X200",
                               AreaName = "Midwest - Size Class B/C",
                               DisplayLevel = "1",
                               SortSequence = "22",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "X300",
                               AreaName = "South - Size Class B/C",
                               DisplayLevel = "1",
                               SortSequence = "32",
                               Selectable = "T",
                           },
                           new CuArea
                           {
                               AreaCode = "X400",
                               AreaName = "West - Size Class B/C",
                               DisplayLevel = "1",
                               SortSequence = "45",
                               Selectable = "T",
                           },

                       };
                return _values;
            }
        }
	}//end CuArea
}//end NoFuture.Rand.Gov.Bls.Codes