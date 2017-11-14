using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus.Opes
{
    /// <summary>
    /// Base implementation of a household income item.
    /// </summary>
    public abstract class IncomeBase : VocaBase, IIncome
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
            var ic = obj as IIncome;
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
