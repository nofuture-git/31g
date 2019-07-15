using System;
using System.Collections.Generic;
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

        /// <remarks>
        /// Deeds form a stack of current to all previous owners where last entry is the sovereign.
        /// </remarks>
        public Stack<ILegalPerson> PreviousDeedHolders { get; } = new Stack<ILegalPerson>();

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeree = this.Offeree(persons);
            var offeror = this.Offeror(persons);

            if (offeree == null || offeror == null)
                return false;

            if (Assent == null)
            {
                AddReasonEntry($"{nameof(Assent)} is unassigned");
                return false;
            }

            //TODO - deal with strict kind of terms present in all deeds

            throw new NotImplementedException();
        }

        
    }
}
