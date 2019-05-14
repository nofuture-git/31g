using System;
using NoFuture.Rand.Law.Criminal.Inchoate.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.Inchoate.US.Defense
{
    /// <inheritdoc cref="IImpossibility"/>
    public class Impossibility : InchoateDefenseBase, IImpossibility
    {
        public Impossibility(ICrime crime) : base(crime)
        {
        }

        public Predicate<ILegalPerson> IsLegalImpossibility { get; set; } = lp => false;

        public virtual Predicate<ILegalPerson> IsFactualImpossibility { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (!TestIsActusReusOfType(typeof(Attempt)))
                return false;

            if (IsFactualImpossibility(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsFactualImpossibility)} is true");
                AddReasonEntry("factual impossibility as a defense does not work");
            }

            if (!IsLegalImpossibility(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsLegalImpossibility)} is false");
                return false;
            }

            return true;
        }
    }
}
