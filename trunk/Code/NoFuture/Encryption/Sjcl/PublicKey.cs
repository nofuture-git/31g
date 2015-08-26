using NoFuture.Shared;

namespace NoFuture.Encryption.Sjcl
{
    public class PublicKey
    {
        public static string ToPlainText(CipherText encryptedData, string privateKey)
        {
            //js and C# string escape seq's are the same...
            var results =
                Resources.ScriptEngine.Evaluate(string.Format("sjcl.decrypt(\"{0}\", \"{1}\" )",
                                                              privateKey,
                                                              encryptedData.ToString().Replace("\"", "\\\"")));
            return results.ToString();
        }
        public static CipherText ToEncryptedText(string plainText, string publicKey)
        {
            var results = Resources.ScriptEngine.Evaluate(string.Format("sjcl.encrypt(\"{0}\",\"{1}\")", publicKey, plainText));
            CipherText cipherText;
            if (CipherText.TryParse(results.ToString(), out cipherText))
                return cipherText;

            return null;
        }
    }
}
