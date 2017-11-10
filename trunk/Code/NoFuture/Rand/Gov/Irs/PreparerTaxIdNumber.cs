using System;

namespace NoFuture.Rand.Gov.Irs
{
    [Serializable]
    public class PreparerTaxIdNumber : TaxpayerIdentificationNumber
    {
        public override string Abbrev => "PTIN";
    }
}