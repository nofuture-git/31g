namespace NoFuture.Rand.Law.US.Contracts.Remedy.MoneyDmg
{
    /// <inheritdoc />
    /// <summary>
    /// sum of money to compensate the plaintiff.
    /// </summary>
    public abstract class MoneyDmgBase<T> : RemedyBase<T> where T : IObjectiveLegalConcept
    {
        protected MoneyDmgBase(IContract<T> contract) : base(contract)
        {
        }
    }
}
