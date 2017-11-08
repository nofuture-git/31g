﻿using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Core
{
    /// <summary>
    /// An implementation to order <see cref="DiachronIdentifier"/>
    /// </summary>
    [Serializable]
    public class DiachronIdComparer : IComparer<DiachronIdentifier>
    {
        public int Compare(DiachronIdentifier x, DiachronIdentifier y)
        {
            if (x?.FromDate == null && y?.FromDate == null)
                return 0;
            if (x?.FromDate == null)
                return 1;
            if (y?.FromDate == null)
                return -1;
            return DateTime.Compare(x.FromDate.Value, y.FromDate.Value);
        }
    }
}