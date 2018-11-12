using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Pneuma
{
    /// <inheritdoc cref="IPersonality" />
    /// <summary>
    /// </summary>
    /// <remarks>
    /// example of reassigning individual traits
    /// <![CDATA[
    /// //all traits are generated with random z-scores
    /// var personality = new Personality();
    /// 
    /// //a trait implements IIdentifier<Dimension> so its Value property is read-write
    /// personality.Openness.Value = new Dimension(0.99); //open to anything and everything
    /// ]]>
    /// </remarks>
    [Serializable]
    public class Personality : IPersonality, IObviate
    {
        public Openness Openness { get; } = new Openness();
        public Conscientiousness Conscientiousness { get; } = new Conscientiousness();
        public Extraversion Extraversion { get; } = new Extraversion();
        public Agreeableness Agreeableness { get; } = new Agreeableness();
        public Neuroticism Neuroticism { get; } = new Neuroticism();

        /// <summary>
        /// Gets a personality at random
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static Personality RandomPersonality()
        {
            return new Personality();
        }

        public override string ToString()
        {
            return "(" +
                   string.Join(",", Openness.ToString(), Conscientiousness.ToString(), Extraversion.ToString(),
                       Agreeableness.ToString(), Neuroticism.ToString()) + ")";
        }

        public bool GetRandomActsIrresponsible()
        {
            return Etx.RandomValueInNormalDist(Conscientiousness.Value.Zscore, Conscientiousness.Value.StdDev) < 0;
        }

        public bool GetRandomActsStressed()
        {
            return Etx.RandomValueInNormalDist(Neuroticism.Value.Zscore, Neuroticism.Value.StdDev) > 0;
        }

        public bool GetRandomActsSpontaneous()
        {
            return Etx.RandomValueInNormalDist(Openness.Value.Zscore, Openness.Value.StdDev) > 0;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is IPersonality p))
                return false;
            var predicates = new[]
            {
                p.Openness.Equals(Openness),
                p.Conscientiousness.Equals(Conscientiousness),
                p.Extraversion.Equals(Extraversion),
                p.Agreeableness.Equals(Agreeableness),
                p.Neuroticism.Equals(Neuroticism)
            };

            return predicates.All(x => x);
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

        public IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x, txtCase);
            var itemData = new Dictionary<string, object>
            {
                {textFormat(nameof(Openness)), Openness.Value.Zscore},
                {textFormat(nameof(Conscientiousness)), Conscientiousness.Value.Zscore},
                {textFormat(nameof(Extraversion)), Extraversion.Value.Zscore},
                {textFormat(nameof(Agreeableness)), Agreeableness.Value.Zscore},
                {textFormat(nameof(Neuroticism)), Neuroticism.Value.Zscore}
            };

            return itemData;
        }
    }
}
