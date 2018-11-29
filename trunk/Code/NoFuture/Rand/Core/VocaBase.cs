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

        public int GetCountOfNames()
        {
            return Names.Count;
        }

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

        public virtual bool AnyNames()
        {
            return Names.Any();
        }

        public virtual bool AnyOfKind(KindsOfNames k)
        {
            return Names.Any(x => x.Item1 == k);
        }

        public virtual bool AnyOfKindContaining(KindsOfNames k)
        {
            var allMyKinds = Names.SelectMany(x => ToDiscreteKindsOfNames(x.Item1)).ToArray();
            var allItsKinds = ToDiscreteKindsOfNames(k);

            return allItsKinds.All(ak => allMyKinds.Contains(ak));
        }

        public virtual bool AnyOfNameAs(string name)
        {
            return Names.Any(x => string.Equals(x.Item2, name));
        }

        public virtual bool AnyOfKindAndValue(KindsOfNames k, string name)
        {
            return Names.Any(x => x.Item1 == k && x.Item2 == name);
        }

        public virtual bool RemoveNameByKind(KindsOfNames k)
        {
            var cname = Names.FirstOrDefault(x => x.Item1 == k);
            if (cname == null)
                return false;
            Names.Remove(cname);
            return true;
        }

        public virtual int RemoveNameByValue(string name)
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

        public virtual bool RemoveNameByKindAndValue(KindsOfNames k, string name)
        {
            var cname = Names.FirstOrDefault(x => x.Item1 == k && x.Item2 == name);
            if (cname == null)
                return false;
            Names.Remove(cname);
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is IVoca voca && Equals(voca, this);
        }

        public static bool Equals(IVoca obj1, IVoca obj2)
        {
            if (obj1 == null || obj2 == null)
                return false;

            if ( obj1.GetCountOfNames() != obj2.GetCountOfNames())
                return false;

            foreach (var kon1 in obj1.GetAllKindsOfNames())
            {
                if (!obj2.AnyOfKindAndValue(kon1, obj1.GetName(kon1)))
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

        public virtual void CopyFrom(IVoca voca)
        {
            if (voca == null)
                return;

            foreach(var k in voca.GetAllKindsOfNames())
                AddName(k, voca.GetName(k));
        }

        /// <summary>
        /// Turns a bitwise combination of <see cref="KindsOfNames"/> into a discrete list
        /// </summary>
        /// <param name="kon"></param>
        /// <returns></returns>
        public static KindsOfNames[] ToDiscreteKindsOfNames(KindsOfNames kon)
        {
            var vals = Enum.GetValues(typeof(KindsOfNames)).Cast<KindsOfNames>().ToArray();
            var dKon = new List<KindsOfNames>();
            foreach (var val in vals)
            {
                var d = (UInt32) val & (UInt32) kon;
                if(d == (UInt32)val)
                    dKon.Add(val);
            }
            
            return dKon.Distinct().ToArray();
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