using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Gov.US
{
    /// <summary>
    /// Represents statistical data associated a particular US State
    /// </summary>
    [Serializable]
    public class UsStateData
    {
        #region constants
        protected const string RATE = "rate";
        protected const string NATIONAL = "national";
        protected const string NAME = "name";
        protected const string CRIME = "crime";
        private const string US_STATES_DATA = "US_States_Data.xml";
        internal static XmlDocument UsStateDataXml;
        #endregion

        #region fields
        private readonly string _stateName;
        private readonly string _nodeName;
        #endregion

        #region ctor

        public UsStateData(string name)
        {
            const string REGION = "region";
            const string STATE = "state";
            _nodeName = STATE;
            if (string.IsNullOrWhiteSpace(name) || string.Equals(name, "United States", StringComparison.OrdinalIgnoreCase))
            {
                _nodeName = "national";
                _stateName = "United States";
            }
            else
            {
                var usState = UsState.GetState(name);
                if (usState == null)
                    return;
                _stateName = usState.ToString();
            }

            UsStateDataXml = UsStateDataXml ??
                             Core.XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_STATES_DATA,
                                 Assembly.GetExecutingAssembly());
            var myNameNode = UsStateDataXml?.SelectSingleNode($"//{_nodeName}[@{NAME}='{_stateName}']") as XmlElement;
            if (myNameNode == null)
                return;
            if (myNameNode.HasAttribute(REGION) &&
                Enum.TryParse(myNameNode.Attributes[REGION].Value, out AmericanRegion reg))
                Region = reg;
            AverageEarnings = GetAvgEarningsPerYear(myNameNode);
            GetEmploymentSectorData(UsStateDataXml);
            GetEduData(UsStateDataXml);
            GetViolentCrimeData(UsStateDataXml);
            GetPropertyCrimeData(UsStateDataXml);
        }
        #endregion

        #region properties
        /// <summary>
        /// BEA data - see MsaAvgEarningPerYear.fsx and CountyAvgEarningPerYear.fsx
        /// </summary>
        public LinearEquation AverageEarnings { get; }

        /// <summary>
        /// FBI Table 4 average by population 2005-2014
        /// </summary>
        public List<Tuple<ViolentCrime, double>> ViolentCrimeRate { get; } = new List<Tuple<ViolentCrime, double>>();

        /// <summary>
        /// FBI Table 4 average by population 2005-2014
        /// </summary>
        public List<Tuple<PropertyCrime, double>> PropertyCrimeRate { get; } = new List<Tuple<PropertyCrime, double>>();

        /// <summary>
        /// BLS SMU series - see State2NaiscSuperSector.fsx
        /// </summary>
        public List<Tuple<string, double>> EmploymentSectors { get; } = new List<Tuple<string, double>>();

        /// <summary>
        /// https://en.wikipedia.org/wiki/List_of_U.S._states_by_educational_attainment
        /// </summary>
        public List<Tuple<OccidentalEdu, double>> PercentOfGrads { get; } = new List<Tuple<OccidentalEdu, double>>();

        public AmericanRegion Region { get; }

        public string StateName => _stateName;

        #endregion

        #region methods


        /// <summary>
        /// Reads the data from the embedded data file
        /// </summary>
        /// <returns></returns>
        public static UsStateData GetStateData(string stateName)
        {
            return new UsStateData(stateName);
        }

        protected void GetPropertyCrimeData(XmlDocument xml = null)
        {

            if (string.IsNullOrWhiteSpace(_stateName))
                return;
            xml = UsStateDataXml ??
                             Core.XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_STATES_DATA,
                                 Assembly.GetExecutingAssembly());
            if (xml == null)
                return;
            
            foreach (var propertyCrime in Enum.GetNames(typeof(PropertyCrime)))
            {
                var crimeNode = xml.SelectSingleNode($"//{_nodeName}[@{NAME}='{_stateName}']//{CRIME}[@{NAME}='{propertyCrime}']");
                if (crimeNode?.Attributes?[RATE]?.Value == null ||
                    !double.TryParse(crimeNode.Attributes?[RATE]?.Value, out var crimeRate)) continue;
                if (!Enum.TryParse(propertyCrime, out PropertyCrime crime))
                    continue;
                PropertyCrimeRate.Add(new Tuple<PropertyCrime, double>(crime, crimeRate));
            }
        }

        protected void GetViolentCrimeData(XmlDocument xml = null)
        {
            if (string.IsNullOrWhiteSpace(_stateName))
                return;
            xml = UsStateDataXml ??
                  Core.XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_STATES_DATA,
                      Assembly.GetExecutingAssembly());
            if (xml == null)
                return;

            foreach (var violentCrime in Enum.GetNames(typeof(ViolentCrime)))
            {
                var crimeNode = xml.SelectSingleNode($"//{_nodeName}[@{NAME}='{_stateName}']//{CRIME}[@{NAME}='{violentCrime}']");
                if (crimeNode?.Attributes?[RATE]?.Value == null ||
                    !double.TryParse(crimeNode?.Attributes?[RATE]?.Value, out var crimeRate)) continue;
                if (!Enum.TryParse(violentCrime, out ViolentCrime crime))
                    continue;
                ViolentCrimeRate.Add(new Tuple<ViolentCrime, double>(crime, crimeRate));
            }
        }

        protected void GetEduData(XmlDocument xml = null)
        {
            const string EDU_DATA = "edu-data";
            const string PERCENT_HIGHSCHOOL_GRAD = "percent-highschool-grad";
            const string PERCENT_COLLEGE_GRAD = "percent-college-grad";
            if (string.IsNullOrWhiteSpace(_stateName))
                return;
            xml = UsStateDataXml ??
                  Core.XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_STATES_DATA,
                      Assembly.GetExecutingAssembly());
            if (xml == null)
                return;

            var eduNode = xml.SelectSingleNode($"//{_nodeName}[@{NAME}='{_stateName}']/{EDU_DATA}");
            var strVal = eduNode?.Attributes?[PERCENT_HIGHSCHOOL_GRAD]?.Value;
            if (!string.IsNullOrWhiteSpace(strVal) && double.TryParse(strVal, out var highSchoolGrad))
            {
                PercentOfGrads.Add(new Tuple<OccidentalEdu, double>(OccidentalEdu.HighSchool | OccidentalEdu.Grad,
                    Math.Round(highSchoolGrad, 2)));
            }
            strVal = eduNode?.Attributes?[PERCENT_COLLEGE_GRAD]?.Value;
            if (!string.IsNullOrWhiteSpace(strVal) && double.TryParse(strVal, out var collegeGrad))
            {
                PercentOfGrads.Add(new Tuple<OccidentalEdu, double>(OccidentalEdu.Bachelor | OccidentalEdu.Grad,
                    Math.Round(collegeGrad, 2)));
            }
        }

        protected void GetEmploymentSectorData(XmlDocument xml = null)
        {
            const string SECTOR = "sector";
            const string CATEGORY_ID = "category-ID";
            const string PERCENT_EMPLOYED = "percent-employed";
            if (string.IsNullOrWhiteSpace(_stateName))
                return;
            xml = UsStateDataXml ??
                  Core.XmlDocXrefIdentifier.GetEmbeddedXmlDoc(US_STATES_DATA,
                      Assembly.GetExecutingAssembly());
            var econDataNodes = xml?.SelectNodes($"//{_nodeName}[@{NAME}='{_stateName}']//{SECTOR}");
            if (econDataNodes == null)
                return;
            for (var i = 0; i < econDataNodes.Count; i++)
            {
                var emplyData = econDataNodes.Item(i) as XmlElement;
                if (emplyData?.Attributes?[PERCENT_EMPLOYED]?.Value == null)
                    continue;
                var strPercent = emplyData.Attributes[PERCENT_EMPLOYED].Value;
                if (!double.TryParse(strPercent, out var percent))
                    continue;
                var description = emplyData.Attributes[CATEGORY_ID]?.Value ?? "";
                if (string.IsNullOrWhiteSpace(description))
                    continue;

                EmploymentSectors.Add(new Tuple<string, double>(description, percent));
            }
        }

        /// <summary>
        /// Converts the attributes data of the 'avg-earning-per-year' into 
        /// a <see cref="LinearEquation"/>
        /// </summary>
        /// <param name="someNode"></param>
        /// <returns></returns>
        protected internal static LinearEquation GetAvgEarningsPerYear(XmlNode someNode)
        {
            const string AVG_EARNINGS_PER_YEAR = "avg-earning-per-year";
            var stateNode = someNode as XmlElement;
            if (String.IsNullOrWhiteSpace(stateNode?.Attributes[AVG_EARNINGS_PER_YEAR]?.Value))
                return null;
            var attrVal = stateNode.Attributes[AVG_EARNINGS_PER_YEAR].Value;
            return !LinearEquation.TryParse(attrVal, out var lq) ? null : lq;
        }

        
        #endregion 
    }
}
