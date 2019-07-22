namespace NoFuture.Rand.Law.US.Terms
{
    /// <summary>
    /// An idea of the weakest possible majority, but a majority nonetheless (e.g. 51%).
    /// </summary>
    public class PreponderanceTerm : ScintillaTerm
    {
        public override int GetRank()
        {
            return base.GetRank() + 1;
        }

        protected override string CategoryName { get; } = "preponderance of evidence";
    }
}
