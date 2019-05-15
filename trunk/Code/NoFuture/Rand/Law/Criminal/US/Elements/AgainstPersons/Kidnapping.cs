using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    /// <summary>
    /// despite the name, the act is applicable to any person - minor or otherwise
    /// </summary>
    [EtymologyNote("English", "'kid' + 'nab'", "join of")]
    public class Kidnapping : FalseImprisonment
    {
        [Aka("movement", "carrying away")]
        public Predicate<ILegalPerson> IsAsportation { get; set; } = lp => false;

        /// <summary>
        /// Sum of money paid for the release of a prisoner
        /// </summary>
        public Predicate<ILegalPerson> IsRansom { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (!base.IsValid(persons))
                return false;

            var ia = IsAsportation(defendant);
            var ir = IsRansom(defendant);

            if (!ia && !ir)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsAsportation)} is false");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsRansom)} is false");
                return false;
            }

            return true;
        }

        public override bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var intent = (criminalIntent as DeadlyWeapon)?.UtilizedWith ?? criminalIntent;
            var isValidIntent = intent is Purposely || intent is SpecificIntent;
            if (!isValidIntent)
            {
                AddReasonEntry($"{nameof(Kidnapping)} requires intent " +
                               $"of {nameof(Purposely)} or {nameof(SpecificIntent)}");
                return false;
            }

            return true;
        }
    }
}
