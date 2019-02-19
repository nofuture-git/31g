namespace NoFuture.Rand.Law.Criminal.US.Terms
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
