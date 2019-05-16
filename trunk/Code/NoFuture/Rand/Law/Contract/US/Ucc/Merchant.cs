using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Contract.US.Ucc
{
    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc cref="IMerchant"/>
    public abstract class Merchant : LegalPerson, IMerchant
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
