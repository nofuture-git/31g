using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US
{
    public class ActOfService : LegalProperty
    {
        public ActOfService()
        {
            base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public ActOfService(string name) : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
                base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public ActOfService(string name, string groupName) : base(name, groupName) { }
    }
}
