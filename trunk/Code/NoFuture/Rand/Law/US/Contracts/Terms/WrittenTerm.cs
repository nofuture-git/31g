namespace NoFuture.Rand.Law.US.Contracts.Terms
{
    public class WrittenTerm : OralTerm
    {
        protected override string CategoryName => "Written";
        public override int GetCategoryRank()
        {
            return 1 + base.GetCategoryRank();
        }
    }
}