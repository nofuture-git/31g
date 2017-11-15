using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Gov;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Domus
{
    [Serializable]
    public class NorthAmericanEdu : IEducation
    {
        #region fields
        private string _eduLevel;
        private OccidentalEdu _eduFlag;

        private readonly HashSet<AmericanHighSchoolStudent> _highSchools = new HashSet<AmericanHighSchoolStudent>();
        private readonly HashSet<AmericanCollegeStudent> _universities = new HashSet<AmericanCollegeStudent>();
        private readonly IComparer<DiachronIdentifier> _comparer = new TemporeComparer();

        public const int DF_MIN_AGE_ENTER_HS = 14;
        #endregion

        #region ctor
        internal NorthAmericanEdu(Tuple<IHighSchool, DateTime?> assignHs)
        {
            if (assignHs?.Item1 is AmericanHighSchool)
                _highSchools.Add(new AmericanHighSchoolStudent((AmericanHighSchool)assignHs.Item1)
                {
                    Graduation = assignHs.Item2
                });
            AssignEduFlagAndLevel();
        }

        internal NorthAmericanEdu(Tuple<IUniversity, DateTime?> assignUniv, Tuple<IHighSchool, DateTime?> assignHs)
        {
            if (assignHs?.Item1 is AmericanHighSchool)
            {
                _highSchools.Add(new AmericanHighSchoolStudent((AmericanHighSchool) assignHs.Item1)
                {
                    Graduation = assignHs.Item2
                });
            }
            if (assignUniv?.Item1 is AmericanUniversity)
            {
                _universities.Add(new AmericanCollegeStudent((AmericanUniversity) assignUniv.Item1)
                {
                    Graduation = assignUniv.Item2
                });
            }
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
            if (!AssignRandomHighSchool(homeCityArea, isLegalAdult, dtAtAge18, out var hsGradDt))
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

        /// <summary>
        /// Gets highest High School attended.
        /// </summary>
        public Tuple<IHighSchool, DateTime?> HighSchool
        {
            get
            {
                var hs = HighSchools.LastOrDefault(x => x.Graduation != null) ?? HighSchools.LastOrDefault();
                return hs == null
                    ? new Tuple<IHighSchool, DateTime?>(null, null)
                    : new Tuple<IHighSchool, DateTime?>(hs.School, hs.Graduation);
            }
        }

        /// <summary>
        /// Gets highest college attended
        /// </summary>
        public Tuple<IUniversity, DateTime?> College
        {
            get
            {
                var univ = Universities.LastOrDefault(x => x.Graduation != null) ?? Universities.LastOrDefault();
                return univ == null
                    ? new Tuple<IUniversity, DateTime?>(null, null)
                    : new Tuple<IUniversity, DateTime?>(univ.School, univ.Graduation);
            }
        }

        protected internal List<AmericanHighSchoolStudent> HighSchools
        {
            get
            {
                var h = _highSchools.ToList();
                h.Sort(_comparer);
                return h;
            }
        }

        protected internal List<AmericanCollegeStudent> Universities
        {
            get
            {
                var u =  _universities.ToList();
                u.Sort(_comparer);
                return u;
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
            var hs = GetAmericanHighSchool(homeCityArea);

            //still in hs or dropped out
            if (!isLegalAdult || Etx.TryAboveOrAt((int)Math.Round(hsGradRate) + 1, Etx.Dice.OneHundred))
            {
                //assign grad hs but no date
                _highSchools.Add(new AmericanHighSchoolStudent(hs));
                AssignEduFlagAndLevel();
                return false;
            }

            //get a date of when amer would be grad'ing from hs
            hsGradDt = GetRandomGraduationDate(dtAtAge18.AddYears(-4), NAmerUtil.Equations.YearsInHighSchool, true);

            //assign grad hs with grad date
            _highSchools.Add(new AmericanHighSchoolStudent(hs) {Graduation = hsGradDt});
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
            var univ = GetAmericanUniversity(homeState);
            if (univ == null)
            {
                AssignEduFlagAndLevel();
                return;
            }

            if (!Etx.TryBelowOrAt((int)Math.Round(bachelorGradRate * 10), Etx.Dice.OneThousand))
            {
                //dropped out of college
                _universities.Add(new AmericanCollegeStudent(univ));
                AssignEduFlagAndLevel();
                return;
            }

            //college grad
            //get a date for when amer would grad from college
            var univGradDt = GetRandomGraduationDate(hsGradDt.Value, NAmerUtil.Equations.YearsInUndergradCollege);

            _universities.Add(new AmericanCollegeStudent(univ) {Graduation = univGradDt});

            //try for post-grad
            var postGradRate = AmericanUniversity.DefaultNationalAvgs.First(
                               x => x.Key == (OccidentalEdu.Master | OccidentalEdu.Grad)).Value;

            if (Etx.TryBelowOrAt((int)postGradRate * 10, Etx.Dice.OneThousand))
            {
                var postGradDt = GetRandomGraduationDate(univGradDt, NAmerUtil.Equations.YearsInPostgradCollege);
                var postGradUniv = GetAmericanUniversity(homeState);

                _universities.Add(new AmericanCollegeStudent(postGradUniv) {Graduation = postGradDt});
            }

            AssignEduFlagAndLevel();
        }

        /// <summary>
        /// Gets a date around a semester&apos;s end moving 
        /// out (x) number of years, at random, from <see cref="fromHere"/>
        /// </summary>
        /// <param name="fromHere"></param>
        /// <param name="eq">Normal dist which determines (x) mentioned above.</param>
        /// <param name="mayOnly">Force the random date to occur only in the month of May</param>
        /// <returns></returns>
        public static DateTime GetRandomGraduationDate(DateTime fromHere, NormalDistEquation eq, bool mayOnly = false)
        {
            var years = Etx.RandomValueInNormalDist(eq);

            var gradDt = fromHere.AddYears((int)Math.Round(years));
            var validMonths = mayOnly ? new[] {5} : new[] {5, 12};
            while (!validMonths.Contains(gradDt.Month))
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
            //determine predicates
            var hasHs = HighSchool?.Item1 != null;
            var isHsGrad = HighSchool?.Item2 != null;
            var hasUndergradCollege = Universities.Any(x => x.School != null);
            var isCollegeGrad = Universities.Any(x => x.Graduation != null);
            var isCollegePostgrad = Universities.Count(x => x.Graduation != null) > 1;
            var isDocGrad = false;

            //determine number of years in post-grad
            if (isCollegePostgrad)
            {
                var firstGradDate = Universities.First(x => x.Graduation != null).Graduation.Value;
                var lastGradDate = Universities.Last(x => x.Graduation != null).Graduation.Value;
                var numYearsPostGrad =
                    Math.Abs((firstGradDate - lastGradDate).TotalDays)/Constants.DBL_TROPICAL_YEAR;
    
                //consider doctorate as right-side second sigma of postgrad years
                isDocGrad = numYearsPostGrad >
                                NAmerUtil.Equations.YearsInPostgradCollege.Mean +
                                NAmerUtil.Equations.YearsInPostgradCollege.StdDev;
            }

            //assign flag and name based on the above
            if (new[] { hasHs, isHsGrad, hasUndergradCollege, isCollegeGrad }.All(x => x == false))
            {
                _eduLevel = "No Education";
                _eduFlag = OccidentalEdu.None;
                return;
            }
            if (isCollegePostgrad)
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
                _eduLevel = "Some High School";
                _eduFlag = OccidentalEdu.HighSchool | OccidentalEdu.Some;
            }
        }

        public override string ToString()
        {
            return string.Join(" ", HighSchool, College);
        }

        /// <summary>
        /// Factory method to get an instance of <see cref="AmericanUniversity"/> at random.
        /// </summary>
        /// <param name="homeState">Will likely be used but may be randomly ignored.</param>
        /// <returns></returns>
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

        public static AmericanHighSchool GetAmericanHighSchool(UsCityStateZip hca)
        {
            if (hca == null)
                return AmericanHighSchool.GetDefaultHs();

            //get all hs for the state
            var hshs = hca.State?.GetHighSchools() ??
                       UsState.GetStateByPostalCode(UsCityStateZip.DF_STATE_ABBREV).GetHighSchools();

            //first try city, then state, last natl
            var hs = hshs.FirstOrDefault(x => x.PostalCode == hca.AddressData?.PostalCode) ??
                         (hshs.Any() ? hshs[Etx.IntNumber(0, hshs.Length - 1)] : AmericanHighSchool.GetDefaultHs());

            //these are on file in all caps
            hs.Name = Etc.CapWords(hs.Name, ' ');
            return hs;
        }
        #endregion
    }
}
