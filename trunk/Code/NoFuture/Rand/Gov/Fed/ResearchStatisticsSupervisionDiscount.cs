using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.Fed
{
    /// <summary>
    /// This is unique id assigned by the Fed to all financial institutions.
    /// see [https://cdr.ffiec.gov/CDR/Public/CDRHelp/FAQs1205.htm#FAQ16]
    /// </summary>
    [Serializable]
    public class ResearchStatisticsSupervisionDiscount : Identifier
    {
        public override string Abbrev => "RSSD";
    }
}