﻿using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.Terms.Convenants
{
    /// <summary>
    /// promise that you have disclosed all other peoples lawful rights over the property
    /// </summary>
    [Aka("warranty against encumbrances")]
    public class AgainstEncumbrancesTerm : Term<ILegalProperty>
    {
        public AgainstEncumbrancesTerm(string name, ILegalProperty reference) : base(name, reference)
        {
        }

        public AgainstEncumbrancesTerm(string name, ILegalProperty reference, params ITermCategory[] categories) : base(name, reference, categories)
        {
        }
    }
}
