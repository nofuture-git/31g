using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Data;
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
    public class FinancialFirm : PublicCorporation
    {
        public FinancialFirm()
        {
            var superSectors = NorthAmericanIndustryClassification.AllSectors;
            if (superSectors == null || superSectors.Length <= 0)
                return;

            PrimarySector =
                superSectors.SelectMany(x => x.Divisions)
                    .Cast<NaicsPrimarySector>()
                    .FirstOrDefault(x => x.Value == "52");
        }

        public bool IsInternational { get; set; }
        public Dictionary<DateTime, FinancialAssets> Assets { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Represent a bank which is under the auspices of the US Federal Reserve
    /// </summary>
    public class Bank : FinancialFirm
    {
        #region ctor
        public Bank() { }

        /// <summary>
        /// Ctor is based on single line from the fed's text report
        /// </summary>
        /// <param name="li"></param>
        internal Bank(dynamic li)
        {
            UpsertName(KindsOfNames.Legal, li.BankName);
            UpsertName(KindsOfNames.Abbrev, li.BankName);
            Rssd = new ResearchStatisticsSupervisionDiscount { Value = li.BankId };
            UsCityStateZip cityOut;
            if (UsCityStateZip.TryParse(li.Location, out cityOut))
                BusinessAddress = new Tuple<UsStreetPo, UsCityStateZip>(null, cityOut);
            if (FedLrgBnk.TypeOfBankAbbrev3Enum.ContainsKey(li.Chtr))
                BankType = FedLrgBnk.TypeOfBankAbbrev3Enum[li.Chtr];
            var assets = new FinancialAssets { Src = FedLrgBnk.RELEASE_URL };
            decimal conAssts = 0;
            decimal domAssts = 0;
            if (decimal.TryParse(li.ConsolAssets.Replace(",", string.Empty), out conAssts))
                assets.TotalAssets = new Pecuniam(conAssts * ONE_THOUSAND);
            if (decimal.TryParse(li.DomesticAssets.Replace(",", string.Empty), out domAssts))
                assets.DomesticAssets = new Pecuniam(domAssts * ONE_THOUSAND);
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
        #endregion

        #region properties
        public ResearchStatisticsSupervisionDiscount Rssd { get; set; }
        public RoutingTransitNumber RoutingNumber { get; set; }
        public FdicNum FdicNumber { get; set; }
        public TypeOfBank BankType { get; set; }
        #endregion

        #region methods
        public static Bank GetRandomBank(CityArea ca)
        {
            Bank bank = null;

            var banks = TreeData.CommercialBankData;
            if (ca?.AddressData != null)
            {
                var stateCode = ca.AddressData.StateAbbrv;
                var cityName = ca.AddressData.City;

                var banksByState =
                    banks.Where(x => x.BusinessAddress?.Item2?.AddressData?.StateAbbrv == stateCode).ToArray();
                var banksByCityState =
                    banksByState.Where(x => x.BusinessAddress?.Item2?.AddressData?.City == cityName).ToArray();

                if (banksByCityState.Any())
                    banks = banksByCityState;
                else if (banksByState.Any())
                    banks = banksByState;
            }

            if (banks.Any())
            {
                var pickOne = Etx.IntNumber(0, banks.Length - 1);
                bank = banks[pickOne];
                bank.LoadXrefXmlData();
                if (bank.RoutingNumber == null)
                    bank.RoutingNumber = RoutingTransitNumber.RandomRoutingNumber();
            }
            return bank;
        }
        public override string ToString()
        {
            return string.Join(" ", Name, RoutingNumber);
        }
        #endregion
    }

    public class FinancialAssets : NetConAssets
    {
        public double PercentForeignOwned { get; set; }
        public int DomesticBranches { get; set; }
        public int ForeignBranches { get; set; }
    }
}
