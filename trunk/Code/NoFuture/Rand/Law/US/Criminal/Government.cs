using System;

namespace NoFuture.Rand.Law.US.Criminal
{
    /// <summary>
    /// criminal prosecution is always instituted by the government
    /// </summary>
    public class Government : LegalPerson
    {
        private static readonly Guid _uid = Guid.NewGuid();
        private Government() { }

        private static Government _value;
        public static Government Value
        {
            get
            {
                if(_value ==null)
                    _value = new Government();
                return _value;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Government;
        }

        public override int GetHashCode()
        {
            return _uid.GetHashCode();
        }
    }
}
