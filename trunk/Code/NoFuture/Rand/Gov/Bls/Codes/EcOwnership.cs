using System;
using System.Collections.Generic;
namespace NoFuture.Rand.Gov.Bls.Codes
{
    public class EcOwnership
    {
        public string OwnershipCode { get; set; }
        public string OwnershipName { get; set; }
        public static List<EcOwnership> Values
        {
            get
            {
                return new List<EcOwnership>
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
            }
        }
	}//end EcOwnership
}//end NoFuture.Rand.Gov.Bls.Codes