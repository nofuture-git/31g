using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts.Defense
{
    /// <summary>
    /// <![CDATA[
    /// A contract may be voidable due to its manner\process of 
    /// formation (e.g. gun-to-the-head) or it may be voidable in its 
    /// result (e.g. unconscionable).
    /// ]]>
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// Voidable is not the same as "void" - a void contract simply never was, 
    /// a voidable contract can be "avoided" without there being a breach.
    /// A "void" contract is simply illegal (e.g. murder-for-hire)
    /// ]]>
    /// </remarks>
    [Aka("procedural unconscionability", "substantive unconscionability")]
    public interface IVoidable : ILegalConcept
    {
    }
}
