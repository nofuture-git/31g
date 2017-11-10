using System;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public class FinancialData
    {
        public SpIncome Income { get; set; }
        public SpAssets Assets { get; set; }
        public override string ToString()
        {
            var i = Income?.ToString() ?? "0";
            var w = Assets?.ToString() ?? "0";
            return $"{w} {i}";
        }
    }
}
