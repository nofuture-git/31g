namespace NoFuture.Rand.Law.Tort.US.Terms
{
    /// <summary>
    /// clear predicates concerning some state or fact (e.g. required to wear a seatbelt)
    /// </summary>
    public class RulesTerm : StandardsTerm
    {
        protected override string CategoryName => "Rules";

        public override int GetRank()
        {
            return base.GetRank() + 1;
        }
    }
}
