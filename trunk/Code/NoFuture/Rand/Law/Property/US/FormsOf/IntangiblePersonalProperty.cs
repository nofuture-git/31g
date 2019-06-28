using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.FormsOf
{
    /// <summary>
    /// personal property which has value but cannot be touched or held
    /// </summary>
    [Eg("stocks", "bonds", "trusts", "escrow")]
    public class IntangiblePersonalProperty : PersonalProperty
    {
        public IntangiblePersonalProperty()
        {
            base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public IntangiblePersonalProperty(string name) : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
                base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public IntangiblePersonalProperty(string name, string groupName) : base(name, groupName) { }
    }
}