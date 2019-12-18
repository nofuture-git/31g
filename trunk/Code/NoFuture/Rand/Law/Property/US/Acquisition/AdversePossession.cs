using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.Acquisition
{
    /// <summary>
    /// Possession of someone else&apos;s property as hostile, actual, visible, exclusive, and continuous.
    /// </summary>
    public class AdversePossession : PropertyConsent, ITempore
    {
        private TimeSpan _statuteOfLimitations;

        public AdversePossession(): this(persons => persons.Disseisor()) { }

        public AdversePossession(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
            _statuteOfLimitations = new TimeSpan(365*5, 5*5, 49*5, 16*5, 320*5);
            Inception = DateTime.UtcNow;
        }

        /// <summary>
        /// The time span the actual owner has to bring a case against the Disseisor. Default is 10 tropical years
        /// </summary>
        public virtual TimeSpan StatuteOfLimitations
        {
            get => _statuteOfLimitations;
            set => _statuteOfLimitations = value;
        }

        /// <summary>
        /// The actions of the entitled owner - defaults to the expected due diligence.
        /// </summary>
        public IAct EntitledOwnersAction { get; set; } = Act.DueDiligence();

        /// <summary>
        /// the use of the property is visible and apparent so the true owner is aware of potential conflict
        /// </summary>
        /// <remarks>
        /// Some secret use or taking of something implied guilt and therefore would fail this test
        /// </remarks>
        public Predicate<ILegalPerson> IsOpenNotoriousPossession { get; set; } = lp => false;

        /// <summary>
        /// the use by the disseisor (or dispossession of entitled owner) must have been
        /// continuous for the amount of time so that statue of limitations has expired
        /// </summary>
        public Predicate<ILegalPerson> IsContinuousPossession { get; set; } = lp => false;

        /// <summary>
        /// use of the land to the exclusion of the true owner
        /// </summary>
        public Predicate<ILegalPerson> IsExclusivePossession { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }

            var title = subj.GetLegalPersonTypeName();
            if (PropertyOwnerIsSubjectPerson(persons))
            {
                return false;
            }
            //actual possession
            if (!PropertyOwnerIsInPossession(persons))
            {
                return false;
            }

            if (!WithoutConsent(persons))
                return false;

            if (EntitledOwnersAction != null && !EntitledOwnersAction.IsValid(persons))
            {
                AddReasonEntryRange(EntitledOwnersAction.GetReasonEntries());
                return false;
            }

            if (!IsOpenNotoriousPossession(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsOpenNotoriousPossession)} is " +
                               $"false for {SubjectProperty.GetType().Name} '{SubjectProperty.Name}'");
                return false;
            }

            if (!IsContinuousPossession(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsContinuousPossession)} is " +
                               $"false for {SubjectProperty.GetType().Name} '{SubjectProperty.Name}'");
                return false;
            }

            if (!IsExclusivePossession(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsExclusivePossession)} is " +
                               $"false for {SubjectProperty.GetType().Name} '{SubjectProperty.Name}'");
                return false;
            }

            var utcInception = Inception.ToUniversalTime();
            var utcTerminus = Terminus.GetValueOrDefault(DateTime.Now).ToUniversalTime();

            var tsOfPossession = utcTerminus - utcInception;
            if (tsOfPossession < StatuteOfLimitations)
            {
                AddReasonEntry($"{title} {subj}, {nameof(StatuteOfLimitations)} required possession " +
                               $"for {StatuteOfLimitations} actual possession is {tsOfPossession} for " +
                               $"{SubjectProperty.GetType().Name} '{SubjectProperty.Name}'");
                return false;
            }

            return true;
        }

        /// <summary>
        /// The point in time when <see cref="AdversePossession"/>
        /// began; namely, when the entitled owner became aware of the disseisor 
        /// </summary>
        public DateTime Inception { get; set; }

        /// <summary>
        /// Optional, time when <see cref="AdversePossession"/> ended.
        /// </summary>
        public DateTime? Terminus { get; set; }

        public bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = Inception <= dt;
            var beforeOrOnToDt = Terminus == null || Terminus.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }
    }
}
