using System;

namespace NoFuture.Rand.Domus
{
    public interface IStudent<T>
    {
        T School { get; }
        DateTime? Graduation { get; set; }
    }
}