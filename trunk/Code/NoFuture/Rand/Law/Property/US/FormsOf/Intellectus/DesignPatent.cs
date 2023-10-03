using NoFuture.Law.Enums;
using NoFuture.Law.Attributes;

namespace NoFuture.Law.Property.US.FormsOf.Intellectus
{
    public class DesignPatent : Patent
    {
        #region ctors
        public DesignPatent()
        {
            base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public DesignPatent(string name) : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
                base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public DesignPatent(string name, string groupName) : base(name, groupName) { }

        public DesignPatent(ILegalProperty property) : base(property) { }
        #endregion

        [Eg("curvature of Coke bottle")]
        public bool IsOrnamental { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            Add2TruePropositions(IsOrnamental, nameof(IsOrnamental));
            return base.IsValid(persons);
        }
    }
}
