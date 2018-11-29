using System;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Domus
{
    [Serializable]
    public class Parent : VocaBase, IRelation
    {
        public Parent(IPerson p, KindsOfNames k)
        {
            Est = p;
            Names.Add(new Tuple<KindsOfNames, string>(k, string.Empty));
        }
        public IPerson Est { get; }

        public override string ToString()
        {
            var parentTitle = "";

            var kons = GetAllKindsOfNames();
            if (kons.Any())
            {
                parentTitle = string.Join(",", kons.First().ToDiscreteKindsOfNames());
            }

            return string.Join(" ", parentTitle, Est?.FirstName, Est?.LastName);
        }

        public override int GetHashCode()
        {
            return Est?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            var p = obj as Parent;
            if (p?.Est == null || Est == null)
                return false;

            return base.Equals(p) && p.Est.Equals(Est);
        }
    }
}
