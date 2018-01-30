using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// Represents a tradable security of high liquidity
    /// </summary>
    [Serializable]
    public class Security : Pecuniam
    {
        #region ctor
        public Security(string cusip, decimal qty) :base(qty)
        {
            Id = new Cusip { Value = cusip };
        }

        public Security(Cusip id, decimal qty) : base(qty)
        {
            Id = id;
        }
        #endregion

        #region properties
        /// <summary>
        /// Gets the <see cref="Cusip"/> id of this security.
        /// </summary>
        public override Identifier Id { get; }
        #endregion
    }
}
