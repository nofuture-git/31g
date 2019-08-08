using System;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra
{
    public abstract class LandPropertyInterestBase : PropertyBase, ILandPropertyInterest
    {
        protected LandPropertyInterestBase(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        protected LandPropertyInterestBase() : base(null)
        {
        }

        public new RealProperty SubjectProperty { get; set; }
    }
}
