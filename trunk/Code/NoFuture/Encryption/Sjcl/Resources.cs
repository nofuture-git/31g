using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using Jurassic;
using NoFuture.Shared;

namespace NoFuture.Encryption.Sjcl
{
    /// <summary>
    /// Responsible for doing the heavy load actions involved in compiling the 'sjcl.js' 
    /// into the .NET CLR along with instantiating the script engine used by the 
    /// <see cref="PublicKey"/>, <see cref="Hash"/> and <see cref="BulkCipherKey"/> methods.
    /// </summary>
    public class Resources
    {
        private static string _sjclJs;
        private static ScriptEngine _scriptEngine;
        private static DataContractJsonSerializer _sjclEncryptionSerializer;

        /// <summary>
        /// Gets the 'sjcl.js' file as a single string the this assembly's embedded resources.
        /// </summary>
        public static string SjclJs
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_sjclJs))
                {
                    var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NoFuture.Encryption.Sjcl.sjcl.js");
                    if (stream == null)
                    {
                        return String.Empty;
                    }
                    var txtSr = new StreamReader(stream);
                    _sjclJs = txtSr.ReadToEnd();
                }
                return _sjclJs;
            }
        }

        /// <summary>
        /// A singleton which will load the 'sjcl.js' into a <see cref="ScriptEngine"/> upon 
        /// its first call.
        /// </summary>
        public static ScriptEngine ScriptEngine
        {
            get
            {
                if (_scriptEngine == null)
                {
                    _scriptEngine = new ScriptEngine();
                    _scriptEngine.Execute(SjclJs);

                }
                return _scriptEngine;
            }
        }

        /// <summary>
        /// A singleton which may be resused to serialize a JSON string into 
        /// the shared <see cref="CipherText"/> type.
        /// </summary>
        public static DataContractJsonSerializer CipherTextSerializer
        {
            get
            {
                if (_sjclEncryptionSerializer == null)
                    _sjclEncryptionSerializer = new DataContractJsonSerializer(typeof(CipherText));

                return _sjclEncryptionSerializer;
            }
        }
    }
}
