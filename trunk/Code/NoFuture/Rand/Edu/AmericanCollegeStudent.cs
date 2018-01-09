using System;

namespace NoFuture.Rand.Edu
{
    [Serializable]
    public class AmericanCollegeStudent : NorthAmericanStudent<AmericanUniversity>
    {
        public AmericanCollegeStudent(AmericanUniversity school) : base(school)
        {
        }

        public override string Abbrev => "College";
    }
}