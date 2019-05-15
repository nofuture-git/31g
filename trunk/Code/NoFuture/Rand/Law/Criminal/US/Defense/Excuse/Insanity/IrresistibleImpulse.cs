using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense.Insanity;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse.Insanity
{
    /// <inheritdoc cref="MNaghten" />
    /// <inheritdoc cref="IIrresistibleImpulse" />
    public class IrresistibleImpulse : MNaghten, IIrresistibleImpulse
    {
        public IrresistibleImpulse(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        public IrresistibleImpulse() : this(ExtensionMethods.Defendant) { }

        public Predicate<ILegalPerson> IsVolitional { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var legalPerson = persons.Defendant();
            if (legalPerson == null)
                return false;

            if (IsVolitional(legalPerson))
            {
                AddReasonEntry($"{legalPerson.GetLegalPersonTypeName()}, {legalPerson.Name}, {nameof(IsVolitional)} is true");
                return false;
            }

            return base.IsValid(persons);
        }
    }
}
