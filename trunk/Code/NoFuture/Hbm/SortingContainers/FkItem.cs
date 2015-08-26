using System;
using System.Collections.Generic;
using NoFuture.Hbm.DbQryContainers;

namespace NoFuture.Hbm.SortingContainers
{
    [Serializable]
    public class FkItem
    {
        protected internal Dictionary<string, List<ColumnMetadata>> manyToOne;

        public Dictionary<string, List<ColumnMetadata>> ManyToOne { get { return manyToOne; } set { manyToOne = value; } }
    }
}
