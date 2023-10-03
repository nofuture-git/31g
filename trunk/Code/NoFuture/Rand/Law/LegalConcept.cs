namespace NoFuture.Law
{
    /// <inheritdoc cref="ILegalConcept"/>
    public abstract class LegalConcept : Rationale, ILegalConcept
    {
        public abstract bool IsValid(params ILegalPerson[] persons);

        public virtual bool IsEnforceableInCourt => true;
    }
}
