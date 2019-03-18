using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse
{
    /// <summary>
    /// Defendant is not subject to criminal prosecution because, being so young, they cannot form criminal intent
    /// </summary>
    public class Infancy: DefenseBase
    {
        public Infancy(ICrime crime) : base(crime)
        {
        }

        public Predicate<ILegalPerson> IsUnderage { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            var isAdult = !IsUnderage(defendant);

            if (isAdult)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsUnderage)} is false");
                return false;
            }

            return true;
        }
    }
}
