namespace NoFuture.Rand.Law.Criminal.US.Terms.Violence
{
    /// <summary>
    /// <![CDATA[
    /// Legally dead there is irreversible cessation of the 
    /// entire brain, including the brain stem 
    /// (Uniform Determination of Death Act, 2010)
    /// ]]>
    /// </summary>
    public class Death: SeriousBodilyInjury
    {
        protected override string CategoryName { get; } = "death";
    }
}
