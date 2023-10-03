namespace NoFuture.Law.Criminal.US.Terms
{
    /// <summary>
    /// Information which, if gathered and/or transmitted is considered espionage.
    /// </summary>
    public class NationalDefenseInformation : TermCategory
    {
        public NationalDefenseInformation() { }

        public NationalDefenseInformation(string name)
        {
            if(!string.IsNullOrWhiteSpace(name))
                CategoryName = name;
        }

        protected override string CategoryName { get; } = "Defense Information";
    }
}
