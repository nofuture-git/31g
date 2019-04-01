using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    /// <summary>
    /// <![CDATA[ RESTATEMENT (SECOND) OF TORTS § 218 (1965). ]]>
    /// </summary>
    [Aka("trespass to personal property")]
    public class TrespassToChattels : TortTrespass
    {
        /// <summary>
        /// dispossession alone, without further damages, is actionable
        /// </summary>
        public Predicate<ILegalPerson> IsCauseDispossession { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var tortfeasor = persons.Tortfeasor();
            if (tortfeasor == null)
                return false;

            if (!WithoutConsent(persons))
                return false;

            var dipossed = IsCauseDispossession(tortfeasor);
            var damaged = IsPhysicalDamage(persons);

            if(!dipossed && !damaged)
            {
                AddReasonEntry($"tortfeasor, {tortfeasor.Name}, {nameof(PropertyDamage)} is false");
                AddReasonEntry($"tortfeasor, {tortfeasor.Name}, {nameof(IsCauseDispossession)} is false");
                return false;
            }
            return true;
        }
    }
}
