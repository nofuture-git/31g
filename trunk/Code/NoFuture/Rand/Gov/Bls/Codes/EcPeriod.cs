using System;
using System.Collections.Generic;
namespace NoFuture.Rand.Gov.Bls.Codes
{
    public class EcPeriod
    {
        public string Period { get; set; }
        public string PeriodAbbr { get; set; }
        public string PeriodName { get; set; }
        public static List<EcPeriod> Values
        {
            get
            {
                return new List<EcPeriod>
                       {
                           
                           new EcPeriod
                           {
                               PeriodAbbr = "QTR1",
                               Period = "Q01",
                               PeriodName = "1st Quarter",
                           },
                           new EcPeriod
                           {
                               PeriodAbbr = "QTR2",
                               Period = "Q02",
                               PeriodName = "2nd Quarter",
                           },
                           new EcPeriod
                           {
                               PeriodAbbr = "QTR3",
                               Period = "Q03",
                               PeriodName = "3rd Quarter",
                           },
                           new EcPeriod
                           {
                               PeriodAbbr = "QTR4",
                               Period = "Q04",
                               PeriodName = "4th Quarter",
                           },
                           new EcPeriod
                           {
                               PeriodAbbr = "AN AV",
                               Period = "Q05",
                               PeriodName = "Annual Average",
                           },

                       };
            }
        }
	}//end EcPeriod
}//end NoFuture.Rand.Gov.Bls.Codes