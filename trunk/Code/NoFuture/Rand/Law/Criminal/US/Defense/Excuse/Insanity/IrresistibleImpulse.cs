using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Defense.Excuse.Insanity
{
    /// <inheritdoc cref="MNaghten" />
    /// <summary>
    /// similar to <see cref="MNaghten"/> only its considered simpler to prove and 
    /// is rejected in most jurisdictions
    /// </summary>
    public class IrresistibleImpulse : MNaghten
    {
        public IrresistibleImpulse(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        public IrresistibleImpulse() : this(ExtensionMethods.Defendant) { }

        /// <summary>
        /// Idea that the defendant can not control their conduct because of the mental defect
        /// </summary>
        [Aka("free choice")]
        public Predicate<ILegalPerson> IsVolitional { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var legalPerson = this.Defendant(persons);
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
