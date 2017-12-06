using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp.Cc;
using NoFuture.Rand.Domus;
using NoFuture.Shared.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public class Mortgage : SecuredFixedRateLoan
    {
        #region fields
        private readonly int _termInYears;
        private float _minPayRate;
        private float _rate;
        private Pecuniam _monthlyPayment;
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
        public Mortgage(Identifier property, DateTime openedDate, float rate, Pecuniam purchasePrice, int termInYears = 30) : base(property, openedDate,
            CreditCardAccount.DF_MIN_PMT_RATE, purchasePrice)
        {
            _rate = rate;

            ValidateRate();

            if (openedDate == DateTime.MinValue)
                throw new ArgumentException($"The opened date {openedDate} is invalid");

            if(purchasePrice == null || purchasePrice <= Pecuniam.Zero)
                throw new ArgumentException($"The purchase price {purchasePrice} is invalid");

            _termInYears = termInYears < 0 ? 30 : termInYears;

            CalcFromRate();
        }
        #endregion

        #region properties

        /// <summary>
        /// The average estimated rate at which the 
        /// value of the real-estate is 
        /// expected to grow (default is <see cref="NAmerUtil.AVG_GDP_GROWTH_RATE"/>).
        /// </summary>
        public double ExpectedAppreciationRate { get; set; } = NAmerUtil.AVG_GDP_GROWTH_RATE;

        /// <summary>
        /// The monthly mortgage payment.
        /// </summary>
        public Pecuniam MonthlyPayment => _monthlyPayment;


        /// <summary>
        /// The borrowing rate for the mortgage.
        /// </summary>
        public override float Rate
        {
            get => _rate;
            set
            {
                var temp = value;
                var reCalc = Math.Abs(_rate - temp) > 0.00001;
                _rate = temp;
                ValidateRate();
                if (reCalc)
                    CalcFromRate();
            }
        }

        /// <summary>
        /// The minimum payment rate.
        /// </summary>
        public override float MinPaymentRate
        {
            get => _minPayRate;
            set => _minPayRate = value;
        }

        /// <summary>
        /// The original purchase price of the real-estate.
        /// </summary>
        public Pecuniam PurchasePrice => GetValueAt(TradeLine.OpennedDate);

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

            if(qDt < TradeLine.OpennedDate)
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
            var pDt = TradeLine.OpennedDate;

            if(qDt < pDt)
                return PurchasePrice;

            var numDays = (qDt - pDt).TotalDays;

            var marketValue =
                PurchasePrice.Abs.Amount.PerDiemInterest(ExpectedAppreciationRate,numDays);

            return marketValue.ToPecuniam();
        }

        /// <summary>
        /// The future value of the mortgage note.
        /// </summary>
        /// <returns></returns>
        protected internal virtual decimal GetFutureValue(double numOfDays)
        {
            ValidateRate();
            var fv = PurchasePrice.Amount.PerDiemInterest(_rate, numOfDays);
            return fv;
        }

        /// <summary>
        /// Validates the rate is reasonable.
        /// </summary>
        protected internal void ValidateRate()
        {
            if (_rate > 1)
                _rate = Convert.ToSingle(Math.Round(_rate / 100, 5));

            if (_rate < 0f)
                throw new ArgumentException($"The rate {_rate} is invalid");

            if (_rate > 0.2f)
                throw new ArgumentException($"The rate {_rate} is usury.");
        }

        /// <summary>
        /// Helper method to (re)assign instance fields whenever a change in rate is noticed.
        /// </summary>
        protected internal void CalcFromRate()
        {
            var fv = GetFutureValue(Constants.TropicalYear.TotalDays * _termInYears);
            _monthlyPayment = new Pecuniam(Math.Round(fv / (_termInYears * 12), 2));
            _minPayRate = fv == 0
                ? CreditCardAccount.DF_MIN_PMT_RATE
                : (float) Math.Round(_monthlyPayment.Amount / fv, 6);
        }

        #endregion
    }
}
