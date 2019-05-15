namespace NoFuture.Rand.Law.Criminal.US.Terms
{
    /// <summary>
    /// High potential for abuse drugs which have some medical use (e.g. Fentanyl, Amphetamine, etc.)
    /// </summary>
    public class ScheduleII : ScheduleIII
    {
        public ScheduleII() : this("Schedule II") { }
        public ScheduleII(string name) : base(name) { }

        public override int GetCategoryRank()
        {
            return base.GetCategoryRank() - 1;
        }
    }
}
