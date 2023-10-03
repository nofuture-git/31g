using System;
using NoFuture.Rand.Law.Enums;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Property.US.Terms;
using NoFuture.Rand.Law.Property.US.Terms.Tm;

namespace NoFuture.Rand.Law.Property.US.FormsOf.Intellectus
{
    /// <summary>
    /// to identify and distinguish his or her goods, including a unique product, from those
    /// manufactured or sold by others and to indicate the source of the goods, even if that
    /// source is unknown
    /// 15 U.S.C. Section 1127
    /// </summary>
    public class Trademark : IntellectualProperty, ILegalConcept
    {
        #region ctors
        public Trademark()
        {
            base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public Trademark(string name) : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
                base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public Trademark(string name, string groupName) : base(name, groupName) { }

        public Trademark(ILegalProperty property) : base(property) { }
        #endregion

        /// <summary>15 U.S.C. Section 1052 (a) </summary>
        [Aka("immoral", "deceptive", "scandalous", "contemptible")]
        public bool IsInjurious { get; set; }

        /// <summary>15 U.S.C. Section 1052 (b)</summary>
        /// <remarks> abandoned vexillum are OK (e.g. Akkadian, USSR) </remarks>
        [Aka("flag", "coat of arms", "vexillium")]
        public bool IsExistingInsignia { get; set; }

        /// <summary>15 U.S.C. Section 1052 (c) </summary>
        [Aka("name", "portrait", "signature")]
        public bool IsExistingPerson { get; set; }

        /// <summary>15 U.S.C. Section 1052 (d) </summary>
        public bool IsCopyOfExisting { get; set; }

        /// <summary>
        /// 15 U.S.C. Section 1052 (e)(5)
        /// is essential to the use, purpose, cost or quality (a patent, not trademark)
        /// </summary>
        public bool IsFunctionalFeature { get; set; }

        /// <summary>
        /// in the minds of the public, the primary significance of a product feature ...
        /// is to identify the source of the product rather than the product itself
        /// </summary>
        public bool IsSecondaryMeaning { get; set; }

        public bool IsValid(params ILegalPerson[] persons)
        {
            if (IsInjurious)
            {
                AddReasonEntry($"{nameof(Trademark)} '{Name}', {nameof(IsInjurious)} is true");
                return false;
            }

            if (IsExistingInsignia)
            {
                AddReasonEntry($"{nameof(Trademark)} '{Name}', {nameof(IsExistingInsignia)} is true");
                return false;
            }

            if (IsExistingPerson)
            {
                AddReasonEntry($"{nameof(Trademark)} '{Name}', {nameof(IsExistingPerson)} is true");
                return false;
            }

            if (IsCopyOfExisting)
            {
                AddReasonEntry($"{nameof(Trademark)} '{Name}', {nameof(IsCopyOfExisting)} is true");
                return false;
            }

            if (IsFunctionalFeature)
            {
                AddReasonEntry($"{nameof(Trademark)} '{Name}', {nameof(IsFunctionalFeature)} is true");
                return false;
            }

            if (IsSecondaryMeaning)
            {
                AddReasonEntry($"{nameof(Trademark)} '{Name}', {nameof(IsSecondaryMeaning)} is true");
                return true;
            }

            return true;
        }

        public bool IsEnforceableInCourt => true;

        public bool EquivalentTo(object obj)
        {
            return Equals(obj);
        }

        public virtual StrengthOfMark GetStrengthOfMark()
        {
            return new GenericMark();
        }
    }
}
