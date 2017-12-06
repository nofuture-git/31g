using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp.Cc;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus;
using NoFuture.Rand.Gov.Nhtsa;

namespace NoFuture.Rand.Data.Sp
{
    /// <inheritdoc cref="IMereo"/>
    /// <summary>
    /// Base implementation a name of any kind of money entry
    /// </summary>
    [Serializable]
    public class Mereo : VocaBase, IMereo
    {
        private readonly List<string> _eg = new List<string>();
        private readonly List<string> _aka = new List<string>();
        private string _definition;

        public Mereo(string name)
        {
            Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, name));
        }

        public Mereo(string name, string groupName)
        {
            Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, name));
            Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Group, groupName));
        }

        public Mereo(IVoca names)
        {
            CopyFrom(names);
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

        public List<string> ExempliGratia => _eg;
        public List<string> Akas => _aka;
        public string Instructions { get; set; }

        public string Definition
        {
            get => _definition;
            set => _definition = value;
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
