using System;

namespace NoFuture.Rand.Law.US.Criminal.Elements.Intent.PenalCode
{
    /// <summary>
    /// This is a null-object type for <see cref="MensRea"/>.  It abrogates the com-law 
    /// approach that behavior is only criminal when the defendant commits acts 
    /// with a guilty mind.
    /// </summary>
    public class StrictLiability : MensRea
    {
        private const string _name = "StrictLiability";
        protected internal StrictLiability()
        {

        }

        private static StrictLiability _value;
        public static StrictLiability Value => _value ?? (_value = new StrictLiability());

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            return true;
        }

        public override int CompareTo(object obj)
        {
            return obj is StrictLiability ? 0 : 1;
        }

        public override bool Equals(object obj)
        {
            return obj is StrictLiability;
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }
    }
}
