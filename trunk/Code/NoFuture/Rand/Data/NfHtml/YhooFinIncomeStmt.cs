using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Data.NfHtml
{
    public class YhooFinIncomeStmt : YhooFinBase
    {
        public YhooFinIncomeStmt(Uri srcUri) :base(srcUri) { }

        public override List<dynamic> ParseContent(object content)
        {
            var webResponseBody = content as string;
            if (webResponseBody == null)
                return null;

            if (!GetDtsAndDictionary(webResponseBody, "Period Ending", "Net Income Applicable To Common Shares"))
                return null;

            var nfCdataOut = new List<dynamic>();

            for (var i = 0; i < 3; i++)
            {
                nfCdataOut.Add(new
                {
                    FiscalYearEndAt = Dts[i].Year
                    ,
                    TotalRevenue = DictionaryNums.ContainsKey("Total Revenue") ? DictionaryNums["Total Revenue"][i] : 0
                    ,
                    CostofRevenue =
                        DictionaryNums.ContainsKey("Cost of Revenue") ? DictionaryNums["Cost of Revenue"][i] : 0
                    ,
                    GrossProfit = DictionaryNums.ContainsKey("Gross Profit") ? DictionaryNums["Gross Profit"][i] : 0
                    ,
                    ResearchDevelopment =
                        DictionaryNums.ContainsKey("Research Development")
                            ? DictionaryNums["Research Development"][i]
                            : 0
                    ,
                    SellingGeneralandAdministrative =
                        DictionaryNums.ContainsKey("Selling General and Administrative")
                            ? DictionaryNums["Selling General and Administrative"][i]
                            : 0
                    ,
                    NonRecurring = DictionaryNums.ContainsKey("Non Recurring") ? DictionaryNums["Non Recurring"][i] : 0
                    ,
                    Others = DictionaryNums.ContainsKey("Others") ? DictionaryNums["Others"][i] : 0
                    ,
                    TotalOperatingExpenses =
                        DictionaryNums.ContainsKey("Total Operating Expenses")
                            ? DictionaryNums["Total Operating Expenses"][i]
                            : 0
                    ,
                    OperatingIncomeorLoss =
                        DictionaryNums.ContainsKey("Operating Income or Loss")
                            ? DictionaryNums["Operating Income or Loss"][i]
                            : 0
                    ,
                    TotalOtherIncomeExpensesNet =
                        DictionaryNums.ContainsKey("Total Other Income/Expenses Net")
                            ? DictionaryNums["Total Other Income/Expenses Net"][i]
                            : 0
                    ,
                    EarningsBeforeInterestAndTaxes =
                        DictionaryNums.ContainsKey("Earnings Before Interest And Taxes")
                            ? DictionaryNums["Earnings Before Interest And Taxes"][i]
                            : 0
                    ,
                    InterestExpense =
                        DictionaryNums.ContainsKey("Interest Expense") ? DictionaryNums["Interest Expense"][i] : 0
                    ,
                    IncomeBeforeTax =
                        DictionaryNums.ContainsKey("Income Before Tax") ? DictionaryNums["Income Before Tax"][i] : 0
                    ,
                    IncomeTaxExpense =
                        DictionaryNums.ContainsKey("Income Tax Expense") ? DictionaryNums["Income Tax Expense"][i] : 0
                    ,
                    MinorityInterest =
                        DictionaryNums.ContainsKey("Minority Interest") ? DictionaryNums["Minority Interest"][i] : 0
                    ,
                    NetIncomeFromContinuingOps =
                        DictionaryNums.ContainsKey("Net Income From Continuing Ops ")
                            ? DictionaryNums["Net Income From Continuing Ops "][i]
                            : 0
                    ,
                    NonrecurringEvents =
                        DictionaryNums.ContainsKey("Non-recurring Events")
                            ? DictionaryNums["Non-recurring Events"][i]
                            : 0
                    ,
                    DiscontinuedOperations =
                        DictionaryNums.ContainsKey("Discontinued Operations")
                            ? DictionaryNums["Discontinued Operations"][i]
                            : 0
                    ,
                    ExtraordinaryItems =
                        DictionaryNums.ContainsKey("Extraordinary Items") ? DictionaryNums["Extraordinary Items"][i] : 0
                    ,
                    EffectOfAccountingChanges =
                        DictionaryNums.ContainsKey("Effect Of Accounting Changes")
                            ? DictionaryNums["Effect Of Accounting Changes"][i]
                            : 0
                    ,
                    OtherItems = DictionaryNums.ContainsKey("Other Items") ? DictionaryNums["Other Items"][i] : 0
                    ,
                    PreferredStockAndOtherAdjustments =
                        DictionaryNums.ContainsKey("Preferred Stock And Other Adjustments")
                            ? DictionaryNums["Preferred Stock And Other Adjustments"][i]
                            : 0
                    ,
                    NetIncome = DictionaryNums.ContainsKey("Net Income") ? DictionaryNums["Net Income"][i] : 0
                    ,
                    NetIncomeApplicableToCommonShares =
                        DictionaryNums.ContainsKey("Net Income Applicable To Common Shares")
                            ? DictionaryNums["Net Income Applicable To Common Shares"][i]
                            : 0
                });
            }
            return nfCdataOut;
        }
    }
}