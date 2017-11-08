﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Rand.Data.NfHtml;
using NoFuture.Rand.Data.NfText;
using NoFuture.Rand.Data.NfXml;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Gov.Fed;
using NoFuture.Rand.Gov.Sec;

namespace NoFuture.Rand.Data
{
    public static class Facit
    {
        /// <summary>
        /// Gets a random Uri host.
        /// </summary>
        /// <returns></returns>
        public static string RandomUriHost(bool withSubDomain = true, bool usCommonOnly = false)
        {
            var webDomains = usCommonOnly ? ListData.UsWebmailDomains : ListData.WebmailDomains;
            var host = new StringBuilder();

            if (withSubDomain)
            {
                var subdomain = Etx.DiscreteRange(ListData.Subdomains);
                host.Append(subdomain + ".");
            }

            if (webDomains != null)
            {
                host.Append(webDomains[Etx.IntNumber(0, webDomains.Length - 1)]);
            }
            else
            {
                host.Append((string) Word());
                host.Append(Etx.DiscreteRange(new[] { ".com", ".net", ".edu", ".org" }));
            }
            return host.ToString();
        }

        /// <summary>
        /// Create a random http scheme uri with optional query string.
        /// </summary>
        /// <param name="useHttps"></param>
        /// <param name="addQry"></param>
        /// <returns></returns>
        public static Uri RandomHttpUri(bool useHttps = false, bool addQry = false)
        {

            var pathSeg = new List<string>();
            var pathSegLen = Etx.IntNumber(0, 5);
            for (var i = 0; i < pathSegLen; i++)
            {
                Etx.DiscreteRange(new Dictionary<string, double>()
                {
                    {Word(), 72},
                    {Etx.Consonant(false).ToString(), 11},
                    {Etx.IntNumber(1, 9999).ToString(), 17}
                });
                pathSeg.Add(Word());
            }

            if (Etx.CoinToss)
            {
                pathSeg.Add(Word() + Etx.DiscreteRange(new[] { ".php", ".aspx", ".html", ".txt", ".asp" }));
            }

            var uri = new UriBuilder
            {
                Scheme = useHttps ? "https" : "http",
                Host = RandomUriHost(),
                Path = String.Join("/", pathSeg.ToArray())
            };

            if (!addQry)
                return uri.Uri;

            var qry = new List<string>();
            var qryParms = Etx.IntNumber(1, 5);
            for (var i = 0; i < qryParms; i++)
            {
                var len = Etx.IntNumber(1, 4);
                var qryParam = new List<string>();
                for (var j = 0; j < len; j++)
                {
                    if (Etx.CoinToss)
                    {
                        qryParam.Add(Word());
                        continue;
                    }
                    if (Etx.CoinToss)
                    {
                        qryParam.Add(Etx.IntNumber(0, 99999).ToString());
                        continue;
                    }
                    qryParam.Add(Etx.Consonant(Etx.CoinToss).ToString());

                }
                qry.Add(String.Join("_", qryParam) + "=" + Etx.SupriseMe());
            }

            uri.Query = String.Join("&", qry);
            return uri.Uri;
        }

        /// <summary>
        /// Creates a random email address 
        /// </summary>
        /// <returns></returns>
        public static string RandomEmailUri(string username = "", bool usCommonOnly = false)
        {
            var host = RandomUriHost(false, usCommonOnly);
            if (!String.IsNullOrWhiteSpace(username))
                return String.Join("@", username, host);
            var bunchOfWords = new List<string>();
            for (var i = 0; i < 4; i++)
            {
                bunchOfWords.Add(Util.Etc.CapWords(Word(), ' '));
                bunchOfWords.Add(Domus.NAmerUtil.GetAmericanFirstName(DateTime.Today, Etx.CoinToss ? Gender.Male : Gender.Female));
            }
            username = String.Join((Etx.CoinToss ? "." : "_"), Etx.DiscreteRange(bunchOfWords.ToArray()),
                Etx.DiscreteRange(bunchOfWords.ToArray()));
            return String.Join("@", username, host);
        }

