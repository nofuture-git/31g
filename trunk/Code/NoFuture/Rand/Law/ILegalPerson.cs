using NoFuture.Rand.Core;

namespace NoFuture.Rand.Law
{
    public interface ILegalPerson : IVoca
    {
    }

    public interface IJudicialPerson : ILegalPerson { }

    public interface INaturalPerson : IJudicialPerson { }
}
