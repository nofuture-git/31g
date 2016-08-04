using System;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Data;
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

        public NorthAmericanEdu(IPerson p)
        {
            var amer = p as NorthAmerican;
            if (amer?.BirthCert == null)
                return;

            var mother = amer.GetMother() as NorthAmerican ??
                         NAmerUtil.SolveForParent(amer.BirthCert.DateOfBirth,
                             NAmerUtil.Equations.FemaleAge2FirstMarriage,
                             Gender.Female) as NorthAmerican;
            var dtAtAge18 = amer.BirthCert.DateOfBirth.AddYears(18);
            var sdf = mother?.GetAddressAt(dtAtAge18);
            if (sdf == null)
                return;
            var homeState = Gov.UsState.GetStateByPostalCode(sdf.HomeCityArea?.AddressData?.StateAbbrv);
            var hsGradData =
                homeState?.GetStateData()
                    .PercentOfGrads.FirstOrDefault(x => x.Item1 == (OccidentalEdu.HighSchool | OccidentalEdu.Grad));
            if (hsGradData == null)
                return;
            var hsGradRate = (int)Math.Round(hsGradData.Item2);
            var hshs = homeState.GetHighSchools();
            if (!hshs.Any())
                return;
            var hs = hshs.FirstOrDefault(x => x.PostalCode == sdf.HomeStreetPo.Data.PostalCode) ??
                         hshs[Etx.IntNumber(0, hshs.Length - 1)];

            var hsGradDt = dtAtAge18;
            //hs drop out
            if (!Etx.TryBelowOrAt(hsGradRate, Etx.Dice.OneHundred))
            {
                HighSchool = new Tuple<IHighSchool, DateTime?>(hs, null);
                return;
            }

            //hs grad
            while (hsGradDt.Month != 5)
                hsGradDt = hsGradDt.AddMonths(1);

            hsGradDt = new DateTime(hsGradDt.Year, hsGradDt.Month, Etx.IntNumber(12, 28));

            HighSchool = new Tuple<IHighSchool, DateTime?>(hs, hsGradDt);

            var univGradData =
                homeState.GetStateData()
                    .PercentOfGrads.FirstOrDefault(x => x.Item1 == (OccidentalEdu.Bachelor | OccidentalEdu.Grad));
            if (univGradData == null)
                return;
            var univGradRate = (int)Math.Round(univGradData.Item2);

            //no college ever
            if (Etx.TryAboveOrAt(univGradRate * 2, Etx.Dice.OneHundred))
            {
                return;
            }

            //mostly made up actual avg is 6 years
            var yearsInCollege = Etx.RandomValueInNormalDist(4.67, 1.58);

            var univGradDt = hsGradDt.AddYears((int)Math.Round(yearsInCollege));
            while (univGradDt.Month != 5)
                univGradDt = univGradDt.AddMonths(1);

            univGradDt = new DateTime(univGradDt.Year, univGradDt.Month, Etx.IntNumber(12, 28));
            IUniversity univ = null;
            //79 percent attend home state is a guess
            if (Etx.TryAboveOrAt(79, Etx.Dice.OneHundred) && homeState.GetUniversities().Any())
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
            if (Etx.TryBelowOrAt(univGradRate, Etx.Dice.OneHundred))
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
