using System;

namespace NoFuture.Rand.Law.US.Criminal.Defense
{
    public class Imminence : DefenseBase
    {
        public Imminence(ICrime crime) : base(crime)
        {
        }

        public Func<ILegalPerson, TimeSpan> GetMinimumResponseTime { get; set; } = lp => TimeSpan.Zero;

        public Predicate<TimeSpan> IsImmediatePresent { get; set; } = ts => false;

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;

            var ts = GetMinimumResponseTime(defendant);
            if (!IsImmediatePresent(ts))
            {
                AddReasonEntry($"defendant, {defendant.Name}, with minimum " +
                               $"response time of {ts} had {nameof(IsImmediatePresent)} as false");
                return false;
            }

            return true;
        }
    }
}
