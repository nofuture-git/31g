﻿using System;

namespace NoFuture.Rand.Org
{
    /// <summary>
    /// This represents the primary grouping level of the NAICS 
    /// </summary>
    [Serializable]
    public class NaicsPrimarySector : ClassificationBase<NaicsSector>
    {
        public override string LocalName => "primary-sector";
    }
}