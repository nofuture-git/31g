using System;

namespace NoFuture.Encryption.Sjcl
{
    public class Hash
    {
        public static string Sign(string candidate, string salt)
        {
            var toHash = String.Format("{0}{1}", candidate, salt);
            var results = Resources.ScriptEngine.Evaluate(string.Format("sjcl.hash.sha256.hash(\"{0}\")", toHash));
            return results.ToString();
        }
    }
}
