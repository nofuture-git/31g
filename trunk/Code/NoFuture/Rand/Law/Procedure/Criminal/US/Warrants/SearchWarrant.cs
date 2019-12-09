using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Procedure.Criminal.US.Warrants
{
    /// <inheritdoc cref="IWarrant{T}"/>
    public class SearchWarrant : WarrantBase<IVoca>
    {
        public override Func<IVoca> GetObjectiveOfSearch { get; set; } = () => null;

        public override Predicate<IVoca> IsObjectiveDescribedWithParticularity { get; set; } = r => false;

        /// <summary>
        /// when the items are not correctly described, the mistake is deemed to have be objectively reasonable
        /// </summary>
        [Aka("good faith exception")]
        public Predicate<IVoca> IsGoodFaithException { get; set; } = r => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;

            var objective = GetObjectiveOfSearch();
            if (objective == null)
            {
                AddReasonEntry($"{nameof(GetObjectiveOfSearch)} returned nothing");
                return false;
            }

            if (!IsObjectiveDescribedWithParticularity(objective) 
                && !IsGoodFaithException(objective))
            {
                AddReasonEntry($"{objective.Name}, {nameof(IsObjectiveDescribedWithParticularity)} and " +
                               $" {nameof(IsGoodFaithException)} are both false");
                return false;
            }

            return true;
        }
    }
}
