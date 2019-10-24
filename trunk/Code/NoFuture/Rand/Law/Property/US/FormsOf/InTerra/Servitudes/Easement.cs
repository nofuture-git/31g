using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Servitudes
{
    /// <summary>
    /// Servitude regarding the use of another&apos;s land
    /// </summary>
    [Aka("frontage", "cartway", "accessway")]
    public class Easement : LandPropertyInterestBase
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}
