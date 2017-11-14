using System;
using NoFuture.Rand.Com;
using NoFuture.Rand.Data.Endo.Grps;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus.Opes
{
    public interface IEmployment
    {
        IFirm Biz { get; set; }
        bool IsOwner { get; set; }
        StandardOccupationalClassification Occupation { get; set; }
        DateTime? FromDate { get; set; }
        DateTime? ToDate { get; set; }
        IMereo Pay { get; set; }
        bool IsInRange(DateTime dt);
    }
}