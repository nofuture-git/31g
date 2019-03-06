namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US.Terms
{
    /// <summary>
    /// Low potential for abuse with medical use (e.g. Xanax)
    /// </summary>
    public class ScheduleIV: ScheduleV
    {
        public ScheduleIV() :this("Schedule IV") { }
        public ScheduleIV(string name) : base(name) { }

        public override int GetCategoryRank()
        {
            return base.GetCategoryRank() - 1;
        }
    }
}
