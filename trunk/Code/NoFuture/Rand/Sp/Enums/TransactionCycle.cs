namespace NoFuture.Rand.Sp.Enums
{
    /// <summary>
    /// The basic phases a transaction cycles through.
    /// </summary>
    public enum TransactionCycle
    {
        /// <summary>
        /// In terms of revenue, the time at which 
        /// a buyer request a service
        /// </summary>
        Promissory,

        /// <summary>
        /// In terms of revenue, the time at which a seller
        /// performs the agreed obligation of the service.
        /// </summary>
        Performant,

        /// <summary>
        /// In terms of revenue, the time in which the seller
        /// receives cash from the buyer for the service.
        /// </summary>
        Payment,
    }
}