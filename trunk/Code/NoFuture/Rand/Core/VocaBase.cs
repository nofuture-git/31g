using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Core
{
    [Serializable]
    public class VocaBase : IVoca
    {
        public List<Tuple<KindsOfNames, string>> Names { get; } = new List<Tuple<KindsOfNames, string>>();

        public virtual void UpsertName(KindsOfNames k, string name)
        {
            var cname = Names.FirstOrDefault(x => x.Item1 == k);

            if (cname != null)
            {
                Names.Remove(cname);
            }
            Names.Add(new Tuple<KindsOfNames, string>(k, name));
        }

        public virtual string GetName(KindsOfNames k)
        {
            var cname = Names.FirstOrDefault(x => x.Item1 == k);
            return cname?.Item2;
        }

        public virtual bool AnyOfKindOfName(KindsOfNames k)
        {
            return Names.Any(x => x.Item1 == k);
        }

        public virtual bool AnyOfNameAs(string name)
        {
            return Names.Any(x => string.Equals(x.Item2, name));
        }

        public bool RemoveNameByKind(KindsOfNames k)
        {
            var cname = Names.FirstOrDefault(x => x.Item1 == k);
            if (cname == null)
                return false;
            Names.Remove(cname);
            return true;
        }

        public int RemoveNameByValue(string name)
        {
            var cnt = 0;
            var byName = Names.Where(x => string.Equals(x.Item2, name)).ToList();
            foreach (var cname in byName)
            {
                Names.Remove(cname);
                cnt += 1;
            }
            return cnt;
        }
    }
}