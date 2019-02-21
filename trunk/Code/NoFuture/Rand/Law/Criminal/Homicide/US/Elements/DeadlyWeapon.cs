using System;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;

namespace NoFuture.Rand.Law.Criminal.Homicide.US.Elements
{
    /// <summary>
    /// Inference of murder <see cref="IMensRea"/> (intent) when the defendant uses a deadly weapon
    /// </summary>
    public class DeadlyWeapon : MensRea
    {
        public string WeaponName { get; }

        public DeadlyWeapon()
        {
            WeaponName = nameof(DeadlyWeapon);
        }

        public DeadlyWeapon(string name)
        {
            WeaponName = name;
        }

        /// <summary>
        /// Would have to be some kind of noun - it cannot be a thought or evil-look, hex, etc.
        /// </summary>
        public Predicate<ILegalPerson> IsUtilized { get; set; } = lp => false;

        /// <summary>
        /// any firearm, other weapon, device, instrument, material or substance [...] capable of producing death
        /// </summary>
        public Predicate<ILegalPerson> IsCanCauseDeath { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!IsUtilized(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsUtilized)} is false");
                return false;
            }

            if (!IsCanCauseDeath(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsUtilized)} is false");
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
