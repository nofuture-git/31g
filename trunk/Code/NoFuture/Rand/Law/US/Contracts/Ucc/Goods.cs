namespace NoFuture.Rand.Law.US.Contracts.Ucc
{
    /// <summary>
    /// <![CDATA[
    /// all things which are movable at the time of identification
    /// ]]>
    /// </summary>
    public class Goods : ObjectiveLegalConcept, IUccItem
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (!IsEnforceableInCourt)
            {
                AddAuditEntry($"{GetType().Name} is not enforceable in court");
                return false;
            }

            if (!IsMovable)
            {
                AddAuditEntry($"{GetType().Name} is not movable");
                return false;
            }

            if (!IsIdentified)
            {
                AddAuditEntry($"{GetType().Name} has no identification");
                return false;
            }

            return true;
        }

        public virtual bool IsMovable { get; set; } = true;
        public virtual bool IsIdentified { get; set; } = true;
        public override bool IsEnforceableInCourt => true;
    }
}
