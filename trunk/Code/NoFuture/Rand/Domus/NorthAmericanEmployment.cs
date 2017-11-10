using NoFuture.Rand.Com;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Endo;

namespace NoFuture.Rand.Domus
{
    public class NorthAmericanEmployment : DiachronIdentifier,  IEmployment
    {
        public IFirm Biz { get; set; }
        public bool IsOwner { get; set; }
        public StandardOccupationalClassification Occupation { get; set; }

        public override string Abbrev => "Employer";
    }
}