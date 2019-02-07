using System;

namespace NoFuture.Rand.Law.US.Criminal.Elements.Intent.ComLaw
{
    /// <summary>
    /// A more sophisticated level of awareness
    /// </summary>
    public class SpecificIntent : MensRea, IComparable
    {
        /// <summary>
        /// example being Mayhem v. Battery where Mayhem is specific intent to disfigure
        /// </summary>
        public Predicate<ILegalPerson> IsTowardSpecificResult { get; set; } = lp => false;

        /// <summary>
        /// example being where the taking of property also needs to be met with intent to keep it
        /// </summary>
        public Predicate<ILegalPerson> IsMoreThanCriminalAct { get; set; } = lp => false;

        /// <summary>
        /// having knowledge that an act is illegal
        /// </summary>
        public Predicate<ILegalPerson> IsScienter { get; set; } = lp => false;

        public virtual int CompareTo(object obj)
        {
            if (obj is MaliceAforethought)
                return -1;
            if (obj is GeneralIntent)
                return 1;
            return 0;
        }
    }
}
