using System;
using System.Collections.Generic;
namespace NoFuture.Rand.Gov.Bls.Codes
{
    public class IpDuration 
    {
        public string DurationCode { get; set; }
        public string DurationText { get; set; }
        private static List<IpDuration> _values;
        public static List<IpDuration> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<IpDuration>
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
                return _values;
            }
        }
	}//end IpDuration
}//end NoFuture.Rand.Gov.Bls.Codes