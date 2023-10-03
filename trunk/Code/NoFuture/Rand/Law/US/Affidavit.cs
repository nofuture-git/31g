using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US
{
    /// <summary>
    /// A voluntary sworn statement of fact given under oath with a authorized-by-law witness
    /// </summary>
    /// <typeparam name="T">
    /// The type or thing on which something is being sworn
    /// </typeparam>
    public class Affidavit<T> : LegalConcept
    {
        /// <summary>
        /// The person who is making the statement
        /// </summary>
        public Func<ILegalPerson[], ILegalPerson> GetAffiant { get; set; } = lps => lps.Affiant();

        /// <summary>
        /// The person authorized-by-law as a witness
        /// </summary>
        public Func<ILegalPerson[], ILegalPerson> GetWitness { get; set; } = lps => lps.NotaryPublic();

        public Predicate<ILegalPerson> IsSigned { get; set; } = lp => false;

        public List<T> FactsThereof { get; set; }

        /// <summary>
        /// The when and where of the sworn statement
        /// </summary>
        [Aka("jurat")]
        public Tuple<IVoca, DateTime?> Attestation { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var affiant = GetAffiant(persons);
            if (affiant == null)
            {
                AddReasonEntry($"{nameof(GetAffiant)} returned nothing");
                return false;
            }

            var notary = GetWitness(persons);
            if (notary == null)
            {
                AddReasonEntry($"{nameof(GetWitness)} returned nothing");
                return false;
            }

            if (Attestation == null)
            {
                AddReasonEntry($"{nameof(Attestation)} is unassigned");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Attestation.Item1?.Name))
            {
                AddReasonEntry($"{nameof(Attestation)} place (Item1) is null, empty-string or whitespace");
                return false;
            }

            if (Attestation.Item2 == null)
            {
                AddReasonEntry($"{nameof(Attestation)} date-time (Item2) is unassigned");
                return false;
            }

            var affiantTitle = affiant.GetLegalPersonTypeName();
            var notaryTitle = notary.GetLegalPersonTypeName();

            if (!IsSigned(affiant))
            {
                AddReasonEntry($"{affiantTitle} {affiant.Name}, {nameof(IsSigned)} is false");
                return false;
            }

            if (!IsSigned(notary))
            {
                AddReasonEntry($"{notaryTitle} {notary.Name}, {nameof(IsSigned)} is false");
                return false;
            }

            if (FactsThereof == null)
            {
                AddReasonEntry($"{nameof(FactsThereof)} is unassigned");
                return false;
            }

            if (!FactsThereof.Any())
            {
                AddReasonEntry($"{nameof(FactsThereof)} is empty");
                return false;
            }

            return true;
        }
    }
}
