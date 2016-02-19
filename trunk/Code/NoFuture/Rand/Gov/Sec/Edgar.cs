using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;
using NoFuture.Rand.Com;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Rand.Gov.Sec
{
    public class Edgar
    {
        public const string SEC_ROOT_URL = "http://www.sec.gov";
        public const string EDGAR_ROOT = SEC_ROOT_URL + "/cgi-bin/browse-edgar";
        public const string INTERACTIVE_ROOT = SEC_ROOT_URL + "/cgi-bin/viewer";
        public const string ARCHIVE_ROOT = SEC_ROOT_URL + "/Archives/edgar/data/";
        public const string ATOM_XML_NS = "http://www.w3.org/2005/Atom";

        /// <summary>
        /// see [http://www.sec.gov/edgar/searchedgar/edgarzones.htm]
        /// </summary>
        public struct FullTextSearch
        {
            public string SicCode;
            public string CompanyName;
            public string ZipCode;
            public string StateOfIncorporation;
            public string BizAddrState;

            public override string ToString()
            {
                if((new []{SicCode, CompanyName, ZipCode, StateOfIncorporation, BizAddrState}).All(string.IsNullOrWhiteSpace))
                    return string.Empty;
                var searchList = new List<string>();
                if(!string.IsNullOrWhiteSpace(SicCode))
                    searchList.Add(HttpUtility.UrlEncode(string.Format("ASSIGNED-SIC={0}", SicCode.Trim())));
                searchList.Add(HttpUtility.UrlEncode("FORM-TYPE=10-K"));
                if (!string.IsNullOrWhiteSpace(CompanyName))
                {
                    searchList.Add(CompanyName.ToCharArray().All(char.IsLetterOrDigit)
                        ? HttpUtility.UrlEncode(string.Format("COMPANY-NAME={0}", CompanyName.Trim()))
                        : string.Format("COMPANY-NAME=%22{0}%22", HttpUtility.UrlEncode(CompanyName.Trim(),Encoding.GetEncoding("ISO-8859-1"))));
                }
                if(!string.IsNullOrWhiteSpace(ZipCode))
                    searchList.Add(HttpUtility.UrlEncode(string.Format("ZIP={0}", ZipCode.Trim())));
                if(!string.IsNullOrWhiteSpace(StateOfIncorporation))
                    searchList.Add(HttpUtility.UrlEncode(string.Format("STATE-OF-INCORPORATION={0}", StateOfIncorporation.Trim())));
                if(!string.IsNullOrWhiteSpace(BizAddrState))
                    searchList.Add(HttpUtility.UrlEncode(string.Format("STATE={0}", BizAddrState.Trim())));
                return string.Join("+AND+", searchList);
            }
        }

        public static Uri UrlFullTextSearchBySic(FullTextSearch fts)
        {
            if (string.IsNullOrWhiteSpace(fts.ToString()))
                return null;
            var urlFullText = new StringBuilder();

            urlFullText.Append(SEC_ROOT_URL);
            urlFullText.AppendFormat("/cgi-bin/srch-edgar?text={0}", fts);
            urlFullText.AppendFormat("&first={0}&last={1}&output=atom", DateTime.Today.Year - 1, DateTime.Today.Year);
            return new Uri(urlFullText.ToString());
        }

        public static Uri UrlCorpDataByExactName(string companyName)
        {
            if (string.IsNullOrWhiteSpace(companyName))
                return null;
            companyName = companyName.Trim();
            var urlbyName = new StringBuilder();
            urlbyName.Append(EDGAR_ROOT);
            urlbyName.AppendFormat("?company={0}&owner=exclude&action=getcompany&output=atom", HttpUtility.UrlEncode(companyName));
            return new Uri(urlbyName.ToString());
        }

        public static Uri UrlCompanyList10KFilings(CentralIndexKey cik)
        {
            var urlByCik = new StringBuilder();
            
            urlByCik.Append(EDGAR_ROOT);
            urlByCik.AppendFormat("?action=getcompany&CIK={0}&type=10-K&dateb=&owner=exclude&count=100&output=atom",
                cik);
            return new Uri(urlByCik.ToString());

        }

        public static PublicCorporation[] ParseCompanyFullTextSearch(string rssContent)
        {
            if (string.IsNullOrWhiteSpace(rssContent))
                return null;

            for (var i = 0; i < 9; i++)
                rssContent = rssContent.Replace(string.Format("<{0} ", i), "<val ");

            var rssXml = new XmlDocument();
            rssXml.LoadXml(rssContent);

            if (!rssXml.HasChildNodes)
                return null;

            var nsMgr = new XmlNamespaceManager(rssXml.NameTable);
            nsMgr.AddNamespace("atom", ATOM_XML_NS);

            var entries = rssXml.SelectNodes("//atom:entry", nsMgr);

            if (entries == null || entries.Count <= 0)
                return null;

            var corporations = new List<PublicCorporation>();
            for (var i = 0; i < entries.Count; i++)
            {

                var xpathRoot = string.Format("//atom:entry[{0}]", i);

                var entry = entries.Item(i);

                if (entry == null)
                    continue;

                var titleNode = entry.SelectSingleNode(xpathRoot + "/atom:title", nsMgr);

                if (titleNode == null || string.IsNullOrWhiteSpace(titleNode.InnerText))
                    continue;

                var idNode = entry.SelectSingleNode(xpathRoot + "/atom:id", nsMgr);
                var linkNode = entry.SelectSingleNode(xpathRoot + "/atom:link", nsMgr);
                var form10KDtNode = entry.SelectSingleNode(xpathRoot + "/atom:updated", nsMgr);
                var summaryNode = entry.SelectSingleNode(xpathRoot + "/atom:summary", nsMgr);

                //this will be an id while looping
                var corpName = ParseNameFromTitle(titleNode.InnerText);

                var corp = corporations.Any(x => string.Equals(x.Name, corpName, StringComparison.OrdinalIgnoreCase)) ? corporations.First(
                    x => string.Equals(x.Name, corpName, StringComparison.OrdinalIgnoreCase)) : new PublicCorporation();

                corp.Name = corpName;

                //annual report
                var form10K = new Form10K();

                if (titleNode.InnerText.StartsWith(SecForm.NotificationOfInabilityToTimelyFile))
                    form10K.IsLate = true;

                DateTime parseRslt;
                if (form10KDtNode != null && !string.IsNullOrWhiteSpace(form10KDtNode.InnerText) && DateTime.TryParse(form10KDtNode.InnerText, out parseRslt))
                {
                    form10K.FilingDate = parseRslt;
                }

                if (linkNode != null && linkNode.Attributes != null && linkNode.Attributes["href"] != null)
                {
                    var linkVal = linkNode.Attributes["href"].Value;
                    form10K.HtmlFormLink = new Uri(SEC_ROOT_URL + linkVal);
                    //this is only presented within the html link s
                    corp.CIK = new CentralIndexKey() { Value = idNode == null ? string.Empty : ParseCikFromUriPath(linkVal) };
                }
                

                if (summaryNode != null && !string.IsNullOrWhiteSpace(summaryNode.InnerText))
                {
                    var summaryText = summaryNode.InnerText;
                    form10K.AccessionNumber = ParseAccessionNumFromSummary(summaryText);
                    form10K.InteractiveFormLink = CtorInteractiveLink(corp.CIK.Value, form10K.AccessionNumber);
                    form10K.XbrlZipLink = CtorXbrlZipLink(corp.CIK.Value, form10K);
                }

                corp.AnnualReports.Add(form10K);
                if (!corporations.Any(x => string.Equals(x.Name, corpName, StringComparison.OrdinalIgnoreCase)))
                    corporations.Add(corp);
            }
            return corporations.ToArray();
            
        }

        public static PublicCorporation SelectRandomCorporation(string rssContent)
        {
            if(string.IsNullOrWhiteSpace(rssContent))
                return null;

            var companies = ParseCompanyFullTextSearch(rssContent);

            if (companies == null || companies.Length <= 0)
                return null;

            var pickOne = Etx.MyRand.Next(0, companies.Length);
            return companies[pickOne];

        }

        public static bool TryGetCorpData(string xmlContent, ref PublicCorporation publicCorporation)
        {
            const string ATOM = "atom";
            const string COMPANY_INFO = ATOM + ":company-info";
            if (string.IsNullOrWhiteSpace(xmlContent))
                return false;
            if(publicCorporation == null)
                publicCorporation = new PublicCorporation();

            var filingXml = new XmlDocument();
            filingXml.LoadXml(xmlContent);

            if (!filingXml.HasChildNodes)
                return false;
            var nsMgr = new XmlNamespaceManager(filingXml.NameTable);
            nsMgr.AddNamespace("atom", ATOM_XML_NS);


            var cikNode = filingXml.SelectSingleNode("//" + COMPANY_INFO + "/" + ATOM + ":cik", nsMgr);
            var sicNode = filingXml.SelectSingleNode("//" + COMPANY_INFO + "/" + ATOM + ":assigned-sic", nsMgr);
            var nameNode = filingXml.SelectSingleNode("//" + COMPANY_INFO + "/" + ATOM + ":conformed-name", nsMgr);

            publicCorporation.Name = nameNode == null ? publicCorporation.Name : nameNode.InnerText;
            publicCorporation.CIK = cikNode == null ? publicCorporation.CIK : new CentralIndexKey {Value = cikNode.InnerText};
            publicCorporation.SIC = sicNode == null
                ? publicCorporation.SIC
                : new StandardIndustryClassification {Value = sicNode.InnerText};

            var stateNode = filingXml.SelectSingleNode("//" + COMPANY_INFO + "/" + ATOM + ":state-of-incorporation", nsMgr);

            publicCorporation.UsStateOfIncorporation = stateNode == null
                ? publicCorporation.UsStateOfIncorporation
                : UsState.GetStateByPostalCode(stateNode.InnerText);

            var sicDescNode = filingXml.SelectSingleNode("//" + COMPANY_INFO + "/" + ATOM + ":assigned-sic-desc", nsMgr);
            if (publicCorporation.SIC != null && sicDescNode != null)
                publicCorporation.SIC.Description = sicDescNode.InnerText;

            var bizAddrNode = filingXml.SelectSingleNode("//" + COMPANY_INFO + "/" + ATOM + ":addresses/" + ATOM + ":address[@type='business']", nsMgr);
            var mailAddrNode = filingXml.SelectSingleNode("//" + COMPANY_INFO + "/" + ATOM + ":addresses/" + ATOM + ":address[@type='mailing']", nsMgr);

            Func<string, Tuple<UsAddress, UsCityStateZip>> resolveAddr =
                c =>
                {
                    var addrData = new AddressData();
                    var addrStreet = filingXml.SelectSingleNode(string.Format("//" + COMPANY_INFO + "/{0}/" + ATOM + ":street1", c), nsMgr);
                    if (addrStreet != null)
                    {
                        UsAddress temp;
                        if (UsAddress.TryParse(addrStreet.InnerText, out temp))
                            addrData = temp.Data;
                    }

                    var addrCity = filingXml.SelectSingleNode(string.Format("//" + COMPANY_INFO + "/{0}/" + ATOM + ":city", c), nsMgr);
                    if (addrCity != null)
                        addrData.City = addrCity.InnerText;

                    var addrState = filingXml.SelectSingleNode(string.Format("//" + COMPANY_INFO + "/{0}/" + ATOM + ":state", c), nsMgr);
                    if (addrState != null)
                        addrData.StateAbbrv = addrState.InnerText;

                    var addrZip = filingXml.SelectSingleNode(string.Format("//" + COMPANY_INFO + "/{0}/" + ATOM + ":zip", c), nsMgr);
                    if (addrZip != null)
                        addrData.PostalCode = addrZip.InnerText;

                    return new Tuple<UsAddress, UsCityStateZip>(new UsAddress(addrData),
                        new UsCityStateZip(addrData));
                };

            Func<string, NorthAmericanPhone> resolvePhone = ph =>
            {
                NorthAmericanPhone phoneOut;
                NorthAmericanPhone.TryParse(ph, out phoneOut);
                return phoneOut;
            };

            if (bizAddrNode != null && bizAddrNode.HasChildNodes)
            {
                publicCorporation.BusinessAddress = resolveAddr(ATOM + ":addresses/" + ATOM + ":address[@type='business']");
                var phNode = filingXml.SelectSingleNode("//" + COMPANY_INFO + "/" + ATOM + ":addresses/" + ATOM + ":address[@type='business']/" + ATOM + ":phone", nsMgr);
                NorthAmericanPhone phoneOut;
                if (phNode != null && NorthAmericanPhone.TryParse(phNode.InnerText, out phoneOut))
                    publicCorporation.Phone = new[] {phoneOut};
            }
            if (mailAddrNode != null && mailAddrNode.HasChildNodes)
            {
                publicCorporation.MailingAddress = resolveAddr(ATOM + ":addresses/" + ATOM + ":address[@type='mailing']");
            }

            return true;
        }

        #region helper methods

        internal static Uri CtorInteractiveLink(string cik, string accessionNum)
        {
            var qry = new StringBuilder();
            qry.Append("?action=view&");
            qry.AppendFormat("cik={0}&", cik);
            qry.AppendFormat("accession_number={0}&", accessionNum);
            qry.Append("xbrl_type=v");

            return new Uri(INTERACTIVE_ROOT + qry);
        }

        internal static Uri CtorXbrlZipLink(string cik, Form10K form10K)
        {
            return new Uri(ARCHIVE_ROOT + cik + "/" + form10K.AccessionNumber + "/" + form10K.FormattedAccessionNumber + "-xbrl.zip");
        }

        internal static string ParseAccessionNumFromSummary(string summaryText)
        {
            if (string.IsNullOrWhiteSpace(summaryText))
                return string.Empty;

            if (!summaryText.Contains("<b>"))
                return string.Empty;

            var summaryParts = summaryText.Split(new[] { "<b>" }, StringSplitOptions.RemoveEmptyEntries);
            if (!summaryParts.Any(x => x.Contains("Accession Number")))
                return string.Empty;

            var accessiontext = summaryParts.First(x => x.Contains("Accession Number"));
            if (!accessiontext.Contains(">"))
                return string.Empty;
            summaryParts = accessiontext.Split('>');
            if (summaryParts.Length <= 1)
                return string.Empty;
            return summaryParts[1].Trim();

        }

        internal static string ParseNameFromTitle(string titleText)
        {
            if (string.IsNullOrWhiteSpace(titleText))
                return string.Empty;
            if (!titleText.Contains("-"))
                return string.Empty;
            var titleParts = titleText.Split('-');
            if (titleParts.Length <= 2)
                return string.Empty;
            var name = titleParts[2];
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;
            return name.Trim();
        }
        internal static string ParseCikFromUriPath(string htmlLink)
        {
            if (string.IsNullOrWhiteSpace(htmlLink) || !htmlLink.Contains("/"))
                return string.Empty;

            var s = htmlLink.Split(new[] {"/"}, StringSplitOptions.RemoveEmptyEntries);
            return s.Length <= 3 ? string.Empty : s[3];
        }
        #endregion

    }
}
