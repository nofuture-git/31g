using System;

namespace NoFuture.Rand.Law.US.Criminal.Defense.Justification
{
    public class DefenseOfOthers : DefenseBase
    {
        public DefenseOfOthers(ICrime crime) : base(crime)
        {
        }

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            throw new NotImplementedException();
        }
    }
}
