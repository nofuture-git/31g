using System;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Contract.US.Ucc
{
    /// <inheritdoc cref="IMerchant{T}"/>
    public abstract class Merchant : LegalPerson, IMerchant<Goods>
    {
        protected Merchant() { }

        protected Merchant(string name) : base(name) { }

        protected Merchant(string name, string groupName) : base(name, groupName) { }

        /// <summary>
        /// To assert if the given merchant has knowledge or skills concerning the particular good.
        /// </summary>
        public virtual Predicate<Goods> IsSkilledOrKnowledgeableOf { get; set; } = lp => false;
    }
}
