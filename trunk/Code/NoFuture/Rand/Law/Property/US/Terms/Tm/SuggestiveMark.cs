namespace NoFuture.Rand.Law.Property.US.Terms.Tm
{
    /// <summary>
    /// Descriptive words that require imagination, thought or perception to be recognize meaning.
    /// </summary>
    public class SuggestiveMark : DescriptiveMark
    {
        protected override string CategoryName => "suggestive mark";

        public override int GetRank()
        {
            return base.GetRank() + 1;
        }
    }
}
