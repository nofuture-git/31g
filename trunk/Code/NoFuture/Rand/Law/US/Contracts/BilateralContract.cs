using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    /// <inheritdoc />
    /// <summary>
    /// Differs from unilateral in that it deals with a promise for a promise or performance
    /// </summary>
    public class BilateralContract : ComLawContract<Promise>
    {
        [Note("assent: expression of approval or agreement")]
        public virtual IAssent MutualAssent { get; set; }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            //test consideration
            if (!base.IsValid(offeror, offeree))
                return false;

            //test mutual assent
            if (MutualAssent == null)
            {
                AddReasonEntry($"{nameof(MutualAssent)} is null");
                return false;
            }

            if (!MutualAssent.IsValid(offeror, offeree))
            {
                AddReasonEntry($"{nameof(MutualAssent)}.{nameof(IsValid)} returned false");
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            var allEntries = GetReasonEntries() as List<string> ?? new List<string>();
            if(MutualAssent != null)
                allEntries.AddRange(MutualAssent.GetReasonEntries());
            if(Consideration != null)
                allEntries.AddRange(Consideration.GetReasonEntries());
            return string.Join(Environment.NewLine, allEntries);
        }
    }
}