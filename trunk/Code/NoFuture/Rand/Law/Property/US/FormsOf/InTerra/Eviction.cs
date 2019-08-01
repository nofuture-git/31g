using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra
{
    [Aka("dispossess a tenant")]
    public class Eviction : Leasehold
    {
        public Eviction()
        {

        }

        public Eviction(Leasehold lease)
        {
            if (lease == null)
                return;
            SubjectProperty = lease.SubjectProperty;
            Inception = lease.Inception;
            Terminus = lease.Terminus;
            Acceptance = lease.Acceptance;
            Offer = lease.Offer;
            Assent = lease.Assent;
        }

        public DateTime CurrentDateTime { get; set; } = DateTime.UtcNow;
        public Predicate<ILessee> IsBreachLeaseCondition { get; set; } = ee => false;

        /// <summary>
        /// Typically only allowed for non-residential property (i.e. commercial).
        /// </summary>
        public Predicate<ILessor> IsPeaceableSelfHelpReentry { get; set; } = rr => false;

        [Aka("dwelling-place")]
        public Predicate<RealProperty> IsResidenceHome { get; set; } = rr => false;

        [Aka("dispossessory proceedings")]
        public Predicate<ILessor> IsJudicialProcessReentry { get; set; } = rr => false;

        /// <summary>
        /// Eval&apos; <see cref="CurrentDateTime"/> <see cref="Leasehold.IsInRange"/>
        /// </summary>
        public bool IsLeaseExpired => IsInRange(CurrentDateTime);

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var lessor = this.Lessor(persons);
            var lessee = this.Lessee(persons);

            if (lessor == null || lessee == null)
                return false;

            var orTitle = lessor.GetLegalPersonTypeName();
            var eeTitle = lessee.GetLegalPersonTypeName();

            //landlord has no right to evict
            if (!IsLeaseExpired && !IsBreachLeaseCondition(lessee))
            {
                AddReasonEntry($"{orTitle} {lessor.Name} and {eeTitle} {lessee.Name}, both {nameof(IsLeaseExpired)} " +
                               $"and {nameof(IsBreachLeaseCondition)} are false");
                return false;
            }

            //lessor isn't the landlord
            if (!SubjectProperty.IsEntitledTo(lessor))
            {
                AddReasonEntry($"{orTitle} {lessor.Name}, {nameof(SubjectProperty)} {nameof(SubjectProperty.IsEntitledTo)} is false");
                return false;
            }

            var isHome = IsResidenceHome(SubjectProperty);

            var isPeaceful = !isHome ? IsPeaceableSelfHelpReentry(lessor) : IsJudicialProcessReentry(lessor);

            if (!isPeaceful)
            {
                var kindOfReentry = !isHome ? nameof(IsPeaceableSelfHelpReentry) : nameof(IsJudicialProcessReentry);
                AddReasonEntry($"{orTitle} {lessor.Name}, {kindOfReentry} is false");
                return false;
            }


            return base.IsValid(persons);
        }
    }
}
