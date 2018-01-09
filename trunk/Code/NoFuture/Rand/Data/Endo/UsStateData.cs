using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Data.Endo.Grps;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Data.Endo
{
    [Serializable]
    public class UsStateData
    {
        #region constants
        protected const string RATE = "rate";
        protected const string STATE = "state";
        protected const string NAME = "name";
        protected const string CRIME = "crime";
        #endregion

        #region fields
        private readonly string _stateName;
        #endregion

        #region ctor

        public UsStateData(string name)
        {
            const string REGION = "region";
            if (string.IsNullOrWhiteSpace(name))
                return;
            //need to put the spaces back into state's name (NewYork as New York)
            _stateName = name;
            if (TreeData.UsStateData == null)
                return;
            var myNameNode = TreeData.UsStateData.SelectSingleNode($"//{STATE}[@{NAME}='{_stateName}']") as XmlElement;
            if (myNameNode == null)
                return;
            if (Enum.TryParse(myNameNode.Attributes[REGION].Value, out AmericanRegion reg))
                Region = reg;
            AverageEarnings = UsCityStateZip.GetAvgEarningsPerYear(myNameNode);
            GetEmploymentSectorData();
            GetEduData();
            GetViolentCrimeData();
            GetPropertyCrimeData();
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
        public List<Tuple<NaicsSuperSector, double>> EmploymentSectors { get; } = new List<Tuple<NaicsSuperSector, double>>();

        /// <summary>
        /// https://en.wikipedia.org/wiki/List_of_U.S._states_by_educational_attainment
        /// </summary>
        public List<Tuple<OccidentalEdu, double>> PercentOfGrads { get; } = new List<Tuple<OccidentalEdu, double>>();

        public AmericanRegion Region { get; }
        #endregion

        #region methods


        /// <summary>
        /// Uses the data in <see cref="TreeData.UsStateData"/>
        /// </summary>
        /// <returns></returns>
        public static UsStateData GetStateData(string stateName)
        {
            if (string.IsNullOrWhiteSpace(stateName))
                return null;
            return new UsStateData(stateName);
        }

        protected void GetPropertyCrimeData()
        {

            if (string.IsNullOrWhiteSpace(_stateName))
                return;
            if (TreeData.UsStateData == null)
                return;
            var xml = TreeData.UsStateData;
            foreach (var propertyCrime in Enum.GetNames(typeof(PropertyCrime)))
            {
                var crimeNode = xml.SelectSingleNode($"//{STATE}[@{NAME}='{_stateName}']//{CRIME}[@{NAME}='{propertyCrime}']");
                if (crimeNode?.Attributes?[RATE]?.Value == null ||
                    !double.TryParse(crimeNode.Attributes?[RATE]?.Value, out var crimeRate)) continue;
                if (!Enum.TryParse(propertyCrime, out PropertyCrime crime))
                    continue;
                PropertyCrimeRate.Add(new Tuple<PropertyCrime, double>(crime, crimeRate));
            }
        }

        protected void GetViolentCrimeData()
        {
            if (string.IsNullOrWhiteSpace(_stateName))
                return;
            if (TreeData.UsStateData == null)
                return;
            var xml = TreeData.UsStateData;

            foreach (var violentCrime in Enum.GetNames(typeof(ViolentCrime)))
            {
                var crimeNode = xml.SelectSingleNode($"//{STATE}[@{NAME}='{_stateName}']//{CRIME}[@{NAME}='{violentCrime}']");
                if (crimeNode?.Attributes?[RATE]?.Value == null ||
                    !double.TryParse(crimeNode?.Attributes?[RATE]?.Value, out var crimeRate)) continue;
                if (!Enum.TryParse(violentCrime, out ViolentCrime crime))
                    continue;
                ViolentCrimeRate.Add(new Tuple<ViolentCrime, double>(crime, crimeRate));
            }
        }

        protected void GetEduData()
        {
            const string EDU_DATA = "edu-data";
            const string PERCENT_HIGHSCHOOL_GRAD = "percent-highschool-grad";
            const string PERCENT_COLLEGE_GRAD = "percent-college-grad";
            if (string.IsNullOrWhiteSpace(_stateName))
                return;
            if (TreeData.UsStateData == null)
                return;

            var eduNode = TreeData.UsStateData.SelectSingleNode($"//{STATE}[@{NAME}='{_stateName}']/{EDU_DATA}");
            var strVal = eduNode?.Attributes?[PERCENT_HIGHSCHOOL_GRAD]?.Value;
            if (!string.IsNullOrWhiteSpace(strVal) && double.TryParse(strVal, out var highSchoolGrad))
            {
                PercentOfGrads.Add(new Tuple<OccidentalEdu, double>(OccidentalEdu.HighSchool | OccidentalEdu.Grad, highSchoolGrad));
            }
            strVal = eduNode?.Attributes?[PERCENT_COLLEGE_GRAD]?.Value;
            if (!string.IsNullOrWhiteSpace(strVal) && double.TryParse(strVal, out var collegeGrad))
            {
                PercentOfGrads.Add(new Tuple<OccidentalEdu, double>(OccidentalEdu.Bachelor | OccidentalEdu.Grad, collegeGrad));
            }
        }

        protected void GetEmploymentSectorData()
        {
            const string SECTOR = "sector";
            const string CATEGORY_ID = "category-ID";
            const string PERCENT_EMPLOYED = "percent-employed";
            if (string.IsNullOrWhiteSpace(_stateName))
                return;
            if (TreeData.UsStateData == null)
                return;
            for (var ssId = 10; ssId <= 90; ssId += 10)
            {
                var emplyData =
                    TreeData.UsStateData.SelectSingleNode($"//{STATE}[@{NAME}='{_stateName}']//{SECTOR}[@{CATEGORY_ID}='{ssId}']");
                if (emplyData?.Attributes?[PERCENT_EMPLOYED]?.Value == null)
                    continue;
                var strPercent = emplyData.Attributes[PERCENT_EMPLOYED].Value;
                if (!double.TryParse(strPercent, out var percent))
                    continue;
                var ss = NorthAmericanIndustryClassification.AllSectors.FirstOrDefault(x => x.Value == ssId.ToString());
                if (ss == null)
                    continue;
                EmploymentSectors.Add(new Tuple<NaicsSuperSector, double>(ss, percent));
            }
        }
        #endregion 
    }
}
