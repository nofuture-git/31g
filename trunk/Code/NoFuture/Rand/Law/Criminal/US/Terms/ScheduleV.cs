namespace NoFuture.Law.Criminal.US.Terms
{
    /// <summary>
    /// Lowest potential for abuse with medical use
    /// </summary>
    public class ScheduleV : TermCategory, IDrugSchedule
    {
        private string _categoryName;

        public ScheduleV() { }
        public ScheduleV(string name)
        {
            _categoryName = name;
        }

        protected override string CategoryName => _categoryName;

        public virtual bool IsAcceptedMedicalUse { get; } = true;
        public override int GetRank()
        {
            return 5;
        }
    }
}
