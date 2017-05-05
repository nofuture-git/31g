using System;
using NoFuture.Rand.Edu;

namespace NoFuture.Rand.Domus
{
    public interface IEducation
    {
        OccidentalEdu EduFlag { get; }
        Tuple<IHighSchool, DateTime?> HighSchool { get; }
        Tuple<IUniversity, DateTime?> College { get; }
    }
}
