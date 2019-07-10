using System;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.FormsOf.Intellectus
{
    /// <summary>
    /// a property interest in creations of human intellect
    /// </summary>
    [Eg("patents", "copyrights", "trademarks")]
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

        public override string ToString()
        {
            return string.Join(Environment.NewLine, GetReasonEntries());
        }
    }
}
