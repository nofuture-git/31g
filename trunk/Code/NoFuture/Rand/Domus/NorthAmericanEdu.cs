using System;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Domus
{
    [Serializable]
    public class NorthAmericanEdu : IEducation
    {
        private string _eduLevel;
        private OccidentalEdu _eduFlag;

        #region ctor
        internal NorthAmericanEdu(Tuple<IHighSchool, DateTime?> assignHs)
        {
            HighSchool = assignHs;
            AssignEduFlagAndLevel();
        }

        internal NorthAmericanEdu(Tuple<IUniversity, DateTime?> assignUniv, Tuple<IHighSchool, DateTime?> assignHs)
        {
            HighSchool = assignHs;
            College = assignUniv;
            AssignEduFlagAndLevel();
        }

        /// <summary>
        /// Ctor using geography, age of <see cref="p"/>
        /// </summary>
        /// <param name="p">
        /// Optional, will return random if this is null or 
        /// not a type of <see cref="NorthAmerican"/>
        /// </param>
        public NorthAmericanEdu(IPerson p)
        {
            var amer = p as NorthAmerican;
            if (amer != null && amer.Age < 14)
                return;

            var dob = amer?.BirthCert?.DateOfBirth ?? NAmerUtil.GetWorkingAdultBirthDate();
            //determine where amer lived when they were 18
            var mother = amer?.BirthCert == null
                ? NAmerUtil.SolveForParent(dob,
                    NAmerUtil.Equations.FemaleAge2FirstMarriage,
                    Gender.Female) as NorthAmerican
                : amer.GetMother() as NorthAmerican;
            
            var dtAtAge18 = dob.AddYears(18);

            var hca = mother?.GetAddressAt(dtAtAge18)?.HomeCityArea ?? CityArea.American();
            var homeState = Gov.UsState.GetStateByPostalCode(hca?.AddressData?.StateAbbrv) ??
                            Gov.UsState.GetStateByPostalCode(UsCityStateZip.DF_STATE_ABBREV);

            //get hs grad data for state amer lived in when 18
            var hsGradData =
                homeState.GetStateData()
                    .PercentOfGrads.FirstOrDefault(x => x.Item1 == (OccidentalEdu.HighSchool | OccidentalEdu.Grad));

            //determine prob. of having hs grad
            var hsGradRate = hsGradData?.Item2 ?? AmericanHighSchool.DF_NATL_AVG;

            //first try city, then state, last natl
            var hs = GetAmericanHighSchool(homeState, hca);

            //still in hs or dropped out
            if (!(amer?.IsLegalAdult(DateTime.Now) ?? true) || Etx.TryAboveOrAt((int)Math.Round(hsGradRate)+1, Etx.Dice.OneHundred))
            {
                //assign grad hs but no date
                HighSchool = new Tuple<IHighSchool, DateTime?>(hs, null);
                AssignEduFlagAndLevel();
                return;
            }

            //get a date of when amer would be grad'ing from hs
            var hsGradDt = dtAtAge18;
            while (hsGradDt.Month != 5)
                hsGradDt = hsGradDt.AddMonths(1);
            hsGradDt = new DateTime(hsGradDt.Year, hsGradDt.Month, Etx.IntNumber(12, 28));

            //assign grad hs with grad date
            HighSchool = new Tuple<IHighSchool, DateTime?>(hs, hsGradDt);

            //get college grad data for same state as hs
            var univGradData =
                homeState.GetStateData()
                    .PercentOfGrads.FirstOrDefault(x => x.Item1 == (OccidentalEdu.Bachelor | OccidentalEdu.Grad));

            var univGradRate = univGradData?.Item2 ?? AmericanUniversity.DF_NATL_AVG;

            //roll for some college
            if (Etx.TryAboveOrAt((int)Math.Round(univGradRate * 2), Etx.Dice.OneHundred))
            {
                AssignEduFlagAndLevel();
                return;
            }

            //get random num of years as undergrad
            var yearsInCollege = Etx.RandomValueInNormalDist(4.67, 1.58);

            //get a date for when amer would grad from college
            var univGradDt = hsGradDt.AddYears((int)Math.Round(yearsInCollege));
            while (univGradDt.Month != 5)
                univGradDt = univGradDt.AddMonths(1);
            univGradDt = new DateTime(univGradDt.Year, univGradDt.Month, Etx.IntNumber(12, 28));

            //pick a univ 
            IUniversity univ = GetAmericanUniversity(homeState);
            if (univ == null)
            {
                AssignEduFlagAndLevel();
                return;
            }
            //college grad
            if (Etx.TryBelowOrAt((int)Math.Round(univGradRate), Etx.Dice.OneHundred))
            {
                College = new Tuple<IUniversity, DateTime?>(univ, univGradDt);
                AssignEduFlagAndLevel();
                return;
            }
            //college drop-out
            College = new Tuple<IUniversity, DateTime?>(univ, null);
            AssignEduFlagAndLevel();
        }
        #endregion

        #region properties

        public OccidentalEdu EduFlag => _eduFlag;
        /// <summary>
        /// Set to a readable string for JSON serialization.
        /// </summary>
        public string EduLevel => _eduLevel;
        
        public Tuple<IHighSchool, DateTime?> HighSchool { get; }
        public Tuple<IUniversity, DateTime?> College { get; }
        #endregion

        #region methods

        protected internal void AssignEduFlagAndLevel()
        {
            var hasHs = HighSchool?.Item1 != null;
            var isHsGrad = HighSchool?.Item2 != null;
            var hasCollge = College?.Item1 != null;
            var isCollegeGrad = College?.Item2 != null;

            if (new[] { hasHs, isHsGrad, hasCollge, isCollegeGrad }.All(x => x == false))
            {
                _eduLevel = "No Education";
                _eduFlag = OccidentalEdu.None;

            }
            if (hasCollge && isCollegeGrad)
            {
                _eduLevel = "College Grad";
                _eduFlag = OccidentalEdu.Bachelor | OccidentalEdu.Grad;
            }
            if (hasCollge)
            {
                _eduLevel = "Some College";
                _eduFlag = OccidentalEdu.Bachelor | OccidentalEdu.Some;
            }
            if (hasHs && isHsGrad)
            {
                _eduLevel = "High School Grad";
                _eduFlag = OccidentalEdu.HighSchool | OccidentalEdu.Grad;
            }
            else
            {
                _eduLevel = "High School dropout";
                _eduFlag = OccidentalEdu.HighSchool | OccidentalEdu.Some;
            }
        }

        public override string ToString()
        {
            return string.Join(" ", HighSchool, College);
        }

        public static AmericanUniversity GetAmericanUniversity(UsState homeState)
        {
            //pick a univ 
            IUniversity univ = null;
            int pick = 0;
            //79 percent attend home state is a guess
            if (Etx.TryBelowOrAt(79, Etx.Dice.OneHundred) && 
                homeState != null && homeState.GetUniversities().Any())
            {
                //pick a univ from the home state
                var stateUnivs = homeState.GetUniversities();
                if (!stateUnivs.Any())
                    return null;
                pick = Etx.IntNumber(0, stateUnivs.Length - 1);
                univ = stateUnivs[pick];
            }
            else
            {
                //pick a university from anywhere in the US
                var allUnivs = TreeData.AmericanUniversityData.SelectNodes("//state");
                if (allUnivs == null)
                    return null;
                AmericanUniversity univOut;
                pick = Etx.IntNumber(0, allUnivs.Count - 1);
                var randUnivXml = allUnivs[pick] as XmlElement;
                if (randUnivXml == null || !randUnivXml.HasChildNodes)
                    return null;
                pick = Etx.IntNumber(0, randUnivXml.ChildNodes.Count - 1);
                var univXmlNode = randUnivXml.ChildNodes[pick] as XmlElement;
                if (univXmlNode == null)
                    return null;
                if (AmericanUniversity.TryParseXml(univXmlNode, out univOut))
                {
                    univ = univOut;
                }
            }
            return (AmericanUniversity)univ;
        }

        public static AmericanHighSchool GetAmericanHighSchool(UsState homeState, CityArea hca)
        {
            //get all hs for the state
            var hshs = homeState.GetHighSchools() ??
                       Gov.UsState.GetStateByPostalCode(UsCityStateZip.DF_STATE_ABBREV).GetHighSchools();

            //first try city, then state, last natl
            var hs = hshs.FirstOrDefault(x => x.PostalCode == hca?.AddressData?.PostalCode) ??
                         (hshs.Any() ? hshs[Etx.IntNumber(0, hshs.Length - 1)] : AmericanHighSchool.GetDefaultHs());
            return hs;
        }
        #endregion
    }
}
