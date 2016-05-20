using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Data.NfHtml
{
    public class YhooFinBalanceSheet : YhooFinBase
    {
        public YhooFinBalanceSheet(Uri srcUri) :base(srcUri) { }

        public override List<dynamic> ParseContent(object content)
        {
            var webResponseBody = content as string;
            if (webResponseBody == null)
                return null;

            if (!GetDtsAndDictionary(webResponseBody, "Period Ending", "Net Tangible Assets"))
                return null;

            var nfCdataOut = new List<dynamic>();

            for (var i = 0; i < 3; i++)
            {
                nfCdataOut.Add(new
                {
                    FiscalYearEndAt = Dts[i].Year,
                    CashAndCashEquivalents =
                        DictionaryNums.ContainsKey("Cash And Cash Equivalents") ? DictionaryNums["Cash And Cash Equivalents"][i] : 0
                    ,
                    ShortTermInvestments =
                        DictionaryNums.ContainsKey("Short Term Investments") ? DictionaryNums["Short Term Investments"][i] : 0
                    ,
                    NetReceivables = DictionaryNums.ContainsKey("Net Receivables") ? DictionaryNums["Net Receivables"][i] : 0
                    ,
                    OtherCurrentAssets =
                        DictionaryNums.ContainsKey("Other Current Assets") ? DictionaryNums["Other Current Assets"][i] : 0
                    ,
                    TotalCurrentAssets =
                        DictionaryNums.ContainsKey("Total Current Assets") ? DictionaryNums["Total Current Assets"][i] : 0
                    ,
                    LongTermInvestments =
                        DictionaryNums.ContainsKey("Long Term Investments") ? DictionaryNums["Long Term Investments"][i] : 0
                    ,
                    PropertyPlantandEquipment =
                        DictionaryNums.ContainsKey("Property Plant and Equipment")
                            ? DictionaryNums["Property Plant and Equipment"][i]
                            : 0
                    ,
                    Goodwill = DictionaryNums.ContainsKey("Goodwill") ? DictionaryNums["Goodwill"][i] : 0
                    ,
                    IntangibleAssets = DictionaryNums.ContainsKey("Intangible Assets") ? DictionaryNums["Intangible Assets"][i] : 0
                    ,
                    AccumulatedAmortization =
                        DictionaryNums.ContainsKey("Accumulated Amortization") ? DictionaryNums["Accumulated Amortization"][i] : 0
                    ,
                    OtherAssets = DictionaryNums.ContainsKey("Other Assets") ? DictionaryNums["Other Assets"][i] : 0
                    ,
                    DeferredLongTermAssetCharges =
                        DictionaryNums.ContainsKey("Deferred Long Term Asset Charges")
                            ? DictionaryNums["Deferred Long Term Asset Charges"][i]
                            : 0
                    ,
                    TotalAssets = DictionaryNums.ContainsKey("Total Assets") ? DictionaryNums["Total Assets"][i] : 0
                    ,
                    AccountsPayable = DictionaryNums.ContainsKey("Accounts Payable") ? DictionaryNums["Accounts Payable"][i] : 0
                    ,
                    ShortCurrentLongTermDebt =
                        DictionaryNums.ContainsKey("Short/Current Long Term Debt")
                            ? DictionaryNums["Short/Current Long Term Debt"][i]
                            : 0
                    ,
                    OtherCurrentLiabilities =
                        DictionaryNums.ContainsKey("Other Current Liabilities") ? DictionaryNums["Other Current Liabilities"][i] : 0
                    ,
                    TotalCurrentLiabilities =
                        DictionaryNums.ContainsKey("Total Current Liabilities") ? DictionaryNums["Total Current Liabilities"][i] : 0
                    ,
                    LongTermDebt = DictionaryNums.ContainsKey("Long Term Debt") ? DictionaryNums["Long Term Debt"][i] : 0
                    ,
                    OtherLiabilities = DictionaryNums.ContainsKey("Other Liabilities") ? DictionaryNums["Other Liabilities"][i] : 0
                    ,
                    DeferredLongTermLiabilityCharges =
                        DictionaryNums.ContainsKey("Deferred Long Term Liability Charges")
                            ? DictionaryNums["Deferred Long Term Liability Charges"][i]
                            : 0
                    ,
                    MinorityInterest = DictionaryNums.ContainsKey("Minority Interest") ? DictionaryNums["Minority Interest"][i] : 0
                    ,
                    NegativeGoodwill = DictionaryNums.ContainsKey("Negative Goodwill") ? DictionaryNums["Negative Goodwill"][i] : 0
                    ,
                    TotalLiabilities = DictionaryNums.ContainsKey("Total Liabilities") ? DictionaryNums["Total Liabilities"][i] : 0
                    ,
                    MiscStocksOptionsWarrants =
                        DictionaryNums.ContainsKey("Misc Stocks Options Warrants")
                            ? DictionaryNums["Misc Stocks Options Warrants"][i]
                            : 0
                    ,
                    RedeemablePreferredStock =
                        DictionaryNums.ContainsKey("Redeemable Preferred Stock")
                            ? DictionaryNums["Redeemable Preferred Stock"][i]
                            : 0
                    ,
                    PreferredStock = DictionaryNums.ContainsKey("Preferred Stock") ? DictionaryNums["Preferred Stock"][i] : 0
                    ,
                    CommonStock = DictionaryNums.ContainsKey("Common Stock") ? DictionaryNums["Common Stock"][i] : 0
                    ,
                    RetainedEarnings = DictionaryNums.ContainsKey("Retained Earnings") ? DictionaryNums["Retained Earnings"][i] : 0
                    ,
                    TreasuryStock = DictionaryNums.ContainsKey("Treasury Stock") ? DictionaryNums["Treasury Stock"][i] : 0
                    ,
                    CapitalSurplus = DictionaryNums.ContainsKey("Capital Surplus") ? DictionaryNums["Capital Surplus"][i] : 0
                    ,
                    OtherStockholderEquity =
                        DictionaryNums.ContainsKey("Other Stockholder Equity") ? DictionaryNums["Other Stockholder Equity"][i] : 0
                    ,
                    TotalStockholderEquity =
                        DictionaryNums.ContainsKey("Total Stockholder Equity") ? DictionaryNums["Total Stockholder Equity"][i] : 0
                    ,
                    NetTangibleAssets =
                        DictionaryNums.ContainsKey("Net Tangible Assets") ? DictionaryNums["Net Tangible Assets"][i] : 0
                });
            }
            return nfCdataOut;

        }
    }
}
