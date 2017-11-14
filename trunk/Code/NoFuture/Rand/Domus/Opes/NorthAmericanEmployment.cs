using System;
using NoFuture.Rand.Com;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Endo.Grps;

namespace NoFuture.Rand.Domus.Opes
{
    [Serializable]
    public class NorthAmericanEmployment : DiachronIdentifier,  IEmployment
    {
        public NorthAmericanEmployment() {}
        public NorthAmericanEmployment(DateTime? startDate, DateTime? endDate) :base (startDate, endDate) { }

        public override string Abbrev => "Employer";
        public IFirm Biz { get; set; }
        public bool IsOwner { get; set; }
        public StandardOccupationalClassification Occupation { get; set; }
        public Pondus Pay { get; }
    }
}