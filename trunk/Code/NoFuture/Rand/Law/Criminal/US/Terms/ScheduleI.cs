namespace NoFuture.Rand.Law.Criminal.US.Terms
{
    /// <summary>
    /// Typically these are street drugs (e.g. crack-cocaine)
    /// </summary>
    public class ScheduleI : ScheduleII
    {
        public ScheduleI() : this("Schedule I") { }
        public ScheduleI(string name) : base(name) { }

        public override bool IsAcceptedMedicalUse { get; } = false;

        public override int GetCategoryRank()
        {
            return base.GetCategoryRank() - 1;
        }
    }
}
