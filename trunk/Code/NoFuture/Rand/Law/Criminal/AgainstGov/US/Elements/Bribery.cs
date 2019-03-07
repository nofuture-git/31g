using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.AgainstGov.US.Elements
{
    /// <summary>
    /// conferring, offering, agreeing to confer, or soliciting, accepting,
    /// or agreeing to accept any benefit upon a public official
    /// </summary>
    public class Bribery : CriminalBase, IPossession
    {

        public Predicate<ILegalPerson> IsPublicOfficial { get; set; } = lp => false;

        /// <summary>
        /// Public official&apos;s vote, opinion, judgment, action,
        /// decision or exercise discretion will be influenced.
        /// </summary>
        public Predicate<ILegalPerson> IsKnowinglyProcured { get; set; }

        /// <summary>
        /// Public official&apos;s vote, opinion, judgment, action,
        /// decision or exercise discretion will be influenced.
        /// </summary>
        public Predicate<ILegalPerson> IsKnowinglyReceived { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var isValid = criminalIntent is Purposely || criminalIntent is SpecificIntent
                                                      || criminalIntent is Knowingly || criminalIntent is GeneralIntent;
            if (!isValid)
            {
                AddReasonEntry($"{nameof(Bribery)} requires intent of {nameof(Purposely)}, " +
                               $"{nameof(SpecificIntent)}, {nameof(Knowingly)}, {nameof(GeneralIntent)}");
                return false;
            }

            return true;
        }

    }
}
