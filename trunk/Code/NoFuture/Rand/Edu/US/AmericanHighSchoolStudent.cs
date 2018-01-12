using System;

namespace NoFuture.Rand.Edu.US
{
    [Serializable]
    public class AmericanHighSchoolStudent : AmericanStudentBase<AmericanHighSchool>
    {
        public override string Abbrev => "HS";

        public AmericanHighSchoolStudent(AmericanHighSchool school) : base(school)
        {
        }
    }
}