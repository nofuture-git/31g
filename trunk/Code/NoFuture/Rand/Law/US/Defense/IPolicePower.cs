using System;

namespace NoFuture.Rand.Law.US.Defense
{
    /// <summary>
    /// The capacity of the states to regulate behavior and enforce order
    /// </summary>
    public interface IPolicePower : ILegalConcept
    {
        /// <summary>
        /// <![CDATA[
        /// since the nation-state is the single holder of legitimate 
        /// use of physical violence - an agent is an extension of that power.
        /// ]]>
        /// </summary>
        Predicate<ILegalPerson> IsAgentOfTheState { get; set; }

        /// <summary>
        /// <![CDATA[
        /// It is much easier to perceive and realize the existence and sources 
        /// of [the police power] than to mark its boundaries, or prescribe limits to exercise.
        /// ]]>
        /// </summary>
        Predicate<ILegalPerson> IsReasonableUseOfForce { get; set; }
    }
}