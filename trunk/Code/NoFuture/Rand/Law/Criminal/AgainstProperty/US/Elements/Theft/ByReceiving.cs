using System;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft
{
    /// <summary>
    /// Model Penal Code 223.6.
    /// </summary>
    public class ByReceiving : ConsolidatedTheft
    {
        public Predicate<ILegalPerson> IsPresentStolen { get; set; } = lp => false;

        /// <summary>
        /// The person receiving it beleives its probably stolen
        /// </summary>
        public Predicate<ILegalPerson> IsApparentStolen { get; set; } = lp => false;

        /// <summary>
        /// Regarding any substantive thing by any action of receiving, retaining, 
        /// disposing, moving, controlling, titling, acquiring, etc.
        /// </summary>
        /// <remarks>
        /// Preceiving, presuming, etc. are present participles whose subject has no
        /// material substance
        /// </remarks>
        public Predicate<ILegalPerson> IsAnySubstantiveParticiple { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            var isStolen = IsPresentStolen(defendant);
            var isBelievedStolen = IsApparentStolen(defendant);

            if (!isStolen && !isBelievedStolen)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsPresentStolen)} is false");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsApparentStolen)} is false");
                return false;
            }

            if (!IsAnySubstantiveParticiple(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsAnySubstantiveParticiple)} is false");
                return false;
            }

            return true;
        }
    }
}
