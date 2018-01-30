
namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Represent finacial loan from a money-lending agent
    /// </summary>
    public interface ILoan
    {
        /// <summary>
        /// The rate used to calc the minimum payment.
        /// </summary>
        float MinPaymentRate { get; set; }

    }
}