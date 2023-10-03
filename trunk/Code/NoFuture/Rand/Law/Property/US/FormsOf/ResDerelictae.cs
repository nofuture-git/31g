using NoFuture.Rand.Law.Enums;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.FormsOf
{
    /// <summary>
    /// things voluntarily abandoned by their owner with the intention
    /// to have them go to the first person taking possession.
    /// </summary>
    [EtymologyNote("Latin", "res derelictae", "things abandoned")]
    public class ResDerelictae : LegalProperty
    {
        public ResDerelictae()
        {
            base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public ResDerelictae(string name) : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
                base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public ResDerelictae(string name, string groupName) : base(name, groupName) { }

        public ResDerelictae(ILegalProperty property) : base(property) { }
    }
}
