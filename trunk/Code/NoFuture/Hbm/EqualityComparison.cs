using System.Collections.Generic;
using System.Linq;
using NoFuture.Hbm.SortingContainers;

namespace NoFuture.Hbm
{
    /// <summary>
    /// For finding the distinct list of <see cref="ColumnMetadata"/> 
    /// by <see cref="ColumnMetadata.constraint_name"/>
    /// </summary>
    public class ConstraintNameComparer : IEqualityComparer<ColumnMetadata>
    {
        public bool Equals(ColumnMetadata x, ColumnMetadata y)
        {
            return string.Equals(x.constraint_name, y.constraint_name);
        }

        public int GetHashCode(ColumnMetadata obj)
        {
            return obj.constraint_name.GetHashCode();
        }
    }

    /// <summary>
    /// For finding the distinct list of <see cref="ColumnMetadata"/> 
    /// solving equality on <see cref="ColumnMetadata.column_ordinal"/>, 
    /// <see cref="ColumnMetadata.data_type"/> and <see cref="ColumnMetadata.column_name"/>
    /// </summary>
    public class FromDataColumnComparer : IEqualityComparer<ColumnMetadata>
    {
        public bool Equals(ColumnMetadata x, ColumnMetadata y)
        {
            if (x == null)
                return false;
            if (y == null)
                return false;
            return x.column_ordinal == y.column_ordinal && string.Equals(x.data_type, y.data_type) &&
                   string.Equals(x.column_name, y.column_name);
        }

        public int GetHashCode(ColumnMetadata obj)
        {
            return obj.column_name.GetHashCode() ^ obj.column_ordinal.GetHashCode() ^ obj.data_type.GetHashCode();
        }
    }

    /// <summary>
    /// For comparison of two lists of <see cref="ColumnMetadata"/> using the result
    /// of <see cref="FromDataColumnComparer"/>.
    /// </summary>
    public class FromDataColumnCollectionComparer : IEqualityComparer<List<ColumnMetadata>>
    {
        /// <summary>
        /// The Equals method will cause some confusion for the compiler as it 
        /// appears as Object.Equals so this is here to mend that.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool AreEqual(List<ColumnMetadata> x, List<ColumnMetadata> y)
        {
            if (x == null)
                return false;
            if (y == null)
                return false;
            if (x.Count != y.Count)
                return false;
            var perColumnComparer = new FromDataColumnComparer();
            for (var i = 0; i < x.Count; i++)
            {
                if (!perColumnComparer.Equals(y[i], x[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Actually just calls <see cref="AreEqual"/>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(List<ColumnMetadata> x, List<ColumnMetadata> y)
        {
            return AreEqual(x, y);
        }

        public int GetHashCode(List<ColumnMetadata> obj)
        {
            if (obj == null)
                return 0;
            return obj.Count <= 0 ? obj.GetHashCode() : obj.Aggregate(0, (current, cm) => current ^ cm.GetHashCode());
        }
    }
}