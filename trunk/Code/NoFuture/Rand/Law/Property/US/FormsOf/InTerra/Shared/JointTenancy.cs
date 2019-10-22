using System;
using System.Linq;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Shared
{
    /// <summary>
    /// The property interest of a deceased cotenant is divided equally among the
    /// remaining cotenants
    /// </summary>
    /// <remarks>
    /// Under common law, the four predicates are called &quot;the four unities&quot;
    /// as interest, time, title and possession.
    /// </remarks>
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

                var nextCreationDate = InterestCreationDate(cotenant);
                if (!creationDate.Equals(nextCreationDate))
                {
                    AddReasonEntry($"{title} {cotenant.Name} {nameof(InterestCreationDate)}, {creationDate} does not equal {nextCreationDate}");
                    return false;
                }

                var nextInstrument = InterestCreationInstrument(cotenant);
                if (!instrument.Equals(nextInstrument))
                {
                    AddReasonEntry($"{title} {cotenant.Name} {nameof(InterestCreationInstrument)}, {instrument} does not equal {nextInstrument}");
                    return false;
                }

                var nextPortion = InterestFraction(cotenant);
                if (Math.Abs(portion - nextPortion) > 0.01)
                {
                    AddReasonEntry($"{title} {cotenant.Name} {nameof(InterestFraction)}, {portion} does not equal {nextPortion}");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// All cotenants interest must have been created at the same time
        /// </summary>
        [Aka("unity of time")]
        public virtual Func<ILegalPerson, DateTime?> InterestCreationDate { get; set; } = lp => null;

        /// <summary>
        /// All cotenants interest must be created in the same instrument
        /// </summary>
        [Aka("unity of title")]
        public virtual Func<ILegalPerson, ILegalConcept> InterestCreationInstrument { get; set; } = lp => null;

        /// <summary>
        /// All cotenants interest must be equal portions
        /// </summary>
        [Aka("unity of interest")]
        public virtual Func<ILegalPerson, double> InterestFraction { get; set; } = lp => 0D;

        [Aka("unity of possession")]
        public override Predicate<ILegalPerson> IsEqualRightToPossessWhole { get; set; } = lp => false;
    }
}
