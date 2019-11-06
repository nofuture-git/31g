namespace NoFuture.Rand.Law.Procedure.Civil.US
{
    /// <summary>
    /// Base type for all US civil procedure types
    /// </summary>
    public abstract class CivilProcedureBase : LegalConcept
    {
        /// <summary>
        /// The basis on which the procedure is being
        /// performed - the reason to go to court in the first place.
        /// </summary>
        public ILegalConcept CausesOfAction { get; set; }
    }
}
