using System;
using NoFuture.Rand.Com;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Endo.Grps;

namespace NoFuture.Rand.Domus.Opes
{
    public interface IEmployment : ITempore
    {
        IFirm Biz { get; set; }
        bool IsOwner { get; set; }
        StandardOccupationalClassification Occupation { get; set; }
        Pondus[] CurrentDeductions { get; }
        Pondus CurrentPay { get; }

        Pondus GetPayAt(DateTime? dt);
        Pondus[] GetDeductionsAt(DateTime? dt);
    }
}