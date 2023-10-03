using System;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;

namespace NoFuture.Law.Criminal.US.Defense.Excuse
{
    /// <summary>
    /// Defendant is not subject to criminal prosecution because, being so young, they cannot form criminal intent
    /// </summary>
    public class Infancy: DefenseBase
    {
        public Infancy() : base(ExtensionMethods.Defendant) { }

        public Infancy(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var legalPerson = this.Defendant(persons);
            if (legalPerson == null)
                return false;
            Predicate<ILegalPerson> isUnderage = lp => lp is IChild;
            var isAdult = !isUnderage(legalPerson);

            if (isAdult)
            {
                AddReasonEntry($"{legalPerson.GetLegalPersonTypeName()}, {legalPerson.Name}, is {nameof(IChild)} is false");
                return false;
            }

            return true;
        }
    }
}
