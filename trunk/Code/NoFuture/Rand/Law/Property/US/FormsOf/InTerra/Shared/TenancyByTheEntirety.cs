using System;
using System.Linq;
using NoFuture.Law.US;

namespace NoFuture.Law.Property.US.FormsOf.InTerra.Shared
{
    /// <summary>
    /// More restrictive form of Joint Tenancy that applies to between spouses -
    /// the unities can only be undone by agreed and concurrent joint action
    /// of the spouses.
    /// </summary>
    public class TenancyByTheEntirety : JointTenancy, IBilateral
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;

            var cotenants = this.Cotenants(persons).ToList();
            if (cotenants.Count != 2 || !IsOneOfTwo(cotenants[0]) || !IsOneOfTwo(cotenants[1]))
            {
                AddReasonEntry($"{nameof(TenancyByTheEntirety)} is only applicable between two spouses.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// neither spouse has a divisible interest in the property - each hold in in its entirety
        /// </summary>
        public override Func<ILegalPerson, double> InterestFraction { get; set; } = lp => 1.0D;

        /// <summary>
        /// Is the given person one of the two spouses 
        /// </summary>
        public Predicate<ILegalPerson> IsOneOfTwo { get; set; } = lp => false;
    }
}
