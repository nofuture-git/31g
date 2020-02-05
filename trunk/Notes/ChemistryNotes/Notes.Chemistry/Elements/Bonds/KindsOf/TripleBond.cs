using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Chemistry.Elements.Bonds.KindsOf
{
    public class TripleBond : BondDecorator
    {
        protected internal TripleBond(IBond toDecorate) : base(toDecorate)
        {
        }
    }
}
