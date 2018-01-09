using System;
using NoFuture.Rand.Edu;

namespace NoFuture.Rand.Domus
{
    [Serializable]
    public class AmericanHighSchoolStudent : NorthAmericanStudent<AmericanHighSchool>
    {
        public override string Abbrev => "HS";

        public AmericanHighSchoolStudent(AmericanHighSchool school) : base(school)
        {
        }
    }
}