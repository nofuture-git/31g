namespace NoFuture.Rand.Law.US.Contracts.Litigate
{
    public abstract class LitigateBase<T> : ObjectiveLegalConcept
    {
        private readonly IContract<T> _contract;

        protected LitigateBase(IContract<T> contract)
        {
            _contract = contract;
        }

        public virtual IContract<T> Contract => _contract;

        public override bool IsEnforceableInCourt => true;
    }
}
