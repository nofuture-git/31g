namespace NoFuture.Rand.Law.US.Contracts.Terms
{
    /// <inheritdoc />
    public class ContractTerm<T> : Term<T>
    {
        public ContractTerm(string name, T reference) : base(name, reference)
        {
        }

        public ContractTerm(string name, T reference, params ITermCategory[] categories) : base(name, reference, categories) { }

        /// <summary>
        /// <![CDATA[
        /// what the parties to a contract wrote and signed — is likely to be 
        /// better evidence of what their actual deal was than is their 
        /// subsequent self-serving testimony
        /// ]]>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override int CompareTo(object obj)
        {
            var nameCompare = base.CompareTo(obj);

            if (nameCompare != 0)
                return nameCompare;

            var contractTerm = obj as ContractTerm<T>;
            if (contractTerm == null)
                return nameCompare;

            var mySource = IsCategory(new WrittenTerm()) ? 1 : 0;
            var theirSource = contractTerm.IsCategory(new WrittenTerm()) ? 1 : 0;
            return theirSource - mySource;
        }
    }
}
