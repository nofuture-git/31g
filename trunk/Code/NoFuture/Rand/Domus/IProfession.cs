using System;

namespace NoFuture.Rand.Domus
{
    public interface IProfession
    {
        IEmployment[] GetEmployment(DateTime? dt);
    }
}
