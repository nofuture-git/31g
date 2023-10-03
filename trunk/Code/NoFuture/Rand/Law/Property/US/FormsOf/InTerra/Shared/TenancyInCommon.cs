using System;
using System.Linq;
using NoFuture.Law.US;

namespace NoFuture.Law.Property.US.FormsOf.InTerra.Shared
{
    /// <summary>
    /// Neither tenant can lawfully exclude the other
    /// </summary>
    /// <remarks>
    /// The default form of co-ownership across the United States
    /// </remarks>
    public class TenancyInCommon : LandPropertyInterestBase
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var cotenants = this.Cotenants(persons).ToList();
            if (!cotenants.Any())
                return false;

            if (cotenants.Count == 1)
            {
                var onlyCotenant = cotenants.First();
                
                AddReasonEntry($"{onlyCotenant.GetLegalPersonTypeName()} {onlyCotenant.Name} " +
                               "is not a cotenant since they are the only one.");
            }

            foreach (var cotenant in cotenants)
            {
                var title = cotenant.GetLegalPersonTypeName();
                if (!IsEqualRightToPossessWhole(cotenant))
                {
                    AddReasonEntry($"{title} {cotenant.Name} {nameof(IsEqualRightToPossessWhole)} is false");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// each cotenant is able to occupy the whole subject to the rights of the other cotenants
        /// </summary>
        public virtual Predicate<ILegalPerson> IsEqualRightToPossessWhole { get; set; } = lp => false;
    }
}
