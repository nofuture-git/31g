﻿using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Gov.Fed;

namespace NoFuture.Rand.Com
{
    public enum TypeOfBank
    {
        NationallyChartered,
        StateChartered,
        StateCharteredNonMember
    }
    /*financial institution 
  commercial bank, savings bank, card issuer, industrial loan company, trust company, savings associations, building and loan, homestead association,
 * cooperative banks, credit union, consumer finance institution
 */
    public class FinancialFirm : PublicCorporation
    {
        public FinancialFirm()
        {
            var superSectors = NorthAmericanIndustryClassification.AllSectors;
            if (superSectors == null || superSectors.Length <= 0)
                return;

            SuperSector = superSectors.FirstOrDefault(x => x.Value == "52");
        }

        public ResearchStatisticsSupervisionDiscount Rssd { get; set; }
        public RoutingTransitNumber RoutingNumber { get; set; }
        public TypeOfBank BankType { get; set; }
        public bool IsInternational { get; set; }
        public Dictionary<DateTime, FinancialAssets> Assets { get; set; }
    }

    /// <summary>
    /// This is a type based on the data released by the Federal Reserve <see cref="LargeCommercialBanks.RELEASE_URL"/>
    /// </summary>
    public class Bank : FinancialFirm
    {
        
        /// <summary>
        /// Ctor is based on single line from the fed's text report
        /// </summary>
        /// <param name="lrgBnkLstLine"></param>
        /// <param name="reportDate"></param>
        public Bank(string lrgBnkLstLine, DateTime reportDate)
        {
            //single line from the report
            var vals = LargeCommercialBanks.SplitLrgBnkListLine(lrgBnkLstLine).ToArray();

            Func<string[], int, string> getLnVal = (strings, i) => strings.Length >= i ? strings[i] : string.Empty;

            Rssd = new ResearchStatisticsSupervisionDiscount {Value = getLnVal(vals,2)};
            UsCityStateZip cityOut;
            if(UsCityStateZip.TryParse(vals[3], out cityOut))
                BusinessAddress = new Tuple<UsAddress, UsCityStateZip>(null, cityOut);
            if (LargeCommercialBanks.TypeOfBankAbbrev3Enum.ContainsKey(getLnVal(vals,4)))
                BankType = LargeCommercialBanks.TypeOfBankAbbrev3Enum[getLnVal(vals,4)];
            var assets = new FinancialAssets();
            decimal conAssts = 0;
            decimal domAssts = 0;
            if (decimal.TryParse(getLnVal(vals,5).Replace(",", string.Empty), out conAssts))
                assets.ConsolidatedAssets = new Pecuniam(conAssts*1000);
            if (decimal.TryParse(getLnVal(vals, 6).Replace(",", string.Empty), out domAssts))
                assets.DomesticAssets = new Pecuniam(domAssts*1000);
            int domBranches = 0;
            int frnBranches = 0;
            int pfo = 0;
            if (int.TryParse(getLnVal(vals,9).Replace(",",string.Empty), out domBranches))
                assets.DomesticBranches = domBranches;
            if (int.TryParse(getLnVal(vals, 10).Replace(",", string.Empty), out frnBranches))
                assets.ForeignBranches = frnBranches;
            IsInternational = getLnVal(vals,11) == "Y";
            if (int.TryParse(getLnVal(vals,12), out pfo))
                assets.PercentForeignOwned =  Math.Round((double)pfo / 100 ,2);
            Assets = new Dictionary<DateTime, FinancialAssets> {{reportDate, assets}};
        }
    }

    public class FinancialAssets : Assets
    {
        public double PercentForeignOwned { get; set; }
        public int DomesticBranches { get; set; }
        public int ForeignBranches { get; set; }
    }
}
