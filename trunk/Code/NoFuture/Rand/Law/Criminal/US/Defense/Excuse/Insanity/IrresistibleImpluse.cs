using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse.Insanity
{
    /// <summary>
    /// similar to <see cref="MNaghten"/> only its considered simpler to prove and 
    /// is rejected in most jurisdictions
    /// </summary>
    public class IrresistibleImpluse : MNaghten
    {
        /// <summary>
        /// Idea that the defendant can not control their conduct because of the mental defect
        /// </summary>
        [Aka("free choice")]
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
