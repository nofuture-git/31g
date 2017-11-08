using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Edu;

namespace NoFuture.Rand.Domus
{
    public interface IEducation
    {
        OccidentalEdu EduFlag { get; }
        Tuple<IHighSchool, DateTime?> HighSchool { get; }
        Tuple<IUniversity, DateTime?> College { get; }
    }

    public interface IStudent<T>
    {
        T School { get; }
        DateTime? Graduation { get; set; }
    }
}
