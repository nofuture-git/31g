using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Rand.Com;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Edu;

namespace NoFuture.Rand.Domus
{
    public interface IProfession
    {
        short GetEduLevel(DateTime? dt);
        Income GetProfessionalIncome(DateTime? dt);
        IEmployment GetEmployment(DateTime? dt);
        IHighSchool GetHighSchool(DateTime? dt);
        IUniversity GetUniversity(DateTime? dt);
    }

    [Flags]
    public enum OccidentalEdu : short
    {
        HighSchool = 1,
        College = 2,
        PostGrad = 4,
    }

    [Flags]
    public enum EduCompletion : short
    {
        Some = 16,
        Grad = 32
    }

    public interface IEmployment
    {
        DateTime FromDate { get; set; }
        DateTime? ToDate { get; set; }
        IFirm Biz { get; set; }
        bool IsOwner { get; set; }
    }

}
