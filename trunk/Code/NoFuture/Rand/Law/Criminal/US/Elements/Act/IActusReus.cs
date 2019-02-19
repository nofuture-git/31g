﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.US.Elements.Act
{
    public interface IActusReus : IElement, IProsecution
    {
        /// <summary>
        /// Determines if this criminal act is valid 
        /// when combined with the particular <see cref="criminalIntent"/>
        /// </summary>
        bool CompareTo(IMensRea criminalIntent);
    }
}