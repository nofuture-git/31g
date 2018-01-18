using System;
using NoFuture.Rand.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public class Mortgage : SecuredFixedRateLoan
    {
        #region fields
        /// <summary>
        /// Src https://data.worldbank.org/indicator/NY.GDP.MKTP.KD 1960-2016
        /// </summary>
        public const float AVG_GDP_GROWTH_RATE = 0.031046655f;
        #endregion

        #region ctors

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

        }

        #endregion

        #region properties

        /// <summary>
        /// The average estimated rate at which the 
        /// value of the real-estate is 
        /// expected to grow (default is <see cref="AVG_GDP_GROWTH_RATE"/>).
        /// </summary>
        public float ExpectedAppreciationRate { get; set; } = AVG_GDP_GROWTH_RATE;

        /// <summary>
        /// The original purchase price of the real-estate.
        /// </summary>
        public Pecuniam PurchasePrice => GetValueAt(Inception);

        #endregion

        #region methods

        /// <summary>
        /// Gets the difference of the current 
        /// market value to the remaining balance on the note.
        /// </summary>
        /// <param name="dt">
        /// The date of query.
        /// </param>
        /// <param name="marketValue">
        /// Optional, defaults to <see cref="GetEstimatedMarketValueAt"/>
        /// </param>
        /// <returns></returns>
        public Pecuniam GetEquityAt(DateTime? dt, Pecuniam marketValue = null)
        {
            var qDt = dt ?? DateTime.Today;

            if(qDt < Inception)
                return Pecuniam.Zero;

            var qRemaining = GetValueAt(qDt).Abs;
            var mv = (marketValue ?? GetEstimatedMarketValueAt(qDt)).Abs;
            return mv - qRemaining;
        }

        /// <summary>
        /// Calculates an estimated market value as the purchase price grown at 
        /// the <see cref="ExpectedAppreciationRate"/>
        /// </summary>
        /// <param name="dt">
        /// The date of query
        /// </param>
        /// <returns></returns>
        public Pecuniam GetEstimatedMarketValueAt(DateTime? dt)
        {
            var qDt = dt ?? DateTime.Today;
            var pDt = Inception;

            if(qDt < pDt)
                return PurchasePrice;

            var numDays = (qDt - pDt).TotalDays;

            var marketValue =
                PurchasePrice.Abs.Amount.PerDiemInterest(ExpectedAppreciationRate,numDays);

            return marketValue.ToPecuniam();
        }
        #endregion
    }
}
