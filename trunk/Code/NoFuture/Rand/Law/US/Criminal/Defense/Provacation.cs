using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Criminal.Defense
{
    /// <summary>
    /// The nature of how a physical attack is provoked
    /// </summary>
    [Aka("unprovoked attack rule")]
    public class Provacation : DefenseBase
    {
        public Provacation(ICrime crime) : base(crime)
        {
        }

        /// <summary>
        /// Preeminent test, when true all other tests are irrelevant.
        /// </summary>
        public Predicate<ILegalPerson> IsInitiatorOfAttack { get; set; } = lp => false;

        /// <summary>
        /// One of two possible exceptions to unprovoked attack rule - e.g. slapping someone who then pulls out a gun.
        /// </summary>
        public Predicate<ILegalPerson> IsInitiatorRespondingToExcessiveForce { get; set; } = lp => false;

        /// <summary>
        /// Other possible exception to the unprovoked attack rule
        /// i.e. the attackor withdraws but the attackee pursues - reversing the roles
        /// </summary>
        public Predicate<ILegalPerson> IsInitiatorWithdraws { get; set; } = lp => false;

        /// <summary>
        /// Asserts that the defendant was unprovoked
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

            if (IsInitiatorRespondingToExcessiveForce(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsInitiatorRespondingToExcessiveForce)} is true");
                return true;
            }

            if (IsInitiatorWithdraws(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsInitiatorWithdraws)} is true");
                return true;
            }

            return false;
        }
    }
}
