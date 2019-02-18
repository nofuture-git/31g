using System;

namespace NoFuture.Rand.Law.US.Contracts.Remedy
{
    /// <inheritdoc />
    /// <summary>
    /// an order from a court, very much like an injunction, compelling a party to do what it was supposed to do
    /// </summary>
    /// <remarks>
    /// unique being when relevant information is difficult to obtain or unreliable
    /// </remarks>
    public class SpecificPerformance<T> : RemedyBase<T> where T : ILegalConcept
    {
        public SpecificPerformance(IContract<T> contract) : base(contract)
        {
        }

        /// <summary>
        /// <![CDATA[
        /// Restatement (Second) of Contracts § 360(a)
        /// difficulty in putting a price on it
        /// ]]>
        /// </summary>
        public Predicate<ILegalPerson> IsDifficultToProveDmg { get; set; } = lp => false;

        /// <summary>
        /// <![CDATA[
        /// Restatement (Second) of Contracts § 360(b)
        /// difficulty in paying someone else to do it
        /// ]]>
        /// </summary>
        public Predicate<ILegalPerson> IsDifficultToSubstitute { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = Contract.GetOfferor(persons);
            var offeree = Contract.GetOfferee(persons);

            if (IsDifficultToProveDmg(offeror) || IsDifficultToProveDmg(offeree))
            {
                AddReasonEntry("the money value of the contract between " +
                               $"{offeror?.Name} and {offeree?.Name} cannot be easily priced.");
                return true;
            }

            if (IsDifficultToSubstitute(offeror) || IsDifficultToSubstitute(offeree))
            {
                AddReasonEntry("the subject of the contract between " +
                               $"{offeror?.Name} and {offeree?.Name} cannot be substituted");
                return true;
            }

            return false;
        }
    }
}
