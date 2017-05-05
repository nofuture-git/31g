using System;
using System.Globalization;
using System.Linq;
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

        bool GetRandomActsIrresponsible();
        bool GetRandomActsStressed();
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

    public interface ITrait : IIdentifier<Dimension>
    {
        INomenclature GetDescription();
    }

    [Serializable]
    public class Dimension
    {
        public Dimension(double z, double stdDev)
        {
            Zscore = z;
            StdDev = stdDev;
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
        public double StdDev { get; } = 0.125D;
    }
    [Serializable]
    public abstract class Trait : ITrait
    {
        protected INomenclature _nom;

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

        public override int GetHashCode()
        {
            var vh = Value?.GetHashCode() ?? 1;
            var ah = Abbrev?.GetHashCode() ?? 0;
            return vh + ah;
        }

        public override bool Equals(object obj)
        {
            var t = obj as ITrait;
            if (t == null)
                return false;
            return t.Abbrev.Equals(Abbrev) && t.Value.Equals(Value);
        }
    }
    [Serializable]
    public class Openness : Trait
    {
        public override string Abbrev => "O";

        public override INomenclature GetDescription()
        {
            return _nom ?? (_nom = new Util.Etymological.Psy.Openness());
        }
    }
    [Serializable]
    public class Conscientiousness : Trait
    {
        public override string Abbrev => "C";

        public override INomenclature GetDescription()
        {
            return _nom ?? (_nom = new Util.Etymological.Psy.Conscientiousness());
        }
    }
    [Serializable]
    public class Extraversion : Trait
    {
        public override string Abbrev => "E";

        public override INomenclature GetDescription()
        {
            return _nom ?? (_nom = new Util.Etymological.Psy.Extraversion());
        }
    }
    [Serializable]
    public class Agreeableness : Trait
    {
        public override string Abbrev => "A";

        public override INomenclature GetDescription()
        {
            return _nom ?? (_nom = new Util.Etymological.Psy.Agreeableness());
        }
    }
    [Serializable]
    public class Neuroticism : Trait
    {
        public override string Abbrev => "N";

        public override INomenclature GetDescription()
        {
            return _nom ?? (_nom = new Util.Etymological.Psy.Neuroticism());
        }
    }

}
