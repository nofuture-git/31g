using System;
using System.Collections.Generic;
namespace NoFuture.Rand.Gov.Bls.Codes
{
    public class IpDuration
    {
        public string DurationCode { get; set; }
        public string DurationText { get; set; }
        public static List<IpDuration> Values
        {
            get
            {
                return new List<IpDuration>
                       {
                           
                           new IpDuration
                           {
                               DurationCode = "0",
                               DurationText = "Indexes or values",
                           },
                           new IpDuration
                           {
                               DurationCode = "1",
                               DurationText = "Annual percent changes",
                           },

                       };
            }
        }
	}//end IpDuration
}//end NoFuture.Rand.Gov.Bls.Codes