using System;
using NoFuture.Rand.Com;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Endo.Grps;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus.Opes
{
    public interface IEmployment : IIdentifier<IFirm>, ITempore
    {
        bool IsOwner { get; set; }
        StandardOccupationalClassification Occupation { get; set; }
        Pondus[] CurrentDeductions { get; }
        Pondus CurrentPay { get; }

        Pondus GetPayAt(DateTime? dt);
        Pondus[] GetDeductionsAt(DateTime? dt);
    }
}