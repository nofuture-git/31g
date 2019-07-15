using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.Terms.Convenants
{
    /// <summary>
    /// promise that you have disclosed all other peoples lawful rights over the property
    /// </summary>
    [Aka("warranty against encumbrances")]
    public class AgainstEncumbrances : Term<ILegalPerson>
    {
        public AgainstEncumbrances(string name, ILegalPerson reference) : base(name, reference)
        {
        }

        public AgainstEncumbrances(string name, ILegalPerson reference, params ITermCategory[] categories) : base(name, reference, categories)
        {
        }
    }
}
