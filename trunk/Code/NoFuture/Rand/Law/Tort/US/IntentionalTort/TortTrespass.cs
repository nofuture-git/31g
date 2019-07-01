using System;
using NoFuture.Rand.Law.Criminal.US.Elements.AgainstProperty.Trespass;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.IntentionalTort
{
    public abstract class TortTrespass : TrespassBase
    {
        protected TortTrespass(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {

        }

        protected TortTrespass() : this(ExtensionMethods.Tortfeasor) { }

        public Causation Causation { get; set; }

        public IInjury Injury { get; set; }
    }
}
