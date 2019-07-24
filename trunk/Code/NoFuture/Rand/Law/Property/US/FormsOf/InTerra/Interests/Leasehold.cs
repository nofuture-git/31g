using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests
{
    public class Leasehold : PropertyBase, ILandPropertyInterest, ITempore
    {
        public Leasehold(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Leasehold() : base(null) { }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }

        public new RealProperty SubjectProperty { get; set; }
        public DateTime Inception { get; set; }
        public DateTime? Terminus { get; set; }
        public bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = Inception <= dt;
            var beforeOrOnToDt = Terminus == null || Terminus.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }
    }
}
