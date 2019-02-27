using System;
using System.Linq;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements
{
    /// <summary>
    /// A relationship between <see cref="ILegalPerson"/> that is based on trust or confidence
    /// </summary>
    public class Treachery : AttendantCircumstances
    {
        public Func<ILegalPerson, ILegalPerson, bool> IsTrustBetween { get; set; } = (lp1, lp2) => false;
        public Func<ILegalPerson, ILegalPerson, bool> IsConfidenceBetween { get; set; } = (lp1, lp2) => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            return false;
        }

        public override bool IsValid(IActusReus criminalAct, params ILegalPerson[] persons)
        {
            var steal = criminalAct as ByTaking;
            if (steal == null)
            {
                AddReasonEntry($"{nameof(Treachery)} is an {nameof(IAttendantElement)} only upon type {nameof(ByTaking)}");
                return base.IsValid(criminalAct, persons);
            }

            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            var victims = persons.Where(lp => lp is IVictim).ToList();
            if (!victims.Any())
                return false;

            foreach (var victim in victims)
            {
                var isTrust = IsTrustBetween(defendant, victim);
                var isConfident = IsConfidenceBetween(defendant, victim);
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsTrustBetween)} is {isTrust} for victim {victim.Name}");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsConfidenceBetween)} is {isConfident} for victim {victim.Name}");
                if (isTrust || isConfident)
                    return true;
            }

            return false;
        }
    }
}
