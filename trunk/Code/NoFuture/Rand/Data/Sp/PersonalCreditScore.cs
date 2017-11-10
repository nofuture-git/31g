using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Domus;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public class PersonalCreditScore : CreditScore
    {
        #region constants
        /// <summary>
        /// [http://www.fico.com/en/blogs/risk-compliance/us-credit-quality-continues-climb-will-level/]
        /// </summary>
        internal const int AVG_AMERICAN_FICO_SCORE = 689;
        /// <summary>
        /// max minus the average divided by three.
        /// </summary>
        internal const int STD_DEV_FICO_SCORE = 33;
        #endregion

        #region fields
        private readonly NorthAmerican _american;
        internal readonly double FicoBaseValue;
        private string _ficoScore;
        #endregion

        #region ctor
        public PersonalCreditScore(NorthAmerican american)
        {
            if (american == null)
                return;
            _american = american;
            //need this to stay same for object lifecycle so repeated calls return same result.
            FicoBaseValue = Etx.RandomValueInNormalDist(AVG_AMERICAN_FICO_SCORE, STD_DEV_FICO_SCORE);
        }
        #endregion

        #region properties
        public override string Abbrev => "FICO";

        public override string Value
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ficoScore))
                {
                    _ficoScore = GetScore(DateTime.Now).ToString();
                }
                return _ficoScore;
            }
            set { _ficoScore = value; }
        }

        #endregion

        #region methods
        /// <summary>
        /// Loosely based on paper from Boston Fed 
        /// [http://www.bostonfed.org/economic/wp/wp2007/wp0703.pdf]
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public override int GetScore(DateTime? dt)
        {
            var agePenalty = GetAgePenalty(dt);
            var discipline = GetUndisciplinedPenalty();
            var consistent = GetInconsistentPenalty();

            var ficoScore = (int)Math.Ceiling(FicoBaseValue + agePenalty + discipline + consistent);

            if (ficoScore > MAX_FICO)
                ficoScore = MAX_FICO-1;
            if (ficoScore < MIN_FICO)
                ficoScore = MIN_FICO+1;

            return ficoScore;
        }

        /// <summary>
        /// Adds a random value between 0-1 to <see cref="baseRate"/> for each std dev the
        /// calc score at time <see cref="atDt"/> is from <see cref="CreditScore.MAX_FICO"/>
        /// </summary>
        /// <param name="atDt"></param>
        /// <param name="baseRate"></param>
        /// <returns></returns>
        public override double GetRandomInterestRate(DateTime? atDt, double baseRate = 3.0D)
        {
            var addPts = baseRate;
            var scoreCounter = MAX_FICO;
            var score = GetScore(atDt);

            while (scoreCounter > score)
            {
                addPts += Etx.MyRand.NextDouble();
                scoreCounter -= STD_DEV_FICO_SCORE;
            }

            return Math.Round(addPts,1) + 0.099;
        }

        protected internal double GetAgePenalty(DateTime? dt)
        {
            Func<double, double> ageCalc = d => Math.Pow(Math.E, (d - 9.5)/-9.5);

            return ageCalc(_american.GetAgeAt(dt)) * (STD_DEV_FICO_SCORE * -1);
        }

        protected internal double GetUndisciplinedPenalty()
        {
            return _american.Personality.Conscientiousness.Value.Zscore * STD_DEV_FICO_SCORE;
        }

        protected internal double GetInconsistentPenalty()
        {
            return _american.Personality.Openness.Value.Zscore * (STD_DEV_FICO_SCORE * -1);
        }
        #endregion
    }
}