using System;

namespace NoFuture.Rand.Domus.Opes
{
    public interface IProfession
    {
        IEmployment[] GetEmployment(DateTime? dt);
    }
}
