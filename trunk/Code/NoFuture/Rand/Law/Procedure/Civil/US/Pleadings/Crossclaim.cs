namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// Similar to <see cref="Counterclaim"/> except it concerns members
    /// of the same adversarial side
    /// (e.g. defendant-against-defendant, plaintiff-against-plaintiff)
    /// </summary>
    public class Crossclaim : Complaint
    {
        /// <summary>
        /// Unlike, <see cref="Counterclaim"/>, this does not have to
        /// be related to the original <see cref="Complaint"/> 
        /// </summary>
        /// <remarks>
        /// Legal reasoning for allowing unrelated claims to be attached to some other
        /// claim is that all the parties are in court so why not resolve all the differences now.
        /// </remarks>
        public ILegalConcept CrossCausesOfAction { get; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            //this is just a complaint - not a valid cross-claim
            if (CrossCausesOfAction == null)
            {
                AddReasonEntry($"{nameof(CrossCausesOfAction)} is unassigned");
                return false;
            }

            return base.IsValid(persons);
        }
    }
}
