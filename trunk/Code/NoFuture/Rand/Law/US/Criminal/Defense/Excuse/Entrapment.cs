using System;

namespace NoFuture.Rand.Law.US.Criminal.Defense.Excuse
{
    public class Entrapment : DefenseBase
    {
        public Entrapment(ICrime crime) : base(crime)
        {
        }

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            throw new NotImplementedException();
        }
    }
}
