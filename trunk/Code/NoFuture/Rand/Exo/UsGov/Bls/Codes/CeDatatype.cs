using System.Collections.Generic;

namespace NoFuture.Rand.Exo.UsGov.Bls.Codes
{
    public class CeDatatype 
    {
        public string DataTypeCode { get; set; }
        public string DataTypeText { get; set; }
        private static List<CeDatatype> _values;
        public static List<CeDatatype> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<CeDatatype>
                           {
                           
                           new CeDatatype
                           {
                               DataTypeCode = "01",
                               DataTypeText = "ALL EMPLOYEES, THOUSANDS",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "02",
                               DataTypeText = "AVERAGE WEEKLY HOURS OF ALL EMPLOYEES",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "03",
                               DataTypeText = "AVERAGE HOURLY EARNINGS OF ALL EMPLOYEES",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "04",
                               DataTypeText = "AVERAGE WEEKLY OVERTIME HOURS OF ALL EMPLOYEES",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "06",
                               DataTypeText = "PRODUCTION AND NONSUPERVISORY EMPLOYEES, THOUSANDS",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "07",
                               DataTypeText = "AVERAGE WEEKLY HOURS OF PRODUCTION AND NONSUPERVISORY EMPLOYEES",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "08",
                               DataTypeText = "AVERAGE HOURLY EARNINGS OF PRODUCTION AND NONSUPERVISORY EMPLOYEES",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "09",
                               DataTypeText = "AVERAGE WEEKLY OVERTIME HOURS OF PRODUCTION AND NONSUPERVISORY EMPLOYEES",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "10",
                               DataTypeText = "WOMEN EMPLOYEES, THOUSANDS",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "11",
                               DataTypeText = "AVERAGE WEEKLY EARNINGS OF ALL EMPLOYEES",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "12",
                               DataTypeText = "AVERAGE WEEKLY EARNINGS OF ALL EMPLOYEES, 1982-1984 DOLLARS",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "13",
                               DataTypeText = "AVERAGE HOURLY EARNINGS OF ALL EMPLOYEES, 1982-1984 DOLLARS",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "15",
                               DataTypeText = "AVERAGE HOURLY EARNINGS OF ALL EMPLOYEES, EXCLUDING OVERTIME",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "16",
                               DataTypeText = "INDEXES OF AGGREGATE WEEKLY HOURS OF ALL EMPLOYEES, 2007=100",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "17",
                               DataTypeText = "INDEXES OF AGGREGATE WEEKLY PAYROLLS OF ALL EMPLOYEES, 2007=100",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "19",
                               DataTypeText = "AVERAGE WEEKLY HOURS OF ALL EMPLOYEES, QUARTERLY AVERAGES, SEASONALLY ADJUSTED",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "20",
                               DataTypeText = "AVERAGE WEEKLY OVERTIME HOURS OF ALL EMPLOYEES, QUARTERLY AVERAGES, SEASONALLY ADJUSTED",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "21",
                               DataTypeText = "DIFFUSION INDEXES, 1-MONTH SPAN, SEASONALLY ADJUSTED",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "22",
                               DataTypeText = "DIFFUSION INDEXES, 3-MONTH SPAN, SEASONALLY ADJUSTED",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "23",
                               DataTypeText = "DIFFUSION INDEXES, 6-MONTH SPAN, SEASONALLY ADJUSTED",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "24",
                               DataTypeText = "DIFFUSION INDEXES, 12-MONTH SPAN, NOT SEASONALLY ADJUSTED",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "25",
                               DataTypeText = "ALL EMPLOYEES, QUARTERLY AVERAGES, SEASONALLY ADJUSTED, THOUSANDS",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "26",
                               DataTypeText = "ALL EMPLOYEES, 3-MONTH AVERAGE CHANGE, SEASONALLY ADJUSTED, THOUSANDS",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "30",
                               DataTypeText = "AVERAGE WEEKLY EARNINGS OF PRODUCTION AND NONSUPERVISORY EMPLOYEES",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "31",
                               DataTypeText = "AVERAGE WEEKLY EARNINGS OF PRODUCTION AND NONSUPERVISORY EMPLOYEES, 1982-84 DOLLARS",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "32",
                               DataTypeText = "AVERAGE HOURLY EARNINGS OF PRODUCTION AND NONSUPERVISORY EMPLOYEES, 1982-84 DOLLARS",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "33",
                               DataTypeText = "AVERAGE HOURLY EARNINGS OF PRODUCTION AND NONSUPERVISORY EMPLOYEES, EXCLUDING OVERTIME",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "34",
                               DataTypeText = "INDEXES OF AGGREGATE WEEKLY HOURS OF PRODUCTION AND NONSUPERVISORY EMPLOYEES, 2002=100",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "35",
                               DataTypeText = "INDEXES OF AGGREGATE WEEKLY PAYROLLS OF PRODUCTION AND NONSUPERVISORY EMPLOYEES, 2002=100",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "36",
                               DataTypeText = "AVERAGE WEEKLY HOURS, PRODUCTION/NONSUPERVISORY EMPLOYEES, QUARTERLY AVERAGES, SEASONALLY ADJUSTED",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "37",
                               DataTypeText = "AVERAGE WEEKLY OVERTIME HOURS,PRODUCTION/NONSUPERVISORY EMPLOYEES,QUARTERLY AVG,SEASONALLY ADJUSTED",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "38",
                               DataTypeText = "PRODUCTION AND NONSUPERVISORY EMPLOYEES-TO-ALL EMPLOYEES RATIO",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "39",
                               DataTypeText = "WOMEN EMPLOYEES-TO-ALL EMPLOYEES RATIO",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "56",
                               DataTypeText = "AGGREGATE WEEKLY HOURS OF ALL EMPLOYEES, THOUSANDS",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "57",
                               DataTypeText = "AGGREGATE WEEKLY PAYROLLS OF ALL EMPLOYEES, THOUSANDS",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "58",
                               DataTypeText = "AGGREGATE WEEKLY OVERTIME HOURS OF ALL EMPLOYEES, THOUSANDS",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "81",
                               DataTypeText = "AGGREGATE WEEKLY HOURS OF PRODUCTION AND NONSUPERVISORY EMPLOYEES, THOUSANDS",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "82",
                               DataTypeText = "AGGREGATE WEEKLY PAYROLLS OF PRODUCTION AND NONSUPERVISORY EMPLOYEES, THOUSANDS",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "83",
                               DataTypeText = "AGGREGATE WEEKLY OVERTIME HOURS OF PRODUCTION AND NONSUPERVISORY EMPLOYEES, THOUSANDS",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "98",
                               DataTypeText = "CPI-U 1982-84",
                           },
                           new CeDatatype
                           {
                               DataTypeCode = "99",
                               DataTypeText = "CPI-W 1982-84",
                           },

                       };
                return _values;
            }
        }
	}//end CeDatatype
}//end NoFuture.Rand.Gov.Bls.Codes