using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Justification
{
    /// <summary>
    /// <![CDATA[
    /// The Model Penal Code defines self-defense in § 3.04(1) as "justifiable when the actor 
    /// believes that such force is immediately necessary for the purpose of protecting himself 
    /// against the use of unlawful force by such other person on the present occasion."
    /// ]]>
    /// </summary>
    public class DefenseOfSelf : DefenseOfBase
    {
        public DefenseOfSelf() : base(ExtensionMethods.Defendant) {  }

        public DefenseOfSelf(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {

        }

        /// <summary>
        /// (4) an objectively reasonable fear of injury or death
        /// </summary>
        public ObjectivePredicate<ILegalPerson> IsReasonableFearOfInjuryOrDeath { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var legalPerson = persons.Defendant();
            if (legalPerson == null)
                return false;
            if (!base.IsValid(persons))
                return false;

            if (!IsReasonableFearOfInjuryOrDeath(legalPerson))
            {
                AddReasonEntry($"{legalPerson.GetLegalPersonTypeName()}, {legalPerson.Name}, {nameof(IsReasonableFearOfInjuryOrDeath)} is false");
                return false;
            }

            return true;
        }
    }
}
