using System;

namespace NoFuture.Rand.Edu
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