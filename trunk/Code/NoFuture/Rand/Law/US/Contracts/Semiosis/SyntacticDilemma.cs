﻿using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.US.Contracts.Semiosis
{
    /// <summary>
    /// A type to handle the problem of a court having to determine 
    /// the syntactic composition of the contract terms (e.g. XOR, OR, AND, etc.)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SyntacticDilemma<T> : DilemmaBase<T> where T : IObjectiveLegalConcept
    {
        public SyntacticDilemma(IContract<T> contract) : base(contract)
        {
        }

        public Predicate<IEnumerable<Term<object>>> IsIntendedComposition { get; set; } = et => false;

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (!TryGetTerms(offeror, offeree))
            {
                return false;
            }

            var offerorIsValid = IsIntendedComposition(OfferorTerms);
            var offereeIsValid = IsIntendedComposition(OffereeTerms);

            if (!offerorIsValid && !offereeIsValid)
            {
                AddReasonEntry($"neither composition from {offeror.Name} nor {offeree.Name} is valid");
                return false;
            }

            var isOnlyOneValid = offerorIsValid ^ offereeIsValid;

            if (!isOnlyOneValid)
            {
                AddReasonEntry($"the compositions between {offeror.Name} and {offeree.Name} are not mutually exclusive.");
                return false;
            }

            if (offerorIsValid)
            {
                AddReasonEntry($"{offeror.Name}'s composition, and NOT {offeree.Name}, is the intended one");
            }

            if (offereeIsValid)
            {
                AddReasonEntry($"{offeree.Name}'s composition, and NOT {offeror.Name}, is the intended one");
            }
            return true;
        }
    }
}
