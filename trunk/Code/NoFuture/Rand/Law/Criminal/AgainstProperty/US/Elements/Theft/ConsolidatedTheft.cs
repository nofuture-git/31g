using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft
{
    /// <summary>
    /// Explanatory Note for Sections 223.1-223.9 of Model Penal Code
    /// </summary>
    public abstract class ConsolidatedTheft : CriminalBase, IActusReus
    {
        public virtual ILegalProperty SubjectOfTheft { get; set; }
        public virtual decimal? AmountOfTheft { get; set; }
        public virtual bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return true;
        }
    }
}
