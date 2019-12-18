using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Justification
{
    public class DefenseOfProperty : DefenseOfBase
    {
        public DefenseOfProperty() : base(ExtensionMethods.Defendant) { }

        public DefenseOfProperty(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {

        }

        public LegalProperty Property { get; set; }

        /// <summary>
        /// This is objective OR subjective based on jurisdiction
        /// </summary>
        public Predicate<ILegalPerson> IsBeliefProtectProperty { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var legalPerson = GetSubjectPerson(persons);
            if (legalPerson == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }
            if (!base.IsValid(persons))
                return false;
            if (!IsBeliefProtectProperty(legalPerson))
            {
                AddReasonEntry($"{legalPerson.GetLegalPersonTypeName()}, {legalPerson.Name}, {nameof(IsBeliefProtectProperty)} is false");
                return false;
            }

            return true;
        }
    }
}
