using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public abstract class CreditScore : Identifier
    {
        public const int MAX_FICO = 850;
        public const int MIN_FICO = 300;
        public abstract int GetScore(DateTime? dt);
        public abstract double GetRandomInterestRate(DateTime? atDt, double baseRate = 3.0D);

        public Pecuniam GetRandomMax(DateTime? dt)
        {
            int ccScore;
            if(!int.TryParse(Value, out ccScore))
                return new Pecuniam(1000);

            if (ccScore >= 800)
                return Pecuniam.GetRandPecuniam(10000, 20000, 100);
            if (ccScore >= 750)
                return Pecuniam.GetRandPecuniam(5000, 10000, 100);
            if (ccScore >= 700)
                return Pecuniam.GetRandPecuniam(3000, 5000, 100);
            if (ccScore >= 600)
                return Pecuniam.GetRandPecuniam(1000, 2000, 100);
           
            return new Pecuniam(1000);
        }
    }
}
