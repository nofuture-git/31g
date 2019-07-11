using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.Property.US;
using NoFuture.Rand.Law.Property.US.FormsOf;
using NoFuture.Rand.Law.Property.US.Terms;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.IntentionalTort
{
    /// <summary>
    /// Infringement on the trademark or celebrity rights of another 
    /// </summary>
    /// <remarks>
    /// Polaroid Corp. v. Polarad Elect. Corp., 287 F.2d 492 (2d Cir. 1961)
    /// </remarks>
    public class FalseEndorsement : Proportionality<ILegalProperty>
    {
        public FalseEndorsement(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// Degree of similarity between the two marks at issue
        /// </summary>
        public Func<ILegalProperty, ILegalProperty, bool> IsSimilarityOfMark { get; set; } = (l1, l2) => false;

        /// <summary>
        /// If consumers are actually being confused
        /// </summary>
        public bool IsActualConfusionExist { get; set; }

        /// <summary>
        /// Similarity of the goods and services at issue
        /// </summary>
        public Func<ILegalProperty, IRankable> GetEconomicMarketSimilarity { get; set; }

        /// <summary>
        /// Quality of the defendant's goods or services
        /// </summary>
        public Func<ILegalProperty, IRankable> GetProductQuality { get; set; }

        /// <summary>
        /// Purchaser sophistication
        /// </summary>
        public bool? IsPurchaserSophisticated { get; set; }

        /// <summary>
        /// Defendant's intent in adopting the mark
        /// </summary>
        public IIntent Intent { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();


            throw new NotImplementedException();
        }

    }
}
