﻿using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Contract.US.Defense.ToFormation
{
    /// <inheritdoc cref="IAgeOfMajority"/>
    public class ByMinor<T> : DefenseBase<T>, IAgeOfMajority where T : ILegalConcept
    {
        public ByMinor(IContract<T> contract) : base(contract)
        {
        }

        public virtual Predicate<ILegalPerson> IsUnderage { get; set; }

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
            var offeror = persons.Offeror();
            var offeree = persons.Offeree();

            if (!base.IsValid(offeror, offeree))
                return false;

            var isMinor = IsUnderage ?? (lp => false);
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
