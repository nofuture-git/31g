namespace NoFuture.Rand.Law.US.Contracts.Terms
{
    /// <summary>
    /// terms that have been specifically mentioned and agreed by both 
    /// parties at the time the contract is made. They can either be oral 
    /// or in writing
    /// </summary>
    public class ExpressTerm : CourseOfPerformanceTerm
    {
        protected override string CategoryName => "Express";

        public override int GetCategoryRank()
        {
            return 1 + base.GetCategoryRank();
        }
    }
}
