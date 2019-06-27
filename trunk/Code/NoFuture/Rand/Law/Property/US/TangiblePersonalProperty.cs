using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US
{
    /// <summary>
    /// personal property which can, typically, be physically handled 
    /// </summary>
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
    }
}