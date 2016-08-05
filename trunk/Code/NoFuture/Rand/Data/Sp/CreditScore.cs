using System;
using NoFuture.Rand.Domus;

namespace NoFuture.Rand.Data.Sp
{
    public abstract class CreditScore
    {
        public const int MAX_FICO = 850;
        public const int MIN_FICO = 300;
        public abstract int GetScore(DateTime? dt);
        public abstract double GetRandomInterestRate(DateTime? atDt, double baseRate = 3.0D);

        public Pecuniam GetRandomMax(DateTime dt)
        {
            var ccScore = GetScore(dt);
            if (ccScore >= 600 && ccScore < 650)
                return Pecuniam.GetRandPecuniam(1000, 2000, 100);
            if (ccScore >= 650 && ccScore < 700)
                return Pecuniam.GetRandPecuniam(3000, 5000, 100);
            if (ccScore >= 750)
                return Pecuniam.GetRandPecuniam(5000, 10000, 100);
            if(ccScore >= 800)
                return Pecuniam.GetRandPecuniam(10000, 20000, 100);
            return new Pecuniam(1000);
        }
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

        public override double GetRandomInterestRate(DateTime? atDt, double baseRate = 3.0D)
        {
            var addPts = baseRate;
            var scoreCounter = MAX_FICO;
            var score = GetScore(atDt);
            var moreRand = 1+((atDt.GetValueOrDefault(DateTime.Today).DayOfYear % 9) * 0.1);

            while (scoreCounter > score)
            {
                addPts += moreRand;
                scoreCounter -= STD_DEV_FICO_SCORE;
            }

            return addPts + 0.099;
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
