using System;
using System.Linq;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Domus.Pneuma
{
    [Serializable]
    public class Personality : IPersonality
    {
        public Openness Openness { get; } = new Openness();
        public Conscientiousness Conscientiousness { get; } = new Conscientiousness();
        public Extraversion Extraversion { get; } = new Extraversion();
        public Agreeableness Agreeableness { get; } = new Agreeableness();
        public Neuroticism Neuroticism { get; } = new Neuroticism();

        public override string ToString()
        {
            return "{" +
                   string.Join(",", Openness.ToString(), Conscientiousness.ToString(), Extraversion.ToString(),
                       Agreeableness.ToString(), Neuroticism.ToString()) + "}";
        }

        public bool GetRandomActsIrresponsible()
        {
            return Etx.RandomValueInNormalDist(Conscientiousness.Value.Zscore, Conscientiousness.Value.StdDev) < 0;
        }

        public bool GetRandomActsStressed()
        {
            return Etx.RandomValueInNormalDist(Neuroticism.Value.Zscore, Neuroticism.Value.StdDev) > 0;
        }

        public override bool Equals(object obj)
        {
            var p = obj as IPersonality;
            if (p == null)
                return false;
            return new[]
            {
                p.Openness.Equals(Openness),
                p.Conscientiousness.Equals(Conscientiousness),
                p.Extraversion.Equals(Extraversion),
                p.Agreeableness.Equals(Agreeableness),
                p.Neuroticism.Equals(Neuroticism)
            }.All(x => x);
        }

        public override int GetHashCode()
        {
            var h = Openness?.GetHashCode() ?? 1;
            h += Conscientiousness?.GetHashCode() ?? 1;
            h += Extraversion?.GetHashCode() ?? 1;
            h += Agreeableness?.GetHashCode() ?? 1;
            h += Neuroticism?.GetHashCode() ?? 1;
            return h;
        }
    }
}
