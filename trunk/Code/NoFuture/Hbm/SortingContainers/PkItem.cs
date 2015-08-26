using System;
using System.Collections.Generic;

namespace NoFuture.Hbm.SortingContainers
{
    [Serializable]
    public class PkItem
    {
        protected internal ColumnMetadata id;
        protected internal Dictionary<string, List<ColumnMetadata>> keyProperty;
        protected internal Dictionary<string, List<ColumnMetadata>> keyManyToOne;

        public ColumnMetadata Id { get { return id; } set { id = value; } }
        public Dictionary<string, List<ColumnMetadata>> KeyProperty 
        { 
            get { return keyProperty; } 
            set { keyProperty = value;} 
        }
        public Dictionary<string, List<ColumnMetadata>> KeyManyToOne
        {
            get { return keyManyToOne; }
            set { keyManyToOne = value; }
        }

    }
}
