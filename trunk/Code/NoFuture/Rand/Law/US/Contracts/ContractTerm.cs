namespace NoFuture.Rand.Law.US.Contracts
{
    public enum TermSource
    {
        Oral,
        Written
    }

    /// <inheritdoc />
    public class ContractTerm<T> : Term<T>
    {
        public ContractTerm(string name, T reference) : base(name, reference)
        {
        }

        public virtual TermSource Source { get; set; }

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

            var mySource = (int)Source;
            var theirSource = (int)contractTerm.Source;
            return theirSource - mySource;
        }
    }
}
