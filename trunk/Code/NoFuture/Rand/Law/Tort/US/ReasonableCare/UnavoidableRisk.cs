using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.ReasonableCare
{
    /// <summary>
    /// A singleton pattern type to stand-in for reasonable care which is never enough
    /// Used in concept of strict liability
    /// </summary>
    public class UnavoidableRisk : ReasonableCareBase
    {
        private static UnavoidableRisk _value;
        private readonly Guid _someId;
        private UnavoidableRisk() : base(ExtensionMethods.Tortfeasor)
        {
            _someId = Guid.Parse("7141b43a-1cd2-488d-95b6-86652517b6a1");
        }

        public static UnavoidableRisk Value => _value ?? (_value = new UnavoidableRisk());

        /// <summary>
        /// Always returns false.
        /// </summary>
        /// <returns>Always returns false</returns>
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();

            AddReasonEntry($"{title} {subj.Name}, with {nameof(UnavoidableRisk)} no " +
                           "amount of reasonable care is sufficient to mitigate the risk");
            return false;
        }

        public override int GetHashCode()
        {
            return _someId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is UnavoidableRisk;
        }
    }
}
