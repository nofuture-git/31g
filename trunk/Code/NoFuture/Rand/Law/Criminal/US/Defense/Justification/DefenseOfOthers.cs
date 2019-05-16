using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Justification
{
    /// <summary>
    /// <![CDATA[
    /// under the circumstances as the actor believes them to be, the 
    /// person whom he seeks to protect would be justified in using 
    /// such protective force (Model Penal Code § 3.05(1) (b))
    /// ]]>
    /// </summary>
    public class DefenseOfOthers : DefenseOfBase
    {
        public DefenseOfOthers() : base(ExtensionMethods.Defendant) { }

        public DefenseOfOthers(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {

        }
        /// <summary>
        /// <![CDATA[
        /// The subjective test where it reasonably appeared that the victim had a right to self-defense 
        /// and, thereby, that right is conveyed to the defendant
        /// ]]>
        /// </summary>
        public SubjectivePredicate<ILegalPerson> IsReasonablyAppearedInjuryOrDeath { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var legalPerson = GetSubjectPerson(persons);
            if (legalPerson == null)
                return false;
            if (!base.IsValid(persons))
                return false;

            if (!IsReasonablyAppearedInjuryOrDeath(legalPerson))
            {
                AddReasonEntry($"{legalPerson.GetLegalPersonTypeName()}, {legalPerson.Name}, {nameof(IsReasonablyAppearedInjuryOrDeath)} is false");
                return false;
            }

            return true;
        }
    }
}
