﻿using System;
using System.Linq;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts.Defense.ToAssent
{
    /// <summary>
    /// <![CDATA[
    /// false statement with intent to mislead and damages result
    /// ]]>
    /// </summary>
    /// <remarks>
    /// <![CDATA[Restatement (Second) of Contracts § 164]]>
    /// </remarks>
    [Aka("misrepresentation")]
    public class ByFraud<T> : DefenseBase<T>, IVoidable
    {
        public ByFraud(IContract<T> contract) : base(contract)
        {
        }

        public Misrepresentation<T> Misrepresentation { get; set; }

        /// <summary>
        /// <![CDATA[3) that the misrepresentation induced the recipient to enter into the contract]]>
        /// </summary>
        public Predicate<ILegalPerson> IsRecipientInduced { get; set; }

        /// <summary>
        /// <![CDATA[(4) that the recipient’s reliance on the misrepresentation was reasonable]]>
        /// </summary>
        public Predicate<ILegalPerson> IsRecipientRelianceReasonable { get; set; }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (!base.IsValid(offeror, offeree))
                return false;

            var isRecipInduceFx = IsRecipientInduced ?? (lp => false);
            var isRecipReasonFx = IsRecipientRelianceReasonable ?? (lp => false);

            var predicates = new [] {false, false, false, false};

            if (Misrepresentation == null)
                return false;

            AddReasonEntry($"there was a {nameof(Misrepresentation)}");
            predicates[0] = true;

            if (Misrepresentation.IsFraudulent(offeror))
            {
                AddReasonEntry($"{offeror?.Name} was fraudulent in misrepresentation");
                predicates[1] = true;
            }
            if (Misrepresentation.IsFraudulent(offeree))
            {
                AddReasonEntry($"{offeree?.Name} was fraudulent in misrepresentation");
                predicates[1] = true;
            }
            if (Misrepresentation.IsMaterial(offeror))
            {
                AddReasonEntry($"{offeror?.Name} was material in misrepresentation");
                predicates[1] = true;
            }
            if (Misrepresentation.IsMaterial(offeree))
            {
                AddReasonEntry($"{offeree?.Name} was material in misrepresentation");
                predicates[1] = true;
            }

            if (isRecipInduceFx(offeror))
            {
                AddReasonEntry($"{offeror?.Name} was induced by misrepresentation");
                predicates[2] = true;
            }
            if (isRecipInduceFx(offeree))
            {
                AddReasonEntry($"{offeree?.Name} was induced by misrepresentation");
                predicates[2] = true;
            }

            if (isRecipReasonFx(offeror))
            {
                AddReasonEntry($"{offeror?.Name} reliance on misrepresentation was reasonable");
                predicates[3] = true;
            }
            if (isRecipReasonFx(offeree))
            {
                AddReasonEntry($"{offeree?.Name} reliance on misrepresentation was reasonable");
                predicates[3] = true;
            }

            return predicates.All(p => p);
        }

    }
}
