﻿using System;
using NoFuture.Rand.Com;
using NoFuture.Rand.Data.Endo.Grps;

namespace NoFuture.Rand.Domus.Opes
{
    public interface IEmployment
    {
        IFirm Biz { get; set; }
        bool IsOwner { get; set; }
        StandardOccupationalClassification Occupation { get; set; }
        DateTime? FromDate { get; set; }
        DateTime? ToDate { get; set; }
        bool IsInRange(DateTime dt);
        Pondus GetPayAt(DateTime? dt);
        Pondus CurrentPay { get; }

    }
}