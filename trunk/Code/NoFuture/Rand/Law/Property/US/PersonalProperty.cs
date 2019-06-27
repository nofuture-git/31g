using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Law.US.Property
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
    }

    /// <summary>
    /// personal property which can, typically, be physically handled 
    /// </summary>
    /// <remarks>
    /// e.g. money, jewelry, vehicles, electronics, cellular telephones, clothing, etc.
    /// </remarks>
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

    /// <summary>
    /// personal property which has value but cannot be touched or held
    /// </summary>
    /// <remarks>
    /// e.g. stocks, bonds, trusts, etc.
    /// </remarks>
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
