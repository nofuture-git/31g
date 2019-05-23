using System;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstPersons
{
    /// <inheritdoc cref="IDominionOfForce"/>
    /// <inheritdoc cref="IAssault"/>
    /// <summary>
    /// Is not to cause physical contact; rather, it is to cause the 
    /// victim to fear physical contact
    /// </summary>
    public class ThreatenedBattery : UnoHomine, IDominionOfForce, IAssault, IActusReus, IElement
    {
        public ThreatenedBattery(): this(ExtensionMethods.Defendant) { }

        public ThreatenedBattery(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
            Imminence = new Imminence(getSubjectPerson);
        }
        public Predicate<ILegalPerson> IsByThreatOfViolence { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsPresentAbility { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsApparentAbility { get; set; } = lp => false;

        public Imminence Imminence { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetSubjectPerson(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            if (!IsByThreatOfViolence(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsByThreatOfViolence)} is false");
                return false;
            }

            var pAble = IsPresentAbility(defendant);
            var aAble = IsApparentAbility(defendant);

            if (!pAble && !aAble)
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsPresentAbility)} is false");
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsApparentAbility)} is false");
                return false;
            }

            if (Imminence == null)
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(Imminence)} is unassigned");
                return false;
            }

            if (!Imminence.IsValid(persons))
            {
                AddReasonEntryRange(Imminence.GetReasonEntries());
                return false;
            }

            return true;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var intent = (criminalIntent as DeadlyWeapon)?.UtilizedWith ?? criminalIntent;
            var isValidIntent = intent is Purposely || intent is SpecificIntent;
            if (!isValidIntent)
            {
                AddReasonEntry($"{nameof(ThreatenedBattery)} requires intent " +
                               $"of {nameof(Purposely)} or {nameof(SpecificIntent)}");
                return false;
            }

            return true;
        }
    }
}
