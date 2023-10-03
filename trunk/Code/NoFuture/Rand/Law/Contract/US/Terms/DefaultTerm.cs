using NoFuture.Law.Attributes;

namespace NoFuture.Law.Contract.US.Terms
{
    /// <summary>
    /// A term that applies unless otherwise agreed
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// what most contracting parties would want in most transactions
    /// ]]>
    /// </remarks>
    [Aka("background","gap-filler", "non-montonic")]
    public class DefaultTerm : ImpliedTerm
    {
        protected override string CategoryName => "Default";
    }
}
