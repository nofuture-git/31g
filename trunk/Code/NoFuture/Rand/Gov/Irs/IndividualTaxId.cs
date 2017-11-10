using System;

namespace NoFuture.Rand.Gov.Irs
{
    [Serializable]
    public class IndividualTaxId : TaxpayerIdentificationNumber
    {
        public override string Abbrev => "ITIN";
    }
}