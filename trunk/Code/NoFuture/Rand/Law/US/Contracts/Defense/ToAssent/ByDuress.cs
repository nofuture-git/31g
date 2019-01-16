using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Rand.Law.US.Contracts.Defense.ToAssent
{
    public class ByDuress<T> : DefenseBase<T>, IVoidable
    {
        public ByDuress(IContract<T> contract) : base(contract)
        {
        }

        public override bool IsEnforceableInCourt => true;
    }
}
