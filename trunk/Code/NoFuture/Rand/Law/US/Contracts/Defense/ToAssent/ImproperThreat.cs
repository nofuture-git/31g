using System;

namespace NoFuture.Rand.Law.US.Contracts.Defense.ToAssent
{
    /// <summary>
    /// <![CDATA[leaves victim no reasonable alternative ]]>
    /// </summary>
    /// <remarks>
    /// <![CDATA[Restatement (Second) of Contracts § 176(1)]]>
    /// </remarks>    
    public class ImproperThreat<T> : DefenseBase<T>
    {
        public ImproperThreat(IContract<T> contract) : base(contract) { }

        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 176(1)(a)]]>
        /// </summary>
        public Predicate<ILegalPerson> IsCrimeOrTort { get; set; } = lp => false;

        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 176(1)(b)]]>
        /// </summary>
        public Predicate<ILegalPerson> IsProsecutionAsCriminal { get; set; } = lp => false;

        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 176(1)(c)]]>
        /// </summary>
        public Predicate<ILegalPerson> IsUseCivilProcessInBadFaith { get; set; } = lp => false;

        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 176(1)(d)]]>
        /// </summary>
        public Predicate<ILegalPerson> IsBreachOfGoodFaithDuty { get; set; } = lp => false;

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (IsCrimeOrTort(offeror))
            {
                AddReasonEntry($"{offeror?.Name} is threatened with a crime or tort");
                return true;
            }
            if (IsCrimeOrTort(offeree))
            {
                AddReasonEntry($"{offeree?.Name} is threatened with a crime or tort");
                return true;
            }

            if (IsProsecutionAsCriminal(offeror))
            {
                AddReasonEntry($"{offeror?.Name} is threatened with criminal prosecution");
                return true;
            }
            if (IsProsecutionAsCriminal(offeree))
            {
                AddReasonEntry($"{offeree?.Name} is threatened with criminal prosecution");
                return true;
            }

            if (IsUseCivilProcessInBadFaith(offeror))
            {
                AddReasonEntry($"{offeror?.Name} is threatened with use of civil process in bad faith");
                return true;
            }
            if (IsUseCivilProcessInBadFaith(offeree))
            {
                AddReasonEntry($"{offeree?.Name} is threatened with use of civil process in bad faith");
                return true;
            }

            if (IsBreachOfGoodFaithDuty(offeror))
            {
                AddReasonEntry($"{offeror?.Name} is threatened with breach of duty in good faith");
                return true;
            }
            if (IsBreachOfGoodFaithDuty(offeree))
            {
                AddReasonEntry($"{offeree?.Name} is threatened with breach of duty in good faith");
                return true;
            }

            return false;
        }
    }
}
