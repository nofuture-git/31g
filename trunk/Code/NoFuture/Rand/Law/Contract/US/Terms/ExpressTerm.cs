using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Contract.US.Terms
{
    /// <summary>
    /// terms that have been specifically mentioned and agreed by both 
    /// parties at the time the contract is made. They can either be oral 
    /// or in writing
    /// </summary>
    [Aka("Intrinsic Term")]
    public class ExpressTerm : CourseOfPerformanceTerm
    {
        protected override string CategoryName => "Express";

        public override int GetCategoryRank()
        {
            return 1 + base.GetCategoryRank();
        }
    }
}
