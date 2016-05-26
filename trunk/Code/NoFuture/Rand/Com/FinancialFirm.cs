using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Data.NfText;
using NoFuture.Rand.Data.Sp;
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
/* commercial bank, savings bank, card issuer, industrial loan company, trust company, savings associations, building and loan, homestead association,
 * cooperative banks, credit union, consumer finance institution */
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
    /// Represent a bank which is under the auspices of the US Federal Reserve
    /// </summary>
    public class Bank : FinancialFirm
    {
        public Bank() { }

        /// <summary>
        /// Ctor is based on single line from the fed's text report
        /// </summary>
        /// <param name="li"></param>
        internal Bank(dynamic li)
        {
            Name = li.BankName;
            Rssd = new ResearchStatisticsSupervisionDiscount { Value = li.BankId };
            UsCityStateZip cityOut;
            if (UsCityStateZip.TryParse(li.Location, out cityOut))
                BusinessAddress = new Tuple<UsAddress, UsCityStateZip>(null, cityOut);
            if (FedLrgBnk.TypeOfBankAbbrev3Enum.ContainsKey(li.Chtr))
                BankType = FedLrgBnk.TypeOfBankAbbrev3Enum[li.Chtr];
            var assets = new FinancialAssets { Src = FedLrgBnk.RELEASE_URL };
            decimal conAssts = 0;
            decimal domAssts = 0;
            if (decimal.TryParse(li.ConsolAssets.Replace(",", string.Empty), out conAssts))
                assets.TotalAssets = new Pecuniam(conAssts * 1000);
            if (decimal.TryParse(li.DomesticAssets.Replace(",", string.Empty), out domAssts))
                assets.DomesticAssets = new Pecuniam(domAssts * 1000);
            int domBranches = 0;
            int frnBranches = 0;
            int pfo = 0;
            if (int.TryParse(li.NumOfDomBranches.Replace(",", string.Empty), out domBranches))
                assets.DomesticBranches = domBranches;
            if (int.TryParse(li.NumOfFgnBranches.Replace(",", string.Empty), out frnBranches))
                assets.ForeignBranches = frnBranches;
            IsInternational = li.Ibf == "Y";
            if (int.TryParse(li.PercentFgnOwned, out pfo))
                assets.PercentForeignOwned = Math.Round((double)pfo / 100, 2);
            Assets = new Dictionary<DateTime, FinancialAssets> { { li.RptDate, assets } };
        }
    }

    public class FinancialAssets : Assets
    {
        public double PercentForeignOwned { get; set; }
        public int DomesticBranches { get; set; }
        public int ForeignBranches { get; set; }
    }
}
