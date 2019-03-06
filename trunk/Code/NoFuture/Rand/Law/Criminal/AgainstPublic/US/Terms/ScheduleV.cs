using System;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US.Terms
{
    /// <summary>
    /// Lowest potential for abuse with medical use
    /// </summary>
    public class ScheduleV : TermCategory, IDrugSchedule
    {
        protected override string CategoryName => "Schedule V";
        public virtual bool IsAcceptedMedicalUse { get; } = true;
        public override int GetCategoryRank()
        {
            return 5;
        }
    }
}
