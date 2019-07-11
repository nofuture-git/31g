using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.Terms
{
    /// <summary>
    /// A neologism whose only existence is due to being a brand.
    /// </summary>
    [Aka("Coined Mark")]
    [Eg("Pepsi", "Exxon")]
    public class FancifulMark : ArbitraryMark
    {
        public override int GetRank()
        {
            return base.GetRank() + 1;
        }
    }
}
