using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests
{
    public abstract class LifeEstate : PropertyBase, ILandPropertyInterest
    {
        protected LifeEstate(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        protected LifeEstate() : base(null) { }

        /// <summary>
        /// A life estate will almost always state that A’s interest is only &quot;for life,&quot;
        /// </summary>
        public Predicate<ILegalPerson> IsOnlyForLife { get; set; } = lp => true;

        public new RealProperty SubjectProperty { get; set; }

    }
}
