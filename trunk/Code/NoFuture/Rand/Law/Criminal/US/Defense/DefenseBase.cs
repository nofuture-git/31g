using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Defense
{
    public abstract class DefenseBase : UnoHomine, IDefense
    {
        protected DefenseBase(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) :base(getSubjectPerson)
        {
        }
    }
}
