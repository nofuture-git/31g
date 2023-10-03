using NoFuture.Law.Enums;
using NoFuture.Law.Attributes;

namespace NoFuture.Law.Property.US.FormsOf
{
    /// <summary>
    /// personal property which can, typically, be physically handled 
    /// </summary>
    [Aka("chattel")]
    [Eg("money", "jewelry", "vehicles", "electronics", "cellular telephones", "clothing")]
    public class TangiblePersonalProperty : PersonalProperty
    {
        public TangiblePersonalProperty()
        {
            base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public TangiblePersonalProperty(string name) : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
                base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public TangiblePersonalProperty(string name, string groupName) : base(name, groupName) { }

        public TangiblePersonalProperty(ILegalProperty property) : base(property) { }
    }
}