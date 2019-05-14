namespace NoFuture.Rand.Law.US.Defense
{
    /// <summary>
    /// the facts as the defendant believes them to be negate the requisite intent for the crime at issue
    /// </summary>
    public interface IMistakeOfFact : ILegalConcept
    {
        SubjectivePredicate<ILegalPerson> IsBeliefNegateIntent { get; set; }
        bool IsStrictLiability { get; set; }
    }
}