namespace NoFuture.Rand.Law.US.Criminal.Terms
{
    public class SeriousBodilyInjury : TermCategory
    {
        protected override string CategoryName { get; } = "serious bodily injury";
        public override int GetCategoryRank()
        {
            return new DeadlyForce().GetCategoryRank();
        }
    }
}
