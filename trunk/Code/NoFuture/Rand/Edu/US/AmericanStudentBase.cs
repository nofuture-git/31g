using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Edu.US
{
    [Serializable]
    public abstract class AmericanStudentBase<T> : DiachronIdentifier, IStudent<T>, IObviate where T:IObviate
    {
        public T School { get; }
        public DateTime? Graduation { get; set; }
        public double? GradePointsAverage { get; set; }

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

        public IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x, txtCase);
            var itemData = School.ToData(txtCase) ?? new Dictionary<string, object>();

            var prefix = "";
            if (School is IUniversity)
            {
                prefix = "University";
            }
            else if (School is IHighSchool)
            {
                prefix = "HighSchool";
            }

            if(Graduation != null)
                itemData.Add(textFormat(prefix + nameof(Graduation)), Graduation.Value.ToString("s"));

            if(GradePointsAverage != null)
                itemData.Add(textFormat(prefix + nameof(GradePointsAverage)), GradePointsAverage);
            return itemData;
        }
    }
}