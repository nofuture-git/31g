using System;
using System.IO.Compression;
using System.IO;
using System.Linq;
using NoFuture.Shared;
using NoFuture.Shared.Core;

namespace NoFuture.Util.Binary
{
    public class Compression
    {
        /// <summary>
        /// Pushes each of <see cref="props"/> into the result where
        /// the one furthest left is index 0.
        /// </summary>
        /// <param name="props"></param>
        /// <returns></returns>
        public static int Propositions(bool[] props)
        {
            if (props == null)
                return 0;
            if (props.All(p => p == false))
                return 0;
            if (props.All(p => p))
                return 0xFF;

            var myBitWise = 0;
            var len = props.Length > 32 ? 32 : props.Length;
            for (var i = 0; i < len; i++)
            {
                var shiftBy = props.Length - 1 - i;
                var asInt = Convert.ToInt32(props[i]);

                myBitWise = myBitWise | (asInt << shiftBy);
            }
            return myBitWise;
        }

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
                    dName = NfConfig.TempDirectories.Root;
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
