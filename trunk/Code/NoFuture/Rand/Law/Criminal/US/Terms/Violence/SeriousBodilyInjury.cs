namespace NoFuture.Rand.Law.Criminal.US.Terms.Violence
{
    public class SeriousBodilyInjury : NondeadlyForce
    {
        protected override string CategoryName { get; } = "serious bodily injury";
        public override int GetCategoryRank()
        {
            return base.GetCategoryRank() + 1;
        }
    }
}
