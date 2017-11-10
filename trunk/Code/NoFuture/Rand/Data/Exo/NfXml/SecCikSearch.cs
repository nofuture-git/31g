using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Gov.Sec;

namespace NoFuture.Rand.Com.NfXml
{
    public class SecCikSearch : NfDynDataBase
    {
        public SecCikSearch(Uri src):base(src) { }

        public static Uri GetUri(CentralIndexKey cik, string rptType = "10-K")
        {
            var urlByCik = new StringBuilder();

            urlByCik.Append(Edgar.EDGAR_ROOT);
            urlByCik.Append($"?action=getcompany&CIK={cik}&type={rptType}&dateb=&owner=exclude&count=100&output=atom");
            return new Uri(urlByCik.ToString());
        }

        public override IEnumerable<dynamic> ParseContent(object content)
        {
            var xmlContent = content as string;
            if (xmlContent == null)
                return null;
            const string ATOM = "atom";
            const string COMPANY_INFO = ATOM + ":company-info";

            var filingXml = new XmlDocument();
            filingXml.LoadXml(xmlContent);

            if (!filingXml.HasChildNodes)
                return null;
            var nsMgr = new XmlNamespaceManager(filingXml.NameTable);
            nsMgr.AddNamespace(ATOM, Edgar.ATOM_XML_NS);

            var cik = string.Empty;
            var sic = string.Empty;
            var name = string.Empty;
            var st = string.Empty;
            var sicDesc = string.Empty;
            var fiEnd = string.Empty;

            var cikNode = filingXml.SelectSingleNode("//" + COMPANY_INFO + "/" + ATOM + ":cik", nsMgr);
            if (cikNode != null)
                cik = cikNode.InnerText;

            var sicNode = filingXml.SelectSingleNode("//" + COMPANY_INFO + "/" + ATOM + ":assigned-sic", nsMgr);
            if (sicNode != null)
                sic = sicNode.InnerText;

            var nameNode = filingXml.SelectSingleNode("//" + COMPANY_INFO + "/" + ATOM + ":conformed-name", nsMgr);
            if (nameNode != null)
                name = nameNode.InnerText;

            var stateNode = filingXml.SelectSingleNode("//" + COMPANY_INFO + "/" + ATOM + ":state-of-incorporation",
                nsMgr);
            if (stateNode != null)
            {
                st = stateNode.InnerText;
            }
            else
            {
                stateNode = filingXml.SelectSingleNode("//" + COMPANY_INFO + "/" + ATOM + ":state-location", nsMgr);
                if (stateNode != null)
                    st = stateNode.InnerText;
            }

            var sicDescNode = filingXml.SelectSingleNode("//" + COMPANY_INFO + "/" + ATOM + ":assigned-sic-desc", nsMgr);
            if (sicDescNode != null)
                sicDesc = sicDescNode.InnerText;

            var fiEndNode = filingXml.SelectSingleNode("//" + COMPANY_INFO + "/" + ATOM + ":fiscal-year-end", nsMgr);
            if (fiEndNode != null)
                fiEnd = fiEndNode.InnerText;

            var bizAddrNode =
                filingXml.SelectSingleNode(
                    "//" + COMPANY_INFO + "/" + ATOM + ":addresses/" + ATOM + ":address[@type='business']", nsMgr);
            var mailAddrNode =
                filingXml.SelectSingleNode(
                    "//" + COMPANY_INFO + "/" + ATOM + ":addresses/" + ATOM + ":address[@type='mailing']", nsMgr);

            var bizSt = string.Empty;
            var bizCity = string.Empty;
            var bizState = string.Empty;
            var bizZip = string.Empty;
            var bizPhone = string.Empty;
            if (bizAddrNode != null && bizAddrNode.HasChildNodes)
            {
                var addrStreet =
                    filingXml.SelectSingleNode(
                        "//" + ATOM + ":company-info/" + ATOM + ":addresses/" + ATOM + ":address[@type='business']/" +
                        ATOM + ":street1", nsMgr);
                if (addrStreet != null)
                    bizSt = addrStreet.InnerText;

                var addrCity =
                    filingXml.SelectSingleNode(
                        "//" + ATOM + ":company-info/" + ATOM + ":addresses/" + ATOM + ":address[@type='business']/" +
                        ATOM + ":city", nsMgr);
                if (addrCity != null)
                    bizCity = addrCity.InnerText;

                var addrState =
                    filingXml.SelectSingleNode(
                        "//" + ATOM + ":company-info/" + ATOM + ":addresses/" + ATOM + ":address[@type='business']/" +
                        ATOM + ":state", nsMgr);
                if (addrState != null)
                    bizState = addrState.InnerText;

                var addrZip =
                    filingXml.SelectSingleNode(
                        "//" + ATOM + ":company-info/" + ATOM + ":addresses/" + ATOM + ":address[@type='business']/" +
                        ATOM + ":zip", nsMgr);
                if (addrZip != null)
                    bizZip = addrZip.InnerText;

                var addrPh =
                    filingXml.SelectSingleNode(
                        "//" + ATOM + ":company-info/" + ATOM + ":addresses/" + ATOM + ":address[@type='business']/" +
                        ATOM + ":phone", nsMgr);
                if (addrPh != null)
                    bizPhone = addrPh.InnerText;

            }
            var mailSt = string.Empty;
            var mailCity = string.Empty;
            var mailState = string.Empty;
            var mailZip = string.Empty;
            var mailPhone = string.Empty;
            if (mailAddrNode != null && mailAddrNode.HasChildNodes)
            {
                var addrStreet =
                    filingXml.SelectSingleNode(
                        "//" + ATOM + ":company-info/" + ATOM + ":addresses/" + ATOM + ":address[@type='mailing']/" +
                        ATOM + ":street1", nsMgr);
                if (addrStreet != null)
                    mailSt = addrStreet.InnerText;

                var addrCity =
                    filingXml.SelectSingleNode(
                        "//" + ATOM + ":company-info/" + ATOM + ":addresses/" + ATOM + ":address[@type='mailing']/" +
                        ATOM + ":city", nsMgr);
                if (addrCity != null)
                    mailCity = addrCity.InnerText;

                var addrState =
                    filingXml.SelectSingleNode(
                        "//" + ATOM + ":company-info/" + ATOM + ":addresses/" + ATOM + ":address[@type='mailing']/" +
                        ATOM + ":state", nsMgr);
                if (addrState != null)
                    mailState = addrState.InnerText;

                var addrZip =
                    filingXml.SelectSingleNode(
                        "//" + ATOM + ":company-info/" + ATOM + ":addresses/" + ATOM + ":address[@type='mailing']/" +
                        ATOM + ":zip", nsMgr);
                if (addrZip != null)
                    mailZip = addrZip.InnerText;

                var addrPh =
                    filingXml.SelectSingleNode(
                        "//" + ATOM + ":company-info/" + ATOM + ":addresses/" + ATOM + ":address[@type='mailing']/" +
                        ATOM + ":phone", nsMgr);
                if (addrPh != null)
                    mailPhone = addrPh.InnerText;
            }
            var formerNamesNode = filingXml.SelectSingleNode("//" + COMPANY_INFO + "/" + ATOM + ":formerly-names", nsMgr);
            var formerNames = new List<dynamic>();
            if (formerNamesNode != null && formerNamesNode.HasChildNodes)
            {
                foreach (var names in formerNamesNode.ChildNodes)
                {
                    var namesNode = names as XmlElement;
                    if (namesNode == null || !namesNode.HasChildNodes)
                        continue;

                    var formerNameValue = string.Empty;
                    var formerNameDate = string.Empty;
                    var firstChild = namesNode.FirstChild;
                    if (firstChild != null)
                    {
                        formerNameDate = firstChild.InnerText;
                    }

                    var secondChild = firstChild.NextSibling;
                    if (secondChild != null)
                    {
                        formerNameValue = secondChild.InnerText;
                    }
                    formerNames.Add(new {FormerName = formerNameValue, FormerDate = formerNameDate});
                }
            }

            return new List<dynamic>
            {
                new
                {
                    Name = name,
                    Cik = cik,
                    Sic = sic,
                    IncorpState = st,
                    SicDesc = sicDesc,
                    FiscalYearEnd = fiEnd,
                    BizAddrStreet = bizSt,
                    BizAddrCity = bizCity,
                    BizAddrState = bizState,
                    BizPostalCode = bizZip,
                    BizPhone = bizPhone,
                    MailAddrStreet = mailSt,
                    MailAddrCity = mailCity,
                    MailAddrState = mailState,
                    MailPostalCode = mailZip,
                    MailPhone = mailPhone,
                    FormerNames = formerNames
                }
            };
        }
    }
}
