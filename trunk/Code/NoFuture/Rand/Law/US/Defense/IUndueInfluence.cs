using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Defense
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
    public interface IUndueInfluence : ILegalConcept
    {
        /// <summary>
        /// <![CDATA[(1) discussion of the transaction at an unusual or inappropriate time,]]>
        /// </summary>
        Predicate<ILegalPerson> IsUnusualTime { get; set; }

        /// <summary>
        /// <![CDATA[(2) consummation of the transaction in an unusual place,]]>
        /// </summary>
        Predicate<ILegalPerson> IsUnusualLocation { get; set; }

        /// <summary>
        /// <![CDATA[(3) insistent demand that the business be finished at once]]>
        /// </summary>
        Predicate<ILegalPerson> IsInsistentOnImmediateCompletion { get; set; }

        /// <summary>
        /// <![CDATA[(4) extreme emphasis on untoward consequences of delay,]]>
        /// </summary>
        Predicate<ILegalPerson> IsExtremeEmphasisOnConsequencesOfDelay { get; set; }

        /// <summary>
        /// <![CDATA[(5) the use of multiple persuaders by the dominant side against a single servient party,]]>
        /// </summary>
        Predicate<ILegalPerson> IsMultiPersuadersOnServientParty { get; set; }

        /// <summary>
        /// <![CDATA[(6) absence of third-party advisers to the servient party,]]>
        /// </summary>
        Predicate<ILegalPerson> IsAbsentAdvisorsToServientParty { get; set; }

        /// <summary>
        /// <![CDATA[(7) statements that there is no time to consult financial advisers or attorneys]]>
        /// </summary>
        Predicate<ILegalPerson> IsInsistentOnNoTime4Advisors { get; set; }

        /// <summary>
        /// <![CDATA[
        /// If a number of these elements are simultaneously present, 
        /// the persuasion may be characterized as excessive.
        /// ]]>
        /// </summary>
        int NumberNeededToTestTrue { get; set; }
    }
}