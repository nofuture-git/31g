using System;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Exo.UsGov.Bls
{
    public abstract class BlsSeriesBase : ISeries
    {
        public abstract Uri ApiLink { get; }
        public abstract string Prefix { get; }

        /// <summary>
        /// Creates the body for a Multiseries POST to the 
        /// BLS API. The <see cref="sYear"/> must be equal to or less
        /// than the <see cref="eYear"/>
        /// </summary>
        /// <param name="seriesCodes">Must not exceed the BLS limit of 50.</param>
        /// <param name="sYear">Start year, has a minimum year of 1950 and max of the current year.</param>
        /// <param name="eYear">End year, has a maximum of the current year and a minimum of 1950</param>
        /// <returns>
        /// JSON string for POST body
        /// </returns>
        /// <remarks>
        /// The <see cref="NfConfig.SecurityKeys.BlsApiRegistrationKey"/> must be assigned before invocation.
        /// See [https://www.bls.gov/developers/api_faqs.htm#register1] concerning the arg-limits.
        /// </remarks>
        public static string GetMultiSeriesPostBody(string[] seriesCodes, int sYear, int eYear)
        {
            if(seriesCodes == null || seriesCodes.Length <=0)
                throw new ArgumentNullException(nameof(seriesCodes));

            if(sYear < 1950 || sYear > DateTime.Today.Year )
                throw new ArgumentException($"The start year {sYear} is not valid - " +
                                            "the minimum is 1950 and the maximum " +
                                            "is the current year.");

            if (eYear < 1950 || eYear > DateTime.Today.Year)
                throw new ArgumentException($"The end year {eYear} is not valid - " +
                                            "the minimum is 1950 and the maximum " +
                                            "is the current year.");

            if (sYear > eYear)
                throw new ArgumentException($"The start year, {sYear}, must be " +
                                            $"greater-than or equal-to the end year, {eYear}.");

            if(eYear - sYear > 20)
                throw new ArgumentException("BLS Api has a max spread of 20 years per " +
                                            $"request.  The {sYear} to {eYear} is a differnce" +
                                            $"of {eYear - sYear} years.");
            if(seriesCodes.Length > 50)
                throw new ArgumentException("BLS Api has a max series count " +
                                            "of 50 - the passed in arg has a count " +
                                            $"of {seriesCodes.Length}.");

            if (string.IsNullOrWhiteSpace(NfConfig.SecurityKeys.BlsApiRegistrationKey))
                throw new RahRowRagee($"The '{nameof(NfConfig.SecurityKeys.BlsApiRegistrationKey)}' " +
                                                 "must be set before calling this property.");
            var payload =
                new Payload
                {
                    seriesid = seriesCodes,
                    startyear = $"{sYear}",
                    endyear = $"{eYear}",
                    registrationKey = NfConfig.SecurityKeys.BlsApiRegistrationKey
                };
            return Newtonsoft.Json.JsonConvert.SerializeObject(payload);
        }

        internal class Payload
        {
            internal string[] seriesid;
            internal string startyear;
            internal string endyear;
            internal string registrationKey;
        }
    }
}
