using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.FormsOf.Intellectus
{
    public class UtilityPatent : Patent
    {
        #region ctors
        public UtilityPatent()
        {
            base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public UtilityPatent(string name) : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
                base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public UtilityPatent(string name, string groupName) : base(name, groupName) { }

        public UtilityPatent(ILegalProperty property) : base(property) { }
        #endregion

        [Aka("utility", "entertainment")]
        public bool IsUseful { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            Add2TruePropositions(IsUseful, nameof(IsUseful));
            return base.IsValid(persons);
        }
    }
}
