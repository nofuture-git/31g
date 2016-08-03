using System;
using NoFuture.Rand.Domus;

namespace NoFuture.Rand.Data.Sp
{
    public abstract class CreditScore
    {
        public const int MAX_FICO = 850;
        public const int MIN_FICO = 300;
    }

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
        internal const int STD_DEV_FICO_SCORE = 53;
        internal const int POSITIVE_FICO = STD_DEV_FICO_SCORE * 3;
        internal const int NEGATIVE_FICO = -1*POSITIVE_FICO;
        #endregion

        #region fields
        private readonly NorthAmerican _american;
        internal readonly double FicoBaseValue;
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

        #region methods
        /// <summary>
        /// Loosely based on paper from Boston Fed 
        /// [http://www.bostonfed.org/economic/wp/wp2007/wp0703.pdf]
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int GetScore(DateTime? dt)
        {
            var agePenalty = GetAgePenalty(dt);
            var discipline = GetUndisciplinedPenalty();
            var consistent = GetInconsistentPenalty();

            var ficoScore = (int)Math.Ceiling(FicoBaseValue + agePenalty + discipline + consistent);

            if (ficoScore > MAX_FICO)
                ficoScore = MAX_FICO;
            if (ficoScore < MIN_FICO)
                ficoScore = MIN_FICO;

            return ficoScore;
        }

        protected internal double GetAgePenalty(DateTime? dt)
        {
            Func<double, double> ageCalc = d => Math.Pow(Math.E, (d - 9.5)/-9.5);

            return ageCalc(_american.GetAgeAt(dt)) * NEGATIVE_FICO;
        }

        protected internal double GetUndisciplinedPenalty()
        {
            return _american.Personality.Conscientiousness.Value.Zscore * POSITIVE_FICO;
        }

        protected internal double GetInconsistentPenalty()
        {
            return _american.Personality.Openness.Value.Zscore * NEGATIVE_FICO;
        }

        public override string ToString()
        {
            return GetScore(DateTime.Now).ToString();
        }
        #endregion
    }
}
