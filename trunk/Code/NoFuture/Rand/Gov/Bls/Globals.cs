namespace NoFuture.Rand.Gov.Bls
{
    public class Globals
    {
        public const string ApiSignature = "http://www.bls.gov/developers/api_signature_v2.htm";

        public static string PostUrl = "http://api.bls.gov/publicAPI/v2/timeseries/data/";

        public const char SeasonallyAdjusted = 'S';
        public const char Unadjusted = 'U';

        public const char Monthly = 'R';
        public const char SemiAnnual = 'S';

        public const char CurrentBaseYear = 'S';
        public const char AlternateBaseYear = 'A';

        public static class Defaults
        {
            public const string CuArea = "0000";
            public const string CuItem = "SA0";
            public const string EcCompensation = "1";
            public const string EcGroup = "000";
            public const string EcOwnership = "1";
            public const string EcPeriod = "Q05";
            public const string IpDuration = "0";
            public const string IpIndustry = "N211___"; //"Oil and gas extraction"
            public const string IpMeasure = "W20"; //"Number of employees (thousands)"
            public const string IpSector = "B"; //Mining
            public const string WpGroup = "00";
            public const string WpItem = "000000";
            public const string CeSupersector = "05";
            public const string CeIndustry = "00000000";
            public const string CeDatatype = "01";
        }
    }
}
