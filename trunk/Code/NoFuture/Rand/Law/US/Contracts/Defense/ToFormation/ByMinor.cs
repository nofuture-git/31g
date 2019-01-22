using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.US.Contracts.Defense.ToFormation
{
    public class ByMinor<T> : DefenseBase<T>
    {
        public ByMinor(IContract<T> contract) : base(contract)
        {
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
            if (!base.IsValid(offeror, offeree))
                return false;

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
    }
}
