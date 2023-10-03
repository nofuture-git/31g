using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Core
{
    /// <inheritdoc />
    /// <summary>
    /// Base implementation of <see cref="T:NoFuture.Rand.Core.IVoca" />
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

        public VocaBase(string name, string groupName): this(name)
        {
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
            if (!AnyNames())
                return GetType().Name;

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

        public void CopyNamesFrom(IVoca voca)
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
                    return ToCamelCase(x);
                case KindsOfTextCase.Pascel:
                    return ToPascalCase(x);
                case KindsOfTextCase.Kabab:
                    return TransformCaseToSeparator(ToCamelCase(x), '-')?.ToLower();
                case KindsOfTextCase.Snake:
                    return TransformCaseToSeparator(ToCamelCase(x), '_')?.ToLower();
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

        private static char[] PunctuationChars { get; set; } = {
            '!', '"', '#', '$', '%', '&', '\\', '\'', '(', ')',
            '*', '+', ',', '-', '.', '/', ':', ';', '<', '=', '>',
            '?','@', '[', ']', '^', '_', '`', '{', '|', '}', '~'
        };

        /// <summary>
        /// Given a string in the form of camel-case (or Pascal case) - a 
        /// <see cref="separator"/> will be inserted between characters 
        /// which are lowercase followed by uppercase.
        /// </summary>
        /// <param name="camelCaseString"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        private static string TransformCaseToSeparator(string camelCaseString, char separator)
        {
            if (String.IsNullOrWhiteSpace(camelCaseString))
                return String.Empty;
            var separatorName = new StringBuilder();
            var charArray = camelCaseString.ToCharArray();
            for (var i = 0; i < charArray.Length; i++)
            {
                separatorName.Append(charArray[i]);
                if (i + 1 >= charArray.Length)
                    continue;
                if (Char.IsLower(charArray[i]) && Char.IsUpper(charArray[i + 1]))
                {
                    separatorName.Append(separator);
                }
            }
            return separatorName.ToString();
        }

        /// <summary>
        /// Transforms a string of mixed case into standard camel-case (e.g. userName)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="perserveSep"></param>
        /// <returns></returns>
        private static string ToCamelCase(string name, bool perserveSep = false)
        {
            //is empty
            if (String.IsNullOrWhiteSpace(name))
                return String.Empty;

            name = name.Trim();

            //has no letters at all
            if (name.ToCharArray().All(x => !Char.IsLetter(x)))
                return name;

            //is all caps
            if (name.ToCharArray().Where(Char.IsLetter).All(Char.IsUpper))
                return name.ToLower();

            var nameFormatted = new StringBuilder();
            var markStart = false;
            var nameChars = name.ToCharArray();
            var sepChars = PunctuationChars.ToList();
            sepChars.Add(' ');
            for (var i = 0; i < nameChars.Length; i++)
            {
                var c = nameChars[i];

                if (sepChars.Contains(c))
                {
                    if (perserveSep)
                    {
                        nameFormatted.Append(c);
                        continue;
                    }
                    if (i + 1 < nameChars.Length)
                    {
                        nameChars[i + 1] = Char.ToUpper(nameChars[i + 1]);
                    }
                    continue;
                }

                if (!markStart)
                {
                    markStart = true;
                    nameFormatted.Append(c.ToString().ToLower());
                    continue;
                }

                if (i > 0 && Char.IsUpper(nameChars[i - 1]))
                {
                    nameFormatted.Append(c.ToString().ToLower());
                    continue;
                }

                nameFormatted.Append(c);

            }
            return nameFormatted.ToString();
        }

        /// <summary>
        /// Transforms <see cref="name"/> into Pascal case
        /// </summary>
        /// <param name="name"></param>
        /// <param name="preserveSep">Optional, set to true to have punctuation marks preserved</param>
        /// <returns></returns>
        private static string ToPascalCase(string name, bool preserveSep = false)
        {
            if (String.IsNullOrWhiteSpace(name))
                return String.Empty;
            var toCamelCase = new StringBuilder();
            var charArray = ToCamelCase(name, preserveSep).ToCharArray();
            toCamelCase.Append(Char.ToUpper(charArray[0]));
            for (var i = 1; i < charArray.Length; i++)
            {
                toCamelCase.Append(charArray[i]);
            }
            return toCamelCase.ToString();
        }
    }
}