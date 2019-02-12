using System;

namespace NoFuture.Rand.Law.US.Criminal.Defense.Justification
{
    public class Provacation : DefenseBase
    {
        public Provacation(ICrime crime) : base(crime)
        {
        }

        public Predicate<ILegalPerson> IsInitiatorOfAttack { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsResponseToExcessiveForce { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsWithdraws { get; set; } = lp => false;

        /// <summary>
        /// Asserts that the defendant was provoked
        /// </summary>
        /// <param name="offeror"></param>
        /// <param name="offeree"></param>
        /// <returns></returns>
        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;

            if (!IsInitiatorOfAttack(defendant))
            {
                return true;
            }

            if (IsResponseToExcessiveForce(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsResponseToExcessiveForce)} is true");
                return true;
            }

            if (IsWithdraws(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsWithdraws)} is true");
                return true;
            }

            return false;
        }
    }
}
