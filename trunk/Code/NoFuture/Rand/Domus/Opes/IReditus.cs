using System;

namespace NoFuture.Rand.Domus.Opes
{
    public interface IReditus
    {
        IEmployment[] GetEmployment(DateTime? dt);
    }
}
