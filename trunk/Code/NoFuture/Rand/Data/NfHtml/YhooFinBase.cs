using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Rand.Data.NfHtml
{
    public abstract class YhooFinBase : INfDynData
    {
        protected YhooFinBase(Uri srcUri)
        {
            SourceUri = srcUri;
        }

        protected DateTime[] Dts;
        protected Dictionary<string, int[]> DictionaryNums;

        protected bool GetDtsAndDictionary(string webResponseBody, string startMarker, string endMarker)
        {
            Func<string, bool> filter = s => s.Trim().Length > 1 && s.Trim() != "&nbsp;";

            string[] d;
            if (!Tokens.AspNetParseTree.TryGetCdata(webResponseBody, filter, out d))
                return false;
            var innerText = d.ToList();
            if (innerText.Count <= 0)
                return false;

            var st =
                innerText.FindIndex(
                    x => string.Equals(x.Trim(), startMarker, StringComparison.OrdinalIgnoreCase));
            var ed =
                innerText.FindIndex(
                    x => string.Equals(x.Trim(), endMarker, StringComparison.OrdinalIgnoreCase));

            if (st < 0 || ed < 0 || st > ed)
                return false;

            var targetData =
                innerText.Skip(st).Take(ed + 4 - st).Select(x => x.Replace("&nbsp;", string.Empty)).ToList();
            //we want the pattern of either text-date-date-date or text-number-number-number
            var outDt00 = DateTime.MinValue;
            var outDt01 = DateTime.MinValue;
            var outDt02 = DateTime.MinValue;
            var flag = false;
            Func<List<string>, int, bool> isTxtDtDtDt =
                (list, i) =>
                    list.Count > i && char.IsLetter(list[i].ToCharArray().First()) &&
                    DateTime.TryParse(list[i + 1], out outDt00) &&
                    DateTime.TryParse(list[i + 2], out outDt01) &&
                    DateTime.TryParse(list[i + 3], out outDt02);
            var intOut00 = 0;
            var intOut01 = 0;
            var intOut02 = 0;
            Func<List<string>, int, bool> isTxtIntIntInt =
                (list, i) =>
                    list.Count > i && char.IsLetter(list[i].ToCharArray().First()) &&
                    int.TryParse(list[i + 1].Replace(",", string.Empty).Replace("(", "-").Replace(")", string.Empty),
                        out intOut00) &&
                    int.TryParse(list[i + 2].Replace(",", string.Empty).Replace("(", "-").Replace(")", string.Empty),
                        out intOut01) &&
                    int.TryParse(list[i + 3].Replace(",", string.Empty).Replace("(", "-").Replace(")", string.Empty),
                        out intOut02);

            //move text list into structured data
            Dts = new[] { outDt00, outDt01, outDt02 };
            DictionaryNums = new Dictionary<string, int[]>();

            for (var i = 0; i < targetData.Count; i++)
            {
                if (!flag && isTxtDtDtDt(targetData, i))
                {
                    Dts = new[] { outDt00, outDt01, outDt02 };
                    flag = true;
                }
                else if (isTxtIntIntInt(targetData, i) && !DictionaryNums.ContainsKey(targetData[i]))
                    DictionaryNums.Add(targetData[i], new[]
                    {
                        intOut00,
                        intOut01,
                        intOut02
                    });
            }

            return DictionaryNums.Count > 0;
        }

        public Uri SourceUri { get; }

        public abstract List<dynamic> ParseContent(object content);
    }
}
