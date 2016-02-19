﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using NoFuture.Exceptions;
using NoFuture.Rand.Com;

namespace NoFuture.Rand.Data
{
    public class TreeData
    {
        #region Fields
        private static XmlDocument _usZipXml;
        private static XmlDocument _caZipXml;
        private static XmlDocument _usAreaCodeXml;
        private static XmlDocument _caAreaCodeXml;
        private static XmlDocument _usInsComXml;
        private static XmlDocument _econSectorXml;
        private static XmlDocument _usUnivXml;
        private static XmlDocument _usFnames;
        private static XmlDocument _usLnames;
        private static XmlDocument _usHighSchools;
        private static XmlDocument _usCities;
        private static XmlDocument _xrefData;
        private static XmlDocument _usZipProbTable;
        private static Bank[] _fedReleaseLrgBnkNames;
        #endregion

        #region Constants

        public const string DATA_SOURCE = @"NoFuture.Rand.Data.Source";

        private const string CA_AREA_CODE_DATA_PATH = "CA_AreaCode.xml";
        private const string US_ZIP_DATA_PATH = "US_Zip.xml";
        private const string CA_ZIP_DATA_PATH = "CA_Zip.xml";
        private const string US_AREA_CODE_DATA_PATH = "US_AreaCode.xml";
        private const string INS_COMPANY_PATH = "US_InsCompanyNames.xml";
        private const string ECON_SECTOR_PATH = "US_EconSectors.xml";
        private const string US_UNIV_PATH = "US_Universities.xml";
        private const string US_FIRST_NAMES_DATA_PATH = "US_FirstNames.xml";
        private const string US_LAST_NAMES_DATA_PATH = "US_LastNames.xml";
        private const string US_HIGH_SCHOOL_DATA_PATH = "US_HighSchools.xml";
        private const string US_CITY_DATA_PATH = "US_City.xml";
        private const string X_REF_DATA = "XRef.xml";
        private const string US_ZIP_PROBTABLE = "US_Zip_ProbTable.xml";
        private const string LRG_BNK_LST = "lrg_bnk_lst.txt";

        #endregion

        #region API

        /// <summary>
        /// Loads the 'US_FirstNames.xml' data into a <see cref="System.Xml.XmlDocument"/> document.
        /// </summary>
        public static XmlDocument AmericanFirstNamesData
        {
            get
            {
                if(_usFnames == null)
                    GetXmlDataSource(US_FIRST_NAMES_DATA_PATH, ref _usFnames);
                return _usFnames;
            }
        }

        /// <summary>
        /// Loads the 'US_LastNames.xml' data into a <see cref="System.Xml.XmlDocument"/> document.
        /// </summary>
        public static XmlDocument AmericanLastNamesData
        {
            get
            {
                if (_usLnames == null)
                    GetXmlDataSource(US_LAST_NAMES_DATA_PATH, ref _usLnames);
                return _usLnames;
            }
            
        }

        /// <summary>
        /// Loads the 'US_Zip.xml' data into a <see cref="System.Xml.XmlDocument"/> document.
        /// </summary>
        public static XmlDocument AmericanZipCodeData
        {
            get
            {
                if (_usZipXml == null)
                    GetXmlDataSource(US_ZIP_DATA_PATH, ref _usZipXml);

                return _usZipXml;
            }
        }

        /// <summary>
        /// Loads the 'US_HighSchools.xml' data into a <see cref="System.Xml.XmlDocument"/> document.
        /// </summary>
        public static XmlDocument AmericanHighSchoolData
        {
            get
            {
                if(_usHighSchools == null)
                    GetXmlDataSource(US_HIGH_SCHOOL_DATA_PATH, ref _usHighSchools);

                return _usHighSchools;
            }
        }

        /// <summary>
        /// Loads the 'US_City.xml' data into a <see cref="System.Xml.XmlDocument"/> document.
        /// </summary>
        public static XmlDocument AmericanCityData
        {
            get
            {
                if(_usCities == null)
                    GetXmlDataSource(US_CITY_DATA_PATH, ref _usCities);
                return _usCities;
            }
        }
        
        /// <summary>
        /// Loads the 'CA_Zip.xml' data into a <see cref="System.Xml.XmlDocument"/> document.
        /// </summary>
        public static XmlDocument CanadianPostalCodeData
        {
            get
            {
                if (_caZipXml == null)
                    GetXmlDataSource(CA_ZIP_DATA_PATH, ref _caZipXml);

                return _caZipXml;
            }
        }

        /// <summary>
        /// Loads 'US_AreaCode.xml' data into <see cref="System.Xml.XmlDocument"/> document.
        /// </summary>
        public static XmlDocument AmericanAreaCodeData
        {
            get
            {
                if (_usAreaCodeXml == null)
                    GetXmlDataSource(US_AREA_CODE_DATA_PATH, ref _usAreaCodeXml);

                return _usAreaCodeXml;
            }
        }

