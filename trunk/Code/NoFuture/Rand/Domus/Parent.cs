namespace NoFuture.Rand.Domus
{
    public class Parent : IRelation
    {
        public Parent(IPerson p)
        {
            Est = p;
        }
        public IPerson Est { get; }

        public override string ToString()
        {
            return string.Join(" ", Est.FirstName, Est.LastName);
        }
    }
}
