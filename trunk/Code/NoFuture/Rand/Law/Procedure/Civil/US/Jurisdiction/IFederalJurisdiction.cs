namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    public interface IFederalJurisdiction : ILegalConcept
    {
        /// <summary>
        /// Test validity of a federal court being appropriate - as compared to a federal court being assigned
        /// </summary>
        /// <param name="persons"></param>
        /// <returns></returns>
        bool IsValidAsFederalCourt(ILegalPerson[] persons);
    }
}