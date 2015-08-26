using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Rand.Gov.Sec
{
    /// <summary>
    /// SEC's Investment Adviser Public Disclosure
    /// </summary>
    public class Iapd
    {
        public const string URL_ROOT = "http://www.adviserinfo.sec.gov";
        public const string URL_IAPD_REPORTS_PARTIAL = "/IAPD/Content/BulkFeed/CompilationDownload.aspx";

        public const string QRY_STR_FIRMS = "?FeedPK=4150&FeedType=IA_FIRM_SEC";
        public const string QRY_STR_INDVLS = "?FeedPK=4152&FeedType=IA_INDVL";

        public const string IARD_XSD_URL = "http://www.iard.com/iapd/compilation.zip";
        public const string IARD_XSD_INDVL_URL = "http://www.iard.com/iapd/iarcompilation.zip";


        //this is an .xlsx sheet which list members of the National Securities Clearing Corp.
        // and is the closest thing to a list of broker-dealers I could find
        //http://www.dtcc.com/~/media/Files/Downloads/client-center/NSCC/nscc.xls
    }
}
