using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse
{
    /// <inheritdoc cref="IIntoxication"/>
    public class Intoxication : DefenseBase, IIntoxication
    {
        public Predicate<ILegalPerson> IsInvoluntary { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsIntoxicated { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (!IsIntoxicated(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsIntoxicated)} is false");
                return false;
            }

            if (!IsInvoluntary(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsInvoluntary)} is true");
                return false;
            }

            return true;
        }
    }
}
