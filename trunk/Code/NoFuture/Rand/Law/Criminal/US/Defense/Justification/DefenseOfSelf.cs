using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Justification
{
    /// <inheritdoc cref="IDefenseOfSelf"/>
    public class DefenseOfSelf : DefenseOfBase, IDefenseOfSelf
    {
        public DefenseOfSelf() : base(ExtensionMethods.Defendant) {  }

        public DefenseOfSelf(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {

        }

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
