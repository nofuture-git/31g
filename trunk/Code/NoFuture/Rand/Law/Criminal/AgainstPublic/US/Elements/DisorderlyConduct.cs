using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US.Elements
{
    /// <summary>
    /// criminal conduct that negatively impacts the quality of
    /// life for citizens in any given city, county or state.
    /// </summary>
    [Aka("disturbing the peace")]
    public class DisorderlyConduct: CriminalBase, IActusReus
    {
        /// <summary>
        /// a loud and unreasonable noise
        /// </summary>
        public Predicate<ILegalPerson> IsUnreasonablyLoud { get; set; } = lp => false;

        /// <summary>
        /// obscene utterance or gesture
        /// </summary>
        public Predicate<ILegalPerson> IsObscene { get; set; } = lp => false;

        /// <summary>
        /// engage in fighting or threatening, or state fighting words
        /// </summary>
        public Predicate<ILegalPerson> IsCombative { get; set; } = lp => false;

        /// <summary>
        /// A situation that is dangerous and poses a risk to others in
        /// the vicinity of the defendant&apos;s conduct
        /// </summary>
        public Predicate<ILegalPerson> IsIllegitimateHazardous { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}
