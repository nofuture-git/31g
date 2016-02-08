using System;
using System.Collections.Generic;
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
        public override string Abbrev { get { return "RSSD"; } }
    }

    /// <summary>
    /// see [http://en.wikipedia.org/wiki/Routing_transit_number]
    /// </summary>
    [Serializable]
    public class RoutingTransitNumber : Identifier {
        public override string Abbrev
        {
            get { return "RTN"; }
        }

        public string FedDistrict
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Value))
                    return string.Empty;
                if (Value.Length < 2)
                    return string.Empty;
                switch (Value.Substring(0, 2))
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

        /// <summary>
        /// [https://en.wikipedia.org/wiki/Federal_Reserve_Bank#Assets]
        /// </summary>
        /// <returns></returns>
        public static RoutingTransitNumber RandomRoutingNumber()
        {
            var rt = new StringBuilder();
            var xx = NEY_YORK;
            var roll = NoFuture.Rand.Etx.Number(1, 100);
            if (roll >= 57 && roll < 69)
                xx = SAN_FRANCISCO;
            if (roll >= 69 && roll < 75)
                xx = RICHMOND;
            if (roll >= 75 && roll < 80)
                xx = ATLANTA;
            if (roll >= 80 && roll < 85)
                xx = CHICAGO;
            if (roll >= 85 && roll < 88)
                xx = DALLAS;
            if (roll >= 88 && roll < 91)
                xx = CLEVELAND;
            if (roll >= 91 && roll < 94)
                xx = PHILADELPHIA;
            if (roll >= 94 && roll < 96)
                xx = BOSTON;
            if (roll == 96 || roll == 97)
                xx = KANSAS_CITY;
            if (roll == 98 || roll == 99)
                xx = ST_LOUIS;
            if (roll == 100)
                xx = MINNEAPOLIS;
            rt.Append(xx);
            for (var i = 0; i < 7; i++)
                rt.Append(Etx.Number(0, 9));

            rt.Append(CalcChkDigit(rt.ToString()));
            return new RoutingTransitNumber {Value = rt.ToString()};
        }

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

        public static int CalcChkDigit(string rtn)
        {
            if (string.IsNullOrWhiteSpace(rtn))
                return -1;
            var digits = rtn.ToCharArray();
            if (digits.Length != 9 && digits.Any(c => !char.IsNumber(c)))
                return -1;
            var chksum = new int[9];
            for (var i = 0; i < 9; i++)
            {
                chksum[i] = int.Parse(new string(new[] {digits[i]}));
            }

            return (7 * (digits[0] + digits[3] + digits[6]) +
                    3 * (digits[1] + digits[4] + digits[7]) +
                    9 * (digits[2] + digits[5])) % 10;
        }
    }
}
