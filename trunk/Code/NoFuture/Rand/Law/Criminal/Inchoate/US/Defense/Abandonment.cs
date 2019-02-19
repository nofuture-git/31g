using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Defense;

namespace NoFuture.Rand.Law.Criminal.Inchoate.US.Defense
{
    public class Abandonment : DefenseBase
    {
        public Abandonment(ICrime crime) : base(crime)
        {
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}
