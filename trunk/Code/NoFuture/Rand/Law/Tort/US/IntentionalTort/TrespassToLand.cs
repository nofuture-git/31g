using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.US;

namespace NoFuture.Law.Tort.US.IntentionalTort
{
    [EtymologyNote("Latin", "quare clausum fregit", "wherefore he broke the close")]
    public class TrespassToLand : TrespassToProperty
    {
        public TrespassToLand(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        public TrespassToLand() : base(ExtensionMethods.Tortfeasor) { }

        /// <summary>
        /// E.G. dust, noise, vibrations, sound waves, electromagnetic radiation, etc
        /// </summary>
        public Predicate<ILegalPerson> IsIntangibleEntry { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var tortfeasor = this.Tortfeasor(persons);
            if (tortfeasor == null)
                return false;

            if (!WithoutConsent(persons))
                return false;

            var obviousEntry = IsTangibleEntry(tortfeasor);

            var subtleEntry = IsIntangibleEntry(tortfeasor) && IsPhysicalDamage(persons);
            var title = tortfeasor.GetLegalPersonTypeName();
            if (!obviousEntry && !subtleEntry)
            {
                AddReasonEntry($"{title} {tortfeasor.Name}, {nameof(IsTangibleEntry)} is false");
                AddReasonEntry($"{title} {tortfeasor.Name}, {nameof(IsIntangibleEntry)} " +
                               $"and {nameof(PropertyDamage)} are both false");
                return false;
            }

            return true;
        }

    }
}
