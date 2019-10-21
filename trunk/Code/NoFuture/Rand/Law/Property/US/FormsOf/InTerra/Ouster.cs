using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Shared;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra
{

    /// <summary>
    /// <![CDATA[
    /// When one cotenant is is barred from property, co-tenancy is
    /// physically impossible because the property size or cotenants
    /// become "so strained and bitter that they could not continue
    /// to reside together in peace and concord"
    /// Maxwell v. Eckert, 109 A. 730, 731 (N.J. Eq. 1920)
    /// ]]>
    /// </summary>
    /// <remarks>
    /// an ousted cotenant will be owed market-value rent for length-of-term of ouster
    /// </remarks>
    public class Ouster : TenancyInCommon, ITempore
    {

        public Ouster()
        {

        }

        public Ouster(TenancyInCommon sharedPropertyInterest)
        {
            if (sharedPropertyInterest == null)
                return;
            SubjectProperty = sharedPropertyInterest.SubjectProperty;
            IsEqualRightToPossessWhole = sharedPropertyInterest.IsEqualRightToPossessWhole;
        }

        /// <summary>
        /// it is NOT an Ouster if a cotenant willingly leaves 
        /// </summary>
        [Aka("abandoned", "surrendered")]
        public Predicate<ICotenant> IsVacated { get; set; } = ee => false;
        public Predicate<ICotenant> IsPhysicallyImpractical { get; set; } = lp => false;
        public Predicate<ICotenant> IsPeacefullyImpossible { get; set; } = lp => false;
        public Predicate<ICotenant> IsDeprivationOfUse { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            //this will test cotenants have a property interest
            if (!base.IsValid(persons))
                return false;

            foreach (var cotenant in this.Cotenants(persons))
            {
                var title = cotenant.GetLegalPersonTypeName();

                if (IsVacated(cotenant as ICotenant))
                {
                    AddReasonEntry($"{title} {cotenant.Name} {nameof(IsVacated)} is true");
                    return false;
                }

                if (IsPeacefullyImpossible(cotenant as ICotenant))
                {
                    AddReasonEntry($"{title} {cotenant.Name} {nameof(IsPeacefullyImpossible)} is true");
                    return true;
                }

                if (IsDeprivationOfUse(cotenant as ICotenant))
                {
                    AddReasonEntry($"{title} {cotenant.Name} {nameof(IsDeprivationOfUse)} is true");
                    return true;
                }
                if (IsPhysicallyImpractical(cotenant as ICotenant))
                {
                    AddReasonEntry($"{title} {cotenant.Name} {nameof(IsPhysicallyImpractical)} is true");
                    return true;
                }
            }

            return false;
        }

        public virtual DateTime Inception { get; set; }
        public virtual DateTime? Terminus { get; set; }
        public virtual bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = Inception <= dt;
            var beforeOrOnToDt = Terminus == null || Terminus.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }
    }
}
