
namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// Represent financial loan from a money-lending agent
    /// </summary>
    public interface ILoan
    {
        /// <summary>
        /// The rate used to calc the minimum payment.
        /// </summary>
        float MinPaymentRate { get; set; }

        /// <summary>
        /// Gets the amount of the first transaction
        /// </summary>
        Pecuniam OriginalBorrowAmount { get; }
    }
}