using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Defense
{
    public abstract class DefenseBase : LegalConcept
    {
        protected internal Func<ILegalPerson[], ILegalPerson> GetSubjectPerson { get; set; }

        protected DefenseBase(Func<ILegalPerson[], ILegalPerson> getSubjectPerson)
        {
            GetSubjectPerson = getSubjectPerson ?? ExtensionMethods.Defendant;
        }
    }
}
