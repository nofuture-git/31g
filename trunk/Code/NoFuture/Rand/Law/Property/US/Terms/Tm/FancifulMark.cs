using NoFuture.Law.Attributes;

namespace NoFuture.Law.Property.US.Terms.Tm
{
    /// <summary>
    /// A neologism whose only existence is due to being a brand.
    /// </summary>
    [Aka("Coined Mark")]
    [Eg("Pepsi", "Exxon")]
    public class FancifulMark : ArbitraryMark
    {
        protected override string CategoryName => "fanciful mark";
        public override int GetRank()
        {
            return base.GetRank() + 1;
        }
    }
}
