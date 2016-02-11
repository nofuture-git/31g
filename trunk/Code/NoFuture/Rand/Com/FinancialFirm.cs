using System;
using System.Collections.Generic;
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
        public FinancialFirm() { }

        /// <summary>
        /// Parse from the respective line found in the <see cref="LargeCommercialBanks.RELEASE_URL"/>
        /// </summary>
        /// <param name="lrgBnkLstLine"></param>
        /// <param name="reportDate"></param>
        public FinancialFirm(string lrgBnkLstLine, DateTime reportDate)
        {
            var vals = LargeCommercialBanks.SplitLrgBnkListLine(lrgBnkLstLine).ToArray();

            Func<string[], int, string> getLnVal = (strings, i) => strings.Length >= i ? strings[i] : string.Empty;

            Name = LargeCommercialBanks.ReplaceBankAbbrev(getLnVal(vals,0));
            Rssd = new ResearchStatisticsSupervisionDiscount {Value = getLnVal(vals,2)};
            UsCityStateZip cityOut;
            if(UsCityStateZip.TryParse(vals[3], out cityOut))
                BusinessAddress = new Tuple<UsAddress, UsCityStateZip>(null, cityOut);
            if (LargeCommercialBanks.TypeOfBankAbbrev3Enum.ContainsKey(getLnVal(vals,4)))
                BankType = LargeCommercialBanks.TypeOfBankAbbrev3Enum[getLnVal(vals,4)];
            var assets = new FinancialAssets();
            Decimal conAssts = 0;
            Decimal domAssts = 0;
            if (Decimal.TryParse(getLnVal(vals,5).Replace(",", string.Empty), out conAssts))
                assets.ConsolidatedAssets = new Pecuniam(conAssts*1000);
            if (Decimal.TryParse(getLnVal(vals, 6).Replace(",", string.Empty), out domAssts))
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

        public ResearchStatisticsSupervisionDiscount Rssd { get; set; }
        public TypeOfBank BankType { get; set; }
        public bool IsInternational { get; set; }
        public Dictionary<DateTime, FinancialAssets> Assets { get; set; }
    }

    public class FinancialAssets : Assets
    {
        public double PercentForeignOwned { get; set; }
        public int DomesticBranches { get; set; }
        public int ForeignBranches { get; set; }
    }
}
