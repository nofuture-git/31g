using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Gov.Sec
{
    public class Edgar
    {
        #region constants
        public const string SEC_ROOT_URL = "https://" + SEC_HOST;
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
        [Serializable]
        public class FullTextSearch
        {
            public string SicCode;
            public string CompanyName;
            public string ZipCode;
            public string StateOfIncorporation;
            public string BizAddrState;
            public SecForm FormType;
            public int PagingStartAt;
            public int PagingCount;
            public int YearStart;
            public int YearEnd;

            public FullTextSearch()
            {
                PagingStartAt = 1;
                PagingCount = 80;
                YearStart = DateTime.Today.Year - 1;
                YearEnd = DateTime.Today.Year;
            }

            public override string ToString()
            {
                if (
                    new[] {SicCode, CompanyName, ZipCode, StateOfIncorporation, BizAddrState}.All(
                        string.IsNullOrWhiteSpace))
                    return string.Empty;

                var searchList = new List<string>();
                if(!string.IsNullOrWhiteSpace(SicCode))
                    searchList.Add(HttpUtility.UrlEncode($"ASSIGNED-SIC={SicCode.Trim()}"));

                FormType = FormType ?? new Form10K();
                searchList.Add(HttpUtility.UrlEncode($"FORM-TYPE={FormType.Abbrev}"));

                if (!string.IsNullOrWhiteSpace(CompanyName))
                {
                    var searchName = Cusip.GetSearchCompanyName(CompanyName);
                    searchName = Uri.EscapeUriString(searchName).Replace("&", "%26");
                    searchList.Add(CompanyName.ToCharArray().All(char.IsLetterOrDigit)
                        ? HttpUtility.UrlEncode($"COMPANY-NAME={searchName}")
                        : $"COMPANY-NAME=%22{searchName}%22");
                }

                if(!string.IsNullOrWhiteSpace(ZipCode))
                    searchList.Add(HttpUtility.UrlEncode($"ZIP={ZipCode.Trim()}"));
                if(!string.IsNullOrWhiteSpace(StateOfIncorporation))
                    searchList.Add(HttpUtility.UrlEncode($"STATE-OF-INCORPORATION={StateOfIncorporation.Trim()}"));
                if(!string.IsNullOrWhiteSpace(BizAddrState))
                    searchList.Add(HttpUtility.UrlEncode($"STATE={BizAddrState.Trim()}"));
                return string.Join("+AND+", searchList);
            }
        }
        #endregion


    }
}
