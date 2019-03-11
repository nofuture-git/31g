﻿using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law
{
    public interface IBargain<T, M> : ILegalConcept
    {
        [Note("Is the manifestation of willingness to enter into a bargain")]
        M Offer { get; set; }

        Func<M, T> Acceptance { get; set; }

        [Note("expression of approval or agreement")]
        IAssent Assent { get; set; }
    }
}