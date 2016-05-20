using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Rand.Com
{
    /// <summary>
    /// Represents a publicly traded corporation
    /// </summary>
    /// <example>
    /// <![CDATA[
    ///  #example in PowerShell
    /// 
    ///  #get a sic code 
    ///  $sic = [NoFuture.Rand.Data.Types.StandardIndustryClassification]::RandomSic();
    /// 
    ///  #get corps by sic code
    ///  $searchCrit = New-Object NoFuture.Rand.Gov.Sec.Edgar+FullTextSearch
    ///  $searchCrit.SicCode = $sic.Value
    ///  $rssContent = Request-File -Url ([NoFuture.Rand.Gov.Sec.Edgar]::GetUriFullTextSearch($searchCrit))
    /// 
    ///  #pick one
    ///  $randomCorp = $publicCorps[3]
    /// 
    ///  #get details from SEC
    ///  $xmlContent = Request-File -Url ([NoFuture.Rand.Gov.Sec.Edgar]::GetUriCikSearch($randomCorp.CIK))
    ///  [NoFuture.Rand.Gov.Sec.Edgar]::TryParseCorpData($xmlContent, [ref] $randomCorp)
    /// 
    ///  #get more details from yahoo finance
    ///  $yahooData = Request-File -Url ([NoFuture.Rand.Com.PublicCorporation]::GetUriTickerSymbolLookup($randomCorp.Name))
    ///  [NoFuture.Rand.Com.PublicCorporation]::MergeTickerLookupFromJson($yahooData, [ref] $randomCorp)
    /// 
    ///  #get any data you defined in the local XRef.xml
    ///  $randomCorp.GetXrefXmlData()
    /// ]]>
    /// </example>
    [Serializable]
    public class PublicCorporation : Firm
    {
        #region fields
        private readonly List<Gov.Sec.Form10K> _annualReports = new List<Gov.Sec.Form10K>();
        #endregion

        #region properties
        public Gov.Irs.EmployerIdentificationNumber EIN { get; set; }

        public Gov.Sec.CentralIndexKey CIK { get; set; }

        public List<Gov.Sec.Form10K> AnnualReports
        {
            get { return _annualReports; }
        }

        public Gov.UsState UsStateOfIncorporation { get; set; }

        public Uri[] WebDomains { get; set; }

        public List<Ticker> TickerSymbols { get; set; }
        #endregion

        #region methods
        /// <summary>
        /// Constructs a URI which may be used to lookup ticker symbols of a public company
        /// based on the the name of the company.
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public static Uri GetUriTickerSymbolLookup(string companyName)
        {
            return
                new Uri("http://www.bloomberg.com/markets/symbolsearch?query=" + Uri.EscapeUriString(companyName.Replace(",","").Replace(".","")) + "&commit=Find+Symbols");
        }

        /// <summary>
        /// Merges the json data returned from <see cref="GetUriTickerSymbolLookup"/> into an 
        /// instance of <see cref="PublicCorporation"/>
        /// </summary>
        /// <param name="webResponseBody"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public static bool TryMergeTickerLookup(string webResponseBody, ref PublicCorporation pc)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(webResponseBody))
                    return false;
                var tempFile = Path.Combine(TempDirectories.AppData, Path.GetRandomFileName());
                File.WriteAllText(tempFile, webResponseBody);
                System.Threading.Thread.Sleep(Shared.Constants.ThreadSleepTime);

                var antlrHtml = Tokens.AspNetParseTree.InvokeParse(tempFile);

                var innerText = antlrHtml.CharData;

                if (innerText.Count <= 0)
                    return false;

                var st =
                    innerText.FindIndex(
                        x => string.Equals(x.Trim(), "Symbol Lookup", StringComparison.OrdinalIgnoreCase));
                var ed =
                    innerText.FindIndex(
                        x => string.Equals(x.Trim(), "Sponsored Link", StringComparison.OrdinalIgnoreCase));

                if (st > ed)
                    return false;

                var targetData = innerText.Skip(st).Take(ed - st).ToList();
                if (targetData.Count <= 0)
                    return false;

                if (targetData.Any(x => x.Contains(" no matches ")))
                    return false;

                st =
                    targetData.FindIndex(
                        x => string.Equals(x.Trim(), "Symbol", StringComparison.OrdinalIgnoreCase));

                targetData = targetData.Skip(st).ToList();

                var isDivisible = targetData.Count % 5 == 0;

                if (!isDivisible)
                    return false;

                var rowCount = Math.Floor(targetData.Count / 5M);

                if (rowCount <= 1)
                    return false;

                rowCount -= 1;
                pc.TickerSymbols = new List<Ticker>();
                for (var i = 1; i <= rowCount; i++)
                {
                    var te = targetData.Skip(i * 5).Take(5).ToArray();
                    pc.TickerSymbols.Add(new Ticker { Symbol = te[0], InstrumentType = te[3] });
                }

                return pc.TickerSymbols.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        public override void GetXrefXmlData()
        {
            var xrefXml = TreeData.XRefXml;
            if (xrefXml == null)
                return;
            var myAssocNodes =
                xrefXml.SelectNodes(string.Format("//x-ref-group[@data-type='{0}']//x-ref-id[text()='{1}']/../../add",
                    GetType().FullName, Name));
            if (myAssocNodes == null || myAssocNodes.Count <= 0)
                return;

            foreach (var node in myAssocNodes)
            {
                var elem = node as XmlElement;
                if (elem == null)
                    continue;

                XRefGroup.SetTypeXrefValue(elem, this);
            }
        }
        #endregion
    }

}