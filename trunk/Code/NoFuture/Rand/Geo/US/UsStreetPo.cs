using System;
using System.Text.RegularExpressions;

namespace NoFuture.Rand.Geo.US
{
    [Serializable]
    public class UsStreetPo : StreetPo
    {
        #region Regex Patterns
        public const string STD_ADDR_LINE_REGEX = @"^([0-9\x5c\x2f\x2d]*)\x20*([\x21-\x7e]*)(\x20*([\x21-\x7e][\x20-\x7e]*))?";

        /// <summary>
        /// These are limited to the abbreviations used for street types (e.g. STREET -> ST) only.
        /// </summary>
        /// <remarks>
        /// see [http://pe.usps.gov/text/pub28/28apc_002.htm]
        /// </remarks>
        public const string US_POSTAL_STREET_KIND_REGEX =
            @"\x20(ST|PL|AVE|BLVD|CIR|DR|LN|PKWY|ALY|ANX|ARC|" +
            "BCH|BG|BGS|BLF|BLFS|BND|BR|BRG|BRK|BRKS|BTM|BYP|BYU|CIRS|CLB|CLF|" +
            "CLFS|CMN|COR|CORS|CP|CPE|CRES|CRK|CRSE|CRST|CSWY|CT|CTR|CTRS|CTS|CURV|CV|" +
            "CVS|CYN|DL|DM|DRS|DV|EST|ESTS|EXPY|EXT|EXTS|FALL|FLD|FLDS|FLS|FLT|FLTS|" +
            "FRD|FRDS|FRG|FRGS|FRK|FRKS|FRST|FRY|FT|FWY|GDN|GDNS|GLN|GLNS|GRN|GRNS|GRV|" +
            "GRVS|GTWY|HBR|HBRS|HL|HLS|HOLW|HTS|HVN|HWY|INLT|IS|ISLE|ISS|JCT|JCTS|KNL|KNLS|" +
            "KY|KYS|LAND|LCK|LCKS|LDG|LF|LGT|LGTS|LK|LKS|LNDG|MDW|MDWS|MEWS|" +
            "ML|MLS|MNR|MNRS|MSN|MT|MTN|MTNS|MTWY|NCK|OPAS|ORCH|OVAL|PASS|PATH|PIKE|" +
            "PLN|PLNS|PLZ|PNE|PNES|PR|PRT|PRTS|PSGE|PT|PTS|RADL|RAMP|RD|RDG|RDGS|" +
            "RDS|RIV|RNCH|ROW|RPD|RPDS|RST|RTE|RUE|RUN|SHL|SHLS|SHR|SHRS|SKWY|SMT|SPG|SPGS|" +
            "SPUR|SQ|SQS|STA|STRA|STRM|STS|TER|TPKE|TRAK|TRCE|TRFY|TRL|TRWY|TUNL|UN|UNS|" +
            @"UPAS|VIA|VIS|VL|VLG|VLGS|VLY|VLYS|VW|VWS|WALK|WALL|WAY|WAYS|WL|WLS|XING|XRD)(\x2e|\x20|$)";

        public const string US_POSTAL_SECONDARY_UNIT_REGEX =
            "\x20(APT|BSMT|BLDG|DEPT|FL|FRNT|HNGR|LBBY|LOT|LOWR|OFC|PH|PIER|REAR|RM|" + 
            "SIDE|SLIP|SPC|STOP|STE|TRLR|UNIT|UPPR|APARTMENT|BASEMENT|BUILDING|" +
            "FLOOR|FRONT|HANGAR|LOBBY|LOT|LOWER|OFFICE|PENTHOUSE|PIER|" +
            @"REAR|ROOM|SUITE|TRAILER|UNIT|UPPER)\x20([\x21-\x7a]*)?";

        public const string US_POSTAL_STREET_KIND_FULLNAME_REGEX = 
            @"\x20(ALLEY|ANEX|ARCADE|AVENUE|BAYOU|BEACH|BEND|BLUFF|BOTTOM|" + 
            "BOULEVARD|BRANCH|BRIDGE|BROOK|BYPASS|CAMP|CANYON|CAPE|" +
            "CAUSEWAY|CENTER|CIRCLE|CLIFF|CLIFFS|CLUB|CORNER|CORNERS|" +
            "COURSE|COURT|COURTS|COVE|CREEK|CRESCENT|CROSSING|DALE|DAM|" +
            "DIVIDE|DRIVE|ESTATE|ESTATES|EXPRESSWAY|EXTENSION|FALLS|" +
            "FERRY|FIELD|FIELDS|FLAT|FLATS|FORD|FOREST|FORGE|FORK|FORKS|" +
            "FORT|FREEWAY|GARDEN|GARDENS|GATEWAY|GLEN|GREEN|GROVE|" +
            "HARBOR|HAVEN|HEIGHTS|HIGHWAY|HILL|HILLS|HOLLOW|ISLAND|" +
            "ISLANDS|ISLE|JUNCTION|JUNCTIONS|KEY|KEYS|KNOLL|KNOLLS|" +
            "LAKE|LAKES|LANDING|LANE|LIGHT|LOAF|LOCK|LOCKS|LODGE|LOOP|" +
            "MANOR|MANORS|MEADOWS|MISSION|MOUNT|MOUNTAIN|MOUNTAINS|" +
            "NECK|ORCHARD|OVAL|PARK|PARKWAY|PARKWAYS|PATH|PIKE|PINES|" +
            "PLAIN|PLAINS|PLAZA|POINT|POINTS|PORT|PORTS|PRAIRIE|RADIAL|" +
            "RANCH|RAPID|RAPIDS|REST|RIDGE|RIDGES|RIVER|ROAD|ROADS|" +
            "SHOAL|SHOALS|SHORE|SHORES|SPRING|SPRINGS|SQUARE|SQUARES|" +
            "STATION|STRAVENUE|STREAM|STREET|SUMMIT|TERRACE|TRACE|TRACK|" +
            "TRAIL|TRAILER|TUNNEL|TURNPIKE|UNION|VALLEY|VALLEYS|VIADUCT|" +
            @"VIEW|VIEWS|VILLAGE|VILLAGES|VILLE|VISTA|WAY|WELLS)(\x2e|\x20|$)";

