using System;
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
        bool IsHomosexual { get; }
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
        private readonly bool _isHomosexual;
        #endregion

        public Personality() { }

        public Personality(bool isHomosexual)
        {
            _isHomosexual = isHomosexual;
        }

        public Openness Openness { get { return _o; } }
        public Conscientiousness Conscientiousness { get { return _c; } }
        public Extraversion Extraversion { get { return _e; } }
        public Agreeableness Agreeableness { get { return _a; } }
        public Neuroticism Neuroticism { get { return _n; } }
        public bool IsHomosexual { get { return _isHomosexual; } }
    }

    public interface ITrait : IIdentifier<Dimension>
    {
        INomenclature GetDescription();
    }

    [Serializable]
    public enum Dimension
    {
        Null = 0,
        Never,
        Rarely,
        Sometimes,
        Often,
        Always
    }
    [Serializable]
    public abstract class Trait : ITrait
    {
        protected Trait()
        {
            var pv = Etx.IntNumber(1, 5);
            switch (pv)
            {
                case 1:
                    Value = Dimension.Never;
                    break;
                case 2:
                    Value = Dimension.Rarely;
                    break;
                case 3:
                    Value = Dimension.Sometimes;
                    break;
                case 4:
                    Value = Dimension.Often;
                    break;
                case 5:
                    Value = Dimension.Always;
                    break;
            }
        }

        public abstract INomenclature GetDescription();
        public abstract string Abbrev { get; }
        public Dimension Value { get; set; }
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
