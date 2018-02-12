using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Exo.NfCsv
{
    public class GoogleFinanceStockPrice : NfDynDataBase
    {
        public const string GOOG_FIN_HOST = "finance.google.com";
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
            var googFin = $"http://{GOOG_FIN_HOST}/finance/historical";

            if(string.IsNullOrWhiteSpace(ticker))
                throw new ArgumentNullException(nameof(ticker));
            var stDt = startDate.GetValueOrDefault(DateTime.Today.AddDays(-60));
            var endDt = endDate.GetValueOrDefault(DateTime.Today);

            var uri = $"{googFin}?q={ticker}";
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
            var csv = content as string;
            if (string.IsNullOrWhiteSpace(csv))
                return null;

            var lines = csv.Split(Constants.LF);
            if (lines.Length <= 1)
                return null;

            var dataOut = new List<dynamic>();
            //TODO - too fragile, need a CSV parser
            for (var i = 1; i < lines.Length - 1; i++)
            {
                var lineData = lines[i].Split(',');
                if(lineData.Length < 6)
                    continue;

                if (new[]
                {
                    DateTime.TryParse(lineData[0], out DateTime date), double.TryParse(lineData[1], out var open),
                    double.TryParse(lineData[2], out var high), double.TryParse(lineData[3], out var low),
                    double.TryParse(lineData[4], out var close), int.TryParse(lineData[5], out var volume)
                }.Any(p => p == false))
                {
                    continue;
                }

                dataOut.Add(new
                {
                    Date = date,
                    Open = open,
                    High = high,
                    Low = low,
                    Close = close,
                    Volume = volume
                });
            }

            return dataOut;
        }
    }
}
