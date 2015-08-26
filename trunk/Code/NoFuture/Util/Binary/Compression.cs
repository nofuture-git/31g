using System.IO.Compression;
using System.IO;

namespace NoFuture.Util.Binary
{
    public class Compression
    {
        public static string GzipDecompress(string fileFullName)
        {
            var retName = string.Empty;
            if (!File.Exists(fileFullName))
                return retName;

            var fi = new FileInfo(fileFullName);

            using (var fs = fi.OpenRead())
            {
                var dName = fi.DirectoryName;
                if (string.IsNullOrWhiteSpace(dName))
                {
                    dName = TempDirectories.Root;
                }
                var cName = fi.FullName;
                var nName = Path.GetFileNameWithoutExtension(cName);
                var nFName = Path.Combine(dName, nName);

                using (var dFs = File.Create(nFName))
                {
                    using (var gzipStream = new GZipStream(fs, CompressionMode.Decompress))
                    {
                        gzipStream.CopyTo(dFs);
                    }
                }
                retName = nFName;
            }

            return retName;
        }
    }
}
