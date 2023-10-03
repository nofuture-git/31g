using System;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Defense
{
    public abstract class DefenseBase : UnoHomine, IDefense
    {
        protected DefenseBase(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) :base(getSubjectPerson)
        {
        }
    }
}
