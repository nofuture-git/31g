namespace NoFuture.Law.Criminal.US.Terms
{
    /// <summary>
    /// Medium potential for abuse with some medical use (e.g. steroids)
    /// </summary>
    public class ScheduleIII : ScheduleIV
    {
        public ScheduleIII() : this("Schedule III") { }
        public ScheduleIII(string name) : base(name) { }

        public override int GetRank()
        {
            return base.GetRank() - 1;
        }
    }
}
