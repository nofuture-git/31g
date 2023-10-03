using System;
using NoFuture.Law.Criminal.US.Elements.Act;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.AgainstGov
{
    /// <summary>
    /// <![CDATA[
    /// the unlawful use of force and violence against persons or property to intimidate
    /// or coerce a government, the civilian population, or any segment thereof,
    /// in furtherance of political or social objectives" (28 C.F.R. Section 0.85).
    /// ]]>
    /// </summary>
    public class Terrorism : LegalConcept, ICapitalOffense, IBattery, IElement
    {
        public Predicate<ILegalPerson> IsByViolence { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsSocioPoliticalObjective { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();

            if (!IsByViolence(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsByViolence)} is false");
                return false;
            }

            if (!IsSocioPoliticalObjective(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsSocioPoliticalObjective)} is false");
                return false;
            }

            return true;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return true;
        }
    }
}
