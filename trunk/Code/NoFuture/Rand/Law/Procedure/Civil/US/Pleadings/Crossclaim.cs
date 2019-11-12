namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// Similar to <see cref="Counterclaim"/> except it concerns members
    /// of the same adversarial side
    /// (e.g. defendant-against-defendant, plaintiff-against-plaintiff)
    /// </summary>
    public class Crossclaim : Complaint
    {
        public Crossclaim(PleadingBase relatedTo)
        {
            RelatedTo = relatedTo;
        }

        /// <summary>
        /// Unlike, <see cref="Counterclaim"/>, this does not have to
        /// be related to the original <see cref="Complaint"/> 
        /// </summary>
        public PleadingBase RelatedTo { get; }
    }
}
