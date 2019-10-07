using System;

namespace NoFuture.Rand.Gov.US.Irs
{
    /// <summary>
    /// id issued by IRS to individual tax payers
    /// </summary>
    [Serializable]
    public class IndividualTaxId : TaxpayerIdentificationNumber
    {
        public override string Abbrev => "ITIN";
        public virtual string RegexPattern { get; } = @"^(9\d{2})([ \-]?)(?!93|89)([789][0-9])([ \-]?)(\d{4})$";
    }
}