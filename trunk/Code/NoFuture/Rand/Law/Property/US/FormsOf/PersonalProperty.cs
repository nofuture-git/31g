using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Law.Property.US.FormsOf
{
    /// <summary>
    /// any moveable item which could be designated with ownership
    /// </summary>
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

        public PersonalProperty(ILegalProperty property) : base(property) { }
    }
}
