using System;
using NoFuture.Rand.Law.US.Contracts.Semiosis;

namespace NoFuture.Rand.Law.US.Contracts.Breach
{
    public class Excuse<T> : DilemmaBase<T> where T : IObjectiveLegalConcept
    {
        public Excuse(IContract<T> contract) : base(contract)
        {
        }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            throw new NotImplementedException();
        }

        public override bool IsEnforceableInCourt => true;

    }
}
