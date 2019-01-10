using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Law.US.Contracts.Ucc
{
    /// <summary>
    /// <![CDATA[
    /// Per UCC 2-104(1) "deals in goods [...] knowledge or skill peculiar to [...] goods"
    /// ]]>
    /// </summary>
    public class Merchant : VocaBase, ILegalPerson
    {
        public Merchant() { }

        public Merchant(string name) : base(name) { }

        public Merchant(string name, string groupName) : base(name, groupName) { }

        /// <summary>
        /// To assert if the given merchant has knowledge or skills concerning the particular good.
        /// </summary>
        /// <param name="goods"></param>
        /// <returns></returns>
        public virtual bool IsSkilledOrKnowledgeableOf(Goods goods)
        {
            return false;
        }
    }
}
