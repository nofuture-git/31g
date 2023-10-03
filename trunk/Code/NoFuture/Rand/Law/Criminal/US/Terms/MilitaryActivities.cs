namespace NoFuture.Law.Criminal.US.Terms
{
    /// <summary>
    /// Information which is protected during a time of war same as 
    /// </summary>
    public class MilitaryActivities : NationalDefenseInformation
    {
        public MilitaryActivities() { }

        public MilitaryActivities(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                CategoryName = name;
        }
        protected override string CategoryName { get; } = "military activities";
    }
}
