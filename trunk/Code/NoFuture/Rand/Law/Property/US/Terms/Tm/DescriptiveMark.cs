namespace NoFuture.Rand.Law.Property.US.Terms.Tm
{
    /// <summary>
    /// Typically nothing more than dressing generic words with declension.
    /// </summary>
    public class DescriptiveMark : GenericMark
    {
        protected override string CategoryName => "descriptive mark";

        public override int GetRank()
        {
            return base.GetRank() + 1;
        }
    }
}
