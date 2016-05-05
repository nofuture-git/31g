using System.Diagnostics;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using NoFuture.Shared;

namespace NoFuture.Util.NfConsole
{
    public abstract class InvokeConsoleBase
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

        protected Process StartRemoteProcess(string exe, string args)
        {
            var si = !string.IsNullOrWhiteSpace(args)
                ? new ProcessStartInfo(exe, args)
                : new ProcessStartInfo(exe);

            si.CreateNoWindow = false;
            si.UseShellExecute = true;
            si.RedirectStandardOutput = false;
            si.RedirectStandardError = false;

            var myProcess = new Process { StartInfo = si };

            myProcess.Start();
            Thread.Sleep(Constants.ThreadSleepTime);

            return myProcess;
        }
    }
}
