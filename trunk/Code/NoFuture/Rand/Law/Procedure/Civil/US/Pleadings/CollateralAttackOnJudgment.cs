﻿using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// Like <see cref="SpecialAppearance"/> except its performed in the defendant&apos;s domicile location
    /// </summary>
    /// <remarks>
    /// This follows from a defendant ignoring a <see cref="Summons"/> in a
    /// foreign jurisdiction.  They wait for the plaintiff to try and collect
    /// and then challenge jurisdiction.
    /// </remarks>
    public class CollateralAttackOnJudgment : SpecialAppearance
    {
        public ICourt NameOfOriginalCourt { get; set; }

        /// <summary>
        /// A defendant may only do this once.  In either foreign location or domicile
        /// location, but they may not pit one court against another.
        /// </summary>
        public Predicate<ILegalPerson> IsJurisdictionAlreadyChallenged { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (NameOfOriginalCourt == null || string.IsNullOrWhiteSpace(NameOfOriginalCourt.Name))
            {
                AddReasonEntry($"{nameof(NameOfOriginalCourt)} is unassigned");
                return false;
            }

            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;

            var title = defendant.GetLegalPersonTypeName();

            if (IsJurisdictionAlreadyChallenged(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsJurisdictionAlreadyChallenged)} is true");
                return false;
            }

            return base.IsValid(persons);
        }
    }
}
