using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Contract.US.Defense.ToFormation
{
    /// <summary> Contracts of minors are voidable </summary>
    public class ByMinor<T> : DefenseBase<T> where T : ILegalConcept
    {
        public ByMinor(IContract<T> contract) : base(contract)
        {
        }

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

            Predicate<ILegalPerson> isMinor = lp => lp is IChild;
            var isDeclareVoid = IsDeclareVoid ?? (lp => false);

            if (isMinor(offeror) && isDeclareVoid(offeror))
            {
                AddReasonEntry($"the {offerorTitle} named {offeror.Name} is " +
                               "a minor and has declared the contract void");
                return true;
            }
            if (isMinor(offeree) && isDeclareVoid(offeree))
            {
                AddReasonEntry($"the {offereeTitle} named {offeree.Name} is " +
                               "a minor and has declared the contract void");
                return true;
            }

            return false;
        }
    }
}
