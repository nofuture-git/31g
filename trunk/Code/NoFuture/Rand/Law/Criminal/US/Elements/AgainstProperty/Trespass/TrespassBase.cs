﻿using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Property;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstProperty.Trespass
{
    public abstract class TrespassBase : PropertyConsent
    {
        protected TrespassBase(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {

        }

        protected TrespassBase(): this(ExtensionMethods.Defendant) { }

        /// <summary>
        /// partial or complete intrusion of either the defendant, the defendant&apos;s body part or a tool or instrument
        /// </summary>
        /// <remarks>
        /// dust, noise, vibrations, etc. are not trespass unless you can prove damages
        /// </remarks>
        public Predicate<ILegalPerson> IsTangibleEntry { get; set; } = lp => false;
    }
}