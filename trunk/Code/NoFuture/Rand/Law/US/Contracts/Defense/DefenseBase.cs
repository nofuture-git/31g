using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Rand.Law.US.Contracts.Defense
{
    public abstract class DefenseBase<T> : ObjectiveLegalConcept
    {
        private readonly IContract<T> _contract;
        protected DefenseBase(IContract<T> contract)
        {
            _contract = contract;
        }

        protected internal virtual IContract<T> Contract => _contract;

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
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
    }
}
