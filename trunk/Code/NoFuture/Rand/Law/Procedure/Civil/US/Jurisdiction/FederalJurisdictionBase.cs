namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    public abstract class FederalJurisdictionBase : JurisdictionBase
    {
        protected FederalJurisdictionBase(ICourt name) : base(name)
        {
        }

        /// <summary>
        /// Test validity without testing the type of <see cref="CivilProcedureBase.Court"/>
        /// </summary>
        /// <param name="persons"></param>
        /// <returns></returns>
        protected internal abstract bool IsValidWithoutTestCourtType(ILegalPerson[] persons);
    }
}
