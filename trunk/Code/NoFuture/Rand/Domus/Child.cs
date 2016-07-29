namespace NoFuture.Rand.Domus
{
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
    }
}
