using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests
{
    public abstract class LifeEstate : PropertyBase, ILandPropertyInterest, ITempore
    {
        protected LifeEstate(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        protected LifeEstate() : base(null) { }

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
