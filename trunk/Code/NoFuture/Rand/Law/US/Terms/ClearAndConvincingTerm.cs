namespace NoFuture.Rand.Law.US.Terms
{
    /// <summary>
    /// The term for being highly probable but does not negate all reasonable doubt.
    /// </summary>
    public class ClearAndConvincingTerm : PreponderanceTerm
    {
        public override int GetRank()
        {
            return base.GetRank() + 1;
        }
        protected override string CategoryName { get; } = "clear and convincing";
    }
}
