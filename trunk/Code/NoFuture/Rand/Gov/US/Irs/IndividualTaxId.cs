using System;

namespace NoFuture.Rand.Gov.US.Irs
{
    [Serializable]
    public class IndividualTaxId : TaxpayerIdentificationNumber
    {
        public override string Abbrev => "ITIN";
    }
}