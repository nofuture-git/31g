using System;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;

namespace NoFuture.Rand.Law.Criminal.US.Elements.Intent
{
    /// <summary>
    /// Inference of <see cref="IMensRea"/> (intent) when the defendant uses a deadly weapon
    /// </summary>
    public class DeadlyWeapon : MensRea
    {
        public string WeaponName { get; }

        public DeadlyWeapon(IMensRea utilizedWith = null)
        {
            WeaponName = nameof(DeadlyWeapon);
            UtilizedWith = utilizedWith ?? new SpecificIntent();
        }

        public DeadlyWeapon(string name, IMensRea utilizedWith = null)
        {
            WeaponName = name;
            UtilizedWith = utilizedWith ?? new SpecificIntent();
        }

        /// <summary>
        /// Would have to be some kind of noun - it cannot be a thought or evil-look, hex, etc.
        /// </summary>
        public Predicate<ILegalPerson> IsUtilizable { get; set; } = lp => true;

        /// <summary>
        /// The underlying intent with which the deadly weapon is used
        /// </summary>
        public IMensRea UtilizedWith { get; }

        /// <summary>
        /// any firearm, other weapon, device, instrument, material or substance [...] capable of producing death
        /// </summary>
        public Predicate<ILegalPerson> IsCanCauseDeath { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!IsUtilizable(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsUtilizable)} is false");
                return false;
            }

            if (!IsCanCauseDeath(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsUtilizable)} is false");
                return false;
            }

            return true;
        }

        public override int CompareTo(object obj)
        {
            if (obj is MaliceAforethought 
                || obj is SpecificIntent)
                return -1;
            return 0;
        }
    }
}
