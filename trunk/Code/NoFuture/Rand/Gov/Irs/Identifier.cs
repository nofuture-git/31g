using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.Irs
{
    [Serializable]
    public class TaxpayerIdentificationNumber : Identifier
    {
        public override string Abbrev => "TIN";
    }

    [Serializable]
    public class IndividualTaxId : TaxpayerIdentificationNumber
    {
        public override string Abbrev => "ITIN";
    }

    /// <summary>
    /// http://www.irs.gov/Businesses/Small-Businesses-&amp;-Self-Employed/How-EINs-are-Assigned-and-Valid-EIN-Prefixes
    /// </summary>
    [Serializable]
    public class EmployerIdentificationNumber : TaxpayerIdentificationNumber
    {
        public override string Abbrev => "EIN";
    }

    [Serializable]
    public class PreparerTaxIdNumber : TaxpayerIdentificationNumber
    {
        public override string Abbrev => "PTIN";
    }
}
