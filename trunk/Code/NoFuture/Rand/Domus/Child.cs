using System;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Domus
{
    [Serializable]
    public class Child : IRelation
    {
        public Child(IPerson p)
        {
            Est = p;
        }
        public IPerson Est { get; }

        public override string ToString()
        {
            if (Est == null)
                return string.Empty;
            var title = Est.MyGender == Gender.Male ? "Son" : "Daughter";
            return string.Join(" ", $"({title})", Est.FirstName, Est.LastName, Est.Age);
        }

        public override bool Equals(object obj)
        {
            var c = obj as Child;
            if (c?.Est == null || Est == null)
                return false;
            return c.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
