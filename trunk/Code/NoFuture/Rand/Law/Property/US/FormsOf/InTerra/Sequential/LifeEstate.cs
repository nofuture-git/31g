using System;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests
{
    public abstract class LifeEstate : LandPropertyInterestBase
    {
        protected LifeEstate(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        protected LifeEstate() : base(null) { }

        /// <summary>
        /// A life estate will almost always state that A’s interest is only &quot;for life,&quot;
        /// </summary>
        public Predicate<ILegalPerson> IsOnlyForLife { get; set; } = lp => true;

    }
}
