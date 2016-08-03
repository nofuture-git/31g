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

        public UsStateData(UsState state) : this(state.ToString()) {  }

        public UsStateData(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;
            //need to put the spaces back into state's name (NewYork as New York)
            _stateName = name;
            if (TreeData.UsStateData == null)
                return;
            var myNameNode = TreeData.UsStateData.SelectSingleNode($"//state[@name='{_stateName}']");
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
                double crimeRate;
                if (crimeNode?.Attributes?["rate"]?.Value == null ||
                    !double.TryParse(crimeNode.Attributes?["rate"]?.Value, out crimeRate)) continue;
                PropertyCrime crime;
                if (!Enum.TryParse(propertyCrime, out crime))
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
                var crimeNode = xml.SelectSingleNode($"//state[@name='{_stateName}']//crime[@name='{violentCrime}']");
                double crimeRate;
                if (crimeNode?.Attributes?["rate"]?.Value == null ||
                    !double.TryParse(crimeNode?.Attributes?["rate"]?.Value, out crimeRate)) continue;
                ViolentCrime crime;
                if (!Enum.TryParse(violentCrime, out crime))
                    continue;
                ViolentCrimeRate.Add(new Tuple<ViolentCrime, double>(crime, crimeRate));
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
            double highSchoolGrad;
            if (!string.IsNullOrWhiteSpace(strVal) && double.TryParse(strVal, out highSchoolGrad))
            {
                PercentOfGrads.Add(new Tuple<OccidentalEdu, double>(OccidentalEdu.HighSchool, highSchoolGrad));
            }
            strVal = eduNode?.Attributes?["percent-college-grad"]?.Value;
            double collegeGrad;
            if (!string.IsNullOrWhiteSpace(strVal) && double.TryParse(strVal, out collegeGrad))
            {
                PercentOfGrads.Add(new Tuple<OccidentalEdu, double>(OccidentalEdu.College, collegeGrad));
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
                double percent;
                if (!double.TryParse(strPercent, out percent))
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
