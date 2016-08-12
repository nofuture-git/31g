using NoFuture.Rand.Com;

namespace NoFuture.Rand.Data.Sp
{
    public interface ILoan
    {
        /// <summary>
        /// The rate used to calc the minimum payment.
        /// </summary>
        float MinPaymentRate { get; set; }

        /// <summary>
        /// Credit history report for this loan
        /// </summary>
        ITradeLine TradeLine { get; }

        IFirm Lender { get; set; }
    }
}
