using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Edu.US
{
    [Serializable]
    public abstract class AmericanStudentBase<T> : DiachronIdentifier, IStudent<T>
    {
        public T School { get; }
        public DateTime? Graduation { get; set; }

        protected AmericanStudentBase(T school)
        {
            School = school;
        }

        public override bool Equals(Identifier obj)
        {
            var student = obj as AmericanStudentBase<T>;
            if (student == null)
                return base.Equals(obj);

            return base.Equals(student)
                   && student.School.Equals(School)
                   && student.Graduation == Graduation;
        }

        public override bool Equals(object obj)
        {
            var student = obj as Identifier;
            return Equals(student);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() +
                   School?.GetHashCode() ?? 1 +
                   Graduation?.GetHashCode() ?? 1;

        }
    }
}