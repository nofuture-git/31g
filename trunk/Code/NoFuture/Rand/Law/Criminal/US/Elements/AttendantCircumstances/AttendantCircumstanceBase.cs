namespace NoFuture.Law.Criminal.US.Elements.AttendantCircumstances
{
    /// <summary>
    /// The facts surrounding an event
    /// </summary>
    public abstract class AttendantCircumstanceBase: LegalConcept, IAttendantElement
    {
        public virtual bool IsValid(IMensRea intent, params ILegalPerson[] persons)
        {
            return false;
        }

        public virtual bool IsValid(IActusReus criminalAct, params ILegalPerson[] persons)
        {
            return false;
        }
    }
}
