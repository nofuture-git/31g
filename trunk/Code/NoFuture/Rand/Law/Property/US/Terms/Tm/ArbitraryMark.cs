using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.Terms.Tm
{
    /// <summary>
    /// Words that have some common usage but also operate as a particular brand-mark.
    /// </summary>
    [Eg("Apple", "Google", "Amazon")]
    public class ArbitraryMark : SuggestiveMark
    {
        protected override string CategoryName => "arbitrary mark";

        public override int GetRank()
        {
            return base.GetRank() + 1;
        }
    }
}
