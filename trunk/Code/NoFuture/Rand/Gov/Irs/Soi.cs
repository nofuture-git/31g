using System;

namespace NoFuture.Rand.Gov.Irs
{
    public class Soi
    {
        public static Uri NaicsSuperSector = new Uri("http://www.irs.gov/file_source/pub/irs-soi/histab14b.xls");
        public static Uri NaicsMajorIndustry = new Uri(string.Format("http://www.irs.gov/file_source/pub/irs-soi/{0:yy}co07ccr.xls",DateTime.Today.AddYears(-3)));
        public static Uri NaicsMinorIndustry = new Uri(string.Format("http://www.irs.gov/file_source/pub/irs-soi/{0:yy}co01ccr.xls", DateTime.Today.AddYears(-3)));
        //http://www.irs.gov/file_source/pub/irs-soi/07in01pw.xls
    }
}
