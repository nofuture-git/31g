using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Procedure.Criminal.US
{

    /// <summary>
    /// Forms of search applied uniformly to many people without any form of suspicion
    /// </summary>
    /// <remarks>
    /// this is the reasoning for checkpoints, drug-tests, etc.
    /// </remarks>
    public class AdministrativeCause<T> : LegalConcept, IRankable where T : IRationale, new()
    {
        [Aka("legislative or administrative standards", "special needs")]
        public Predicate<T> IsPolicyBased { get; set; } = l => false;

        public Predicate<T> IsMinimumIntrusion { get; set; } = l => false;

        public Predicate<T> IsSpecificDetection { get; set; } = l => false;

        /// <summary>
        /// When the policy is a subterfuge to law enforcement 
        /// </summary>
        public Predicate<T> IsToDetectCriminalActivity { get; set; } = l => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var objOfSearch = new T();

            if (!IsPolicyBased(objOfSearch))
            {
                AddReasonEntry($"{nameof(IsPolicyBased)} returned false");
                AddReasonEntryRange(objOfSearch.GetReasonEntries());
                return false;
            }

            if (!IsMinimumIntrusion(objOfSearch))
            {
                AddReasonEntry($"{nameof(IsMinimumIntrusion)} returned false");
                AddReasonEntryRange(objOfSearch.GetReasonEntries());
                return false;
            }
            if (!IsSpecificDetection(objOfSearch))
            {
                AddReasonEntry($"{nameof(IsSpecificDetection)} returned false");
                AddReasonEntryRange(objOfSearch.GetReasonEntries());
                return false;
            }

            if (IsToDetectCriminalActivity(objOfSearch))
            {
                AddReasonEntry($"{nameof(IsToDetectCriminalActivity)} returned true");
                AddReasonEntryRange(objOfSearch.GetReasonEntries());
                return false;
            }

            return true;
        }

        public int GetRank()
        {
            return new ReasonableSuspicion().GetRank() - 1;
        }

    }
}
