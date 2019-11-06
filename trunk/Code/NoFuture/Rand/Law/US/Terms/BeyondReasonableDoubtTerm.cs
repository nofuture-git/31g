namespace NoFuture.Rand.Law.US.Terms
{
    /// <summary>
    /// The highest level of evidence - there is no reason to believe otherwise
    /// </summary>
    public class BeyondReasonableDoubtTerm : ClearAndConvincingTerm
    {
        public override int GetRank()
        {
            return base.GetRank() + 1;
        }

        protected override string CategoryName { get; } = "beyond a reasonable doubt";
    }
}
