using System;
using NoFuture.Rand.Com;
using NoFuture.Rand.Data.Endo;

namespace NoFuture.Rand.Domus
{
    public interface IProfession
    {
        IEmployment[] GetEmployment(DateTime? dt);
    }

    public interface IEmployment
    {
        IFirm Biz { get; set; }
        bool IsOwner { get; set; }
        StandardOccupationalClassification Occupation { get; set; }
    }

}
