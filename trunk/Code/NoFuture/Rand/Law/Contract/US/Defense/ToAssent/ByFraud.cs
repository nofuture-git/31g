using System;
using System.Linq;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Contract.US.Defense.ToAssent
{
    /// <inheritdoc cref="IFraud"/>
    public class ByFraud<T> : DefenseBase<T>, IFraud where T : ILegalConcept
    {
        public ByFraud(IContract<T> contract) : base(contract) { }

        public IMisrepresentation Misrepresentation { get; set; }

        public Predicate<ILegalPerson> IsRecipientInduced { get; set; } = llp => false;

        public Predicate<ILegalPerson> IsRecipientRelianceReasonable { get; set; } = llp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.Offeror();
            var offeree = persons.Offeree();

            if (!base.IsValid(offeror, offeree))
                return false;

            var isRecipInduceFx = IsRecipientInduced ?? (lp => false);
            var isRecipReasonFx = IsRecipientRelianceReasonable ?? (lp => false);

            var predicates = new [] {false, false, false, false};

            if (Misrepresentation == null)
                return false;

            AddReasonEntry($"there was a {nameof(Misrepresentation)}");
            predicates[0] = true;

            if (Misrepresentation.IsFraudulent(offeror))
            {
                AddReasonEntry($"{offeror?.Name} was fraudulent in misrepresentation");
                predicates[1] = true;
            }
            if (Misrepresentation.IsFraudulent(offeree))
            {
                AddReasonEntry($"{offeree?.Name} was fraudulent in misrepresentation");
                predicates[1] = true;
            }
            if (Misrepresentation.IsMaterial(offeror))
            {
                AddReasonEntry($"{offeror?.Name} was material in misrepresentation");
                predicates[1] = true;
            }
            if (Misrepresentation.IsMaterial(offeree))
            {
                AddReasonEntry($"{offeree?.Name} was material in misrepresentation");
                predicates[1] = true;
            }

            if (isRecipInduceFx(offeror))
            {
                AddReasonEntry($"{offeror?.Name} was induced by misrepresentation");
                predicates[2] = true;
            }
            if (isRecipInduceFx(offeree))
            {
                AddReasonEntry($"{offeree?.Name} was induced by misrepresentation");
                predicates[2] = true;
            }

            if (isRecipReasonFx(offeror))
            {
                AddReasonEntry($"{offeror?.Name} reliance on misrepresentation was reasonable");
                predicates[3] = true;
            }
            if (isRecipReasonFx(offeree))
            {
                AddReasonEntry($"{offeree?.Name} reliance on misrepresentation was reasonable");
                predicates[3] = true;
            }

            return predicates.All(p => p);
        }

    }
}
