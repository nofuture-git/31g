using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests
{
    /// <summary>
    /// Are in many places are not considered interest in land at all but are a contract 
    /// </summary>
    public class Leasehold : LandPropertyInterestBase, ITempore, IBargain<ILandPropertyInterest, RealProperty>
    {
        public Leasehold() : base(null)
        {
            Acceptance = GetAcceptance;
        }

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

            if (Acceptance?.Invoke(Offer) == null )
            {
                AddReasonEntry($"{orTitle} {lessor.Name} and {eeTitle} {lessee.Name}, {nameof(Acceptance)} " +
                               $"using {nameof(Offer)} '{Offer.Name}' returned nothing" );
                return false;
            }

            return true;

        }

        public DateTime Inception { get; set; }
        public DateTime? Terminus { get; set; }
        public bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = Inception <= dt;
            var beforeOrOnToDt = Terminus == null || Terminus.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }

        public RealProperty Offer
        {
            get => SubjectProperty;
            set => SubjectProperty = value;
        }

        /// <summary>
        /// Optional, defaults to SubjectProperty return this
        /// </summary>
        public Func<RealProperty, ILandPropertyInterest> Acceptance { get; set; }

        /// <summary>
        /// Optional, defaults to &quot;consent is given&quot;
        /// </summary>
        public IAssent Assent { get; set; } = Consent.IsGiven();

        protected virtual ILandPropertyInterest GetAcceptance(RealProperty property)
        {
            var isSameProperty = SubjectProperty?.Equals(property) ?? false;
            return isSameProperty ? this : null;
        }
    }
}
