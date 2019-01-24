namespace NoFuture.Rand.Law.US.Contracts.Terms
{
    /// <summary>
    /// <![CDATA[
    /// Restatement (Second) of Contracts § 202(3)(b)
    /// technical terms and words of art are given their technical meaning when used in a transaction within their technical field.
    /// ]]>
    /// </summary>
    public class TechnicalTerm : TermCategory
    {
        protected override string CategoryName => "Technical";

        public virtual  bool IsTechnicalContext { get; set; }

        public override int GetCategoryRank()
        {
            var s = IsTechnicalContext ? new CommonUseTerm().GetCategoryRank() + 1 : 0;
            return s + base.GetCategoryRank();
        }
    }
}