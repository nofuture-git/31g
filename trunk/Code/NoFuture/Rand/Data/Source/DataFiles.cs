using System;
using System.IO;
using System.Reflection;

namespace NoFuture.Rand.Data.Source
{
    public static class DataFiles
    {
        public const string RAND_DATA = "NoFuture.Rand.Data.Source";
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
        public const string US_PERSON_DEBT_DATA_FILE = "US_PersonalDebt.xml";
        public const string US_PERSON_WEALTH_DATA_FILE = "US_PersonalWealth.xml";
        public const string VIN_WMI_DATA_FILE = "Vin_Wmi.xml";
        public const string ENGLISH_WORDS_DATA_FILE = "English_Words.xml";
        public const string WEBMAIL_DOMAINS = "webmailDomains.txt";
        public const string US_OCCUPATIONS = "US_Occupations.xml";
        public const string US_BANKS = "US_Banks.xml";
        public const string US_DOMUS_OPES = "US_DomusOpes.xml";
        public const string US_OCCUPATIONS_PROBTABLE = "US_Occupations_ProbTable.xml";

        public static string GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var asm = Assembly.GetExecutingAssembly();

            var data = asm.GetManifestResourceStream($"{asm.GetName().Name}.{name}");
            if (data == null)
                return null;

            var strmRdr = new StreamReader(data);
            return strmRdr.ReadToEnd();
        }
    }
}
