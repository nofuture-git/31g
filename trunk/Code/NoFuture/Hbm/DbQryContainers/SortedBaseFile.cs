using System.IO;

namespace NoFuture.Hbm.DbQryContainers
{
    public abstract class SortedBaseFile
    {
        public abstract string Name { get; }
        public abstract string OutputPath { get; }
    }
}
