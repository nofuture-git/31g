using System;
using NoFuture.Rand.Law.Enums;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Property.US.FormsOf
{
    /// <summary>
    /// Property which belongs to the government
    /// </summary>
    [EtymologyNote("Latin", "res publicae", "things owned by the state")]
    public class GovernmentProperty : LegalProperty
    {
        public GovernmentProperty()
        {
            base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public GovernmentProperty(string name) : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
                base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public GovernmentProperty(ILegalProperty property) : base(property) { }

        public override Predicate<ILegalPerson> IsEntitledTo { get; set; } = lp => lp is Government;
    }
}
