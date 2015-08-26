﻿using System;

namespace NoFuture.Rand.Gov.Irs
{
    [Serializable]
    public class TaxpayerIdentificationNumber : Identifier
    {
        public override string Abbrev { get { return "TIN"; } }
    }

    [Serializable]
    public class IndividualTaxId : TaxpayerIdentificationNumber
    {
        public override string Abbrev { get { return "ITIN"; } }
    }

    [Serializable]
    public class EmployerIdentificationNumber : TaxpayerIdentificationNumber
    {
        //http://www.irs.gov/Businesses/Small-Businesses-&-Self-Employed/How-EINs-are-Assigned-and-Valid-EIN-Prefixes
        public override string Abbrev { get { return "EIN"; } }
    }

    [Serializable]
    public class PreparerTaxIdNumber : TaxpayerIdentificationNumber
    {
        public override string Abbrev { get { return "PTIN"; } }
    }
}
