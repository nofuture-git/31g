using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Source;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Gov.Fed;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Com.NfText
{
    public class FedLrgBnk : NfDynDataBase
    {
        private static Bank[] _fedReleaseLrgBnkNames;
        public const string RELEASE_URL = "http://www.federalreserve.gov/releases/lbr/current/lrg_bnk_lst.txt";
        public FedLrgBnk():base(new Uri(RELEASE_URL)) { }

        public static Dictionary<string, TypeOfBank> TypeOfBankAbbrev3Enum = new Dictionary<string, TypeOfBank>
        {
            {"SMB", TypeOfBank.StateChartered},
            {"NAT", TypeOfBank.NationallyChartered},
            {"SNM", TypeOfBank.StateCharteredNonMember}
        };

        public static Uri GetUri()
        {
            return new Uri(RELEASE_URL);
        }

        /// <summary>
        /// Ctor is based on single line from the fed's text report
        /// </summary>
        /// <param name="li"></param>
        public static Bank GetBankFromDynData(dynamic li)
        {
            var bank = new Bank();
            const string COMMA = ",";
            const string LETTER_Y = "Y";
            var unfoldedName = Bank.GetBankFullName(li.BankName);

            bank.UpsertName(KindsOfNames.Legal, unfoldedName);
            bank.UpsertName(KindsOfNames.Abbrev, li.BankName);
            bank.Rssd = new ResearchStatisticsSupervisionDiscount { Value = li.BankId };
            if (UsCityStateZip.TryParse(li.Location, out UsCityStateZip cityOut, false))
                bank.BusinessAddress = new Tuple<UsStreetPo, UsCityStateZip>(null, cityOut);
            if (TypeOfBankAbbrev3Enum.ContainsKey(li.Chtr))
                bank.BankType = TypeOfBankAbbrev3Enum[li.Chtr];
            var assets = new FinancialAssets { Src = FedLrgBnk.RELEASE_URL };
            if (decimal.TryParse(li.ConsolAssets.Replace(COMMA, string.Empty), out decimal conAssts))
                assets.TotalAssets = new Pecuniam(conAssts * 1000);
            if (decimal.TryParse(li.DomesticAssets.Replace(COMMA, string.Empty), out decimal domAssts))
                assets.DomesticAssets = new Pecuniam(domAssts * 1000);
            if (int.TryParse(li.NumOfDomBranches.Replace(COMMA, string.Empty), out int domBranches))
                assets.DomesticBranches = domBranches;
            if (int.TryParse(li.NumOfFgnBranches.Replace(COMMA, string.Empty), out int frnBranches))
                assets.ForeignBranches = frnBranches;
            bank.IsInternational = li.Ibf == LETTER_Y;
            if (int.TryParse(li.PercentFgnOwned, out int pfo))
                assets.PercentForeignOwned = Math.Round((double)pfo / 100, 2);
            bank.Assets = new Dictionary<DateTime, FinancialAssets> { { li.RptDate, assets } };
            return bank;
        }

        public override IEnumerable<dynamic> ParseContent(object content)
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

        /// <summary>
        /// Loads a list of <see cref="FinancialFirm"/> by parsing the data from <see cref="DataFiles.LRG_BNK_LST_DATA_FILE"/> 
        /// </summary>
        public static Bank[] CommercialBankData
        {
            get
            {
                if (_fedReleaseLrgBnkNames != null && _fedReleaseLrgBnkNames.Length > 0)
                    return _fedReleaseLrgBnkNames;

                var rawData = DataFiles.GetByName(DataFiles.LRG_BNK_LST_DATA_FILE);
                if (string.IsNullOrWhiteSpace(rawData))
                    return new Bank[0];//return empty list for missing data

                var myDynData = DynamicDataFactory.GetDataParser(new Uri(RELEASE_URL));
                var myDynDataRslt = myDynData.ParseContent(rawData);


                //take each line data structure and compose a full object
                var tempList = myDynDataRslt.Select(pd => GetBankFromDynData(pd)).Cast<Bank>().ToList();

                if (tempList.Count > 0)
                    _fedReleaseLrgBnkNames = tempList.ToArray();
                return _fedReleaseLrgBnkNames;
            }
        }
    }
}
//DataDownloadProgram ="http://www.federalreserve.gov/datadownload/";

//UnknownSearchPage = "http://www.ffiec.gov/nicpubweb/nicweb/SearchForm.aspx";

//AnotherUnknownSearchPage =
//    "https://cdr.ffiec.gov/CDR/SystemManagement/AccountEnrollment/lookupOrg.aspx";

//FdicListOfAllInstitutions = "https://www2.fdic.gov/idasp/Institutions2.zip";