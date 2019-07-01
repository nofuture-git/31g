using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.FormsOf
{
    /// <summary>
    /// Land property or that which is attached to it permanently, more-or-less.
    /// </summary>
    /// <remarks>
    /// claim of air above land: owns at least as much of the space above the 
    /// ground as he can occupy or use in connection with the land
    /// </remarks>
    [Aka("land", "realty")]
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
