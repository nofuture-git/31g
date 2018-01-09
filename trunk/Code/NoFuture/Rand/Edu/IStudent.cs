using System;

namespace NoFuture.Rand.Edu
{
    public interface IStudent<T>
    {
        T School { get; }
        DateTime? Graduation { get; set; }
    }
}