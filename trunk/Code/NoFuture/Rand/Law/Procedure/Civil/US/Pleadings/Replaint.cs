namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// A base type for cross, counter and contribution claims -
    /// which are all related in that they are reactive to a original complaint
    /// </summary>
    /// <remarks>
    /// *neologism
    /// </remarks>
    public abstract class Replaint : Complaint
    {
        /// <summary>
        /// The cause-of-action which makes this a counter\cross claim instead of just a stand-alone <see cref="Complaint"/>.
        /// </summary>
        public ILegalConcept OppositionCausesOfAction { get; set; }

    }
}
