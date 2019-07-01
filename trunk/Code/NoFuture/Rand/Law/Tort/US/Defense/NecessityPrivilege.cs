using System;
using NoFuture.Rand.Law.Criminal.US.Defense.Justification;
using NoFuture.Rand.Law.Tort.US.Elements;
using NoFuture.Rand.Law.Tort.US.IntentionalTort;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Defense
{
    /// <inheritdoc />
    /// <summary>
    /// <![CDATA[RESTATEMENT (SECOND) OF TORTS §§ 262, 263 & cmt. d (1965)]]>
    /// </summary>
    public class NecessityPrivilege<T> : NecessityDefense<T> where T : IRankable
    {
        public NecessityPrivilege() : this(ExtensionMethods.Tortfeasor) { }

        public NecessityPrivilege(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
            
        }
        public TrespassToProperty Trespass { get; set; }
        public Predicate<ILegalProperty> IsPublicInterest { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var tortFeaser = GetSubjectPerson(persons);
            if (tortFeaser == null)
                return false;

            var title = tortFeaser.GetLegalPersonTypeName();
            var isNecessary = base.IsValid(persons);

            if (!isNecessary)
                return false;

            if (Trespass == null)
            {
                AddReasonEntry($"{title} {tortFeaser.Name}, {nameof(Trespass)} is unassigned");
                return true;
            }

            Trespass.GetSubjectPerson = GetSubjectPerson;
            var isDmgBySubject = Trespass.IsPhysicalDamage(persons);
            AddReasonEntryRange(Trespass.GetReasonEntries());

            if (!isDmgBySubject)
                return true;

            var property = Trespass.PropertyDamage.SubjectProperty ?? new LegalProperty();
            var isPublicInterest = IsPublicInterest(property);

            AddReasonEntry($"{title} {tortFeaser.Name}, {nameof(IsPublicInterest)} " +
                           $"for property '{property.Name}' is {isPublicInterest}");
            return isPublicInterest;
        }
    }
}
