using System;
using System.Text;

namespace NoFuture.Rand.Exo.UsGov.Bls
{
    public class ProducerPriceIndex : ISeries
    {
        public Uri ApiLink => new Uri("https://www.bls.gov/help/hlpforma.htm#WP");
        public string Prefix => "WP";

        public char? SeasonalAdjustment { get; set; }

        public Bls.Codes.WpGroup Group { get; set; }
        public Bls.Codes.WpItem Item { get; set; }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append(Prefix);
            str.Append(SeasonalAdjustment ?? Globals.Unadjusted);
            str.Append(Group == null ? Globals.Defaults.WpGroup : Group.GroupCode);
            str.Append(Item == null ? Globals.Defaults.WpItem : Item.ItemCode);

            return Globals.PostUrl + str;
        }
    }
}