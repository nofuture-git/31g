using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Exo.NfJson
{
    /// <summary>
    /// See https://iextrading.com/developer/docs/#getting-started
    /// IEX ToS  https://iextrading.com/api-exhibit-a
    /// </summary>
    [Serializable]
    public class IexApi : NfDynDataBase, ICited
    {
        public const string IEX_API_HOST = "https://api.iextrading.com/1.0";

        public IexApi() : base(new Uri($"{IEX_API_HOST}/ref-data/symbols"))
        {
        }

        public IexApi(Uri uri) : base(uri)
        {

        }

        public string Src
        {
            get { return "Data provided for free by IEX."; }
            set
            {
                //no-op
            }
        }

        public override IEnumerable<dynamic> ParseContent(object content)
        {
            var data = content as string;

            if (string.IsNullOrWhiteSpace(data))
                return null;

            data = data.Trim();
            if (data.StartsWith("//"))
                data = data.Substring(2, data.Length - 2);

            var dataOut = data.StartsWith("{")
                ? new[] {Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(data)}
                : Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<dynamic>>(data);
            foreach (var d in dataOut)
                d.Src = this.Src;

            return dataOut;
        }

        /// <summary>
        /// Cross reference to the abbreviation for &quot;type&quot; returned in the IEX call to 
        /// /ref-data/symbols to the full name.
        /// </summary>
        public static Dictionary<string, string> SymbolTypeAbbrev2Name => new Dictionary<string, string>
        {
            {"ad", "American Depositary Receipt"},
            {"re", "Real Estate Investment Trust"},
            {"et", "Exchange Traded Fund"},
            {"ce", "Closed End Fund"},
            {"si", "Secondary Issue"},
            {"lp", "Limited Partnerships"},
            {"cs", "Common Stock"}
        };
    }
}
