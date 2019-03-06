using NoFuture.Rand.Law.Criminal.AgainstPublic.US.Terms;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US.Elements
{
    /// <inheritdoc cref="Possession"/>
    public class DrugPossession : Possession, IControlledSubstance
    {
        public IDrugSchedule SubjectDrug { get; set; } = new ScheduleI();
    }
}
