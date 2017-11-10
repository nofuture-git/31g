using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Rand.Data.Exo.UsGov
{
    public static class Links
    {
        /// <summary>
        /// SEC's Investment Adviser Public Disclosure
        /// </summary>
        public static class SecIapd
        {
            public const string URL_ROOT = "http://www.adviserinfo.sec.gov";
            public const string URL_IAPD_REPORTS_PARTIAL = "/IAPD/Content/BulkFeed/CompilationDownload.aspx";

            public const string QRY_STR_FIRMS = "?FeedPK=4150&FeedType=IA_FIRM_SEC";
            public const string QRY_STR_INDVLS = "?FeedPK=4152&FeedType=IA_INDVL";

            public const string IARD_XSD_URL = "http://www.iard.com/iapd/compilation.zip";
            public const string IARD_XSD_INDVL_URL = "http://www.iard.com/iapd/iarcompilation.zip";

            /// <summary>
            /// This is an .xlsx sheet which list members of the National Securities Clearing Corp.
            /// and is the closest thing to a list of broker-dealers I could find
            /// </summary>
            public const string XLSX_BROKER_DEALERS =
                "http://www.dtcc.com/~/media/Files/Downloads/client-center/NSCC/nscc.xls";

        }

        /// <summary>
        /// IRS&apos;s NAICS listing in MS Excel
        /// </summary>
        public static class IrsSoi
        {
            public static Uri NaicsSuperSectorXls = new Uri("http://www.irs.gov/file_source/pub/irs-soi/histab14b.xls");
            public static Uri NaicsMajorIndustryXls = new Uri(string.Format("http://www.irs.gov/file_source/pub/irs-soi/{0:yy}co07ccr.xls", DateTime.Today.AddYears(-3)));
            public static Uri NaicsMinorIndustryXls = new Uri(string.Format("http://www.irs.gov/file_source/pub/irs-soi/{0:yy}co01ccr.xls", DateTime.Today.AddYears(-3)));
            //http://www.irs.gov/file_source/pub/irs-soi/07in01pw.xls
        }
        public class Ffiec
        {
            public const string SEARCH_URL_BASE = "https://www.ffiec.gov/nicpubweb/nicweb/";
        }
    }
}
