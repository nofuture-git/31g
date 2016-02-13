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
        Income GetProfessionalIncome(DateTime? dt);
        IEmployment GetEmployment(DateTime? dt);
    }

    public interface IEmployment
    {
        DateTime FromDate { get; set; }
        DateTime? ToDate { get; set; }
        IFirm Biz { get; set; }
        bool IsOwner { get; set; }
    }

}
