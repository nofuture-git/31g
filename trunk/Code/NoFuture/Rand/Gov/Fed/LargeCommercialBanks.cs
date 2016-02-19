using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Com;
using NoFuture.Shared;

namespace NoFuture.Rand.Gov.Fed
{
    public class LargeCommercialBanks
    {
        public const string RELEASE_URL = "http://www.federalreserve.gov/releases/lbr/current/lrg_bnk_lst.txt";
        private static List<Tuple<string, string>> _bankNames;

        public static Uri GetUriLargeBankList { get { return new Uri(RELEASE_URL);} }

        public static Dictionary<string, TypeOfBank> TypeOfBankAbbrev3Enum = new Dictionary<string, TypeOfBank>
        {
            {"SMB", TypeOfBank.StateChartered},
            {"NAT", TypeOfBank.NationallyChartered},
            {"SNM", TypeOfBank.StateCharteredNonMember}
        };
        public static List<Tuple<string, string>> ParseBankData(string lrgBnkLstTxt, out DateTime? rptDate)
        {
            rptDate = null;
            if (_bankNames != null)
                return _bankNames;

            const string TRAILER_LINE_TEXT = "Summary:";
            const string LINE_ITEM_REGEX = @"^\x20([A-Z0-9\x20\x26]{27})([0-9\x20]{10})([0-9\x20]{11})([A-Z\x20\x2C]{23})";

            var rtrnList = new List<Tuple<string, string>>();

            if (string.IsNullOrWhiteSpace(lrgBnkLstTxt))
            {
                return null;
            }

            var crlf = new char[] {(char) 0x0D, (char) 0x0A};
            var lines = lrgBnkLstTxt.Split(crlf);

            //find the range for the reports body
            int startAtLine = 0;
            int numOfLines = 0;
            int matchCount = 0;
            for (var i = 0; i < lines.Length; i++)
            {
                var ln = lines[i];
                if (string.IsNullOrWhiteSpace(ln))
                    continue;
                if (ln.ToCharArray().All(c => c == '-' || c == (char)0x20))
                    matchCount += 1;
                if (matchCount == 2)
                {
                    startAtLine = i;
                    matchCount = 0;
                    continue;
                }

                if (startAtLine > 0)
                    numOfLines += 1;

                if (ln == TRAILER_LINE_TEXT)
                    break;
            }

            if (numOfLines < 1)
                return null;
            var rptBody = new string[numOfLines];
            var rptHdr = new string[startAtLine];
            Array.Copy(lines, startAtLine, rptBody, 0, numOfLines);
            Array.Copy(lines, 0, rptHdr, 0, startAtLine);
            var regexCatalog = new RegexCatalog();
            var dtRegex = new System.Text.RegularExpressions.Regex(regexCatalog.LongDate);
            foreach (var hdrLn in rptHdr)
            {
                if (!dtRegex.IsMatch(hdrLn))
                    continue;
                var dtMatch = dtRegex.Match(hdrLn);
                var strDt = dtMatch.Groups[0].Captures[0].Value;
                var rptDt = DateTime.MinValue;
                if (DateTime.TryParse(strDt, out rptDt))
                    rptDate = rptDt;
            }

            var lnRegex = new System.Text.RegularExpressions.Regex(LINE_ITEM_REGEX);
            foreach (var line in rptBody)
            {
                if (!lnRegex.IsMatch(line))
                    continue;
                var matches = lnRegex.Match(line);

                var bankName = matches.Groups[1].Captures[0].Value;
                
                if (string.IsNullOrWhiteSpace(bankName))
                    continue;

                rtrnList.Add(new Tuple<string, string>(bankName, line)); 

            }
            _bankNames = rtrnList;
            return rtrnList;
        }

        public static List<string> SplitLrgBnkListLine(string lrgBnkLstLine)
        {
            if (string.IsNullOrWhiteSpace(lrgBnkLstLine))
                return null;
            if (lrgBnkLstLine.Length < 126)
                return null;
            var charIdx = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(0, 30),
                new Tuple<int, int>(31, 8),
                new Tuple<int, int>(39, 12),
                new Tuple<int, int>(51, 23),
                new Tuple<int, int>(74, 3),
                new Tuple<int, int>(77, 10),
                new Tuple<int, int>(87, 10),
                new Tuple<int, int>(96, 4),
                new Tuple<int, int>(100, 5),
                new Tuple<int, int>(105, 8),
                new Tuple<int, int>(113, 4),
                new Tuple<int, int>(117, 4),
                new Tuple<int, int>(121, 5)
            };
            return charIdx.Select(idx => lrgBnkLstLine.Substring(idx.Item1, idx.Item2).Trim()).ToList();

        }

        //DataDownloadProgram ="http://www.federalreserve.gov/datadownload/";

        //UnknownSearchPage = "http://www.ffiec.gov/nicpubweb/nicweb/SearchForm.aspx";

        //AnotherUnknownSearchPage =
        //    "https://cdr.ffiec.gov/CDR/SystemManagement/AccountEnrollment/lookupOrg.aspx";

        //FdicListOfAllInstitutions = "https://www2.fdic.gov/idasp/Institutions2.zip";

    }
}
