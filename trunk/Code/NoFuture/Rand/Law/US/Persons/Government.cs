using System;

namespace NoFuture.Law.US.Persons
{
    /// <summary>
    /// criminal prosecution is always instituted by the government
    /// </summary>
    public class Government : LegalPerson, IGovernment
    {
        private static readonly Guid _uid = Guid.NewGuid();
        protected Government() { }
        protected Government(string name) : base(name) { }

        private static IGovernment _value;
        public static IGovernment Value => _value ?? (_value = new Government());

        public override bool Equals(object obj)
        {
            return obj is IGovernment;
        }

        public override int GetHashCode()
        {
            return _uid.GetHashCode();
        }
    }
}
