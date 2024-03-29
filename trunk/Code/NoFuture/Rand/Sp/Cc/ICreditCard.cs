﻿using System;

namespace NoFuture.Rand.Sp.Cc
{
    /// <summary>
    /// Represents the std properties from a card-issuer
    /// </summary>
    public interface ICreditCard
    {
        CreditCardNumber Number { get; }
        DateTime ExpDate { get; }
        string CardHolderName { get; }
        //Card Verification Value
        string Cvv { get; }
        DateTime CardHolderSince { get; }
        string CcName { get; }
    }
}