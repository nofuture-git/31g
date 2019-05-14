using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Contract.US.Defense.ToAssent
{
    /// <inheritdoc cref="IImproperThreatByOne"/>
    /// <inheritdoc cref="IImproperThreatByTwo"/>
    public class ImproperThreat<T> : DefenseBase<T>, IImproperThreatByOne, IImproperThreatByTwo where T : ILegalConcept
    {
        public ImproperThreat(IContract<T> contract) : base(contract) { }

        public Predicate<ILegalPerson> IsCrimeOrTort { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsProsecutionAsCriminal { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsUseCivilProcessInBadFaith { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsBreachOfGoodFaithDuty { get; set; } = lp => false;

        protected internal bool IsImproperByOne(ILegalPerson lp)
        {
            if (IsCrimeOrTort(lp))
            {
                AddReasonEntry($"{lp?.Name} is threatened with a crime or tort");
                return true;
            }

            if (IsProsecutionAsCriminal(lp))
            {
                AddReasonEntry($"{lp?.Name} is threatened with criminal prosecution");
                return true;
            }

            if (IsUseCivilProcessInBadFaith(lp))
            {
                AddReasonEntry($"{lp?.Name} is threatened with use of civil process in bad faith");
                return true;
            }

            if (IsBreachOfGoodFaithDuty(lp))
            {
                AddReasonEntry($"{lp?.Name} is threatened with breach of duty in good faith");
                return true;
            }

            return false;
        }

        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 176(2)]]>
        /// </summary>
        protected internal bool IsImproperByTwo(ILegalPerson lp)
        {
            if (!IsUnfairTerms(lp))
                return false;

            AddReasonEntry("the resulting exchange is not on fair terms");

            if (IsAllHarmNoBenefit(lp))
            {
                AddReasonEntry($"threatened act would harm the {lp?.Name} and " +
                               "would not significantly benefit the party making the threat");
                return true;
            }

            if (IsSignificantViaPriorUnfairDeal(lp))
            {
                AddReasonEntry($"the effectiveness of the threat in inducing {lp?.Name}'s " +
                               "assent is significant by prior unfair dealing " +
                               "with the party");
                return true;
            }

            if (IsUsePowerIllegitimateEnds(lp))
            {
                AddReasonEntry($"{lp.Name} is threatened with a use of power for " +
                               "illegitimate ends");
                return true;
            }

            return false;
        }

        public Predicate<ILegalPerson> IsUnfairTerms { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsAllHarmNoBenefit { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsSignificantViaPriorUnfairDeal { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsUsePowerIllegitimateEnds { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.Offeror();
            var offeree = persons.Offeree();

            return IsImproperByOne(offeror) || IsImproperByOne(offeree)
                                            || IsImproperByTwo(offeror) || IsImproperByTwo(offeree);
        }
    }
}
