namespace NoFuture.Rand.Law.US.Contracts.Breach
{
    public abstract class StandardsBase<T> : ObjectiveLegalConcept
    {
        protected internal StandardsBase(IContract<T> contract)
        {
            Contract = contract;
        }

        public virtual IContract<T> Contract { get; }

        public override bool IsEnforceableInCourt => true;
    }
}
