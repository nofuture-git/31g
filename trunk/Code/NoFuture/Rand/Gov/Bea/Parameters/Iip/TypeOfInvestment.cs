using System;
using System.Collections.Generic;
namespace NoFuture.Rand.Gov.Bea.Parameters.Iip
{
    public class TypeOfInvestment : NoFuture.Rand.Gov.Bea.BeaParameter
    {
        public override string Description { get; set; }
        public override string Val { get; set; }
        private static List<TypeOfInvestment> _values;
        public static List<TypeOfInvestment> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<TypeOfInvestment>
                           {
                           
                           new TypeOfInvestment
                           {
                               Val = "CurrAndDepAssets",
                               Description = "U.S. assets; other investment; currency and deposits",
                           },
                           new TypeOfInvestment
                           {
                               Val = "CurrAndDepLiabs",
                               Description = "U.S. liabilities; other investment; currency and deposits",
                           },
                           new TypeOfInvestment
                           {
                               Val = "CurrAndDepLiabsFoa",
                               Description = "U.S. liabilities to foreign official agencies; other investment; currency and deposits",
                           },
                           new TypeOfInvestment
                           {
                               Val = "CurrAndDepReserveAssets",
                               Description = "U.S. assets; other reserve assets; currency and deposits",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DebtSecAssets",
                               Description = "U.S. assets; portfolio investment; debt securities",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DebtSecLiabs",
                               Description = "U.S. liabilities; portfolio investment; debt securities",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DebtSecLiabsFoa",
                               Description = "U.S. liabilities to foreign official agencies; portfolio investment; debt securities",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvAssets",
                               Description = "U.S. assets; direct investment at market value, asset/liability basis",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvAssetsCurrCost",
                               Description = "U.S. assets; direct investment at current cost, asset/liability basis",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvAssetsHistCostToMarketValueAdj",
                               Description = "U.S. assets; direct investment; adjustment to revalue equity from historical cost to market value",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvDebtInstAssets",
                               Description = "U.S. assets; direct investment, asset/liability basis; debt instruments",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvDebtInstInward",
                               Description = "Inward direct investment (foreign direct investment in the United States), directional basis; debt instruments",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvDebtInstLiabs",
                               Description = "U.S. liabilities; direct investment, asset/liability basis; debt instruments",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvDebtInstOutward",
                               Description = "Outward direct investment (U.S. direct investment abroad), directional basis; debt instruments",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvDebtInstUsAffiliatesClaims",
                               Description = "Direct investment; debt instruments; U.S. affiliates' claims",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvDebtInstUsAffiliatesLiabs",
                               Description = "Direct investment; debt instruments; U.S. affiliates' liabilites",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvDebtInstUsParentsClaims",
                               Description = "Direct investment; debt instruments; U.S. parents' claims",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvDebtInstUsParentsLiabs",
                               Description = "Direct investment; debt instruments; U.S. parents' liabilites",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvDirectionalBasisAdj",
                               Description = "Direct investment; adjustments to convert to directional basis",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvEquityAssets",
                               Description = "U.S. assets; direct investment at market value; equity",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvEquityAssetsCurrCost",
                               Description = "U.S. assets; direct investment at current cost; equity",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvEquityAssetsHistCost",
                               Description = "U.S. assets; direct investment at historical cost; equity",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvEquityInward",
                               Description = "U.S. liabilities; direct investment at market value; equity",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvEquityLiabs",
                               Description = "U.S. liabilities; direct investment at market value; equity",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvEquityLiabsCurrCost",
                               Description = "U.S. liabilities; direct investment at current cost; equity",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvEquityLiabsHistCost",
                               Description = "U.S. liabilities; direct investment at historical cost; equity",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvInwardCurrCost",
                               Description = "Inward direct investment (foreign direct investment in the United States) at current cost, directional basis",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvInwardHistCost",
                               Description = "Inward direct investment (foreign direct investment in the United States) at historical cost, directional basis",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvInwardMarketValue",
                               Description = "Inward direct investment (foreign direct investment in the United States) at market value, directional basis",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvLiabs",
                               Description = "U.S. liabilities; direct investment at market value, asset/liability basis",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvLiabsCurrCost",
                               Description = "U.S. liabilities; direct investment at current cost, asset/liability basis",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvLiabsHistCostToMarketValueAdj",
                               Description = "U.S. liabilities; direct investment; adjustment to revalue equity from historical cost to market value",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvOutwardCurrCost",
                               Description = "Outward direct investment (U.S. direct investment abroad) at current cost, directional basis",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvOutwardHistCost",
                               Description = "Outward direct investment (U.S. direct investment abroad) at historical cost, directional basis",
                           },
                           new TypeOfInvestment
                           {
                               Val = "DiInvOutwardMarketValue",
                               Description = "Outward direct investment (U.S. direct investment abroad) at market value, directional basis",
                           },
                           new TypeOfInvestment
                           {
                               Val = "EquityAndInvFundSharesAssets",
                               Description = "U.S. assets; portfolio investment; equity and investment fund shares",
                           },
                           new TypeOfInvestment
                           {
                               Val = "EquityAndInvFundSharesLiabs",
                               Description = "U.S. liabilities; portfolio investment; equity and investment fund shares",
                           },
                           new TypeOfInvestment
                           {
                               Val = "EquityAndInvFundSharesLiabsFoa",
                               Description = "U.S. liabilities to foreign official agencies; portfolio investment; equity and investment fund shares",
                           },
                           new TypeOfInvestment
                           {
                               Val = "FinAssets",
                               Description = "U.S. assets",
                           },
                           new TypeOfInvestment
                           {
                               Val = "FinAssetsExclFinDeriv",
                               Description = "U.S. assets excluding financial derivatives",
                           },
                           new TypeOfInvestment
                           {
                               Val = "FinDerivAssets",
                               Description = "U.S. assets; financial derivatives other than reserves, gross positive fair value",
                           },
                           new TypeOfInvestment
                           {
                               Val = "FinDerivExchTradedAssets",
                               Description = "U.S. assets; financial derivatives other than reserves, gross positive fair value; exchange-traded contracts",
                           },
                           new TypeOfInvestment
                           {
                               Val = "FinDerivExchTradedLiabs",
                               Description = "U.S. liabilities; financial derivatives other than reserves, gross negative fair value; exchange-traded contracts",
                           },
                           new TypeOfInvestment
                           {
                               Val = "FinDerivForExAssets",
                               Description = "U.S. assets; financial derivatives other than reserves, gross positive fair value; over-the-counter contracts; foreign exchange contracts",
                           },
                           new TypeOfInvestment
                           {
                               Val = "FinDerivForExLiabs",
                               Description = "U.S. liabilities; financial derivatives other than reserves, gross negative fair value; over-the-counter contracts; foreign exchange contracts",
                           },
                           new TypeOfInvestment
                           {
                               Val = "FinDerivLiabs",
                               Description = "U.S. liabilities; financial derivatives other than reserves, gross negative fair value",
                           },
                           new TypeOfInvestment
                           {
                               Val = "FinDerivNet",
                               Description = "U.S. net international investment position; financial derivatives other than reserves",
                           },
                           new TypeOfInvestment
                           {
                               Val = "FinDerivOtcAssets",
                               Description = "U.S. assets; financial derivatives other than reserves, gross positive fair value; over-the-counter contracts",
                           },
                           new TypeOfInvestment
                           {
                               Val = "FinDerivOtcLiabs",
                               Description = "U.S. liabilities; financial derivatives other than reserves, gross negative fair value; over-the-counter contracts",
                           },
                           new TypeOfInvestment
                           {
                               Val = "FinDerivOthAssets",
                               Description = "U.S. assets; financial derivatives other than reserves, gross positive fair value; other over-the-counter contracts",
                           },
                           new TypeOfInvestment
                           {
                               Val = "FinDerivOthLiabs",
                               Description = "U.S. liabilities; financial derivatives other than reserves, gross negative fair value; other over-the-counter contracts",
                           },
                           new TypeOfInvestment
                           {
                               Val = "FinDerivReserveAssets",
                               Description = "U.S. assets; other reserve assets; financial derivatives",
                           },
                           new TypeOfInvestment
                           {
                               Val = "FinDerivSingleCurrAssets",
                               Description = "U.S. assets; financial derivatives other than reserves, gross positive fair value; over-the-counter contracts; single-currency interest rate contracts",
                           },
                           new TypeOfInvestment
                           {
                               Val = "FinDerivSingleCurrLiabs",
                               Description = "U.S. liabilities; financial derivatives other than reserves, gross negative fair value; over-the-counter contracts; single-currency interest rate contracts",
                           },
                           new TypeOfInvestment
                           {
                               Val = "FinLiabs",
                               Description = "U.S. liabilities",
                           },
                           new TypeOfInvestment
                           {
                               Val = "FinLiabsExclFinDeriv",
                               Description = "U.S. liabilities excluding financial derivatives",
                           },
                           new TypeOfInvestment
                           {
                               Val = "FinLiabsFoa",
                               Description = "U.S. liabilities to foreign official agencies",
                           },
                           new TypeOfInvestment
                           {
                               Val = "GoldReserveAssets",
                               Description = "U.S. assets; reserve assets; monetary gold",
                           },
                           new TypeOfInvestment
                           {
                               Val = "ImfReserveAssets",
                               Description = "U.S. assets; reserve assets; reserve position in the International Monetary Fund",
                           },
                           new TypeOfInvestment
                           {
                               Val = "InsTechReservesAssets",
                               Description = "U.S. assets; other investment; insurance technical reserves",
                           },
                           new TypeOfInvestment
                           {
                               Val = "InsTechReservesLiabs",
                               Description = "U.S. liabilities; other investment; insurance technical reserves",
                           },
                           new TypeOfInvestment
                           {
                               Val = "LoansAssets",
                               Description = "U.S. assets; other investment; loans",
                           },
                           new TypeOfInvestment
                           {
                               Val = "LoansLiabs",
                               Description = "U.S. liabilities; other investment; loans",
                           },
                           new TypeOfInvestment
                           {
                               Val = "LoansLiabsFoa",
                               Description = "U.S. liabilities to foreign official agencies; other investment; loans",
                           },
                           new TypeOfInvestment
                           {
                               Val = "LtDebtSecAssets",
                               Description = "U.S. assets; portfolio investment; long-term debt securities",
                           },
                           new TypeOfInvestment
                           {
                               Val = "LtDebtSecLiabs",
                               Description = "U.S. liabilities; portfolio investment; long-term debt securities",
                           },
                           new TypeOfInvestment
                           {
                               Val = "LtDebtSecLiabsFoa",
                               Description = "U.S. liabilities to foreign official agencies; portfolio investment; long-term debt securities",
                           },
                           new TypeOfInvestment
                           {
                               Val = "LtDebtSecTreasLiabsFoa",
                               Description = "U.S. liabilities to foreign official agencies; portfolio investment; long-term debt securities; Treasury bonds and notes",
                           },
                           new TypeOfInvestment
                           {
                               Val = "Net",
                               Description = "U.S. net international investment position",
                           },
                           new TypeOfInvestment
                           {
                               Val = "NetExclFinDeriv",
                               Description = "U.S. net international investment position excluding financial derivatives",
                           },
                           new TypeOfInvestment
                           {
                               Val = "OthClmReserveAssets",
                               Description = "U.S. assets; other reserve assets; other claims",
                           },
                           new TypeOfInvestment
                           {
                               Val = "OthInvAssets",
                               Description = "U.S. assets; other investment",
                           },
                           new TypeOfInvestment
                           {
                               Val = "OthInvLiabs",
                               Description = "U.S. liabilities; other investment",
                           },
                           new TypeOfInvestment
                           {
                               Val = "OthInvLiabsFoa",
                               Description = "U.S. liabilities to foreign official agencies; other investment",
                           },
                           new TypeOfInvestment
                           {
                               Val = "OthLtDebtSecLiabs",
                               Description = "U.S. liabilities; portfolio investment; long-term debt securities excluding Treasury bonds and notes",
                           },
                           new TypeOfInvestment
                           {
                               Val = "OthLtDebtSecLiabsFoa",
                               Description = "U.S. liabilities to foreign official agencies; portfolio investment; long-term debt securities excluding Treasury bonds and notes",
                           },
                           new TypeOfInvestment
                           {
                               Val = "OthReserveAssets",
                               Description = "U.S. assets; other reserve assets",
                           },
                           new TypeOfInvestment
                           {
                               Val = "OthStDebtSecLiabs",
                               Description = "U.S. liabilities; portfolio investment; short-term debt securities excluding Treasury bills and certificates",
                           },
                           new TypeOfInvestment
                           {
                               Val = "OthStDebtSecLiabsFoa",
                               Description = "U.S. liabilities to foreign official agencies; portfolio investment; short-term debt securities excluding Treasury bills and certificates",
                           },
                           new TypeOfInvestment
                           {
                               Val = "PfInvAssets",
                               Description = "U.S. assets; portfolio investment",
                           },
                           new TypeOfInvestment
                           {
                               Val = "PfInvLiabs",
                               Description = "U.S. liabilities; portfolio investment",
                           },
                           new TypeOfInvestment
                           {
                               Val = "PfInvLiabsFoa",
                               Description = "U.S. liabilities to foreign official agencies; portfolio investment",
                           },
                           new TypeOfInvestment
                           {
                               Val = "ReserveAssets",
                               Description = "U.S. assets; reserve assets",
                           },
                           new TypeOfInvestment
                           {
                               Val = "SdrAllocLiabs",
                               Description = "U.S. liabilities; other investment; special drawing rights allocations",
                           },
                           new TypeOfInvestment
                           {
                               Val = "SdrAllocLiabsFoa",
                               Description = "U.S. liabilities to foreign official agencies; other investment; special drawing rights allocations",
                           },
                           new TypeOfInvestment
                           {
                               Val = "SdrReserveAssets",
                               Description = "U.S. assets; reserve assets; special drawing rights",
                           },
                           new TypeOfInvestment
                           {
                               Val = "SecReserveAssets",
                               Description = "U.S. assets; other reserve assets; securities",
                           },
                           new TypeOfInvestment
                           {
                               Val = "StDebtSecAssets",
                               Description = "U.S. assets; portfolio investment; short-term debt securities",
                           },
                           new TypeOfInvestment
                           {
                               Val = "StDebtSecLiabs",
                               Description = "U.S. liabilities; portfolio investment; short-term debt securities",
                           },
                           new TypeOfInvestment
                           {
                               Val = "StDebtSecLiabsFoa",
                               Description = "U.S. liabilities to foreign official agencies; portfolio investment; short-term debt securities",
                           },
                           new TypeOfInvestment
                           {
                               Val = "StDebtSecTreasLiabsFoa",
                               Description = "U.S. liabilities to foreign official agencies; portfolio investment; short-term debt securities; Treasury bills and certificates",
                           },
                           new TypeOfInvestment
                           {
                               Val = "TrdCredAndAdvAssets",
                               Description = "U.S. assets; other investment; trade credit and advances",
                           },
                           new TypeOfInvestment
                           {
                               Val = "TrdCredAndAdvLiabs",
                               Description = "U.S. liabilities; other investment; trade credit and advances",
                           },
                           new TypeOfInvestment
                           {
                               Val = "TrdCredAndAdvLiabsFoa",
                               Description = "U.S. liabilities to foreign official agencies; other investment; trade credit and advances",
                           },
                           new TypeOfInvestment
                           {
                               Val = "TreasBillsAndCertsLiabs",
                               Description = "U.S. liabilities; portfolio investment; short-term debt securities; Treasury bills and certificates",
                           },
                           new TypeOfInvestment
                           {
                               Val = "TreasBondsAndNotesLiabs",
                               Description = "U.S. liabilities; portfolio investment; long-term debt securities; Treasury bonds and notes",
                           },

                       };
                return _values;
            }
        }
	}//end TypeOfInvestment
}//end NoFuture.Rand.Gov.Bea.Parameters.Iip