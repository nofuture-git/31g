namespace NoFuture.Law.US.Terms
{
    /// <summary>
    /// The smallest possible presence above nothing.
    /// </summary>
    public class ScintillaTerm : TermCategory
    {
        public override int GetRank()
        {
            return 0;
        }

        protected override string CategoryName { get; } = "scintilla";
    }
}
