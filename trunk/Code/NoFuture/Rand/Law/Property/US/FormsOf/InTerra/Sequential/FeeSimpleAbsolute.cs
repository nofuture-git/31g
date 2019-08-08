using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Sequential
{
    /// <summary>
    /// The simplest form of property interest - a kind of default which is assumed unless otherwise stated
    /// </summary>
    public class FeeSimpleAbsolute : LandPropertyInterestBase
    {
        public FeeSimpleAbsolute(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public FeeSimpleAbsolute() : base(null) { }

        public static IList<bool[]> FactoryPaths = new List<bool[]> {new[] {true, true}};

        public Predicate<ILegalPerson> IsExactlyThisPerson { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;

            var title = subj.GetLegalPersonTypeName();

            if (!IsExactlyThisPerson(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsExactlyThisPerson)} is false");
                return false;
            }

            return true;
        }
    }
}
