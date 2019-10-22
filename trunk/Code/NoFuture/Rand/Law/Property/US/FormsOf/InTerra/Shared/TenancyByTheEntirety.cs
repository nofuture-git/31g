using System;
using System.Linq;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Shared
{
    /// <summary>
    /// More restrictive form of Joint Tenancy that applies to between spouses -
    /// the unities can only be undone by agreed and concurrent joint action
    /// of the spouses.
    /// </summary>
    public class TenancyByTheEntirety : JointTenancy
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;

            var cotenants = this.Cotenants(persons).ToList();
            if (cotenants.Count != 2 || !AreSpouses(cotenants[0], cotenants[1]))
            {
                AddReasonEntry($"{nameof(TenancyByTheEntirety)} is only applicable between two spouses.");
                return false;
            }

            return true;
        }

        public virtual Func<ILegalPerson, ILegalPerson, bool> AreSpouses { get; set; } = (lp1, lp2) => false;

        /// <summary>
        /// neither spouse has a divisible interest in the property - each hold in in its entirety
        /// </summary>
        public override Func<ILegalPerson, double> InterestFraction { get; set; } = lp => 1.0D;
    }
}
