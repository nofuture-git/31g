using System;
using System.Linq;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Shared
{
    /// <summary>
    /// The property interest of a deceased cotenant is divided equally among the
    /// remaining cotenants
    /// </summary>
    public class JointTenancy : TenancyInCommon
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;

            var cotenants = this.Cotenants(persons).ToList();
            if (!cotenants.Any())
                return false;

            DateTime? creationDate = null;
            ILegalConcept instrument = null;
            var portion = 0D;

            for (var i = 0; i < cotenants.Count; i++)
            {
                var cotenant = cotenants[i];
                var title = cotenant.GetLegalPersonTypeName();
                if (i == 0)
                {
                    creationDate = InterestCreationDate(cotenants[i]);
                    instrument = InterestCreationInstrument(cotenants[i]);
                    portion = InterestFraction(cotenants[i]);
                    continue;
                }

                if (portion == 0D)
                {
                    AddReasonEntry($"{title} {cotenant.Name} {nameof(InterestFraction)} is null");
                    return false;
                }
                if (instrument == null)
                {
                    AddReasonEntry($"{title} {cotenant.Name} {nameof(InterestCreationInstrument)} is null");
                    return false;
                }
                if (creationDate == null)
                {
                    AddReasonEntry($"{title} {cotenant.Name} {nameof(InterestCreationDate)} is null");
                    return false;
                }

                if (!creationDate.Equals(InterestCreationDate(cotenant)))
                {
                    AddReasonEntry($"{title} {cotenant.Name} {nameof(InterestCreationDate)} does not equal {creationDate}");
                    return false;
                }

                if (!instrument.Equals(InterestCreationInstrument(cotenant)))
                {
                    AddReasonEntry($"{title} {cotenant.Name} {nameof(InterestCreationInstrument)} does not equal {instrument}");
                    return false;
                }

                if (Math.Abs(portion - InterestFraction(cotenant)) > 0.01)
                {
                    AddReasonEntry($"{title} {cotenant.Name} {nameof(InterestFraction)} does not equal {portion}");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// All cotenants interest must have been created at the same time
        /// </summary>
        public virtual Func<ILegalPerson, DateTime?> InterestCreationDate { get; set; } = lp => null;

        /// <summary>
        /// All cotenants interest must be created in the same instrument
        /// </summary>
        public virtual Func<ILegalPerson, ILegalConcept> InterestCreationInstrument { get; set; } = lp => null;

        /// <summary>
        /// All cotenants interest must be equal portions
        /// </summary>
        public virtual Func<ILegalPerson, double> InterestFraction { get; set; } = lp => 0D;
    }
}
