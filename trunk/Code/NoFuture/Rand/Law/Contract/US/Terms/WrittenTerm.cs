namespace NoFuture.Rand.Law.Contract.US.Terms
{
    public class WrittenTerm : OralTerm
    {
        protected override string CategoryName => "Written";
        public override int GetRank()
        {
            return 1 + base.GetRank();
        }
    }
}