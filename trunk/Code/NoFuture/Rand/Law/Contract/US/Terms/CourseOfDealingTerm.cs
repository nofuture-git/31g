namespace NoFuture.Rand.Law.Contract.US.Terms
{
    public class CourseOfDealingTerm : UsageOfTradeTerm
    {
        protected override string CategoryName => "Course of Dealing";

        public override int GetRank()
        {
            return 1 + base.GetRank();
        }
    }
}
