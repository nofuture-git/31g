using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts.Terms
{
    /// <summary>
    /// conditions read into a contract by courts (e.g. Notice)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// even if the implied term was suggested 
    /// to the original parties drafting the contract
    /// they would decline to include it, saying 
    /// something like "duh!"
    /// ]]>
    /// </remarks>
    [Aka("Constructive")]
    [Note("so obvious it goes without saying")]
    public class ImpliedTerm : TermCategory
    {
        protected override string CategoryName => "Implied";
    }
}
