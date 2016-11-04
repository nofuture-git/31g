using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;

namespace NoFuture.Util.NfConsole
{
    /// <summary>
    /// A concrete implementation of the calling assembly side's
    /// invocation of a command on a remote console.exe.
    /// </summary>
    public abstract class InvokeCmdBase
    {
        protected static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        protected static string ConvertJsonFromBuffer(byte[] buffer)
        {
            var decoder = Encoding.UTF8.GetDecoder();

            var jsonChars = new char[decoder.GetCharCount(buffer, 0, buffer.Length)];
            decoder.GetChars(buffer, 0, buffer.Length, jsonChars, 0);

            var jsonStringBldr = new StringBuilder();
            jsonStringBldr.Append(jsonChars);

            return jsonStringBldr.ToString();
        }

        protected internal bool IsMyProcessRunning(int pid)
        {
            if (pid == 0)
                return false;
            var proc = Process.GetProcessById(pid);
            proc.Refresh();
            return !proc.HasExited;
        }
    }
}
