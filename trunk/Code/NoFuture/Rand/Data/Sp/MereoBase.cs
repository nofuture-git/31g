using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus.Opes;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Base implementation of a household income item.
    /// </summary>
    public abstract class MereoBase : VocaBase, IMereo
    {
        public string Src { get; set; }
        public string Abbrev => GetName(KindsOfNames.Abbrev);
        public Pecuniam Value { get; set; }
        public IncomeInterval Interval { get; set; }

        public virtual string Name
        {
            get => GetName(KindsOfNames.Legal);
            set => UpsertName(KindsOfNames.Legal, value);
        }

        public override bool Equals(object obj)
        {
            var ic = obj as IMereo;
            if(ic == null)
                return base.Equals(obj);

            return ic.Value == Value
                   && ic.Interval == ic.Interval
                   && ic.Name == ic.Name;
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 1 +
                   Name?.GetHashCode() ?? 1 +
                   Interval.GetHashCode();
        }
    }
}
