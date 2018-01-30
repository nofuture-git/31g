using System;

namespace NoFuture.Rand.Sp.Cc
{
    /// <summary>
    /// Represents the std properites from a card-issuer
    /// </summary>
    public interface ICreditCard
    {
        CreditCardNumber Number { get; }
        DateTime ExpDate { get; }
        string CardHolderName { get; }
        string Cvv { get; }
        DateTime CardHolderSince { get; }
        string CcName { get; }
    }
}