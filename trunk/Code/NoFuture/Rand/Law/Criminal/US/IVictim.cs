using System;

namespace NoFuture.Rand.Law.Criminal
{
    public interface IVictim
    {
        Func<ILegalPerson[], ILegalPerson> GetVictim { get; set; }
    }
}
