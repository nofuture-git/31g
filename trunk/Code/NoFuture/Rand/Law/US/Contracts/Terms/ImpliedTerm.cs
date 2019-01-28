using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts.Terms
{
    /// <summary>
    /// conditions read into a contract by courts (e.g. Notice)
    /// </summary>
    [Aka("Constructive")]
    public class ImpliedTerm : TermCategory
    {
        protected override string CategoryName => "Implied";
    }
}
