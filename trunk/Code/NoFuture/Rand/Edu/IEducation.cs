using System;
using NoFuture.Rand.Data.Endo.Enums;

namespace NoFuture.Rand.Edu
{
    public interface IEducation
    {
        OccidentalEdu EduFlag { get; }
        Tuple<IHighSchool, DateTime?> HighSchool { get; }
        Tuple<IUniversity, DateTime?> College { get; }
    }
}
