using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace NoFuture.Shared
{
    /// <summary>
    /// .NET representation of the sjcl.js encryption object
    /// </summary>
    public class CipherText
    {
        public string iv { get; set; }
        public int v { get; set; }
        public int iter { get; set; }
        public int ks { get; set; }
        public int ts { get; set; }
        public string mode { get; set; }
        public string adata { get; set; }
        public string cipher { get; set; }
        public string salt { get; set; }
        public string ct { get; set; }

        /// <summary>
        /// Using the <see cref="DataContractJsonSerializer"/> serializes 
        /// this instance to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var bytes = new byte[] {0x5C, 0x5C, 0x5C};
            var tripleBackslash = Encoding.UTF8.GetString(bytes);
            bytes = new byte[] {0x5C};
            var singleBackslash = Encoding.UTF8.GetString(bytes);

            //this doesn't appear to be a performance hit...
            var jsonSerializer = new DataContractJsonSerializer(this.GetType());
            var ms = new MemoryStream();
            jsonSerializer.WriteObject(ms, this);
            var rdr = new StreamReader(ms);
            ms.Position = 0;
            return rdr.ReadToEnd().Replace(tripleBackslash, singleBackslash);
        }

        /// <summary>
        /// Given a string as JSON the method attempt to parse the 
        /// string into <see cref="CipherText"/>.
        /// </summary>
        /// <param name="sjclJson">The JSON string</param>
        /// <param name="parseResult">The object reference which will recieve the data upon a successful parse.</param>
        /// <returns>True if string parsed, false otherwise.</returns>
        public static bool TryParse(string sjclJson, out CipherText parseResult)
        {
            try
            {
                var data = Encoding.UTF8.GetBytes(sjclJson);
                var jsonSerializer = new DataContractJsonSerializer(typeof(CipherText));
                using (var ms = new MemoryStream(data))
                {
                    parseResult = (CipherText)jsonSerializer.ReadObject(ms);
                    return true;
                }
            }
            catch
            {
                parseResult = null;
                return false;
            }
        }
    }
}
