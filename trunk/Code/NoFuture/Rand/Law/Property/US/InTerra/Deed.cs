using System;
using NoFuture.Rand.Law.Property.US.FormsOf;

namespace NoFuture.Rand.Law.Property.US.InTerra
{
    /// <summary>
    /// A kind of contract which transfers real property from one person to another
    /// </summary>
    public abstract class Deed : LegalConcept, IBargain<RealProperty, RealProperty>
    {
        public virtual RealProperty Offer { get; set; }
        public virtual Func<RealProperty, RealProperty> Acceptance { get; set; }
        public virtual IAssent Assent { get; set; }

    }
}
