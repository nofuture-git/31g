using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse
{
    /// <summary>
    /// Defendant is not subject to criminal prosecution because, being so young, they cannot form criminal intent
    /// </summary>
    public class Infancy: DefenseBase
    {
        public Infancy() : base(ExtensionMethods.Defendant) { }

        public Infancy(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        /// <summary> Contracts of minors are voidable </summary>
        public Predicate<ILegalPerson> IsUnderage { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var legalPerson = persons.Defendant();
            if (legalPerson == null)
                return false;

            var isAdult = !IsUnderage(legalPerson);

            if (isAdult)
            {
                AddReasonEntry($"{legalPerson.GetLegalPersonTypeName()}, {legalPerson.Name}, {nameof(IsUnderage)} is false");
                return false;
            }

            return true;
        }
    }
}
