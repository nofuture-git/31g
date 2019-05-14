using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense.Insanity;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse.Insanity
{
    /// <inheritdoc cref="MNaghten" />
    /// <inheritdoc cref="IIrresistibleImpulse" />
    public class IrresistibleImpulse : MNaghten, IIrresistibleImpulse
    {
        public Predicate<ILegalPerson> IsVolitional { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (IsVolitional(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsVolitional)} is true");
                return false;
            }

            return base.IsValid(persons);
        }
    }
}
