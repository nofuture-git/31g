using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Law.Property.US.FormsOf
{
    /// <summary>
    /// a property interest in creations of human intellect
    /// </summary>
    public class IntellectualProperty : IntangiblePersonalProperty
    {
        public IntellectualProperty()
        {
            base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public IntellectualProperty(string name) : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
                base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public IntellectualProperty(string name, string groupName) : base(name, groupName) { }

        public IntellectualProperty(ILegalProperty property) : base(property) { }
    }
}
