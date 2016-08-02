using System;
using System.Globalization;
using NoFuture.Util.Etymological;

namespace NoFuture.Rand.Domus.Pneuma
{
    //some data at http://personality-testing.info/_rawdata/
    public interface IPersonality
    {
        Openness Openness { get; }
        Conscientiousness Conscientiousness { get; }
        Extraversion Extraversion { get; }
        Agreeableness Agreeableness { get; }
        Neuroticism Neuroticism { get; }
    }
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
    }

    public interface ITrait : IIdentifier<Dimension>
    {
        INomenclature GetDescription();
    }

    [Serializable]
    public class Dimension
    {
        public Dimension(double z)
        {
            Zscore = z;
        }

        public Dimension()
        {
            var s = Etx.PlusOrMinusOne;
            Zscore = s*Math.Round(Etx.MyRand.NextDouble(), 7);
        }

        public override int GetHashCode()
        {
            return Zscore.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var z = obj as Dimension;
            if (z == null)
                return false;
            return Math.Abs(Zscore - z.Zscore) < 0.0000000;
        }

        public override string ToString()
        {
            return Zscore.ToString(CultureInfo.InvariantCulture);
        }

        public double Zscore { get; }
    }
    [Serializable]
    public abstract class Trait : ITrait
    {
        protected Trait()
        {
            Value = new Dimension();
        }
        public virtual string Src { get; set; }
        public abstract INomenclature GetDescription();
        public abstract string Abbrev { get; }
        public Dimension Value { get; set; }
        public override string ToString()
        {
            return Abbrev + ":" + Value;
        }
    }
    [Serializable]
    public class Openness : Trait
    {
        public override string Abbrev => "O";

        public override INomenclature GetDescription()
        {
            return new Util.Etymological.Psy.Openness();
        }
    }
    [Serializable]
    public class Conscientiousness : Trait
    {
        public override string Abbrev => "C";

        public override INomenclature GetDescription()
        {
            return new Util.Etymological.Psy.Conscientiousness();
        }
    }
    [Serializable]
    public class Extraversion : Trait
    {
        public override string Abbrev => "E";

        public override INomenclature GetDescription()
        {
            return new Util.Etymological.Psy.Extraversion();
        }
    }
    [Serializable]
    public class Agreeableness : Trait
    {
        public override string Abbrev => "A";

        public override INomenclature GetDescription()
        {
            return new Util.Etymological.Psy.Agreeableness();
        }
    }
    [Serializable]
    public class Neuroticism : Trait
    {
        public override string Abbrev => "N";

        public override INomenclature GetDescription()
        {
            return new Util.Etymological.Psy.Neuroticism();
        }
    }

}
