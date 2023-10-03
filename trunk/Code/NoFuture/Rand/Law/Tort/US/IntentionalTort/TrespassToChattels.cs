using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.US;

namespace NoFuture.Law.Tort.US.IntentionalTort
{
    /// <summary>
    /// <![CDATA[ RESTATEMENT (SECOND) OF TORTS § 218 (1965). ]]>
    /// </summary>
    [Aka("trespass to personal property")]
    public class TrespassToChattels : TrespassToProperty
    {
        /// <summary>
        /// dispossession alone, without further damages, is actionable
        /// </summary>
        public Predicate<ILegalPerson> IsCauseDispossession { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var tortfeasor = this.Tortfeasor(persons);
            if (tortfeasor == null)
                return false;

            if (!WithoutConsent(persons))
                return false;

            var dipossed = IsCauseDispossession(tortfeasor);
            var damaged = IsPhysicalDamage(persons);
            var title = tortfeasor.GetLegalPersonTypeName();
            if(!dipossed && !damaged)
            {
                AddReasonEntry($"{title} {tortfeasor.Name}, {nameof(PropertyDamage)} is false");
                AddReasonEntry($"{title} {tortfeasor.Name}, {nameof(IsCauseDispossession)} is false");
                return false;
            }
            return true;
        }
    }
}
