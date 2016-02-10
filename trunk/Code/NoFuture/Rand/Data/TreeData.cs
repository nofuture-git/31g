using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
        private static List<FinancialFirm> _fedReleaseLrgBnkNames;
        #endregion

        #region Constants

        public const string DATA_SOURCE = @"Data\Source\";

        private const string CA_AREA_CODE_DATA_PATH = "CA_AreaCode.xml";
        private const string US_ZIP_DATA_PATH = "US_Zip.xml";
        private const string CA_ZIP_DATA_PATH = "CA_Zip.xml";
        private const string US_AREA_CODE_DATA_PATH = "US_AreaCode.xml";
        private const string INS_COMPANY_PATH = "US_InsCompanyNames.xml";
        private const string ECON_SECTOR_PATH = "US_EconSectors.xml";
        private const string US_UNIV_PATH = "US_Universities.xml";
        private const string FED_RELEASE_LRG_BANKS_PATH = "lrg_bnk_lst.txt";
        private const string US_FIRST_NAMES_DATA_PATH = "US_FirstNames.xml";
        private const string US_LAST_NAMES_DATA_PATH = "US_LastNames.xml";
        private const string US_HIGH_SCHOOL_DATA_PATH = "US_HighSchools.xml";
        private const string US_CITY_DATA_PATH = "US_City.xml";
        private const string X_REF_DATA = "XRef.xml";

        #endregion

        #region API

        /// <summary>
        /// Loads the 'US_FirstNames.xml' data into a <see cref="System.Xml.XmlDocument"/> document.
        /// <remarks>
        /// A path must be set to <see cref="NoFuture.BinDirectories.Root"/> which contains a 'Data\Source' folder
        /// and it is within that folder the method expects to find 'US_FirstNames.xml'.
        /// </remarks>
        /// </summary>
        public static XmlDocument AmericanFirstNamesData
        {
            get
            {
                if(_usFnames == null)
                    GetXmlDataSource(GetDataDocPath(US_FIRST_NAMES_DATA_PATH), ref _usFnames);
                return _usFnames;
            }
        }

        /// <summary>
        /// Loads the 'US_LastNames.xml' data into a <see cref="System.Xml.XmlDocument"/> document.
        /// <remarks>
        /// A path must be set to <see cref="NoFuture.BinDirectories.Root"/> which contains a 'Data\Source' folder
        /// and it is within that folder the method expects to find 'US_LastNames.xml'.
        /// </remarks>
        /// </summary>
        public static XmlDocument AmericanLastNamesData
        {
            get
            {
                if (_usLnames == null)
                    GetXmlDataSource(GetDataDocPath(US_LAST_NAMES_DATA_PATH), ref _usLnames);
                return _usLnames;
            }
            
        }

        /// <summary>
        /// Loads the 'US_Zip.xml' data into a <see cref="System.Xml.XmlDocument"/> document.
        /// <remarks>
        /// A path must be set to <see cref="NoFuture.BinDirectories.Root"/> which contains a 'Data\Source' folder
        /// and it is within that folder the method expects to find 'US_Zip.xml'.
        /// </remarks>
        /// </summary>
        public static XmlDocument AmericanZipCodeData
        {
            get
            {
                if (_usZipXml == null)
                    GetXmlDataSource(GetDataDocPath(US_ZIP_DATA_PATH), ref _usZipXml);

                return _usZipXml;
            }
        }

        /// <summary>
        /// Loads the 'US_HighSchools.xml' data into a <see cref="System.Xml.XmlDocument"/> document.
        /// <remarks>
        /// A path must be set to <see cref="NoFuture.BinDirectories.Root"/> which contains a 'Data\Source' folder
        /// and it is within that folder the method expects to find 'US_HighSchools.xml'.
        /// </remarks>
        /// </summary>
        public static XmlDocument AmericanHighSchoolData
        {
            get
            {
                if(_usHighSchools == null)
                    GetXmlDataSource(GetDataDocPath(US_HIGH_SCHOOL_DATA_PATH), ref _usHighSchools);

                return _usHighSchools;
            }
        }

        /// <summary>
        /// Loads the 'US_City.xml' data into a <see cref="System.Xml.XmlDocument"/> document.
        /// <remarks>
        /// A path must be set to <see cref="NoFuture.BinDirectories.Root"/> which contains a 'Data\Source' folder
        /// and it is within that folder the method expects to find 'US_City.xml'.
        /// </remarks>
        /// </summary>
        public static XmlDocument AmericanCityData
        {
            get
            {
                if(_usCities == null)
                    GetXmlDataSource(GetDataDocPath(US_CITY_DATA_PATH), ref _usCities);
                return _usCities;
            }
        }
        
        /// <summary>
        /// Loads the 'CA_Zip.xml' data into a <see cref="System.Xml.XmlDocument"/> document.
        /// <remarks>
        /// A path must be set to <see cref="NoFuture.BinDirectories.Root"/> which contains a 'Data\Source' folder
        /// and it is within that folder the method expects to find 'CA_Zip.xml'.
        /// </remarks>
        /// </summary>
        public static XmlDocument CanadianPostalCodeData
        {
            get
            {
                if (_caZipXml == null)
                    GetXmlDataSource(GetDataDocPath(CA_ZIP_DATA_PATH), ref _caZipXml);

                return _caZipXml;
            }
        }

        /// <summary>
        /// Loads 'US_AreaCode.xml' data into <see cref="System.Xml.XmlDocument"/> document.
        /// </summary>
        /// <remarks>
        /// A path must be set to <see cref="NoFuture.BinDirectories.Root"/> which contains a 'Data\Source' folder
        /// and it is within that folder the method expects to find 'US_AreaCode.xml'.
        /// </remarks>
        public static XmlDocument AmericanAreaCodeData
        {
            get
            {
                if (_usAreaCodeXml == null)
                    GetXmlDataSource(GetDataDocPath(US_AREA_CODE_DATA_PATH), ref _usAreaCodeXml);

                return _usAreaCodeXml;
            }
        }

        /// <summary>
        /// Loads 'CA_AreaCode.xml' data into <see cref="System.Xml.XmlDocument"/> document.
        /// </summary>
        /// <remarks>
        /// A path must be set to <see cref="NoFuture.BinDirectories.Root"/> which contains a 'Data\Source' folder
        /// and it is within that folder the method expects to find 'CA_AreaCode.xml'.
        /// </remarks>
        public static XmlDocument CanadianAreaCodeData
        {
            get
            {
                if (_caAreaCodeXml == null)
                    GetXmlDataSource(GetDataDocPath(CA_AREA_CODE_DATA_PATH), ref _caAreaCodeXml);

                return _caAreaCodeXml;
            }
        }

        /// <summary>
        /// Loads the 'US_InsCompanyNames.xml' data into <see cref="System.Xml.XmlDocument"/> document.
        /// <remarks>
        /// A path must be set to <see cref="NoFuture.BinDirectories.Root"/> which contains a 'Data\Source' folder
        /// and it is within that folder the method expects to find 'US_InsCompanyNames.xml'.
        /// </remarks>
        /// </summary>
        public static XmlDocument InsuranceCompanyNameData
        {
            get
            {
                if (_usInsComXml == null)
                    GetXmlDataSource(GetDataDocPath(INS_COMPANY_PATH), ref _usInsComXml);

                return _usInsComXml;
            }
        }

        /// <summary>
        /// Loads the 'US_EconSectors.xml' data into a <see cref="System.Xml.XmlDocument"/> document.
        /// <remarks>
        /// A path must be set to <see cref="NoFuture.BinDirectories.Root"/> which contains a 'Data\Source' folder
        /// and it is within that folder the method expects to find 'US_EconSectors.xml'.
        /// </remarks>
        /// </summary>
        public static XmlDocument EconSectorData
        {
            get
            {
                if (_econSectorXml == null)
                    GetXmlDataSource(GetDataDocPath(ECON_SECTOR_PATH), ref _econSectorXml);

                return _econSectorXml;
            }
        }

        /// <summary>
        /// Loads the 'US_EconSectors.xml' data into a <see cref="System.Xml.XmlDocument"/> document.
        /// <remarks>
        /// A path must be set to <see cref="NoFuture.BinDirectories.Root"/> which contains a 'Data\Source' folder
        /// and it is within that folder the method expects to find 'US_EconSectors.xml'.
        /// </remarks>
        /// </summary>
        public static XmlDocument AmericanUniversityData
        {
            get
            {
                if (_usUnivXml == null)
                    GetXmlDataSource(GetDataDocPath(US_UNIV_PATH), ref _usUnivXml);

                return _usUnivXml;
            }
        }

        /// <summary>
        /// Loads the 'XRef.xml' data into a <see cref="System.Xml.XmlDocument"/> document.
        /// <remarks>
        /// A path must be set to <see cref="NoFuture.BinDirectories.Root"/> which contains a 'Data\Source' folder
        /// and it is within that folder the method expects to find 'XRef.xml'.
        /// </remarks>
        /// </summary>
        public static XmlDocument XRefXml
        {
            get
            {
                if(_xrefData == null)
                    GetXmlDataSource(GetDataDocPath(X_REF_DATA), ref _xrefData);
                return _xrefData;
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
        public static List<FinancialFirm> CommercialBankData
        {
            get
            {
                if (String.IsNullOrWhiteSpace(BinDirectories.Root))
                    throw new NullReferenceException(
                        "A path must be assigned to NoFuture.BinDirectories.Root prior to calling this method.");

                if (_fedReleaseLrgBnkNames != null && _fedReleaseLrgBnkNames.Count > 0)
                    return _fedReleaseLrgBnkNames;

                var dataDir = Path.Combine(BinDirectories.Root, DATA_SOURCE);

                var rawData =
                    File.ReadAllText(Path.Combine(dataDir, FED_RELEASE_LRG_BANKS_PATH));
                DateTime? rptDt;
                var parsedData = Gov.Fed.LargeCommercialBanks.ParseBankData(rawData, out rptDt);

                var useDate = rptDt == null ? DateTime.Today : rptDt.Value;
                var tempList = parsedData.Select(pd => new FinancialFirm(pd.Item2, useDate)).ToList();

                if (tempList.Count > 0)
                    _fedReleaseLrgBnkNames = tempList;
                return _fedReleaseLrgBnkNames;
            }
        }

        #endregion
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static void GetXmlDataSource(string path, ref XmlDocument assignTo)
        {
            //load once as singleton
            if (assignTo != null) return;

            if (String.IsNullOrWhiteSpace(BinDirectories.Root))
                throw new NullReferenceException("A path must be assigned to NoFuture.BinDirectories.Root prior to calling this method.");

            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                throw new ItsDeadJim(string.Format("Cannot find an Xml file at '{0}'", path));

            var buffer = File.ReadAllText(path);
            assignTo = new XmlDocument();
            assignTo.LoadXml(buffer);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static string GetDataDocPath(string somename)
        {
            if(string.IsNullOrWhiteSpace(somename))
                throw new ArgumentNullException("somename");
            var dataFolder = Path.Combine(BinDirectories.Root, DATA_SOURCE);

            return Path.Combine(dataFolder, somename);
        }
    }
}
