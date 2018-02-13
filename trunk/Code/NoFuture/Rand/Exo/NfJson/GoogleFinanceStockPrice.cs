using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Exo.NfJson
{
    public class GoogleFinanceStockPrice : NfDynDataBase
    {
        public const string GOOG_FIN_HOST = "finance.google.com";
        public GoogleFinanceStockPrice(Uri src) : base(src)
        {
        }

        /// <summary>
        /// Src [https://stackoverflow.com/questions/46070126/google-finance-json-stock-quote-stopped-working/]
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="startDate">Optional, defaults to 60 days past</param>
        /// <param name="endDate">Optional, defaults to the current date</param>
        /// <returns></returns>
        public static Uri GetUri(string ticker, DateTime? startDate = null, DateTime? endDate = null)
        {
            var googFin = $"http://{GOOG_FIN_HOST}/finance";

            if(string.IsNullOrWhiteSpace(ticker))
                throw new ArgumentNullException(nameof(ticker));

            var uri = $"{googFin}?q={ticker}&output=json";

            return new Uri(uri);
        }

        public override IEnumerable<dynamic> ParseContent(object content)
        {
            var data = content as string;

            if (string.IsNullOrWhiteSpace(data))
                return null;

            data = data.Trim();
            if (data.StartsWith("//"))
                data = data.Substring(2, data.Length - 2);

            return (IEnumerable<dynamic>) Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(data);
        }
    }
}
