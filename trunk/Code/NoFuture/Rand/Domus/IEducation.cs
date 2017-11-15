using System;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo;
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
