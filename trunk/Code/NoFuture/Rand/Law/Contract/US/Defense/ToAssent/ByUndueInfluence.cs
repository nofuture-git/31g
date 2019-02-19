using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Contract.US.Defense.ToAssent
{
    /// <summary>
    /// <![CDATA[
    /// improper pressure to enter into a contract not the result of direct threats
    /// ]]>
    /// </summary>
    /// <remarks>
    /// ODORIZZI v. BLOOMFIELD SCHOOL DISTRICT 
    /// Court of Appeal of California, Second Appellate District 
    /// 246 Cal.App. 2d 123, 54 Cal.Rptr. 533 (2d Dist. 1966)    
    /// </remarks>
    [Aka("overpersuasion")]
    public class ByUndueInfluence<T> : DefenseBase<T> where T : ILegalConcept
    {
        public ByUndueInfluence(IContract<T> contract) : base(contract)
        {
        }

        /// <summary>
        /// <![CDATA[(1) discussion of the transaction at an unusual or inappropriate time,]]>
        /// </summary>
        public Predicate<ILegalPerson> IsUnusualTime { get; set; } = lp => false;

        /// <summary>
        /// <![CDATA[(2) consummation of the transaction in an unusual place,]]>
        /// </summary>
        public Predicate<ILegalPerson> IsUnusualLocation { get; set; } = lp => false;

        /// <summary>
        /// <![CDATA[(3) insistent demand that the business be finished at once]]>
        /// </summary>
        public Predicate<ILegalPerson> IsInsistentOnImmediateCompletion { get; set; } = lp => false;

        /// <summary>
        /// <![CDATA[(4) extreme emphasis on untoward consequences of delay,]]>
        /// </summary>
        public Predicate<ILegalPerson> IsExtremeEmphasisOnConsequencesOfDelay { get; set; } = lp => false;

        /// <summary>
        /// <![CDATA[(5) the use of multiple persuaders by the dominant side against a single servient party,]]>
        /// </summary>
        public Predicate<ILegalPerson> IsMultiPersuadersOnServientParty { get; set; } = lp => false;

        /// <summary>
        /// <![CDATA[(6) absence of third-party advisers to the servient party,]]>
        /// </summary>
        public Predicate<ILegalPerson> IsAbsentAdvisorsToServientParty { get; set; } = lp => false;

        /// <summary>
        /// <![CDATA[(7) statements that there is no time to consult financial advisers or attorneys]]>
        /// </summary>
        public Predicate<ILegalPerson> IsInsistentOnNoTime4Advisors { get; set; } = lp => false;

        /// <summary>
        /// <![CDATA[
        /// If a number of these elements are simultaneously present, 
        /// the persuasion may be characterized as excessive.
        /// ]]>
        /// </summary>
        public int NumberNeededToTestTrue { get; set; } = 5;

        /// <summary>
        /// The objective test for the legal concept
        /// </summary>
        /// <param name="offeror">
        /// <![CDATA[
        /// a.k.a. "dominant" party, 
        /// "fiduciaries" a party in a position of trust (e.g. lawyers, 
        /// physicians, trustees, guardians, etc.)
        /// ]]>
        /// </param>
        /// <param name="offeree">
        /// <![CDATA[
        /// a.k.a. "servient" party
        /// ]]>
        /// </param>
        /// <returns></returns>
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = Contract.GetOfferor(persons);
            var offeree = Contract.GetOfferee(persons);

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
