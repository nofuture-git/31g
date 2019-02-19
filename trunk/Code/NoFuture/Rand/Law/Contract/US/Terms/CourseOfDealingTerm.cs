namespace NoFuture.Rand.Law.Contract.US.Terms
{
    public class CourseOfDealingTerm : UsageOfTradeTerm
    {
        protected override string CategoryName => "Course of Dealing";

        public override int GetCategoryRank()
        {
            return 1 + base.GetCategoryRank();
        }
    }
}
