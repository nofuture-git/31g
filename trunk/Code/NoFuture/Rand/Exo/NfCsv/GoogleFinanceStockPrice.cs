using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Exo.NfCsv
{
    public class GoogleFinanceStockPrice : NfDynDataBase
    {
        public GoogleFinanceStockPrice(Uri src) : base(src)
        {
        }

        /// <summary>
        /// Src [https://github.com/joshuaulrich/quantmod/blob/master/R/getSymbols.R]
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static Uri GetUri(string ticker, DateTime? startDate = null, DateTime? endDate = null)
        {
            const string GOOG_FIN = "http://finance.google.com/finance/historical";

            if(string.IsNullOrWhiteSpace(ticker))
                throw new ArgumentNullException(nameof(ticker));
            var stDt = startDate.GetValueOrDefault(DateTime.Today.AddDays(-60));
            var endDt = endDate.GetValueOrDefault(DateTime.Today);

            var uri = $"{GOOG_FIN}?q={ticker}";
            uri += $"&{GetQueryStringDate(stDt)}";
            uri += $"&{GetQueryStringDate(endDt,true)}";
            uri += "&output=csv";

            return new Uri(uri);
        }

        internal static string GetQueryStringDate(DateTime dt, bool isEndDate = false)
        {
            var d = isEndDate ? "enddate" : "startdate";
            return $"{d}={dt.MonthAbbrev()}+{dt.Day:00}+{dt.Year:0000}";
        }

        public override IEnumerable<dynamic> ParseContent(object content)
        {
            throw new NotImplementedException();
        }
    }
}
