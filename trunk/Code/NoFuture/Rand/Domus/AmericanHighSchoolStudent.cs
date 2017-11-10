using System;
using NoFuture.Rand.Edu;

namespace NoFuture.Rand.Domus
{
    [Serializable]
    public class AmericanHighSchoolStudent : NorthAmericanStudent<AmericanHighSchool>
    {
        public override string Abbrev => "HighSchool";

        public AmericanHighSchoolStudent(AmericanHighSchool school) : base(school)
        {
        }
    }
}