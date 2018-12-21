using System.Collections.Generic;

namespace NoFuture.Rand.Law.US.Contracts
{
    public class Advertisement : LegalDuty
    {
        public override bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            return IsEnforceableInCourt;
        }

        /// <summary>
        /// Lefkowitz v. Great Minneapolis Surplus Store, 86 N.W.2d 689, 691 (Minn. 1957)
        /// </summary>
        public bool IsClear { get; set; }
        /// <summary>
        /// Lefkowitz v. Great Minneapolis Surplus Store, 86 N.W.2d 689, 691 (Minn. 1957)
        /// </summary>
        public bool IsDefinite { get; set; }
        /// <summary>
        /// Lefkowitz v. Great Minneapolis Surplus Store, 86 N.W.2d 689, 691 (Minn. 1957)
        /// </summary>
        public bool IsExplicit { get; set; }
        /// <summary>
        /// Lefkowitz v. Great Minneapolis Surplus Store, 86 N.W.2d 689, 691 (Minn. 1957)
        /// </summary>
        public bool IsNothingLeftToOpenNegotiation { get; set; }

        private readonly List<string> _audit = new List<string>();
        public override List<string> Audit => _audit;

        public override bool IsEnforceableInCourt
        {
            get
            {
                var rslt = IsClear && IsDefinite && IsExplicit && IsNothingLeftToOpenNegotiation;
                if (!rslt)
                {
                    _audit.Add($"This {nameof(Advertisement)} is not enforceable: " +
                               $"{nameof(IsClear)} = {IsClear} " +
                               $"{nameof(IsDefinite)} = {IsDefinite} " +
                               $"{nameof(IsExplicit)} = {IsExplicit} " +
                               $"{nameof(IsNothingLeftToOpenNegotiation)} = {IsNothingLeftToOpenNegotiation}");
                }
                return rslt;
            }
        }
            
    }
}
