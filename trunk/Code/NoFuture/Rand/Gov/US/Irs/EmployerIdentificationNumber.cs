using System;

namespace NoFuture.Rand.Gov.Irs
{
    /// <summary>
    /// http://www.irs.gov/Businesses/Small-Businesses-&amp;-Self-Employed/How-EINs-are-Assigned-and-Valid-EIN-Prefixes
    /// </summary>
    [Serializable]
    public class EmployerIdentificationNumber : TaxpayerIdentificationNumber
    {
        public override string Abbrev => "EIN";
    }
}