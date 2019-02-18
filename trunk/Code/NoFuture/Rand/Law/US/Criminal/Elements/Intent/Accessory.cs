using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Rand.Law.US.Criminal.Elements.Intent
{
    /// <summary>
    /// intent from assistance given after the crime
    /// </summary>
    public class Accessory : MensRea
    {
        /// <summary>
        /// awareness that the principal committed a crime
        /// </summary>
        public Predicate<ILegalPerson> IsAwareOfCrime { get; set; } = lp => false;

        /// <summary>
        /// help or assist the principal escape or evade arrest or prosecution 
        /// for and conviction of the offense with specific intent or purposely
        /// </summary>
        public Predicate<ILegalPerson> IsAssistToEvadeProsecution { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {

            throw new NotImplementedException();
        }

        public override int CompareTo(object obj)
        {
            return obj is Accessory ? 0 : 1;
        }
    }
}
