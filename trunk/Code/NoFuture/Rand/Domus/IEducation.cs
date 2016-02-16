using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Rand.Edu;

namespace NoFuture.Rand.Domus
{
    public interface IEducation
    {
        OccidentalEdu GetEduLevel(DateTime? dt);
        IHighSchool GetHighSchool(DateTime? dt);
        IUniversity GetUniversity(DateTime? dt);
    }
}
