namespace NoFuture.Rand.Law.US.Contracts.Ucc
{
    /// <summary>
    /// <![CDATA[
    /// Per UCC 2-104(1) "deals in goods [...] knowledge or skill peculiar to [...] goods"
    /// ]]>
    /// </summary>
    public abstract class Merchant : LegalPerson
    {
        protected Merchant() { }

        protected Merchant(string name) : base(name) { }

        protected Merchant(string name, string groupName) : base(name, groupName) { }

        /// <summary>
        /// To assert if the given merchant has knowledge or skills concerning the particular good.
        /// </summary>
        /// <param name="goods"></param>
        /// <returns></returns>
        public abstract bool IsSkilledOrKnowledgeableOf(Goods goods);
    }
}
