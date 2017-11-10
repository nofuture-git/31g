using System;
using NoFuture.Rand.Edu;

namespace NoFuture.Rand.Domus
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