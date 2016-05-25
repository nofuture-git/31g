using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NoFuture.Rand.Com;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Rand.Gov.Sec
{
    public class Edgar
    {
        #region constants
        public const string SEC_ROOT_URL = "http://" + SEC_HOST;
        public const string SEC_HOST = "www.sec.gov";
        public const string EDGAR_ROOT = SEC_ROOT_URL + "/cgi-bin/browse-edgar";
        public const string INTERACTIVE_ROOT = SEC_ROOT_URL + "/cgi-bin/viewer";
        public const string ARCHIVE_ROOT = SEC_ROOT_URL + "/Archives/edgar/data/";
        public const string ATOM_XML_NS = "http://www.w3.org/2005/Atom";
        #endregion

        #region inner types
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
        #endregion

        #region methods
        public static Uri GetUriFullTextSearch(FullTextSearch fts)
        {
            if (string.IsNullOrWhiteSpace(fts.ToString()))
                return null;
            var urlFullText = new StringBuilder();

            urlFullText.Append(SEC_ROOT_URL);
            urlFullText.AppendFormat("/cgi-bin/srch-edgar?text={0}", fts);
            urlFullText.AppendFormat("&first={0}&last={1}&output=atom", DateTime.Today.Year - 1, DateTime.Today.Year);
            return new Uri(urlFullText.ToString());
        }

        public static Uri GetUriCompanyNameSearch(string companyName)
        {
            if (string.IsNullOrWhiteSpace(companyName))
                return null;
            companyName = companyName.Trim();
            var urlbyName = new StringBuilder();
            urlbyName.Append(EDGAR_ROOT);
            urlbyName.AppendFormat("?company={0}&owner=exclude&action=getcompany&output=atom", HttpUtility.UrlEncode(companyName));
            return new Uri(urlbyName.ToString());
        }

        public static Uri GetUriCikSearch(CentralIndexKey cik)
        {
            var urlByCik = new StringBuilder();
            
            urlByCik.Append(EDGAR_ROOT);
            urlByCik.AppendFormat("?action=getcompany&CIK={0}&type=10-K&dateb=&owner=exclude&count=100&output=atom",
                cik);
            return new Uri(urlByCik.ToString());

        }

        public static PublicCorporation[] ParseFullTextSearch(string rssContent, Uri srcUri)
        {
            if (string.IsNullOrWhiteSpace(rssContent))
                return null;

            var myDynData = Etx.DynamicDataFactory(srcUri);
            var myDynDataRslts = myDynData.ParseContent(rssContent);

            if (myDynDataRslts == null || myDynDataRslts.Count <= 0)
                return null;

            var corporations = new List<PublicCorporation>();
            foreach(var dd in myDynDataRslts)
            {

                var titleNode = dd.Title;

                if (titleNode == null || string.IsNullOrWhiteSpace(titleNode))
                    continue;

                var idNode = dd.Id;
                var linkVal = dd.Link;
                var form10KDtNode = dd.Update;
                var summaryNode = dd.Summary;

                //this will be an id while looping
                var corpName = ParseNameFromTitle(titleNode);

                var corp = corporations.Any(x => string.Equals(x.Name, corpName, StringComparison.OrdinalIgnoreCase))
                    ? corporations.First(
                        x => string.Equals(x.Name, corpName, StringComparison.OrdinalIgnoreCase))
                    : new PublicCorporation();

                corp.Name = corpName;

                //annual report
                var form10K = new Form10K();

                if (titleNode.StartsWith(SecForm.NotificationOfInabilityToTimelyFile))
                    form10K.IsLate = true;

                var parseRslt = DateTime.Now;
                if (form10KDtNode != null && !string.IsNullOrWhiteSpace(form10KDtNode) &&
                    DateTime.TryParse(form10KDtNode.ToString(), out parseRslt))
                {
                    form10K.FilingDate = parseRslt;
                }

                form10K.HtmlFormLink = new Uri(SEC_ROOT_URL + linkVal);
                //this is only presented within the html link s
                corp.CIK = new CentralIndexKey { Value = idNode == string.Empty ? string.Empty : ParseCikFromUriPath(linkVal) };

                if (summaryNode != null && !string.IsNullOrWhiteSpace(summaryNode))
                {
                    var summaryText = summaryNode;
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

        public static bool TryParseCorpData(string xmlContent, Uri srcUri, ref PublicCorporation publicCorporation)
        {
            if (string.IsNullOrWhiteSpace(xmlContent))
                return false;

            var myDynData = Etx.DynamicDataFactory(srcUri);
            var myDynDataRslt = myDynData.ParseContent(xmlContent);

            if (myDynDataRslt == null || myDynDataRslt.Count <= 0)
                return false;

            var pr = myDynDataRslt.First();

            if(publicCorporation == null)
                publicCorporation = new PublicCorporation();

            publicCorporation.Name = string.IsNullOrWhiteSpace(pr.Name) ? publicCorporation.Name : pr.Name;
            publicCorporation.CIK = string.IsNullOrWhiteSpace(pr.Cik) ? publicCorporation.CIK : new CentralIndexKey { Value = pr.Cik };
            publicCorporation.SIC = string.IsNullOrWhiteSpace(pr.Sic)
                ? publicCorporation.SIC
                : new StandardIndustryClassification { Value = pr.Sic };

            publicCorporation.UsStateOfIncorporation = string.IsNullOrWhiteSpace(pr.IncorpState)
                ? publicCorporation.UsStateOfIncorporation
                : UsState.GetStateByPostalCode(pr.IncorpState);

            if (publicCorporation.SIC != null && !string.IsNullOrWhiteSpace(pr.SicDesc))
                publicCorporation.SIC.Description = pr.SicDesc;

            var bizAddr = new AddressData();
            if (!string.IsNullOrWhiteSpace(pr.BizAddrStreet))
            {
                UsAddress temp;
                if (UsAddress.TryParse(pr.BizAddrStreet, out temp))
                    bizAddr = temp.Data;
            }

            if (!string.IsNullOrWhiteSpace(pr.BizAddrCity))
                bizAddr.City = pr.BizAddrCity;

            if (!string.IsNullOrWhiteSpace(pr.BizAddrState))
                bizAddr.StateAbbrv = pr.BizAddrState;

            if (!string.IsNullOrWhiteSpace(pr.BizPostalCode))
                bizAddr.PostalCode = pr.BizPostalCode;

            publicCorporation.BusinessAddress = new Tuple<UsAddress, UsCityStateZip>(new UsAddress(bizAddr),
                new UsCityStateZip(bizAddr));

            var mailAddr = new AddressData();
            if (!string.IsNullOrWhiteSpace(pr.MailAddrStreet))
            {
                UsAddress temp;
                if (UsAddress.TryParse(pr.MailAddrStreet, out temp))
                    mailAddr = temp.Data;
            }

            if (!string.IsNullOrWhiteSpace(pr.MailAddrCity))
                mailAddr.City = pr.MailAddrCity;

            if (!string.IsNullOrWhiteSpace(pr.MailAddrState))
                mailAddr.StateAbbrv = pr.MailAddrState;

            if (!string.IsNullOrWhiteSpace(pr.MailPostalCode))
                mailAddr.PostalCode = pr.MailPostalCode;

            publicCorporation.MailingAddress = new Tuple<UsAddress, UsCityStateZip>(new UsAddress(mailAddr),
                new UsCityStateZip(mailAddr));

            var phs = new List<NorthAmericanPhone>();
            if (publicCorporation.Phone != null && publicCorporation.Phone.Length > 0)
            {
                phs.AddRange(publicCorporation.Phone);
            }
            NorthAmericanPhone phOut;
            if (NorthAmericanPhone.TryParse(pr.BizPhone, out phOut) && phs.All(x => !x.Equals(phOut)))
            {
                phs.Add(phOut);
            }

            if (NorthAmericanPhone.TryParse(pr.MailPhone, out phOut) && phs.All(x => !x.Equals(phOut)))
            {
                phs.Add(phOut);
            }
            publicCorporation.Phone = phs.ToArray();
            return true;
        }
        #endregion

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
