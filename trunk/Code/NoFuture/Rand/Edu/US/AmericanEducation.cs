using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Edu.US
{
    /// <summary>
    /// Represents the composite of personal education over time.
    /// </summary>
    [Serializable]
    public class AmericanEducation : IEducation
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
        public AmericanEducation(){ }

        public AmericanEducation(Tuple<IHighSchool, DateTime?> assignHs)
        {
            if (assignHs?.Item1 is AmericanHighSchool)
                AddHighSchool((AmericanHighSchool)assignHs.Item1, assignHs.Item2);
            AssignEduFlagAndLevel();
        }

        public AmericanEducation(Tuple<IUniversity, DateTime?> assignUniv, Tuple<IHighSchool, DateTime?> assignHs)
        {
            if (assignHs?.Item1 is AmericanHighSchool)
            {
                AddHighSchool((AmericanHighSchool)assignHs.Item1, assignHs.Item2);
            }
            if (assignUniv?.Item1 is AmericanUniversity)
            {
                AddUniversity((AmericanUniversity)assignUniv.Item1, assignUniv.Item2);
            }
            AssignEduFlagAndLevel();
        }

        #endregion

        #region properties

        public OccidentalEdu EduFlag => _eduFlag;

        /// <summary>
        /// Set to a readable string for JSON serialization.
        /// </summary>
        public string EduLevel => _eduLevel;

        /// <summary>
        /// Gets last High School attended.
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
        /// Gets last college attended
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
        /// Gets an education at random.
        /// </summary>
        /// <param name="birthDate"></param>
        /// <param name="homeState"></param>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        [RandomFactory]
        public static AmericanEducation RandomEducation(DateTime? birthDate = null, string homeState = null, string zipCode = null)
        {
            var edu = new AmericanEducation();
            var dob = birthDate ?? Etx.GetWorkingAdultBirthDate();
            var age = Etc.CalcAge(dob);

            if (age < DF_MIN_AGE_ENTER_HS)
                return edu;

            var dtAtAge18 = dob.AddYears(UsState.AGE_OF_ADULT);
            var isLegalAdult = age >= UsState.AGE_OF_ADULT;
            if (!edu.AssignRandomHighSchool(homeState, zipCode, isLegalAdult, dtAtAge18, out var hsGradDt))
                return edu;

            var usstate = UsState.GetState(homeState);

            edu.AssignRandomCollege(usstate?.ToString(), hsGradDt);
            return edu;
        }

        /// <summary>
        /// Assigns a value to <see cref="HighSchool"/> at random based on the given inputs
        /// </summary>
        /// <param name="homeState"></param>
        /// <param name="zipCode"></param>
        /// <param name="isLegalAdult"></param>
        /// <param name="dtAtAge18"></param>
        /// <param name="hsGradDt"></param>
        /// <returns></returns>
        protected internal bool AssignRandomHighSchool(string homeState, string zipCode, bool isLegalAdult, DateTime dtAtAge18,
            out DateTime? hsGradDt)
        {
            hsGradDt = null;

            //get hs grad data for state amer lived in when 18
            var hsGradData =
                UsStateData.GetStateData(homeState)?.PercentOfGrads
                    .FirstOrDefault(x => x.Item1 == (OccidentalEdu.HighSchool | OccidentalEdu.Grad));

            //determine prob. of having hs grad
            var hsGradRate = hsGradData?.Item2 ?? AmericanHighSchool.DF_NATL_AVG;

            //first try city, then state, last natl
            var hss = AmericanHighSchool.GetHighSchoolsByZipCode(zipCode)
                      ?? AmericanHighSchool.GetHighSchoolsByState(homeState);

            var hs = hss.Length > 1 ? hss[Etx.IntNumber(0,hss.Length-1)] : hss.First();

            //still in hs or dropped out
            if (!isLegalAdult || Etx.TryAboveOrAt((int)Math.Round(hsGradRate) + 1, Etx.Dice.OneHundred))
            {
                //assign grad hs but no date
                _highSchools.Add(new AmericanHighSchoolStudent(hs));
                AssignEduFlagAndLevel();
                return false;
            }

            //get a date of when amer would be grad'ing from hs
            hsGradDt = GetRandomGraduationDate(dtAtAge18.AddYears(-4), AmericanHighSchool.YearsInHighSchool, true);

            //assign grad hs with grad date
            _highSchools.Add(new AmericanHighSchoolStudent(hs) {Graduation = hsGradDt});
            return true;
        }

        /// <summary>
        /// Assigns a value to <see cref="College"/> at random based on the given values
        /// </summary>
        /// <param name="homeState"></param>
        /// <param name="hsGradDt"></param>
        protected internal void AssignRandomCollege(string homeState, DateTime? hsGradDt)
        {
            if (hsGradDt == null)
                return;

            //get college grad data for same state as hs
            var univGradData =
                UsStateData.GetStateData(homeState)?.PercentOfGrads
                    .FirstOrDefault(x => x.Item1 == (OccidentalEdu.Bachelor | OccidentalEdu.Grad));

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
                AddUniversity(univ, null);
                AssignEduFlagAndLevel();
                return;
            }

            //college grad
            //get a date for when amer would grad from college
            var univGradDt = GetRandomGraduationDate(hsGradDt.Value, AmericanUniversity.YearsInUndergradCollege);

            AddUniversity(univ, univGradDt);

            //try for post-grad
            var postGradRate = AmericanUniversity.DefaultNationalAvgs.First(
                               x => x.Key == (OccidentalEdu.Master | OccidentalEdu.Grad)).Value;

            if (Etx.TryBelowOrAt((int)postGradRate * 10, Etx.Dice.OneThousand))
            {
                var postGradDt = GetRandomGraduationDate(univGradDt, AmericanUniversity.YearsInPostgradCollege);
                var postGradUniv = GetAmericanUniversity(homeState);

                AddUniversity(postGradUniv, postGradDt);
            }

            AssignEduFlagAndLevel();
        }

        /// <summary>
        /// Gets a date around a semester&apos;s end moving 
        /// out (x) number of years, at random, from <see cref="fromHere"/>
        /// </summary>
        /// <param name="fromHere"></param>
        /// <param name="eq">Normal dist which determines (x) mentioned above.</param>
        /// <param name="monthOfMayOnly">Force the random date to occur only in the month of May</param>
        /// <returns></returns>
        public static DateTime GetRandomGraduationDate(DateTime fromHere, NormalDistEquation eq, bool monthOfMayOnly = false)
        {
            var years = Etx.RandomValueInNormalDist(eq);

            var gradDt = fromHere.AddYears((int)Math.Round(years));
            var validMonths = monthOfMayOnly ? new[] {5} : new[] {5, 12};
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
                            AmericanUniversity.YearsInPostgradCollege.Mean +
                            AmericanUniversity.YearsInPostgradCollege.StdDev;
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

        protected internal void AddHighSchool(AmericanHighSchool hs, DateTime? gradDt)
        {
            _highSchools.Add(new AmericanHighSchoolStudent(hs) { Graduation = gradDt });
        }

        protected internal void AddUniversity(AmericanUniversity univ, DateTime? gradDt)
        {
            _universities.Add(new AmericanCollegeStudent(univ) { Graduation = gradDt });
        }

        /// <summary>
        /// Factory method to get an instance of <see cref="AmericanUniversity"/> at random.
        /// </summary>
        /// <param name="homeState">
        /// There is a 73 percent chance this will be used, otherwise the result is anywhere
        /// in the nation
        /// src [https://www.washingtonpost.com/blogs/govbeat/wp/2014/06/05/map-the-states-college-kids-cant-wait-to-leave]
        /// </param>
        /// <returns></returns>
        internal static AmericanUniversity GetAmericanUniversity(string homeState)
        {
            homeState = Etx.TryBelowOrAt(73, Etx.Dice.OneHundred) ? homeState : null;

            return AmericanUniversity.RandomUniversity(homeState);
        }
        #endregion
    }
}
