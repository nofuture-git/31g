using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Data.Sp;
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

        #region properties
        public ResearchStatisticsSupervisionDiscount Rssd { get; set; }
        public RoutingTransitNumber RoutingNumber { get; set; }
        public FdicNum FdicNumber { get; set; }
        public TypeOfBank BankType { get; set; }
        #endregion

        #region methods

        /// <summary>
        /// Unfolds a bank&apos;s name from the abbreviated form.
        /// </summary>
        /// <param name="abbrev"></param>
        /// <returns></returns>
        public static string GetBankFullName(string abbrev)
        {
            var bankNameReplacements = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("Cmrl", "Commercial"),
                new Tuple<string, string>("Cty", "City"),
                new Tuple<string, string>("Nb", "National Bank"),
                new Tuple<string, string>("Bkg", "Bank"),
                new Tuple<string, string>("Bks", "Banks"),
                new Tuple<string, string>("Grp", "Group"),
                new Tuple<string, string>("Tc", "Trust Company"),
                new Tuple<string, string>("Fncl", "Financial"),
                new Tuple<string, string>("Mrch", "Merchent"),
                new Tuple<string, string>("Alli", "Allied"),
                new Tuple<string, string>("Cmrc", "Commerce"),
                new Tuple<string, string>("Ny", "New York"),
                new Tuple<string, string>("Fl", "Flordia"),
                new Tuple<string, string>("Tx", "Texas"),
                new Tuple<string, string>("Ut", "Utah"),
                new Tuple<string, string>("Ia", "Iowa"),
                new Tuple<string, string>("De", "Delaware"),
                new Tuple<string, string>("Nj", "New Jersey"),
                new Tuple<string, string>("Ca", "California"),
                new Tuple<string, string>("Az", "Arizona"),
                new Tuple<string, string>("Nc", "North Carolina"),
                new Tuple<string, string>("Wv", "West Virginia"),
                new Tuple<string, string>("Mi", "Michigan"),
                new Tuple<string, string>("Ks", "Kansas"),
                new Tuple<string, string>("Tn", "Tennessee"),
                new Tuple<string, string>("Mw", "Midwest"),
                new Tuple<string, string>("Sw", "Southwest"),
                new Tuple<string, string>("Bc", "Bancorp"),
                new Tuple<string, string>("Scty", "Security"),
                new Tuple<string, string>("Indep", "Independent"),
                new Tuple<string, string>("Usa", "USA"),
                new Tuple<string, string>("B&tc", "Bank & Trust Company"),
                new Tuple<string, string>("Nb&tc", "National Bank & Trust Company"),
                new Tuple<string, string>("Bkg&tc", "Banking & Trust Company"),
                new Tuple<string, string>("B&t", "Bank & Trust"),
            };

            var unfoldedName = Cusip.GetNameFull(abbrev);

            foreach (var pair in bankNameReplacements)
            {
                if (unfoldedName.EndsWith($" {pair.Item1}"))
                    unfoldedName = unfoldedName.Substring(0, unfoldedName.Length - $" {pair.Item1}".Length) + $" {pair.Item2}";

                if (unfoldedName.Contains($" {pair.Item1} "))
                    unfoldedName = unfoldedName.Replace($" {pair.Item1} ", $" {pair.Item2} ");

                if (unfoldedName.StartsWith($"{pair.Item1} "))
                    unfoldedName = unfoldedName.Substring(0, unfoldedName.Length - $"{pair.Item1} ".Length) + $" {pair.Item2}";

            }

            if (unfoldedName.EndsWith(" Na"))
            {
                //national association is part of the legal name
                unfoldedName = unfoldedName.Substring(0, unfoldedName.Length - 3) + ", N.A.";
            }

            return unfoldedName;
        }

        /// <summary>
        /// Gets a Bank at random first trying to match on <see cref="ca"/>, if given.
        /// </summary>
        /// <param name="ca"></param>
        /// <returns></returns>
        public static Bank GetRandomBank(CityArea ca)
        {
            var bank = new Bank();
            var bankXml = GetBankXmlElement(ca);
            if (bankXml == null)
                return bank;

            var city = bankXml.GetAttribute("city");
            var state = bankXml.GetAttribute("us-state");
            var name = bankXml.GetAttribute("name");
            var abbrev = bankXml.GetAttribute("abbrev");
            var rssd = bankXml.GetAttribute("rssd");

            bank.BusinessAddress = new Tuple<UsStreetPo, UsCityStateZip>(null,
                new UsCityStateZip(new AddressData {City = city, StateAbbrv = state}));
            bank.Rssd = new ResearchStatisticsSupervisionDiscount {Value = rssd};
            bank.UpsertName(KindsOfNames.Legal, name);
            bank.UpsertName(KindsOfNames.Abbrev, abbrev);

            bank.LoadXrefXmlData();
            return bank;
        }

        public override string ToString()
        {
            return string.Join(" ", Name, RoutingNumber);
        }

        /// <summary>
        /// Picks a single xml element at random from the <see cref="TreeData.UsBanks"/> 
        /// first attempting to match on <see cref="ca"/>, if given.
        /// </summary>
        /// <param name="ca"></param>
        /// <returns></returns>
        internal static XmlElement GetBankXmlElement(CityArea ca)
        {
            var xml = TreeData.UsBanks;
            if (xml == null)
                return null;
            XmlElement bankXmlElem = null;
            var pickFromList = new List<XmlElement>();

            if (!string.IsNullOrWhiteSpace(ca?.AddressData?.City) &&
                !string.IsNullOrWhiteSpace(ca.AddressData?.StateAbbrv))
            {
                var cityName = UsCityStateZip.FinesseCityName(ca.AddressData.City);

                var nodes = xml.SelectNodes($"//com[@us-state='{ca.AddressData.StateAbbrv.ToUpper()}']");
                if (nodes != null && nodes.Count > 0)
                {
                    foreach (var node in nodes)
                    {
                        var elem = node as XmlElement;
                        if (elem == null)
                            continue;
                        if (!elem.HasAttributes)
                            continue;
                        if (elem.GetAttribute("city") != cityName)
                            continue;
                        pickFromList.Add(elem);
                    }

                }
            }

            if (!pickFromList.Any())
            {
                var allNodes = xml.SelectNodes("//com");
                if (allNodes == null)
                    return null;
                foreach (var node in allNodes)
                {
                    var elem = node as XmlElement;
                    if (elem == null)
                        continue;
                    if (!elem.HasAttributes)
                        continue;
                    pickFromList.Add(elem);
                }
            }

            if (pickFromList.Any())
            {
                bankXmlElem = pickFromList[Etx.IntNumber(0, pickFromList.Count - 1)];
            }
            return bankXmlElem;
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
