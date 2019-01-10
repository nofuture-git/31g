using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    public interface IContract<T>
    {
        /// <summary>
        /// What the promisor is putting out there.
        /// </summary>
        /// <remarks>
        /// May be terminated by
        /// (a) rejection or counter-offer by the offeree, or
        /// (b) lapse of time, or
        /// (c) revocation by the offeror, or
        /// (d) death or incapacity of the offeror or offeree.
        /// </remarks>
        [Note("Is the manifestation of willingness to enter into a bargain")]
        IObjectiveLegalConcept Offer { get; set; }

        /// <summary>
        /// A function which resolves what the offer gets in return.
        /// </summary>
        /// <remarks>
        /// when an offer has indicated the mode and means of acceptance, 
        /// an acceptance in accordance with that mode or means is binding 
        /// on the offeror
        /// </remarks>
        Func<IObjectiveLegalConcept, T> Acceptance { get; set; }
    }
}
