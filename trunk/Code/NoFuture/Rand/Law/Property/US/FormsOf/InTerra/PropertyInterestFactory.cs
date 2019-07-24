using System;
using System.Collections.Generic;
using System.Linq;
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

        public Predicate<ILegalPerson> IsFutureInterest { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsPresentInterest { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsExecutory { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsPossibilityOfReverter { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsRightOfEntry { get; set; } = lp => false;

        public ILandPropertyInterest GetPropertyInterest(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return null;
            var title = subj.GetLegalPersonTypeName();

            var others = persons.Where(p => !p.IsSamePerson(subj)).ToList();

            var subjHasPresentInterest = IsPresentInterest(subj);
            var othersHasPresentInterest = others.Any(o => IsPresentInterest(o));

            var subjHasFutureInterest = IsFutureInterest(subj);
            var othersHasFutureInterest = others.Any(o => IsFutureInterest(o));



            throw new NotImplementedException();
        }
    }
}
