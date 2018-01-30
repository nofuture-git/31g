using System;
using NoFuture.Rand.Core;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Sp
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
        internal readonly double FicoBaseValue;
        private string _ficoScore;
        private readonly DateTime _personsDob;
        #endregion

        #region ctor
        public PersonalCreditScore(DateTime? personsBirthDate = null)
        {
            //need this to stay same for object lifecycle so repeated calls return same result.
            FicoBaseValue = Etx.RandomValueInNormalDist(AVG_AMERICAN_FICO_SCORE, STD_DEV_FICO_SCORE);
            ConscientiousnessZscore = GetRandomZscore();
            OpennessZscore = GetRandomZscore();
            _personsDob = personsBirthDate ?? Etx.RandomAdultBirthDate();
        }
        #endregion

        #region properties
        public override string Abbrev => "FICO";

        //int GetAgeAt(DateTime? atTime)
        public double ConscientiousnessZscore { get; set; }
        public double OpennessZscore { get; set; }

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
            set => _ficoScore = value;
        }

        #endregion

        #region methods

        /// <summary>
        /// Gets a personal credit score at random
        /// </summary>
        /// <param name="birthDate"></param>
        /// <returns></returns>
        [RandomFactory]
        public static PersonalCreditScore RandomCreditScore(DateTime? birthDate = null)
        {
            birthDate = birthDate ?? Etx.RandomAdultBirthDate();
            return new PersonalCreditScore(birthDate);
        }

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
            return Math.Pow(Math.E, (Etc.CalcAge(_personsDob, dt) - 9.5) / -9.5) * (STD_DEV_FICO_SCORE * -1);
        }

        protected internal double GetUndisciplinedPenalty()
        {

            return ConscientiousnessZscore * STD_DEV_FICO_SCORE;
        }

        protected internal double GetInconsistentPenalty()
        {
            return OpennessZscore * (STD_DEV_FICO_SCORE * -1);
        }

        private double GetRandomZscore()
        {
            var s = Etx.RandomPlusOrMinus();
            return s * Math.Round(Etx.MyRand.NextDouble(), 7);
        }
        #endregion
    }
}