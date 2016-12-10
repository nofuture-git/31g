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
        #region fields
        private static XmlDocument _usZipXml;
        private static XmlDocument _usStateData;
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
        private static XmlDocument _usPersonalDebt;
        private static XmlDocument _usPersonalWealth;
        private static XmlDocument _vinWmi;
        private static List<Tuple<string, double>> _enWords;
        private static Bank[] _fedReleaseLrgBnkNames;
        #endregion

        #region inner types

        public static class DataFiles
        {
            public const string CA_AREA_CODE_DATA_FILE = "CA_AreaCode.xml";
            public const string US_ZIP_DATA_FILE = "US_Zip.xml";
            public const string CA_ZIP_DATA_FILE = "CA_Zip.xml";
            public const string US_AREA_CODE_DATA_FILE = "US_AreaCode.xml";
            public const string INS_COMPANY_DATA_FILE = "US_InsCompanyNames.xml";
            public const string ECON_SECTOR_DATA_FILE = "US_EconSectors.xml";
            public const string US_UNIV_DATA_FILE = "US_Universities.xml";
            public const string US_FIRST_NAMES_DATA_FILE = "US_FirstNames.xml";
            public const string US_STATE_DATA_FILE = "US_States.xml";
            public const string US_LAST_NAMES_DATA_FILE = "US_LastNames.xml";
            public const string US_HIGH_SCHOOL_DATA_FILE = "US_HighSchools.xml";
            public const string US_CITY_DATA_FILE = "US_City.xml";
            public const string X_REF_DATA_FILE = "XRef.xml";
            public const string US_ZIP_PROB_TABLE_DATA_FILE = "US_Zip_ProbTable.xml";
            public const string LRG_BNK_LST_DATA_FILE = "lrg_bnk_lst.txt";
            public const string US_PERSON_DEBT_DATA_FILE = "US_PersonalDebt.xml";
            public const string US_PERSON_WEALTH_DATA_FILE = "US_PersonalWealth.xml";
            public const string VIN_WMI_DATA_FILE = "Vin_Wmi.xml";
            public const string ENGLISH_WORDS_DATA_FILE = "English_Words.xml";
        }

        #endregion

        #region methods
        /// <summary>
        /// Loads the <see cref="DataFiles.US_STATE_DATA_FILE"/> data into a <see cref="XmlDocument"/> document.
        /// </summary>
        public static XmlDocument UsStateData
        {
            get
            {
                if (_usStateData == null)
                    GetXmlDataSource(DataFiles.US_STATE_DATA_FILE, ref _usStateData);
                return _usStateData;
            }
        }
        /// <summary>
        /// Loads the <see cref="DataFiles.US_FIRST_NAMES_DATA_FILE"/> data into a <see cref="XmlDocument"/> document.
        /// </summary>
        public static XmlDocument AmericanFirstNamesData
        {
            get
            {
                if(_usFnames == null)
                    GetXmlDataSource(DataFiles.US_FIRST_NAMES_DATA_FILE, ref _usFnames);
                return _usFnames;
            }
        }

        /// <summary>
        /// Loads the <see cref="DataFiles.US_LAST_NAMES_DATA_FILE"/> data into a <see cref="XmlDocument"/> document.
        /// </summary>
        public static XmlDocument AmericanLastNamesData
        {
            get
            {
                if (_usLnames == null)
                    GetXmlDataSource(DataFiles.US_LAST_NAMES_DATA_FILE, ref _usLnames);
                return _usLnames;
            }
            
        }

        /// <summary>
        /// Loads the <see cref="DataFiles.US_ZIP_DATA_FILE"/> data into a <see cref="XmlDocument"/> document.
        /// </summary>
        public static XmlDocument AmericanZipCodeData
        {
            get
            {
                if (_usZipXml == null)
                    GetXmlDataSource(DataFiles.US_ZIP_DATA_FILE, ref _usZipXml);

                return _usZipXml;
            }
        }

        /// <summary>
        /// Loads the <see cref="DataFiles.US_HIGH_SCHOOL_DATA_FILE"/> data into a <see cref="XmlDocument"/> document.
        /// </summary>
        public static XmlDocument AmericanHighSchoolData
        {
            get
            {
                if(_usHighSchools == null)
                    GetXmlDataSource(DataFiles.US_HIGH_SCHOOL_DATA_FILE, ref _usHighSchools);

                return _usHighSchools;
            }
        }

        /// <summary>
        /// Loads the <see cref="DataFiles.US_CITY_DATA_FILE"/> data into a <see cref="XmlDocument"/> document.
        /// </summary>
        public static XmlDocument AmericanCityData
        {
            get
            {
                if(_usCities == null)
                    GetXmlDataSource(DataFiles.US_CITY_DATA_FILE, ref _usCities);
                return _usCities;
            }
        }

        /// <summary>
        /// Loads the <see cref="DataFiles.CA_ZIP_DATA_FILE"/> data into a <see cref="XmlDocument"/> document.
        /// </summary>
        public static XmlDocument CanadianPostalCodeData
        {
            get
            {
                if (_caZipXml == null)
                    GetXmlDataSource(DataFiles.CA_ZIP_DATA_FILE, ref _caZipXml);

                return _caZipXml;
            }
        }

        /// <summary>
        /// Loads <see cref="DataFiles.US_AREA_CODE_DATA_FILE"/> data into <see cref="XmlDocument"/> document.
        /// </summary>
        public static XmlDocument AmericanAreaCodeData
        {
            get
            {
                if (_usAreaCodeXml == null)
                    GetXmlDataSource(DataFiles.US_AREA_CODE_DATA_FILE, ref _usAreaCodeXml);

                return _usAreaCodeXml;
            }
        }

        /// <summary>
        /// Loads <see cref="DataFiles.CA_AREA_CODE_DATA_FILE"/> data into <see cref="XmlDocument"/> document.
        /// </summary>
        public static XmlDocument CanadianAreaCodeData
        {
            get
            {
                if (_caAreaCodeXml == null)
                    GetXmlDataSource(DataFiles.CA_AREA_CODE_DATA_FILE, ref _caAreaCodeXml);

                return _caAreaCodeXml;
            }
        }

        /// <summary>
        /// Loads the <see cref="DataFiles.INS_COMPANY_DATA_FILE"/> data into <see cref="XmlDocument"/> document.
        /// </summary>
        public static XmlDocument InsuranceCompanyNameData
        {
            get
            {
                if (_usInsComXml == null)
                    GetXmlDataSource(DataFiles.INS_COMPANY_DATA_FILE, ref _usInsComXml);

                return _usInsComXml;
            }
        }

        /// <summary>
        /// Loads the <see cref="DataFiles.ECON_SECTOR_DATA_FILE"/> data into a <see cref="XmlDocument"/> document.
        /// </summary>
        public static XmlDocument EconSectorData
        {
            get
            {
                if (_econSectorXml == null)
                    GetXmlDataSource(DataFiles.ECON_SECTOR_DATA_FILE, ref _econSectorXml);

                return _econSectorXml;
            }
        }

        /// <summary>
        /// Loads the <see cref="DataFiles.US_UNIV_DATA_FILE"/> data into a <see cref="XmlDocument"/> document.
        /// </summary>
        public static XmlDocument AmericanUniversityData
        {
            get
            {
                if (_usUnivXml == null)
                    GetXmlDataSource(DataFiles.US_UNIV_DATA_FILE, ref _usUnivXml);

                return _usUnivXml;
            }
        }

        /// <summary>
        /// Loads the <see cref="DataFiles.X_REF_DATA_FILE"/> data into a <see cref="XmlDocument"/> document.
        /// </summary>
        public static XmlDocument XRefXml
        {
            get
            {
                if(_xrefData == null)
                    GetXmlDataSource(DataFiles.X_REF_DATA_FILE, ref _xrefData);
                return _xrefData;
            }
        }

        /// <summary>
        /// Loads the <see cref="DataFiles.US_ZIP_PROB_TABLE_DATA_FILE"/> data into a <see cref="XmlDocument"/> document.
        /// </summary>
        public static XmlDocument UsZipProbabilityTable
        {
            get
            {
                if (_usZipProbTable == null)
                    GetXmlDataSource(DataFiles.US_ZIP_PROB_TABLE_DATA_FILE, ref _usZipProbTable);
                return _usZipProbTable;
            }
        }

        /// <summary>
        /// Loads the <see cref="DataFiles.US_PERSON_DEBT_DATA_FILE"/> data into a <see cref="XmlDocument"/> document.
        /// </summary>
        public static XmlDocument UsPersonalDebt
        {
            get
            {
                if(_usPersonalDebt == null)
                    GetXmlDataSource(DataFiles.US_PERSON_DEBT_DATA_FILE, ref _usPersonalDebt);
                return _usPersonalDebt;
            }
        }

        /// <summary>
        /// Loads the <see cref="DataFiles.US_PERSON_WEALTH_DATA_FILE"/> data into a <see cref="XmlDocument"/> document.
        /// </summary>
        public static XmlDocument UsPersonalWealth
        {
            get
            {
                if (_usPersonalWealth == null)
                    GetXmlDataSource(DataFiles.US_PERSON_WEALTH_DATA_FILE, ref _usPersonalWealth);
                return _usPersonalWealth;
            }
        }

        /// <summary>
        /// Loads the <see cref="DataFiles.VIN_WMI_DATA_FILE"/> data into a <see cref="XmlDocument"/> document.
        /// </summary>
        public static XmlDocument VinWmi
        {
            get
            {
                if(_vinWmi == null)
                    GetXmlDataSource(DataFiles.VIN_WMI_DATA_FILE, ref _vinWmi);
                return _vinWmi;
            }
        }

        /// <summary>
        /// Loads the <see cref="DataFiles.ENGLISH_WORDS_DATA_FILE"/> data into a <see cref="XmlDocument"/> document.
        /// </summary>
        public static List<Tuple<string, double>> EnglishWords
        {
            get
            {
                const string WORD = "word";
                const string LANG = "lang";
                const string COUNT = "count";
                try
                {
                    if (_enWords != null && _enWords.Count > 0)
                        return _enWords;

                    if (!TestDataFileIsPresent(DataFiles.ENGLISH_WORDS_DATA_FILE))
                        return null;

                    XmlDocument enWordsXml = null;
                    GetXmlDataSource(DataFiles.ENGLISH_WORDS_DATA_FILE, ref enWordsXml);
                    var wordNodes = enWordsXml?.SelectNodes($"//{WORD}[@{LANG}='en']");
                    if (wordNodes == null)
                        return null;

                    _enWords = new List<Tuple<string, double>>();
                    foreach (var node in wordNodes)
                    {
                        var elem = node as XmlElement;
                        var word = elem?.InnerText;
                        if (string.IsNullOrWhiteSpace(word))
                            continue;
                        var countStr = elem.Attributes[COUNT]?.Value;
                        if (string.IsNullOrWhiteSpace(countStr))
                            continue;
                        double count;
                        if (!double.TryParse(countStr, out count))
                            continue;
                        _enWords.Add(new Tuple<string, double>(word, count));
                    }
                    return _enWords;
                }
                catch (Exception ex)//keep this contained
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                }
                return null;
            }
        }

        /// <summary>
        /// Loads a list of <see cref="FinancialFirm"/> by parsing the data from <see cref="DataFiles.LRG_BNK_LST_DATA_FILE"/> 
        /// <remarks>
        /// A path must be set to <see cref="BinDirectories.Root"/> which contains a 'Data\Source' folder
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

                var rawData = GetTextDataSource(DataFiles.LRG_BNK_LST_DATA_FILE);
                if(string.IsNullOrWhiteSpace(rawData))
                    return new Bank[0];//return empty list for missing data

                var myDynData = Etx.DynamicDataFactory(new Uri(NfText.FedLrgBnk.RELEASE_URL));
                var myDynDataRslt = myDynData.ParseContent(rawData);


                //take each line data structure and compose a full object
                var tempList = myDynDataRslt.Select(pd => new Bank(pd)).ToList();

                if (tempList.Count > 0)
                    _fedReleaseLrgBnkNames = tempList.ToArray();
                return _fedReleaseLrgBnkNames;
            }
        }


        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static string GetTextDataSource(string name)
        {
            ThrowExOnFileNotFound(name);

            return string.IsNullOrWhiteSpace(name)
                ? null
                : File.ReadAllText(Path.Combine(BinDirectories.DataRoot, name));
        }
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static void GetXmlDataSource(string name, ref XmlDocument assignTo)
        {
            ThrowExOnFileNotFound(name);

            if (string.IsNullOrWhiteSpace(name))
                return;

            if(!File.Exists(Path.Combine(BinDirectories.DataRoot, name)))
                throw new RahRowRagee($"{name} file is missing from {BinDirectories.DataRoot}");

            var xmlData = File.ReadAllText(Path.Combine(BinDirectories.DataRoot, name));
            if (string.IsNullOrWhiteSpace(xmlData))
            {
                assignTo = null;
                return;
            }

            assignTo = new XmlDocument();
            assignTo.LoadXml(xmlData);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static void ThrowExOnFileNotFound(string name)
        {
            if (!TestDataFileIsPresent(name))
            {
                throw new InvalidOperationException("Data is stored on the drive in the folder assigned to " +
                                                    $"{nameof(BinDirectories.DataRoot)}.  Assign this variable " +
                                                    "and try again.");
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static bool TestDataFileIsPresent(string name)
        {
            if (string.IsNullOrWhiteSpace(BinDirectories.DataRoot) || !Directory.Exists(BinDirectories.DataRoot))
                return false;
            return !string.IsNullOrWhiteSpace(name) && File.Exists(Path.Combine(BinDirectories.DataRoot, name));
        }
        #endregion

    }
}
