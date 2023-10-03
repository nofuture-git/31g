using System;
using NoFuture.Law.US;

namespace NoFuture.Law.Property.US
{
    public abstract class PropertyBase : UnoHomine, ILegalConceptWithProperty<ILegalProperty>
    {
        protected PropertyBase(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public virtual ILegalProperty SubjectProperty { get; set; }

        protected virtual bool PropertyOwnerIsSubjectPerson(ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            return ExtensionMethods.PropertyOwnerIsSubjectPerson(this, SubjectProperty, subj);
        }

        protected virtual bool PropertyOwnerIsInPossession(ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            return ExtensionMethods.PropertyOwnerIsInPossession(this, SubjectProperty, subj);
        }
    }
}
