using System;

namespace NoFuture.Rand.Law.US.Elements
{
    public abstract class TrespassBase : AgainstPropertyBase
    {
        /// <summary>
        /// partial or complete intrusion of either the defendant, the defendant&apos;s body part or a tool or instrument
        /// </summary>
        public Predicate<ILegalPerson> IsEntry { get; set; } = lp => false;
    }
}
