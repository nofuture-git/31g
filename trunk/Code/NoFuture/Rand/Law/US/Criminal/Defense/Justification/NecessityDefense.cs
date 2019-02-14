using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Criminal.Defense.Justification
{
    /// <summary>
    /// protects defendant from criminal responsibility when the defendant commits a crime to avoid a greater, imminent harm
    /// </summary>
    [Aka("choice of evils defense")]
    public class NecessityDefense : DefenseBase
    {
        public NecessityDefense(ICrime crime) : base(crime)
        {
            Proportionality = new Proportionality<ITermCategory>(crime);
            Imminence = new Imminence(crime);
        }

        /// <summary>
        /// (1) there must be more than one harm that will occur under the circumstances
        /// </summary>
        public Predicate<ILegalPerson> IsMultipleInHarm { get; set; } = lp => false;

        /// <summary>
        /// (2) the harms must be ranked, with one of the harms ranked more severe than the other
        /// </summary>
        public Proportionality<ITermCategory> Proportionality { get; set; }

        /// <summary>
        /// (3) the defendant must have objectively reasonable belief that the greater harm is imminent
        /// </summary>
        public Imminence Imminence { get; set; }

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            throw new NotImplementedException();
        }
    }
}
