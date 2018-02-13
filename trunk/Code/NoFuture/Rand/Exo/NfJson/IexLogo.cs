﻿using System;

namespace NoFuture.Rand.Exo.NfJson
{
    /// <inheritdoc />
    /// <summary>
    /// see [https://iextrading.com/developer/docs/#logo]
    /// </summary>
    [Serializable]
    public class IexLogo : IexApi
    {
        public IexLogo(Uri uri) : base(uri)
        {

        }

        public static Uri GetUri(string tickerSymbol)
        {
            if (string.IsNullOrWhiteSpace(tickerSymbol))
                throw new ArgumentNullException(nameof(tickerSymbol));

            return new Uri($"{IEX_API_HOST}/stock/{tickerSymbol}/logo");
        }
    }
}