        public const string POUND_UNIT_ID = @"\x20\x23\x20?([0-9]+)";
        #endregion

        public UsStreetPo(AddressData d) : base(d) { }

        #region properties
        public string CountyTownship { get; set; }
        public string PostBox => data.AddressNumber;
        public string StreetName => data.StreetName;
        public string StreetKind => data.StreetType;
        public string SecondaryUnit => $"{data.SecondaryUnitDesignator} {data.SecondaryUnitId}".Trim();
        #endregion

        /// <summary>
        /// Based on the USPS Pub. 28 [http://pe.usps.gov/cpim/ftp/pubs/Pub28/pub28.pdf]
        /// </summary>
        /// <param name="addressLine"></param>
        /// <param name="streetPo"></param>
        /// <returns></returns>
        public static bool TryParse(string addressLine, out UsStreetPo streetPo)
        {
            if (string.IsNullOrWhiteSpace(addressLine))
            {
                streetPo = null;
                return false;
            }

            addressLine = addressLine.Trim();

            var regex = new Regex(STD_ADDR_LINE_REGEX,RegexOptions.IgnoreCase);
            if (!regex.IsMatch(addressLine))
            {
                streetPo = null;
                return false;
            }

            var matches = regex.Match(addressLine);

            if (matches.Groups.Count < 2)
            {
                streetPo = null;
                return false;
            }
            
            //get the address box number off the front of the string
            var addrData = new AddressData
            {
                AddressNumber = string.Empty,
                StreetName = string.Empty,
                StreetType = string.Empty,
                SecondaryUnitDesignator = string.Empty,
                SecondaryUnitId = string.Empty
            };

            var addrNumber = matches.Groups[1].Success && matches.Groups[1].Captures.Count > 0
                ? matches.Groups[1].Captures[0].Value
                : string.Empty;
            addrData.AddressNumber = addrNumber.Trim();

            Func<Regex, string, string> getStreetKind = (regex1, s) =>
            {
                var m = regex1.Match(s);
                var stKind = m.Groups[0].Success && m.Groups[0].Captures.Count > 0
                    ? m.Groups[0].Captures[0].Value
                    : string.Empty;
                return stKind.Trim();
            };

            //find a match to the street kind
            regex = new Regex(US_POSTAL_STREET_KIND_REGEX, RegexOptions.IgnoreCase);
            if (regex.IsMatch(addressLine))
            {
                addrData.StreetType = getStreetKind(regex, addressLine);
            }
            else
            {
                regex = new Regex(US_POSTAL_STREET_KIND_FULLNAME_REGEX, RegexOptions.IgnoreCase);
                if (regex.IsMatch(addressLine))
                {
                    addrData.StreetType = getStreetKind(regex, addressLine);
                }
            }

            //look for a secondary unit designator
            regex = new Regex(US_POSTAL_SECONDARY_UNIT_REGEX, RegexOptions.IgnoreCase);//postal standard
            if (regex.IsMatch(addressLine))
            {
                matches = regex.Match(addressLine);

                var secUnit = matches.Groups.Count >= 2 && 
                              matches.Groups[1].Success && 
                              matches.Groups[1].Captures.Count > 0
                    ? matches.Groups[1].Captures[0].Value
                    : string.Empty;

                var secUnitId = matches.Groups.Count >= 3 && 
                                matches.Groups[2].Success &&
                                matches.Groups[2].Captures.Count > 0
                    ? matches.Groups[2].Captures[0].Value
                    : string.Empty;

                addrData.SecondaryUnitDesignator = secUnit.Trim();
                addrData.SecondaryUnitId = secUnitId.Trim();
            }

            regex = new Regex(POUND_UNIT_ID, RegexOptions.IgnoreCase);//typical non-standard
            if (regex.IsMatch(addressLine))
            {
                matches = regex.Match(addressLine);

                var secUnitId = matches.Groups.Count > 1 && matches.Groups[1].Success &&
                                matches.Groups[1].Captures.Count > 0
                    ? matches.Groups[1].Captures[0].Value
                    : string.Empty;
                addrData.SecondaryUnitDesignator = secUnitId.Trim();
            }

            //consider whatever remains after rem of other parts as 'StreetName'
            var streetName = addressLine;

            if(addrData.AddressNumber.Length > 0)
                streetName = streetName.Replace(addrData.AddressNumber, string.Empty);
            if(addrData.StreetType.Length > 0)
                streetName = streetName.Replace(addrData.StreetType, string.Empty);
            if(addrData.SecondaryUnitDesignator.Length > 0)
                streetName = streetName.Replace(addrData.SecondaryUnitDesignator, string.Empty);
            if(addrData.SecondaryUnitId.Length > 0)
                streetName = streetName.Replace(addrData.SecondaryUnitId, string.Empty);

            addrData.StreetName = streetName.Replace("#", "").Trim();//per the standard, these should be removed

            //consider whatever remains as the street's name
            streetPo = new UsStreetPo(addrData);
            return true;
        }
    }
}