using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US.Elements
{
    /// <inheritdoc cref="ISexBipartitie"/>
    /// <summary>
    /// Offering, agreeing, or engaging in sexual conduct for money, property, or anything of value
    /// </summary>
    public class Prostitution : CriminalBase, IPossession, ISexBipartitie
    {
        public Predicate<ILegalPerson> IsSexualIntercourse { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsKnowinglyProcured { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsKnowinglyReceived { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            var procured = IsKnowinglyProcured(defendant);
            var received = IsKnowinglyReceived(defendant);

            if (!procured && !received)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsKnowinglyProcured)} is false");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsKnowinglyReceived)} is false");
                return false;
            }

            if (!IsSexualIntercourse(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsSexualIntercourse)} is false");
                return false;
            }

            return true;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var isValid = criminalIntent is StrictLiability || criminalIntent is Purposely ||
                          criminalIntent is Knowingly || criminalIntent is GeneralIntent;
            if (!isValid)
            {
                AddReasonEntry($"{nameof(Prostitution)} requires intent of " +
                               $"{nameof(StrictLiability)}, {nameof(Purposely)}, " +
                               $"{nameof(Knowingly)} or {nameof(GeneralIntent)}");
                return false;
            }

            return true;
        }

    }
}
