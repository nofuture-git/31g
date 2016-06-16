using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Shared;
using NoFuture.Util;
using NoFuture.Util.Etymological;
using NoFuture.Util.Etymological.Biz;

namespace NoFuture.Gen
{
    /// <summary>
    /// Utility methods ported from powershell scripts.
    /// </summary>
    public class Matching
    {
        #region assembly flattend match

        public static string GetFlattenedMemberObjPath(string flattenedMember, string strSeparator)
        {
            if (String.IsNullOrWhiteSpace(flattenedMember))
                return null;

            var separator = Constants.DEFAULT_TYPE_SEPARATOR;
            var sp = Convert.ToChar(" ");

            if (!String.IsNullOrWhiteSpace(strSeparator))
                separator = Convert.ToChar(strSeparator.Substring(0, 1));

            if (separator != sp && flattenedMember.Contains(sp) && flattenedMember.Split(sp).Length == 2)
                flattenedMember = flattenedMember.Split(sp)[1];

            if (!flattenedMember.Contains(separator))
                return string.Format(".{0}", flattenedMember);

            var srcName = flattenedMember.Replace(separator, Constants.DEFAULT_TYPE_SEPARATOR);
            var startAt = srcName.IndexOf(Constants.DEFAULT_TYPE_SEPARATOR);

            var lengthOf = srcName.Length - startAt;

            return srcName.Substring(startAt, lengthOf);
        }

        public static List<string> GetFlattenedPossiableMatches(string searchFor, List<string> fromFlatMembers, string strSeparator)
        {
            if (string.IsNullOrWhiteSpace(searchFor))
                return null;
            if (fromFlatMembers == null || fromFlatMembers.Count == 0)
                return null;

            var nomenclatures = new List<INomenclature>
            {
                new Demonyms(),
                new Metrix(),
                new NetworkResource(),
                new TelecoResource(),
                new Toponyms(),
                new Chronos(),
                new Monetary(),
                new CardinalOrdinal(),
                new Identity(),
                new BizStrings()
            };

            return
                fromFlatMembers.Where(ff => AutoMapperMatchingRules(searchFor, ff, strSeparator, nomenclatures))
                    .ToList();

        }
        public static bool AutoMapperMatchingRules(string searchFor, string searchIn, string strSeparator, List<INomenclature> nomenclatures)
        {
            var separator = Constants.DEFAULT_TYPE_SEPARATOR;
            if (!String.IsNullOrWhiteSpace(strSeparator))
                separator = Convert.ToChar(strSeparator.Substring(0, 1));

            var sp = Convert.ToChar(" ");

            if (String.IsNullOrWhiteSpace(searchIn) || String.IsNullOrWhiteSpace(searchFor))
                return false;

            string searchForType = string.Empty;
            string searchInType = string.Empty;
            if (separator != sp && searchFor.Contains(sp) && searchFor.Split(sp).Length == 2)
            {
                searchForType = searchFor.Split(sp)[0];
                searchFor = searchFor.Split(sp)[1];
            }

            if (separator != sp && searchIn.Contains(sp) && searchIn.Split(sp).Length == 2)
            {
                searchInType = searchIn.Split(sp)[0];
                searchIn = searchIn.Split(sp)[1];
            }

            //send back boolean types as match
            if (searchForType.Contains("Boolean") && searchInType.Contains("Boolean"))
                return true;

            var stopWords = new[]
            {
                "has", "are", "as", "at", "by", "can", "for", "from", "have", "in", "of", "than", "then", "to", "was",
                "with", "use"
            };

            if (searchIn.Split(separator).Any(x => string.Equals(x, searchFor, StringComparison.OrdinalIgnoreCase)))
                return true;

            if (nomenclatures == null || nomenclatures.Count <= 0)
                return false;

            var searchInWholeWords = Util.Etc.DistillToWholeWords(searchIn);
            var subject = Util.Etc.DistillToWholeWords(searchFor);
            var nomenclatureMatches = new List<INomenclature>();

            foreach (var subjectWord in subject)
            {
                if (stopWords.Any(x => string.Equals(x, subjectWord, StringComparison.OrdinalIgnoreCase)))
                    continue;
                var nomen = nomenclatures.FirstOrDefault(x => x.HasSemblance(subjectWord));
                
                if (nomen == null)
                    continue;

                nomenclatureMatches.Add(nomen);
            }

            return nomenclatureMatches.Count > 0 && nomenclatureMatches.All(n => n.HasSemblance(searchInWholeWords));
        }

        #endregion
    }
}
