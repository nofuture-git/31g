using System;
using System.Text;

namespace NoFuture.Rand.Exo.UsGov.Bls
{
    public class ConsumerPriceIndex : ISeries
    {
        public Uri ApiLink => new Uri("https://www.bls.gov/help/hlpforma.htm#CU");
        public string Prefix => "CU";

        public char? SeasonalAdjustment { get; set; }
        public char? Periodicity { get; set; }
        public char? BaseYear { get; set; }

        public Bls.Codes.CuArea Area { get; set; }
        public Bls.Codes.CuItem Item { get; set; }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append(Prefix);
            str.Append(SeasonalAdjustment ?? Globals.Unadjusted);
            str.Append(Periodicity ?? Globals.Monthly);
            str.Append(Area == null ? Globals.Defaults.CuArea : Area.AreaCode);
            str.Append(BaseYear ?? Globals.CurrentBaseYear);
            str.Append(Item == null ? Globals.Defaults.CuItem : Item.ItemCode);

            return str.ToString();

        }

    }
}