        /// <summary>
        /// Loads 'CA_AreaCode.xml' data into <see cref="System.Xml.XmlDocument"/> document.
        /// </summary>
        public static XmlDocument CanadianAreaCodeData
        {
            get
            {
                if (_caAreaCodeXml == null)
                    GetXmlDataSource(CA_AREA_CODE_DATA_PATH, ref _caAreaCodeXml);

                return _caAreaCodeXml;
            }
        }

        /// <summary>
        /// Loads the 'US_InsCompanyNames.xml' data into <see cref="System.Xml.XmlDocument"/> document.
        /// </summary>
        public static XmlDocument InsuranceCompanyNameData
        {
            get
            {
                if (_usInsComXml == null)
                    GetXmlDataSource(INS_COMPANY_PATH, ref _usInsComXml);

                return _usInsComXml;
            }
        }

        /// <summary>
        /// Loads the 'US_EconSectors.xml' data into a <see cref="System.Xml.XmlDocument"/> document.
        /// </summary>
        public static XmlDocument EconSectorData
        {
            get
            {
                if (_econSectorXml == null)
                    GetXmlDataSource(ECON_SECTOR_PATH, ref _econSectorXml);

                return _econSectorXml;
            }
        }

        /// <summary>
        /// Loads the 'US_EconSectors.xml' data into a <see cref="System.Xml.XmlDocument"/> document.
        /// </summary>
        public static XmlDocument AmericanUniversityData
        {
            get
            {
                if (_usUnivXml == null)
                    GetXmlDataSource(US_UNIV_PATH, ref _usUnivXml);

                return _usUnivXml;
            }
        }

        /// <summary>
        /// Loads the 'XRef.xml' data into a <see cref="System.Xml.XmlDocument"/> document.
        /// </summary>
        public static XmlDocument XRefXml
        {
            get
            {
                if(_xrefData == null)
                    GetXmlDataSource(X_REF_DATA, ref _xrefData);
                return _xrefData;
            }
        }

        /// <summary>
        /// Loads the 'XRef.xml' data into a <see cref="System.Xml.XmlDocument"/> document.
        /// </summary>
        public static XmlDocument UsZipProbabilityTable
        {
            get
            {
                if (_usZipProbTable == null)
                    GetXmlDataSource(US_ZIP_PROBTABLE, ref _usZipProbTable);
                return _usZipProbTable;
            }
        }

        /// <summary>
        /// Loads a list of <see cref="FinancialFirm"/> by parsing the data from <see cref="NoFuture.Rand.Gov.Fed.LargeCommercialBanks.RELEASE_URL"/> 
        /// <remarks>
        /// A path must be set to <see cref="NoFuture.BinDirectories.Root"/> which contains a 'Data\Source' folder
        /// and it is within that folder the method expects to find 'lrg_bnk_lst.txt'.
        /// This function uses the local copy of the doc and does not fetch it from The Fed over the net.
        /// </remarks>
        /// </summary>
        public static Bank[] CommercialBankData
        {
            get
            {
                if (_fedReleaseLrgBnkNames != null && _fedReleaseLrgBnkNames.Length > 0)
                    return _fedReleaseLrgBnkNames;

                var rawData = GetTextDataSource(LRG_BNK_LST);
                if(string.IsNullOrWhiteSpace(rawData))
                    return new Bank[0];//return empty list for missing data

                DateTime? rptDt;
                //turn line data into a list of tuples 
                var parsedData = Gov.Fed.LargeCommercialBanks.ParseBankData(rawData, out rptDt);
                var useDate = rptDt == null ? DateTime.Today : rptDt.Value;

                //take each line data structure and compose a full object
                var tempList =
                    parsedData.Where(pd => !string.IsNullOrWhiteSpace(pd.Item1))
                        .Select(pd => new Bank(pd.Item2, useDate) {Name = pd.Item1.Trim()})
                        .ToList();

                if (tempList.Count > 0)
                    _fedReleaseLrgBnkNames = tempList.ToArray();
                return _fedReleaseLrgBnkNames;
            }
        }

        #endregion

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static string GetTextDataSource(string name)
        {
            var embeddedStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DATA_SOURCE + "." + name);
            if (embeddedStream == null)
            {
                return null;
            }

            var strRdr = new StreamReader(embeddedStream);
            return strRdr.ReadToEnd();
        }
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static void GetXmlDataSource(string name, ref XmlDocument assignTo)
        {
            if (assignTo != null) return;

            var embeddedStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DATA_SOURCE + "." + name);
            if (embeddedStream == null)
            {
                assignTo = null;
                return;
            }

            var strRdr = new StreamReader(embeddedStream);
            var embeddedXml = strRdr.ReadToEnd();

            assignTo = new XmlDocument();
            assignTo.LoadXml(embeddedXml);
        }
    }
}
