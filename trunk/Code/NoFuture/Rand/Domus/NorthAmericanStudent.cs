using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Domus
{
    [Serializable]
    public abstract class NorthAmericanStudent<T> : DiachronIdentifier, IStudent<T>
    {
        public T School { get; }
        public DateTime? Graduation { get; set; }

        protected NorthAmericanStudent(T school)
        {
            School = school;
        }
    }
}