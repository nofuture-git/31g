using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law
{
    /// <summary>
    /// mutual voluntary agreement between two parties to exchange this for that
    /// </summary>
    /// <typeparam name="T">The type of what is being given in return</typeparam>
    /// <typeparam name="M">The type of what is being offered</typeparam>
    public interface IBargain<T, M> : ILegalConcept
    {
        /// <summary>
        /// What the offeror is putting out there.
        /// </summary>
        /// <remarks>
        /// May be terminated by
        /// (a) rejection or counter-offer by the offeree, or
        /// (b) lapse of time, or
        /// (c) revocation by the offeror, or
        /// (d) death or incapacity of the offeror or offeree.
        /// </remarks>
        [Note("Is the manifestation of willingness to enter into a bargain")]
        M Offer { get; set; }

        /// <summary>
        /// A function which resolves what the offer gets in return.
        /// </summary>
        /// <remarks>
        /// when an offer has indicated the mode and means of acceptance, 
        /// an acceptance in accordance with that mode or means is binding 
        /// on the offeror
        /// </remarks>
        Func<M, T> Acceptance { get; set; }


        /// <summary>
        /// An outward expression of approval that a reasonable person would understand
        /// </summary>
        /// <remarks>
        /// src [LUCY v. ZEHMER Supreme Court of Virginia 196 Va. 493; 84 S.E.2d 516 (1954)]
        /// <![CDATA[
        /// If his words and acts, judged by a reasonable standard, manifest an intention 
        /// to agree, it is immaterial what may be the real but unexpressed state of his mind.
        /// ]]>
        /// </remarks>
        [Note("expression of approval or agreement")]
        IAssent Assent { get; set; }
    }
}