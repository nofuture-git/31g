using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Geo.US;
using NoFuture.Rand.Org;
using NoFuture.Rand.Tele;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Com
{
    /// <summary>
    /// A base implementation of <see cref="IFirm"/>
    /// </summary>
    [Serializable]
    public abstract class Firm : VocaBase, IFirm
    {
        #region constants
        protected const int ONE_THOUSAND = 1000;
        #endregion

        #region fields
        private NaicsPrimarySector _primarySector;
        private NaicsSector _sector;
        private NaicsMarket _market;
        private int _fiscalYearEndDay = 1;
        private readonly HashSet<Uri> _netUris = new HashSet<Uri>();
        #endregion

        #region properties

        public string Name
        {
            get => GetName(KindsOfNames.Legal);
            set => UpsertName(KindsOfNames.Legal, value);
        }
        public IEnumerable<Uri> NetUri => _netUris;
        public string Description { get; set; }
        public PostalAddress MailingAddress { get; set; }
        public PostalAddress BusinessAddress { get; set; }
        public NorthAmericanPhone[] Phone { get; set; }
        public StandardIndustryClassification SIC { get; set; }
        public NaicsPrimarySector PrimarySector
        {
            get
            {
                if (_primarySector == null && SIC != null) 
                    ResolveNaicsOnSic();
                return _primarySector;
            }
            set => _primarySector = value;
        }

        public NaicsSector Sector
        {
            get
            {
                if(_sector == null && SIC != null)
                    ResolveNaicsOnSic();
                return _sector;
            }
            set => _sector = value;
        }

        public NaicsMarket Market
        {
            get
            {
                if(_market == null && SIC != null)
                    ResolveNaicsOnSic();
                return _market;
            }
            set => _market = value;
        }

        public int FiscalYearEndDay
        {
            get => _fiscalYearEndDay;
            set => _fiscalYearEndDay = value;
        }
        #endregion

        #region methods
        public abstract void LoadXrefXmlData();

        protected internal void ResolveNaicsOnSic()
        {
            if (SIC == null)
                return;
            var naics = StandardIndustryClassification.LookupNaicsBySic(SIC);
            if (naics == null)
                return;
            _primarySector = naics.Item1;
            _sector = naics.Item2;
            _market = naics.Item3;
        }

        private static Dictionary<string, string> _fullWord2Abbrev = new Dictionary<string, string>();

        internal static Dictionary<string, string> FullWord2Abbrev
        {
            get
            {
                if (_fullWord2Abbrev.Count > 0)
                    return _fullWord2Abbrev;

                _fullWord2Abbrev.Add(" AND", " &");
                _fullWord2Abbrev.Add(" DOLLAR", " $");
                _fullWord2Abbrev.Add(" PERCENT", " %");
                _fullWord2Abbrev.Add("ACCEPTANCE", "ACCEP ");
                _fullWord2Abbrev.Add("ACCIDENT", "ACC");
                _fullWord2Abbrev.Add("ADJUSTED", "ADJ");
                _fullWord2Abbrev.Add("ADJUSTMENT", "ADJ");
                _fullWord2Abbrev.Add("ADMINISTRATION", "ADMIN");
                _fullWord2Abbrev.Add("AGRICULTURAL", "AGRIC");
                _fullWord2Abbrev.Add("AGENCY", "AGY");
                _fullWord2Abbrev.Add("AIRLINE", "AIRL");
                _fullWord2Abbrev.Add("AIRPORT", "ARPT");
                _fullWord2Abbrev.Add("AIRWAY", "AWY");
                _fullWord2Abbrev.Add("ALABAMA", "ALA");
                _fullWord2Abbrev.Add("ALBERTA", "ALTA");
                _fullWord2Abbrev.Add("ALUMINUM", "ALUM");
                _fullWord2Abbrev.Add("AMALGAMATED", "AMAL");
                _fullWord2Abbrev.Add("AMERICA", "AMER");
                _fullWord2Abbrev.Add("AMERICAN", "AMERN");
                _fullWord2Abbrev.Add("AMERICAN DEPOSITORY", "ADR");
                _fullWord2Abbrev.Add("AMORTIZATION", "AMORT");
                _fullWord2Abbrev.Add("ANTICIPATION", "ANTIC");
                _fullWord2Abbrev.Add("APARTMENT", "APT");
                _fullWord2Abbrev.Add("ARIZONA", "ARIZ");
                _fullWord2Abbrev.Add("ARKANSAS", "ARK");
                _fullWord2Abbrev.Add("ASSENTED", "ASNTD");
                _fullWord2Abbrev.Add("ASSESSMENT", "ASSMT");
                _fullWord2Abbrev.Add("ASSOCIATED", "ASSD");
                _fullWord2Abbrev.Add("ASSOCIATES", "ASSOC");
                _fullWord2Abbrev.Add("ASSOCIATION", "ASSN");
                _fullWord2Abbrev.Add("ASSURANCE", "ASSURN");
                _fullWord2Abbrev.Add("ASSURED", "ASSUR");
                _fullWord2Abbrev.Add("ATTACHED", "ATT");
                _fullWord2Abbrev.Add("AUDITORIUM", "AUD");
                _fullWord2Abbrev.Add("AUTHORITY", "AUTH");
                _fullWord2Abbrev.Add("AUTHORIZED", "AUTHZ");
                _fullWord2Abbrev.Add("AVENUE", "AVE");
                _fullWord2Abbrev.Add("BANK", "BK");
                _fullWord2Abbrev.Add("BANKING", "BKG");
                _fullWord2Abbrev.Add("BEARER", "BR");
                _fullWord2Abbrev.Add("BENEFICIAL", "BEN");
                _fullWord2Abbrev.Add("BENEVOLENT", "BENEV");
                _fullWord2Abbrev.Add("BOARD", "BRD");
                _fullWord2Abbrev.Add("BOND", "BD");
                _fullWord2Abbrev.Add("BOROUGH", "BORO");
                _fullWord2Abbrev.Add("BOULEVARD", "BLVD");
                _fullWord2Abbrev.Add("BRANCH", "BRH");
                _fullWord2Abbrev.Add("BRIDGE", "BRDG");
                _fullWord2Abbrev.Add("BRITAIN", "BRITN");
                _fullWord2Abbrev.Add("BRITISH", "BRIT");
                _fullWord2Abbrev.Add("BRITISH COLUMBIA", "B C");
                _fullWord2Abbrev.Add("BROTHERS", "BROS");
                _fullWord2Abbrev.Add("BUILDER", "BLDR");
                _fullWord2Abbrev.Add("BUILDING", "BLDG");
                _fullWord2Abbrev.Add("BUREAU", "BUR");
                _fullWord2Abbrev.Add("CALIFORNIA", "CALIF");
                _fullWord2Abbrev.Add("CALLABLE", "CALL");
                _fullWord2Abbrev.Add("CANADA", "CDA");
                _fullWord2Abbrev.Add("CANADIAN", "CDN");
                _fullWord2Abbrev.Add("CANAL ZONE", "C Z");
                _fullWord2Abbrev.Add("CAPITAL", "CAP");
                _fullWord2Abbrev.Add("CASUALTY", "CAS");
                _fullWord2Abbrev.Add("CAUSEWAY", "CSWY");
                _fullWord2Abbrev.Add("CEMENT", "CEM");
                _fullWord2Abbrev.Add("CENTER", "CTR");
                _fullWord2Abbrev.Add("CENTRAL", "CENT");
                _fullWord2Abbrev.Add("CENTURY", "CENTY");
                _fullWord2Abbrev.Add("CERTIFICATE", "CTF");
                _fullWord2Abbrev.Add("CHEMICAL", "CHEM");
                _fullWord2Abbrev.Add("CIGARETTE", "CIG");
                _fullWord2Abbrev.Add("CITIZEN", "CTZN");
                _fullWord2Abbrev.Add("CLASS", "CL");
                _fullWord2Abbrev.Add("COLLATERAL", "COLL");
                _fullWord2Abbrev.Add("COLORADO", "COLO");
                _fullWord2Abbrev.Add("COMMERCE", "COMM");
                _fullWord2Abbrev.Add("COMMERCIAL", "COML");
                _fullWord2Abbrev.Add("COMMISSION", "COMMN");
                _fullWord2Abbrev.Add("COMMISSIONER", "COMMR");
                _fullWord2Abbrev.Add("COMMON", "COM");
                _fullWord2Abbrev.Add("COMMONWEALTH", "COMWLTH");
                _fullWord2Abbrev.Add("COMMUNITY", "CMNTY");
                _fullWord2Abbrev.Add("COMPANY", "CO");
                _fullWord2Abbrev.Add("CONNECTICUT", "CONN");
                _fullWord2Abbrev.Add("CONSERVATION", "CONSV");
                _fullWord2Abbrev.Add("CONSOLIDATION", "CONSLDTN");
                _fullWord2Abbrev.Add("CONSTRUCTION", "CONSTR");
                _fullWord2Abbrev.Add("CONTINENTAL", "CONTL");
                _fullWord2Abbrev.Add("CONTROL", "CTL");
                _fullWord2Abbrev.Add("CONVERTIBLE", "CONV");
                _fullWord2Abbrev.Add("COOPERATIVE", "COOP");
                _fullWord2Abbrev.Add("CORPORATION", "CORP");
                _fullWord2Abbrev.Add("COUNTRY", "CTRY");
                _fullWord2Abbrev.Add("COUNTY", "CNTY");
                _fullWord2Abbrev.Add("COUPON", "CPN");
                _fullWord2Abbrev.Add("COURT", "CT");
                _fullWord2Abbrev.Add("CREDIT", "CR");
                _fullWord2Abbrev.Add("CUMULATIVE", "CUM");
                _fullWord2Abbrev.Add("DATED", "DTD");
                _fullWord2Abbrev.Add("DEBENTURE", "DEB");
                _fullWord2Abbrev.Add("DEFERRED", "DEFD");
                _fullWord2Abbrev.Add("DELAWARE", "DEL");
                _fullWord2Abbrev.Add("DEPARTMENT", "DEPT");
                _fullWord2Abbrev.Add("DEPOSIT", "DEP");
                _fullWord2Abbrev.Add("DEVELOPMENT", "DEV");
                _fullWord2Abbrev.Add("DISCOUNT", "DISC");
                _fullWord2Abbrev.Add("DISPOSAL", "DISP");
                _fullWord2Abbrev.Add("DISTRIBUTING", "DISTRG");
                _fullWord2Abbrev.Add("DISTRIBUTION", "DISTR");
                _fullWord2Abbrev.Add("DISTRIBUTORS", "DISTRS");
                _fullWord2Abbrev.Add("DISTRICT", "DIST");
                _fullWord2Abbrev.Add("DISTRICT OF COLUMBIA", "D C");
                _fullWord2Abbrev.Add("DIVIDEND", "DIVID");
                _fullWord2Abbrev.Add("DIVISION", "DIV");
                _fullWord2Abbrev.Add("DOLLAR", "DLR");
                _fullWord2Abbrev.Add("DORMITORY", "DORM");
                _fullWord2Abbrev.Add("DRAINAGE", "DRAIN");
                _fullWord2Abbrev.Add("EASTERN", "EASTN");
                _fullWord2Abbrev.Add("EDUCATION", "ED");
                _fullWord2Abbrev.Add("EDUCATIONAL", "EDL");
                _fullWord2Abbrev.Add("ELECTRIC", "ELEC");
                _fullWord2Abbrev.Add("ELECTRONIC", "ELECTR");
                _fullWord2Abbrev.Add("ELEMENTARY", "ELEM");
                _fullWord2Abbrev.Add("ELIMINATION", "ELIM");
                _fullWord2Abbrev.Add("ENGINEERING", "ENGR");
                _fullWord2Abbrev.Add("ENGLAND", "ENG");
                _fullWord2Abbrev.Add("ENTERTAINMENT EQUIPEQUIPMENT", "ENTMT");
                _fullWord2Abbrev.Add("ET CETERA", "ETC");
                _fullWord2Abbrev.Add("EXEMPTED", "EXMP");
                _fullWord2Abbrev.Add("EXHIBITION", "EXHIB");
                _fullWord2Abbrev.Add("EXPIRE", "EXP");
                _fullWord2Abbrev.Add("EXPLORATION", "EXPL");
                _fullWord2Abbrev.Add("EXPORT", "EXPT");
                _fullWord2Abbrev.Add("EXPRESSWAY", "EXPWY");
                _fullWord2Abbrev.Add("EXTENDED", "EXTD");
                _fullWord2Abbrev.Add("EXTENSION", "EXTN");
                _fullWord2Abbrev.Add("EXTERNAL", "EXTL");
                _fullWord2Abbrev.Add("FACILITY", "FAC");
                _fullWord2Abbrev.Add("FARMER", "FMR");
                _fullWord2Abbrev.Add("FEDERAL", "FED");
                _fullWord2Abbrev.Add("FEDERATED", "FEDT");
                _fullWord2Abbrev.Add("FEDERATION", "FEDN");
                _fullWord2Abbrev.Add("FIDELITY", "FID");
                _fullWord2Abbrev.Add("FINANCE", "FIN");
                _fullWord2Abbrev.Add("FINANCIAL", "FINL");
                _fullWord2Abbrev.Add("FINANCING", "FING");
                _fullWord2Abbrev.Add("FLORIDA", "FLA");
                _fullWord2Abbrev.Add("FOREIGN", "FGN");
                _fullWord2Abbrev.Add("FOREST", "FST");
                _fullWord2Abbrev.Add("FORT", "FT");
                _fullWord2Abbrev.Add("FOUNDATION", "FNDTN");
                _fullWord2Abbrev.Add("FOUNDRY", "FDRY");
                _fullWord2Abbrev.Add("FRACTIONAL", "FR");
                _fullWord2Abbrev.Add("FREIGHT", "FGHT");
                _fullWord2Abbrev.Add("FUND", "FD");
                _fullWord2Abbrev.Add("FUNDING", "FDG");
                _fullWord2Abbrev.Add("GENERAL", "GEN");
                _fullWord2Abbrev.Add("GEORGIA", "GA");
                _fullWord2Abbrev.Add("GOVERNMENT", "GOVT");
                _fullWord2Abbrev.Add("GRANT", "GRNT");
                _fullWord2Abbrev.Add("GREATER", "GTR");
                _fullWord2Abbrev.Add("GUARANTEE", "GTEE");
                _fullWord2Abbrev.Add("GUARANTEED", "GTD");
                _fullWord2Abbrev.Add("GUARANTY", "GTY");
                _fullWord2Abbrev.Add("GYMNASIUM", "GYM");
                _fullWord2Abbrev.Add("HARBOR", "HBR");
                _fullWord2Abbrev.Add("HIGHWAY", "HWY");
                _fullWord2Abbrev.Add("HOLDING", "HLDG");
                _fullWord2Abbrev.Add("HOSPITAL", "HOSP");
                _fullWord2Abbrev.Add("HOUSING", "HSG");
                _fullWord2Abbrev.Add("ILLINOIS", "ILL");
                _fullWord2Abbrev.Add("ILLUMINATING", "ILLUM");
                _fullWord2Abbrev.Add("IMPROVEMENT", "IMPT");
                _fullWord2Abbrev.Add("INCINERATOR", "INCIN");
                _fullWord2Abbrev.Add("INCLUSIVE", "INCL");
                _fullWord2Abbrev.Add("INCORPORATE", "INC");
                _fullWord2Abbrev.Add("INCORPORATED", "INC");
                _fullWord2Abbrev.Add("INDEBTEDNESS", "INDBT");
                _fullWord2Abbrev.Add("INDEMNITY", "INDTY");
                _fullWord2Abbrev.Add("INDENTURE", "INDENT");
                _fullWord2Abbrev.Add("INDEPENDENT", "INDPT");
                _fullWord2Abbrev.Add("INDIANA", "IND");
                _fullWord2Abbrev.Add("INDUSTRIAL", "INDL");
                _fullWord2Abbrev.Add("INDUSTRY", "IND");
                _fullWord2Abbrev.Add("INSTITUTE", "INST");
                _fullWord2Abbrev.Add("INSTITUTION", "INSTN");
                _fullWord2Abbrev.Add("INSTITUTIONAL", "INSTL");
                _fullWord2Abbrev.Add("INSTRUCTION", "INSTRN");
                _fullWord2Abbrev.Add("INSTRUMENT", "INSTR");
                _fullWord2Abbrev.Add("INSURANCE", "INS");
                _fullWord2Abbrev.Add("INSURED", "INSD");
                _fullWord2Abbrev.Add("INTEREST", "INT");
                _fullWord2Abbrev.Add("INTERMEDIATE", "INTER");
                _fullWord2Abbrev.Add("INTERNATIONAL", "INTL");
                _fullWord2Abbrev.Add("INTERSTATE", "INTST");
                _fullWord2Abbrev.Add("INVESTMENT", "INVT");
                _fullWord2Abbrev.Add("INVESTOR", "INV");
                _fullWord2Abbrev.Add("IRRIGATION", "IRR");
                _fullWord2Abbrev.Add("ISLAND", "IS");
                _fullWord2Abbrev.Add("JOINT", "JT");
                _fullWord2Abbrev.Add("JUNCTION", "JCT");
                _fullWord2Abbrev.Add("JUNIOR", "JR");
                _fullWord2Abbrev.Add("KANSAS", "KANS");
                _fullWord2Abbrev.Add("KENTUCKY", "KY");
                _fullWord2Abbrev.Add("LABORATORY", "LAB");
                _fullWord2Abbrev.Add("LAND", "LD");
                _fullWord2Abbrev.Add("LIBRARY", "LIBR");
                _fullWord2Abbrev.Add("LIGHT", "LT");
                _fullWord2Abbrev.Add("LIGHTING", "LTG");
                _fullWord2Abbrev.Add("LIMITED", "LTD");
                _fullWord2Abbrev.Add("LIQUIDATION", "LIQ");
                _fullWord2Abbrev.Add("LOAN", "LN");
                _fullWord2Abbrev.Add("LOCAL", "LOC");
                _fullWord2Abbrev.Add("LOUISIANA", "LA");
                _fullWord2Abbrev.Add("LUMBER", "LMBR");
                _fullWord2Abbrev.Add("MACHINE", "MACH");
                _fullWord2Abbrev.Add("MACHINERY", "MACHY");
                _fullWord2Abbrev.Add("MAINE", "ME");
                _fullWord2Abbrev.Add("MANAGEMENT", "MGMT");
                _fullWord2Abbrev.Add("MANITOBA", "MAN");
                _fullWord2Abbrev.Add("MARYLAND", "MD");
                _fullWord2Abbrev.Add("MASSACHUSETTS", "MASS");
                _fullWord2Abbrev.Add("MATERIAL", "MATL");
                _fullWord2Abbrev.Add("MATURITY", "MAT");
                _fullWord2Abbrev.Add("MEDICAL", "MED");
                _fullWord2Abbrev.Add("MEMORIAL", "MEM");
                _fullWord2Abbrev.Add("METROPOLITAN", "MET");
                _fullWord2Abbrev.Add("MICHIGAN", "MICH");
                _fullWord2Abbrev.Add("MILLING", "MLG");
                _fullWord2Abbrev.Add("MILLS", "MLS");
                _fullWord2Abbrev.Add("MINING", "MNG");
                _fullWord2Abbrev.Add("MINNESOTA", "MINN");
                _fullWord2Abbrev.Add("MISSISSIPPI", "MISS");
                _fullWord2Abbrev.Add("MISSOURI", "MO");
                _fullWord2Abbrev.Add("MONTANA", "MONT");
                _fullWord2Abbrev.Add("MORTGAGE", "MTG");
                _fullWord2Abbrev.Add("MOTOR", "MTR");
                _fullWord2Abbrev.Add("MOUNT", "MT");
                _fullWord2Abbrev.Add("MOUNTAIN", "MTN");
                _fullWord2Abbrev.Add("MUNICIPAL", "MUN");
                _fullWord2Abbrev.Add("MUTUAL", "MUT");
                _fullWord2Abbrev.Add("NATIONAL", "NATL");
                _fullWord2Abbrev.Add("NATURAL", "NAT");
                _fullWord2Abbrev.Add("NAVIGATION", "NAV");
                _fullWord2Abbrev.Add("NEBRASKA", "NEB");
                _fullWord2Abbrev.Add("NETHERLANDS", "NETH");
                _fullWord2Abbrev.Add("NEVADA", "NEV");
                _fullWord2Abbrev.Add("NEW BRUNSWICK", "N B");
                _fullWord2Abbrev.Add("NEW HAMPSHIRE", "N H");
                _fullWord2Abbrev.Add("NEW JERSEY", "N J");
                _fullWord2Abbrev.Add("NEW MEXICO", "N MEX");
                _fullWord2Abbrev.Add("NEW YORK", "N Y");
                _fullWord2Abbrev.Add("NEWFOUNDLAND", "NFLD");
                _fullWord2Abbrev.Add("NORTH", "NORTH EASTN");
                _fullWord2Abbrev.Add("NORTH CAROLINA", "N C");
                _fullWord2Abbrev.Add("NORTH DAKOTA", "N D");
                _fullWord2Abbrev.Add("NORTHEASTERN", "NORTHEASTN");
                _fullWord2Abbrev.Add("NORTHERN", "NORTHN");
                _fullWord2Abbrev.Add("NORTHWESTERN", "NORTHWESTN");
                _fullWord2Abbrev.Add("NOTE", "NT");
                _fullWord2Abbrev.Add("NOVA SCOTIA", "N S");
                _fullWord2Abbrev.Add("NUMBER", "NO");
                _fullWord2Abbrev.Add("OBLIGATION", "OBLIG");
                _fullWord2Abbrev.Add("OKLAHOMA", "OKLA");
                _fullWord2Abbrev.Add("ONTARIO", "ONT");
                _fullWord2Abbrev.Add("OPERATING", "OPER");
                _fullWord2Abbrev.Add("OPTION", "OPT");
                _fullWord2Abbrev.Add("OPTIONAL", "OPTL");
                _fullWord2Abbrev.Add("ORDINARY", "ORD");
                _fullWord2Abbrev.Add("OREGON", "ORE");
                _fullWord2Abbrev.Add("ORIGINAL", "ORIG");
                _fullWord2Abbrev.Add("PACIFIC", "PAC");
                _fullWord2Abbrev.Add("PAR VALUE", "PAR");
                _fullWord2Abbrev.Add("PARK", "PK");
                _fullWord2Abbrev.Add("PARKING", "PKG");
                _fullWord2Abbrev.Add("PARKWAY", "PKWY");
                _fullWord2Abbrev.Add("PARTICIPATING", "PART");
                _fullWord2Abbrev.Add("PARTICIPATION", "PARTN");
                _fullWord2Abbrev.Add("PAVING", "PAV");
                _fullWord2Abbrev.Add("PAYABLE", "PAY");
                _fullWord2Abbrev.Add("PAYMENT", "PMT");
                _fullWord2Abbrev.Add("PENNSYLVANIA", "PA");
                _fullWord2Abbrev.Add("PERMANENT", "PERM");
                _fullWord2Abbrev.Add("PERPETUAL", "PERP");
                _fullWord2Abbrev.Add("PERSONAL", "PERS");
                _fullWord2Abbrev.Add("PETROLEUM", "PETE");
                _fullWord2Abbrev.Add("PLACE", "PL");
                _fullWord2Abbrev.Add("PLANT", "PLT");
                _fullWord2Abbrev.Add("PLAYGROUND", "PLGD");
                _fullWord2Abbrev.Add("PLEDGE", "PLG");
                _fullWord2Abbrev.Add("POLICY", "POL");
                _fullWord2Abbrev.Add("POWER", "PWR");
                _fullWord2Abbrev.Add("PREFERENCE", "PREF");
                _fullWord2Abbrev.Add("PREFERRED", "PFD");
                _fullWord2Abbrev.Add("PRELIMINARY", "PRELIM");
                _fullWord2Abbrev.Add("PREMIUM", "PREM");
                _fullWord2Abbrev.Add("PRIMARY", "PRIM");
                _fullWord2Abbrev.Add("PRINCIPAL", "PRIN");
                _fullWord2Abbrev.Add("PRINTING", "PRTG");
                _fullWord2Abbrev.Add("PRIOR", "PR");
                _fullWord2Abbrev.Add("PRIVILEGE", "PRIV");
                _fullWord2Abbrev.Add("PRODUCT", "PROD");
                _fullWord2Abbrev.Add("PRODUCTION", "PRODTN");
                _fullWord2Abbrev.Add("PROJECT", "PROJ");
                _fullWord2Abbrev.Add("PROMISSORY", "PROM");
                _fullWord2Abbrev.Add("PROPERTY", "PPTY");
                _fullWord2Abbrev.Add("PROTECTION", "PROTN");
                _fullWord2Abbrev.Add("PROTECTIVE", "PROT");
                _fullWord2Abbrev.Add("PROVINCE", "PROV");
                _fullWord2Abbrev.Add("PUBLIC", "PUB");
                _fullWord2Abbrev.Add("PUBLICATION", "PUBN");
                _fullWord2Abbrev.Add("PUBLISHING", "PUBG");
                _fullWord2Abbrev.Add("PUERTO RICO", "P R");
                _fullWord2Abbrev.Add("PURCHASE", "PUR");
                _fullWord2Abbrev.Add("PURPOSE", "PURP");
                _fullWord2Abbrev.Add("QUEBEC", "QUE");
                _fullWord2Abbrev.Add("RAILROAD", "RR");
                _fullWord2Abbrev.Add("RAILWAY", "RY");
                _fullWord2Abbrev.Add("REALTY", "RLTY");
                _fullWord2Abbrev.Add("RECEIPT", "RCPT");
                _fullWord2Abbrev.Add("RECONSTRUCTION", "RECON");
                _fullWord2Abbrev.Add("RECREATION", "REC");
                _fullWord2Abbrev.Add("REDEEMABLE", "RED");
                _fullWord2Abbrev.Add("REDEVELOPMENT", "REDEV");
                _fullWord2Abbrev.Add("REFINING", "REFNG");
                _fullWord2Abbrev.Add("REFRIGERATION", "REFRIG");
                _fullWord2Abbrev.Add("REFUNDING", "REF");
                _fullWord2Abbrev.Add("REGENTS", "REGT");
                _fullWord2Abbrev.Add("REGIONAL", "REGL");
                _fullWord2Abbrev.Add("REGISTERED", "REG");
                _fullWord2Abbrev.Add("REGULAR", "REGR");
                _fullWord2Abbrev.Add("REINSURANCE", "REINS");
                _fullWord2Abbrev.Add("RENTAL", "RENT");
                _fullWord2Abbrev.Add("REORGANIZATION", "REORGN");
                _fullWord2Abbrev.Add("REORGANIZED", "REORG");
                _fullWord2Abbrev.Add("REPRESENT", "REPST");
                _fullWord2Abbrev.Add("REPRESENTED", "REPSTD");
                _fullWord2Abbrev.Add("REPRESENTING", "REPSTG");
                _fullWord2Abbrev.Add("REPUBLIC", "REP");
                _fullWord2Abbrev.Add("RESEARCH", "RESH");
                _fullWord2Abbrev.Add("RESERVE", "RESV");
                _fullWord2Abbrev.Add("RESOURCES", "RES");
                _fullWord2Abbrev.Add("REVENUE", "REV");
                _fullWord2Abbrev.Add("RHODE ISLAND", "R I");
                _fullWord2Abbrev.Add("RIGHT", "RT");
                _fullWord2Abbrev.Add("RIVER", "RIV");
                _fullWord2Abbrev.Add("ROAD", "RD");
                _fullWord2Abbrev.Add("ROUTE", "RT");
                _fullWord2Abbrev.Add("ROYALTY", "RTY");
                _fullWord2Abbrev.Add("RUBBER", "RUBR");
                _fullWord2Abbrev.Add("SAINT", "ST");
                _fullWord2Abbrev.Add("SANITARY", "SAN");
                _fullWord2Abbrev.Add("SANITATION", "SANTN");
                _fullWord2Abbrev.Add("SASKATCHEWAN", "SASK");
                _fullWord2Abbrev.Add("SAVINGS", "SVGS");
                _fullWord2Abbrev.Add("SCHOOL", "SCH");
                _fullWord2Abbrev.Add("SECTION", "SECT");
                _fullWord2Abbrev.Add("SECURED", "SECD");
                _fullWord2Abbrev.Add("SECURITY", "SEC");
                _fullWord2Abbrev.Add("SENIOR", "SR");
                _fullWord2Abbrev.Add("SERIES", "SER");
                _fullWord2Abbrev.Add("SERVICE", "SVC");
                _fullWord2Abbrev.Add("SEWAGE", "SEW");
                _fullWord2Abbrev.Add("SEWER", "SWR");
                _fullWord2Abbrev.Add("SHARE", "SH");
                _fullWord2Abbrev.Add("SINKING FUND", "SF");
                _fullWord2Abbrev.Add("SMELTING", "SMLT");
                _fullWord2Abbrev.Add("SOCIETY", "SOC");
                _fullWord2Abbrev.Add("SOUTH", "SOUTH EASTN");
                _fullWord2Abbrev.Add("SOUTH CAROLINA", "S C");
                _fullWord2Abbrev.Add("SOUTH DAKOTA", "S D");
                _fullWord2Abbrev.Add("SOUTHEASTERN", "SOUTHEASTN");
                _fullWord2Abbrev.Add("SOUTHERN", "SOUTHN");
                _fullWord2Abbrev.Add("SOUTHWESTERN", "SOUTHWESTN");
                _fullWord2Abbrev.Add("SPECIAL", "SPL");
                _fullWord2Abbrev.Add("STADIUM", "STAD");
                _fullWord2Abbrev.Add("STAMPED", "STPD");
                _fullWord2Abbrev.Add("STANDARD", "STD");
                _fullWord2Abbrev.Add("STATE", "ST");
                _fullWord2Abbrev.Add("STATION", "STA");
                _fullWord2Abbrev.Add("STATUTORY", "STAT");
                _fullWord2Abbrev.Add("STEAMSHIP", "SS");
                _fullWord2Abbrev.Add("STEEL", "STL");
                _fullWord2Abbrev.Add("STOCK", "STK");
                _fullWord2Abbrev.Add("STOCKYARD", "STKYD");
                _fullWord2Abbrev.Add("STREET", "STR");
                _fullWord2Abbrev.Add("SUBDIVISION", "SUBDIV");
                _fullWord2Abbrev.Add("SUBORDINATED", "SUB");
                _fullWord2Abbrev.Add("SUBSCRIPTION", "SUBS");
                _fullWord2Abbrev.Add("SUBSTITUTE", "SUBT");
                _fullWord2Abbrev.Add("SUBURBAN", "SUBN");
                _fullWord2Abbrev.Add("SURPLUS", "SURP");
                _fullWord2Abbrev.Add("SWITZERLAND", "SWITZ");
                _fullWord2Abbrev.Add("SYNDICATE", "SYND");
                _fullWord2Abbrev.Add("SYSTEM", "SYS");
                _fullWord2Abbrev.Add("TELEPHONE", "TEL");
                _fullWord2Abbrev.Add("TERMINAL", "TERM");
                _fullWord2Abbrev.Add("TERRACE", "TER");
                _fullWord2Abbrev.Add("TERRITORY", "TERR");
                _fullWord2Abbrev.Add("TEXAS", "TEX");
                _fullWord2Abbrev.Add("THOROUGHFARE", "THORO");
                _fullWord2Abbrev.Add("THROUGH", "THRU");
                _fullWord2Abbrev.Add("THRUWAY", "TWY");
                _fullWord2Abbrev.Add("TOBACCO", "TOB");
                _fullWord2Abbrev.Add("TOLLWAY", "TWY");
                _fullWord2Abbrev.Add("TOWNSHIP", "TWP");
                _fullWord2Abbrev.Add("TRANSIT", "TRAN");
                _fullWord2Abbrev.Add("TRANSPORTATION", "TRANSN");
                _fullWord2Abbrev.Add("TREASURER", "TREASR");
                _fullWord2Abbrev.Add("TREASURY", "TREAS");
                _fullWord2Abbrev.Add("TRUST", "TR");
                _fullWord2Abbrev.Add("TUITION", "TUIT");
                _fullWord2Abbrev.Add("TUNNEL", "TUNL");
                _fullWord2Abbrev.Add("TURNPIKE", "TPK");
                _fullWord2Abbrev.Add("UNIFIED", "UNI");
                _fullWord2Abbrev.Add("UNION", "UN");
                _fullWord2Abbrev.Add("UNITED", "UTD");
                _fullWord2Abbrev.Add("UNITED ARAB REPUBLIC", "U A R");
                _fullWord2Abbrev.Add("UNITED KINGDOM", "U K");
                _fullWord2Abbrev.Add("UNITED STATES", "U S");
                _fullWord2Abbrev.Add("UNIVERSAL", "UNVL");
                _fullWord2Abbrev.Add("UNIVERSITY", "UNIV");
                _fullWord2Abbrev.Add("UTILITY", "UTIL");
                _fullWord2Abbrev.Add("VALLEY", "VY");
                _fullWord2Abbrev.Add("VARIOUS", "VAR");
                _fullWord2Abbrev.Add("VEHICLE", "VEH");
                _fullWord2Abbrev.Add("VERMONT", "VT");
                _fullWord2Abbrev.Add("VETERAN", "VET");
                _fullWord2Abbrev.Add("VICINITY", "VIC");
                _fullWord2Abbrev.Add("VILLAGE", "VLG");
                _fullWord2Abbrev.Add("VIRGIN ISLANDS", "V I");
                _fullWord2Abbrev.Add("VIRGINIA", "VA");
                _fullWord2Abbrev.Add("VOLUNTARY", "VOL");
                _fullWord2Abbrev.Add("VOTING", "VTG");
                _fullWord2Abbrev.Add("WAREHOUSE", "WHSE");
                _fullWord2Abbrev.Add("WARRANTS", "WT");
                _fullWord2Abbrev.Add("WASHINGTON", "WASH");
                _fullWord2Abbrev.Add("WATER", "WTR");
                _fullWord2Abbrev.Add("WATERWORKS", "WTRWKS");
                _fullWord2Abbrev.Add("WEST VIRGINIA", "W VA");
                _fullWord2Abbrev.Add("WESTERN", "WESTN");
                _fullWord2Abbrev.Add("WHOLESALE", "WHSL");
                _fullWord2Abbrev.Add("WHOLESALER", "WHSLR");
                _fullWord2Abbrev.Add("WISCONSIN", "WIS");
                _fullWord2Abbrev.Add("WORK", "WK");
                _fullWord2Abbrev.Add("WYOMING", "WYO");
                _fullWord2Abbrev.Add("YARD", "YD");
                _fullWord2Abbrev.Add("YEAR", "YR");
                return _fullWord2Abbrev;
            }
        }

        /// <summary>
        /// Transforms <see cref="someAbbrev"/> into its full name using the
        /// <see cref="FullWord2Abbrev"/> table.
        /// </summary>
        /// <param name="someAbbrev"></param>
        /// <returns></returns>
        public static string GetNameFull(string someAbbrev)
        {
            var ssout = new List<string>();
            foreach (var sp in someAbbrev.ToLower().Trim().Split(' '))
            {
                var dk = FullWord2Abbrev.FirstOrDefault(x => String.Equals(x.Value, sp, StringComparison.OrdinalIgnoreCase));
                var f = dk.Key ?? sp;

                ssout.Add(f);
            }
            return Etc.CapWords(String.Join(" ", ssout), ' ');
        }

        /// <summary>
        /// Transforms in <see cref="someString"/> into an abbreviated
        /// value using the <see cref="FullWord2Abbrev"/> table.
        /// </summary>
        /// <param name="someString"></param>
        /// <returns></returns>
        /// <remarks>
        /// Src [https://www.cusip.com/pdf/CUSIP_Intro_03.14.11.pdf]
        /// </remarks>
        public static string GetNameAbbrev(string someString)
        {
            var ssout = new List<string>();
            foreach (var sp in someString.ToUpper().Trim().Split(' '))
            {
                ssout.Add(FullWord2Abbrev.ContainsKey(sp) ? FullWord2Abbrev[sp] : sp);
            }

            var rejoined = String.Join(" ", ssout);
            foreach (var keyWithSp in FullWord2Abbrev.Keys.Where(x => x.Contains(" ")))
            {
                if (!rejoined.Contains(keyWithSp))
                    continue;
                var val = FullWord2Abbrev[keyWithSp];
                rejoined = rejoined.Replace(keyWithSp, val);
            }

            return rejoined;
        }

        /// <summary>
        /// Follows a standard from the CUSIP 
        /// Src [https://www.cusip.com/pdf/CUSIP_Intro_03.14.11.pdf]
        /// @ section 'RULES CONCERNING THE ISSUER’S NAME'
        /// also <see cref="GetNameAbbrev"/>.
        /// </summary>
        public static string GetSearchCompanyName(string someName)
        {
            if (String.IsNullOrWhiteSpace(someName))
                return String.Empty;
            var comName = new StringBuilder();

            //remove all non alpha-num chars except amp
            foreach (var c in someName.ToCharArray())
            {
                if (c == '&' || Char.IsLetterOrDigit(c))
                {
                    comName.Append(c);
                    continue;
                }
                comName.Append(" ");
            }

            //only one space between unique words
            var searchCompanyName = comName.ToString().ToUpper().DistillCrLf();

            //remove stop words 
            var nameParts = new List<string>();
            var searchNameWords = searchCompanyName.Split(' ');
            var stopWords = new[] { "IN", "OF", "THE" };
            foreach (var iWord in searchNameWords)
            {
                if (stopWords.Any(x => iWord == x))
                    continue;
                nameParts.Add(iWord);
            }

            return GetNameAbbrev(String.Join(" ", nameParts));
        }


        public void AddUri(Uri uri)
        {
            //don't allow callers to add telephone Uri's since there is another storage place for those
            if (uri != null && uri.Scheme != Tele.Phone.UriSchemaTelephone)
                _netUris.Add(uri);
        }

        public virtual void AddUri(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
                return;

            if (!uri.StartsWith(Uri.UriSchemeMailto) &&
                System.Text.RegularExpressions.Regex.IsMatch(uri, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
                uri = $"{Uri.UriSchemeMailto}:{uri}";
            if (!Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out var oUri))
                return;

            AddUri(oUri);
        }

        #endregion
    }
}
