using System;
using System.Globalization;
using NoFuture.Util.Etymological;

namespace NoFuture.Rand.Domus.Pneuma
{
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
        #region fields
        private readonly Openness _o = new Openness();
        private readonly Conscientiousness _c = new Conscientiousness();
        private readonly Extraversion _e = new Extraversion();
        private readonly Agreeableness _a = new Agreeableness();
        private readonly Neuroticism _n = new Neuroticism();
        #endregion

        public Openness Openness { get { return _o; } }
        public Conscientiousness Conscientiousness { get { return _c; } }
        public Extraversion Extraversion { get { return _e; } }
        public Agreeableness Agreeableness { get { return _a; } }
        public Neuroticism Neuroticism { get { return _n; } }
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

        public double Zscore { get; set; }
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
            return "'" +  Abbrev + "':" + Value;
        }
    }
    [Serializable]
    public class Openness : Trait
    {
        public override string Abbrev
        {
            get { return "O"; }
        }

        public override INomenclature GetDescription()
        {
            return new Util.Etymological.Psy.Openness();
        }
    }
    [Serializable]
    public class Conscientiousness : Trait
    {
        public override string Abbrev
        {
            get { return "C"; }
        }

        public override INomenclature GetDescription()
        {
            return new Util.Etymological.Psy.Conscientiousness();
        }
    }
    [Serializable]
    public class Extraversion : Trait
    {
        public override string Abbrev
        {
            get { return "E"; }
        }

        public override INomenclature GetDescription()
        {
            return new Util.Etymological.Psy.Extraversion();
        }
    }
    [Serializable]
    public class Agreeableness : Trait
    {
        public override string Abbrev
        {
            get { return "A"; }
        }

        public override INomenclature GetDescription()
        {
            return new Util.Etymological.Psy.Agreeableness();
        }
    }
    [Serializable]
    public class Neuroticism : Trait
    {
        public override string Abbrev
        {
            get { return "N"; }
        }

        public override INomenclature GetDescription()
        {
            return new Util.Etymological.Psy.Neuroticism();
        }
    }

}
