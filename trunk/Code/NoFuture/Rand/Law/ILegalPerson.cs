using NoFuture.Rand.Core;

namespace NoFuture.Rand.Law
{
    public interface ILegalPerson : IVoca
    {
    }

    public interface INaturalPerson  : ILegalPerson { }

    public interface IJudicialPerson : ILegalPerson { }
}
