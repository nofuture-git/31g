﻿using System;

namespace NoFuture.Rand.Law.US.Elements
{
    public abstract class TrespassBase : AgainstPropertyBase
    {
        /// <summary>
        /// partial or complete intrusion of either the defendant, the defendant&apos;s body part or a tool or instrument
        /// </summary>
        /// <remarks>
        /// dust, noise, vibrations, etc. are not trespass unless you can prove damages
        /// </remarks>
        public Predicate<ILegalPerson> IsTangibleEntry { get; set; } = lp => false;
    }
}