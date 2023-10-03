namespace NoFuture.Law.Contract.US.Defense.ToAssent
{
    /// <summary>
    /// <![CDATA[
    /// a mistake concerning something fundamental to the nature of the contract
    /// ]]>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ByMistake<T> : DefenseBase<T> where T : ILegalConcept
    {
        public ByMistake(IContract<T> contract) : base(contract)
        {
        }
    }
}
