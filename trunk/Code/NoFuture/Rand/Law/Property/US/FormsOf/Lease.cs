using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.FormsOf
{
    public abstract class Lease<T> : PropertyBase, ILease<T> where T : ILegalProperty
    {
        private Func<T, ILease<T>> _acceptance;

        protected Lease() : base(null)
        {
            _acceptance = GetAcceptance;
        }

        public new T SubjectProperty { get; set; }

        public Predicate<T> IsResidenceHome { get; set; } = sp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var lessor = this.Lessor(persons);
            var lessee = this.Lessee(persons);

            if (lessor == null || lessee == null)
                return false;

            var orTitle = lessor.GetLegalPersonTypeName();
            var eeTitle = lessee.GetLegalPersonTypeName();

            if (Inception == DateTime.MinValue)
            {
                AddReasonEntry($"{orTitle} {lessor.Name} and {eeTitle} {lessee.Name}, {nameof(Inception)} is unassigned");
                return false;
            }

            if (Terminus != null && Terminus.Value <= Inception)
            {
                AddReasonEntry($"{orTitle} {lessor.Name} and {eeTitle} {lessee.Name}, {nameof(Terminus)} " +
                               $"date of {Terminus.Value.ToShortDateString()} is less-than-equal-to " +
                               $"the {nameof(Inception)} date of {Inception.ToShortDateString()}");
                return false;
            }

            if (Assent != null && !Assent.IsValid(persons))
            {
                AddReasonEntryRange(Assent.GetReasonEntries());
                return false;
            }

            if (SubjectProperty == null || Offer == null)
            {
                AddReasonEntry($"{orTitle} {lessor.Name} and {eeTitle} {lessee.Name}, " +
                               $"{nameof(SubjectProperty)} '{SubjectProperty?.Name}' " +
                               $"or {nameof(Offer)} '{Offer?.Name}' is unassigned");
                return false;
            }

            if (Acceptance?.Invoke(Offer) == null)
            {
                AddReasonEntry($"{orTitle} {lessor.Name} and {eeTitle} {lessee.Name}, {nameof(Acceptance)} " +
                               $"using {nameof(Offer)} '{Offer.Name}' returned nothing");
                return false;
            }

            return true;

        }

        public virtual DateTime Inception { get; set; }
        public virtual DateTime? Terminus { get; set; }
        public virtual bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = Inception <= dt;
            var beforeOrOnToDt = Terminus == null || Terminus.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }

        public virtual T Offer
        {
            get => SubjectProperty;
            set => SubjectProperty = value;
        }

        /// <summary>
        /// Optional, defaults to SubjectProperty return this
        /// </summary>
        public virtual Func<T, ILease<T>> Acceptance
        {
            get => _acceptance;
            set => _acceptance = value;
        }

        /// <summary>
        /// Optional, defaults to &quot;consent is given&quot;
        /// </summary>
        public virtual IAssent Assent { get; set; } = Consent.IsGiven();

        protected virtual ILease<T> GetAcceptance(T property)
        {
            var isSameProperty = SubjectProperty?.Equals(property) ?? false;
            return isSameProperty ? this : null;
        }
    }
}