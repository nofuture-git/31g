using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse
{
    /// <inheritdoc cref="IEntrapment"/>
    public class Entrapment : DefenseBase, IEntrapment
    {
        public Entrapment() : base(ExtensionMethods.Defendant) { }

        public Entrapment(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        public Predicate<ILegalPerson> IsIntentOriginFromLawEnforcement { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var legalPerson = persons.Defendant();
            if (legalPerson == null)
                return false;

            if (!IsIntentOriginFromLawEnforcement(legalPerson))
            {
                AddReasonEntry($"{legalPerson.GetLegalPersonTypeName()}, {legalPerson.Name}, mens rea is {nameof(IsIntentOriginFromLawEnforcement)}");
                return false;
            }

            return true;
        }
    }
}
