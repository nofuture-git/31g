
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US
{
    public class RealProperty : LegalProperty
    {
        public RealProperty()
        {
            base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public RealProperty(string name) : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
                base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public RealProperty(string name, string groupName) : base(name, groupName) { }
    }
}
