using System.Collections.Generic;

namespace NoFuture.Rand.Data.Exo.UsGov.Bea.Parameters.Ita
{
    public class AreaOrCountry : BeaParameter
    {
        public override string Description { get; set; }
        public override string Val { get; set; }
        private static List<AreaOrCountry> _values;
        public static List<AreaOrCountry> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<AreaOrCountry>
                           {
                           
                           new AreaOrCountry
                           {
                               Val = "Africa",
                               Description = "Africa",
                           },
                           new AreaOrCountry
                           {
                               Val = "AfricaOthGdsNsaDetail",
                               Description = "Africa; other countries (those not listed in table 2.3)",
                           },
                           new AreaOrCountry
                           {
                               Val = "Algeria",
                               Description = "Algeria",
                           },
                           new AreaOrCountry
                           {
                               Val = "AllCountries",
                               Description = "All countries",
                           },
                           new AreaOrCountry
                           {
                               Val = "AllOthSeas",
                               Description = "All other countries (those not listed separately in tables 2.2 and 3.2)",
                           },
                           new AreaOrCountry
                           {
                               Val = "AllOthThanCanada",
                               Description = "All countries other than Canada",
                           },
                           new AreaOrCountry
                           {
                               Val = "Argentina",
                               Description = "Argentina",
                           },
                           new AreaOrCountry
                           {
                               Val = "AsiaAndPac",
                               Description = "Asia and Pacific",
                           },
                           new AreaOrCountry
                           {
                               Val = "AsiaAndPacOthNsaDetail",
                               Description = "Asia and Pacific; other countries (those not listed in table 2.3)",
                           },
                           new AreaOrCountry
                           {
                               Val = "Australia",
                               Description = "Australia",
                           },
                           new AreaOrCountry
                           {
                               Val = "Austria",
                               Description = "Austria",
                           },
                           new AreaOrCountry
                           {
                               Val = "Belgium",
                               Description = "Belgium",
                           },
                           new AreaOrCountry
                           {
                               Val = "Brazil",
                               Description = "Brazil",
                           },
                           new AreaOrCountry
                           {
                               Val = "Canada",
                               Description = "Canada",
                           },
                           new AreaOrCountry
                           {
                               Val = "Chile",
                               Description = "Chile",
                           },
                           new AreaOrCountry
                           {
                               Val = "China",
                               Description = "China",
                           },
                           new AreaOrCountry
                           {
                               Val = "Colombia",
                               Description = "Colombia",
                           },
                           new AreaOrCountry
                           {
                               Val = "EU",
                               Description = "European Union",
                           },
                           new AreaOrCountry
                           {
                               Val = "EuroArea",
                               Description = "Euro area",
                           },
                           new AreaOrCountry
                           {
                               Val = "Europe",
                               Description = "Europe",
                           },
                           new AreaOrCountry
                           {
                               Val = "EuropeanUnion",
                               Description = "European Union",
                           },
                           new AreaOrCountry
                           {
                               Val = "EuropeExclEU",
                               Description = "Europe excluding European Union",
                           },
                           new AreaOrCountry
                           {
                               Val = "EuropeOthNsaDetail",
                               Description = "Europe; other countries (those not listed in table 2.3)",
                           },
                           new AreaOrCountry
                           {
                               Val = "Finland",
                               Description = "Finland",
                           },
                           new AreaOrCountry
                           {
                               Val = "France",
                               Description = "France",
                           },
                           new AreaOrCountry
                           {
                               Val = "Germany",
                               Description = "Germany",
                           },
                           new AreaOrCountry
                           {
                               Val = "Greece",
                               Description = "Greece",
                           },
                           new AreaOrCountry
                           {
                               Val = "HongKong",
                               Description = "Hong Kong",
                           },
                           new AreaOrCountry
                           {
                               Val = "India",
                               Description = "India",
                           },
                           new AreaOrCountry
                           {
                               Val = "Indonesia",
                               Description = "Indonesia",
                           },
                           new AreaOrCountry
                           {
                               Val = "IntOrgAndUnalloc",
                               Description = "International organizations and unallocated",
                           },
                           new AreaOrCountry
                           {
                               Val = "Ireland",
                               Description = "Ireland",
                           },
                           new AreaOrCountry
                           {
                               Val = "Israel",
                               Description = "Israel",
                           },
                           new AreaOrCountry
                           {
                               Val = "Italy",
                               Description = "Italy",
                           },
                           new AreaOrCountry
                           {
                               Val = "Japan",
                               Description = "Japan",
                           },
                           new AreaOrCountry
                           {
                               Val = "KoreaRepOf",
                               Description = "Republic of Korea",
                           },
                           new AreaOrCountry
                           {
                               Val = "LatAmAndOthWestHem",
                               Description = "Latin America and Other Western Hemisphere",
                           },
                           new AreaOrCountry
                           {
                               Val = "Luxembourg",
                               Description = "Luxembourg",
                           },
                           new AreaOrCountry
                           {
                               Val = "Malaysia",
                               Description = "Malaysia",
                           },
                           new AreaOrCountry
                           {
                               Val = "Mexico",
                               Description = "Mexico",
                           },
                           new AreaOrCountry
                           {
                               Val = "MiddleEast",
                               Description = "Middle East",
                           },
                           new AreaOrCountry
                           {
                               Val = "MiddleEastOthGdsNsaDetail",
                               Description = "Middle East; other countries (those not listed in table 2.3)",
                           },
                           new AreaOrCountry
                           {
                               Val = "Netherlands",
                               Description = "Netherlands",
                           },
                           new AreaOrCountry
                           {
                               Val = "Nigeria",
                               Description = "Nigeria",
                           },
                           new AreaOrCountry
                           {
                               Val = "Norway",
                               Description = "Norway",
                           },
                           new AreaOrCountry
                           {
                               Val = "Opec",
                               Description = "Members of OPEC",
                           },
                           new AreaOrCountry
                           {
                               Val = "OthAfrica",
                               Description = "Other Africa",
                           },
                           new AreaOrCountry
                           {
                               Val = "OthAsiaAndPac",
                               Description = "Other Asia and Pacific",
                           },
                           new AreaOrCountry
                           {
                               Val = "OthEU",
                               Description = "Other European Union",
                           },
                           new AreaOrCountry
                           {
                               Val = "OthEuroArea",
                               Description = "Other Euro area",
                           },
                           new AreaOrCountry
                           {
                               Val = "OthSouthAndCenAm",
                               Description = "Other South and Central America",
                           },
                           new AreaOrCountry
                           {
                               Val = "OthWestHem",
                               Description = "Other Western Hemisphere",
                           },
                           new AreaOrCountry
                           {
                               Val = "Philippines",
                               Description = "Philippines",
                           },
                           new AreaOrCountry
                           {
                               Val = "Portugal",
                               Description = "Portugal",
                           },
                           new AreaOrCountry
                           {
                               Val = "ResidualSeas",
                               Description = "Residual between the seasonally adjusted total based on service type or commodity and the sum of the seasonally adjusted individual countries and the 'all other countries' aggregate",
                           },
                           new AreaOrCountry
                           {
                               Val = "Russia",
                               Description = "Russia",
                           },
                           new AreaOrCountry
                           {
                               Val = "SaudiArabia",
                               Description = "Saudi Arabia",
                           },
                           new AreaOrCountry
                           {
                               Val = "Singapore",
                               Description = "Singapore",
                           },
                           new AreaOrCountry
                           {
                               Val = "SouthAfrica",
                               Description = "South Africa",
                           },
                           new AreaOrCountry
                           {
                               Val = "SouthAndCenAm",
                               Description = "South and Central America",
                           },
                           new AreaOrCountry
                           {
                               Val = "SouthAndCenAmOthNsaDetail",
                               Description = "South and Central America; other countries (those not listed in table 2.3)",
                           },
                           new AreaOrCountry
                           {
                               Val = "Spain",
                               Description = "Spain",
                           },
                           new AreaOrCountry
                           {
                               Val = "Sweden",
                               Description = "Sweden",
                           },
                           new AreaOrCountry
                           {
                               Val = "Switzerland",
                               Description = "Switzerland",
                           },
                           new AreaOrCountry
                           {
                               Val = "Taiwan",
                               Description = "Taiwan",
                           },
                           new AreaOrCountry
                           {
                               Val = "Thailand",
                               Description = "Thailand",
                           },
                           new AreaOrCountry
                           {
                               Val = "Turkey",
                               Description = "Turkey",
                           },
                           new AreaOrCountry
                           {
                               Val = "UnitedKingdom",
                               Description = "United Kingdom",
                           },
                           new AreaOrCountry
                           {
                               Val = "Venezuela",
                               Description = "Venezuela",
                           },

                       };
                return _values;
            }
        }
	}//end AreaOrCountry
}//end NoFuture.Rand.Gov.Bea.Parameters.Ita