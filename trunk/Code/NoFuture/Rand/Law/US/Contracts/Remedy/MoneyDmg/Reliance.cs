using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Rand.Law.US.Contracts.Remedy.MoneyDmg
{
    public class Reliance<T> : MoneyDmgBase<T> where T : IObjectiveLegalConcept
    {
        public Reliance(IContract<T> contract) : base(contract)
        {
        }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            throw new NotImplementedException();
        }
    }
}
