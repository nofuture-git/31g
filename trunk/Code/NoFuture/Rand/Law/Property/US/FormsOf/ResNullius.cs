using NoFuture.Rand.Law.Enums;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.FormsOf
{
    /// <summary>
    /// things capable of individual appropriation that belong to no one
    /// </summary>
    /// <remarks>
    /// src [https://lawdigitalcommons.bc.edu/cgi/viewcontent.cgi?referer=https://www.google.com/&httpsredir=1&article=1334&context=ealr]
    /// page 503
    /// </remarks>
    [Eg("wild animals")]
    [EtymologyNote("Latin", "res nullius", "things owned by no one")]
    public class ResNullius : LegalProperty
    {
        public ResNullius()
        {
            base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public ResNullius(string name) : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
                base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public ResNullius(string name, string groupName) : base(name, groupName) { }

        public ResNullius(ILegalProperty property) : base(property) { }

    }
}
