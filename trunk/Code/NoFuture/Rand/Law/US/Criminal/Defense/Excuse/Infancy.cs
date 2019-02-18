using System;

namespace NoFuture.Rand.Law.US.Criminal.Defense.Excuse
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
            var defendant = Crime.GetDefendant(persons);
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
