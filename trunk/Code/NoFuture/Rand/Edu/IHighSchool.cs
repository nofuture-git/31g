using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo;

namespace NoFuture.Rand.Edu
{
    public interface IHighSchool
    {
        string Name { get; set; }
        UrbanCentric UrbanCentric { get; set; }
        double TotalTeachers { get; set; }
        int TotalStudents { get; set; }
    }
}