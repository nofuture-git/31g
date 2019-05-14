using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Contract.US.Defense.ToAssent
{
    /// <inheritdoc cref="IUndueInfluence"/>
    public class ByUndueInfluence<T> : DefenseBase<T>, IUndueInfluence where T : ILegalConcept
    {
        public ByUndueInfluence(IContract<T> contract) : base(contract)
        {
        }

        public Predicate<ILegalPerson> IsUnusualTime { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsUnusualLocation { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsInsistentOnImmediateCompletion { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsExtremeEmphasisOnConsequencesOfDelay { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsMultiPersuadersOnServientParty { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsAbsentAdvisorsToServientParty { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsInsistentOnNoTime4Advisors { get; set; } = lp => false;

        public int NumberNeededToTestTrue { get; set; } = 5;

        /// <summary>
        /// The objective test for the legal concept
        /// </summary>
        /// <param name="persons">
        /// <![CDATA[
        ///  "fiduciaries" a party in a position of trust (e.g. lawyers, 
        /// physicians, trustees, guardians, etc.)
        /// the two sides are labelled as "dominant" party and the "servient" party 
        /// ]]>
        /// </param>
        /// <returns></returns>
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.Offeror();
            var offeree = persons.Offeree();

            if (!base.IsValid(offeror, offeree))
                return false;
            var testTrueCount = 0;
            foreach (var kvp in GetPredicate2ReasonString())
            {
                var p = kvp.Key;
                if (p(offeror) || p(offeree))
                {
                    AddReasonEntry(kvp.Value);
                    testTrueCount += 1;
                }
            }

            if (testTrueCount >= NumberNeededToTestTrue)
            {
                AddReasonEntry("therefore, persuasion is excessive");
                return true;
            }

            return false;

        }

        protected internal virtual Dictionary<Predicate<ILegalPerson>, string> GetPredicate2ReasonString()
        {
            return new Dictionary<Predicate<ILegalPerson>, string>
            {
                {IsUnusualTime, "discussion of the transaction at an unusual or inappropriate time,"},
                {IsUnusualLocation, "consummation of the transaction in an unusual place,"},
                {IsInsistentOnImmediateCompletion, "insistent demand that the business be finished at once,"},
                {IsExtremeEmphasisOnConsequencesOfDelay, "extreme emphasis on untoward consequences of delay,"},
                {IsMultiPersuadersOnServientParty, "the use of multiple persuaders by the dominant side against a single servient party,"},
                {IsAbsentAdvisorsToServientParty, "absence of third-party advisers to the servient party,"},
                {IsInsistentOnNoTime4Advisors, "statements that there is no time to consult financial advisers or attorneys,"}
            };
        }
    }
}
