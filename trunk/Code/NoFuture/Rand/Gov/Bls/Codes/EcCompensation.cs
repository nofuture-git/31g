using System;
using System.Collections.Generic;
namespace NoFuture.Rand.Gov.Bls.Codes
{
    public class EcCompensation 
    {
        public string CompCode { get; set; }
        public string CompText { get; set; }
        private static List<EcCompensation> _values;
        public static List<EcCompensation> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<EcCompensation>
                           {
                           
                           new EcCompensation
                           {
                               CompCode = "1",
                               CompText = "Total compensation",
                           },
                           new EcCompensation
                           {
                               CompCode = "2",
                               CompText = "Wages and salaries",
                           },
                           new EcCompensation
                           {
                               CompCode = "3",
                               CompText = "Benefits",
                           },

                       };
                return _values;
            }
        }
	}//end EcCompensation
}//end NoFuture.Rand.Gov.Bls.Codes