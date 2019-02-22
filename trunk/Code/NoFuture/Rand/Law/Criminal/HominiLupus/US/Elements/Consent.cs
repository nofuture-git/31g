using System;
using NoFuture.Rand.Law.Criminal.US.Elements;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    /// <summary>
    /// the inability or firm denial of willingness to engage in intercourse
    /// </summary>
    public class Consent : AttendantCircumstances
    {
        public Predicate<ILegalPerson> IsCapableThereof { get; set; } = lp => false;

        /// <summary>
        /// This is the typical place where prosecution\defense battle
        /// </summary>
        public Predicate<ILegalPerson> IsFirmDenial { get; set; } = lp => true;

        public Func<ILegalPerson[], ILegalPerson> GetVictim { get; set; } = lps => null;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var victim = GetVictim(persons);
            if (victim == null)
            {
                AddReasonEntry($"{nameof(GetVictim)} returned null");
                return false;
            }

            if (!IsCapableThereof(victim))
            {
                AddReasonEntry($"victim, {victim.Name}, {nameof(IsCapableThereof)} is false");
                return false;
            }

            if (IsFirmDenial(victim))
            {
                AddReasonEntry($"victim, {victim.Name}, {nameof(IsFirmDenial)} is true");
                return false;
            }

            AddReasonEntry($"victim, {victim.Name}, {nameof(IsFirmDenial)} is false");
            return true;
        }
    }
}
