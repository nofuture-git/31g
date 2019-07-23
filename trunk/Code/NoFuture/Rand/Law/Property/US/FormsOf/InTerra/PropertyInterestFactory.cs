using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra
{
    /// <summary>
    /// The manner to determine a kind of property interest
    /// </summary>
    public class PropertyInterestFactory
    {
        private readonly RealProperty _subjectProperty;

        public PropertyInterestFactory(RealProperty property, Func<ILegalPerson[], ILegalPerson> getSubjectPerson)
        {
            _subjectProperty = property;
            GetSubjectPerson = getSubjectPerson;
        }

        public Func<ILegalPerson[], ILegalPerson> GetSubjectPerson { get; }

        public virtual ILegalProperty SubjectProperty => _subjectProperty;

        public Predicate<ILegalPerson> IsDefinitelyFiniteGrant { get; set; } = lp => false;

        public ILandPropertyInterest GetPropertyInterest(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return null;
            var title = subj.GetLegalPersonTypeName();

            if (!IsDefinitelyFiniteGrant(subj))
            {
                return new FeeSimpleInterest(GetSubjectPerson) {SubjectProperty = _subjectProperty };
            }


            throw new NotImplementedException();
        }
    }
}
