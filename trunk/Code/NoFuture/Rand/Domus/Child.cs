using System;

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
            var title = Est.MyGender == Gender.Male ? "Son" : "Daughter";
            return string.Join(" ", $"({title})", Est.FirstName, Est.LastName, Est.Age);
        }

        public override bool Equals(object obj)
        {
            var c = obj as Child;
            if (c == null)
                return false;
            return c.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
