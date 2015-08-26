using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;
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
        private static List<FinancialFirm> _fedReleaseLrgBnkNames;
        #endregion

        #region Constants

        public const string DATA_SOURCE = @"Data\Source\";

        private const string CA_AREA_CODE_DATA_PATH = DATA_SOURCE + "CA_AreaCode.xml";
        private const string US_ZIP_DATA_PATH = DATA_SOURCE + "US_Zip.xml";
        private const string CA_ZIP_DATA_PATH = DATA_SOURCE + "CA_Zip.xml";
        private const string US_AREA_CODE_DATA_PATH = DATA_SOURCE + "US_AreaCode.xml";
        private const string INS_COMPANY_PATH = DATA_SOURCE + "US_InsCompanyNames.xml";
        private const string ECON_SECTOR_PATH = DATA_SOURCE + "US_EconSectors.xml";
        private const string US_UNIV_PATH = DATA_SOURCE + "US_Universities.xml";
        private const string FED_RELEASE_LRG_BANKS_PATH = DATA_SOURCE + "lrg_bnk_lst.txt";
        private const string US_FIRST_NAMES_DATA_PATH = DATA_SOURCE + "US_FirstNames.xml";
        private const string US_LAST_NAMES_DATA_PATH = DATA_SOURCE + "US_LastNames.xml";
        private const string US_HIGH_SCHOOL_DATA_PATH = DATA_SOURCE + "US_HighSchools.xml";
        private const string US_CITY_DATA_PATH = DATA_SOURCE + "US_City.xml";

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
                    GetXmlDataSource(Path.Combine(BinDirectories.Root, US_FIRST_NAMES_DATA_PATH), ref _usFnames);
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
                    GetXmlDataSource(Path.Combine(BinDirectories.Root, US_LAST_NAMES_DATA_PATH), ref _usLnames);
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
                    GetXmlDataSource(Path.Combine(BinDirectories.Root, US_ZIP_DATA_PATH), ref _usZipXml);

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
                    GetXmlDataSource(Path.Combine(BinDirectories.Root, US_HIGH_SCHOOL_DATA_PATH), ref _usHighSchools);

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
                    GetXmlDataSource(Path.Combine(BinDirectories.Root, US_CITY_DATA_PATH), ref _usCities);
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
                    GetXmlDataSource(Path.Combine(BinDirectories.Root, CA_ZIP_DATA_PATH), ref _caZipXml);

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
                    GetXmlDataSource(Path.Combine(BinDirectories.Root, US_AREA_CODE_DATA_PATH), ref _usAreaCodeXml);

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
                    GetXmlDataSource(Path.Combine(BinDirectories.Root, CA_AREA_CODE_DATA_PATH), ref _caAreaCodeXml);

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
                    GetXmlDataSource(Path.Combine(BinDirectories.Root, INS_COMPANY_PATH), ref _usInsComXml);

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
                    GetXmlDataSource(Path.Combine(BinDirectories.Root, ECON_SECTOR_PATH), ref _econSectorXml);

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
                    GetXmlDataSource(Path.Combine(BinDirectories.Root, US_UNIV_PATH), ref _usUnivXml);

                return _usUnivXml;
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

                var rawData =
                    File.ReadAllText(Path.Combine(BinDirectories.Root, FED_RELEASE_LRG_BANKS_PATH));
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
            if (String.IsNullOrWhiteSpace(BinDirectories.Root))
                throw new NullReferenceException("A path must be assigned to NoFuture.BinDirectories.Root prior to calling this method.");
            //load once as singleton
            if (assignTo != null) return;
            var buffer = File.ReadAllText(Path.Combine(BinDirectories.Root, path));
            assignTo = new XmlDocument();
            assignTo.LoadXml(buffer);
        }
    }
}
