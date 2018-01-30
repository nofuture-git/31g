using System;

namespace NoFuture.Rand.Edu
{
    /// <summary>
    /// Represent an attending student of a <see cref="T"/> type school
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IStudent<T>
    {
        T School { get; }
        DateTime? Graduation { get; set; }
    }
}