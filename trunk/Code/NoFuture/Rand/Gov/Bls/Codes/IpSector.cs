using System;
using System.Collections.Generic;
namespace NoFuture.Rand.Gov.Bls.Codes
{
    public class IpSector
    {
        public string SectorCode { get; set; }
        public string SectorText { get; set; }
        public static List<IpSector> Values
        {
            get
            {
                return new List<IpSector>
                       {
                           
                           new IpSector
                           {
                               SectorText = "Mining",
                               SectorCode = "B",
                           },
                           new IpSector
                           {
                               SectorText = "Utilities",
                               SectorCode = "C",
                           },
                           new IpSector
                           {
                               SectorText = "Manufacturing",
                               SectorCode = "E",
                           },
                           new IpSector
                           {
                               SectorText = "Wholesale Trade",
                               SectorCode = "G",
                           },
                           new IpSector
                           {
                               SectorText = "Retail Trade",
                               SectorCode = "H",
                           },
                           new IpSector
                           {
                               SectorText = "Transportation and Warehousing",
                               SectorCode = "I",
                           },
                           new IpSector
                           {
                               SectorText = "Information",
                               SectorCode = "J",
                           },
                           new IpSector
                           {
                               SectorText = "Finance and Insurance",
                               SectorCode = "K",
                           },
                           new IpSector
                           {
                               SectorText = "Real Estate and Rental and Leasing",
                               SectorCode = "L",
                           },
                           new IpSector
                           {
                               SectorText = "Professional, Scientific, and Technical Services",
                               SectorCode = "M",
                           },
                           new IpSector
                           {
                               SectorText = "Administrative and Support and Waste Management and Remediation Services",
                               SectorCode = "P",
                           },
                           new IpSector
                           {
                               SectorText = "Health Care and Social Assistance",
                               SectorCode = "R",
                           },
                           new IpSector
                           {
                               SectorText = "Arts, Entertainment, and Recreation",
                               SectorCode = "S",
                           },
                           new IpSector
                           {
                               SectorText = "Accommodation and Food Services",
                               SectorCode = "T",
                           },
                           new IpSector
                           {
                               SectorText = "Other Services (except Public Administration)",
                               SectorCode = "U",
                           },

                       };
            }
        }
	}//end IpSector
}//end NoFuture.Rand.Gov.Bls.Codes