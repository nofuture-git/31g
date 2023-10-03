using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.Property.US.FormsOf.InTerra.Sequential;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;

namespace NoFuture.Law.Property.US.FormsOf.InTerra
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

        [Aka("abandoned", "surrendered")]
        public Predicate<ILessee> IsVacated { get; set; } = ee => false;

        public Predicate<ILessee> IsBreachLeaseCondition { get; set; } = ee => false;

        /// <summary>
        /// Typically only allowed for non-residential property (i.e. commercial).
        /// </summary>
        public Predicate<ILessor> IsPeaceableSelfHelpReentry { get; set; } = rr => false;

        [Aka("dispossessory proceedings")]
        public Predicate<ILessor> IsJudicialProcessReentry { get; set; } = rr => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var lessor = this.Lessor(persons);
            var lessee = this.Lessee(persons);

            if (lessor == null || lessee == null)
                return false;

            var orTitle = lessor.GetLegalPersonTypeName();
            var eeTitle = lessee.GetLegalPersonTypeName();

            //landlord has no right to evict
            if (!IsLeaseExpired && !IsBreachLeaseCondition(lessee) && !IsVacated(lessee))
            {
                AddReasonEntry($"{orTitle} {lessor.Name} and {eeTitle} {lessee.Name}, {nameof(IsLeaseExpired)}, " +
                               $"{nameof(IsBreachLeaseCondition)} and {nameof(IsVacated)} are all false");
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
