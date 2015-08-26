using System;
using System.Collections.Generic;
namespace NoFuture.Rand.Gov.Bea.Parameters.Ita
{
    public class Indicator : NoFuture.Rand.Gov.Bea.BeaParameter
    {
        public override string Description { get; set; }
        public override string Val { get; set; }
        private static List<Indicator> _values;
        public static List<Indicator> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<Indicator>
                           {
                           
                           new Indicator
                           {
                               Val = "BalCapAcct",
                               Description = "Balance on capital account",
                           },
                           new Indicator
                           {
                               Val = "BalCurrAcct",
                               Description = "Balance on current account",
                           },
                           new Indicator
                           {
                               Val = "BalGds",
                               Description = "Balance on goods",
                           },
                           new Indicator
                           {
                               Val = "BalGdsServ",
                               Description = "Balance on goods and services",
                           },
                           new Indicator
                           {
                               Val = "BalPrimInc",
                               Description = "Balance on primary income",
                           },
                           new Indicator
                           {
                               Val = "BalSecInc",
                               Description = "Balance on secondary income (current transfers)",
                           },
                           new Indicator
                           {
                               Val = "BalServ",
                               Description = "Balance on services",
                           },
                           new Indicator
                           {
                               Val = "CapTransPayAndOthDeb",
                               Description = "Capital transfer payments and other debits",
                           },
                           new Indicator
                           {
                               Val = "CapTransRecAndOthCred",
                               Description = "Capital transfer receipts and other credits",
                           },
                           new Indicator
                           {
                               Val = "CompOfEmplPay",
                               Description = "Compensation of employees; payments",
                           },
                           new Indicator
                           {
                               Val = "CompOfEmplRec",
                               Description = "Compensation of employees; receipts",
                           },
                           new Indicator
                           {
                               Val = "CurrAndDepAssets",
                               Description = "Net U.S. acquisition of other investment assets; currency and deposits",
                           },
                           new Indicator
                           {
                               Val = "CurrAndDepAssetsCentralBank",
                               Description = "Net U.S. acquistion of other investment assets; currency and deposits; held by central bank",
                           },
                           new Indicator
                           {
                               Val = "CurrAndDepAssetsDepTaking",
                               Description = "Net U.S. acquistion of other investment assets; currency and deposits; held by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "CurrAndDepAssetsOthFinNonFin",
                               Description = "Net U.S. acquistion of other investment assets; currency and deposits; held by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "CurrAndDepLiabs",
                               Description = "Net U.S. incurrence of other investment liabilities; currency and deposits",
                           },
                           new Indicator
                           {
                               Val = "CurrAndDepLiabsCentralBank",
                               Description = "Net U.S. incurrence of other investment liabilities; currency and deposits; issued by central bank",
                           },
                           new Indicator
                           {
                               Val = "CurrAndDepLiabsDepTaking",
                               Description = "Net U.S. incurrence of other investment liabilities; currency and deposits; issued by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "CurrAndDepLiabsFoa",
                               Description = "Net U.S. incurrence of other investment liabilities to foreign official agencies; currency and deposits",
                           },
                           new Indicator
                           {
                               Val = "CurrAndDepLiabsOthFinNonFin",
                               Description = "Net U.S. incurrence of other investment liabilities; currency and deposits; issued by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "CurrAndDepReserveAssets",
                               Description = "Net U.S. acquisition of reserve assets; other; currency and deposits",
                           },
                           new Indicator
                           {
                               Val = "CurrAssets",
                               Description = "Net U.S. acquisition of other investment assets; currency (short term)",
                           },
                           new Indicator
                           {
                               Val = "CurrLiabs",
                               Description = "Net U.S. incurrence of other investment liabilities; currency (short term)",
                           },
                           new Indicator
                           {
                               Val = "CurrLiabsCentralBank",
                               Description = "Net U.S. incurrence of other investment liabilities; currency (short term); issued by central bank",
                           },
                           new Indicator
                           {
                               Val = "DebtSecAssets",
                               Description = "Net U.S. acquisition of portfolio investment assets; debt securities",
                           },
                           new Indicator
                           {
                               Val = "DebtSecAssetsDepTaking",
                               Description = "Net U.S. acquisition of portfolio investment assets; debt securities; held by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "DebtSecAssetsNonFin",
                               Description = "Net U.S. acquisition of portfolio investment assets; debt securities; held by nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "DebtSecAssetsOthFin",
                               Description = "Net U.S. acquisition of portfolio investment assets; debt securities; held by non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "DebtSecFedSponsorAgencyIncPay",
                               Description = "Portfolio investment income payments; interest on federally sponsored agency securties",
                           },
                           new Indicator
                           {
                               Val = "DebtSecFedSponsorAgencyLiabs",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; federally sponsored agency securities",
                           },
                           new Indicator
                           {
                               Val = "DebtSecIncPay",
                               Description = "Portfolio investment income payments; interest on debt securities",
                           },
                           new Indicator
                           {
                               Val = "DebtSecIncPayDepTaking",
                               Description = "Portfolio investment income payments; interest on debt securities; deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "DebtSecIncPayGenGovt",
                               Description = "Portfolio investment income payments; interest on debt securities; general government",
                           },
                           new Indicator
                           {
                               Val = "DebtSecIncPayNonFin",
                               Description = "Portfolio investment income payments; interest on debt securities; nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "DebtSecIncPayOthFin",
                               Description = "Portfolio investment income payments; interest on debt securities; non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "DebtSecIncRec",
                               Description = "Portfolio investment income receipts; interest on debt securities",
                           },
                           new Indicator
                           {
                               Val = "DebtSecIncRecDepTaking",
                               Description = "Portfolio investment income receipts; interest on debt securities; deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "DebtSecIncRecNonFin",
                               Description = "Portfolio investment income receipts; interest on debt securities; nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "DebtSecIncRecOthFin",
                               Description = "Portfolio investment income receipts; interest on debt securities; non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "DebtSecLiabs",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; debt securities",
                           },
                           new Indicator
                           {
                               Val = "DebtSecLiabsDepTaking",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; debt securities; issued by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "DebtSecLiabsFoa",
                               Description = "Net U.S. incurrence of portfolio investment liabilities to foreign official agencies; debt securities",
                           },
                           new Indicator
                           {
                               Val = "DebtSecLiabsGenGovt",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; debt securities; issued by general government",
                           },
                           new Indicator
                           {
                               Val = "DebtSecLiabsNonFin",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; debt securities; issued by nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "DebtSecLiabsOthFin",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; debt securities; issued by non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "DebtSecOthThanFedSponsorAgencyIncPayOthFin",
                               Description = "Portfolio investment income payments; interest on debt securities other than federally sponsored agency securties; non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "DebtSecOthThanFedSponsorAgencyLiabsOthFin",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; debt securities other than federally sponsored agency securities; issued by non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "DebtSecTreasIncPay",
                               Description = "Portfolio investment income payments; interest on U.S. Treasury securities",
                           },
                           new Indicator
                           {
                               Val = "DebtSecTreasLiabs",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; U.S. Treasury securities",
                           },
                           new Indicator
                           {
                               Val = "DepAssets",
                               Description = "Net U.S. acquisition of other investment assets; deposits",
                           },
                           new Indicator
                           {
                               Val = "DepAssetsCentralBank",
                               Description = "Net U.S. acquistion of other investment assets; deposits; held by central bank",
                           },
                           new Indicator
                           {
                               Val = "DepAssetsDepTaking",
                               Description = "Net U.S. acquistion of other investment assets; deposits; held by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "DepAssetsOthFinNonFin",
                               Description = "Net U.S. acquistion of other investment assets; deposits; held by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "DepLiabs",
                               Description = "Net U.S. incurrence of other investment liabilities; deposits",
                           },
                           new Indicator
                           {
                               Val = "DepLiabsCentralBank",
                               Description = "Net U.S. incurrence of other investment liabilities; deposits; issued by central bank",
                           },
                           new Indicator
                           {
                               Val = "DepLiabsDepTaking",
                               Description = "Net U.S. incurrence of other investment liabilities; deposits; issued by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "DepLiabsOthFinNonFin",
                               Description = "Net U.S. incurrence of other investment liabilities; deposits; issued by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "DepRepurchaseLiabsDepTaking",
                               Description = "Net U.S. incurrence of other investment liabilities; deposits, of which repurchase agreements; issued by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "DepResaleAssetsDepTaking",
                               Description = "Net U.S. acquistion of other investment assets; deposits, of which resale agreements; held by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "DiInvAssets",
                               Description = "Net U.S. acquisition of direct investment assets, asset/liability basis",
                           },
                           new Indicator
                           {
                               Val = "DiInvCurrCostAdjAssets",
                               Description = "Net U.S. acquisition of direct investment assets; current-cost adjustment",
                           },
                           new Indicator
                           {
                               Val = "DiInvCurrCostAdjIncPay",
                               Description = "Direct investment income on liabilities; current-cost adjustment",
                           },
                           new Indicator
                           {
                               Val = "DiInvCurrCostAdjIncRec",
                               Description = "Direct investment income on assets; current-cost adjustment",
                           },
                           new Indicator
                           {
                               Val = "DiInvCurrCostAdjLiabs",
                               Description = "Net U.S. incurrence of direct investment liabilities; current-cost adjustment",
                           },
                           new Indicator
                           {
                               Val = "DiInvDebtInstAssets",
                               Description = "Net U.S. acquisition of direct investment assets, asset/liability basis; debt instruments",
                           },
                           new Indicator
                           {
                               Val = "DiInvDebtInstInward",
                               Description = "Financial transactions for inward direct investment (foreign direct investment in the United States), directional basis; debt instruments",
                           },
                           new Indicator
                           {
                               Val = "DiInvDebtInstInwardByInd",
                               Description = "Financial transactions for inward direct investment (foreign direct investment in the United States), directional basis; debt instruments; by industry",
                           },
                           new Indicator
                           {
                               Val = "DiInvDebtInstInwardFinAndIns",
                               Description = "Financial transactions for inward direct investment (foreign direct investment in the United States), directional basis; debt instruments; finance (including depository institutions) and insurance",
                           },
                           new Indicator
                           {
                               Val = "DiInvDebtInstInwardMnfctr",
                               Description = "Financial transactions for inward direct investment (foreign direct investment in the United States), directional basis; debt instruments; manufacturing",
                           },
                           new Indicator
                           {
                               Val = "DiInvDebtInstInwardOthInd",
                               Description = "Financial transactions for inward direct investment (foreign direct investment in the United States), directional basis; debt instruments; other industries (those not listed under incurrence of liabilities in table 6.1)",
                           },
                           new Indicator
                           {
                               Val = "DiInvDebtInstInwardWhlslTrd",
                               Description = "Financial transactions for inward direct investment (foreign direct investment in the United States), directional basis; debt instruments; wholesale trade",
                           },
                           new Indicator
                           {
                               Val = "DiInvDebtInstLiabs",
                               Description = "Net U.S. incurrence of direct investment liabilities, asset/liability basis; debt instruments",
                           },
                           new Indicator
                           {
                               Val = "DiInvDebtInstOutward",
                               Description = "Financial transactions for outward direct investment (U.S. direct investment abroad), directional basis; debt instruments",
                           },
                           new Indicator
                           {
                               Val = "DiInvDebtInstOutwardByInd",
                               Description = "Financial transactions for outward direct investment (U.S. direct investment abroad), directional basis; debt instruments; by industry",
                           },
                           new Indicator
                           {
                               Val = "DiInvDebtInstOutwardFinAndIns",
                               Description = "Financial transactions for outward direct investment (U.S. direct investment abroad), directional basis; debt instruments; finance (including depository institutions) and insurance",
                           },
                           new Indicator
                           {
                               Val = "DiInvDebtInstOutwardHoldExcBank",
                               Description = "Financial transactions for outward direct investment (U.S. direct investment abroad), directional basis; debt instruments; holding companies except bank holding companies",
                           },
                           new Indicator
                           {
                               Val = "DiInvDebtInstOutwardMnfctr",
                               Description = "Financial transactions for outward direct investment (U.S. direct investment abroad), directional basis; debt instruments; manufacturing",
                           },
                           new Indicator
                           {
                               Val = "DiInvDebtInstOutwardOthInd",
                               Description = "Financial transactions for outward direct investment (U.S. direct investment abroad), directional basis; debt instruments; other industries (those not listed under acquisition of assets in table 6.1)",
                           },
                           new Indicator
                           {
                               Val = "DiInvDebtInstOutwardWhlslTrd",
                               Description = "Financial transactions for outward direct investment (U.S. direct investment abroad), directional basis; debt instruments; wholesale trade",
                           },
                           new Indicator
                           {
                               Val = "DiInvDebtInstUsAffiliatesClaims",
                               Description = "Financial transactions for direct investment; debt instruments; U.S. affiliates' claims",
                           },
                           new Indicator
                           {
                               Val = "DiInvDebtInstUsAffiliatesLiabs",
                               Description = "Financial transactions for direct investment; debt instruments; U.S. affiliates' liabilities",
                           },
                           new Indicator
                           {
                               Val = "DiInvDebtInstUsParentsClaims",
                               Description = "Financial transactions for direct investment; debt instruments; U.S. parents' claims",
                           },
                           new Indicator
                           {
                               Val = "DiInvDebtInstUsParentsLiabs",
                               Description = "Financial transactions for direct investment; debt instruments; U.S. parents' liabilities",
                           },
                           new Indicator
                           {
                               Val = "DiInvDirectionalBasisAdj",
                               Description = "Financial transactions for direct investment; adjustments to convert to directional basis",
                           },
                           new Indicator
                           {
                               Val = "DiInvDirectionalBasisAdjIncPay",
                               Description = "Direct investment income; adjustments to convert to directional basis",
                           },
                           new Indicator
                           {
                               Val = "DiInvDirectionalBasisAdjIncRec",
                               Description = "Direct investment income; adjustments to convert to directional basis",
                           },
                           new Indicator
                           {
                               Val = "DiInvDivWithdrawIncPay",
                               Description = "Direct investment income on liabilities; dividends and withdrawals",
                           },
                           new Indicator
                           {
                               Val = "DiInvDivWithdrawIncRec",
                               Description = "Direct investment income on assets; dividends and withdrawals",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityAssets",
                               Description = "Net U.S. acquisition of direct investment assets; equity",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityIncPay",
                               Description = "Direct investment income on liabilities; equity",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityIncRec",
                               Description = "Direct investment income on assets; equity",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityIntIncRecFinAndIns",
                               Description = "Direct investment income on outward investment (U.S. direct investment abroad), directional basis; interest, net receipts; finance (including depository institutions) and insurance",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityIntIncRecHoldExcBank",
                               Description = "Direct investment income on outward investment (U.S. direct investment abroad), directional basis; interest, net receipts; holding companies except bank holding companies",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityIntIncRecMnfctr",
                               Description = "Direct investment income on outward investment (U.S. direct investment abroad), directional basis; interest, net receipts; manufacturing",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityIntIncRecOthInd",
                               Description = "Direct investment income on outward investment (U.S. direct investment abroad), directional basis; interest, net receipts; manufacturing; other industries (those not listed under receipts in table 4.2)",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityIntIncRecWhlslTrd",
                               Description = "Direct investment income on outward investment (U.S. direct investment abroad), directional basis; interest, net receipts; wholesale trade",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityLiabs",
                               Description = "Net U.S. incurrence of direct investment liabilities; equity",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityOthThanReinvestEarnAssets",
                               Description = "Net U.S. acquisition of direct investment assets; equity other than reinvestment of earnings",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityOthThanReinvestEarnAssetsFinAndIns",
                               Description = "Net U.S. acquisition of direct investment assets; equity other than reinvestment of earnings; finance (including depository institutions) and insurance",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityOthThanReinvestEarnAssetsHoldExcBank",
                               Description = "Net U.S. acquisition of direct investment assets; equity other than reinvestment of earnings; holding companies except bank holding companies",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityOthThanReinvestEarnAssetsMnfctr",
                               Description = "Net U.S. acquisition of direct investment assets; equity other than reinvestment of earnings; manufacturing",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityOthThanReinvestEarnAssetsOthInd",
                               Description = "Net U.S. acquisition of direct investment assets; equity other than reinvestment of earnings; other industries (those not listed under acquisition of assets in table 6.1)",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityOthThanReinvestEarnAssetsWhlslTrd",
                               Description = "Net U.S. acquisition of direct investment assets; equity other than reinvestment of earnings; wholesale trade",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityOthThanReinvestEarnDecAssets",
                               Description = "Net U.S. acquisition of direct investment assets; equity other than reinvestment of earnings; decreases",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityOthThanReinvestEarnDecLiabs",
                               Description = "Net U.S. incurrence of direct investment liabilities; equity other than reinvestment of earnings; decreases",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityOthThanReinvestEarnIncAssets",
                               Description = "Net U.S. acquisition of direct investment assets; equity other than reinvestment of earnings; increases",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityOthThanReinvestEarnIncLiabs",
                               Description = "Net U.S. incurrence of direct investment liabilities; equity other than reinvestment of earnings; increases",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityOthThanReinvestEarnLiabs",
                               Description = "Net U.S. incurrence of direct investment liabilities; equity other than reinvestment of earnings",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityOthThanReinvestEarnLiabsFinAndIns",
                               Description = "Net U.S. incurrence of direct investment liabilities; equity other than reinvestment of earnings; finance (including depository institutions) and insurance",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityOthThanReinvestEarnLiabsHoldExcBank",
                               Description = "Net U.S. incurrence of direct investment liabilities; equity other than reinvestment of earnings; other industries (those not listed under incurrence of liabilities in table 6.1)",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityOthThanReinvestEarnLiabsMnfctr",
                               Description = "Net U.S. incurrence of direct investment liabilities; equity other than reinvestment of earnings; manufacturing",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityOthThanReinvestEarnLiabsWhlslTrd",
                               Description = "Net U.S. incurrence of direct investment liabilities; equity other than reinvestment of earnings; wholesale trade",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityWithoutCurrCostAdjIncPay",
                               Description = "Direct investment income without current cost adjustment on liabilities; equity",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityWithoutCurrCostAdjIncPayFinAndIns",
                               Description = "Direct investment income without current cost adjustment on liabilities; equity; finance (including depository institutions) and insurance",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityWithoutCurrCostAdjIncPayMnfctr",
                               Description = "Direct investment income without current cost adjustment on liabilities; equity; manufacturing",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityWithoutCurrCostAdjIncPayOthInd",
                               Description = "Direct investment income without current cost adjustment on liabilities; equity; other industries (those not listed under receipts in table 4.2)",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityWithoutCurrCostAdjIncPayWhlslTrd",
                               Description = "Direct investment income without current cost adjustment on liabilities; equity; wholesale trade",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityWithoutCurrCostAdjIncRec",
                               Description = "Direct investment income without current cost adjustment on assets; equity",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityWithoutCurrCostAdjIncRecFinAndIns",
                               Description = "Direct investment income without current cost adjustment on assets; equity; finance (including depository institutions) and insurance",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityWithoutCurrCostAdjIncRecHoldExcBank",
                               Description = "Direct investment income without current cost adjustment on assets; equity; holding companies except bank holding companies",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityWithoutCurrCostAdjIncRecMnfctr",
                               Description = "Direct investment income without current cost adjustment on assets; equity; manufacturing",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityWithoutCurrCostAdjIncRecOthInd",
                               Description = "Direct investment income without current cost adjustment on assets; equity; manufacturing; other industries (those not listed under receipts in table 4.2)",
                           },
                           new Indicator
                           {
                               Val = "DiInvEquityWithoutCurrCostAdjIncRecWhlslTrd",
                               Description = "Direct investment income without current cost adjustment on assets; equity; wholesale trade",
                           },
                           new Indicator
                           {
                               Val = "DiInvIncPay",
                               Description = "Direct investment income on liabilities, asset/liability basis",
                           },
                           new Indicator
                           {
                               Val = "DiInvIncRec",
                               Description = "Direct investment income on assets, asset/liability basis",
                           },
                           new Indicator
                           {
                               Val = "DiInvIntIncInward",
                               Description = "Direct investment income on inward investment (foreign direct investment in the United States), directional basis; interest, net payments",
                           },
                           new Indicator
                           {
                               Val = "DiInvIntIncInwardFinAndIns",
                               Description = "Direct investment income on inward investment (foreign direct investment in the United States), directional basis; interest, net payments; finance (including depository institutions) and insurance",
                           },
                           new Indicator
                           {
                               Val = "DiInvIntIncInwardMnfctr",
                               Description = "Direct investment income on inward investment (foreign direct investment in the United States), directional basis; interest, net payments; manufacturing",
                           },
                           new Indicator
                           {
                               Val = "DiInvIntIncInwardOthInd",
                               Description = "Direct investment income on inward investment (foreign direct investment in the United States), directional basis; interest, net payments; other industries (those not listed under receipts in table 4.2)",
                           },
                           new Indicator
                           {
                               Val = "DiInvIntIncInwardWhlslTrd",
                               Description = "Direct investment income on inward investment (foreign direct investment in the United States), directional basis; interest, net payments; wholesale trade",
                           },
                           new Indicator
                           {
                               Val = "DiInvIntIncOutward",
                               Description = "Direct investment income on outward investment (U.S. direct investment abroad), directional basis; interest, net receipts",
                           },
                           new Indicator
                           {
                               Val = "DiInvIntIncPay",
                               Description = "Direct investment income on liabilities, asset/liability basis; interest",
                           },
                           new Indicator
                           {
                               Val = "DiInvIntIncRec",
                               Description = "Direct investment income on assets, asset/liability basis; interest",
                           },
                           new Indicator
                           {
                               Val = "DiInvIntUsAffiliatesIncPay",
                               Description = "Direct investment income; U.S. affiliates' interest payments",
                           },
                           new Indicator
                           {
                               Val = "DiInvIntUsAffiliatesIncRec",
                               Description = "Direct investment income; U.S. affiliates' interest receipts",
                           },
                           new Indicator
                           {
                               Val = "DiInvIntUsParentsIncPay",
                               Description = "Direct investment income; U.S. parents' interest payments",
                           },
                           new Indicator
                           {
                               Val = "DiInvIntUsParentsIncRec",
                               Description = "Direct investment income; U.S. parents' interest receipts",
                           },
                           new Indicator
                           {
                               Val = "DiInvInwardDirectionalBasis",
                               Description = "Financial transactions for inward direct investment (foreign direct investment in the United States), directional basis",
                           },
                           new Indicator
                           {
                               Val = "DiInvLiabs",
                               Description = "Net U.S. incurrence of direct investment liabilities, asset/liability basis",
                           },
                           new Indicator
                           {
                               Val = "DiInvOutward",
                               Description = "Financial transactions for outward direct investment (U.S. direct investment abroad), directional basis",
                           },
                           new Indicator
                           {
                               Val = "DiInvReinvestEarnAssets",
                               Description = "Net U.S. acquisition of direct investment assets; reinvestment of earnings",
                           },
                           new Indicator
                           {
                               Val = "DiInvReinvestEarnIncPay",
                               Description = "Direct investment income on liabilities; reinvested earnings",
                           },
                           new Indicator
                           {
                               Val = "DiInvReinvestEarnIncRec",
                               Description = "Direct investment income on assets; reinvested earnings",
                           },
                           new Indicator
                           {
                               Val = "DiInvReinvestEarnLiabs",
                               Description = "Net U.S. incurrence of direct investment liabilities; reinvestment of earnings",
                           },
                           new Indicator
                           {
                               Val = "DiInvReinvestEarnWithoutCurrCostAdjAssets",
                               Description = "Net U.S. acquisition of direct investment assets; reinvestment of earnings without current-cost adjustment",
                           },
                           new Indicator
                           {
                               Val = "DiInvReinvestEarnWithoutCurrCostAdjAssetsFinAndIns",
                               Description = "Net U.S. acquisition of direct investment assets; reinvestment of earnings without current-cost adjustment; finance (including depository institutions) and insurance",
                           },
                           new Indicator
                           {
                               Val = "DiInvReinvestEarnWithoutCurrCostAdjAssetsHoldExcBank",
                               Description = "Net U.S. acquisition of direct investment assets; reinvestment of earnings without current-cost adjustment; holding companies except bank holding companies",
                           },
                           new Indicator
                           {
                               Val = "DiInvReinvestEarnWithoutCurrCostAdjAssetsMnfctr",
                               Description = "Net U.S. acquisition of direct investment assets; reinvestment of earnings without current-cost adjustment; manufacturing",
                           },
                           new Indicator
                           {
                               Val = "DiInvReinvestEarnWithoutCurrCostAdjAssetsOthInd",
                               Description = "Net U.S. acquisition of direct investment assets; reinvestment of earnings without current-cost adjustment; other industries (those not listed under acquisition of assets in table 6.1)",
                           },
                           new Indicator
                           {
                               Val = "DiInvReinvestEarnWithoutCurrCostAdjAssetsWhlslTrd",
                               Description = "Net U.S. acquisition of direct investment assets; reinvestment of earnings without current-cost adjustment; wholesale trade",
                           },
                           new Indicator
                           {
                               Val = "DiInvReinvestEarnWithoutCurrCostAdjIncPay",
                               Description = "Direct investment income on liabilities; reinvested earnings without current-cost adjustment",
                           },
                           new Indicator
                           {
                               Val = "DiInvReinvestEarnWithoutCurrCostAdjIncRec",
                               Description = "Direct investment income on assets; reinvested earnings without current-cost adjustment",
                           },
                           new Indicator
                           {
                               Val = "DiInvReinvestEarnWithoutCurrCostAdjLiabs",
                               Description = "Net U.S. incurrence of direct investment liabilities; reinvestment of earnings without current-cost adjustment",
                           },
                           new Indicator
                           {
                               Val = "DiInvReinvestEarnWithoutCurrCostAdjLiabsFinAndIns",
                               Description = "Net U.S. incurrence of direct investment liabilities; reinvestment of earnings without current-cost adjustment; finance (including depository institutions) and insurance",
                           },
                           new Indicator
                           {
                               Val = "DiInvReinvestEarnWithoutCurrCostAdjLiabsMnfctr",
                               Description = "Net U.S. incurrence of direct investment liabilities; reinvestment of earnings without current-cost adjustment; manufacturing",
                           },
                           new Indicator
                           {
                               Val = "DiInvReinvestEarnWithoutCurrCostAdjLiabsOthInd",
                               Description = "Net U.S. incurrence of direct investment liabilities; reinvestment of earnings without current-cost adjustment; other industries (those not listed under incurrence of liabilities in table 6.1)",
                           },
                           new Indicator
                           {
                               Val = "DiInvReinvestEarnWithoutCurrCostAdjLiabsWhlslTrd",
                               Description = "Net U.S. incurrence of direct investment liabilities; reinvestment of earnings without current-cost adjustment; wholesale trade",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjIncInward",
                               Description = "Direct investment income without current-cost adjustment on inward investment, directional basis",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjIncInwardFinAndIns",
                               Description = "Direct investment income without current-cost adjustment on inward investment, directional basis; finance (including depository institutions) and insurance",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjIncInwardMnfctr",
                               Description = "Direct investment income without current-cost adjustment on inward investment, directional basis; manufacturing",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjIncInwardOthInd",
                               Description = "Direct investment income without current-cost adjustment on inward investment, directional basis; other industries (those not listed under receipts in table 4.2)",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjIncInwardWhlslTrd",
                               Description = "Direct investment income without current-cost adjustment on inward investment, directional basis; wholesale trade",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjIncOutwardFinAndIns",
                               Description = "Direct investment income without current cost adjustment on outward investment (U.S. direct investment abroad), directional basis; finance (including depository institutions) and insurance",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjIncOutwardHoldExcBank",
                               Description = "Direct investment income without current cost adjustment on outward investment (U.S. direct investment abroad), directional basis; holding companies except bank holding companies",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjIncOutwardMnfctr",
                               Description = "Direct investment income without current cost adjustment on outward investment (U.S. direct investment abroad), directional basis; manufacturing",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjIncOutwardOthInd",
                               Description = "Direct investment income without current cost adjustment on outward investment (U.S. direct investment abroad), directional basis; manufacturing; other industries (those not listed under receipts in table 4.2)",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjIncOutwardWhlslTrd",
                               Description = "Direct investment income without current cost adjustment on outward investment (U.S. direct investment abroad), directional basis; wholesale trade",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjInward",
                               Description = "Financial transactions without current-cost adjustment for inward direct investment (foreign direct investment in the United States), directional basis",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjInwardFinAndIns",
                               Description = "Financial transactions without current-cost adjustment for inward direct investment (foreign direct investment in the United States), directional basis; finance (including depository institutions) and insurance",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjInwardMnfctr",
                               Description = "Financial transactions without current-cost adjustment for inward direct investment (foreign direct investment in the United States), directional basis; manufacturing",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjInwardOthInd",
                               Description = "Financial transactions without current-cost adjustment for inward direct investment (foreign direct investment in the United States), directional basis; other industries (those not listed under incurrence of liabilities in table 6.1)",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjInwardWhlslTrd",
                               Description = "Financial transactions without current-cost adjustment for inward direct investment (foreign direct investment in the United States), directional basis; wholesale trade",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjOutward",
                               Description = "Financial transactions without current-cost adjustment for outward direct investment (U.S. direct investment abroad), directional basis",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjOutwardFinAndIns",
                               Description = "Financial transactions without current-cost adjustment for outward direct investment (U.S. direct investment abroad), directional basis; finance (including depository institutions) and insurance",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjOutwardHoldExcBank",
                               Description = "Financial transactions without current-cost adjustment for outward direct investment (U.S. direct investment abroad), directional basis; holding companies except bank holding companies",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjOutwardMnfctr",
                               Description = "Financial transactions without current-cost adjustment for outward direct investment (U.S. direct investment abroad), directional basis; manufacturing",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjOutwardOthInd",
                               Description = "Financial transactions without current-cost adjustment for outward direct investment (U.S. direct investment abroad), directional basis; other industries (those not listed under acquisition of assets in table 6.1)",
                           },
                           new Indicator
                           {
                               Val = "DiInvWithoutCurrCostAdjOutwardWhlslTrd",
                               Description = "Financial transactions without current-cost adjustment for outward direct investment (U.S. direct investment abroad), directional basis; wholesale trade",
                           },
                           new Indicator
                           {
                               Val = "EquityAndInvFundSharesAssets",
                               Description = "Net U.S. acquisition of portfolio investment assets; equity and investment fund shares",
                           },
                           new Indicator
                           {
                               Val = "EquityAndInvFundSharesAssetsDepTaking",
                               Description = "Net U.S. acquisition of portfolio investment assets; equity and investment fund shares; held by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "EquityAndInvFundSharesAssetsNonFin",
                               Description = "Net U.S. acquisition of portfolio investment assets; equity and investment fund shares; held by nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "EquityAndInvFundSharesAssetsOthFin",
                               Description = "Net U.S. acquisition of portfolio investment assets; equity and investment fund shares; held by non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "EquityAndInvFundSharesIncPay",
                               Description = "Portfolio investment income payments; income on equity and investment fund shares",
                           },
                           new Indicator
                           {
                               Val = "EquityAndInvFundSharesIncPayDepTaking",
                               Description = "Portfolio investment income payments on equity and investment fund shares; deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "EquityAndInvFundSharesIncPayNonFin",
                               Description = "Portfolio investment income payments on equity and investment fund shares; nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "EquityAndInvFundSharesIncPayOthFin",
                               Description = "Portfolio investment income payments on equity and investment fund shares; non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "EquityAndInvFundSharesIncRec",
                               Description = "Portfolio investment income receipts; income on equity and investment fund shares",
                           },
                           new Indicator
                           {
                               Val = "EquityAndInvFundSharesIncRecDepTaking",
                               Description = "Portfolio investment income receipts on equity and investment fund shares; deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "EquityAndInvFundSharesIncRecNonFin",
                               Description = "Portfolio investment income receipts on equity and investment fund shares; nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "EquityAndInvFundSharesIncRecOthFin",
                               Description = "Portfolio investment income receipts on equity and investment fund shares; non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "EquityAndInvFundSharesLiabs",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; equity and investment fund shares",
                           },
                           new Indicator
                           {
                               Val = "EquityAndInvFundSharesLiabsDepTaking",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; equity and investment fund shares; issued by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "EquityAndInvFundSharesLiabsFoa",
                               Description = "Net U.S. incurrence of portfolio investment liabilities to foreign official agencies; equity and investment fund shares",
                           },
                           new Indicator
                           {
                               Val = "EquityAndInvFundSharesLiabsNonFin",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; equity and investment fund shares; issued by nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "EquityAndInvFundSharesLiabsOthFin",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; equity and investment fund shares; issued by non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "EquityOthThanInvFundSharesAssets",
                               Description = "Net U.S. acquisition of portfolio investment assets; equity other than investment fund shares",
                           },
                           new Indicator
                           {
                               Val = "EquityOthThanInvFundSharesIncPay",
                               Description = "Portfolio investment income payments; dividends on equity other than investment fund shares",
                           },
                           new Indicator
                           {
                               Val = "EquityOthThanInvFundSharesIncRec",
                               Description = "Portfolio investment income receipts; dividends on equity other than investment fund shares",
                           },
                           new Indicator
                           {
                               Val = "EquityOthThanInvFundSharesLiabs",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; equity other than investment fund shares",
                           },
                           new Indicator
                           {
                               Val = "EquityOthThanInvFundSharesLiabsFoa",
                               Description = "Net U.S. incurrence of portfolio investment liabilities to foreign official agencies; equity excluding investment fund shares",
                           },
                           new Indicator
                           {
                               Val = "ExpGds",
                               Description = "Exports of goods",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsAgFoodsFeedsAndBevs",
                               Description = "Exports of agricultural foods, feeds, and beverages",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsAgIsm",
                               Description = "Exports of agricultural industrial supplies and materials",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsAppFootAndHouse",
                               Description = "Exports of apparel, footwear, and household goods",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsAutoEngAndEngParts",
                               Description = "Exports of automotive engines and engine parts",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsAutoVehPartsAndEngines",
                               Description = "Exports of automotive vehicles, parts, and engines",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsBauxAndAlum",
                               Description = "Exports of bauxite and aluminum",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsBopAdj",
                               Description = "Exports of goods; balance of payments adjustments, net",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsBuildMatsExcMetals",
                               Description = "Exports of building materials except metals",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsCapGoodsExclAuto",
                               Description = "Exports of capital goods except automotive",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsCensus",
                               Description = "Exports of goods, Census basis",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsChemsExcMeds",
                               Description = "Exports of chemicals except medicinals",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsCivAir",
                               Description = "Exports of civilian aircraft, complete, all types",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsCivAirEngAndParts",
                               Description = "Exports of civilian aircraft, engines, and parts",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsCoalAndRelProds",
                               Description = "Exports of coal and related products",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsComp",
                               Description = "Exports of computers",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsCompAccPeriAndParts",
                               Description = "Exports of computer accessories, peripherals, and parts",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsConsGoodsExcFoodAndAuto",
                               Description = "Exports of consumer goods except food and automotive",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsCopper",
                               Description = "Exports of copper",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsCorn",
                               Description = "Exports of corn",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsCrudePet",
                               Description = "Exports of crude petroleum",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsDistBevAndOthNonAgFoodsFeedsAndBevs",
                               Description = "Exports of distilled beverages and other nonagricultural foods, feeds, and beverages",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsDurCons",
                               Description = "Exports of durable consumer goods",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsElecGenMachElecAppAndParts",
                               Description = "Exports of electric-generating machinery, electric apparatus, and parts",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsEnergyProd",
                               Description = "Exports of energy products",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsEngAndPartsForCivAir",
                               Description = "Exports of engines and parts for civilian aircraft",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsFertPestAndInsect",
                               Description = "Exports of fertilizers, pesticides, and insecticides",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsFishShellfish",
                               Description = "Exports of fish and shellfish",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsFoodsFeedsAndBevs",
                               Description = "Exports of foods, feeds, and beverages",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsFuelOil",
                               Description = "Exports of fuel oil",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsGdsProcPortsBopAdj",
                               Description = "Exports of goods; balance of payments adjustments, net; goods procured in U.S. ports by foreign carriers",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsGemDiamAndOthGem",
                               Description = "Exports of gem diamonds and other gemstones",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsGenMerch",
                               Description = "Exports of general merchandise",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsGrainsPreps",
                               Description = "Exports of grains and preparations",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsHidesSkins",
                               Description = "Exports of hides and skins, including furskins",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsHouseAndKitchApp",
                               Description = "Exports of household and kitchen appliances",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsHouseFurnAndRelProds",
                               Description = "Exports of household furnishings and related products",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsHouseKitchAppAndOthHouse",
                               Description = "Exports of household and kitchen appliances and other household goods",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsIndEngPumpsComps",
                               Description = "Exports of industrial engines, pumps, and compressors",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsIndInorgChems",
                               Description = "Exports of industrial inorganic chemicals",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsIndOrgChems",
                               Description = "Exports of industrial organic chemicals",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsIronAndSteelProds",
                               Description = "Exports of iron and steel products",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsIsm",
                               Description = "Exports of industrial supplies and materials",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsJewelryAndCollect",
                               Description = "Exports of jewelry and collectibles",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsLiqPetGases",
                               Description = "Exports of liquified petroleum gases",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsMachAndEquipExcCons",
                               Description = "Exports of machinery and equipment except consumer-type",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsMachToolsMetalworkMach",
                               Description = "Exports of machine tools and metalworking machinery",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsMeasTestControlInst",
                               Description = "Exports of measuring, testing, and control instruments",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsMeatProdsPoultry",
                               Description = "Exports of meat products and poultry",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsMedDentAndPharm",
                               Description = "Exports of medicinal, dental, and pharmaceutical products",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsMerchantingBopAdj",
                               Description = "Exports of goods; balance of payments adjustments, net; net exports of goods under merchanting",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsMerchantingNet",
                               Description = "Net exports of goods under merchanting",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsMetalsAndNonmetProds",
                               Description = "Exports of metals and nonmetallic products",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsNaturalGas",
                               Description = "Exports of natural gas",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsNonAgFoodsFeedsAndBevs",
                               Description = "Exports of nonagricultural foods, feeds, and beverages",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsNonAgIsm",
                               Description = "Exports of nonagricultural industrial supplies and materials",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsNondurCons",
                               Description = "Exports of nondurable consumer goods",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsNonferrousMetals",
                               Description = "Exports of nonferrous metals",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsNonmonetaryGold",
                               Description = "Exports of nonmonetary gold",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsNonmonGoldBopAdj",
                               Description = "Exports of goods; balance of payments adjustments, net; nonmonetary gold",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsNuclearFuelAndElecEnergy",
                               Description = "Exports of nuclear fuel and electric energy",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsOilDrillMiningConstMach",
                               Description = "Exports of oil-drilling, mining, and construction machinery",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsOthAgFoodsFeedsAndBevs",
                               Description = "Exports of other agricultural foods, feeds, and beverages",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsOthAgIsm",
                               Description = "Exports of other agricultural industrial supplies",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsOthAutoPartsAndAcc",
                               Description = "Exports of other automotive parts and accessories",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsOthBopAdj",
                               Description = "Exports of goods; balance of payments adjustments, net; other adjustments, net",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsOthChems",
                               Description = "Exports of other chemicals",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsOthDurCons",
                               Description = "Exports of other durable consumer goods",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsOthFeeds",
                               Description = "Exports of other feeds",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsOthGenMerch",
                               Description = "Exports of other general merchandise",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsOthHouseIncCellPhones",
                               Description = "Exports of other household goods, including cell phones",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsOthIndMach",
                               Description = "Exports of other industrial machinery",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsOthMetalsAndNonmetProds",
                               Description = "Exports of other metals and nonmetallic products",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsOthNondurCons",
                               Description = "Exports of other nondurable consumer goods",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsOthNonferrousMetals",
                               Description = "Exports of other nonferrous metals",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsOthNonmetals",
                               Description = "Exports of other nonmetals",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsOthOfficeAndBusMach",
                               Description = "Exports of other office and business machines",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsOthPetProds",
                               Description = "Exports of other petroleum products",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsOthServIndAndAgMach",
                               Description = "Exports of other service-industry and agricultural machinery",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsOthTransEquip",
                               Description = "Exports of other transportation equipment",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsPaperAndPaperBaseStocks",
                               Description = "Exports of paper and paper-base stocks",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsPassCars",
                               Description = "Exports of passenger cars, new and used",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsPetAndProds",
                               Description = "Exports of petroleum and products",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsPlasticMaterials",
                               Description = "Exports of plastic materials",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsPrecMetalsExcNonmonGold",
                               Description = "Exports of precious metals except nonmonetary gold",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsPrivGiftParcelRemitBopAdj",
                               Description = "Exports of goods; balance of payments adjustments, net; private gift parcel remittances",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsRadioAndStereoEquip",
                               Description = "Exports of radio and stereo equipment, including recorded media",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsRawCotton",
                               Description = "Exports of raw cotton",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsRepairEquipBopAdj",
                               Description = "Exports of goods; balance of payments adjustments, net; repair of equipment",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsRiceOthFoodGrains",
                               Description = "Exports of rice and other food grains",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsSciHospAndMedEquipAndParts",
                               Description = "Exports of scientific, hospital, and medical equipment and parts",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsSemiconductors",
                               Description = "Exports of semiconductors",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsServ",
                               Description = "Exports of goods and services",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsServIncRec",
                               Description = "Exports of goods and services and income receipts (credits)",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsSoybeans",
                               Description = "Exports of soybeans",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsSteelmakingMats",
                               Description = "Exports of steelmaking materials",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsTelecomEquip",
                               Description = "Exports of telecommunications equipment",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsTextileSupAndRelMats",
                               Description = "Exports of textile supplies and related materials",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsToilAndCosmet",
                               Description = "Exports of toiletries and cosmetics",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsToysAndSport",
                               Description = "Exports of toys and sporting goods, including bicycles",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsTrucksBusesSpecPurpVeh",
                               Description = "Exports of trucks, buses, and special purpose vehicles",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsTvsVidRecAndOthVidEquip",
                               Description = "Exports of televisions, video receivers, and other video equipment",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsUnmanufTobacco",
                               Description = "Exports of unmanufactured tobacco",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsUsMilAgencyBopAdj",
                               Description = "Exports of goods; balance of payments adjustments, net; exports under U.S. military agency sales contracts",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsVegFruitNutPreps",
                               Description = "Exports of vegetables, fruits, nuts, and preparations",
                           },
                           new Indicator
                           {
                               Val = "ExpGdsWheat",
                               Description = "Exports of wheat",
                           },
                           new Indicator
                           {
                               Val = "ExpGenMerch",
                               Description = "Exports of general merchandise",
                           },
                           new Indicator
                           {
                               Val = "ExpServ",
                               Description = "Exports of services",
                           },
                           new Indicator
                           {
                               Val = "ExpServChargesForTheUseOfIpNie",
                               Description = "Charges for the use of intellectual property n.i.e.; exports",
                           },
                           new Indicator
                           {
                               Val = "ExpServCipAudVisRelated",
                               Description = "Charges for the use of intellectual property n.i.e.; exports; audio-visual and related products",
                           },
                           new Indicator
                           {
                               Val = "ExpServCipCompSoft",
                               Description = "Charges for the use of intellectual property n.i.e.; exports; computer software",
                           },
                           new Indicator
                           {
                               Val = "ExpServCipIndProcess",
                               Description = "Charges for the use of intellectual property n.i.e.; exports; industrial processes",
                           },
                           new Indicator
                           {
                               Val = "ExpServCipOth",
                               Description = "Charges for the use of intellectual property n.i.e.; exports; other intellectual property",
                           },
                           new Indicator
                           {
                               Val = "ExpServCipTrademarkFranchiseFees",
                               Description = "Charges for the use of intellectual property n.i.e.; exports; trademarks and franchise fees",
                           },
                           new Indicator
                           {
                               Val = "ExpServComp",
                               Description = "Exports of computer services",
                           },
                           new Indicator
                           {
                               Val = "ExpServFinancial",
                               Description = "Exports of financial services",
                           },
                           new Indicator
                           {
                               Val = "ExpServFinCredCardOthCredRelated",
                               Description = "Exports of credit card and other credit-related services",
                           },
                           new Indicator
                           {
                               Val = "ExpServFinFinManFinAdvCust",
                               Description = "Exports of financial management, financial advisory, and custody services",
                           },
                           new Indicator
                           {
                               Val = "ExpServFinSecBrokUwRelated",
                               Description = "Exports of securities brokerage, underwriting, and related services",
                           },
                           new Indicator
                           {
                               Val = "ExpServFinSecLendEftOth",
                               Description = "Exports of securities lending, electronic funds transfer, and other services",
                           },
                           new Indicator
                           {
                               Val = "ExpServGovtGoodsAndServicesNie",
                               Description = "Exports of government goods and services n.i.e.",
                           },
                           new Indicator
                           {
                               Val = "ExpServInfo",
                               Description = "Exports of information services",
                           },
                           new Indicator
                           {
                               Val = "ExpServInsurance",
                               Description = "Exports of insurance services",
                           },
                           new Indicator
                           {
                               Val = "ExpServInsuranceAuxIns",
                               Description = "Exports of auxiliary insurance services",
                           },
                           new Indicator
                           {
                               Val = "ExpServInsuranceDirect",
                               Description = "Exports of direct insurance services",
                           },
                           new Indicator
                           {
                               Val = "ExpServInsuranceReIns",
                               Description = "Exports of reinsurance services",
                           },
                           new Indicator
                           {
                               Val = "ExpServMaintenanceAndRepairNie",
                               Description = "Exports of maintenance and repair services n.i.e.",
                           },
                           new Indicator
                           {
                               Val = "ExpServOtherBusiness",
                               Description = "Exports of other business services",
                           },
                           new Indicator
                           {
                               Val = "ExpServProfMgmtConsult",
                               Description = "Exports of professional and management consulting services",
                           },
                           new Indicator
                           {
                               Val = "ExpServResearchAndDev",
                               Description = "Exports of research and development services",
                           },
                           new Indicator
                           {
                               Val = "ExpServTechTradeRelatedOth",
                               Description = "Exports of technical, trade-related, and other business services",
                           },
                           new Indicator
                           {
                               Val = "ExpServTelecom",
                               Description = "Exports of telecommunications services",
                           },
                           new Indicator
                           {
                               Val = "ExpServTelecomCompAndInfo",
                               Description = "Exports of telecommunications, computer, and information services",
                           },
                           new Indicator
                           {
                               Val = "ExpServTransport",
                               Description = "Exports of transport services",
                           },
                           new Indicator
                           {
                               Val = "ExpServTransportAir",
                               Description = "Exports of air transport services",
                           },
                           new Indicator
                           {
                               Val = "ExpServTransportAirFreight",
                               Description = "Exports of air freight services",
                           },
                           new Indicator
                           {
                               Val = "ExpServTransportAirPass",
                               Description = "Exports of air passenger services",
                           },
                           new Indicator
                           {
                               Val = "ExpServTransportAirPort",
                               Description = "Exports of air port services",
                           },
                           new Indicator
                           {
                               Val = "ExpServTransportOth",
                               Description = "Exports of transport services; other modes of transport",
                           },
                           new Indicator
                           {
                               Val = "ExpServTransportSea",
                               Description = "Exports of sea transport services",
                           },
                           new Indicator
                           {
                               Val = "ExpServTransportSeaFreight",
                               Description = "Exports of sea freight services",
                           },
                           new Indicator
                           {
                               Val = "ExpServTransportSeaPort",
                               Description = "Exports of sea port services",
                           },
                           new Indicator
                           {
                               Val = "ExpServTravel",
                               Description = "Exports of travel services (for all purposes including education)",
                           },
                           new Indicator
                           {
                               Val = "ExpServTravelBusiness",
                               Description = "Exports of business travel services",
                           },
                           new Indicator
                           {
                               Val = "ExpServTravelBusinessOth",
                               Description = "Exports of other business travel services",
                           },
                           new Indicator
                           {
                               Val = "ExpServTravelEducation",
                               Description = "Exports of education-related services",
                           },
                           new Indicator
                           {
                               Val = "ExpServTravelHealth",
                               Description = "Exports of health-related services",
                           },
                           new Indicator
                           {
                               Val = "ExpServTravelPersonal",
                               Description = "Exports of personal travel services",
                           },
                           new Indicator
                           {
                               Val = "ExpServTravelPersonalOth",
                               Description = "Exports of other personal travel services",
                           },
                           new Indicator
                           {
                               Val = "ExpServTravelShortTermWork",
                               Description = "Expenditures in the U.S. by border, seasonal, and other short-term workers",
                           },
                           new Indicator
                           {
                               Val = "FinAssetsExclFinDeriv",
                               Description = "Net U.S. acquisition of financial assets excluding financial derivatives",
                           },
                           new Indicator
                           {
                               Val = "FinDeriv",
                               Description = "Financial derivatives other than reserves, net transactions",
                           },
                           new Indicator
                           {
                               Val = "FinDerivReserveAssets",
                               Description = "Net U.S. acquisition of reserve assets; other; financial derivatives",
                           },
                           new Indicator
                           {
                               Val = "FinLiabsExclFinDeriv",
                               Description = "Net U.S. incurrence of liabilities excluding financial derivatives",
                           },
                           new Indicator
                           {
                               Val = "FinLiabsFoa",
                               Description = "Net U.S. incurrence of liabilities to foreign official agencies",
                           },
                           new Indicator
                           {
                               Val = "GoldReserveAssets",
                               Description = "Net U.S. acquisition of reserve assets; monetary gold",
                           },
                           new Indicator
                           {
                               Val = "ImfReserveAssets",
                               Description = "Net U.S. acquisition of reserve assets; reserve position in the International Monetary Fund",
                           },
                           new Indicator
                           {
                               Val = "ImpGds",
                               Description = "Imports of goods",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsAgFoodsFeedsAndBevs",
                               Description = "Imports of agricultural foods, feeds, and beverages",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsAgIsm",
                               Description = "Imports of agricultural industrial supplies and materials",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsAppFootAndHouse",
                               Description = "Imports of apparel, footwear, and household goods",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsAutoEngAndEngParts",
                               Description = "Imports of automotive engines and engine parts",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsAutoVehPartsAndEngines",
                               Description = "Imports of automotive vehicles, parts, and engines",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsBauxAndAlum",
                               Description = "Imports of bauxite and aluminum",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsBopAdj",
                               Description = "Imports of goods; balance of payments adjustments, net",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsBuildMatsExcMetals",
                               Description = "Imports of building materials except metals",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsCapGoodsExclAuto",
                               Description = "Imports of capital goods except automotive",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsCensus",
                               Description = "Imports of goods, Census basis",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsChemsExcMeds",
                               Description = "Imports of chemicals except medicinals",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsCivAir",
                               Description = "Imports of civilian aircraft, complete, all types",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsCivAirEngAndParts",
                               Description = "Imports of civilian aircraft, complete, all types",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsCoalAndRelProds",
                               Description = "Imports of coal and related products",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsCocoaAndSugar",
                               Description = "Imports of cocoa beans and sugar",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsComp",
                               Description = "Imports of computers",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsCompAccPeriAndParts",
                               Description = "Imports of computer accessories, peripherals, and parts",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsConsGoodsExcFoodAndAuto",
                               Description = "Imports of consumer goods except food and automotive",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsCrudePet",
                               Description = "Imports of crude petroleum",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsDistBevAndOthNonAgFoodsFeedsAndBevs",
                               Description = "Imports of distilled beverages and other nonagricultural foods, feeds, and beverages",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsDurCons",
                               Description = "Imports of durable consumer goods",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsElecGenMachElecAppAndParts",
                               Description = "Imports of electric-generating machinery, electric apparatus and parts",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsEnergyProds",
                               Description = "Imports of energy products",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsEngAndPartsForCivAir",
                               Description = "Imports of engines and parts for civilian aircraft",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsFertPestAndInsect",
                               Description = "Imports of fertilizers, pesticides, and insecticides",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsFishShellfish",
                               Description = "Imports of fish and shellfish",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsFoodsFeedsAndBevs",
                               Description = "Imports of foods, feeds, and beverages",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsFuelOil",
                               Description = "Imports of fuel oil",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsGdsProcPortsBopAdj",
                               Description = "Imports of goods; balance of payments adjustments, net; goods procured in foreign ports by U.S. carriers",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsGemDiamAndOthGem",
                               Description = "Imports of gem diamonds and other gemstones",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsGenMerch",
                               Description = "Imports of general merchandise",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsGreenCoffee",
                               Description = "Imports of green coffee",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsHouseAndKitchApp",
                               Description = "Imports of household and kitchen appliances",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsHouseFurnAndRelProds",
                               Description = "Imports of household furnishings and related products",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsHouseKitchAppAndOthHouse",
                               Description = "Imports of household and kitchen appliances and other household goods",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsIndEngPumpsComps",
                               Description = "Imports of industrial engines, pumps, and compressors",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsIndInorgChems",
                               Description = "Imports of industrial inorganic chemicals",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsIndOrgChems",
                               Description = "Imports of industrial organic chemicals",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsInlandFreightCanMexBopAdj",
                               Description = "Imports of goods; balance of payments adjustments, net; inland freight in Canada and Mexico",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsIronAndSteelProds",
                               Description = "Imports of iron and steel products",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsIsm",
                               Description = "Imports of industrial supplies and materials",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsJewelryAndCollect",
                               Description = "Imports of jewelry and collectibles",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsLiqPetGases",
                               Description = "Imports of liquified petroleum gases",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsLocoRailBopAdj",
                               Description = "Imports of goods; balance of payments adjustments, net; locomotives and railcars",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsMachAndEquipExcCons",
                               Description = "Imports of machinery and equipment except consumer-type",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsMachToolsMetalworkMach",
                               Description = "Imports of machine tools and metalworking machinery",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsMeasTestControlInst",
                               Description = "Imports of measuring, testing, and control instruments",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsMeatProdsPoultry",
                               Description = "Imports of meat products and poultry",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsMedDentAndPharm",
                               Description = "Imports of medicinal, dental, and pharmaceutical products",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsMetalsAndNonmetProds",
                               Description = "Imports of metals and nonmetallic products",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsNaturalGas",
                               Description = "Imports of natural gas",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsNonAgFoodsFeedsAndBevs",
                               Description = "Imports of nonagricultural foods, feeds, and beverages",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsNonAgIsm",
                               Description = "Imports of nonagricultural industrial supplies and materials",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsNondurCons",
                               Description = "Imports of nondurable consumer goods",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsNonferrousMetals",
                               Description = "Imports of nonferrous metals",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsNonmonetaryGold",
                               Description = "Imports of nonmonetary gold",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsNonmonGoldBopAdj",
                               Description = "Imports of goods; balance of payments adjustments, net; nonmonetary gold",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsNuclearFuelAndElecEnergy",
                               Description = "Imports of nuclear fuel and electric energy",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsOilDrillMiningConstMach",
                               Description = "Imports of oil-drilling, mining, and construction machinery",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsOthAgFoodsFeedsAndBevs",
                               Description = "Imports of other agricultural foods, feeds, and beverages",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsOthAutoPartsAndAcc",
                               Description = "Imports of other automotive parts and accessories",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsOthBopAdj",
                               Description = "Imports of goods; balance of payments adjustments, net; other adjustments, net",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsOthChems",
                               Description = "Imports of other chemicals",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsOthDurCons",
                               Description = "Imports of other durable consumer goods",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsOthGenMerch",
                               Description = "Imports of other general merchandise",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsOthHouseIncCellPhones",
                               Description = "Imports of other household goods, including cell phones",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsOthIndMach",
                               Description = "Imports of other industrial machinery",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsOthMetalsAndNonmetProds",
                               Description = "Imports of other metals and nonmetallic products",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsOthNondurCons",
                               Description = "Imports of other nondurable consumer goods",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsOthNonferrousMetals",
                               Description = "Imports of other nonferrous metals",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsOthNonmetals",
                               Description = "Imports of other nonmetals",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsOthOfficeAndBusMach",
                               Description = "Imports of other office and business machines",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsOthPetProds",
                               Description = "Imports of other petroleum products",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsOthServIndAndAgMach",
                               Description = "Imports of other service-industry and agricultural machinery",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsOthTransEquip",
                               Description = "Imports of other transportation equipment",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsPaperAndPaperBaseStocks",
                               Description = "Imports of paper and paper-base stocks",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsPassCars",
                               Description = "Imports of passenger cars, new and used",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsPetAndProds",
                               Description = "Imports of petroleum and products",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsPlasticMaterials",
                               Description = "Imports of plastic materials",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsPrecMetalsExcNonmonGold",
                               Description = "Imports of precious metals except nonmonetary gold",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsRadioAndStereoEquip",
                               Description = "Imports of radio and stereo equipment, including recorded media",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsRepairEquipBopAdj",
                               Description = "Imports of goods; balance of payments adjustments, net; repair of equipment",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsSciHospAndMedEquipAndParts",
                               Description = "Imports of scientific, hospital, and medical equipment and parts",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsSemiconductors",
                               Description = "Imports of semiconductors",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsServ",
                               Description = "Imports of goods and services",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsServIncPay",
                               Description = "Imports of goods and services and income payments (debits)",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsSoftRevalBopAdj",
                               Description = "Imports of goods; balance of payments adjustments, net; software revaluation",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsSteelmakingMats",
                               Description = "Imports of steelmaking materials",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsTelecomEquip",
                               Description = "Imports of telecommunications equipment",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsTextileSupAndRelMats",
                               Description = "Imports of textile supplies and related materials",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsToilAndCosmet",
                               Description = "Imports of toiletries and cosmetics",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsToysAndSport",
                               Description = "Imports of toys and sporting goods, including bicycles",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsTrucksBusesSpecPurpVeh",
                               Description = "Imports of trucks, buses, and special purpose vehicles",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsTvsVidRecAndOthVidEquip",
                               Description = "Imports of televisions, video receivers, and other video equipment",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsUsMilAgencyBopAdj",
                               Description = "Imports of goods; balance of payments adjustments, net; imports by U.S. military agencies",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsVegFruitNutPreps",
                               Description = "Imports of vegetables, fruits, nuts, and preparations",
                           },
                           new Indicator
                           {
                               Val = "ImpGdsWineBeerRelProds",
                               Description = "Imports of wine, beer, and related products",
                           },
                           new Indicator
                           {
                               Val = "ImpServ",
                               Description = "Imports of services",
                           },
                           new Indicator
                           {
                               Val = "ImpServChargesForTheUseOfIpNie",
                               Description = "Charges for the use of intellectual property n.i.e.; imports",
                           },
                           new Indicator
                           {
                               Val = "ImpServCipAudVisRelated",
                               Description = "Charges for the use of intellectual property n.i.e.; imports; audio-visual and related products",
                           },
                           new Indicator
                           {
                               Val = "ImpServCipCompSoft",
                               Description = "Charges for the use of intellectual property n.i.e.; imports; computer software",
                           },
                           new Indicator
                           {
                               Val = "ImpServCipIndProcess",
                               Description = "Charges for the use of intellectual property n.i.e.; imports; industrial processes",
                           },
                           new Indicator
                           {
                               Val = "ImpServCipOth",
                               Description = "Charges for the use of intellectual property n.i.e.; imports; other intellectual property",
                           },
                           new Indicator
                           {
                               Val = "ImpServCipTrademarkFranchiseFees",
                               Description = "Charges for the use of intellectual property n.i.e.; imports; trademarks and franchise fees",
                           },
                           new Indicator
                           {
                               Val = "ImpServComp",
                               Description = "Imports of computer services",
                           },
                           new Indicator
                           {
                               Val = "ImpServFinancial",
                               Description = "Imports of financial services",
                           },
                           new Indicator
                           {
                               Val = "ImpServFinCredCardOthCredRelated",
                               Description = "Imports of credit card and other credit-related services",
                           },
                           new Indicator
                           {
                               Val = "ImpServFinFinManFinAdvCust",
                               Description = "Imports of financial management, financial advisory, and custody services",
                           },
                           new Indicator
                           {
                               Val = "ImpServFinSecBrokUwRelated",
                               Description = "Imports of securities brokerage, underwriting, and related services",
                           },
                           new Indicator
                           {
                               Val = "ImpServFinSecLendEftOth",
                               Description = "Imports of securities lending, electronic funds transfer, and other services",
                           },
                           new Indicator
                           {
                               Val = "ImpServGovtGoodsAndServicesNie",
                               Description = "Imports of government goods and services n.i.e.",
                           },
                           new Indicator
                           {
                               Val = "ImpServInfo",
                               Description = "Imports of information services",
                           },
                           new Indicator
                           {
                               Val = "ImpServInsurance",
                               Description = "Imports of insurance services",
                           },
                           new Indicator
                           {
                               Val = "ImpServInsuranceAuxIns",
                               Description = "Imports of auxiliary insurance services",
                           },
                           new Indicator
                           {
                               Val = "ImpServInsuranceDirect",
                               Description = "Imports of direct insurance services",
                           },
                           new Indicator
                           {
                               Val = "ImpServInsuranceReIns",
                               Description = "Imports of reinsurance services",
                           },
                           new Indicator
                           {
                               Val = "ImpServMaintenanceAndRepairNie",
                               Description = "Imports of maintenance and repair services n.i.e.",
                           },
                           new Indicator
                           {
                               Val = "ImpServOtherBusiness",
                               Description = "Imports of other business services",
                           },
                           new Indicator
                           {
                               Val = "ImpServProfMgmtConsult",
                               Description = "Imports of professional and management consulting services",
                           },
                           new Indicator
                           {
                               Val = "ImpServResearchAndDev",
                               Description = "Imports of research and development services",
                           },
                           new Indicator
                           {
                               Val = "ImpServTechTradeRelatedOth",
                               Description = "Imports of technical, trade-related, and other business services",
                           },
                           new Indicator
                           {
                               Val = "ImpServTelecom",
                               Description = "Imports of telecommunications services",
                           },
                           new Indicator
                           {
                               Val = "ImpServTelecomCompAndInfo",
                               Description = "Imports of telecommunications, computer, and information services",
                           },
                           new Indicator
                           {
                               Val = "ImpServTransport",
                               Description = "Imports of transport services",
                           },
                           new Indicator
                           {
                               Val = "ImpServTransportAir",
                               Description = "Imports of air transport services",
                           },
                           new Indicator
                           {
                               Val = "ImpServTransportAirFreight",
                               Description = "Imports of air freight services",
                           },
                           new Indicator
                           {
                               Val = "ImpServTransportAirPass",
                               Description = "Imports of air passenger services",
                           },
                           new Indicator
                           {
                               Val = "ImpServTransportAirPort",
                               Description = "Imports of air port services",
                           },
                           new Indicator
                           {
                               Val = "ImpServTransportOth",
                               Description = "Imports of transport services; other modes of transport",
                           },
                           new Indicator
                           {
                               Val = "ImpServTransportSea",
                               Description = "Imports of sea transport services",
                           },
                           new Indicator
                           {
                               Val = "ImpServTransportSeaFreight",
                               Description = "Imports of sea freight services",
                           },
                           new Indicator
                           {
                               Val = "ImpServTransportSeaPort",
                               Description = "Imports of sea port services",
                           },
                           new Indicator
                           {
                               Val = "ImpServTravel",
                               Description = "Imports of travel services (for all purposes including education)",
                           },
                           new Indicator
                           {
                               Val = "ImpServTravelBusiness",
                               Description = "Imports of business travel services",
                           },
                           new Indicator
                           {
                               Val = "ImpServTravelBusinessOth",
                               Description = "Imports of other business travel services",
                           },
                           new Indicator
                           {
                               Val = "ImpServTravelEducation",
                               Description = "Imports of education-related services",
                           },
                           new Indicator
                           {
                               Val = "ImpServTravelHealth",
                               Description = "Imports of health-related services",
                           },
                           new Indicator
                           {
                               Val = "ImpServTravelPersonal",
                               Description = "Imports of personal travel services",
                           },
                           new Indicator
                           {
                               Val = "ImpServTravelPersonalOth",
                               Description = "Imports of other personal travel services",
                           },
                           new Indicator
                           {
                               Val = "ImpServTravelShortTermWork",
                               Description = "Expenditures abroad by border, seasonal, and other short-term workers",
                           },
                           new Indicator
                           {
                               Val = "InsLossesPaid",
                               Description = "Insurance losses paid",
                           },
                           new Indicator
                           {
                               Val = "InsLossesRecovered",
                               Description = "Insurance losses recovered",
                           },
                           new Indicator
                           {
                               Val = "InsPremiumsPaid",
                               Description = "Insurance premiums paid",
                           },
                           new Indicator
                           {
                               Val = "InsPremiumsReceived",
                               Description = "Insurance premiums received",
                           },
                           new Indicator
                           {
                               Val = "InsTechReservesAssets",
                               Description = "Net U.S. acquisition of other investment assets; insurance technical reserves",
                           },
                           new Indicator
                           {
                               Val = "InsTechReservesAssetsOthFinNonFin",
                               Description = "Net U.S. acquistion of other investment assets; insurance technical reserves; held by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "InsTechReservesLiabs",
                               Description = "Net U.S. incurrence of other investment liabilities; insurance technical reserves",
                           },
                           new Indicator
                           {
                               Val = "InsTechReservesLiabsOthFinNonFin",
                               Description = "Net U.S. incurrence of other investment liabilities; insurance technical reserves; issued by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "InvFundSharesAssets",
                               Description = "Net U.S. acquisition of portfolio investment assets; investment fund shares",
                           },
                           new Indicator
                           {
                               Val = "InvFundSharesIncPay",
                               Description = "Portfolio investment income payments; income attributable to investment fund shareholders",
                           },
                           new Indicator
                           {
                               Val = "InvFundSharesIncRec",
                               Description = "Portfolio investment income receipts; income attributable to investment fund shareholders",
                           },
                           new Indicator
                           {
                               Val = "InvFundSharesLiabs",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; investment fund shares",
                           },
                           new Indicator
                           {
                               Val = "InvFundSharesLiabsFoa",
                               Description = "Net U.S. incurrence of portfolio investment liabilities to foreign official agencies; investment fund shares",
                           },
                           new Indicator
                           {
                               Val = "InvIncPay",
                               Description = "Investment income payments",
                           },
                           new Indicator
                           {
                               Val = "InvIncRec",
                               Description = "Investment income receipts",
                           },
                           new Indicator
                           {
                               Val = "InvRaIncRec",
                               Description = "Reserve asset income receipts",
                           },
                           new Indicator
                           {
                               Val = "InvRaIntIncRec",
                               Description = "Reserve asset income receipts; interest",
                           },
                           new Indicator
                           {
                               Val = "LoansAssets",
                               Description = "Net U.S. acquisition of other investment assets; loans",
                           },
                           new Indicator
                           {
                               Val = "LoansAssetsDepTaking",
                               Description = "Net U.S. acquistion of other investment assets; loans; held by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "LoansAssetsGenGovt",
                               Description = "Net U.S. acquistion of other investment assets; loans; held by general government",
                           },
                           new Indicator
                           {
                               Val = "LoansAssetsOthFinNonFin",
                               Description = "Net U.S. acquistion of other investment assets; loans; held by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "LoansLiabs",
                               Description = "Net U.S. incurrence of other investment liabilities; loans",
                           },
                           new Indicator
                           {
                               Val = "LoansLiabsDepTaking",
                               Description = "Net U.S. incurrence of other investment liabilities; loans; issued by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "LoansLiabsFoa",
                               Description = "Net U.S. incurrence of other investment liabilities to foreign official agencies; loans",
                           },
                           new Indicator
                           {
                               Val = "LoansLiabsOthFinNonFin",
                               Description = "Net U.S. incurrence of other investment liabilities; loans; issued by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "LoansRepurchaseLiabsOthFinNonFin",
                               Description = "Net U.S. incurrence of other investment liabilities; loans, of which repurchase agreements; issued by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "LoansResaleAssetsOthFinNonFin",
                               Description = "Net U.S. acquistion of other investment assets; loans, of which resale agreements; held by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecAssets",
                               Description = "Net U.S. acquisition of portfolio investment assets; long-term debt securities",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecAssetsDepTaking",
                               Description = "Net U.S. acquisition of portfolio investment assets; long-term debt securities; held by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecAssetsNonFin",
                               Description = "Net U.S. acquisition of portfolio investment assets; long-term debt securities; held by nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecAssetsOthFin",
                               Description = "Net U.S. acquisition of portfolio investment assets; long-term debt securities; held by non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecCorpAssets",
                               Description = "Net U.S. acquisition of portfolio investment assets; corporate bonds and notes",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecCorpLiabs",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; corporate bonds and notes",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecCorpLiabsFoa",
                               Description = "Net U.S. incurrence of portfolio investment liabilities to foreign official agencies; corporate bonds and notes",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecFedSponsorAgencyIncPay",
                               Description = "Portfolio investment income payments; interest on long-term federally sponsored agency securties",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecFedSponsorAgencyLiabs",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; long-term federally sponsored agency securities",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecFedSponsorAgencyLiabsFoa",
                               Description = "Net U.S. incurrence of portfolio investment liabilities to foreign official agencies; long-term federally sponsored agency securities",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecGovtAssets",
                               Description = "Net U.S. acquisition of portfolio investment assets; long-term government debt securities",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecIncPay",
                               Description = "Portfolio investment income payments; interest on long-term debt securities",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecIncPayDepTaking",
                               Description = "Portfolio investment income payments; interest on long-term debt securities; deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecIncPayNonFin",
                               Description = "Portfolio investment income payments; interest on long-term debt securities; nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecIncRec",
                               Description = "Portfolio investment income receipts; interest on long-term debt securities",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecIncRecDepTaking",
                               Description = "Portfolio investment income receipts; interest on long-term debt securities; deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecIncRecNonFin",
                               Description = "Portfolio investment income receipts; interest on long-term debt securities; nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecIncRecOthFin",
                               Description = "Portfolio investment income receipts; interest on long-term debt securities; non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecLiabs",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; long-term debt securities",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecLiabsDepTaking",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; long-term debt securities; issued by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecLiabsFoa",
                               Description = "Net U.S. incurrence of portfolio investment liabilities to foreign official agencies; long-term debt securities",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecLiabsNonFin",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; long-term debt securities; issued by nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecNegCdAssets",
                               Description = "Net U.S. acquisition of portfolio investment assets; long-term negotiable certificates of deposit",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecNegCdLiabs",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; long-term negotiable certificates of deposit",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecNegCdLiabsFoa",
                               Description = "Net U.S. incurrence of portfolio investment liabilities to foreign official agencies; long-term negotiable certificates of deposit",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecOthThanFedSponsorAgencyIncPayOthFin",
                               Description = "Portfolio investment income payments; interest on long-term debt securities other than federally sponsored agency securties; non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecOthThanFedSponsorAgencyLiabsOthFin",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; long-term debt securities other than federally sponsored agency securities; issued by non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecStateLocalGovtIncPay",
                               Description = "Portfolio investment income payments; interest on state and local government long-term securities",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecStateLocalGovtLiabs",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; state and local government long-term securities",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecStateLocalGovtLiabsFoa",
                               Description = "Net U.S. incurrence of portfolio investment liabilities to foreign official agencies; state and local government long-term securities",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecTreasIncPay",
                               Description = "Portfolio investment income payments; interest on long-term U.S. Treasury securities",
                           },
                           new Indicator
                           {
                               Val = "LtDebtSecTreasLiabs",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; Treasury bonds and notes",
                           },
                           new Indicator
                           {
                               Val = "LtDepAssets",
                               Description = "Net U.S. acquisition of other investment assets; long-term deposits",
                           },
                           new Indicator
                           {
                               Val = "LtDepAssetsDepTaking",
                               Description = "Net U.S. acquistion of other investment assets; long-term deposits; held by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "LtDepAssetsOthFinNonFin",
                               Description = "Net U.S. acquistion of other investment assets; long-term deposits; held by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "LtDepLiabs",
                               Description = "Net U.S. incurrence of other investment liabilities; long-term deposits",
                           },
                           new Indicator
                           {
                               Val = "LtDepLiabsDepTaking",
                               Description = "Net U.S. incurrence of other investment liabilities; long-term deposits; issued by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "LtDepLiabsOthFinNonFin",
                               Description = "Net U.S. incurrence of other investment liabilities; long-term deposits; issued by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "LtLoansAssets",
                               Description = "Net U.S. acquisition of other investment assets; long-term loans",
                           },
                           new Indicator
                           {
                               Val = "LtLoansAssetsDepTaking",
                               Description = "Net U.S. acquistion of other investment assets; long-term loans; held by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "LtLoansAssetsGenGovt",
                               Description = "Net U.S. acquistion of other investment assets; long-term loans; held by general government",
                           },
                           new Indicator
                           {
                               Val = "LtLoansAssetsOthFinNonFin",
                               Description = "Net U.S. acquistion of other investment assets; long-term loans; held by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "LtLoansLiabs",
                               Description = "Net U.S. incurrence of other investment liabilities; long-term loans",
                           },
                           new Indicator
                           {
                               Val = "LtLoansLiabsDepTaking",
                               Description = "Net U.S. incurrence of other investment liabilities; long-term loans; issued by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "LtLoansLiabsOthFinNonFin",
                               Description = "Net U.S. incurrence of other investment liabilities; long-term loans; issued by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "LtTrdCredAndAdvAssets",
                               Description = "Net U.S. acquisition of other investment assets; long-term trade credit and advances",
                           },
                           new Indicator
                           {
                               Val = "LtTrdCredAndAdvAssetsOthFinNonFin",
                               Description = "Net U.S. acquistion of other investment assets; long-term trade credit and advances; held by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "LtTrdCredAndAdvLiabs",
                               Description = "Net U.S. incurrence of other investment liabilities; long-term trade credit and advances",
                           },
                           new Indicator
                           {
                               Val = "LtTrdCredAndAdvLiabsGenGovt",
                               Description = "Net U.S. incurrence of other investment liabilities; long-term trade credit and advances; issued by general government",
                           },
                           new Indicator
                           {
                               Val = "LtTrdCredAndAdvLiabsOthFinNonFin",
                               Description = "Net U.S. incurrence of other investment liabilities; long-term trade credit and advances; issued by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "NetLendBorrCurrCapAcct",
                               Description = "Net lending (+) or net borrowing (-) from current- and capital-account transactions",
                           },
                           new Indicator
                           {
                               Val = "NetLendBorrFinAcct",
                               Description = "Net lending (+) or net borrowing (-) from financial-account transactions",
                           },
                           new Indicator
                           {
                               Val = "OthClmReserveAssets",
                               Description = "Net U.S. acquisition of reserve assets; other; other claims",
                           },
                           new Indicator
                           {
                               Val = "OthInvAssets",
                               Description = "Net U.S. acquisition of other investment assets",
                           },
                           new Indicator
                           {
                               Val = "OthInvAssetsCentralBank",
                               Description = "Net U.S. acquistion of other investment assets; held by central bank",
                           },
                           new Indicator
                           {
                               Val = "OthInvAssetsDepTaking",
                               Description = "Net U.S. acquistion of other investment assets; held by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "OthInvAssetsGenGovt",
                               Description = "Net U.S. acquistion of other investment assets; held by general government",
                           },
                           new Indicator
                           {
                               Val = "OthInvAssetsOthFinNonFin",
                               Description = "Net U.S. acquistion of other investment assets; held by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "OthInvIncPay",
                               Description = "Other investment income payments",
                           },
                           new Indicator
                           {
                               Val = "OthInvIncPayCentralBank",
                               Description = "Other investment income payments; on liabilities issued by central bank",
                           },
                           new Indicator
                           {
                               Val = "OthInvIncPayDepTaking",
                               Description = "Other investment income payments; on liabilities issued by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "OthInvIncPayGenGovt",
                               Description = "Other investment income payments; on liabilities issued by general government",
                           },
                           new Indicator
                           {
                               Val = "OthInvIncPayOthFinNonFin",
                               Description = "Other investment income payments; on liabilities issued by non-deposit-taking financial institutions and nonfinancial instutitions except general government",
                           },
                           new Indicator
                           {
                               Val = "OthInvIncRec",
                               Description = "Other investment income receipts",
                           },
                           new Indicator
                           {
                               Val = "OthInvIncRecCentralBank",
                               Description = "Other investment income receipts; on assets held by central bank",
                           },
                           new Indicator
                           {
                               Val = "OthInvIncRecDepTaking",
                               Description = "Other investment income receipts; on assets held by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "OthInvIncRecGenGovt",
                               Description = "Other investment income receipts; on assets held by general government",
                           },
                           new Indicator
                           {
                               Val = "OthInvIncRecOthFinNonFin",
                               Description = "Other investment income receipts; on assets held by non-deposit-taking financial institutions and nonfinancial instutitions except general government",
                           },
                           new Indicator
                           {
                               Val = "OthInvInsPolHoldtIncPay",
                               Description = "Other investment income payments; income attributable to insurance policyholders",
                           },
                           new Indicator
                           {
                               Val = "OthInvInsPolHoldtIncPayOthFinNonFin",
                               Description = "Other investment income payments; income attributable to insurance policyholders",
                           },
                           new Indicator
                           {
                               Val = "OthInvInsPolHoldtIncRec",
                               Description = "Other investment income receipts; income attributable to insurance policyholders",
                           },
                           new Indicator
                           {
                               Val = "OthInvInsPolHoldtIncRecOthFinNonFin",
                               Description = "Other investment income receipts; income attributable to insurance policyholders",
                           },
                           new Indicator
                           {
                               Val = "OthInvInterbankAssets",
                               Description = "Net U.S. acquistion of other investment assets; interbank transactions",
                           },
                           new Indicator
                           {
                               Val = "OthInvInterbankLiabsDepTaking",
                               Description = "Net U.S. incurrence of other investment liabilities; interbank transactions",
                           },
                           new Indicator
                           {
                               Val = "OthInvIntIncPay",
                               Description = "Other investment income payments; interest",
                           },
                           new Indicator
                           {
                               Val = "OthInvIntIncPayCentralBank",
                               Description = "Other investment income payments; interest; on liabilities issued by central bank",
                           },
                           new Indicator
                           {
                               Val = "OthInvIntIncPayDepTaking",
                               Description = "Other investment income payments; interest; on liabilities issued by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "OthInvIntIncPayGenGovt",
                               Description = "Other investment income payments; interest on special drawing rights allocations",
                           },
                           new Indicator
                           {
                               Val = "OthInvIntIncPayOthFinNonFin",
                               Description = "Other investment income payments; interest; on liabilities issued by non-deposit-taking financial institutions and nonfinancial instutitions except general government",
                           },
                           new Indicator
                           {
                               Val = "OthInvIntIncRec",
                               Description = "Other investment income receipts; interest",
                           },
                           new Indicator
                           {
                               Val = "OthInvIntIncRecCentralBank",
                               Description = "Other investment income receipts; interest; on assets held by central bank",
                           },
                           new Indicator
                           {
                               Val = "OthInvIntIncRecDepTaking",
                               Description = "Other investment income receipts; interest; on assets held by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "OthInvIntIncRecGenGovt",
                               Description = "Other investment income receipts; interest; on assets held by general government",
                           },
                           new Indicator
                           {
                               Val = "OthInvIntIncRecOthFinNonFin",
                               Description = "Other investment income receipts; interest; on assets held by non-deposit-taking financial institutions and nonfinancial instutitions except general government",
                           },
                           new Indicator
                           {
                               Val = "OthInvLiabs",
                               Description = "Net U.S. incurrence of other investment liabilities",
                           },
                           new Indicator
                           {
                               Val = "OthInvLiabsCentralBank",
                               Description = "Net U.S. incurrence of other investment liabilities; issued by central bank",
                           },
                           new Indicator
                           {
                               Val = "OthInvLiabsDepTaking",
                               Description = "Net U.S. incurrence of other investment liabilities; issued by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "OthInvLiabsFoa",
                               Description = "Net U.S. incurrence of other investment liabilities to foreign official agencies",
                           },
                           new Indicator
                           {
                               Val = "OthInvLiabsGenGovt",
                               Description = "Net U.S. incurrence of other investment liabilities; issued by general government",
                           },
                           new Indicator
                           {
                               Val = "OthInvLiabsOthFinNonFin",
                               Description = "Net U.S. incurrence of other investment liabilities; issued by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "OthReserveAssets",
                               Description = "Net U.S. acquisition of reserve assets; other",
                           },
                           new Indicator
                           {
                               Val = "PfInvAssets",
                               Description = "Net U.S. acquisition of portfolio investment assets",
                           },
                           new Indicator
                           {
                               Val = "PfInvAssetsDepTaking",
                               Description = "Net U.S. acquisition of portfolio investment assets; held by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "PfInvAssetsNonFin",
                               Description = "Net U.S. acquisition of portfolio investment assets; held by nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "PfInvAssetsOthFin",
                               Description = "Net U.S. acquisition of portfolio investment assets; held by non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "PfInvIncPay",
                               Description = "Portfolio investment income payments",
                           },
                           new Indicator
                           {
                               Val = "PfInvIncPayDepTaking",
                               Description = "Portfolio investment income payments; deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "PfInvIncPayGenGovt",
                               Description = "Portfolio investment income payments; general government",
                           },
                           new Indicator
                           {
                               Val = "PfInvIncPayNonFin",
                               Description = "Portfolio investment income payments; nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "PfInvIncPayOthFin",
                               Description = "Portfolio investment income payments; non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "PfInvIncRec",
                               Description = "Portfolio investment income receipts",
                           },
                           new Indicator
                           {
                               Val = "PfInvIncRecDepTaking",
                               Description = "Portfolio investment income receipts; deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "PfInvIncRecNonFin",
                               Description = "Portfolio investment income receipts; nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "PfInvIncRecOthFin",
                               Description = "Portfolio investment income receipts; non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "PfInvLiabs",
                               Description = "Net U.S. incurrence of portfolio investment liabilities",
                           },
                           new Indicator
                           {
                               Val = "PfInvLiabsDepTaking",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; issued by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "PfInvLiabsFoa",
                               Description = "Net U.S. incurrence of portfolio investment liabilities to foreign official agencies",
                           },
                           new Indicator
                           {
                               Val = "PfInvLiabsGenGovt",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; issued by general government",
                           },
                           new Indicator
                           {
                               Val = "PfInvLiabsNonFin",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; issued by nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "PfInvLiabsOthFin",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; issued by non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "PrimIncPay",
                               Description = "Primary income payments",
                           },
                           new Indicator
                           {
                               Val = "PrimIncRec",
                               Description = "Primary income receipts",
                           },
                           new Indicator
                           {
                               Val = "ReserveAssets",
                               Description = "Net U.S. acquisition of reserve assets",
                           },
                           new Indicator
                           {
                               Val = "SdrAllocLiabs",
                               Description = "Net U.S. incurrence of other investment liabilities; special drawing rights allocations",
                           },
                           new Indicator
                           {
                               Val = "SdrAllocLiabsFoa",
                               Description = "Net U.S. incurrence of other investment liabilities to foreign official agencies; special drawing rights allocations",
                           },
                           new Indicator
                           {
                               Val = "SdrReserveAssets",
                               Description = "Net U.S. acquisition of reserve assets; special drawing rights",
                           },
                           new Indicator
                           {
                               Val = "SeasAdjDisc",
                               Description = "Seasonal adjustment discrepancy",
                           },
                           new Indicator
                           {
                               Val = "SecIncPay",
                               Description = "Secondary income (current transfer) payments",
                           },
                           new Indicator
                           {
                               Val = "SecIncPayOthPrivate",
                               Description = "Secondary income (current transfer) payments; private transfers other than personal transfers",
                           },
                           new Indicator
                           {
                               Val = "SecIncPayPersonal",
                               Description = "Secondary income (current transfer) payments; personal transfers",
                           },
                           new Indicator
                           {
                               Val = "SecIncPayPrivate",
                               Description = "Secondary income (current transfer) payments; private transfers",
                           },
                           new Indicator
                           {
                               Val = "SecIncPayUsGovt",
                               Description = "Secondary income (current transfer) payments; U.S. government transfers",
                           },
                           new Indicator
                           {
                               Val = "SecIncPayUsGovtGrants",
                               Description = "Secondary income (current transfer) payments; U.S. government grants",
                           },
                           new Indicator
                           {
                               Val = "SecIncPayUsGovtPensionsAndOth",
                               Description = "Secondary income (current transfer) payments; U.S. government pensions and other transfers",
                           },
                           new Indicator
                           {
                               Val = "SecIncRec",
                               Description = "Secondary income (current transfer) receipts",
                           },
                           new Indicator
                           {
                               Val = "SecIncRecPrivate",
                               Description = "Secondary income (current transfer) receipts; private transfers",
                           },
                           new Indicator
                           {
                               Val = "SecIncRecUsGovt",
                               Description = "Secondary income (current transfer) receipts; U.S. government transfers",
                           },
                           new Indicator
                           {
                               Val = "SecReserveAssets",
                               Description = "Net U.S. acquisition of reserve assets; other; securities",
                           },
                           new Indicator
                           {
                               Val = "StatDisc",
                               Description = "Statistical discrepancy",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecAssets",
                               Description = "Net U.S. acquisition of portfolio investment assets; short-term debt securities",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecAssetsDepTaking",
                               Description = "Net U.S. acquisition of portfolio investment assets; short-term debt securities; held by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecAssetsNonFin",
                               Description = "Net U.S. acquisition of portfolio investment assets; short-term debt securities; held by nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecAssetsOthFin",
                               Description = "Net U.S. acquisition of portfolio investment assets; short-term debt securities; held by non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecCommPaperAndOthLiabs",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; commercial paper and other short-term debt securities (those not listed in table 7.1)",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecCommPaperAndOthLiabsFoa",
                               Description = "Net U.S. incurrence of portfolio investment liabilities to foreign official agencies; commercial paper and other short-term debt securities (those not listed in table 9.1)",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecCommPaperAssets",
                               Description = "Net U.S. acquisition of portfolio investment assets; short-term commercial paper",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecFedSponsorAgencyIncPay",
                               Description = "Portfolio investment income payments; interest on short-term federally sponsored agency securties",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecFedSponsorAgencyLiabs",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; short-term federally sponsored agency securities",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecFedSponsorAgencyLiabsFoa",
                               Description = "Net U.S. incurrence of portfolio investment liabilities to foreign official agencies; short-term federally sponsored agency securities",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecIncPay",
                               Description = "Portfolio investment income payments; interest on short-term debt securities",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecIncPayDepTaking",
                               Description = "Portfolio investment income payments; interest on short-term debt securities; deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecIncPayNonFin",
                               Description = "Portfolio investment income payments; interest on short-term debt securities; nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecIncRec",
                               Description = "Portfolio investment income receipts; interest on short-term debt securities",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecIncRecDepTaking",
                               Description = "Portfolio investment income receipts; interest on short-term debt securities; deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecIncRecNonFin",
                               Description = "Portfolio investment income receipts; interest on short-term debt securities; nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecIncRecOthFin",
                               Description = "Portfolio investment income receipts; interest on short-term debt securities; non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecLiabs",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; short-term debt securities",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecLiabsDepTaking",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; short-term debt securities; issued by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecLiabsFoa",
                               Description = "Net U.S. incurrence of portfolio investment liabilities to foreign official agencies; short-term debt securities",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecLiabsNonFin",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; short-term debt securities; issued by nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecNegCdAssets",
                               Description = "Net U.S. acquisition of portfolio investment assets; short-term negotiable certificates of deposit",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecNegCdLiabs",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; short-term negotiable certificates of deposit",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecNegCdLiabsFoa",
                               Description = "Net U.S. incurrence of portfolio investment liabilities to foreign official agencies; short-term negotiable certificates of deposit",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecOthAssets",
                               Description = "Net U.S. acquisition of portfolio investment assets; short-term debt securities other than negotiable certificates of deposit and commercial paper",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecOthThanFedSponsorAgencyIncPayOthFin",
                               Description = "Portfolio investment income payments; interest on short-term debt securities other than federally sponsored agency securties; non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecOthThanFedSponsorAgencyLiabsOthFin",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; short-term debt securities other than federally sponsored agency securities; issued by non-deposit-taking financial institutions",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecTreasIncPay",
                               Description = "Portfolio investment income payments; interest on short-term U.S. Treasury securities",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecTreasLiabs",
                               Description = "Net U.S. incurrence of portfolio investment liabilities; Treasury bills and certificates",
                           },
                           new Indicator
                           {
                               Val = "StDebtSecTreasLiabsFoa",
                               Description = "Net U.S. incurrence of portfolio investment liabilities to foreign official agencies; Treasury bills and certificates",
                           },
                           new Indicator
                           {
                               Val = "StDepAssets",
                               Description = "Net U.S. acquisition of other investment assets; short-term deposits",
                           },
                           new Indicator
                           {
                               Val = "StDepAssetsCentralBank",
                               Description = "Net U.S. acquistion of other investment assets; short-term deposits; held by central bank",
                           },
                           new Indicator
                           {
                               Val = "StDepAssetsDepTaking",
                               Description = "Net U.S. acquistion of other investment assets; short-term deposits; held by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "StDepAssetsOthFinNonFin",
                               Description = "Net U.S. acquistion of other investment assets; short-term deposits; held by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "StDepLiabs",
                               Description = "Net U.S. incurrence of other investment liabilities; short-term deposits",
                           },
                           new Indicator
                           {
                               Val = "StDepLiabsCentralBank",
                               Description = "Net U.S. incurrence of other investment liabilities; short-term deposits; issued by central bank",
                           },
                           new Indicator
                           {
                               Val = "StDepLiabsDepTaking",
                               Description = "Net U.S. incurrence of other investment liabilities; short-term deposits; issued by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "StDepLiabsOthFinNonFin",
                               Description = "Net U.S. incurrence of other investment liabilities; short-term deposits; issued by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "StLoansAssets",
                               Description = "Net U.S. acquisition of other investment assets; short-term loans",
                           },
                           new Indicator
                           {
                               Val = "StLoansAssetsDepTaking",
                               Description = "Net U.S. acquistion of other investment assets; short-term loans; held by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "StLoansAssetsOthFinNonFin",
                               Description = "Net U.S. acquistion of other investment assets; short-term loans; held by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "StLoansLiabs",
                               Description = "Net U.S. incurrence of other investment liabilities; short-term loans",
                           },
                           new Indicator
                           {
                               Val = "StLoansLiabsDepTaking",
                               Description = "Net U.S. incurrence of other investment liabilities; short-term loans; issued by deposit-taking institutions except central bank",
                           },
                           new Indicator
                           {
                               Val = "StLoansLiabsOthFinNonFin",
                               Description = "Net U.S. incurrence of other investment liabilities; short-term loans; issued by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "StTrdCredAndAdvAssets",
                               Description = "Net U.S. acquisition of other investment assets; short-term trade credit and advances",
                           },
                           new Indicator
                           {
                               Val = "StTrdCredAndAdvAssetsOthFinNonFin",
                               Description = "Net U.S. acquistion of other investment assets; short-term trade credit and advances; held by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "StTrdCredAndAdvLiabs",
                               Description = "Net U.S. incurrence of other investment liabilities; short-term trade credit and advances",
                           },
                           new Indicator
                           {
                               Val = "StTrdCredAndAdvLiabsOthFinNonFin",
                               Description = "Net U.S. incurrence of other investment liabilities; short-term trade credit and advances; issued by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "TrdCredAndAdvAssets",
                               Description = "Net U.S. acquisition of other investment assets; trade credit and advances",
                           },
                           new Indicator
                           {
                               Val = "TrdCredAndAdvAssetsOthFinNonFin",
                               Description = "Net U.S. acquistion of other investment assets; trade credit and advances; held by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "TrdCredAndAdvLiabs",
                               Description = "Net U.S. incurrence of other investment liabilities; trade credit and advances",
                           },
                           new Indicator
                           {
                               Val = "TrdCredAndAdvLiabsFoa",
                               Description = "Net U.S. incurrence of other investment liabilities to foreign official agencies; trade credit and advances",
                           },
                           new Indicator
                           {
                               Val = "TrdCredAndAdvLiabsGenGovt",
                               Description = "Net U.S. incurrence of other investment liabilities; trade credit and advances; issued by general government",
                           },
                           new Indicator
                           {
                               Val = "TrdCredAndAdvLiabsOthFinNonFin",
                               Description = "Net U.S. incurrence of other investment liabilities; trade credit and advances; issued by non-deposit-taking institutions and nonfinancial institutions except general government",
                           },
                           new Indicator
                           {
                               Val = "TreasBondsAndNotesLiabsFoa",
                               Description = "Net U.S. incurrence of portfolio investment liabilities to foreign official agencies; Treasury bonds and notes",
                           },
                           new Indicator
                           {
                               Val = "TSI_ItaDiInvIncInward",
                               Description = "Direct investment income on inward investment (foreign direct investment in the United States), directional basis",
                           },
                           new Indicator
                           {
                               Val = "TSI_ItaDiInvIncOutward",
                               Description = "Direct investment income on outward investment (U.S. direct investment abroad), directional basis",
                           },
                           new Indicator
                           {
                               Val = "TSI_ItaDiInvWithoutCurrCostAdjIncOutward",
                               Description = "Direct investment income without current cost adjustment on outward investment (U.S. direct investment abroad), directional basis",
                           },

                       };
                return _values;
            }
        }
	}//end Indicator
}//end NoFuture.Rand.Gov.Bea.Parameters.Ita