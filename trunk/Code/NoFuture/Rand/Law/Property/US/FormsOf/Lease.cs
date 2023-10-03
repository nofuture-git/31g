using System;
using System.Collections.Generic;
using NoFuture.Law.US;

namespace NoFuture.Law.Property.US.FormsOf
{
    /// <inheritdoc cref="ILease{T}"/>
    public abstract class Lease<T> : PropertyBase, ILease<T> where T : ILegalProperty
    {
        private Func<T, ILease<T>> _acceptance;

        protected Lease() : base(null)
        {
            _acceptance = GetAcceptance;
        }

        public DateTime CurrentDateTime { get; set; } = DateTime.UtcNow;

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

        public bool IsLeaseExpired => IsInRange(CurrentDateTime);

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
        /// Optional, defaults to SubjectProperty, returns this instance
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

        /// <summary>
        /// This is default implementation of <see cref="Acceptance"/>
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected virtual ILease<T> GetAcceptance(T property)
        {
            var isSameProperty = SubjectProperty?.Equals(property) ?? false;
            return isSameProperty ? this : null;
        }

        /// <summary>
        /// Optional details concerning the exact terms present in a lease agreement
        /// </summary>
        public virtual Func<ILegalPerson, ISet<Term<object>>> TermsOfAgreement { get; set; }

        public virtual ISet<Term<object>> GetAgreedTerms(ILegalPerson lessor, ILegalPerson lessee)
        {
            return ExtensionMethods.GetAgreedTerms(this, lessor, lessee);
        }

        public virtual ISet<Term<object>> GetAdditionalTerms(ILegalPerson lessor, ILegalPerson lessee)
        {
            return ExtensionMethods.GetAdditionalTerms(this, lessor, lessee);
        }

        public virtual ISet<Term<object>> GetInNameAgreedTerms(ILegalPerson lessor, ILegalPerson lessee)
        {
            return ExtensionMethods.GetInNameAgreedTerms(this, lessor, lessee);
        }
    }
}