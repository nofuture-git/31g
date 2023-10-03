using System;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Defense.Excuse
{
    /// <summary>
    /// <![CDATA[
    /// intoxication which (a) is not self-induced ... is an affirmative defense if 
    /// by reason of such intoxication 211 Criminal Law the actor at the time of his 
    /// conduct lacks substantial capacity either to appreciate its criminality 
    /// [wrongfulness] or  to conform his conduct to the requirements of law 
    /// (Model Penal Code § 2.08 (4)).
    /// ]]>
    /// </summary>
    public class Intoxication : DefenseBase
    {
        public Intoxication() : base(ExtensionMethods.Defendant) { }

        public Intoxication(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        /// <summary>
        /// <![CDATA[
        /// Involuntary intoxication could affect the defendant's ability to form 
        /// criminal intent, thus negating specific intent
        /// ]]>
        /// </summary>
        public Predicate<ILegalPerson> IsInvoluntary { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsIntoxicated { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var legalPerson = this.Defendant(persons);
            if (legalPerson == null)
                return false;
            var lpTypeName = legalPerson.GetLegalPersonTypeName();
            if (!IsIntoxicated(legalPerson))
            {
                AddReasonEntry($"{lpTypeName}, {legalPerson.Name}, {nameof(IsIntoxicated)} is false");
                return false;
            }

            if (!IsInvoluntary(legalPerson))
            {
                AddReasonEntry($"{lpTypeName}, {legalPerson.Name}, {nameof(IsInvoluntary)} is true");
                return false;
            }

            return true;
        }
    }
}
