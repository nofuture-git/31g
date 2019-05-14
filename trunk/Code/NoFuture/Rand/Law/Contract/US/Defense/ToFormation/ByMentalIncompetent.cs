using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Contract.US.Defense.ToFormation
{
    /// <inheritdoc cref="IMentalIncompetent"/>
    public class ByMentalIncompetent<T> : DefenseBase<T>, IMentalIncompetent where T : ILegalConcept
    {
        public ByMentalIncompetent(IContract<T> contract) : base(contract)
        {
        }

        public virtual Predicate<ILegalPerson> IsMentallyIncompetent { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.Offeror();
            var offeree = persons.Offeree();

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
    }
}
