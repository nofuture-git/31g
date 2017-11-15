using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp.Cc;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus;
using NoFuture.Rand.Gov.Nhtsa;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Base implementation a name of any kind of money entry
    /// </summary>
    [Serializable]
    public class Mereo : VocaBase, IMereo
    {
        public Mereo(string name)
        {
            Name = name;
        }

        public string Src { get; set; }
        public string Abbrev => GetName(KindsOfNames.Abbrev);

        public Interval Interval { get; set; }
        public Classification Classification { get; set; }

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

            return ic.Name == ic.Name;
        }

        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 1;
        }

        public override string ToString()
        {
            return Name;
        }

        public static IMereo GetMereoById(Identifier property, string prefix = null)
        {
            switch (property)
            {
                case null:
                    return new Mereo(prefix);
                case ResidentAddress residenceLoan:
                    return residenceLoan.IsLeased
                        ? new Mereo(String.Join(" ", prefix, "Rent Payment"))
                        : new Mereo(String.Join(" ", prefix, "Mortgage Payment"));
                case Vin _:
                    return new Mereo(String.Join(" ", prefix, "Vehicle Payment"));
                case CreditCardNumber _:
                    return new Mereo(String.Join(" ", prefix, "Cc Payment"));
                case AccountId _:
                    return new Mereo(String.Join(" ", prefix, "Bank Account Transfer"));
            }

            return new Mereo(prefix);
        }
    }
}
