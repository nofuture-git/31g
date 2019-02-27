using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Law.Criminal.US
{
    public class Victim : LegalPerson, IVictim
    {
        public Victim()
        {
            base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public Victim(string name) : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
                base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public Victim(string name, string groupName) : base(name, groupName) { }
    }
}
