using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    /// <summary>
    /// Is not to cause physical contact; rather, it is to cause the 
    /// victim to fear physical contact
    /// </summary>
    public class ThreatenedBatteryAssault : CriminalBase, IActusReus
    {
        public Predicate<ILegalPerson> IsImminentPhysicalContact { get; set; } = lp => false;

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
