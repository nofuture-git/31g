using System;

namespace NoFuture.Rand.Edu.US
{
    [Serializable]
    public class AmericanCollegeStudent : AmericanStudentBase<AmericanUniversity>
    {
        public AmericanCollegeStudent(AmericanUniversity school) : base(school)
        {
        }

        public override string Abbrev => "College";
    }
}