using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Criminal.US
{
    /// <inheritdoc cref="IWarrant{T}"/>
    public class ArrestWarrant : WarrantBase<ILegalPerson>
    {
        public override Func<ILegalPerson> GetObjectiveOfSearch { get; set; } = () => null;

        public override Predicate<ILegalPerson> IsObjectiveDescribedWithParticularity { get; set; } = r => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;

            var arrestee = GetObjectiveOfSearch();
            if (arrestee == null)
            {
                AddReasonEntry($"{nameof(GetObjectiveOfSearch)} returned nothing");
                return false;
            }

            var arresteeTitle = arrestee.GetLegalPersonTypeName();

            if (!IsObjectiveDescribedWithParticularity(arrestee))
            {
                AddReasonEntry($"{arresteeTitle} {arrestee}, {nameof(IsObjectiveDescribedWithParticularity)} is false");
                return false;
            }

            return true;
        }
    }
}
