using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Data.Endo.Enums;

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