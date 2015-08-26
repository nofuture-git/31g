using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NoFuture.Util.Etymological
{
    public interface INomenclature
    {
        List<string> Synonyms { get; }
        List<string> Antonyms { get; }
        bool HasSemblance(string someString);
        bool HasSemblance(string[] variousString);
        bool IsContrast(string someString);
        bool IsContrast(string[] variousString);
    }

    public abstract class NomenclatureBase : INomenclature
    {
        protected NomenclatureBase()
        {
            Synonyms = new List<string>();
            Antonyms = new List<string>();
        }
        public List<string> Synonyms { get; private set; }
        public List<string> Antonyms { get; private set; } 

        public bool HasSemblance(string someString)
        {
            if (string.IsNullOrWhiteSpace(someString))
                return false;
            return Synonyms.Any(x => Regex.IsMatch(someString, x, RegexOptions.IgnoreCase)) &&
                   !Antonyms.Any(x => Regex.IsMatch(someString, x, RegexOptions.IgnoreCase));
        }

        public virtual bool HasSemblance(string[] variousStrings)
        {
            if (variousStrings == null || variousStrings.Length <= 0)
                return false;
            return variousStrings.Any(HasSemblance);
        }

        public bool IsContrast(string someString)
        {
            if (string.IsNullOrWhiteSpace(someString))
                return false;
            return !Synonyms.Any(x => Regex.IsMatch(someString, x, RegexOptions.IgnoreCase)) &&
                   Antonyms.Any(x => Regex.IsMatch(someString, x, RegexOptions.IgnoreCase));
        }

        public bool IsContrast(string[] variousStrings)
        {
            if (variousStrings == null || variousStrings.Length <= 0)
                return false;
            return variousStrings.Any(IsContrast);
        }

        protected string GetNextToLast(string[] variousStrings)
        {
            if (variousStrings == null || variousStrings.Length <= 1)
                return string.Empty;
            for (var i = variousStrings.Length - 2; i >= 0; i--)
            {
                if (string.Equals("Of", variousStrings[i], StringComparison.OrdinalIgnoreCase))
                    continue;
                return variousStrings[i];
            }
            return string.Empty;
        }
    }
}