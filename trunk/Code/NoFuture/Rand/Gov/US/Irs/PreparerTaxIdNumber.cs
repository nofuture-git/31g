using System;

namespace NoFuture.Rand.Gov.US.Irs
{
    [Serializable]
    public class PreparerTaxIdNumber : TaxpayerIdentificationNumber
    {
        public override string Abbrev => "PTIN";
    }
}