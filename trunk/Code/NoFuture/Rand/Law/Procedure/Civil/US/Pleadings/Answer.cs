namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// The response of a defendant to the complaint of the plaintiff
    /// </summary>
    public class Answer : PleadingBase, ILinkedLegalConcept
    {
        public ILegalConcept LinkedTo { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            return IsCourtAssigned();
        }
    }
}
