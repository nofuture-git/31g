using System;
using System.Linq;

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
