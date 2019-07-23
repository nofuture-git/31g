using System;
using NoFuture.Rand.Law.Property.US;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    /// <summary>
    /// Typically when one builds upon the land of another
    /// </summary>
    public class Encroachment : PropertyConsent
    {
        public Encroachment(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// when the encroachment was an honest mistake
        /// </summary>
        public Predicate<IDefendant> IsGoodFaithIntent { get; set; } = lp => false;

        /// <summary>
        /// The result affect of the encroachment for the plaintiff is slight or none
        /// </summary>
        public Predicate<IPlaintiff> IsUseAffected { get; set; } = lp => false;

        /// <summary>
        /// The removal of the encroachment would cost a great deal to the defendant (e.g. tear down whole building for 2 inch encroachment)
        /// </summary>
        public Predicate<IDefendant> IsRemovalCostGreat { get; set; } = lp => false;

        /// <summary>
        /// Similar to <see cref="IsRemovalCostGreat"/> in terms of overall hardship.
        /// </summary>
        public Predicate<IDefendant> IsRemovalGraveHardship { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!WithoutConsent(persons))
                return false;

            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
                return false;
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;

            var pTitle = plaintiff.GetLegalPersonTypeName();
            var dTitle = defendant.GetLegalPersonTypeName();

            var isHonestMistake = IsGoodFaithIntent(defendant);
            if (!isHonestMistake)
            {
                AddReasonEntry($"{dTitle} {defendant.Name}, {nameof(IsGoodFaithIntent)} is false (i.e. intentional encroachment)");
                return true;
            }

            var remCost = IsRemovalCostGreat(defendant);
            var remHard = IsRemovalGraveHardship(defendant);

            if (!IsUseAffected(plaintiff) && (remCost || remHard))
            {
                AddReasonEntry($"{pTitle} {plaintiff.Name}, {nameof(IsUseAffected)} is false " +
                               $"while {dTitle} {defendant.Name}, {nameof(IsRemovalCostGreat)} " +
                               $"is {remCost} and/or {nameof(IsRemovalGraveHardship)} is {remHard}");
                return false;
            }

            return true;
        }
    }
}
