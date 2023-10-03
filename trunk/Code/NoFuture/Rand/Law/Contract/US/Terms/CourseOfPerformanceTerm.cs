namespace NoFuture.Law.Contract.US.Terms
{
    /// <summary>
    /// <![CDATA[
    /// Restatement (Second) of Contracts § 202(4)
    /// Where the use of the word (i.e. its meaning) has been performed before without objection
    /// ]]>
    /// </summary>
    public class CourseOfPerformanceTerm : CourseOfDealingTerm
    {
        protected override string CategoryName => "Course of Performance";
        public override int GetRank()
        {
            return 1 + base.GetRank();
        }
    }
}
