using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Data.Endo.Grps;

namespace NoFuture.Rand.Com
{
    /// <summary>
    /// A public corporation which is in the Finacial sector
    /// </summary>
    [Serializable]
    public class FinancialFirm : PublicCorporation
    {
        public FinancialFirm()
        {
            const string FIFTY_TWO = "52";
            var superSectors = NorthAmericanIndustryClassification.AllSectors;
            if (superSectors == null || superSectors.Length <= 0)
                return;

            PrimarySector =
                superSectors.SelectMany(x => x.Divisions)
                    .FirstOrDefault(x => x.Value == FIFTY_TWO);
        }

        public bool IsInternational { get; set; }
        public Dictionary<DateTime, BankAssets> Assets { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
