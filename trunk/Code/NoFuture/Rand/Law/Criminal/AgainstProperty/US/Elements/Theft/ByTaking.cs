using System;
using System.Linq;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft
{
    /// <summary>
    /// A union of the various ways of directly stealing;  Model Penal Code 223.2. and 223.7.
    /// </summary>
    public class ByTaking : ConsolidatedTheft
    {
        /// <summary>
        /// The typical idea of theft as grab and run stealing
        /// </summary>
        public Predicate<ILegalPerson> IsTakenUnlawful { get; set; } = lp => false;

        /// <summary>
        /// The idea that control of the property has been taken unlawfully
        /// </summary>
        public Predicate<ILegalPerson> IsControlOverUnlawful { get; set; } = lp => false;

        /// <summary>
        /// The manner of stealing immovable property since it cannot be physically pocketed
        /// </summary>
        public Predicate<ILegalPerson> IsTransferUnlawful { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsToDepriveEntitled { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsToBenefitUnentitled { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;

            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            var isTaken = IsTakenUnlawful(defendant);
            var isTransfer = IsTransferUnlawful(defendant);
            var isControl = IsControlOverUnlawful(defendant);

            if (new[] {isTaken, isTransfer, isControl}.All(p => p == false))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsTakenUnlawful)} is false");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsTransferUnlawful)} is false");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsControlOverUnlawful)} is false");
                return false;
            }

            var isDepriveOwner = IsToDepriveEntitled(defendant);
            var isBenefitTheif = IsToBenefitUnentitled(defendant);

            if (!isDepriveOwner && !isBenefitTheif)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsToDepriveEntitled)} is false");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsToBenefitUnentitled)} is false");
                return false;
            }

            return true;
        }
    }
}
