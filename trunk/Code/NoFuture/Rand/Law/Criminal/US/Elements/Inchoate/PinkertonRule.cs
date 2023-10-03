using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.Inchoate
{
    /// <summary>
    /// <![CDATA[
    /// A criminal act committed by a co-conspirator where their act becomes
    /// the defendant's act because it was a reasonably foreseeable crime of the 
    /// conspiracy they had together
    /// ]]>
    /// </summary>
    public class PinkertonRule : LegalConcept, IActusReus
    {
        public PinkertonRule(Conspiracy conspiracy)
        {
            Conspiracy = conspiracy;
        }

        public Conspiracy Conspiracy { get; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            if (Conspiracy == null)
            {
                AddReasonEntry($"{title} {defendant}, {nameof(PinkertonRule)} " +
                               $"cannot be applied with {nameof(Conspiracy)}");
                return false;
            }

            var isValid = Conspiracy.IsValid(persons);
            if (isValid == false)
                AddReasonEntryRange(Conspiracy.GetReasonEntries());
            return isValid;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return Conspiracy?.CompareTo(criminalIntent) ?? false;
        }
    }
}