        /// <summary>
        /// Creates a random email address in a typical format
        /// </summary>
        /// <param name="names"></param>
        /// <param name="isProfessional">
        /// set this to true to have the username look unprofessional
        /// </param>
        /// <param name="usCommonOnly">
        /// true uses <see cref="NoFuture.Rand.Data.ListData.UsWebmailDomains"/>
        /// false uses <see cref="NoFuture.Rand.Data.ListData.WebmailDomains"/>
        /// </param>
        /// <returns></returns>
        public static string RandomEmailUri(string[] names, bool isProfessional = true, bool usCommonOnly = true)
        {
            if (names == null || !names.Any())
                return RandomEmailUri();

            //get childish username
            if (!isProfessional)
            {
                var shortWords = TreeData.EnglishWords.Where(x => x.Item1.Length <= 3).Select(x => x.Item1).ToArray();
                var shortWordList = new List<string>();
                for (var i = 0; i < 3; i++)
                {
                    var withUcase = Util.Etc.CapWords(Etx.DiscreteRange(shortWords), ' ');
                    shortWordList.Add(withUcase);
                }
                shortWordList.Add((Etx.CoinToss ? "_" : "") + Etx.IntNumber(100, 9999));
                return RandomEmailUri(String.Join("", shortWordList), usCommonOnly);
            }

            var fname = names.First().ToLower();
            var lname = names.Last().ToLower();
            string mi = null;
            if (names.Length > 2)
            {
                mi = names[1].ToLower();
                mi = Etx.CoinToss ? mi.First().ToString() : mi;
            }

            var unParts = new List<string> { Etx.CoinToss ? fname : fname.First().ToString(), mi, lname };
            var totalLength = unParts.Sum(x => x.Length);
            if (totalLength <= 7)
                return RandomEmailUri(String.Join(Etx.CoinToss ? "" : "_", String.Join(Etx.CoinToss ? "." : "_", unParts),
                    Etx.IntNumber(100, 9999)), usCommonOnly);
            return RandomEmailUri(totalLength > 20
                ? String.Join(Etx.CoinToss ? "." : "_", unParts.Take(2))
                : String.Join(Etx.CoinToss ? "." : "_", unParts), usCommonOnly);
        }

        /// <summary>
        /// Attempts to return a common english
        /// </summary>
        /// <returns></returns>
        public static string Word()
        {
            var enWords = TreeData.EnglishWords;
            if (enWords == null || enWords.Count <= 0)
                return Etx.Word(8);
            var pick = Etx.IntNumber(0, enWords.Count - 1);
            var enWord = enWords[pick]?.Item1;
            return !String.IsNullOrWhiteSpace(enWord)
                ? enWord
                : Etx.Word(8);
        }

        /// <summary>
        /// Factory method to get a concrete implementation of <see cref="INfDynData"/>
        /// based on a Uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static INfDynData DynamicDataFactory(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            if (uri.Host == "www.bloomberg.com")
            {
                return new BloombergSymbolSearch(uri);
            }
            if (uri.Host == Edgar.SEC_HOST)
            {
                if (uri.LocalPath == "/cgi-bin/srch-edgar")
                {
                    return new SecFullTxtSearch(uri);
                }
                if (uri.LocalPath == "/cgi-bin/browse-edgar" && uri.Query.StartsWith("?action=getcompany&CIK="))
                {
                    return new SecCikSearch(uri);
                }
                if (uri.LocalPath.StartsWith("/Archives/edgar/data"))
                {
                    if (uri.LocalPath.EndsWith("index.htm"))
                        return new SecGetXbrlUri(uri);
                    if (uri.LocalPath.EndsWith(".xml"))
                        return new SecXbrlInstanceFile(uri);
                }
            }
            if (uri.Host == new Uri(FedLrgBnk.RELEASE_URL).Host)
            {
                return new FedLrgBnk();
            }
            if (uri.Host == new Uri(Ffiec.SEARCH_URL_BASE).Host)
            {
                return new FfiecInstitProfile(uri);
            }

            throw new NotImplementedException();
        }
    }
}
