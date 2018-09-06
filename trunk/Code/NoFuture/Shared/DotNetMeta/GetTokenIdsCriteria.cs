using System;
using System.Text;

namespace NoFuture.Shared
{
    /// <summary>
    /// A criteria type to send across the wire to a listening socket.
    /// </summary>
    [Serializable]
    public class GetTokenIdsCriteria
    {
        private string _ranlB64;//persist this as a base64 since regex can be difficult to encode\decode
        public string AsmName;

        /// <summary>
        /// A regex pattern on which <see cref="MetadataTokenId"/> should be resolved. 
        /// The default is to resolve only the top-level assembly.
        /// </summary>
        public string ResolveAllNamedLike
        {
            get { return Encoding.UTF8.GetString(Convert.FromBase64String(_ranlB64)); }
            set
            {
                var t = value;
                if (string.IsNullOrWhiteSpace(t))
                    _ranlB64 = null;
                _ranlB64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
            }
        }
    }
}