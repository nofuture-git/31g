using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp.Enums;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Base implementation of a household income item.
    /// </summary>
    public class Mereo : VocaBase, IMereo
    {
        public Mereo(string name)
        {
            Name = name;
        }

        public string Src { get; set; }
        public string Abbrev => GetName(KindsOfNames.Abbrev);
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

            return ic.Interval == ic.Interval
                   && ic.Name == ic.Name;
        }

        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 1 +
                   Interval.GetHashCode();
        }
    }
}
