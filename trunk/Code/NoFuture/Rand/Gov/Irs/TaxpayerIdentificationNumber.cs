﻿using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.Irs
{
    [Serializable]
    public class TaxpayerIdentificationNumber : Identifier
    {
        public override string Abbrev => "TIN";
    }
}