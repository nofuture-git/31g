using System;
using System.Linq;
using System.Text;

namespace NoFuture.Rand.Gov.Fed
{
    /// <summary>
    /// This is unique id assigned by the Fed to all financial institutions.
    /// see [https://cdr.ffiec.gov/CDR/Public/CDRHelp/FAQs1205.htm#FAQ16]
    /// </summary>
    [Serializable]
    public class ResearchStatisticsSupervisionDiscount : Identifier
    {
        public override string Abbrev => "RSSD";
    }

    /// <summary>
    /// see [http://en.wikipedia.org/wiki/Routing_transit_number]
    /// </summary>
    [Serializable]
    public class RoutingTransitNumber : Identifier
    {
        #region constants
        public const string BOSTON = "01";
        public const string NEY_YORK = "02";
        public const string PHILADELPHIA = "03";
        public const string CLEVELAND = "04";
        public const string RICHMOND = "05";
        public const string ATLANTA = "06";
        public const string CHICAGO = "07";
        public const string ST_LOUIS = "08";
        public const string MINNEAPOLIS = "09";
        public const string KANSAS_CITY = "10";
        public const string DALLAS = "11";
        public const string SAN_FRANCISCO = "12";

        public const string REGEX_PATTERN = "(" + BOSTON + "|" + NEY_YORK + "|" + PHILADELPHIA + "|" + CLEVELAND
                                            + "|" + RICHMOND + "|" + ATLANTA + "|" + CHICAGO + "|" + ST_LOUIS + "|" +
                                            MINNEAPOLIS + "|" + KANSAS_CITY + "|" + DALLAS + "|" + ")[0-9]{7}"; 
        #endregion

        #region fields
        private int _checkDigit = -1;
        #endregion

        #region properties
        public override string Abbrev => "RTN";

