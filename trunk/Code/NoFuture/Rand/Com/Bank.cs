using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Geo.US;
using NoFuture.Rand.Gov.US.Fed;

namespace NoFuture.Rand.Com
{
    /// <summary>
    /// Represent a bank which is under the auspices of the US Federal Reserve
    /// </summary>
    [Serializable]
    public class Bank : FinancialFirm
    {
        internal const string US_BANKS = "US_Banks.xml";
        internal static XmlDocument UsBanksXml;

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

            var unfoldedName = GetNameFull(abbrev);

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
        [RandomFactory]
        public static Bank RandomBank(CityArea ca = null)
        {
            var bank = new Bank();
            ca = ca ?? CityArea.RandomAmericanCity();
            var bankXml = GetBankXmlElement(ca);
            if (bankXml == null)
                return bank;

            var city = bankXml.GetAttribute("city");
            var state = bankXml.GetAttribute("us-state");
            var name = bankXml.GetAttribute("name");
            var abbrev = bankXml.GetAttribute("abbrev");
            var rssd = bankXml.GetAttribute("rssd");

            bank.BusinessAddress = new PostalAddress
            {
                CityArea = new UsCityStateZip(new AddressData {City = city, StateAbbrev = state})
            };
            bank.Rssd = new ResearchStatisticsSupervisionDiscount {Value = rssd};
            bank.RoutingNumber = RoutingTransitNumber.RandomRoutingNumber();
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
        /// Picks a single xml element at random from the US Banks data file 
        /// first attempting to match on <see cref="ca"/>, if given.
        /// </summary>
        /// <param name="ca"></param>
        /// <returns></returns>
        internal static XmlElement GetBankXmlElement(CityArea ca)
        {
            UsBanksXml = UsBanksXml ?? XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_BANKS, Assembly.GetExecutingAssembly());
            if (UsBanksXml == null)
                return null;
            XmlElement bankXmlElem = null;
            var pickFromList = new List<XmlElement>();

            if (!string.IsNullOrWhiteSpace(ca?.AddressData?.City) &&
                !string.IsNullOrWhiteSpace(ca.AddressData?.StateAbbrev))
            {
                var cityName = UsCityStateZip.FinesseCityName(ca.AddressData.City);

                var nodes = UsBanksXml.SelectNodes($"//com[@us-state='{ca.AddressData.StateAbbrev.ToUpper()}']");
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
                var allNodes = UsBanksXml.SelectNodes("//com");
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
                bankXmlElem = pickFromList[Etx.RandomInteger(0, pickFromList.Count - 1)];
            }
            return bankXmlElem;
        }
        #endregion
    }
}