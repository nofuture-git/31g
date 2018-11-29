using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core.Enums;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Core
{
    /// <inheritdoc />
    /// <summary>
    /// Base implemenation of <see cref="T:NoFuture.Rand.Core.IVoca" />
    /// </summary>
    [Serializable]
    public class VocaBase : IVoca
    {
        protected internal List<Tuple<KindsOfNames, string>> Names { get; } = new List<Tuple<KindsOfNames, string>>();

        public VocaBase()
        {

        }

        public VocaBase(string name)
        {
            Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, name));
        }

        public VocaBase(string name, string groupName)
        {
            Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, name));
            Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Group, groupName));
        }

        public virtual string Name
        {
            get => GetName(KindsOfNames.Legal);
            set => AddName(KindsOfNames.Legal, value);
        }

        public virtual int NamesCount => Names.Count;

        public virtual void AddName(KindsOfNames k, string name)
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

        public bool AnyNames(Predicate<KindsOfNames> filter)
        {
            return filter == null ? Names.Any() : Names.Any(t => filter(t.Item1));
        }

        public bool AnyNames(Predicate<string> filter)
        {
            return filter == null ? Names.Any() : Names.Any(t => filter(t.Item2));
        }

        public virtual bool AnyNames(Func<KindsOfNames, string, bool> filter)
        {
            return filter == null ? Names.Any() : Names.Any(v => filter(v.Item1, v.Item2));
        }

        public virtual bool AnyNames()
        {
            return Names.Any();
        }

        public virtual int RemoveName(Predicate<KindsOfNames> filter)
        {
            filter = filter ?? (k => true);
            var cnt = 0;
            var byName = Names.Where(x => filter(x.Item1)).ToList();
            foreach (var cname in byName)
            {
                Names.Remove(cname);
                cnt += 1;
            }
            return cnt;
        }

        public virtual int RemoveName(Predicate<string> filter)
        {
            filter = filter ?? (k => true);
            var cnt = 0;
            var byName = Names.Where(x => filter(x.Item2)).ToList();
            foreach (var cname in byName)
            {
                Names.Remove(cname);
                cnt += 1;
            }
            return cnt;
        }

        public virtual int RemoveName(Func<KindsOfNames, string, bool> filter)
        {
            filter = filter ?? ((k,v) => true);
            var cnt = 0;
            var byName = Names.Where(x => filter(x.Item1, x.Item2)).ToList();
            foreach (var cname in byName)
            {
                Names.Remove(cname);
                cnt += 1;
            }
            return cnt;
        }

        public override bool Equals(object obj)
        {
            return obj is IVoca voca && Equals(voca, this);
        }

        public static bool Equals(IVoca obj1, IVoca obj2)
        {
            if (obj1 == null || obj2 == null)
                return false;

            if ( obj1.NamesCount != obj2.NamesCount)
                return false;

            foreach (var kon1 in obj1.GetAllKindsOfNames())
            {
                if (!obj2.AnyNames((k,v) => k == kon1 && v == obj1.GetName(kon1)))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return Names.GetHashCode();
        }

        public virtual IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => TransformText(x, txtCase);
            var itemData = new Dictionary<string, object>();

            foreach (var nameTuple in Names)
            {
                if(nameTuple == null)
                    continue;
                var grp = nameTuple.Item1;
                var nm = nameTuple.Item2;
                if (string.IsNullOrWhiteSpace(nm))
                    continue;
                itemData.Add(textFormat(grp + "Name"), nm);
            }

            return itemData;
        }

        public virtual KindsOfNames[] GetAllKindsOfNames()
        {
            return Names.Select(n => n.Item1).ToArray();
        }

        public virtual void CopyNamesFrom(IVoca voca)
        {
            if (voca == null)
                return;

            foreach(var k in voca.GetAllKindsOfNames())
                AddName(k, voca.GetName(k));
        }

        public static string TransformText(string x, KindsOfTextCase txtCase)
        {
            x = x.Replace('\n', ' ').Replace('\r', ' ');
            switch (txtCase)
            {
                case KindsOfTextCase.Camel:
                    return Etc.ToCamelCase(x);
                case KindsOfTextCase.Pascel:
                    return Etc.ToPascelCase(x);
                case KindsOfTextCase.Kabab:
                    return Etc.TransformCaseToSeparator(Etc.ToCamelCase(x), '-')?.ToLower();
                case KindsOfTextCase.Snake:
                    return Etc.TransformCaseToSeparator(Etc.ToCamelCase(x), '_')?.ToLower();
            }

            return x;
        }

        protected static void AddOrReplace(IDictionary<string, object> a, IDictionary<string, object> b)
        {
            a = a ?? new Dictionary<string, object>();
            b = b ?? new Dictionary<string, object>();

            foreach (var k in b.Keys)
            {
                if (a.ContainsKey(k))
                    a[k] = b[k];
                else
                    a.Add(k, b[k]);
            }
        }
    }
}