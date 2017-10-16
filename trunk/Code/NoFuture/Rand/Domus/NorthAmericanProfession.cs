using System;
using System.Collections.Generic;
using NoFuture.Rand.Com;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Rand.Domus
{
    public class NorthAmericanProfession : IProfession
    {
        protected internal List<IEmployment> employment = new List<IEmployment>();

        public NetConIncome GetProfessionalIncome(DateTime? dt)
        {
            throw new NotImplementedException();
        }

        public IEmployment GetEmployment(DateTime? dt)
        {
            throw new NotImplementedException();
        }
    }

    public class NorthAmericanEmployment : IEmployment
    {
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public IFirm Biz { get; set; }
        public bool IsOwner { get; set; }
        public StandardOccupationalClassification Occupation { get; set; }
    }
}
