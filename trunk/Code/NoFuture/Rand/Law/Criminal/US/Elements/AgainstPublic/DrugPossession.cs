using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.Criminal.US.Terms;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstPublic
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
