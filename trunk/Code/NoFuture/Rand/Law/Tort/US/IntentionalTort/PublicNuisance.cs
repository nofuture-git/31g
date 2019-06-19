using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.US.Property;

namespace NoFuture.Rand.Law.Tort.US.IntentionalTort
{
    /// <summary>
    /// an unreasonable interference with a right common to the general public
    /// </summary>
    public class PublicNuisance : PropertyConsent
    {
        public PublicNuisance(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Predicate<ILegalPerson> IsUnreasonableInterference { get; set; } = lp => false;

        /// <summary>
        /// reserved [...] for those indivisible resources shared by the public at large, such as air, water, or public rights of way
        /// </summary>
        public Predicate<ILegalProperty> IsRightCommonToPublic { get; set; } = lp => false;

        /// <summary>
        /// proscribed by a statute, ordinance or administrative regulation
        /// </summary>
        public Predicate<ILegalPerson> IsProscribedByGovernment { get; set; } = lp => false;

        /// <summary>
        /// for a private individual they must have suffered harm of a kind different from that
        /// suffered by other members of the public
        /// </summary>
        public Predicate<IPlaintiff> IsPrivatePeculiarInjury { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;

            var title = subj.GetLegalPersonTypeName();

            if (IsProscribedByGovernment(subj))
                return true;

            if (IsUnreasonableInterference(subj) && !IsRightCommonToPublic(SubjectProperty))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsRightCommonToPublic)} is false for the property {SubjectProperty?.Name}");
                return false;
            }

            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
            {
                return false;
            }

            if (plaintiff is IGovernment)
                return true;

            if (!IsPrivatePeculiarInjury(plaintiff))
            {
                AddReasonEntry($"{title} {plaintiff.Name}, {nameof(IsPrivatePeculiarInjury)} is false");
                return false;
            }

            return true;
        }
    }
}
