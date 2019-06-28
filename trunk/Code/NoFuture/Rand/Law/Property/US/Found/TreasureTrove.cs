using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.Found
{
    public class TreasureTrove : PropertyBase
    {
        public TreasureTrove(Func<IEnumerable<ILegalPerson>, ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();

            if (base.PropertyOwnerIsInPossession(persons))
                return false;


            throw new NotImplementedException();
        }
    }
}
