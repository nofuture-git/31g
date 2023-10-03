using NoFuture.Law.US;

namespace NoFuture.Law.Contract.US.Defense
{
    public abstract class DefenseBase<T> : DilemmaBase<T>, IVoidable, IDefense where T : ILegalConcept
    {
        protected DefenseBase(IContract<T> contract) : base(contract)
        {
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = this.Offeror(persons);
            var offeree = this.Offeree(persons);
            if (offeree == null || offeror == null)
                return false;

            if (Contract == null)
            {
                AddReasonEntry($"there was not contract between {offeror.Name} and {offeree.Name}");
                return false;
            }

            var isValidContract = Contract.IsValid(offeror, offeree);
            AddReasonEntryRange(Contract.GetReasonEntries());

            if (!isValidContract)
            {
                AddReasonEntry("this defense is only applicable to a valid contract");
                return false;
            }

            return true;
        }

        /// <summary>
        /// <![CDATA[
        /// Since all Defense are Voidable by definition, it implies 
        /// they are legal since an illegal contract is just "Void" 
        /// ]]>
        /// </summary>
        public override bool IsEnforceableInCourt => true;
    }
}
