using System;
using NoFuture.Rand.Law.US.Elements;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    public class TrespassToLand : TrespassBase
    {
        /// <summary>
        /// E.G. dust, noise, vibrations, sound waves, electromagnetic radiation, etc
        /// </summary>
        public Predicate<ILegalPerson> IsIntangibleEntry { get; set; } = lp => false;

        /// <summary>
        /// Only applicable to <see cref="IsIntangibleEntry"/>
        /// </summary>
        public Predicate<ILegalPerson> IsPhysicalDamage { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            ClearReasons();
            var tortfeasor = persons.Tortfeasor();
            if (tortfeasor == null)
                return false;

            if (!WithoutConsent(persons))
                return false;

            var obviousEntry = IsTangibleEntry(tortfeasor);

            var subtleEntry = IsIntangibleEntry(tortfeasor) && IsPhysicalDamage(tortfeasor);

            if (!obviousEntry && !subtleEntry)
            {
                AddReasonEntry($"tortfeasor, {tortfeasor.Name}, {nameof(IsTangibleEntry)} is false");
                AddReasonEntry($"tortfeasor, {tortfeasor.Name}, {nameof(IsIntangibleEntry)} " +
                               $"and {nameof(IsPhysicalDamage)} are both false");
                return false;
            }

            return true;
        }
    }
}
