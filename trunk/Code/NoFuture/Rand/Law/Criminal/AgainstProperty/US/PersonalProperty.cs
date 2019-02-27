using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US
{
    public class PersonalProperty : LegalProperty
    {
        public PersonalProperty()
        {
            base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public PersonalProperty(string name) : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
                base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public PersonalProperty(string name, string groupName) : base(name, groupName) { }
    }
}
