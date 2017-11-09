using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Com.NfText;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Gov.Fed;

namespace NoFuture.Rand.Com
{
    [Serializable]
    public enum TypeOfBank
    {
        NationallyChartered,
        StateChartered,
        StateCharteredNonMember
    }
    [Serializable]
    public class FinancialFirm : PublicCorporation
    {
        public FinancialFirm()
        {
            const string FIFTY_TWO = "52";
            var superSectors = NorthAmericanIndustryClassification.AllSectors;
            if (superSectors == null || superSectors.Length <= 0)
                return;

            PrimarySector =
                superSectors.SelectMany(x => x.Divisions)
                    .Cast<NaicsPrimarySector>()
                    .FirstOrDefault(x => x.Value == FIFTY_TWO);
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
    [Serializable]
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
            const string COMMA = ",";
            const string LETTER_Y = "Y";
            var unfoldedName = GetBankFullName(li.BankName);

            UpsertName(KindsOfNames.Legal, unfoldedName);
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
            if (decimal.TryParse(li.ConsolAssets.Replace(COMMA, string.Empty), out conAssts))
                assets.TotalAssets = new Pecuniam(conAssts * ONE_THOUSAND);
            if (decimal.TryParse(li.DomesticAssets.Replace(COMMA, string.Empty), out domAssts))
                assets.DomesticAssets = new Pecuniam(domAssts * ONE_THOUSAND);
            int domBranches = 0;
            int frnBranches = 0;
            int pfo = 0;
            if (int.TryParse(li.NumOfDomBranches.Replace(COMMA, string.Empty), out domBranches))
                assets.DomesticBranches = domBranches;
            if (int.TryParse(li.NumOfFgnBranches.Replace(COMMA, string.Empty), out frnBranches))
                assets.ForeignBranches = frnBranches;
            IsInternational = li.Ibf == LETTER_Y;
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

        public static string GetBankFullName(string abbrev)
        {
            var unfoldedName = Cusip.GetNameFull(abbrev);

            if (unfoldedName.EndsWith(" Na"))
            {
                //national association is part of the legal name
                unfoldedName = unfoldedName.Substring(0, unfoldedName.Length - 3) + ", N.A.";
            }
            //cusip uses COML so this one gets missed
            if (unfoldedName.Contains(" Cmrl "))
                unfoldedName = unfoldedName.Replace(" Cmrl ", " Commercial ");

            if (unfoldedName.Contains(" Cty "))
                unfoldedName = unfoldedName.Replace(" Cty ", " City ");


            if(unfoldedName.EndsWith(" Nb"))
                unfoldedName = unfoldedName.Substring(0, unfoldedName.Length - 3) + " National Bank";

            if (unfoldedName.Contains(" Nb "))
                unfoldedName = unfoldedName.Replace(" Nb ", " National Bank ");

            if (unfoldedName.Contains(" Bkg&"))
                unfoldedName = unfoldedName.Replace(" Bkg&", " Bank &");

            if(unfoldedName.EndsWith(" Tc"))
                unfoldedName = unfoldedName.Substring(0, unfoldedName.Length - 3) + " Trust Company";
            if (unfoldedName.Contains(" Tc "))
                unfoldedName = unfoldedName.Replace(" Tc ", " Trust Company ");

            if (unfoldedName.EndsWith(" B&tc"))
                unfoldedName = unfoldedName.Substring(0, unfoldedName.Length - 5) + " Bank & Trust Company";

            if (unfoldedName.Contains(" Fncl "))
                unfoldedName = unfoldedName.Replace(" Fncl ", " Financial ");

            if (unfoldedName.Contains(" Mrch "))
                unfoldedName = unfoldedName.Replace(" Mrch ", " Merchent ");

            return unfoldedName;
        }

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
