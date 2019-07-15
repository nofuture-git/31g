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
    public class QuietEnjoyment : Term<ILegalPerson>
    {
        public QuietEnjoyment(string name, ILegalPerson reference) : base(name, reference)
        {
        }

        public QuietEnjoyment(string name, ILegalPerson reference, params ITermCategory[] categories) : base(name, reference, categories)
        {
        }
    }
}
