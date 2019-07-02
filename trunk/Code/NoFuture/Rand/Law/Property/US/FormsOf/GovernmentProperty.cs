using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Exceptions;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Property.US.FormsOf
{
    /// <summary>
    /// Property which belongs to the government
    /// </summary>
    [EtymologyNote("Latin", "res publicae", "things owned by the state")]
    public class GovernmentProperty : LegalProperty
    {
        private ILegalPerson _gov = Government.Value;

        public GovernmentProperty()
        {
            base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public GovernmentProperty(string name) : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
                base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public override ILegalPerson EntitledTo
        {
            get => _gov;
            set
            {
                if(!(value is Government))
                    throw new LawException("The property of the government can only be assigned to the government");
                _gov = value;
            }
        }
        public override ILegalPerson InPossessionOf { get; set; } = Government.Value;
    }
}
