using System;
using System.Linq;
using NoFuture.Rand.Law.Criminal.US.Elements.AgainstProperty.Theft;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AttendantCircumstances
{
    /// <summary>
    /// A relationship between <see cref="ILegalPerson"/> that is based on trust or confidence
    /// </summary>
    /// <remarks>
    /// lessor-lessee, debtor-creditor, pledgor-pledgee, settlor-trustee
    /// </remarks>
    public class FiduciaryRelationship : AttendantCircumstanceBase
    {
        public Func<ILegalPerson, ILegalPerson, bool> IsTrustBetween { get; set; } = (lp1, lp2) => false;
        public Func<ILegalPerson, ILegalPerson, bool> IsConfidenceBetween { get; set; } = (lp1, lp2) => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            return false;
        }

        public override bool IsValid(IActusReus criminalAct, params ILegalPerson[] persons)
        {
            var steal = criminalAct as ConsolidatedTheft;
            if (steal == null)
            {
                AddReasonEntry($"{nameof(FiduciaryRelationship)} is an {nameof(IAttendantElement)} " +
                               $"only upon type {nameof(ByTaking)}");
                return base.IsValid(criminalAct, persons);
            }

            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var defendantTitle = defendant.GetLegalPersonTypeName();
            var victims = persons.Victims().ToList();
            if (!victims.Any())
                return false;

            foreach (var victim in victims)
            {
                var victimTitle = victim.GetLegalPersonTypeName();
                var isTrust = IsTrustBetween(defendant, victim);
                var isConfident = IsConfidenceBetween(defendant, victim);
                AddReasonEntry($"{defendantTitle} {defendant.Name}, {nameof(IsTrustBetween)} " +
                               $"is {isTrust} for {victimTitle} {victim.Name}");
                AddReasonEntry($"{defendantTitle} {defendant.Name}, {nameof(IsConfidenceBetween)} " +
                               $"is {isConfident} for {victimTitle} {victim.Name}");
                if (isTrust || isConfident)
                    return true;
            }

            return false;
        }
    }
}
