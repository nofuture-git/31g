using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.US.Contracts.Defense
{
    /// <summary>
    /// <![CDATA[
    /// one of the "defenses to formation"
    /// ]]>
    /// </summary>
    public class Voidable<T> : ObjectiveLegalConcept
    {
        private readonly IContract<T> _contract;

        public Voidable(IContract<T> contract)
        {
            _contract = contract;
        }

        /// <summary>
        /// <![CDATA[
        /// contracts of minors are voidable
        /// ]]>
        /// </summary>
        public virtual Predicate<ILegalPerson> IsMinor { get; set; }

        /// <summary>
        /// <![CDATA[
        /// if the minor disaffirms the contract, the minor is not liable under it
        /// ]]>
        /// </summary>
        public virtual Predicate<ILegalPerson> IsDeclareVoid { get; set; }

        /// <summary>
        /// <![CDATA[
        /// where a minor has received "necessaries" under a contract, the 
        /// minor will be required to pay for the reasonable value of what 
        /// was provided
        /// ]]>
        /// </summary>
        public virtual Func<ILegalPerson, ISet<Term<object>>> GetNecessaries { get; set; }

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

            if (_contract == null)
            {
                AddReasonEntry($"there was not contract between {offeror.Name} and {offeree.Name}");
                return false;
            }

            var isValidContract = _contract.IsValid(offeror, offeree);
            AddReasonEntryRange(_contract.GetReasonEntries());

            if (!isValidContract)
            {
                AddReasonEntry("this defense is only applicable to valid contrax");
                return false;
            }

            var isMinor = IsMinor ?? (lp => false);
            var isDeclareVoid = IsDeclareVoid ?? (lp => false);

            if (isMinor(offeror) && isDeclareVoid(offeror))
            {
                AddReasonEntry($"the {nameof(offeror)} named {offeror.Name} is " +
                               "a minor and has declared the contract void");
                return true;
            }
            if (isMinor(offeree) && isDeclareVoid(offeree))
            {
                AddReasonEntry($"the {nameof(offeree)} named {offeree.Name} is " +
                               "a minor and has declared the contract void");
                return true;
            }

            return false;
        }

        public override bool IsEnforceableInCourt => true;
    }
}
