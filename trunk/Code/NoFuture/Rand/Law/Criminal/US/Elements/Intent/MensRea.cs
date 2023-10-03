namespace NoFuture.Law.Criminal.US.Elements.Intent
{
    /// <inheritdoc cref="IMensRea"/>
    public abstract class MensRea : LegalConcept, IMensRea, IElement
    {
        public abstract int CompareTo(object obj);

        public virtual bool CompareTo(IActusReus criminalAct, params ILegalPerson[] persons)
        {
            return true;
        }
    }
}
