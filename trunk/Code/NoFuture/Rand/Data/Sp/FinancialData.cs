using System;

namespace NoFuture.Rand.Data.Sp
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
