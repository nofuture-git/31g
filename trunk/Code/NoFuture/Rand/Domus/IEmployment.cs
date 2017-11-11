﻿using NoFuture.Rand.Com;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Data.Endo.Grps;

namespace NoFuture.Rand.Domus
{
    public interface IEmployment
    {
        IFirm Biz { get; set; }
        bool IsOwner { get; set; }
        StandardOccupationalClassification Occupation { get; set; }
    }
}