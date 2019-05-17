using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US
{
    public class IntentionalTort : LegalConcept
    {
        public IAct Act { get; set; }

        public IIntent Intent { get; set; }

        public Causation Causation { get; set; }

        public IAssault Contact { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var tortfeasor = persons.Tortfeasor();
            if (tortfeasor == null)
                return false;

            if (!Act.IsValid(persons))
            {
                AddReasonEntry($"tortfeasor, {tortfeasor.Name}, {nameof(Act)} {nameof(IsValid)} is false");
                return false;
            }
            if (!Intent.IsValid(persons))
            {
                AddReasonEntry($"tortfeasor, {tortfeasor.Name}, {nameof(Intent)} {nameof(IsValid)} is false");
                return false;
            }
            if (!Causation.IsValid(persons))
            {
                AddReasonEntry($"tortfeasor, {tortfeasor.Name}, {nameof(Causation)} {nameof(IsValid)} is false");
                return false;
            }
            if (!Contact.IsValid(persons))
            {
                AddReasonEntry($"tortfeasor, {tortfeasor.Name}, {nameof(Contact)} {nameof(IsValid)} is false");
                return false;
            }

            return true;
        }
    }
}
