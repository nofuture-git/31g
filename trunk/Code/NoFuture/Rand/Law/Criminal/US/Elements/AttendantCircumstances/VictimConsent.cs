using System;
using System.Linq;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AttendantCircumstances
{
    /// <summary>
    /// the inability or firm denial of willingness to engage
    /// </summary>
    public class VictimConsent : AttendantCircumstanceBase, IConsent
    {
        public Predicate<ILegalPerson> IsCapableThereof { get; set; } = lp => false;

        /// <summary>
        /// This is the typical place where prosecution\defense battle
        /// </summary>
        public Predicate<ILegalPerson> IsApprovalExpressed { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (persons == null || !persons.Any())
            {
                AddReasonEntry($"{nameof(persons)} is null or empty");
                return false;
            }

            foreach (var victim in persons.Victims())
            {
                if (!IsCapableThereof(victim))
                {
                    AddReasonEntry($"victim, {victim.Name}, {nameof(IsCapableThereof)} is false");
                    return false;
                }

                if (!IsApprovalExpressed(victim))
                {
                    AddReasonEntry($"victim, {victim.Name}, {nameof(IsApprovalExpressed)} is false");
                    return false;
                }
            }

            return true;
        }
    }
}
