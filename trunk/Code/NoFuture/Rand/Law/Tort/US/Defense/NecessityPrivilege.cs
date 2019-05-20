using System;
using NoFuture.Rand.Law.Criminal.US.Defense.Justification;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Defense
{
    /// <summary>
    /// <![CDATA[RESTATEMENT (SECOND) OF TORTS §§ 262, 263 & cmt. d (1965)]]>
    /// </summary>
    /// <inheritdoc cref="NecessityDefense"/>
    public class NecessityPrivilege : NecessityDefense<ILegalProperty>
    {
        public NecessityPrivilege() : this(ExtensionMethods.Tortfeasor) { }

        public NecessityPrivilege(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Predicate<ILegalProperty> IsPublicInterest { get; set; } = lp => false;

        public TortTrespass Trespass { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var tortFeaser = GetSubjectPerson(persons);
            if (tortFeaser == null)
                return false;

            var isNecessary = base.IsValid(persons);

            

            



            return base.IsValid(persons);
        }
    }
}
