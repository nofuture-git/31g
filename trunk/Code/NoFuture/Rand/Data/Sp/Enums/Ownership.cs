using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Rand.Data.Sp.Enums
{
    /// <summary>
    /// Kinds of business ownership
    /// src [http://bls.dor.wa.gov/ownershipstructures.aspx]
    /// </summary>
    [Serializable]
    [Flags]
    public enum Ownership
    {
        None = 0,
        Proprietorship = 1,
        Partnership = 2,
        Limited = 4,
        LimitedLiablity = 8,
        Corporation = 16,

        /// <summary>
        /// A legal entity and is typically run to further an ideal or goal rather than in the interests of profit
        /// </summary>
        Nonprofit = 32,
        Company = 64,

        /// <summary>
        /// A Trust is a legal relationship in which one person, called the 
        /// trustee, holds property for the benefit of another person, called the beneficiary.
        /// </summary>
        Trust = 128,

        /// <summary>
        /// A Joint Venture is formed for a limited length of time to carry out a 
        /// business transaction or operation.
        /// </summary>
        JointVenture = 256,

        /// <summary>
        /// A Municipality is a public corporation established as a subdivision of a state 
        /// for local governmental purposes.
        /// </summary>
        Municipality = 512,

        /// <summary>
        /// An Association is an organized group of people who share in a common interest, 
        /// activity, or purpose.
        /// </summary>
        Association = 1024
    }
}
