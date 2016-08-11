using System;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Edu;

namespace NoFuture.Rand.Domus
{
    public class NorthAmericanEdu : IEducation
    {

        #region ctor
        internal NorthAmericanEdu(Tuple<IHighSchool, DateTime?> assignHs)
        {
            HighSchool = assignHs;
        }

        internal NorthAmericanEdu(Tuple<IUniversity, DateTime?> assignUniv, Tuple<IHighSchool, DateTime?> assignHs)
        {
            HighSchool = assignHs;
            College = assignUniv;
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
            if (amer != null && !amer.IsLegalAdult(DateTime.Now))
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

            //get all hs for the state
            var hshs = homeState.GetHighSchools() ??
                       Gov.UsState.GetStateByPostalCode(UsCityStateZip.DF_STATE_ABBREV).GetHighSchools();

            //first try city, then state, last natl
            var hs = hshs.FirstOrDefault(x => x.PostalCode == hca?.AddressData?.PostalCode) ??
                         (hshs.Any() ? hshs[Etx.IntNumber(0, hshs.Length - 1)] : AmericanHighSchool.GetDefaultHs());

            //hs drop out
            if (Etx.TryAboveOrAt((int)Math.Round(hsGradRate)+1, Etx.Dice.OneHundred))
            {
                //assign grad hs but no date
                HighSchool = new Tuple<IHighSchool, DateTime?>(hs, null);
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
            IUniversity univ = null;
            //79 percent attend home state is a guess
            if (Etx.TryBelowOrAt(79, Etx.Dice.OneHundred) && homeState.GetUniversities().Any())
            {
                //pick a univ from the home state
                var stateUnivs = homeState.GetUniversities();
                if (!stateUnivs.Any())
                    return;

                univ = stateUnivs[Etx.IntNumber(0, stateUnivs.Length - 1)];
            }
            else
            {
                //pick a university from anywhere in the US
                var allUnivs = TreeData.AmericanUniversityData.SelectNodes("//state");
                if (allUnivs == null)
                    return;
                AmericanUniversity univOut;
                var randUnivXml = allUnivs[Etx.IntNumber(0, allUnivs.Count-1)] as XmlElement;
                if (randUnivXml == null)
                    return;
                if (AmericanUniversity.TryParseXml(randUnivXml, out univOut))
                {
                    univ = univOut;
                }
            }
            if (univ == null)
                return;
            //college grad
            if (Etx.TryBelowOrAt((int)Math.Round(univGradRate), Etx.Dice.OneHundred))
            {
                College = new Tuple<IUniversity, DateTime?>(univ, univGradDt);
                return;
            }
            //college drop-out
            College = new Tuple<IUniversity, DateTime?>(univ, null);

        }
        #endregion

        #region properties
        public OccidentalEdu EduLevel
        {
            get
            {
                var hasHs = HighSchool?.Item1 != null;
                var isHsGrad = HighSchool?.Item2 != null;
                var hasCollge = College?.Item1 != null;
                var isCollegeGrad = College?.Item2 != null;

                if (new[] { hasHs, isHsGrad, hasCollge, isCollegeGrad }.All(x => x == false))
                    return OccidentalEdu.None;
                if (hasCollge && isCollegeGrad)
                    return OccidentalEdu.Bachelor | OccidentalEdu.Grad;
                if (hasCollge)
                    return OccidentalEdu.Bachelor | OccidentalEdu.Some;
                if (hasHs && isHsGrad)
                    return OccidentalEdu.HighSchool | OccidentalEdu.Grad;

                return OccidentalEdu.HighSchool | OccidentalEdu.Some;
            }
        }

        public Tuple<IHighSchool, DateTime?> HighSchool { get; }
        public Tuple<IUniversity, DateTime?> College { get; }
        #endregion

        #region methods

        public override string ToString()
        {
            return string.Join(" ", HighSchool, College);
        }

        #endregion
    }
}
