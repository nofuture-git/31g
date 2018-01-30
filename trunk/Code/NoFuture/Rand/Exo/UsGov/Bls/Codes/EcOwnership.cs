using System;
using System.Collections.Generic;
namespace NoFuture.Rand.Exo.UsGov.Bls.Codes
{
    public class EcOwnership 
    {
        public string OwnershipCode { get; set; }
        public string OwnershipName { get; set; }
        private static List<EcOwnership> _values;
        public static List<EcOwnership> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<EcOwnership>
                           {
                           
                           new EcOwnership
                           {
                               OwnershipName = "Civilian",
                               OwnershipCode = "1",
                           },
                           new EcOwnership
                           {
                               OwnershipName = "Private industry",
                               OwnershipCode = "2",
                           },
                           new EcOwnership
                           {
                               OwnershipName = "State and local government",
                               OwnershipCode = "3",
                           },

                       };
                return _values;
            }
        }
	}//end EcOwnership
}//end NoFuture.Rand.Gov.Bls.Codes