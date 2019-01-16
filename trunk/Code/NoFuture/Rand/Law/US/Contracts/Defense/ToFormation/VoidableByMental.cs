using System;

namespace NoFuture.Rand.Law.US.Contracts.Defense.ToFormation
{
    public class VoidableByMental<T> : DefenseBase<T>
    {
        public VoidableByMental(IContract<T> contract) : base(contract)
        {
        }

        /// <summary>
        /// A person considered by the court to be without the capacity to contract
        /// </summary>
        public virtual Predicate<ILegalPerson> IsMentallyIncompetent { get; set; }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (!base.IsValid(offeror, offeree))
                return false;

            var isMental = IsMentallyIncompetent ?? (lp => false);
            if (isMental(offeror))
            {
                AddReasonEntry($"the {nameof(offeror)} named {offeror.Name} is " +
                               "a mentally incompetent");
                return true;
            }

            if (isMental(offeree))
            {
                AddReasonEntry($"the {nameof(offeree)} named {offeree.Name} is " +
                               "a mentally incompetent");
                return true;
            }

            return false;
        }

        public override bool IsEnforceableInCourt => true;
    }
}
