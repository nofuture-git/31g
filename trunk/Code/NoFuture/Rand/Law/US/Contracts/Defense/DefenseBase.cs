using NoFuture.Rand.Law.US.Contracts.Semiosis;

namespace NoFuture.Rand.Law.US.Contracts.Defense
{
    public abstract class DefenseBase<T> : DilemmaBase<T>, IVoidable where T : IObjectiveLegalConcept
    {
        protected DefenseBase(IContract<T> contract) : base(contract)
        {
        }

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            if (offeror == null)
            {
                AddReasonEntry($"The {nameof(offeror)} is unassigned");
                return false;
            }

            if (offeree == null)
            {
                AddReasonEntry($"The {nameof(offeree)} is unassigned");
                return false;
            }

            if (Contract == null)
            {
                AddReasonEntry($"there was not contract between {offeror.Name} and {offeree.Name}");
                return false;
            }

            var isValidContract = Contract.IsValid(offeror, offeree);
            AddReasonEntryRange(Contract.GetReasonEntries());

            if (!isValidContract)
            {
                AddReasonEntry("this defense is only applicable to valid contrax");
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
