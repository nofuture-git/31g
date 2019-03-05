using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    /// <inheritdoc cref="IDominionOfForce"/>
    /// <inheritdoc cref="IAssault"/>
    /// <summary>
    /// Is not to cause physical contact; rather, it is to cause the 
    /// victim to fear physical contact
    /// </summary>
    public class ThreatenedBattery : CriminalBase, IDominionOfForce, IAssault, IActusReus
    {
        public Predicate<ILegalPerson> IsByThreatOfViolence { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsPresentAbility { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsApparentAbility { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!IsByThreatOfViolence(defendant))
            {
                AddReasonEntry($"defendant {defendant.Name}, {nameof(IsByThreatOfViolence)} is false");
                return false;
            }

            var pAble = IsPresentAbility(defendant);
            var aAble = IsApparentAbility(defendant);

            if (!pAble && !aAble)
            {
                AddReasonEntry($"defendant {defendant.Name}, {nameof(IsPresentAbility)} is false");
                AddReasonEntry($"defendant {defendant.Name}, {nameof(IsApparentAbility)} is false");
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
