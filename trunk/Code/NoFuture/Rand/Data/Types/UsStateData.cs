using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Gov;
using NoFuture.Util.Math;

namespace NoFuture.Rand.Data.Types
{
    public class UsStateData
    {
        #region fields
        private readonly string _stateName;
        #endregion

        #region ctor
        public UsStateData(string name)
        {
            _stateName = name;
            if (TreeData.UsStateData == null)
                return;
            var myNameNode = TreeData.UsStateData.SelectSingleNode($"//state[@name='{name}']");
            if (myNameNode == null)
                return;
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
        public List<Tuple<ViolentCrime, float>> ViolentCrimeRate { get; } = new List<Tuple<ViolentCrime, float>>();

        /// <summary>
        /// FBI Table 4 average by population 2005-2014
        /// </summary>
        public List<Tuple<PropertyCrime, float>> PropertyCrimeRate { get; } = new List<Tuple<PropertyCrime, float>>();

        /// <summary>
        /// BLS SMU series - see State2NaiscSuperSector.fsx
        /// </summary>
        public List<Tuple<NaicsSuperSector, float>> EmploymentSectors { get; } = new List<Tuple<NaicsSuperSector, float>>();

        /// <summary>
        /// https://en.wikipedia.org/wiki/List_of_U.S._states_by_educational_attainment
        /// </summary>
        public List<Tuple<OccidentalEdu, float>> PercentOfGrads { get; } = new List<Tuple<OccidentalEdu, float>>();
        #endregion

        #region methods
        protected void GetPropertyCrimeData()
        {
            if (string.IsNullOrWhiteSpace(_stateName))
                return;
            if (TreeData.UsStateData == null)
                return;
            var xml = TreeData.UsStateData;
            foreach (var propertyCrime in Enum.GetNames(typeof(PropertyCrime)))
            {
                var crimeNode = xml.SelectSingleNode($"//state[@name='{_stateName}']//crime[@name='{propertyCrime}']");
                float crimeRate;
                if (crimeNode?.Attributes?["rate"]?.Value == null ||
                    !float.TryParse(crimeNode.Attributes?["rate"]?.Value, out crimeRate)) continue;
                PropertyCrime crime;
                if (!Enum.TryParse(propertyCrime, out crime))
                    continue;
                PropertyCrimeRate.Add(new Tuple<PropertyCrime, float>(crime, crimeRate));
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
                var crimeNode = xml.SelectSingleNode($"//state[@name='{_stateName}']//crime[@name='{violentCrime}']");
                float crimeRate;
                if (crimeNode?.Attributes?["rate"]?.Value == null ||
                    !float.TryParse(crimeNode?.Attributes?["rate"]?.Value, out crimeRate)) continue;
                ViolentCrime crime;
                if (!Enum.TryParse(violentCrime, out crime))
                    continue;
                ViolentCrimeRate.Add(new Tuple<ViolentCrime, float>(crime, crimeRate));
            }
        }

        protected void GetEduData()
        {
            if (string.IsNullOrWhiteSpace(_stateName))
                return;
            if (TreeData.UsStateData == null)
                return;

            var eduNode = TreeData.UsStateData.SelectSingleNode($"//state[@name='{_stateName}']/edu-data");
            var strVal = eduNode?.Attributes?["percent-highschool-grad"]?.Value;
            float highSchoolGrad;
            if (!string.IsNullOrWhiteSpace(strVal) && float.TryParse(strVal, out highSchoolGrad))
            {
                PercentOfGrads.Add(new Tuple<OccidentalEdu, float>(OccidentalEdu.HighSchool, highSchoolGrad));
            }
            strVal = eduNode?.Attributes?["percent-college-grad"]?.Value;
            float collegeGrad;
            if (!string.IsNullOrWhiteSpace(strVal) && float.TryParse(strVal, out collegeGrad))
            {
                PercentOfGrads.Add(new Tuple<OccidentalEdu, float>(OccidentalEdu.College, collegeGrad));
            }
        }

        protected void GetEmploymentSectorData()
        {
            if (string.IsNullOrWhiteSpace(_stateName))
                return;
            if (TreeData.UsStateData == null)
                return;
            for (var ssId = 10; ssId <= 90; ssId += 10)
            {
                var emplyData =
                    TreeData.UsStateData.SelectSingleNode($"//state[@name='{_stateName}']//sector[@category-ID='{ssId}']");
                if (emplyData?.Attributes?["percent-employed"]?.Value == null)
                    continue;
                var strPercent = emplyData.Attributes["percent-employed"].Value;
                float percent;
                if (!float.TryParse(strPercent, out percent))
                    continue;
                var ss = NorthAmericanIndustryClassification.AllSectors.FirstOrDefault(x => x.Value == ssId.ToString());
                if (ss == null)
                    continue;
                EmploymentSectors.Add(new Tuple<NaicsSuperSector, float>(ss, percent));
            }
        }
        #endregion 
    }
}
