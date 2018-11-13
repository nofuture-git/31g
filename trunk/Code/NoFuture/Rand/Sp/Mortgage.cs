using System;
using NoFuture.Rand.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Sp
{
    [Serializable]
    public class Mortgage : SecuredFixedRateLoan
    {
        /// <summary>
        /// Creates a new mortgage with the given data
        /// </summary>
        /// <param name="property"></param>
        /// <param name="openedDate"></param>
        /// <param name="rate"></param>
        /// <param name="purchasePrice"></param>
        /// <param name="termInYears"></param>
        public Mortgage(Identifier property, DateTime openedDate, float rate, Pecuniam purchasePrice,
            int termInYears = 30) : base(property, openedDate, purchasePrice, rate, termInYears)
        {
            FormOfCredit = Enums.FormOfCredit.Mortgage;
        }

        /// <summary>
        /// Gets the difference of the current 
        /// market value to the remaining balance on the note.
        /// </summary>
        /// <param name="dt">
        /// The date of query.
        /// </param>
        /// <param name="marketValue">
        /// </param>
        /// <returns></returns>
        public virtual Pecuniam GetEquityAt(DateTime? dt, Pecuniam marketValue = null)
        {
            var qDt = dt ?? DateTime.Today;

            if(qDt < Inception)
                return Pecuniam.Zero;

            var qRemaining = GetValueAt(qDt).GetAbs();
            var mv = (marketValue ?? GetEstimatedMarketValueAt(qDt)).GetAbs();
            return mv - qRemaining;
        }

    }
}
