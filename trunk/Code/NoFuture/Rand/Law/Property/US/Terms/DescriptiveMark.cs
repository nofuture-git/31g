namespace NoFuture.Rand.Law.Property.US.Terms
{
    /// <summary>
    /// Typically nothing more than dressing generic words with declension.
    /// </summary>
    public class DescriptiveMark : GenericMark
    {
        public override int GetRank()
        {
            return base.GetRank() + 1;
        }
    }
}
