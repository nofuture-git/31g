using System;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Com
{
    [Serializable]
    public class FinancialData
    {
        public IncomeSummary IncomeSummary { get; set; }
        public AssetsSummary AssetsSummary { get; set; }
        public override string ToString()
        {
            var i = IncomeSummary?.ToString() ?? "0";
            var w = AssetsSummary?.ToString() ?? "0";
            return $"{w} {i}";
        }
    }
}