        public string FedDistrictFullName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FedDistrict))
                    return string.Empty;
                if (FedDistrict.Length < 2)
                    return string.Empty;
                switch (FedDistrict.Substring(0, 2))
                {
                    case ATLANTA:
                        return "Federal Reserve Bank of Atlanta";
                    case BOSTON:
                        return "Federal Reserve Bank of Boston";
                    case CHICAGO:
                        return "Federal Reserve Bank of Chicago";
                    case CLEVELAND:
                        return "Federal Reserve Bank of Cleveland";
                    case DALLAS:
                        return "Federal Reserve Bank of Dallas";
                    case KANSAS_CITY:
                        return "Federal Reserve Bank of Kansas City";
                    case MINNEAPOLIS:
                        return "Federal Reserve Bank of Minneapolis";
                    case PHILADELPHIA:
                        return "Federal Reserve Bank of Philadelphia";
                    case RICHMOND:
                        return "Federal Reserve Bank of Richmond";
                    case SAN_FRANCISCO:
                        return "Federal Reserve Bank of San Francisco";
                    case ST_LOUIS:
                        return "Federal Reserve Bank of St. Louis";
                    default:
                        return "Federal Reserve Bank of New York";
                }
            }
        }
        public string FedDistrict { get; set; }
        public string CheckProcCenter { get; set; }
        public string AbaInstitutionId { get; set; }
        public int CheckDigit { get { return _checkDigit; } set { _checkDigit = value; } }
        public override string Value
        {
            get
            {
                var rt = new StringBuilder();
                rt.Append(FedDistrict);
                rt.Append(CheckProcCenter);
                rt.Append(AbaInstitutionId);
                if (CheckDigit < 0)
                {
                    CheckDigit = CalcChkDigit(rt.ToString());
                }
                rt.Append(CheckDigit);
                return rt.ToString();
            }
            set
            {
                var rt = value;
                if (string.IsNullOrWhiteSpace(rt))
                {
                    FedDistrict = null;
                    CheckProcCenter = null;
                    AbaInstitutionId = null;
                    _checkDigit = -1;
                    return;
                }
                rt = rt.Trim();
                var rtChars = rt.ToCharArray();
                var rtStrBldr = new StringBuilder();
                for (var i = 0; i < rtChars.Length; i++)
                {
                    rtStrBldr.Append(rtChars[i]);
                    if (i == 1)
                    {
                        FedDistrict = rtStrBldr.ToString();
                        rtStrBldr.Clear();
                    }
                    if (i == 3)
                    {
                        CheckProcCenter = rtStrBldr.ToString();
                        rtStrBldr.Clear();
                    }
                    if (i == 7)
                    {
                        AbaInstitutionId = rtStrBldr.ToString();
                        rtStrBldr.Clear();
                    }
                    if (i == 8)
                    {
                        int cdOut;
                        _checkDigit = int.TryParse(rtStrBldr.ToString(), out cdOut)
                            ? cdOut
                            : CalcChkDigit(string.Join("", FedDistrict, CheckProcCenter, AbaInstitutionId));
                    }
                }

            }
        }

        #endregion

        #region methods
        /// <summary>
        /// [https://en.wikipedia.org/wiki/Federal_Reserve_Bank#Assets]
        /// </summary>
        /// <returns></returns>
        public static RoutingTransitNumber RandomRoutingNumber()
        {
            var f = new RoutingTransitNumber {FedDistrict = NEY_YORK};

            var roll = Etx.IntNumber(1, 100);
            if (roll >= 57 && roll < 69)
                f.FedDistrict = SAN_FRANCISCO;
            if (roll >= 69 && roll < 75)
                f.FedDistrict = RICHMOND;
            if (roll >= 75 && roll < 80)
                f.FedDistrict = ATLANTA;
            if (roll >= 80 && roll < 85)
                f.FedDistrict = CHICAGO;
            if (roll >= 85 && roll < 88)
                f.FedDistrict = DALLAS;
            if (roll >= 88 && roll < 91)
                f.FedDistrict = CLEVELAND;
            if (roll >= 91 && roll < 94)
                f.FedDistrict = PHILADELPHIA;
            if (roll >= 94 && roll < 96)
                f.FedDistrict = BOSTON;
            if (roll == 96 || roll == 97)
                f.FedDistrict = KANSAS_CITY;
            if (roll == 98 || roll == 99)
                f.FedDistrict = ST_LOUIS;
            if (roll == 100)
                f.FedDistrict = MINNEAPOLIS;

            var rt = new StringBuilder();
            rt.Append(Etx.IntNumber(0, 9));
            rt.Append(Etx.IntNumber(0, 9));
            f.CheckProcCenter = rt.ToString();
            rt.Clear();

            for (var i = 0; i < 4; i++)
                rt.Append(Etx.IntNumber(0, 9));
            f.AbaInstitutionId = rt.ToString();

            rt.Clear();
            rt.Append(f.FedDistrict);
            rt.Append(f.CheckProcCenter);
            rt.Append(f.AbaInstitutionId);

            f.CheckDigit = CalcChkDigit(rt.ToString());
            return f;
        }

        public static int CalcChkDigit(string rtn)
        {
            if (string.IsNullOrWhiteSpace(rtn))
                return -1;
            var digits = rtn.ToCharArray();
            if (digits.Length != 8 && digits.Any(c => !char.IsNumber(c)))
                return -1;
            var chksum = new int[8];
            for (var i = 0; i < 8; i++)
            {
                chksum[i] = int.Parse(digits[i].ToString());
            }

            return (7 * (digits[0] + digits[3] + digits[6]) +
                    3 * (digits[1] + digits[4] + digits[7]) +
                    9 * (digits[2] + digits[5])) % 10;
        }
        #endregion
    }

    [Serializable]
    public class FdicNum : Identifier
    {
        //https://research.fdic.gov/bankfind/
        public override string Abbrev => "FDIC #";
    }
}
