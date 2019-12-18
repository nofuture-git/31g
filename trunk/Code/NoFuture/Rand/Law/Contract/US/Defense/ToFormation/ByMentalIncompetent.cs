using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Contract.US.Defense.ToFormation
{
    public class ByMentalIncompetent<T> : DefenseBase<T> where T : ILegalConcept
    {
        /// <summary>
        /// A person considered by the court to be without the capacity to contract
        /// </summary>
        public ByMentalIncompetent(IContract<T> contract) : base(contract)
        {
        }

        public virtual Predicate<ILegalPerson> IsMentallyIncompetent { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = this.Offeror(persons);
            var offeree = this.Offeree(persons);
            if (offeree == null || offeror == null)
                return false;

            if (!base.IsValid(offeror, offeree))
                return false;

            var offerorTitle = offeror.GetLegalPersonTypeName();
            var offereeTitle = offeree.GetLegalPersonTypeName();

            var isMental = IsMentallyIncompetent ?? (lp => false);
            if (isMental(offeror))
            {
                AddReasonEntry($"the {offerorTitle} named {offeror.Name} is " +
                               "a mentally incompetent");
                return true;
            }

            if (isMental(offeree))
            {
                AddReasonEntry($"the {offereeTitle} named {offeree.Name} is " +
                               "a mentally incompetent");
                return true;
            }

            return false;
        }
    }
}
