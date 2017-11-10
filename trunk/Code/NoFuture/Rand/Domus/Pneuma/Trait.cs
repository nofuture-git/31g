using System;

namespace NoFuture.Rand.Domus.Pneuma
{
    [Serializable]
    public abstract class Trait : ITrait
    {

        protected Trait()
        {
            Value = new Dimension();
        }
        public virtual string Src { get; set; }
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
}