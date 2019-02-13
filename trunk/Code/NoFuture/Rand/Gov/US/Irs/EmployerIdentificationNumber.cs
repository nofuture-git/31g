using System;

namespace NoFuture.Rand.Gov.US.Irs
{
    /// <summary>
    /// http://www.irs.gov/Businesses/Small-Businesses-&amp;-Self-Employed/How-EINs-are-Assigned-and-Valid-EIN-Prefixes
    /// </summary>
    /// <remarks>
    /// <![CDATA[^(0[1-6]|1[0-6]|2[0-7]|[35][0-9]|[468][0-8]|7[1-7]|9[0-589])[ \-]?\d{7}$ ]]>
    /// </remarks>
    [Serializable]
    public class EmployerIdentificationNumber : TaxpayerIdentificationNumber
    {
        public override string Abbrev => "EIN";
    }
}