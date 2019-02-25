using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    [Aka("attempted battery", "threatened battery")]
    public class Assault : CriminalBase, IActusReus
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}
