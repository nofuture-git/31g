using System;
using System.Linq;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Law.Property.US.FormsOf
{
    public class ActOfService : LegalProperty, IAct
    {
        public ActOfService()
        {
            base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public ActOfService(string name) : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
                base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public ActOfService(string name, string groupName) : base(name, groupName) { }

        public ActOfService(ILegalProperty property) : base(property) { }

        public bool IsValid(params ILegalPerson[] persons)
        {
            persons = persons ?? new ILegalPerson[] { };
            if (persons.All(lp => !IsVoluntary(lp)))
            {
                AddReasonEntry($"{nameof(IsVoluntary)} is false for all persons");
                return false;
            }
            if (persons.All(lp => !IsAction(lp)))
            {
                AddReasonEntry($"{nameof(IsAction)} is false for all persons");
                return false;
            }

            return true;
        }

        public virtual bool IsEnforceableInCourt { get; } = true;
        public virtual bool EquivalentTo(object obj)
        {
            return Equals(obj);
        }

        public virtual Predicate<ILegalPerson> IsVoluntary { get; set; } = lp => true;
        public virtual Predicate<ILegalPerson> IsAction { get; set; } = lp => true;
    }
}
