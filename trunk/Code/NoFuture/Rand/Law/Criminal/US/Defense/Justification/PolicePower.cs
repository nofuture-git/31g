using System;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Defense
{
    /// <summary>
    /// The capacity of the states to regulate behavior and enforce order
    /// </summary>
    public class PolicePower : DefenseBase
    {
        public PolicePower() : base(ExtensionMethods.Defendant) { }

        public PolicePower(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        /// <summary>
        /// <![CDATA[
        /// since the nation-state is the single holder of legitimate 
        /// use of physical violence - an agent is an extension of that power.
        /// ]]>
        /// </summary>
        public Predicate<ILegalPerson> IsAgentOfTheState { get; set; } = lp => false;

        /// <summary>
        /// <![CDATA[
        /// It is much easier to perceive and realize the existence and sources 
        /// of [the police power] than to mark its boundaries, or prescribe limits to exercise.
        /// ]]>
        /// </summary>
        public Predicate<ILegalPerson> IsReasonableUseOfForce { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var legalPerson = GetSubjectPerson(persons);
            if (legalPerson == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }
            var lpPersonType = legalPerson.GetLegalPersonTypeName();
            if (!IsAgentOfTheState(legalPerson))
            {
                AddReasonEntry($"{lpPersonType}, {legalPerson.Name}, {nameof(IsAgentOfTheState)} is false");
                return false;
            }

            if (!IsReasonableUseOfForce(legalPerson))
            {
                AddReasonEntry($"{lpPersonType}, {legalPerson.Name}, {nameof(IsReasonableUseOfForce)} is false");
                return false;
            }

            return true;
        }
    }
}
