using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.Terms.Convenants
{
    /// <summary>
    /// promise that you will defend against someone who claims title.
    /// </summary>
    /// <remarks>
    /// applicable only when someone disturbs the grantee&apos;s ownership
    /// </remarks>
    [Aka("warranty of quiet enjoyment")]
    public class QuietEnjoymentTerm : Term<ILegalProperty>
    {
        public QuietEnjoymentTerm(string name, ILegalProperty reference) : base(name, reference)
        {
        }

        public QuietEnjoymentTerm(string name, ILegalProperty reference, params ITermCategory[] categories) : base(name, reference, categories)
        {
        }
    }
}
