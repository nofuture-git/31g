using System;

namespace NoFuture.Rand.Law.Tort.US.Terms
{
    /// <summary>
    /// a shopkeeper is allowed to detain a suspected shoplifter on store
    /// property for a reasonable period of time, so long as the shopkeeper
    /// has cause to believe that the person detained in fact committed, or
    /// attempted to commit, theft 
    /// </summary>
    public class ShopkeeperPrivilege : TermCategory
    {
        protected override string CategoryName { get; } = "shopkeeper’s privilege";
    }
}
