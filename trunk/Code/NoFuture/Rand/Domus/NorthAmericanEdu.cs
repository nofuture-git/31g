using System;
using System.Collections.Generic;
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
        #region fields
        private string _eduLevel;
        private OccidentalEdu _eduFlag;
        private Tuple<IHighSchool, DateTime?> _highSchool;
        private readonly List<Tuple<IUniversity, DateTime?>> _colleges = new List<Tuple<IUniversity, DateTime?>>();

        public const int DF_MIN_AGE_ENTER_HS = 14;
        #endregion

        #region ctor
        internal NorthAmericanEdu(Tuple<IHighSchool, DateTime?> assignHs)
        {
            _highSchool = assignHs;
            AssignEduFlagAndLevel();
        }

        internal NorthAmericanEdu(Tuple<IUniversity, DateTime?> assignUniv, Tuple<IHighSchool, DateTime?> assignHs)
        {
            _highSchool = assignHs;
            if(assignUniv != null)
                _colleges.Add(assignUniv);
            AssignEduFlagAndLevel();
        }

        /// <summary>
        /// Instantiates a new instance of <see cref="IEducation"/> using <see cref="p"/>
        /// </summary>
        /// <param name="p">
        /// Expected to be of type <see cref="NorthAmerican"/>
        /// </param>
        public NorthAmericanEdu(IPerson p)
        {
            var amer = p as NorthAmerican;

            //only deal with highschool and up
            if (amer != null && amer.Age < DF_MIN_AGE_ENTER_HS)
                return;

            var dob = amer?.BirthCert?.DateOfBirth ?? NAmerUtil.GetWorkingAdultBirthDate();
            //determine where amer lived when they were 18
            var mother = amer?.BirthCert == null
                ? NAmerUtil.SolveForParent(dob,
                    NAmerUtil.Equations.FemaleAge2FirstMarriage,
                    Gender.Female) as NorthAmerican
                : amer.GetMother() as NorthAmerican;

            var dtAtAge18 = dob.AddYears(UsState.AGE_OF_ADULT);

            var homeCityArea = mother?.GetAddressAt(dtAtAge18)?.HomeCityArea as UsCityStateZip ?? CityArea.American();

            var isLegalAdult = amer?.IsLegalAdult(DateTime.Now) ?? true;
            DateTime? hsGradDt;
            if (!AssignRandomHighSchool(homeCityArea, isLegalAdult, dtAtAge18, out hsGradDt))
                return;

            AssignRandomCollege(homeCityArea.State, hsGradDt);
        }

        #endregion

        #region properties

        public OccidentalEdu EduFlag => _eduFlag;
        /// <summary>
        /// Set to a readable string for JSON serialization.
        /// </summary>
        public string EduLevel => _eduLevel;
        
        public Tuple<IHighSchool, DateTime?> HighSchool { get {return _highSchool;} }

        public Tuple<IUniversity, DateTime?> College
        {
            get
            {
                return _colleges.Any(x => x?.Item2 != null)
                    ? _colleges.Where(x => x?.Item2 != null).OrderByDescending(x => x.Item2).FirstOrDefault()
                    : _colleges.FirstOrDefault();
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Assigns a value to <see cref="HighSchool"/> at random based on the given inputs
        /// </summary>
        /// <param name="homeCityArea"></param>
        /// <param name="isLegalAdult"></param>
        /// <param name="dtAtAge18"></param>
        /// <param name="hsGradDt"></param>
        /// <returns></returns>
        public bool AssignRandomHighSchool(UsCityStateZip homeCityArea, bool isLegalAdult, DateTime dtAtAge18,
            out DateTime? hsGradDt)
        {
            hsGradDt = null;

            if (homeCityArea == null)
                return false;

            //get hs grad data for state amer lived in when 18
            var hsGradData =
                homeCityArea.State.GetStateData()
                    .PercentOfGrads.FirstOrDefault(x => x.Item1 == (OccidentalEdu.HighSchool | OccidentalEdu.Grad));

            //determine prob. of having hs grad
            var hsGradRate = hsGradData?.Item2 ?? AmericanHighSchool.DF_NATL_AVG;

            //first try city, then state, last natl
            var hs = GetAmericanHighSchool(homeCityArea.State, homeCityArea);

            //still in hs or dropped out
            if (!isLegalAdult || Etx.TryAboveOrAt((int)Math.Round(hsGradRate) + 1, Etx.Dice.OneHundred))
            {
                //assign grad hs but no date
                _highSchool = new Tuple<IHighSchool, DateTime?>(hs, null);
                AssignEduFlagAndLevel();
                return false;
            }

            //get a date of when amer would be grad'ing from hs
            hsGradDt = dtAtAge18;
            while (hsGradDt.Value.Month != 5)
                hsGradDt = hsGradDt.Value.AddMonths(1);
            hsGradDt = new DateTime(hsGradDt.Value.Year, hsGradDt.Value.Month, Etx.IntNumber(12, 28));

            //assign grad hs with grad date
            _highSchool = new Tuple<IHighSchool, DateTime?>(hs, hsGradDt);
            return true;
        }

        /// <summary>
        /// Assigns a value to <see cref="College"/> at random based on the given values
        /// </summary>
        /// <param name="homeState"></param>
        /// <param name="hsGradDt"></param>
        public void AssignRandomCollege(UsState homeState, DateTime? hsGradDt)
        {
            if (hsGradDt == null)
                return;

            //get college grad data for same state as hs
            var univGradData =
                homeState.GetStateData()
                    .PercentOfGrads.FirstOrDefault(x => x.Item1 == (OccidentalEdu.Bachelor | OccidentalEdu.Grad));

            var bachelorGradRate = univGradData?.Item2 ??
                               AmericanUniversity.DefaultNationalAvgs.First(
                                   x => x.Key == (OccidentalEdu.Bachelor | OccidentalEdu.Grad)).Value;

            //roll for some college
            if (Etx.TryAboveOrAt((int)Math.Round(bachelorGradRate * 2), Etx.Dice.OneHundred))
            {
                AssignEduFlagAndLevel();
                return;
            }

            //pick a univ 
            IUniversity univ = GetAmericanUniversity(homeState);
            if (univ == null)
            {
                AssignEduFlagAndLevel();
                return;
            }

            if (!Etx.TryBelowOrAt((int)Math.Round(bachelorGradRate * 10), Etx.Dice.OneThousand))
            {
                //dropped out of college
                _colleges.Add(new Tuple<IUniversity, DateTime?>(univ, null));
                AssignEduFlagAndLevel();
                return;
            }

            //college grad
            //get a date for when amer would grad from college
            var univGradDt = GetRandomGraduationDate(hsGradDt.Value, NAmerUtil.Equations.YearsInUndergradCollege);

            _colleges.Add(new Tuple<IUniversity, DateTime?>(univ, univGradDt));

            //try for post-grad
            var postGradRate = AmericanUniversity.DefaultNationalAvgs.First(
                               x => x.Key == (OccidentalEdu.Master | OccidentalEdu.Grad)).Value;

            if (Etx.TryBelowOrAt((int)postGradRate * 10, Etx.Dice.OneThousand))
            {
                var postGradDt = GetRandomGraduationDate(univGradDt, NAmerUtil.Equations.YearsInPostgradCollege);
                var postGradUniv = GetAmericanUniversity(homeState);

                _colleges.Add(new Tuple<IUniversity, DateTime?>(postGradUniv, postGradDt));
            }

            AssignEduFlagAndLevel();
        }

        /// <summary>
        /// Gets a date around a semester&apos;s end moving 
        /// out (x) number of years, at random, from <see cref="fromHere"/>
        /// </summary>
        /// <param name="fromHere"></param>
        /// <param name="eq">Normal dist which determines (x) mentioned above.</param>
        /// <returns></returns>
        public static DateTime GetRandomGraduationDate(DateTime fromHere, Util.Math.NormalDistEquation eq)
        {
            var years = Etx.RandomValueInNormalDist(eq);

            var gradDt = fromHere.AddYears((int)Math.Round(years));
            while (gradDt.Month != 5 && gradDt.Month != 12)
                gradDt = gradDt.AddMonths(1);
            gradDt = new DateTime(gradDt.Year, gradDt.Month, Etx.IntNumber(12, 28));
            return gradDt;
        }

        /// <summary>
        /// Helper method to assign the <see cref="EduLevel"/> and <see cref="EduFlag"/>
        /// by this instances current fields.
        /// </summary>
        protected internal void AssignEduFlagAndLevel()
        {
            //order colleges where grad date desc
            var orderedColleges = _colleges.Where(x => x?.Item2 != null).OrderByDescending(x => x.Item2.Value);

            //determine predicates
            var hasHs = HighSchool?.Item1 != null;
            var isHsGrad = HighSchool?.Item2 != null;
            var hasUndergradCollege = _colleges.Any();
            var isCollegeGrad = orderedColleges.LastOrDefault()?.Item2 != null;
            var hasPostgradCollege = isCollegeGrad && orderedColleges.Count(x => x.Item2 != null) > 1;

            //determine number of years in post-grad
            var numYearsPostGrad = hasPostgradCollege
                ? (orderedColleges.First().Item2.Value - orderedColleges.Last().Item2.Value).TotalDays/
                  Shared.Constants.DBL_TROPICAL_YEAR
                : 0;

            //consider doctorate as right-side second sigma of postgrad years
            var isDocGrad = numYearsPostGrad >
                            NAmerUtil.Equations.YearsInPostgradCollege.Mean +
                            NAmerUtil.Equations.YearsInPostgradCollege.StdDev;

            //assign flag and name based on the above
            if (new[] { hasHs, isHsGrad, hasUndergradCollege, isCollegeGrad }.All(x => x == false))
            {
                _eduLevel = "No Education";
                _eduFlag = OccidentalEdu.None;
                return;
            }
            if (hasPostgradCollege)
            {
                _eduLevel = isDocGrad ? "Doctorate" : "Masters Grad";
                _eduFlag = isDocGrad
                    ? OccidentalEdu.Doctorate | OccidentalEdu.Grad
                    : OccidentalEdu.Master | OccidentalEdu.Grad;
                return;
            }
            if (hasUndergradCollege && isCollegeGrad)
            {
                _eduLevel = "College Grad";
                _eduFlag = OccidentalEdu.Bachelor | OccidentalEdu.Grad;
                return;
            }
            if (hasUndergradCollege)
            {
                _eduLevel = "Some College";
                _eduFlag = OccidentalEdu.Bachelor | OccidentalEdu.Some;
                return;
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
                       UsState.GetStateByPostalCode(UsCityStateZip.DF_STATE_ABBREV).GetHighSchools();

            //first try city, then state, last natl
            var hs = hshs.FirstOrDefault(x => x.PostalCode == hca?.AddressData?.PostalCode) ??
                         (hshs.Any() ? hshs[Etx.IntNumber(0, hshs.Length - 1)] : AmericanHighSchool.GetDefaultHs());

            //these are on file in all caps
            hs.Name = Util.Etc.CapWords(hs.Name, ' ');
            return hs;
        }
        #endregion
    }
}
