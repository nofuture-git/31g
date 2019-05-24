using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    [EtymologyNote("Latin", "quare clausum fregit", "wherefore he broke the close")]
    public class TrespassToLand : TortTrespass
    {
        /// <summary>
        /// E.G. dust, noise, vibrations, sound waves, electromagnetic radiation, etc
        /// </summary>
        public Predicate<ILegalPerson> IsIntangibleEntry { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var tortfeasor = persons.Tortfeasor();
            if (tortfeasor == null)
                return false;

            if (!WithoutConsent(persons))
                return false;

            var obviousEntry = IsTangibleEntry(tortfeasor);

            var subtleEntry = IsIntangibleEntry(tortfeasor) && IsPhysicalDamage(persons);

            if (!obviousEntry && !subtleEntry)
            {
                AddReasonEntry($"tortfeasor, {tortfeasor.Name}, {nameof(IsTangibleEntry)} is false");
                AddReasonEntry($"tortfeasor, {tortfeasor.Name}, {nameof(IsIntangibleEntry)} " +
                               $"and {nameof(PropertyDamage)} are both false");
                return false;
            }

            return true;
        }

    }
}
