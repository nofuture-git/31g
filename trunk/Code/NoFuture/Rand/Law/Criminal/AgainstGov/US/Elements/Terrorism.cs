using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.AgainstGov.US.Elements
{
    /// <summary>
    /// <![CDATA[
    /// the unlawful use of force and violence against persons or property to intimidate
    /// or coerce a government, the civilian population, or any segment thereof,
    /// in furtherance of political or social objectives" (28 C.F.R. Section 0.85).
    /// ]]>
    /// </summary>
    public class Terrorism : CriminalBase, ICapitalOffense, IBattery
    {
        public Predicate<ILegalPerson> IsByViolence { get; set; }

        public Predicate<ILegalPerson> IsSocioPoliticalObjective { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!IsByViolence(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByViolence)} is false");
                return false;
            }

            if (!IsSocioPoliticalObjective(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsSocioPoliticalObjective)} is false");
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
