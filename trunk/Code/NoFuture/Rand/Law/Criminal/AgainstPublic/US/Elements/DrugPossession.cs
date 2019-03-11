using NoFuture.Rand.Law.Criminal.AgainstPublic.US.Terms;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US.Elements
{
    /// <inheritdoc cref="Possession"/>
    public class DrugPossession : Possession, IControlledSubstance
    {
        public IDrugSchedule Offer { get; set; } = new ScheduleI();

        public override bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var isValid = criminalIntent is Knowingly || criminalIntent is GeneralIntent;

            if (!isValid)
            {
                AddReasonEntry($"{nameof(DrugPossession)} requires intent {nameof(Knowingly)} or {nameof(GeneralIntent)}");
                return false;
            }

            return true;
        }
    }
}
