using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense;

namespace NoFuture.Rand.Law.Criminal.Inchoate.US.Defense
{
    public class Impossibility : DefenseBase
    {
        public Impossibility(ICrime crime) : base(crime)
        {
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}
