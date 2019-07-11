namespace NoFuture.Rand.Law.Property.US.Terms
{
    /// <summary>
    /// Descriptive words that require imagination, thought or perception to be recognize meaning.
    /// </summary>
    public class SuggestiveMark : DescriptiveMark
    {
        public override int GetRank()
        {
            return base.GetRank() + 1;
        }
    }
}
