using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    /// <summary>
    /// Intended to punish conduct that is a precursor to assault, battery, or other crimes
    /// </summary>
    /// <remarks>
    /// Examples include harassing, approaching, pursuing, making explicit or implicit threat
    /// </remarks>
    public class Stalking : CriminalBase, IActusReus
    {
        /// <summary>
        /// is unique amoung criminal acts in that it must occur on more than one occasion or repeatedly
        /// </summary>
        public IEnumerable<IDominionOfForce> Occasions { get; set; } = new List<IDominionOfForce>();

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
