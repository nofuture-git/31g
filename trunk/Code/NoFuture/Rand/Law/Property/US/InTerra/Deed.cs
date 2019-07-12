using System;
using NoFuture.Rand.Law.Property.US.FormsOf;
using NoFuture.Rand.Law.US;

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

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (Assent == null)
            {
                AddReasonEntry($"{nameof(Assent)} is unassigned");
                return false;
            }

            var mutualAssent = Assent as IAssentTerms;
            if (mutualAssent == null)
            {
                AddReasonEntry($"{nameof(Assent)} cannot be cast as {nameof(IAssentTerms)}");
                return false;
            }
            
            //TODO - deal with strict kind of terms present in all deeds

            throw new NotImplementedException();
        }
    }
}
