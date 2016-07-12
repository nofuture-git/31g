using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Com;
using NoFuture.Rand.Data.Types;
using NoFuture.Shared;

namespace NoFuture.Rand.Data.NfText
{
    public class FedLrgBnk : INfDynData
    {
        public const string RELEASE_URL = "http://www.federalreserve.gov/releases/lbr/current/lrg_bnk_lst.txt";
        public Uri SourceUri => new Uri(RELEASE_URL);

        public static Dictionary<string, TypeOfBank> TypeOfBankAbbrev3Enum = new Dictionary<string, TypeOfBank>
        {
            {"SMB", TypeOfBank.StateChartered},
            {"NAT", TypeOfBank.NationallyChartered},
            {"SNM", TypeOfBank.StateCharteredNonMember}
        };

        public List<dynamic> ParseContent(object content)
        {
            DateTime rptDt = DateTime.Today;
            var lrgBnkLstTxt = content as string;
            const string TRAILER_LINE_TEXT = "Summary:";
            const string LINE_ITEM_REGEX = @"^\x20([A-Z0-9\x20\x26]{27})([0-9\x20]{10})([0-9\x20]{11})([A-Z\x20\x2C]{23})";

            if (string.IsNullOrWhiteSpace(lrgBnkLstTxt))
            {
                return null;
            }

            var crlf = new[] { (char)0x0D, (char)0x0A };
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
                DateTime.TryParse(strDt, out rptDt);
            }
            Func<string[], int, string> getLnVal = (strings, i) => strings.Length >= i ? strings[i] : string.Empty;
            var dataOut = new List<dynamic>();
            var lnRegex = new System.Text.RegularExpressions.Regex(LINE_ITEM_REGEX);
            foreach (var line in rptBody)
            {
                if (!lnRegex.IsMatch(line))
                    continue;
                var matches = lnRegex.Match(line);

                var bankName = matches.Groups[1].Captures[0].Value;

                if (string.IsNullOrWhiteSpace(bankName))
                    continue;

                var li = SplitLrgBnkListLine(line).ToArray();

                dataOut.Add(new
                {
                    RptDate = rptDt,
                    BankName = getLnVal(li, 0), 
                    NatlRank = getLnVal(li, 1), 
                    BankId = getLnVal(li, 2), 
                    Location = getLnVal(li, 3), 
                    Chtr = getLnVal(li, 4),
                    ConsolAssets = getLnVal(li, 5),
                    DomesticAssets = getLnVal(li, 6),
                    DomAsPercentCons = getLnVal(li,7),
                    CumlAsPercentCons = getLnVal(li,8),
                    NumOfDomBranches = getLnVal(li, 9),
                    NumOfFgnBranches = getLnVal(li, 10),
                    Ibf = getLnVal(li,11),
                    PercentFgnOwned = getLnVal(li, 12)
                });
            }
            
            return dataOut;
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
    }
}
//DataDownloadProgram ="http://www.federalreserve.gov/datadownload/";

//UnknownSearchPage = "http://www.ffiec.gov/nicpubweb/nicweb/SearchForm.aspx";

//AnotherUnknownSearchPage =
//    "https://cdr.ffiec.gov/CDR/SystemManagement/AccountEnrollment/lookupOrg.aspx";

//FdicListOfAllInstitutions = "https://www2.fdic.gov/idasp/Institutions2.zip";