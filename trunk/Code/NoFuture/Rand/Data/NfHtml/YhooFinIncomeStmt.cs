using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Rand.Data.NfHtml
{
    public class YhooFinIncomeStmt : INfCdata
    {
        public Uri SourceUri { get { return new Uri("http://finance.yahoo.com"); } }

        public List<dynamic> ParseContent(string webResponseBody)
        {
            Func<string, bool> filter = s => s.Trim().Length > 1 && s.Trim() != "&nbsp;";

            string[] d = null;
            if (!Tokens.AspNetParseTree.TryGetCdata(webResponseBody, filter, out d))
                return null;
            var innerText = d.ToList();
            if (innerText.Count <= 0)
                return null;

            var st =
                innerText.FindIndex(
                    x => string.Equals(x.Trim(), "Period Ending", StringComparison.OrdinalIgnoreCase));
            var ed =
                innerText.FindIndex(
                    x => string.Equals(x.Trim(), "Net Tangible Assets", StringComparison.OrdinalIgnoreCase));

            if (st < 0 || ed < 0 || st > ed)
                return null;

            var targetData =
                innerText.Skip(st).Take(ed + 4 - st).Select(x => x.Replace("&nbsp;", string.Empty)).ToList();
            //we want the pattern of either text-date-date-date or text-number-number-number
            var outDt00 = DateTime.MinValue;
            var outDt01 = DateTime.MinValue;
            var outDt02 = DateTime.MinValue;
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
            DateTime[] dts = { outDt00, outDt01, outDt02 };
            var dictNums = new Dictionary<string, int[]>();

            for (var i = 0; i < targetData.Count; i++)
            {
                if (isTxtDtDtDt(targetData, i))
                    dts = new []{outDt00, outDt01, outDt02};
                else if (isTxtIntIntInt(targetData, i) && !dictNums.ContainsKey(targetData[i]))
                    dictNums.Add(targetData[i], new[]
                    {
                        intOut00,
                        intOut01,
                        intOut02
                    });
            }
          
            var nfCdataOut = new List<dynamic>();

            for (var i = 0; i < 3; i++)
            {
                nfCdataOut.Add(new
                {
                    FiscalYearEndAt = dts[i].Year,
                    CashAndCashEquivalents =
                        dictNums.ContainsKey("Cash And Cash Equivalents") ? dictNums["Cash And Cash Equivalents"][i] : 0
                    ,
                    ShortTermInvestments =
                        dictNums.ContainsKey("Short Term Investments") ? dictNums["Short Term Investments"][i] : 0
                    ,
                    NetReceivables = dictNums.ContainsKey("Net Receivables") ? dictNums["Net Receivables"][i] : 0
                    ,
                    OtherCurrentAssets =
                        dictNums.ContainsKey("Other Current Assets") ? dictNums["Other Current Assets"][i] : 0
                    ,
                    TotalCurrentAssets =
                        dictNums.ContainsKey("Total Current Assets") ? dictNums["Total Current Assets"][i] : 0
                    ,
                    LongTermInvestments =
                        dictNums.ContainsKey("Long Term Investments") ? dictNums["Long Term Investments"][i] : 0
                    ,
                    PropertyPlantandEquipment =
                        dictNums.ContainsKey("Property Plant and Equipment")
                            ? dictNums["Property Plant and Equipment"][i]
                            : 0
                    ,
                    Goodwill = dictNums.ContainsKey("Goodwill") ? dictNums["Goodwill"][i] : 0
                    ,
                    IntangibleAssets = dictNums.ContainsKey("Intangible Assets") ? dictNums["Intangible Assets"][i] : 0
                    ,
                    AccumulatedAmortization =
                        dictNums.ContainsKey("Accumulated Amortization") ? dictNums["Accumulated Amortization"][i] : 0
                    ,
                    OtherAssets = dictNums.ContainsKey("Other Assets") ? dictNums["Other Assets"][i] : 0
                    ,
                    DeferredLongTermAssetCharges =
                        dictNums.ContainsKey("Deferred Long Term Asset Charges")
                            ? dictNums["Deferred Long Term Asset Charges"][i]
                            : 0
                    ,
                    TotalAssets = dictNums.ContainsKey("Total Assets") ? dictNums["Total Assets"][i] : 0
                    ,
                    AccountsPayable = dictNums.ContainsKey("Accounts Payable") ? dictNums["Accounts Payable"][i] : 0
                    ,
                    ShortCurrentLongTermDebt =
                        dictNums.ContainsKey("Short/Current Long Term Debt")
                            ? dictNums["Short/Current Long Term Debt"][i]
                            : 0
                    ,
                    OtherCurrentLiabilities =
                        dictNums.ContainsKey("Other Current Liabilities") ? dictNums["Other Current Liabilities"][i] : 0
                    ,
                    TotalCurrentLiabilities =
                        dictNums.ContainsKey("Total Current Liabilities") ? dictNums["Total Current Liabilities"][i] : 0
                    ,
                    LongTermDebt = dictNums.ContainsKey("Long Term Debt") ? dictNums["Long Term Debt"][i] : 0
                    ,
                    OtherLiabilities = dictNums.ContainsKey("Other Liabilities") ? dictNums["Other Liabilities"][i] : 0
                    ,
                    DeferredLongTermLiabilityCharges =
                        dictNums.ContainsKey("Deferred Long Term Liability Charges")
                            ? dictNums["Deferred Long Term Liability Charges"][i]
                            : 0
                    ,
                    MinorityInterest = dictNums.ContainsKey("Minority Interest") ? dictNums["Minority Interest"][i] : 0
                    ,
                    NegativeGoodwill = dictNums.ContainsKey("Negative Goodwill") ? dictNums["Negative Goodwill"][i] : 0
                    ,
                    TotalLiabilities = dictNums.ContainsKey("Total Liabilities") ? dictNums["Total Liabilities"][i] : 0
                    ,
                    MiscStocksOptionsWarrants =
                        dictNums.ContainsKey("Misc Stocks Options Warrants")
                            ? dictNums["Misc Stocks Options Warrants"][i]
                            : 0
                    ,
                    RedeemablePreferredStock =
                        dictNums.ContainsKey("Redeemable Preferred Stock")
                            ? dictNums["Redeemable Preferred Stock"][i]
                            : 0
                    ,
                    PreferredStock = dictNums.ContainsKey("Preferred Stock") ? dictNums["Preferred Stock"][i] : 0
                    ,
                    CommonStock = dictNums.ContainsKey("Common Stock") ? dictNums["Common Stock"][i] : 0
                    ,
                    RetainedEarnings = dictNums.ContainsKey("Retained Earnings") ? dictNums["Retained Earnings"][i] : 0
                    ,
                    TreasuryStock = dictNums.ContainsKey("Treasury Stock") ? dictNums["Treasury Stock"][i] : 0
                    ,
                    CapitalSurplus = dictNums.ContainsKey("Capital Surplus") ? dictNums["Capital Surplus"][i] : 0
                    ,
                    OtherStockholderEquity =
                        dictNums.ContainsKey("Other Stockholder Equity") ? dictNums["Other Stockholder Equity"][i] : 0
                    ,
                    TotalStockholderEquity =
                        dictNums.ContainsKey("Total Stockholder Equity") ? dictNums["Total Stockholder Equity"][i] : 0
                    ,
                    NetTangibleAssets =
                        dictNums.ContainsKey("Net Tangible Assets") ? dictNums["Net Tangible Assets"][i] : 0
                });
            }
            return nfCdataOut;

        }
    }
}
